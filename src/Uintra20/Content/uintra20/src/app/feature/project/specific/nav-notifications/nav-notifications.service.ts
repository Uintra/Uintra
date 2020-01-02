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
  }

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

  constructor(
    private http: HttpClient
  ) { }

  getNotifications(): Observable<INotificationsData[]> {
    return this.http.get<INotificationsData[]>(`/ubaseline/api/notificationApi/NotificationList`);
  }

  getNotifiedCount(): Observable<number> {
    return this.http.get<number>(`/ubaseline/api/notificationApi/GetNotNotifiedCount`);
  }
}
