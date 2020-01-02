import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CommentsService } from '../../helpers/comments.service';

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

  isEditing: boolean = false;
  editedValue: string;
  initialValue: any;
  isReply: boolean;
  subcommentDescription: string = "";

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

  constructor(private cs: CommentsService) { }

  ngOnInit() {
    this.editedValue = this.data.text;
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
      Id: subcomment.id || this.data.id,
      EntityId: subcomment.entityId || this.data.activityId,
      EntityType: this.activityType,
      Text: subcomment.text || this.editedValue,
    }

    this.cs.editComment(data).then((res: any) => {
      this.editComment.emit(res.comments);
      this.toggleEditingMode();
    })
  }

  onToggleReply() {
    this.isReply = !this.isReply;
  }

  onCommentReply() {
    this.replyComment.emit({ parentId: this.data.id, description: this.subcommentDescription });
  }
}
