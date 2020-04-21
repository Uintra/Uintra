import { Component, Input } from '@angular/core';
import { CommentsService } from './helpers/comments.service';
import { TranslateService } from '@ngx-translate/core';
import { RTEStripHTMLService } from 'src/app/feature/specific/activity/rich-text-editor/helpers/rte-strip-html.service';
import { RichTextEditorService } from '../../inputs/rich-text-editor/rich-text-editor.service';

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
  @Input() commentsActivity: number;
  @Input() isGroupMember: boolean = true;
  description = '';
  inProgress: boolean;
  isReplyInProgress: boolean;
  linkPreviewId: number;

  get isSubmitDisabled(): boolean {
    const isEmpty = this.stripHTML.isEmpty(this.description);

    return isEmpty
      ? true
      : false;
  }

  constructor(
    private commentsService: CommentsService,
    private stripHTML: RTEStripHTMLService,
    private translate: TranslateService,
    private RTEService: RichTextEditorService,
  ) { }

  onCommentSubmit(replyData?) {
    if (replyData) {this.isReplyInProgress = true}
    this.inProgress = true;
    const data = {
      entityId: this.commentDetails.entityId,
      entityType: this.activityType,
      parentId: replyData ? replyData.parentId : null,
      text: replyData ? replyData.description : this.description,
      linkPreviewId: replyData ? replyData.linkPreviewId : this.linkPreviewId
    };
    this.commentsService.onCreate(data).then((res: any) => {
      this.comments.data = res.comments;
      this.description = '';
      this.RTEService.linkPreviewSource.next(null);
      this.RTEService.cleanLinksToSkip();
    }).finally(() => {
      this.inProgress = false;
      this.isReplyInProgress = false;
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

  addLinkPreview(linkPreviewId: number) {
    this.linkPreviewId = linkPreviewId;
  }
}
