import { Component, OnInit, NgZone, ChangeDetectorRef, OnDestroy } from "@angular/core";
import {
  NavNotificationsService,
  INotificationsData,
  INotificationsListData
} from "./nav-notifications.service";
import { SignalrService } from "./helpers/signalr.service";
import { DesktopNotificationService } from "./helpers/desktop-notification.service";
import { Subject } from 'rxjs';

declare let $: any;

@Component({
  selector: "app-nav-notifications",
  templateUrl: "./nav-notifications.component.html",
  styleUrls: ["./nav-notifications.component.less"]
})
export class NavNotificationsComponent implements OnInit, OnDestroy {
  notifications: INotificationsData[];
  notificationCount: number;
  notificationPageUrl: string;
  isShow = false;
  isLoading = false;

  permission: NotificationPermission = null;

  constructor(
    private desktopNotificationService: DesktopNotificationService,
    private signalrService: SignalrService,
    private navNotificationsService: NavNotificationsService,
    private ngNotificationZone: NgZone
  ) {}

  ngOnInit() {
    this.navNotificationsService
      .getNotifiedCount()
      .subscribe((response: number) => {
        this.notificationCount = response;
      });
    
    this.signalrService.startHub();
    this.signalrService.getUpdateNotificationsSubjects().subscribe(notifications => {      
      this.getNewNotification(notifications)
    })

    if ("Notification" in window) {
      Notification.requestPermission(status => {
        return (this.permission = status);
      });
    }
  }

  ngOnDestroy(): void {
    //debugger;
    //this.signalrService.hubConnectionStop();
  }

  getNewNotification(notifications = []) {
    if (this.permission === "granted") {
      this.createDesktopNotifications(notifications);
    } else {
      this.setNotificationCount(notifications.length);
    }
  }

  handleClick() {
    this.hide();
  }

  private setNotificationCount(count: number) {
    this.ngNotificationZone.run(() => {
      this.notificationCount = count;
    });
  }

  private createDesktopNotifications(notifications) {
    this.ngNotificationZone.runOutsideAngular(() => {

      const notificationsForDesktop = notifications.filter(
        notification => notification.Value.isDesktopNotificationEnabled
      );
      const notificationsForWeb = notifications.filter(
        notification => !notification.Value.isDesktopNotificationEnabled
      );

      this.desktopNotificationService.createNotifications(
        notificationsForDesktop
      );

      this.setNotificationCount(notificationsForWeb.length);
    });
  }

  private loadNotifications() {
    this.isLoading = true;

    this.navNotificationsService
      .getNotifications()
      .subscribe((response: INotificationsListData) => {
        this.notifications = response.notifications;
        this.notificationPageUrl = response.notificationPageUrl;
        this.isLoading = false;
      });
  }

  onToggle() {
    if (!this.isShow) {
      this.notifications = null;
      this.notificationCount = 0;
      this.show();
      this.loadNotifications();
    } else {
      this.hide();
    }
  }

  show() {
    this.isShow = true;
  }
  hide() {
    this.isShow = false;
  }
}
