import { Component, OnInit, Input } from "@angular/core";
import ParseHelper from "src/app/shared/utils/parse.helper";
import { Router } from "@angular/router";
import { RouterResolverService } from "src/app/shared/services/general/router-resolver.service";
import { HasDataChangedService } from "src/app/shared/services/general/has-data-changed.service";
import { ActivityService } from "src/app/feature/specific/activity/activity.service";
import { INewsCreateModel, IActivityCreatePanel } from "src/app/feature/specific/activity/activity.interfaces";

@Component({
  selector: "app-news-create",
  templateUrl: "./news-create.component.html",
  styleUrls: ["./news-create.component.less"]
})
export class NewsCreateComponent implements OnInit {
  @Input() data: IActivityCreatePanel;
  newsData: INewsCreateModel;
  members: Array<any>;
  creator: any;
  tags: Array<any>;
  isSubmitLoading: boolean = false;

  panelData;

  constructor(
    private activityService: ActivityService,
    private router: Router,
    private routerResolverService: RouterResolverService,
    private hasDataChangedService: HasDataChangedService
  ) {}

  ngOnInit() {
    this.panelData = ParseHelper.parseUbaselineData(this.data);
    this.members = (Object.values(this.panelData.members) as Array<any>) || [];
    this.creator = this.panelData.creator;
    this.tags = Object.values(this.panelData.tags);

    this.newsData = {
      ownerId: this.creator.id,
      title: null,
      description: null,
      publishDate: null
    };
  }

  onSubmit(data) {
    if (this.panelData.groupId) {
      data.groupId = this.panelData.groupId;
    }

    this.isSubmitLoading = true;

    this.activityService.submitNewsContent(data).subscribe(
      (r: any) => {
        this.routerResolverService.removePageRouter(r.originalUrl);
        this.hasDataChangedService.reset();
        this.router.navigate([r.originalUrl]);
      },
      err => {
        this.isSubmitLoading = false;
      }
    );
  }

  onCancel() {
    this.hasDataChangedService.reset();
    this.router.navigate([this.panelData.links.feed.originalUrl]);
  }
}
