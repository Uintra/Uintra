import { Component, OnInit, Input } from '@angular/core';
import { ISocialDetails } from '../../social-details.interface';

@Component({
  selector: 'app-post-header',
  templateUrl: './post-header.component.html',
  styleUrls: ['./post-header.component.less']
})
export class PostHeaderComponent implements OnInit {
  @Input() details: ISocialDetails;
  @Input() activityName: string;

  constructor() { }

  ngOnInit() {
  }
}
