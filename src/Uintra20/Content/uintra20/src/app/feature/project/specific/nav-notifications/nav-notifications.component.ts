import { Component, OnInit, NgZone } from '@angular/core';
import { NavNotificationsService, INotificationsData } from './nav-notifications.service';
import { NavigationEnd, Router } from '@angular/router';
import { SignalrService } from './helpers/signalr.service';

declare var $: any;

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

  constructor(
    private signalrService: SignalrService,
    private navNotificationsService: NavNotificationsService,
    private ngZone: NgZone) { }

  ngOnInit() {
    this.navNotificationsService.getNotifiedCount().subscribe((response: number) => {
      this.notificationCount = response;
    });

    this.signalrService.createHub(this.updateNotificationCountValue.bind(this));
  }

  updateNotificationCountValue(count: number) {
    this.ngZone.run(() => {
      this.notificationCount = count;
    });
  }

  loadNotifications() {
    this.isLoading = true;

    this.navNotificationsService.getNotifications().subscribe((response: INotificationsData[]) => {
      this.notifications = response;
      this.isLoading = false;
    });
  }

  onShow() {
    this.notifications = null;
    this.notificationCount = 0;
    this.show();
    this.loadNotifications();
  }

  show() { this.isShow = true; }
  hide() { this.isShow = false; }
}
