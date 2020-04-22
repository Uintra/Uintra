import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { NavNotificationsService } from '../nav-notifications.service';
import { INotificationsData } from 'src/app/shared/interfaces/pages/notifications/notifications-page.interface';

@Component({
  selector: 'app-notifications-item',
  templateUrl: './notifications-item.component.html',
  styleUrls: ['./notifications-item.component.less']
})
export class NotificationsItemComponent implements OnInit {
  @Input() notification: INotificationsData;
  @Input() link: string;
  @Output() handleClick = new EventEmitter();

  get notificationUrlParams() {
    const params = this.notification.value.url.params;

    if (params && Array.isArray(params)) {
      return params.reduce((acc, val) => {
        acc[val.name] = val.value;
        return acc;
      }, {});
    }

    return {};
  }

  constructor(private navNotificationsService: NavNotificationsService) { }

  ngOnInit() {
  }

  onMarkAsViewed() {
    this.handleClick.emit();
    this.navNotificationsService.markAsViewed(this.notification.id).subscribe(r => {
      this.notification.isViewed = true;
    });
  }
}
