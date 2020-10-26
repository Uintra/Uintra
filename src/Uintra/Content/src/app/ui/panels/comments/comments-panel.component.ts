import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ICommentsPanel } from 'src/app/shared/interfaces/panels/comments/comments-panel.interface';

@Component({
  selector: 'comments-panel',
  templateUrl: './comments-panel.html',
  styleUrls: ['./comments-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class CommentsPanel implements OnInit {

  public data: ICommentsPanel;

  constructor(private activatedRoute: ActivatedRoute) {
    this.activatedRoute.data.subscribe((data: ICommentsPanel) => this.data = data);
  }

  public ngOnInit(): void { }
}
