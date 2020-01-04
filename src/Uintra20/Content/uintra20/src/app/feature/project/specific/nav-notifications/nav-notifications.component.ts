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

  private notificationsHub: any;
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
  }

  setNotNotifiedCount() {
    this.notificationsHub.server.getNotNotifiedCount()
      .then((data) => {        
        this.updateNotificationCountValue(data);
      });
  } 

  onShow() {
    this.notifications = null;    
    this.show();
    this.loadNotifications();
  }

  updateNotificationCountValue(count: any) {
    debugger;
    this.notificationCount = count;
  }

  subscribe() {   

    this.notificationsHub = $.connection.notificationsHub;
    this.notificationsHub.client.updateNotificationsCount = this.updateNotificationCountValue;

    $.connection.hub.disconnected(function () {
      if ($.connection.hub.lastError) { alert("Disconnected. Reason: " + $.connection.hub.lastError.message); }
    });

    $.connection.hub.reconnected(function () {
      $.connection
        .hub
        .start()
        .done(r => {
          this.setNotNotifiedCount();
        })
        .catch(r => {
          console.log(r)
        });

    });

    $.connection
      .hub
      .start()
      .done(r => {
        this.setNotNotifiedCount();
      })
      .catch(r => {
        console.log(r)
      });
  }

  loadNotifications() {
    this.isLoading = true;

    this.navNotificationsService.getNotifications().subscribe((response: INotificationsData[]) => {
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
