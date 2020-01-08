import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface INotificationsData {
  id: string;
  date: string;
  isNotified: boolean;
  isViewed: boolean;
  type: number;

  notifier: {
    id: string;
    displayedName: string;
    photo: string;
    photoId: number;
    email: string;
    loginName: string;
    inactive: boolean;
  };

  value: {
    message: string;
    url: string;
    notifierId: string;
    isPinned: boolean;
    isPinActual: boolean;
    desktopMessage: string;
    desktopTitle: string;
    isDesktopNotificationEnabled: boolean;
  };
}

@Injectable({
  providedIn: 'root'
})
export class NavNotificationsService {
  readonly api = '/ubaseline/api/notificationApi';

  constructor(
    private http: HttpClient
  ) { }

  getNotifications(): Observable<INotificationsData[]> {
    return this.http.get<INotificationsData[]>(this.api + `/NotificationList`);
  }

  getNotifiedCount(): Observable<number> {
    return this.http.get<number>(this.api + `/GetNotNotifiedCount`);
  }
}
