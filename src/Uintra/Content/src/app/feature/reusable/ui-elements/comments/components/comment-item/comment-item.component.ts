import { Component, OnInit, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import { CommentsService } from '../../helpers/comments.service';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { ICreator } from 'src/app/shared/interfaces/general.interface';
import { ILikeData } from '../../../like-button/like-button.interface';
import { RTEStripHTMLService } from 'src/app/feature/specific/activity/rich-text-editor/helpers/rte-strip-html.service';
import { finalize } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { ICommentItem } from 'src/app/shared/interfaces/components/comments/item/comment-item.interface';
import { ILinkPreview } from 'src/app/feature/reusable/inputs/rich-text-editor/rich-text-editor.interface';
import { RichTextEditorService } from 'src/app/feature/reusable/inputs/rich-text-editor/rich-text-editor.service';
import { IntranetEntity } from 'src/app/shared/enums/intranet-entity.enum';

@Component({
  selector: 'app-comment-item',
  templateUrl: './comment-item.component.html',
  styleUrls: ['./comment-item.component.less']
})
export class CommentItemComponent implements OnInit, OnDestroy {

  private $editCommentSubscription: Subscription;
  @Input()
  public data: ICommentItem;
  @Input()
  public activityType: any;
  @Input()
  public commentsActivity: any;
  @Input()
  public isReplyInProgress: boolean;
  @Output()
  public deleteComment = new EventEmitter();
  @Output()
  public editComment = new EventEmitter();
  @Output()
  public replyComment = new EventEmitter();

  public isEditing = false;
  public editedValue: string;
  public initialValue: any;
  public isReply = false;
  public subcommentDescription: string;
  public likeModel: ILikeData;
  public commentCreator: ICreator;
  public sanitizedContent: SafeHtml;
  public isEditSubmitLoading: boolean;
  public isReplyEditingInProgress: boolean;
  linkPreview: ILinkPreview;
  replyLinkPreviewId: number;
  editLinkPreviewId: number;

  public get isSubcommentSubmitDisabled() {
    return this.stripHTML.isEmpty(this.subcommentDescription) || this.isReplyInProgress;
  }

  public get isEditSubmitDisabled() {
    return this.stripHTML.isEmpty(this.editedValue) || this.isEditSubmitLoading;
  }

  constructor(
    private commentsService: CommentsService,
    private sanitizer: DomSanitizer,
    private stripHTML: RTEStripHTMLService,
    private RTEService: RichTextEditorService) { }

  public ngOnDestroy(): void {
    if (this.$editCommentSubscription) { this.$editCommentSubscription.unsubscribe(); }
  }

  public ngOnInit(): void {
    this.editedValue = this.data.text.toString();
    this.sanitizedContent = this.sanitizer.bypassSecurityTrustHtml(this.data.text.toString());
    this.commentCreator = this.data.creator;
    this.linkPreview = this.data.linkPreview;
    this.likeModel = {
      likedByCurrentUser: !!this.data.likeModel.likedByCurrentUser,
      id: this.data.id,
      activityType: IntranetEntity.Comment,
      likes: this.data.likes
    };
  }

  public onCommentDelete(subcommentId?): void {
    this.deleteComment.emit({
      targetId: this.data.activityId,
      targetType: this.activityType,
      commentId: subcommentId || this.data.id
    });
  }

  public toggleEditingMode(): void {
    this.isEditing = !this.isEditing;
    if (this.isEditing) {
      this.initialValue = this.data.text;
    } else {
      this.editedValue = this.initialValue;
    }
    this.RTEService.linkPreviewSource.next(null);
    this.RTEService.cleanLinksToSkip();
  }

  public onSubmitEditedValue(subcomment?): void {
    if (subcomment) { this.isReplyEditingInProgress = true; }
    this.isEditSubmitLoading = true;
    this.$editCommentSubscription = this.commentsService.editComment(this.buildComment(subcomment))
      .pipe(
        finalize(() => {
          this.isEditSubmitLoading = false;
          this.isReplyEditingInProgress = false;
        })
      )
      .subscribe((res: any) => {
        this.editComment.emit(res.comments);
        this.toggleEditingMode();
      });
  }

  public onToggleReply(): void {
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
