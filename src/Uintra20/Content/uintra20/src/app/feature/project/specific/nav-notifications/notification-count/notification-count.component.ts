import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-notification-count',
  templateUrl: './notification-count.component.html',
  styleUrls: ['./notification-count.component.less']
})
export class NotificationCountComponent {
  @Input() count: number;

  constructor() { }
}
