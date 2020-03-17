import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ActivityService } from 'src/app/feature/specific/activity/activity.service';
import ParseHelper from 'src/app/shared/utils/parse.helper';
import { IULink } from 'src/app/shared/interfaces/general.interface';

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
  ) {
    this.route.data.subscribe(data => {
      if (!data.requiresRedirect.get()) {
        this.data = data;
        this.parsedData = ParseHelper.parseUbaselineData(this.data);
        this.parsedData.data.members = Object.values(this.parsedData.data.members);
        this.parsedData.data.tags = Object.values(this.parsedData.data.tags);
      } else {
        this.router.navigate([data.errorLink.get().originalUrl.get()]);
      }
    });
  }

  onSubmit(data) {
    this.activityService.createEvent(data).subscribe((res: IULink) => {
      this.router.navigate([res.originalUrl]);
    })
  }

  onCancel() {
    this.router.navigate([this.parsedData.data.links.feed.originalUrl]);
  }

  onHide() {

  }
}
