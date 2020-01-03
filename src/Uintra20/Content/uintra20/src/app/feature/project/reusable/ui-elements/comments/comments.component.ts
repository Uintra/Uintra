import { Component, OnInit, Input } from '@angular/core';
import { CommentsService } from 'src/app/ui/panels/comments/helpers/comments.service';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.less']
})
export class CommentsComponent implements OnInit {

  @Input()
  comments: any;
  @Input()
  activityType: number;
  description = '';

  get isSubmitDisabled() {
    if (!this.description) {
      return true;
    }

    return false;
  }

  constructor(private commentsService: CommentsService) { }

  ngOnInit() {
    console.log(this.comments);
    console.log(this.activityType);
  }

  onCommentSubmit(replyData?) {
    const data = {
      EntityId: window.location.href.slice(window.location.href.indexOf('id=') + 3),
      EntityType: this.activityType,
      ParentId: replyData ? replyData.parentId : null,
      Text: replyData ? replyData.description : this.description,
    };

    this.commentsService.onCreate(data).then( (res: any) => {
      this.comments.data = res.comments;
      this.description = '';
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
