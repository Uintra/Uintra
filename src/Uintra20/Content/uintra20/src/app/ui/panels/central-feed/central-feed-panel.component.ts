import { Component, ViewEncapsulation } from '@angular/core';
import { ICentralFeedPanel } from './central-feed-panel.interface';

@Component({
  selector: 'central-feed-panel',
  templateUrl: './central-feed-panel.html',
  styleUrls: ['./central-feed-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class CentralFeedPanel {
  data: ICentralFeedPanel;
}