import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ILikeData } from '../../../like-button/like-button.interface.js';
import { RTEStripHTMLService } from 'src/app/feature/specific/activity/rich-text-editor/helpers/rte-strip-html.service.js';

@Component({
  selector: 'app-subcomment-item',
  templateUrl: './subcomment-item.component.html',
  styleUrls: ['./subcomment-item.component.less']
})
export class SubcommentItemComponent implements OnInit {
  @Input() public data: any;
  @Input() public activityType: any;
  @Input() public commentsActivity: any;
  @Input() public isReplyEditingInProgress: boolean;
  @Output() public submitEditedValue = new EventEmitter();
  @Output() public deleteComment = new EventEmitter();

  public isEditing = false;
  public initialValue = '';
  public editedValue = '';
  public likeModel: ILikeData;

  public get isEditSubmitDisabled() {
    return this.stripHTML.isEmpty(this.editedValue) || this.isReplyEditingInProgress;
  }

  constructor(private sanitizer: DomSanitizer, private stripHTML: RTEStripHTMLService) { }

  public ngOnInit(): void {
    this.editedValue = this.data.text;
    this.data.text = this.sanitizer.bypassSecurityTrustHtml(this.data.text);
    this.likeModel = {
      likedByCurrentUser: !!this.data.likeModel.likedByCurrentUser,
      id: this.data.id,
      activityType: this.commentsActivity,
      likes: this.data.likes,
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
