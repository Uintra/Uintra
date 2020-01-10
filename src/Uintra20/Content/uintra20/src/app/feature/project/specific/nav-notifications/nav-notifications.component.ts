import { Component, OnInit, NgZone } from "@angular/core";
import {
  NavNotificationsService,
  INotificationsData,
  INotificationsListData
} from "./nav-notifications.service";
import { SignalrService } from "./helpers/signalr.service";
import { DesktopNotificationService } from './helpers/desktop-notification.service';

declare let $: any;

@Component({
  selector: "app-nav-notifications",
  templateUrl: "./nav-notifications.component.html",
  styleUrls: ["./nav-notifications.component.less"]
})
export class NavNotificationsComponent implements OnInit {
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
    private ngZone: NgZone
  ) {}

  ngOnInit() {
    this.navNotificationsService
      .getNotifiedCount()
      .subscribe((response: number) => {
        this.notificationCount = response;
      });

    this.signalrService.createHub(this.getNewNotification.bind(this));

    if ("Notification" in window) {
      Notification.requestPermission(status => {
        return (this.permission = status);
      });
    }
  }

  getNewNotification(notifications = []) {
    if (this.permission === "granted") {
      this.ngZone.runOutsideAngular(() => {

        const notificationsForDesktop = notifications.filter(
          notification => notification.Value.isDesktopNotificationEnabled
        );
        const notificationsForWeb = notifications.filter(
          notification => !notification.Value.isDesktopNotificationEnabled
        );

        this.desktopNotificationService.createNotifications(notificationsForDesktop)

        this.setNotificationCount(notificationsForWeb.length);
      });
    } else {
      this.setNotificationCount(notifications.length);
    }
  }

  setNotificationCount(count: number) {
    this.ngZone.run(() => {
      this.notificationCount = count;
    });
  }

  loadNotifications() {
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
