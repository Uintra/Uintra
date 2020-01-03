import { Component, ViewEncapsulation, OnInit } from '@angular/core';

@Component({
  selector: 'comments-panel',
  templateUrl: './comments-panel.html',
  styleUrls: ['./comments-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class CommentsPanel implements OnInit {

  constructor() {
  }

  ngOnInit(): void {
  }
}
