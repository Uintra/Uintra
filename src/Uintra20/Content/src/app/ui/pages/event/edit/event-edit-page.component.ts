import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ActivityService } from 'src/app/feature/specific/activity/activity.service';
import ParseHelper from 'src/app/shared/utils/parse.helper';
import { IULink } from 'src/app/shared/interfaces/general.interface';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { HasDataChangedService } from 'src/app/shared/services/general/has-data-changed.service';
import { CanDeactivateGuard } from 'src/app/shared/services/general/can-deactivate.service';

@Component({
  selector: 'event-edit-page',
  templateUrl: './event-edit-page.html',
  styleUrls: ['./event-edit-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class EventEditPage {
  data: any;
  parsedData: any;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private activityService: ActivityService,
    private translate: TranslateService,
    private hasDataChangedService: HasDataChangedService,
    private canDeactivateService: CanDeactivateGuard,
  ) {
    this.route.data.subscribe(data => {
      if (!data.requiresRedirect.get()) {
        this.data = data;
        this.parsedData = ParseHelper.parseUbaselineData(this.data);
        this.parsedData.details.members = Object.values(this.parsedData.members);
        this.parsedData.details.availableTags = Object.values(this.parsedData.details.availableTags);
        this.parsedData.details.selectedTags = Object.values(this.parsedData.details.tags);
        this.parsedData.details.lightboxPreviewModel.medias = Object.values(this.parsedData.details.lightboxPreviewModel.medias || []);
        this.parsedData.details.lightboxPreviewModel.otherFiles = Object.values(this.parsedData.details.lightboxPreviewModel.otherFiles || []);
        this.parsedData.details.title = this.parsedData.details.headerInfo.title;
        this.parsedData.details.creator = {id: this.parsedData.details.headerInfo.owner.id};
        this.parsedData.details.pinAllowed = this.parsedData.pinAllowed;
      } else {
        this.router.navigate([data.errorLink.get().originalUrl.get()]);
      }
    });
  }

  onSubmit(data) {
    const mapedData = this.requesModelBuilder(data);
    this.activityService.updateEvent(mapedData).subscribe((res: IULink) => {
      this.router.navigate([res.originalUrl]);
    })
  }

  onCancel() {
    this.router.navigate([this.parsedData.links.details.originalUrl]);
  }

  onHide() {
    if (confirm(this.translate.instant('common.AreYouSure'))) {
      const isNotificationNeeded = confirm(this.translate.instant('common.NotifyAllSubscribers'));
      this.activityService.hideEvent(this.parsedData.details.id, isNotificationNeeded).subscribe((res: IULink) => {
        this.router.navigate([this.parsedData.links.feed.originalUrl]);
      })
    }
  }

  requesModelBuilder(data) {
    const copyObject = JSON.parse(JSON.stringify(data));

    const otherFilesIds = copyObject.media.otherFiles.map(m => m.id);
    const mediaIds = copyObject.media.medias.map(m => m.id);

    copyObject.media = otherFilesIds.concat(mediaIds).join(',');
    copyObject["id"] = this.parsedData.details.id;
    copyObject["notifyAllSubscribers"] = confirm(this.translate.instant('common.NotifyAllSubscribers'));

    return copyObject;
  }

  canDeactivate(): Observable<boolean> | boolean {
    if (this.hasDataChangedService.hasDataChanged) {
      return this.canDeactivateService.canDeacrivateConfirm();
    }

    return true;
  }
}
