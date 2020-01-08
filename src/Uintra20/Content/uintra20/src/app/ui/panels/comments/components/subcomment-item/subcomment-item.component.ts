import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ILikeData } from 'src/app/feature/project/reusable/ui-elements/like-button/like-button.interface';

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
    // this.likeModel = {
    //   likedByCurrentUser: !this.data.likeModel.canAddLike,
    //   id: this.data.id,
    //   activityType: this.activityType,
    //   likes: this.data.likeModel.likes,
    // }
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
