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
  status: number | string;
  listId: string | null;
  title: string | null;
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
        return err.status === 404 ? of([{status: 404}]) : of([{status: 500}]);
      }),
      map((data: ISubscribeResponseModel[]) => {

        const parsed = data.reduce((a: ISubscribeResponseModel, b: ISubscribeResponseModel) => {
          b.status = this.normalizeStatus(b.status);
          a.status = this.normalizeStatus(a.status);

          if (a.status < b.status) a = b;

          return a;
        }, {status: 0, listId: null, title: null});

        return  parsed.status;
      })).toPromise();
  }

  private normalizeStatus(status: string | number): number
  {
    const n = parseInt(status as string);
    if (Number.isInteger(n) && n > 400) return n;

    return 200;
  }
}
