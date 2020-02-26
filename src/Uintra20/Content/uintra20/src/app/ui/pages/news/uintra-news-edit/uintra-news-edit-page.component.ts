import { Component, ViewEncapsulation, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import ParseHelper from "src/app/feature/shared/helpers/parse.helper";
import {
  INewsCreateModel,
  IOwner
} from "src/app/feature/project/specific/activity/activity.interfaces";
import { ITagData } from "src/app/feature/project/reusable/inputs/tag-multiselect/tag-multiselect.interface";
import { ActivityService } from "src/app/feature/project/specific/activity/activity.service";
import { ParamsPipe } from "src/app/services/pipes/link/params.pipe";
import { RouterResolverService } from 'src/app/services/general/router-resolver.service';
import { AddButtonService } from 'src/app/ui/main-layout/left-navigation/components/my-links/add-button.service';
import { HasDataChangedService } from 'src/app/services/general/has-data-changed.service';
import { Observable } from 'rxjs';
import { CanDeactivateGuard } from 'src/app/services/general/can-deactivate.service';

@Component({
  selector: "uintra-news-edit-page",
  templateUrl: "./uintra-news-edit-page.html",
  styleUrls: ["./uintra-news-edit-page.less"],
  encapsulation: ViewEncapsulation.None
})
export class UintraNewsEditPage implements OnInit {
  data: any;
  newsData: INewsCreateModel;
  panelData: any;
  details: any;
  members: IOwner[];
  creator: IOwner;
  tags: ITagData[];

  constructor(
    private route: ActivatedRoute,
    private activityService: ActivityService,
    private router: Router,
    private routerResolverService: RouterResolverService,
    private addButtonService: AddButtonService,
    private hasDataChangedService: HasDataChangedService,
    private canDeactivateService: CanDeactivateGuard,
  ) {
    this.route.data.subscribe(data => {
      this.data = data;
      this.addButtonService.setPageId(data.id);
    });
  }

  ngOnInit(): void {
    this.panelData = ParseHelper.parseUbaselineData(this.data);
    this.details = this.panelData.details;
    this.members = (Object.values(this.panelData.members) as Array<any>) || [];
    this.creator = this.details.headerInfo.owner;
    this.tags = Object.values(this.details.availableTags) || [];

    this.newsData = {
      ownerId: this.details.ownerId,
      title: this.details.headerInfo.title,
      description: this.details.description,
      publishDate: this.details.publishDate,

      unpublishDate: this.details.unpublishDate,
      media: {
        medias:
          (Object.values(this.details.lightboxPreviewModel.medias) as Array<
            any
          >) || [],
        otherFiles:
          (Object.values(this.details.lightboxPreviewModel.otherFiles) as Array<
            any
          >) || []
      },
      endPinDate: this.details.endPinDate,

      isPinned: this.details.isPinned,
      location: {
        address: this.details.location.address,
        shortAddress: this.details.location.shortAddress
      },
      tags: (Object.values(this.details.tags) as Array<any>) || []
    };
  }

  onSubmit(data) {
    const copyObject = this.requesModelBuilder(data);

    this.activityService.updateNews(copyObject).subscribe((r: any) => {
      this.routerResolverService.removePageRouter(r.originalUrl);
      this.hasDataChangedService.reset();
      this.router.navigate([r.originalUrl]);
    });
  }

  requesModelBuilder(data) {
    const copyObject = JSON.parse(JSON.stringify(data));

    const otherFilesIds = copyObject.media.otherFiles.map(m => m.id);
    const mediaIds = copyObject.media.medias.map(m => m.id);

    copyObject.media = otherFilesIds.concat(mediaIds).join(',');
    copyObject["id"] = this.details.id;

    return copyObject;
  }

  canDeactivate(): Observable<boolean> | boolean {
    if (this.hasDataChangedService.hasDataChanged) {
      this.canDeactivateService.canDeacrivateConfirm();
    }

    return true;
  }
}
