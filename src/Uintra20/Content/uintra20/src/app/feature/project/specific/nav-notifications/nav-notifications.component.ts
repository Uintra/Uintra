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
  isLoading: boolean = false;

  constructor(private navNotificationsService: NavNotificationsService) { }

  ngOnInit() {
    this.navNotificationsService.getNotifiedCount().subscribe(count => {
      this.notificationCount = count;
    })
  }

  onShow() {
    this.notifications = null;
    this.show();
    this.loadNotifications();
  }

  loadNotifications() {
    this.isLoading = true;

    this.navNotificationsService.getNotifications().subscribe( (response: INotificationsData[]) => {
      this.notifications = response;
      this.isLoading = false;
    });
  }

  show() {
    this.isShow = true;
  }
  hide() {
    this.isShow = false;
  }
}
