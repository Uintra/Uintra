import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Subject } from 'rxjs';
import { ISocialCreateModel, INewsCreateModel, ISocialEdit } from './activity.interfaces';

@Injectable({
  providedIn: 'root'
})
export class ActivityService {
  private feedRefreshTrigger = new Subject();
  private routePrevix = '/ubaseline/api/';

  feedRefreshTrigger$ = this.feedRefreshTrigger.asObservable();

  constructor(private http: HttpClient) { }

  public submitSocialContent = (data: ISocialCreateModel) =>
    this.http.post(`${this.routePrevix}social/create`, data)

  public updateSocial = (model: ISocialEdit) =>
    this.http.put(`${this.routePrevix}social/Update`, model)

  public deleteSocial = (id: string) =>
    this.http.delete(`${this.routePrevix}social/Delete`, {
      params: new HttpParams().set('id', id)
    })

  public updateNews = (model: INewsCreateModel) =>
    this.http.put(`${this.routePrevix}newsApi/edit`, model)

  public submitNewsContent = (data: INewsCreateModel) =>
    this.http.post(`${this.routePrevix}newsApi/create`, data)

  public createEvent = (data) =>
    this.http.post(`${this.routePrevix}events/create`, data)

  public updateEvent = (data) =>
    this.http.put(`${this.routePrevix}events/edit`, data)

  public hideEvent = (id, isNotificationNeeded) =>
    this.http.post(`${this.routePrevix}events/hide?id=${id}&isNotificationNeeded=${isNotificationNeeded}`, {})

  public refreshFeed = (): void =>
    this.feedRefreshTrigger.next()
}
