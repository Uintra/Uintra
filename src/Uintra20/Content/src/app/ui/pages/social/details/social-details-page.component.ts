import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import {
  IDocument,
  ISocialDetails,
  IUserTag,
  IMedia
} from 'src/app/feature/specific/activity/activity.interfaces';
import { ILikeData } from 'src/app/feature/reusable/ui-elements/like-button/like-button.interface';
import { ICommentData } from 'src/app/feature/reusable/ui-elements/comments/comments.component';
import { ImageGalleryService } from 'src/app/feature/reusable/ui-elements/image-gallery/image-gallery.service';
import { IGroupDetailsHeaderData } from 'src/app/feature/specific/groups/groups.interface';
import { TranslateService } from '@ngx-translate/core';
import { ISocialDetailsPage } from 'src/app/shared/interfaces/pages/social/details/social-details-page.interface';

@Component({
  selector: 'social-details',
  templateUrl: './social-details-page.component.html',
  styleUrls: ['./social-details-page.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class SocialDetailsPanelComponent implements OnInit {

  public data: ISocialDetailsPage;
  public details: ISocialDetails;
  public tags: Array<IUserTag>;
  public activityName: string;
  public likeData: ILikeData;
  public medias: Array<IMedia> = new Array<IMedia>();
  public documents: Array<IDocument> = new Array<IDocument>();
  public commentDetails: ICommentData;
  public detailsDescription: SafeHtml;
  public groupHeader: IGroupDetailsHeaderData;
  public isGroupMember: boolean;
  public canView: boolean;

  constructor(
    private activatedRoute: ActivatedRoute,
    private imageGalleryService: ImageGalleryService,
    private sanitizer: DomSanitizer,
    private translateService: TranslateService
  ) {
    this.activatedRoute.data.subscribe((data: ISocialDetailsPage) => this.data = data);
  }

  public ngOnInit(): void {
    if (this.data) {
      this.details = this.data.details;
      this.commentDetails = {
        entityId: this.data.details.id,
        entityType: this.data.details.activityType
      };
      this.canView = !this.data.requiresRedirect;
      this.isGroupMember = this.data.isGroupMember;
      this.groupHeader = this.data.groupHeader;
      this.activityName = this.translateService.instant('socialDetailsTitle.lbl');
      this.tags = this.data.tags;
      this.medias = this.data.details.lightboxPreviewModel.medias;
      this.documents = this.data.details.lightboxPreviewModel.otherFiles;
      this.detailsDescription = this.sanitizer.bypassSecurityTrustHtml(this.details.description);
    }
  }

  public openGallery(i): void {
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

  public trackIndex = (index: number, item) => index;
}
