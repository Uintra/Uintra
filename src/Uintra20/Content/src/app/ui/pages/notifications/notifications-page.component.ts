import { Component, ViewEncapsulation, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AddButtonService } from '../../main-layout/left-navigation/components/my-links/add-button.service';
import { NavNotificationsService } from 'src/app/feature/specific/nav-notifications/nav-notifications.service';
import { INotificationsPage, INotificationsData } from 'src/app/shared/interfaces/pages/notifications/notifications-page.interface';
import { Subscription } from 'rxjs';

@Component({
  selector: 'notifications-page',
  templateUrl: './notifications-page.html',
  styleUrls: ['./notifications-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class NotificationsPage implements OnInit, OnDestroy {

  public data: INotificationsPage;
  public notifications: INotificationsData[] = [];
  public currentPage: number;
  public isLoading = false;
  public isScrollDisabled = false;
  public notificationsSubscription: Subscription;

  constructor(
    private route: ActivatedRoute,
    private navNotificationsService: NavNotificationsService,
    private addButtonService: AddButtonService
  ) {
    this.route.data.subscribe((data: INotificationsPage) => {
      this.data = data;
      this.addButtonService.setPageId(data.id.toString());
    });
  }

  public ngOnInit(): void {
    this.currentPage = 1;
    this.getNotifications();
  }

  public ngOnDestroy(): void {
    this.notificationsSubscription.unsubscribe();
  }

  public onScroll(): void {
    this.currentPage += 1;
    this.getNotifications();
  }

  public addNotifications(notifications = []): void {
    if (notifications.length) {
      this.notifications = this.notifications.concat(notifications);
    } else {
      this.isScrollDisabled = true;
    }
  }

  public getNotifications(): void {
    this.isLoading = true;

    this.notificationsSubscription = this.navNotificationsService
      .getNotificationsByPage(this.currentPage)
      .subscribe((response: any) => {
        this.addNotifications(response);
      });
  }

  public trackIndex = (index, item): string => item.id;
}
