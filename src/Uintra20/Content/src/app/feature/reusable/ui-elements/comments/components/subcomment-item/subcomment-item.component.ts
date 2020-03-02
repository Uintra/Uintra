import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CommentActivity } from '../../_constants.js';
import ParseHelper from 'src/app/shared/utils/parse.helper';
import { DomSanitizer } from '@angular/platform-browser';
import { ILikeData } from '../../../like-button/like-button.interface.js';

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

  isEditing = false;
  initialValue = '';
  editedValue = '';
  likeModel: ILikeData;

  get isEditSubmitDisabled() {
    if (!this.editedValue) {
      return true;
    }

    return false;
  }

  constructor(private sanitizer: DomSanitizer) { }

  public ngOnInit(): void {
    this.editedValue = this.data.text;
    this.data.text = this.sanitizer.bypassSecurityTrustHtml(this.data.text);
    const parsed = ParseHelper.parseUbaselineData(this.data);
    this.likeModel = {
      likedByCurrentUser: !!parsed.likeModel.likedByCurrentUser,
      id: this.data.id,
      activityType: CommentActivity,
      likes: parsed.likes,
    };
  }

  public toggleEditingMode(): void {
    this.isEditing = !this.isEditing;
    if (this.isEditing) {
      this.initialValue = this.data.text;
    }
  }

  public onSubmitEditedValue(): void {
    this.submitEditedValue.emit({
      id: this.data.id,
      entityId: this.data.activityId,
      text: this.editedValue,
    });
  }

  public onCommentDelete(): void {
    this.deleteComment.emit(this.data.id);
  }
}
