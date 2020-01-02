import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { INotificationsData } from 'src/app/feature/project/specific/nav-notifications/nav-notifications.service';

@Component({
  selector: 'notifications-page',
  templateUrl: './notifications-page.html',
  styleUrls: ['./notifications-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class NotificationsPage {
  data: any;
  notifications: INotificationsData[];

  constructor(
    private route: ActivatedRoute
  ) {
    this.route.data.subscribe(data => this.data = data);
  }

  ngOnInit() {
    this.notifications = this.data.notifications.toJSON();
  }
}
