import { Component, ViewEncapsulation, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ActivityService } from 'src/app/feature/specific/activity/activity.service';
import { IULink } from 'src/app/shared/interfaces/general.interface';
import { HasDataChangedService } from 'src/app/shared/services/general/has-data-changed.service';
import { Observable, Subscription } from 'rxjs';
import { CanDeactivateGuard } from 'src/app/shared/services/general/can-deactivate.service';
import { IEventCreatePage } from 'src/app/shared/interfaces/pages/event/create/event-create-page';
import { AppService } from 'src/app/app.service';

@Component({
  selector: 'event-create-page',
  templateUrl: './event-create-page.html',
  styleUrls: ['./event-create-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class EventCreatePage implements OnDestroy {

  private $eventSubscription: Subscription;
  public data: IEventCreatePage;
  public inProgress: boolean;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private activityService: ActivityService,
    private hasDataChangedService: HasDataChangedService,
    private canDeactivateService: CanDeactivateGuard,
    private appService: AppService
  ) {
    this.activatedRoute.data.subscribe((data: IEventCreatePage) => {
      this.data = data;
      this.appService.setPageAccess(data.allowAccess);
    });
  }

  public ngOnDestroy(): void {
    if (this.$eventSubscription) { this.$eventSubscription.unsubscribe(); }
  }

  public onSubmit(data): void {
    this.inProgress = true;
    this.$eventSubscription = this.activityService.createEvent(data).subscribe((res: IULink) => {
      this.hasDataChangedService.reset();
      this.router.navigate([res.originalUrl]);
    }, () => this.inProgress = false);
  }

  public onCancel(): void {
    this.router.navigate([this.data.data.links.feed.originalUrl]);
  }

  public canDeactivate(): Observable<boolean> | boolean {
    if (this.hasDataChangedService.hasDataChanged) {
      return this.canDeactivateService.canDeacrivateConfirm();
    }

    return true;
  }
}
