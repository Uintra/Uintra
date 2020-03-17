import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ActivityService } from 'src/app/feature/specific/activity/activity.service';
import ParseHelper from 'src/app/shared/utils/parse.helper';
import { IULink } from 'src/app/shared/interfaces/general.interface';
import { TranslateService } from '@ngx-translate/core';

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
  ) {
    this.route.data.subscribe(data => {
      if (!data.requiresRedirect.get()) {
        this.data = data;
        this.parsedData = ParseHelper.parseUbaselineData(this.data);
        this.parsedData.details.members = Object.values(this.parsedData.members);
        this.parsedData.details.availableTags = Object.values(this.parsedData.details.availableTags);
        this.parsedData.details.selectedTags = Object.values(this.parsedData.details.tags);
        this.parsedData.details.title = this.parsedData.details.headerInfo.title;
        this.parsedData.details.creator = {id: this.parsedData.details.creatorId};
        this.parsedData.details.pinAllowed = this.parsedData.pinAllowed;
      } else {
        this.router.navigate([data.errorLink.get().originalUrl.get()]);
      }
    });
  }

  onSubmit(data) {
    this.activityService.updateEvent(data).subscribe((res: IULink) => {
      this.router.navigate([res.originalUrl]);
    })
  }

  onCancel() {
    this.router.navigate([this.parsedData.data.links.feed.originalUrl]);
  }

  onHide() {
    if (confirm(this.translate.instant('common.AreYouSure'))) {
      const isNotificationNeeded = confirm(this.translate.instant('common.NotifyAllSubscribers'));
      this.activityService.hideEvent(this.parsedData.details.id, isNotificationNeeded).subscribe(res => {
        console.log(res);
      })
    }
  }
}
