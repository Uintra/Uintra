import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ILikeData } from 'src/app/feature/project/reusable/ui-elements/like-button/like-button.interface';
import { CommentActivity } from '../../_constants.js';
import ParseHelper from 'src/app/feature/shared/helpers/parse.helper';

@Component({
  selector: 'app-subcomment-item',
  templateUrl: './subcomment-item.component.html',
  styleUrls: ['./subcomment-item.component.less']
})
export class SubcommentItemComponent implements OnInit {
  @Input() data: any;
  @Input() activityType: any;
  @Output() submitEditedValue = new EventEmitter();
  @Output() deleteComment = new EventEmitter();

  isEditing: boolean = false;
  initialValue: string = '';
  editedValue: string = '';
  likeModel: ILikeData;

  get isEditSubmitDisabled() {
    if (!this.editedValue) {
      return true;
    }

    return false;
  }

  constructor() { }

  ngOnInit() {
    this.editedValue = this.data.text;
    const parsed = ParseHelper.parseUbaselineData(this.data);
    this.likeModel = {
      likedByCurrentUser: !!parsed.likeModel.likedByCurrentUser,
      id: this.data.id,
      activityType: CommentActivity,
      likes: parsed.likes,
    };
  }

  toggleEditingMode() {
    this.isEditing = !this.isEditing;
    if (this.isEditing) {
      this.initialValue = this.data.text;
    }
  }

  onSubmitEditedValue() {
    const data = {
      id: this.data.id,
      entityId: this.data.activityId,
      text: this.editedValue,
    }

    this.submitEditedValue.emit(data);
  }

  onCommentDelete() {
    this.deleteComment.emit(this.data.id);
  }
}
