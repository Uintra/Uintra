import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CommentsService } from '../../helpers/comments.service';
import { ILikeData } from 'src/app/feature/project/reusable/ui-elements/like-button/like-button.interface';
import { ICommentCreator } from './comment-item.interface';
import ParseHelper from 'src/app/feature/shared/helpers/parse.helper';
import { CommentActivity } from '../../_constants.js';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

@Component({
  selector: 'app-comment-item',
  templateUrl: './comment-item.component.html',
  styleUrls: ['./comment-item.component.less']
})
export class CommentItemComponent implements OnInit {
  @Input() data: any;
  @Input() activityType: any;
  @Output() deleteComment = new EventEmitter();
  @Output() editComment = new EventEmitter();
  @Output() replyComment = new EventEmitter();

  isEditing = false;
  editedValue: string;
  initialValue: any;
  isReply: boolean;
  subcommentDescription: string;
  likeModel: ILikeData;
  commentCreator: ICommentCreator;
  sanitizedContent: SafeHtml;

  get isSubcommentSubmitDisabled() {
    if (!this.subcommentDescription) {
      return true;
    }

    return false;
  }

  get isEditSubmitDisabled() {
    if (!this.editedValue) {
      return true;
    }

    return false;
  }

  constructor(private commentsService: CommentsService, private sanitizer: DomSanitizer) { }

  ngOnInit() {
    console.log(this.data);
    this.editedValue = this.data.text;
    this.sanitizedContent = this.sanitizer.bypassSecurityTrustHtml(this.data.text);
    const parsed = ParseHelper.parseUbaselineData(this.data);
    this.commentCreator = parsed.creator;
    this.likeModel = {
      likedByCurrentUser: !!parsed.likeModel.likedByCurrentUser,
      id: this.data.id,
      activityType: CommentActivity,
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
  }

  onSubmitEditedValue(subcomment?) {
    this.commentsService.editComment(
      this.buildComment(subcomment)
      ).then((res: any) => {
        this.editComment.emit(res.comments);
        this.toggleEditingMode();
      });
  }

  onToggleReply() {
    this.isReply = !this.isReply;
  }

  onCommentReply() {
    this.replyComment.emit({ parentId: this.data.id, description: this.subcommentDescription });
  }

  private buildComment(subcomment?) {
    return {
      Id: subcomment ? subcomment.id : this.data.id,
      EntityId: subcomment ? subcomment.entityId : this.data.activityId,
      EntityType: this.activityType,
      Text: subcomment ? subcomment.text : this.editedValue,
    };
  }
}
