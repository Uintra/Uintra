import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SafeHtml, DomSanitizer } from '@angular/platform-browser';
import { ISocialDetails, IUserTag, IMedia, IDocument } from 'src/app/feature/specific/activity/activity.interfaces';
import { ImageGalleryService } from 'src/app/feature/reusable/ui-elements/image-gallery/image-gallery.service';
import { ILikeData } from 'src/app/feature/reusable/ui-elements/like-button/like-button.interface';
import { TranslateService } from '@ngx-translate/core';
import { IUintraNewsDetailsPage } from 'src/app/shared/interfaces/pages/news/details/uintra-news-details-page.interface';
import { ICommentData } from 'src/app/shared/interfaces/panels/comments/comments-panel.interface';

@Component({
  selector: 'uintra-news-details-page',
  templateUrl: './uintra-news-details-page.html',
  styleUrls: ['./uintra-news-details-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class UintraNewsDetailsPage implements OnInit {

  data: IUintraNewsDetailsPage;
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
    private translateService: TranslateService
  ) {
    this.activatedRoute.data.subscribe((data: IUintraNewsDetailsPage) => this.data = data);
  }

  public ngOnInit(): void {
    if (this.data) {
      this.details = this.data.details;
      this.commentDetails = {
        entityId: this.data.details.id,
        entityType: this.data.details.activityType
      };
      this.activityName = this.translateService.instant('newsDetails.Title');
      this.tags = this.data.tags;
      this.medias = this.data.details.lightboxPreviewModel.medias;
      this.documents = this.data.details.lightboxPreviewModel.otherFiles;
      this.detailsDescription = this.sanitizer.bypassSecurityTrustHtml(this.details.description);
      this.detailsTitle = this.sanitizer.bypassSecurityTrustHtml(this.details.headerInfo.title);
    }
  }

  public openGallery(i) {
    const items = this.medias.map(el => {
      if (el.extension === 'mp4') {
        return {
          html: `<div class='gallery__video'>
                  <div class='pswp__video-box'>
                    <video class='pswp__video' src='${el.url}' controls=''></video>
                  <\div>
                <\div>`,
          w: el.width,
          h: el.height
        };
      } else {
        return {
          src: el.url,
          w: el.width,
          h: el.height
        };
      }
    });

    this.imageGalleryService.open(items, i);
  }
}
