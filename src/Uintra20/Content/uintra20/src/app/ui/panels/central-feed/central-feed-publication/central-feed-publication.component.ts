import { Component, Input, OnInit } from '@angular/core';
import { ILikeData } from '../../../../feature/project/reusable/ui-elements/like-button/like-button.interface';
import { Router} from '@angular/router';

@Component({
  selector: 'app-central-feed-publication',
  templateUrl: './central-feed-publication.component.html',
  styleUrls: ['./central-feed-publication.component.less']
})
export class CentralFeedPublicationComponent implements OnInit {

  @Input() publication;

  get commentsCount() {
    return this.publication.activity.commentsCount || 'Comment';
  }

  likeData: ILikeData;

  constructor(private router: Router) {
  }

  ngOnInit(): void {
    this.likeData = {
      likedByCurrentUser: this.publication.activity.likedByCurrentUser,
      id: this.publication.activity.id,
      activityType: this.publication.activity.activityType,
      likes: this.publication.activity.likes
    };
  }

  getPublicationDate() {
    return this.publication.activity.dates.length
      ? this.publication.activity.dates[0]
      : '';
  }
}
