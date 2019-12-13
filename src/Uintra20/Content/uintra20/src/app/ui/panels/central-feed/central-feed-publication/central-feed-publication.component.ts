import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-central-feed-publication',
  templateUrl: './central-feed-publication.component.html',
  styleUrls: ['./central-feed-publication.component.less']
})
export class CentralFeedPublicationComponent {
  @Input() publication;

  get commentsCount() {
    return this.publication.activity.comments.length || 0;
  }
  get likesCount() {
    return this.publication.activity.likes.length || 0;
  }

  constructor() { }

  getPublicationDate() {
    return this.publication.activity.dates.length ? this.publication.activity.dates[0] : '';
  }
}
