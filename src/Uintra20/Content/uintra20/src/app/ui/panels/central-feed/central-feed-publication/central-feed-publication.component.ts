import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-central-feed-publication',
  templateUrl: './central-feed-publication.component.html',
  styleUrls: ['./central-feed-publication.component.less']
})
export class CentralFeedPublicationComponent {
  @Input() publication;

  constructor() { }
}
