import { Component, OnInit } from '@angular/core';
import { NavNotificationsService, INotificationsData } from './nav-notifications.service';
import { NavigationEnd, Router } from '@angular/router';

declare var $: any;

@Component({
  selector: 'app-nav-notifications',
  templateUrl: './nav-notifications.component.html',
  styleUrls: ['./nav-notifications.component.less']
})
export class NavNotificationsComponent implements OnInit {

  private connection: any;
  private proxy: any;
  private ulr: any;


  notifications: INotificationsData[];
  notificationCount: number;
  isShow: boolean = false;
  isLoading: boolean = false;

  constructor(private navNotificationsService: NavNotificationsService, private router: Router) {
    router.events.subscribe((val) => {
      if (val instanceof NavigationEnd) {
        this.hide();
      }
    });
  }

  ngOnInit() {
    this.navNotificationsService.getNotifiedCount().subscribe(count => {
      this.notificationCount = count;
    });
    // this.connection = $.hubConnection('/umbraco/backoffice/signalr/hubs');
    // this.proxy = this.connection.createHubProxy('ProcessingHub');

    this.proxy = $.connection.notificationsHub;

    $.connection.hub.start().done(r => {

      this.proxy.server.getNotNotifiedCount('28241e2b-e993-4f60-9357-577b5d42eb63').then((count) => {
        debugger;
      });

      }).catch(r => {
        debugger;
      });
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
