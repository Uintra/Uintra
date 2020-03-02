import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ILikeData } from 'src/app/feature/project/reusable/ui-elements/like-button/like-button.interface';
import { CommentActivity } from '../../_constants.js';
import ParseHelper from 'src/app/feature/shared/helpers/parse.helper';
import { DomSanitizer } from '@angular/platform-browser';
import { RTEStripHTMLService } from '../../../../inputs/rich-text-editor/helpers/rte-strip-html.service.js';

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
    return this.stripHTML.isEmpty(this.editedValue);
  }

  constructor(private sanitizer: DomSanitizer, private stripHTML: RTEStripHTMLService) { }

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
