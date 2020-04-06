import { Component, ViewEncapsulation, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { SafeHtml, DomSanitizer } from "@angular/platform-browser";
import ParseHelper from "src/app/shared/utils/parse.helper";
import { AddButtonService } from 'src/app/ui/main-layout/left-navigation/components/my-links/add-button.service';
import { ISocialDetails, IUserTag, IMedia, IDocument } from 'src/app/feature/specific/activity/activity.interfaces';
import { ICommentData } from 'src/app/feature/reusable/ui-elements/comments/comments.component';
import { ImageGalleryService } from 'src/app/feature/reusable/ui-elements/image-gallery/image-gallery.service';
import { ILikeData } from 'src/app/feature/reusable/ui-elements/like-button/like-button.interface';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: "uintra-news-details-page",
  templateUrl: "./uintra-news-details-page.html",
  styleUrls: ["./uintra-news-details-page.less"],
  encapsulation: ViewEncapsulation.None
})
export class UintraNewsDetailsPage implements OnInit {
  parsedData: any;
  data: any;
  details: ISocialDetails;
  tags: Array<IUserTag>;
  activityName: string;
  likeData: ILikeData;
  medias: Array<IMedia> = new Array<IMedia>();
  documents: Array<IDocument> = new Array<IDocument>();
  commentDetails: ICommentData;
  detailsDescription: SafeHtml;
  detailsTitle: SafeHtml;
  constructor(
    private activatedRoute: ActivatedRoute,
    private imageGalleryService: ImageGalleryService,
    private sanitizer: DomSanitizer,
    private addButtonService: AddButtonService,
    private router: Router,
    private translateService: TranslateService
  ) {
    this.activatedRoute.data.subscribe(data => {
      if (!data.requiresRedirect.get()) {
        this.data = data;
        this.addButtonService.setPageId(data.id);
      } else {
        this.router.navigate([data.errorLink.get().originalUrl.get()]);
      }
    });
  }

  public ngOnInit(): void {
    if (this.data) {
      this.parsedData = ParseHelper.parseUbaselineData(this.data);
      console.log(this.parsedData);
      this.details = this.parsedData.details;
      this.commentDetails = {
        entityId: this.parsedData.details.id,
        entityType: this.parsedData.details.activityType
      };
      this.activityName = this.translateService.instant('newsDetails.Title');
      this.tags = Object.values(this.parsedData.tags);
      this.medias = Object.values(this.parsedData.details.lightboxPreviewModel.medias);
      this.documents = Object.values(
        this.parsedData.details.lightboxPreviewModel.otherFiles
      );

      this.detailsDescription = this.sanitizer.bypassSecurityTrustHtml(
        this.details.description
      );
      this.detailsTitle = this.sanitizer.bypassSecurityTrustHtml(
        this.details.headerInfo.title
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
