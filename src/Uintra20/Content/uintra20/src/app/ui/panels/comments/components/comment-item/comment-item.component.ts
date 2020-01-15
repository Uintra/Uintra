import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CommentsService } from '../../helpers/comments.service';
import { ILikeData } from 'src/app/feature/project/reusable/ui-elements/like-button/like-button.interface';
import { ICommentCreator } from './comment-item.interface';
import ParseHelper from 'src/app/feature/shared/helpers/parse.helper';
import { parse } from 'querystring';

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
  commentBody: string;

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

  constructor(private commentsService: CommentsService) { }

  ngOnInit() {
    this.editedValue = this.data.text;
    const parsed = ParseHelper.parseUbaselineData(this.data);
    this.commentCreator = parsed.creator;
    this.commentBody = parsed.text;
    // this.likeModel = {
    //   likedByCurrentUser: !this.data.likeModel.canAddLike,
    //   id: this.data.id,
    //   activityType: this.activityType,
    //   likes: this.data.likeModel.likes,
    // }
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
    }
  }

  onSubmitEditedValue(subcomment?) {
    const data = {
      Id: subcomment ? subcomment.id : this.data.id,
      EntityId: subcomment ? subcomment.entityId : this.data.activityId,
      EntityType: this.activityType,
      Text: subcomment ? subcomment.text : this.editedValue,
    };

    this.commentsService.editComment(data).then((res: any) => {
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
}
