import { Component, OnInit } from '@angular/core';
import { NavNotificationsService, INotificationsData } from './nav-notifications.service';

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

  constructor(private navNotificationsService: NavNotificationsService) { }

  ngOnInit() {
    this.navNotificationsService.getNotifiedCount().subscribe(count => {
      this.notificationCount = count;
    });
    // this.connection = $.hubConnection('/umbraco/backoffice/signalr/hubs');
    // this.proxy = this.connection.createHubProxy('ProcessingHub');

    const notificationsHub = $.connection.notificationsHub;
    this.proxy = $.connection.notificationsHub;

    $.connection.hub.start().done(r => {
      debugger;
      var count=$.connection.notificationsHub.server.getNotNotifiedCount('28241e2b-e993-4f60-9357-577b5d42eb63');
      }).catch(r => {
        debugger;

      });
    // .done(r => {
    //   debugger;
    // }).catch(r => {
    //   debugger;

    // })



    //   .start().done((data: any) => {
    //     console.log('Connected to Processing Hub');
    // }).catch((error: any) => {
    //     console.log('Hub error -> ' + error);
    // });
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
