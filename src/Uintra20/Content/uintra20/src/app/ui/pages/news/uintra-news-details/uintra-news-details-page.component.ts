import { Component, ViewEncapsulation, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { ILikeData } from "src/app/feature/project/reusable/ui-elements/like-button/like-button.interface";
import { ICommentData } from "src/app/feature/project/reusable/ui-elements/comments/comments.component";
import { SafeHtml, DomSanitizer } from "@angular/platform-browser";
import { ImageGalleryService } from "src/app/feature/project/reusable/ui-elements/image-gallery/image-gallery.service";
import ParseHelper from "src/app/feature/shared/helpers/parse.helper";
import { ISocialDetails, IUserTag, IMedia, IDocument } from 'src/app/feature/project/specific/activity/activity.interfaces';
import { AddButtonService } from 'src/app/ui/main-layout/left-navigation/components/my-links/add-button.service';

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
    private addButtonService: AddButtonService
  ) {
    this.activatedRoute.data.subscribe(data => {
      this.data = data;
      this.addButtonService.setPageId(data.id);
    });
  }

  public ngOnInit(): void {
    this.parsedData = ParseHelper.parseUbaselineData(this.data);
    this.details = this.parsedData.details;
    this.commentDetails = {
      entityId: this.parsedData.details.id,
      entityType: this.parsedData.details.activityType
    };
    this.activityName = ParseHelper.parseActivityType(
      this.details.activityType
    );

    this.tags = Object.values(this.parsedData.tags);
    this.medias = Object.values(this.parsedData.details.lightboxPreviewModel.medias);
    this.documents = Object.values(
      this.parsedData.details.lightboxPreviewModel.otherFiles
    );

    this.likeData = {
      likedByCurrentUser: !!this.parsedData.likedByCurrentUser,
      id: this.parsedData.details.id,
      activityType: this.parsedData.details.activityType,
      likes: Object.values(this.parsedData.likes)
    };

    this.detailsDescription = this.sanitizer.bypassSecurityTrustHtml(
      this.details.description
    );
    this.detailsTitle = this.sanitizer.bypassSecurityTrustHtml(
      this.details.headerInfo.title
    );
  }

  public openGallery(i) {
    const items = this.medias.map(el => ({
      src: el.url,
      w: el.width,
      h: el.height
    }));

    this.imageGalleryService.open(items, i);
  }
}
