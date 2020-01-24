import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface INotificationsListData {
  notificationPageUrl: string;
  notifications: Array<INotificationsData>;
}

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
    url: {
      baseUrl: string;
      params: Array<{
        name: string;
        value: string;
      }>;
    };
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

  getNotifications(): Observable<INotificationsListData> {
    return this.http.get<INotificationsListData>(this.api + `/NotificationList`);
  }

  getNotificationsByPage(page: number): Observable<INotificationsListData> {
    return this.http.get<INotificationsListData>(this.api + `/Get?page=${page}`);
  }

  getNotifiedCount(): Observable<number> {
    return this.http.get<number>(this.api + `/GetNotNotifiedCount`);
  }

  markAsViewed(id: string): Observable<number> {
    return this.http.post<number>(this.api + `/View?id=${id}`, {});
  }

  markAsNotified(id: string): Observable<boolean> {
    return this.http.post<boolean>(this.api + `/Notified?id=${id}`, {});
  }
}
