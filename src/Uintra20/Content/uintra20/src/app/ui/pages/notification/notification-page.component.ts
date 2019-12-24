import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'notification-page',
  templateUrl: './notification-page.html',
  styleUrls: ['./notification-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class NotificationPage {
  data: any;

  constructor(
    private route: ActivatedRoute
  ) {
    this.route.data.subscribe(data => this.data = data);
    debugger
  }
}
