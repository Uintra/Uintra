import {Injectable} from "@angular/core";
import {interval, from} from "rxjs";
import {zip} from "rxjs/operators";
import {NavNotificationsService} from "../nav-notifications.service";

@Injectable({
  providedIn: "root"
})
export class DesktopNotificationService {
  constructor(private navNotificationsService: NavNotificationsService) {
  }

  createNotifications(notificationsForDesktop) {
    from(notificationsForDesktop)
      .pipe(zip(interval(1000), a => a))
      .subscribe(notification => {
        this.createDesktopNotifications(notification);
      });
  }

  LoadDocument(url): Promise<any> {
    const xhr = new XMLHttpRequest();
    xhr.open('GET', url);
    xhr.responseType = 'blob';
    xhr.send();

    return new Promise<any>((resolve, reject) => {
      xhr.onload = function () {
        resolve(xhr.response)
      }
      xhr.onerror = function () {
        reject("Load error")
      }
    })
  }

  DownloadNotifierAvatar(xhrResponse: any): Promise<string> {

    const reader = new FileReader();
    reader.readAsDataURL(xhrResponse);

    return new Promise((resolve, reject) => {
      reader.onloadend = function () {
        resolve(reader.result.toString())
      }
      reader.onerror = function () {
        reject(reader.error)
      }
    })
  }

  async createDesktopNotifications(notification) {
    const _this = this;
    let avatar = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyJpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6MDFBMDYyNzUxMzZCMTFFOTk5NDJENkNBN0M1NDVGQTAiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6MDFBMDYyNzYxMzZCMTFFOTk5NDJENkNBN0M1NDVGQTAiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDowMUEwNjI3MzEzNkIxMUU5OTk0MkQ2Q0E3QzU0NUZBMCIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDowMUEwNjI3NDEzNkIxMUU5OTk0MkQ2Q0E3QzU0NUZBMCIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/Pk3TRCkAAAFHSURBVHja7JfZaoRAEEXLBUSRuIAbPor//zs++WKQEXFJK6jgkpQwZMGgaeI0DBaIIjT30H1raW5ZlgQAXoBNEO4DYAGGwSMFQ33CA+O4AC4AkXZhXdfrM00TOI4Dqqo+DiBJEsjzHO4lhBACtm2D7/vnH0Hbtt/EMXAXbrcbDMPwGICfxZPjuPXdNA1bE87zzBbgvhOnAvzWu1AcvXA6gCRJm/9RXNO08wFM09wUMgwDZFk+HwCNZlkW8PznUkEQwHVdKhPiQPJ2ZCKqqgrKslxTDYW2UhFBFEUBz/OOVkayC4BnG8fxKnxkeEIQBNR1HYIg2MsMIu45Pooi6Pv+T1mCokVRrN9hGNJ7AMtr13V0TUYU1x6RZRk9QJqmVMXla2DfoO6G2OHGcaSGQC+gKf8lC66p+AJ4WgDml1OsA68s0/BdgAEAmjaocUhguUgAAAAASUVORK5CYII=";

    if (notification.Notifier.Photo) {
      let loadResponse = await this.LoadDocument(notification.Notifier.Photo);
      avatar = await this.DownloadNotifierAvatar(loadResponse);
    }

    const objParam = {
      body: notification.Value.desktopMessage,
      icon: avatar,
      requireInteraction: true,
      timeout: 5000
    };

    this.navNotificationsService
      .markAsNotified(notification.Id)
      .subscribe(r => {
      });
    const newDeskNotification = new Notification(
      notification.Value.desktopTitle,
      objParam
    );

    setTimeout(() => {
      newDeskNotification.close();
    }, 5000);

    newDeskNotification.onclick = function () {
      const pushWindow = this;

      _this.navNotificationsService
        .markAsViewed(notification.Id)
        .subscribe(r => {
          const destUrl = window.location.origin + notification.Value.url.originalUrl;
          window.focus();
          window.location.assign(destUrl);
          if (_this.equals(destUrl, window.location.href)) {
            window.location.reload();
          }
          pushWindow.close();
        });
    };
  }

  equals(destUrl, currentUrl) {
    const destIndex = destUrl.indexOf("#");
    const destPath =
      destIndex !== -1 ? destUrl.substring(0, destIndex) : destUrl;
    const currentIndex = currentUrl.indexOf("#");
    const currentPath =
      currentIndex !== -1 ? currentUrl.substring(0, currentIndex) : currentUrl;
    return destPath === currentPath;
  }
}
