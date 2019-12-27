import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-nav-notifications',
  templateUrl: './nav-notifications.component.html',
  styleUrls: ['./nav-notifications.component.less']
})
export class NavNotificationsComponent implements OnInit {

  isShow: boolean = false;

  constructor() { }

  ngOnInit() {
  }

  show() {
    this.isShow = true;
  }
  hide() {
    this.isShow = false;
  }
}
