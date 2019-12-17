import { Component, Input, OnInit } from '@angular/core';
import { PublicationsService, IAddLikeRequest } from '../helpers/publications.service';

@Component({
  selector: 'app-central-feed-publication',
  templateUrl: './central-feed-publication.component.html',
  styleUrls: ['./central-feed-publication.component.less']
})
export class CentralFeedPublicationComponent implements OnInit {
  @Input() publication;

  get commentsCount() {
    return this.publication.activity.comments.length || 'Comment';
  }
  get likesCount() {
    return this.newLikesCount || 'Like';
  }

  newLikesCount: number = null;

  constructor(private publicationsService: PublicationsService) { }

  ngOnInit(): void {
    this.newLikesCount = this.publication.activity.likes.length;
  }

  getPublicationDate() {
    return this.publication.activity.dates.length ? this.publication.activity.dates[0] : '';
  }

  onClickLike() {
    const canAddLike = this.publication.activity.likedByCurrentUser === false;

    const data: IAddLikeRequest = {
      entityId: this.publication.activity.id,
      entityType: this.publication.activity.activityType,
    };

    if (canAddLike) {
      this.publicationsService.addLike(data).then(response => {
        // this.newLikesCount = response['count'];
      });
      this.newLikesCount += 1;
      this.publication.activity.likedByCurrentUser = true;
    } else {
      this.publicationsService.removeLike(data).then(response => {
        // this.newLikesCount = response['count'];
      });
      this.newLikesCount -= 1;
      this.publication.activity.likedByCurrentUser = false;
    }
  }
}
