import { Component, Input } from '@angular/core';
import { CommentsService } from 'src/app/ui/panels/comments/helpers/comments.service';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.less']
})
export class CommentsComponent {
  @Input() comments: any;
  @Input() activityType: number;

  description = '';
  inProgress: boolean;

  get isSubmitDisabled(): boolean {
    const isEmpty = this.isNullOrWhitespace(this.stripHtml(this.description));

    return isEmpty
      ? true
      : false;
  }

  constructor(private commentsService: CommentsService) { }

  onCommentSubmit(replyData?) {
    this.inProgress = true;
    const data = {
      EntityId: window.location.href.slice(window.location.href.indexOf('id=') + 3),
      EntityType: this.activityType,
      ParentId: replyData ? replyData.parentId : null,
      Text: replyData ? replyData.description : this.description,
    };

    this.commentsService.onCreate(data).then((res: any) => {
      this.comments.data = res.comments;
      this.description = '';
    }).finally(() => {
      this.inProgress = false;
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

  stripHtml(html: string): string {
    const stripped = html.replace(/<[^>]*>?/gm, '');

    return stripped;
  }

  isNullOrWhitespace(value: string): boolean {
    if (!value) {
      return true;
    }

    return value.replace(/\s/g, '').length < 1;
  }
}
