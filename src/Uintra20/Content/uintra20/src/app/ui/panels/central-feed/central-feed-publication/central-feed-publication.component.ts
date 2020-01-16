import { Component, Input, OnInit } from '@angular/core';
import { ILikeData } from '../../../../feature/project/reusable/ui-elements/like-button/like-button.interface';
import { Router} from '@angular/router';
import { ImageGalleryService } from 'src/app/feature/project/reusable/ui-elements/image-gallery/image-gallery.service';
import { IMedia, IDocument } from 'src/app/ui/pages/social-details/social-details.interface';

@Component({
  selector: 'app-central-feed-publication',
  templateUrl: './central-feed-publication.component.html',
  styleUrls: ['./central-feed-publication.component.less']
})
export class CentralFeedPublicationComponent implements OnInit {
  @Input() publication;
  mediaCount: any;
  documentsCount: any;
  additionalImages: number;

  medias: Array<IMedia> = new Array<IMedia>();
  documents: Array<IDocument> = new Array<IDocument>();

  get commentsCount() {
    return this.publication.activity.commentsCount || 'Comment';
  }

  get detailsParams() {
    return {
      ...this.publication.activity.links.details.params
        .map(param => ({ [param.name]: param.value }))
    };
  }

  likeData: ILikeData;

  constructor(private imageGalleryService: ImageGalleryService) { }

  ngOnInit(): void {
    this.medias = Object.values(this.publication.activity.mediaPreview.medias);
    this.mediaCount = this.medias.length;
    this.documents = Object.values(this.publication.activity.mediaPreview.otherFiles);
    this.documentsCount = this.documents.length;
    this.additionalImages = this.publication.activity.mediaPreview.additionalImages;
    this.likeData = {
      likedByCurrentUser: this.publication.activity.likedByCurrentUser,
      id: this.publication.activity.id,
      activityType: this.publication.activity.activityType,
      likes: this.publication.activity.likes
    };
  }

  public openGallery(i) {
    const items = this.mediaCount.map(el => ({
      src: el.url,
      w: el.width,
      h: el.height,
    }));

    this.imageGalleryService.open(items, i);
  }


  getPublicationDate() {
    return this.publication.activity.dates.length
      ? this.publication.activity.dates[0]
      : '';
  }
}
