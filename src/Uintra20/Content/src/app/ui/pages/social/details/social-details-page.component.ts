import { Component, OnInit, ViewEncapsulation } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";

import ParseHelper from "src/app/shared/utils/parse.helper";
import { DomSanitizer, SafeHtml } from "@angular/platform-browser";
import {
  IDocument,
  ISocialDetails,
  IUserTag,
  IMedia
} from "src/app/feature/specific/activity/activity.interfaces";
import { ILikeData } from "src/app/feature/reusable/ui-elements/like-button/like-button.interface";
import { ICommentData } from "src/app/feature/reusable/ui-elements/comments/comments.component";
import { ImageGalleryService } from "src/app/feature/reusable/ui-elements/image-gallery/image-gallery.service";
import { IGroupDetailsHeaderData } from 'src/app/feature/specific/groups/groups.interface';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: "social-details",
  templateUrl: "./social-details-page.component.html",
  styleUrls: ["./social-details-page.component.less"],
  encapsulation: ViewEncapsulation.None
})
export class SocialDetailsPanelComponent implements OnInit {
  data: any;
  details: ISocialDetails;
  tags: Array<IUserTag>;
  activityName: string;
  likeData: ILikeData;
  medias: Array<IMedia> = new Array<IMedia>();
  documents: Array<IDocument> = new Array<IDocument>();
  commentDetails: ICommentData;
  detailsDescription: SafeHtml;
  groupHeader: IGroupDetailsHeaderData;
  isGroupMember: boolean;
  canView: boolean;

  constructor(
    private activatedRoute: ActivatedRoute,
    private imageGalleryService: ImageGalleryService,
    private sanitizer: DomSanitizer,
    private router: Router,
    private translateService: TranslateService
  ) {
    this.activatedRoute.data.subscribe(data => {
      if (!data.requiresRedirect.get()) {
        this.data = data;
      } else {
        this.router.navigate([data.errorLink.get().originalUrl.get()]);
      }
    });
  }

  public ngOnInit(): void {
    if (this.data) {
      const parsedData = ParseHelper.parseUbaselineData(this.data);
      this.details = parsedData.details;
      this.commentDetails = {
        entityId: parsedData.details.id,
        entityType: parsedData.details.activityType
      };
      this.canView = !parsedData.requiresRedirect;
      this.isGroupMember = parsedData.isGroupMember;
      this.groupHeader = parsedData.groupHeader;
      this.activityName = this.translateService.instant('socialDetailsTitle.lbl');
      this.tags = Object.values(parsedData.tags);
      this.medias = Object.values(parsedData.details.lightboxPreviewModel.medias);
      this.documents = Object.values(
        parsedData.details.lightboxPreviewModel.otherFiles
      );

      this.likeData = {
        likedByCurrentUser: !!parsedData.likedByCurrentUser,
        id: parsedData.details.id,
        activityType: parsedData.details.activityType,
        likes: parsedData.likes ? Object.values(parsedData.likes) : []
      };
      this.detailsDescription = this.sanitizer.bypassSecurityTrustHtml(
        this.details.description
      );
    }
  }

  public openGallery(i) {
    const items = this.medias.map(el => {
      if (el.extension == 'mp4') {
        return {
          html: `<div class="gallery__video">
                  <div class="pswp__video-box">
                    <video class="pswp__video" src="${el.url}" controls=""></video>
                  <\div>
                <\div>`,
          w: el.width,
          h: el.height
        }
      } else {
        return {
          src: el.url,
          w: el.width,
          h: el.height
        }
      }
    });

    this.imageGalleryService.open(items, i);
  }
}
