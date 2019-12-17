import { Component, Input } from '@angular/core';
import { PublicationsService, IAddLikeRequest } from '../helpers/publications.service';

@Component({
  selector: 'app-central-feed-publication',
  templateUrl: './central-feed-publication.component.html',
  styleUrls: ['./central-feed-publication.component.less']
})
export class CentralFeedPublicationComponent {
  @Input() publication;

  get commentsCount() {
    return this.publication.activity.comments.length || 'Comment';
  }
  get likesCount() {
    return this.publication.activity.likes.length || 'Like';
  }

  constructor(private publicationsService: PublicationsService) {}

  getPublicationDate() {
    return this.publication.activity.dates.length ? this.publication.activity.dates[0] : '';
  }

  onClickLike() {
    const data: IAddLikeRequest = {
      entityId: this.publication.activity.id,
      entityType: this.publication.activity.type,
    };
    this.publicationsService.addLike(data);
  }
}
