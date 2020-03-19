import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class EventSubscriptionService {

  constructor(private http: HttpClient) { }

  subscribe(activityId: string) {
    return this.http.post(`/ubaseline/api/Subscribe/Subscribe?activityId=${activityId}`, {})
  }

  unsubscribe(activityId: string) {
    return this.http.post(`/ubaseline/api/Subscribe/Unsubscribe?activityId=${activityId}`, {})
  }

  toggleNotification(activityId: string, value: boolean) {
    return this.http.post(`/ubaseline/api/Subscribe/ChangeNotificationDisabled?activityId=${activityId}&value=${value}`, {})
  }

  getListOfUsers(activityId: string) {
    return this.http.get(`/ubaseline/api/Subscribe/List?activityId=${activityId}`);
  }
}
