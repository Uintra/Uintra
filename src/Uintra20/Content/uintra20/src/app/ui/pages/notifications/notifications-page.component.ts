import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'notifications-page',
  templateUrl: './notifications-page.html',
  styleUrls: ['./notifications-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class NotificationsPage {
  data: any;

  constructor(
    private route: ActivatedRoute
  ) {
    this.route.data.subscribe(data => this.data = data);
  }
}