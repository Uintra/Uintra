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
// tslint:disable-next-line: component-class-suffix
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
