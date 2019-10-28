import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SUBSCRIBE_MODULE_CONFIG, ISubscribeConfig } from '../config';
import { catchError, map } from 'rxjs/operators';
import { of } from 'rxjs';

export enum SubscribeServiceResponseStatus {
  statussubscribed = 200,
  status400 = 400,
  statusnotfound = 404,
  statusservererror = 500,
}
export interface ISubscribeModel {
  email: string;
  listIds: string[];
  agreementText: {title: string, description: string};
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
        return err.status === 404 ? of({status: 'notfound'}) : of({status: 'servererror'});
      }),
      map((data: {status: string}) => {
        return  SubscribeServiceResponseStatus[`status${data.status}`];
      })).toPromise();
  }
}
