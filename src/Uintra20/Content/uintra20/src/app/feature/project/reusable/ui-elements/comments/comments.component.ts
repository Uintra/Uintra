import { Component, OnInit, Input } from '@angular/core';
import { CommentsService } from 'src/app/ui/panels/comments/helpers/comments.service';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.less']
})
export class CommentsComponent implements OnInit {
  @Input() comments: any;
  @Input() activityType: number;

  description = '';
  disabledSubmit: boolean;

  get isSubmitDisabled() {
    if (!this.description) {
      return true;
    }

    return false;
  }

  constructor(private commentsService: CommentsService) { }

  ngOnInit() {
  }

  onCommentSubmit(replyData?) {
    this.disabledSubmit = true;
    const data = {
      EntityId: window.location.href.slice(window.location.href.indexOf('id=') + 3),
      EntityType: this.activityType,
      ParentId: replyData ? replyData.parentId : null,
      Text: replyData ? replyData.description : this.description,
    };

    this.commentsService.onCreate(data).then( (res: any) => {
      this.comments.data = res.comments;
      this.description = '';
    }).finally(() => {
      this.disabledSubmit = false;
    });
  }

  deleteComment(obj) {
    this.commentsService.deleteComment(obj)
      .then((res: any) => {
        this.comments.data = res.comments;
      });
  }

  editComment(comments) {
    this.comments.data = comments;
  }
}
