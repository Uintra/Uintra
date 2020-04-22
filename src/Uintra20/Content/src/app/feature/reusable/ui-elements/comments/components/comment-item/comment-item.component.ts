import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CommentsService } from '../../helpers/comments.service';
import ParseHelper from 'src/app/shared/utils/parse.helper';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { ICreator } from 'src/app/shared/interfaces/general.interface';
import { ILikeData } from '../../../like-button/like-button.interface';
import { RTEStripHTMLService } from 'src/app/feature/specific/activity/rich-text-editor/helpers/rte-strip-html.service';
import { ILinkPreview } from 'src/app/feature/reusable/inputs/rich-text-editor/rich-text-editor.interface';
import { RichTextEditorService } from 'src/app/feature/reusable/inputs/rich-text-editor/rich-text-editor.service';

@Component({
  selector: 'app-comment-item',
  templateUrl: './comment-item.component.html',
  styleUrls: ['./comment-item.component.less']
})
export class CommentItemComponent implements OnInit {
  @Input() data: any;
  @Input() activityType: any;
  @Input() commentsActivity: any;
  @Input() isReplyInProgress: boolean;
  @Output() deleteComment = new EventEmitter();
  @Output() editComment = new EventEmitter();
  @Output() replyComment = new EventEmitter();

  isEditing = false;
  editedValue: string;
  initialValue: any;
  isReply: boolean = false;
  subcommentDescription: string;
  likeModel: ILikeData;
  commentCreator: ICreator;
  sanitizedContent: SafeHtml;
  isEditSubmitLoading: boolean;
  isReplyEditingInProgress: boolean;
  linkPreview: ILinkPreview;
  replyLinkPreviewId: number;
  editLinkPreviewId: number;

  get isSubcommentSubmitDisabled() {
    return this.stripHTML.isEmpty(this.subcommentDescription) || this.isReplyInProgress;
  }

  get isEditSubmitDisabled() {
    return this.stripHTML.isEmpty(this.editedValue) || this.isEditSubmitLoading;
  }

  constructor(
    private commentsService: CommentsService,
    private sanitizer: DomSanitizer,
    private stripHTML: RTEStripHTMLService,
    private RTEService: RichTextEditorService) { }

  ngOnInit() {
    this.editedValue = this.data.text;
    this.sanitizedContent = this.sanitizer.bypassSecurityTrustHtml(this.data.text);
    const parsed = ParseHelper.parseUbaselineData(this.data);
    this.commentCreator = parsed.creator;
    this.linkPreview = parsed.linkPreview;
    this.likeModel = {
      likedByCurrentUser: !!parsed.likeModel.likedByCurrentUser,
      id: this.data.id,
      activityType: this.commentsActivity,
      likes: parsed.likes,
    };
  }

  onCommentDelete(subcommentId?) {
    this.deleteComment.emit({
      targetId: this.data.activityId,
      targetType: this.activityType,
      commentId: subcommentId || this.data.id
    });
  }

  toggleEditingMode() {
    this.isEditing = !this.isEditing;
    if (this.isEditing) {
      this.initialValue = this.data.text;
    } else {
      this.editedValue = this.initialValue;
    }
    this.RTEService.linkPreviewSource.next(null);
    this.RTEService.cleanLinksToSkip();
  }

  onSubmitEditedValue(subcomment?) {
    if (subcomment) {this.isReplyEditingInProgress = true}
    this.isEditSubmitLoading = true;
    this.commentsService.editComment(
      this.buildComment(subcomment)
      ).then((res: any) => {
        this.editComment.emit(res.comments);
        this.toggleEditingMode();
      }).finally(() => {
        this.isEditSubmitLoading = false;
        this.isReplyEditingInProgress = false;
      });
  }

  onToggleReply() {
    this.isReply = !this.isReply;
    this.RTEService.linkPreviewSource.next(null);
    this.RTEService.cleanLinksToSkip();
  }

  onCommentReply() {
    this.replyComment.emit({
      parentId: this.data.id,
      description: this.subcommentDescription,
      linkPreviewId: this.replyLinkPreviewId
     });
    this.RTEService.linkPreviewSource.next(null);
    this.RTEService.cleanLinksToSkip();
  }

  private buildComment(subcomment?) {
    return {
      Id: subcomment ? subcomment.id : this.data.id,
      EntityId: subcomment ? subcomment.entityId : this.data.activityId,
      EntityType: this.activityType,
      Text: subcomment ? subcomment.text : this.editedValue,
      linkPreviewId: subcomment ? subcomment.linkPreviewId : this.editLinkPreviewId,
    };
  }

  addReplyLinkPreview(linkPreviewId: number) {
    this.replyLinkPreviewId = linkPreviewId;
  }

  addEditLinkPreview(linkPreviewId: number) {
    this.editLinkPreviewId = linkPreviewId;
  }
}
