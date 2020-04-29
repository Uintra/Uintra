import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ILikeData } from '../../../like-button/like-button.interface.js';
import { RTEStripHTMLService } from 'src/app/feature/specific/activity/rich-text-editor/helpers/rte-strip-html.service.js';
import { ICommentItem } from 'src/app/shared/interfaces/components/comments/item/comment-item.interface.js';
import { ILinkPreview } from 'src/app/feature/reusable/inputs/rich-text-editor/rich-text-editor.interface.js';
import { RichTextEditorService } from 'src/app/feature/reusable/inputs/rich-text-editor/rich-text-editor.service.js';
import { IntranetEntity } from 'src/app/shared/enums/intranet-entity.enum.js';

@Component({
  selector: 'app-subcomment-item',
  templateUrl: './subcomment-item.component.html',
  styleUrls: ['./subcomment-item.component.less']
})
export class SubcommentItemComponent implements OnInit {
  @Input() public data: ICommentItem;
  @Input() public activityType: any;
  @Input() public commentsActivity: any;
  @Input() public isReplyEditingInProgress: boolean;
  @Output() public submitEditedValue = new EventEmitter();
  @Output() public deleteComment = new EventEmitter();

  isEditing = false;
  initialValue = '';
  editedValue = '';
  likeModel: ILikeData;
  linkPreview: ILinkPreview;
  editLinkPreviewId: number;

  public get isEditSubmitDisabled() {
    return this.stripHTML.isEmpty(this.editedValue) || this.isReplyEditingInProgress;
  }

  constructor(
    private sanitizer: DomSanitizer,
    private stripHTML: RTEStripHTMLService,
    private RTEService: RichTextEditorService) { }

  public ngOnInit(): void {
    this.editedValue = this.data.text.toString();
    this.data.text = this.sanitizer.bypassSecurityTrustHtml(this.data.text.toString());
    this.linkPreview = this.data.linkPreview;
    this.likeModel = {
      likedByCurrentUser: !!this.data.likeModel.likedByCurrentUser,
      id: this.data.id,
      activityType: IntranetEntity.Comment,
      likes: this.data.likes,
    };
  }

  public toggleEditingMode(): void {
    this.isEditing = !this.isEditing;
    if (this.isEditing) {
      this.initialValue = this.data.text.toString();
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
