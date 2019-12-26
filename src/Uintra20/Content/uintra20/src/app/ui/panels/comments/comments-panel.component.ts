import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ICommentsPanel } from './comments-panel.interface';
import { CommentsService } from './helpers/comments.service';

@Component({
  selector: 'comments-panel',
  templateUrl: './comments-panel.html',
  styleUrls: ['./comments-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class CommentsPanel implements OnInit {
  data: ICommentsPanel;
  description: string = "";

  constructor(private cs: CommentsService) {
  }

  ngOnInit(): void {
    console.log(this.data);
  }

  onCommentSubmit() {
    const data = {
      EntityId: window.location.href.slice(window.location.href.indexOf('id=') + 3),
      EntityType: this.data.activityType,
      ParentId: null,
      Text: this.description,
    }

    this.cs.onCreate(data).then( (res: any) => {
      this.data.comments.data = res.comments;
      this.description = '';
    });
  }

  deleteComment(obj) {
    this.cs.deleteComment(obj)
      .then((res: any) => {
        this.data.comments.data = res.comments.filter(comment => comment.id !== obj.commentId);
      });
  }
}
