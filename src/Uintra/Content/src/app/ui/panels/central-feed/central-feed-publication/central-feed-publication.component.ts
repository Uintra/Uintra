import { Component, Input, OnInit, HostListener } from '@angular/core';
import { Router } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { MqService } from 'src/app/shared/services/general/mq.service';
import { IMedia, IDocument } from 'src/app/feature/specific/activity/activity.interfaces';
import { ILikeData } from 'src/app/feature/reusable/ui-elements/like-button/like-button.interface';
import { ImageGalleryService } from 'src/app/feature/reusable/ui-elements/image-gallery/image-gallery.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-central-feed-publication',
  templateUrl: './central-feed-publication.component.html',
  styleUrls: ['./central-feed-publication.component.less']
})
export class CentralFeedPublicationComponent implements OnInit {

  @Input()
  public publication;
  public deviceWidth: number;
  public documentsCount: any;
  public additionalImages: number;
  public countToDisplay: number;
  public medias: Array<IMedia> = new Array<IMedia>();
  public documents: Array<IDocument> = new Array<IDocument>();
  public likeData: ILikeData;
  public commentLinkPlaceholder: string;

  constructor(
    private imageGalleryService: ImageGalleryService,
    private router: Router,
    private sanitizer: DomSanitizer,
    private mq: MqService,
    private translate: TranslateService,
  ) { }

  public ngOnInit(): void {
    this.deviceWidth = window.innerWidth;
    this.publication.description = this.sanitizer.bypassSecurityTrustHtml(this.publication.description);
    this.medias = this.publication.mediaPreview.medias;
    this.countToDisplay =
      this.medias.length > 2
        ? this.getItemsCountToDisplay()
        : this.medias.length;
    this.additionalImages = this.medias.length - this.countToDisplay;
    this.documents = this.publication.mediaPreview.otherFiles;
    this.documentsCount = this.documents.length;
    this.likeData = {
      likedByCurrentUser: this.publication.likedByCurrentUser,
      id: this.publication.id,
      activityType: this.publication.activityType,
      likes: this.publication.likes
    };
    this.commentLinkPlaceholder = this.translate.instant('activity.Comment.lnk');
  }

  public get commentsCount() {
    return this.publication.commentsCount || this.commentLinkPlaceholder;
  }

  @HostListener('window:resize', ['$event'])
  public getScreenSize(event?) {
    this.deviceWidth = window.innerWidth;
    this.countToDisplay =
      this.medias.length > 2
        ? this.getItemsCountToDisplay()
        : this.medias.length;
    this.additionalImages = this.medias.length - this.countToDisplay;
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

  public getPublicationDate() {
    if (!this.publication.dates) {
      return '';
    }
    return this.publication.dates.join(' - ');
  }

  public checkForRightRoute(e) {
    if (!e.target.href) {
      this.router.navigate(['/social-details'], {
        queryParams: { id: this.publication.id }
      });
    }
  }

  public getItemsCountToDisplay() {
    if (!this.mq.isTablet(this.deviceWidth)) {
      return 2;
    }

    return 3;
  }

  public getDocumentsText() {
    return this.documentsCount > 1
      ? this.translate.instant('lightboxGallery.Count.Many.lbl')
      : this.translate.instant('lightboxGallery.Count.One.lbl');
  }
}
