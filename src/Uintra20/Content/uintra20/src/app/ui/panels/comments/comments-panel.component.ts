import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ICommentData } from 'src/app/feature/project/reusable/ui-elements/comments/comments.component';
import { ActivatedRoute } from '@angular/router';
import ParseHelper from 'src/app/feature/shared/helpers/parse.helper';

@Component({
  selector: 'comments-panel',
  templateUrl: './comments-panel.html',
  styleUrls: ['./comments-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class CommentsPanel implements OnInit {
  data: any;
  comments: any;
  commentDetails: ICommentData;
  activityType: number;

  constructor(private activatedRoute: ActivatedRoute) {
    this.activatedRoute.data.subscribe(data => this.data = data);
  }

  ngOnInit(): void {
    const parsedData = ParseHelper.parseUbaselineData(this.data);
    this.activityType = parsedData.activityId;
    this.commentDetails = {
      entityId: parsedData.entityId,
      entityType: parsedData.activityId
    };
  }
}
