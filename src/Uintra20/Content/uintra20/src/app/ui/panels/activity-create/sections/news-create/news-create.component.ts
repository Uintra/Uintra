import { Component, OnInit, Input } from "@angular/core";
import { IActivityCreatePanel } from "../../activity-create-panel.interface";
import ParseHelper from "src/app/feature/shared/helpers/parse.helper";
import { Router } from "@angular/router";
import { ActivityService } from 'src/app/feature/project/specific/activity/activity.service';
import { INewsCreateModel } from 'src/app/feature/project/specific/activity/activity.interfaces';
import { RouterResolverService } from 'src/app/services/general/router-resolver.service';

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

  panelData;

  constructor(
      private activityService: ActivityService,
    private router: Router,
    private routerResolverService: RouterResolverService
  ) {}

  ngOnInit() {
    this.panelData = ParseHelper.parseUbaselineData(this.data);
    this.members = (Object.values(this.panelData.members) as Array<any>) || [];
    this.creator = this.panelData.creator;
    this.tags = Object.values(this.panelData.tags.userTagCollection);

    this.newsData = {
      ownerId: this.creator.id,
      title: null,
      description: null,
      publishDate: null,
    };
  }

  onSubmit(data) {
    if (this.panelData.groupId) {data.groupId = this.panelData.groupId}
    this.activityService
    .submitNewsContent(data)
    .subscribe((r: any) => {
      this.routerResolverService.removePageRouter(r.originalUrl);
      this.router.navigate([r.originalUrl]);
    });
  }

  onCancel() {
    this.router.navigate([this.panelData.links.feed.originalUrl]);
  }
}
