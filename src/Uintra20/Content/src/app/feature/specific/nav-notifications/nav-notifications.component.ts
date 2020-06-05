import {Component, OnInit, NgZone, OnDestroy} from "@angular/core";
import {NavNotificationsService} from "./nav-notifications.service";
import {DesktopNotificationService} from "./helpers/desktop-notification.service";
import {SignalrService} from "src/app/shared/services/general/signalr.service";
import {
  INotificationsData,
  INotificationsListData
} from 'src/app/shared/interfaces/pages/notifications/notifications-page.interface';
import {PopUpComponent} from "../../../shared/ui-elements/pop-up/pop-up.component";
import {ModalService} from "../../../shared/services/general/modal.service";

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
    private modalService: ModalService,
    private desktopNotificationService: DesktopNotificationService,
    private signalrService: SignalrService,
    private navNotificationsService: NavNotificationsService,
    private ngNotificationZone: NgZone
  ) {
  }

  ngOnInit() {
    this.navNotificationsService
      .getNotifiedCount()
      .subscribe((response: number) => {
        this.notificationCount = response;
      });


    //this.signalrService.startHub();
    // this.signalrService
    //   .getUpdateNotificationsSubjects()
    //   .subscribe(notifications => {
    //     this.getNewNotification(notifications);
    //   });

    // this.signalrService
    //   .getShowPopup()
    //   .subscribe(n => {
    //     n.forEach((val, index, arr) => {
    //       this.modalService.appendComponentToBody(PopUpComponent, {data: val}, null, index.toString(), index === arr.length - 1);
    //     })

    //   })

    if ("Notification" in window) {
      Notification.requestPermission(status => {
        return (this.permission = status);
      });
    }
  }

  ngOnDestroy(): void {
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
