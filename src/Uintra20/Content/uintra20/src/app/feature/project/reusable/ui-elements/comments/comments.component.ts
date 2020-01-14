import { Component, Input } from '@angular/core';
import { CommentsService } from 'src/app/ui/panels/comments/helpers/comments.service';

export interface ICommentData {
  entityType: number;
  entityId: string;
}

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.less']
})
export class CommentsComponent {
  @Input() comments: any;
  @Input() commentDetails: ICommentData;
  activityType: number;
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
    this.activityType = this.commentDetails.entityType;
    this.inProgress = true;
    const data = {
      entityId: this.commentDetails.entityId,
      entityType: this.activityType,
      parentId: replyData ? replyData.parentId : null,
      text: replyData ? replyData.description : this.description,
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
    if (!html) {
      return '';
    }

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
