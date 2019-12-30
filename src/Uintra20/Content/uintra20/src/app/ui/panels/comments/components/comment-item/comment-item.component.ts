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

  isEditing: boolean = false;
  initialValue: any;

  constructor(private cs: CommentsService) { }

  ngOnInit() {
    console.log(this.data)
  }

  onCommentDelete() {
    this.deleteComment.emit({ targetId: this.data.activityId, targetType: this.activityType, commentId: this.data.id });
  }

  toggleEditingMode() {
    this.isEditing = !this.isEditing;
    if (this.isEditing) {
      this.initialValue = this.data.text;
    }
  }

  cancelEditing() {
    this.data.text = this.initialValue;
    this.toggleEditingMode();
  }

  onSubmitEditedValue() {
    const data = {
      Id: this.data.id,
      EntityId: this.data.activityId,
      EntityType: this.activityType,
      Text: this.data.text,
    }

    this.cs.editComment(data).then((res: any) => {
      this.toggleEditingMode();
    })
  }
}
