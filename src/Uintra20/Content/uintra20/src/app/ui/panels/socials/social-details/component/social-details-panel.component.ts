import { Component, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'social-details',
  templateUrl: './social-details-panel.html',
  styleUrls: ['./social-details-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class SocialDetailsPanelComponent implements OnInit {

  constructor() { }

  public ngOnInit(): void {
    console.log('Social Detail Works');
  }
}
