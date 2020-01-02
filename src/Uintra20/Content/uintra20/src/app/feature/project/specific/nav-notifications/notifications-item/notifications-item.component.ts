import { Component, OnInit, Input } from '@angular/core';
import { INotificationsData } from '../nav-notifications.service';

@Component({
  selector: 'app-notifications-item',
  templateUrl: './notifications-item.component.html',
  styleUrls: ['./notifications-item.component.less']
})
export class NotificationsItemComponent implements OnInit {
  @Input() notification: INotificationsData;
  @Input() link: string;

  constructor() { }

  ngOnInit() {
  }
}
