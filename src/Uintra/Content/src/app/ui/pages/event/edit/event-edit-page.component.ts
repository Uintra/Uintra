import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ActivityService } from 'src/app/feature/specific/activity/activity.service';
import { IULink } from 'src/app/shared/interfaces/general.interface';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { HasDataChangedService } from 'src/app/shared/services/general/has-data-changed.service';
import { CanDeactivateGuard } from 'src/app/shared/services/general/can-deactivate.service';
import { IEventEditPage } from 'src/app/shared/interfaces/pages/event/edit/event-edit-page.interface';
import { AppService } from 'src/app/app.service';

@Component({
  selector: 'event-edit-page',
  templateUrl: './event-edit-page.html',
  styleUrls: ['./event-edit-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class EventEditPage {

  public data: IEventEditPage;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private activityService: ActivityService,
    private translate: TranslateService,
    private hasDataChangedService: HasDataChangedService,
    private canDeactivateService: CanDeactivateGuard,
    private appService: AppService
  ) {
    this.route.data.subscribe((data: IEventEditPage) => {
      this.data = data;
      this.appService.setPageAccess(data.allowAccess);

      //TODO move data mapping to back-end...
      this.data.details.title = this.data.details.headerInfo.title;
      this.data.details.creator = { id: this.data.details.headerInfo.owner.id };
      this.data.details.pinAllowed = this.data.pinAllowed;
      this.data.details.members = this.data.members;

      this.data.details.selectedTags = this.data.details.tags;
      this.data.details.lightboxPreviewModel.medias = this.data.details.lightboxPreviewModel.medias || [];
      this.data.details.lightboxPreviewModel.otherFiles = this.data.details.lightboxPreviewModel.otherFiles || [];
    });
  }

  public onSubmit(data): void {
    const mapedData = this.requesModelBuilder(data);
    this.activityService.updateEvent(mapedData).subscribe((res: IULink) => {
      this.hasDataChangedService.reset();
      this.router.navigate([res.originalUrl]);
    });
  }

  public onCancel(): void {
    this.router.navigate([this.data.links.details.originalUrl]);
  }

  public onHide(): void {
    if (confirm(this.translate.instant('common.AreYouSure'))) {
      const isNotificationNeeded = !this.data.details.hasSubscribers || confirm(this.translate.instant('common.NotifyAllSubscribers'));
      this.activityService.hideEvent(this.data.details.id, isNotificationNeeded).subscribe((res: IULink) => {
        this.hasDataChangedService.reset();
        this.router.navigate([this.data.links.feed.originalUrl]);
      });
    }
  }

  public requesModelBuilder(data): void {
    const otherFilesIds = data.media.otherFiles.map(m => m.id);
    const mediaIds = data.media.medias.map(m => m.id);

    data.media = otherFilesIds.concat(mediaIds).join(',');
    data['id'] = this.data.details.id;
    data['notifyAllSubscribers'] = this.data.details.hasSubscribers
      ? confirm(this.translate.instant('common.NotifyAllSubscribers'))
      : false;

    return data;
  }

  public canDeactivate(): Observable<boolean> | boolean {
    if (this.hasDataChangedService.hasDataChanged) {
      return this.canDeactivateService.canDeacrivateConfirm();
    }

    return true;
  }
}
