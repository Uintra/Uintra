import { Component, ViewEncapsulation } from '@angular/core';
import { ILikesPanel } from './likes-panel.interface';

@Component({
  selector: 'likes-panel',
  templateUrl: './likes-panel.html',
  styleUrls: ['./likes-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class LikesPanel {
  data: ILikesPanel;
}