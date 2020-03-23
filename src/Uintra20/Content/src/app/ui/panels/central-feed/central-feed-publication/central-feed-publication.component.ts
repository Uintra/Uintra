import { Component, Input, OnInit, HostListener } from "@angular/core";
import { Router } from "@angular/router";
import { DomSanitizer } from "@angular/platform-browser";
import { MqService } from "src/app/shared/services/general/mq.service";
import {
  IMedia,
  IDocument
} from "src/app/feature/specific/activity/activity.interfaces";
import { ILikeData } from "src/app/feature/reusable/ui-elements/like-button/like-button.interface";
import { ImageGalleryService } from "src/app/feature/reusable/ui-elements/image-gallery/image-gallery.service";
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: "app-central-feed-publication",
  templateUrl: "./central-feed-publication.component.html",
  styleUrls: ["./central-feed-publication.component.less"]
})
export class CentralFeedPublicationComponent implements OnInit {
  @Input() publication;
  deviceWidth: number;
  documentsCount: any;
  additionalImages: number;
  countToDisplay: number;
  medias: Array<IMedia> = new Array<IMedia>();
  documents: Array<IDocument> = new Array<IDocument>();
  likeData: ILikeData;
  commentLinkPlaceholder: string;

  constructor(
    private imageGalleryService: ImageGalleryService,
    private router: Router,
    private sanitizer: DomSanitizer,
    private mq: MqService,
    private translate: TranslateService,
  ) { }

  ngOnInit(): void {
    this.deviceWidth = window.innerWidth;
    this.publication.description = this.sanitizer.bypassSecurityTrustHtml(
      this.publication.description
    );
    this.medias = Object.values(this.publication.mediaPreview.medias);
    this.countToDisplay =
      this.medias.length > 2
        ? this.getItemsCountToDisplay()
        : this.medias.length;
    this.additionalImages = this.medias.length - this.countToDisplay;
    this.documents = Object.values(
      this.publication.mediaPreview.otherFiles
    );
    this.documentsCount = this.documents.length;
    this.likeData = {
      likedByCurrentUser: this.publication.likedByCurrentUser,
      id: this.publication.id,
      activityType: this.publication.activityType,
      likes: this.publication.likes
    };
    this.commentLinkPlaceholder = this.translate.instant('activity.Comment.lnk');
  }

  get commentsCount() {
    return this.publication.commentsCount || this.commentLinkPlaceholder;
  }

  @HostListener("window:resize", ["$event"])
  getScreenSize(event?) {
    this.deviceWidth = window.innerWidth;
    this.countToDisplay =
      this.medias.length > 2
        ? this.getItemsCountToDisplay()
        : this.medias.length;
    this.additionalImages = this.medias.length - this.countToDisplay;
  }

  public openGallery(i) {
    const items = this.medias.map(el => ({
      src: el.url,
      w: el.width,
      h: el.height
    }));

    this.imageGalleryService.open(items, i);
  }

  getPublicationDate() {
    if (!this.publication.dates) {
      return "";
    }
    return this.publication.dates.join(" - ");
  }

  checkForRightRoute(e) {
    if (!e.target.href) {
      this.router.navigate(["/social-details"], {
        queryParams: { id: this.publication.id }
      });
    }
  }

  getItemsCountToDisplay() {
    if (!this.mq.isTablet(this.deviceWidth)) {
      return 2;
    }

    return 3;
  }

  getDocumentsText() {
    return this.documentsCount > 1
      ? this.translate.instant('lightboxGallery.Count.Many.lbl')
      : this.translate.instant('lightboxGallery.Count.One.lbl');
  }
}
