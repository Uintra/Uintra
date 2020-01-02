import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-subcomment-item',
  templateUrl: './subcomment-item.component.html',
  styleUrls: ['./subcomment-item.component.less']
})
export class SubcommentItemComponent implements OnInit {
  @Input() data: any;
  @Output() submitEditedValue = new EventEmitter();
  @Output() deleteComment = new EventEmitter();

  isEditing: boolean = false;
  initialValue: string = '';
  editedValue: string = '';

  get isEditSubmitDisabled() {
    if (!this.editedValue) {
      return true;
    }

    return false;
  }

  constructor() { }

  ngOnInit() {
    this.editedValue = this.data.text;
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
