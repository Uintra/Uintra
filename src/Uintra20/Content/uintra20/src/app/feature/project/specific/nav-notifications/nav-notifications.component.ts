import { Component, OnInit } from '@angular/core';
import { NavNotificationsService, INotificationsData, INotificationsListData } from './nav-notifications.service';

@Component({
  selector: 'app-nav-notifications',
  templateUrl: './nav-notifications.component.html',
  styleUrls: ['./nav-notifications.component.less']
})
export class NavNotificationsComponent implements OnInit {
  notifications: INotificationsData[];
  notificationCount: number;
  isShow: boolean = false;

  constructor(private navNotificationsService: NavNotificationsService) { }

  ngOnInit() {
    this.navNotificationsService.getNotifications().subscribe( (response: INotificationsListData) => {
      this.notifications = response.notifications;
    });

    this.navNotificationsService.getNotifiedCount().subscribe(count => {
      this.notificationCount = count;
    })
  }

  show() {
    this.isShow = true;
  }
  hide() {
    this.isShow = false;
  }
}
