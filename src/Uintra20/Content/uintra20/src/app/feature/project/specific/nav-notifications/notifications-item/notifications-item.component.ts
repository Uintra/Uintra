import { Component, OnInit, Input } from '@angular/core';
import { INotificationsData, NavNotificationsService } from '../nav-notifications.service';

@Component({
  selector: 'app-notifications-item',
  templateUrl: './notifications-item.component.html',
  styleUrls: ['./notifications-item.component.less']
})
export class NotificationsItemComponent implements OnInit {
  @Input() notification: INotificationsData;
  @Input() link: string;

  constructor(private navNotificationsService: NavNotificationsService) { }

  ngOnInit() {
  }

  onMarkAsViewed() {
    this.navNotificationsService.markAsViewed(this.notification.id).subscribe(r => {
      this.notification.isViewed = true;
    });
  }
}
