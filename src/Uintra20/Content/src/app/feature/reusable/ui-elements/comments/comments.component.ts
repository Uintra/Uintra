import { Component, Input } from '@angular/core';
import { CommentsService } from './helpers/comments.service';
import { TranslateService } from '@ngx-translate/core';

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
  @Input() activityType: number;
  @Input() isGroupMember: boolean = true;
  description = '';
  inProgress: boolean;

  get isSubmitDisabled(): boolean {
    const isEmpty = this.isNullOrWhitespace(this.stripHtml(this.description));

    return isEmpty
      ? true
      : false;
  }

  constructor(
    private commentsService: CommentsService,
    private translate: TranslateService) { }

  onCommentSubmit(replyData?) {
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
    if (confirm(this.translate.instant('common.AreYouSure'))) {
      this.commentsService.deleteComment(obj)
      .then((res: any) => {
        this.comments.data = res.comments;
      });
    }
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
