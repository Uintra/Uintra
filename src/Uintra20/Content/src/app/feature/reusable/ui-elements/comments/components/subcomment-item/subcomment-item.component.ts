import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import ParseHelper from 'src/app/shared/utils/parse.helper';
import { DomSanitizer } from '@angular/platform-browser';
import { ILikeData } from '../../../like-button/like-button.interface.js';
import { RTEStripHTMLService } from 'src/app/feature/specific/activity/rich-text-editor/helpers/rte-strip-html.service.js';
import { ILinkPreview } from 'src/app/feature/reusable/inputs/rich-text-editor/rich-text-editor.interface.js';
import { RichTextEditorService } from 'src/app/feature/reusable/inputs/rich-text-editor/rich-text-editor.service.js';

@Component({
  selector: 'app-subcomment-item',
  templateUrl: './subcomment-item.component.html',
  styleUrls: ['./subcomment-item.component.less']
})
export class SubcommentItemComponent implements OnInit {
  @Input() data: any;
  @Input() activityType: any;
  @Input() commentsActivity: any;
  @Input() isReplyEditingInProgress: boolean;
  @Output() submitEditedValue = new EventEmitter();
  @Output() deleteComment = new EventEmitter();

  isEditing = false;
  initialValue = '';
  editedValue = '';
  likeModel: ILikeData;
  linkPreview: ILinkPreview;
  editLinkPreviewId: number;

  get isEditSubmitDisabled() {
    return this.stripHTML.isEmpty(this.editedValue) || this.isReplyEditingInProgress;
  }

  constructor(
    private sanitizer: DomSanitizer,
    private stripHTML: RTEStripHTMLService,
    private RTEService: RichTextEditorService) { }

  public ngOnInit(): void {
    this.editedValue = this.data.text;
    this.data.text = this.sanitizer.bypassSecurityTrustHtml(this.data.text);
    const parsed = ParseHelper.parseUbaselineData(this.data);
    this.linkPreview = parsed.linkPreview;
    this.likeModel = {
      likedByCurrentUser: !!parsed.likeModel.likedByCurrentUser,
      id: this.data.id,
      activityType: this.commentsActivity,
      likes: parsed.likes,
    };
  }

  public toggleEditingMode(): void {
    this.isEditing = !this.isEditing;
    if (this.isEditing) {
      this.initialValue = this.data.text;
    }
    this.RTEService.linkPreviewSource.next(null);
    this.RTEService.cleanLinksToSkip();
  }

  public onSubmitEditedValue(): void {
    this.submitEditedValue.emit({
      id: this.data.id,
      entityId: this.data.activityId,
      text: this.editedValue,
      linkPreviewId: this.editLinkPreviewId
    });
    this.RTEService.linkPreviewSource.next(null);
    this.RTEService.cleanLinksToSkip();
  }

  public onCommentDelete(): void {
    this.deleteComment.emit(this.data.id);
  }

  public addEditLinkPreview(linkPreviewId: number) {
    this.editLinkPreviewId = linkPreviewId;
  }
}
