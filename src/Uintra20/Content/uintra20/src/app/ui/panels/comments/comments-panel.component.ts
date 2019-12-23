import { Component, ViewEncapsulation } from '@angular/core';
import { ICommentsPanel } from './comments-panel.interface';

@Component({
  selector: 'comments-panel',
  templateUrl: './comments-panel.html',
  styleUrls: ['./comments-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class CommentsPanel {
  data: ICommentsPanel;
}