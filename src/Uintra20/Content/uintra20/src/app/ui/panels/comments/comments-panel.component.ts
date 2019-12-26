import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ICommentsPanel } from './comments-panel.interface';

@Component({
  selector: 'comments-panel',
  templateUrl: './comments-panel.html',
  styleUrls: ['./comments-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class CommentsPanel implements OnInit {

  data: ICommentsPanel;
  description: string = "";

  constructor() {
  }

  ngOnInit(): void {
    console.log(this.data);
  }

  onCommentSubmit() {
    
  }
}
