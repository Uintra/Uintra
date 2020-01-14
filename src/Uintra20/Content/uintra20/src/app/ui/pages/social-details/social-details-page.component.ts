import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ISocialDetails, IUserTag, IMedia, IDocument } from './social-details.interface';
import { ILikeData } from 'src/app/feature/project/reusable/ui-elements/like-button/like-button.interface';
import { ImageGalleryService } from 'src/app/feature/project/reusable/ui-elements/image-gallery/image-gallery.service';
import ParseHelper from 'src/app/feature/shared/helpers/parse.helper';

@Component({
  selector: 'social-details',
  templateUrl: './social-details-page.component.html',
  styleUrls: ['./social-details-page.component.less'],
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

  constructor(
    private activatedRoute: ActivatedRoute,
    private imageGalleryService: ImageGalleryService
  ) {
    this.activatedRoute.data.subscribe(data => this.data = data);
  }

  public ngOnInit(): void {
    const parsedData = ParseHelper.parseUbaselineData(this.data);
    this.details = parsedData.details;
    this.activityName = ParseHelper.parseActivityType(this.details.activityType);
    this.tags = Object.values(parsedData.tags);
    this.medias = Object.values(parsedData.details.lightboxPreviewModel.medias);
    this.documents = Object.values(parsedData.details.lightboxPreviewModel.otherFiles);
    this.likeData = {
      likedByCurrentUser: parsedData.likedByCurrentUser,
      id: parsedData.details.id,
      activityType: parsedData.details.activityType,
      likes: Object.values(parsedData.likes)
    };
  }

  public openGallery(i) {
    const items = this.medias.map(el => ({
      src: el.url,
      w: el.width,
      h: el.height,
    }));

    this.imageGalleryService.open(items, i);
  }
}
