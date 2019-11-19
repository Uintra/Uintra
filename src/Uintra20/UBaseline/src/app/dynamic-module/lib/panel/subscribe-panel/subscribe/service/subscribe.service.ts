import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SUBSCRIBE_MODULE_CONFIG, ISubscribeConfig } from '../config';
import { catchError, map } from 'rxjs/operators';
import { of } from 'rxjs';

export enum SubscribeServiceResponseStatus {
  statussubscribed = 200,
  // Since member exist status is 400, and we count it like success 200
  status400 = 200,
  statusnotfound = 404,
  statusservererror = 500,
}
export interface ISubscribeModel {
  email: string;
  lists: {id: string, groups: string[]}[];
  agreementText: {title: string, description: string};
}

interface ISubscribeResponseModel {
  status: number;
  listId: string;
  title: string;
}
@Injectable()
export class SubscribeService {

  constructor(
    private http: HttpClient,
    @Inject(SUBSCRIBE_MODULE_CONFIG) private config: ISubscribeConfig
  ) { }

  subscribe(data: ISubscribeModel)
  {
    return this.http.post(`${this.config.api}/MailchimpPanel/subscribe`, data).pipe(
      catchError(err => {
        return err.status === 404 ? of([{status: 'notfound'}]) : of([{status: 'servererror'}]);
      }),
      map((data: ISubscribeResponseModel[]) => {

        const parsed = data.reduce((a: ISubscribeResponseModel, b: ISubscribeResponseModel) => {
          if (a.status < b.status) a = b;
          return a;
        }, {status: 0, listId: null, title: null});

        return  SubscribeServiceResponseStatus[`status${parsed.status}`];
      })).toPromise();
  }
}
