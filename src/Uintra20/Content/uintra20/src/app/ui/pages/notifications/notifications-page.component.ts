import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { INotificationsData, NavNotificationsService } from 'src/app/feature/project/specific/nav-notifications/nav-notifications.service';
import { UmbracoFlatPropertyModel } from '@ubaseline/next';

@Component({
  selector: 'notifications-page',
  templateUrl: './notifications-page.html',
  styleUrls: ['./notifications-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class NotificationsPage {
  data: any;
  notifications: INotificationsData[] = [];
  currentPage: number;
  isLoading: boolean = false;
  isScrollDisabled: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private navNotificationsService: NavNotificationsService
  ) {
    this.route.data.subscribe(data => this.data = data);
  }

  ngOnInit() {
    // this.notifications = this.data.notifications.toJSON();
    this.currentPage = 1;
    this.getNotifications();
  }

  onScroll() {
    this.currentPage += 1;
    this.getNotifications();
  }

  addNotifications(notifications = []) {
    if (notifications.length) {
      this.notifications = this.notifications.concat(notifications);
    } else {
      this.isScrollDisabled = true;
    }
    // const newNotificationsArray = notifications.map(notification => {
    //   return new UmbracoFlatPropertyModel(notification);
    // });
    // Array.prototype.push.apply(this.notifications, newNotificationsArray);
  }

  getNotifications() {

    this.isLoading = true;

    this.navNotificationsService
      .getNotificationsByPage(this.currentPage)
      .subscribe((response: any) => {
        this.addNotifications(response);
      });
  }
}
