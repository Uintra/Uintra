import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { CommentsService } from './helpers/comments.service';
import { TranslateService } from '@ngx-translate/core';
import { RTEStripHTMLService } from 'src/app/feature/specific/activity/rich-text-editor/helpers/rte-strip-html.service';
import { RichTextEditorService } from '../../inputs/rich-text-editor/rich-text-editor.service';
import { Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { ICommentItem } from 'src/app/shared/interfaces/components/comments/item/comment-item.interface';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.less']
})
export class CommentsComponent implements OnInit, OnDestroy {

  private $deleteCommentSubscription: Subscription;
  private $createCommentSubscription: Subscription;
  @Input()
  public comments: Array<ICommentItem>;
  @Input()
  public entityId: string;
  @Input()
  public activityType: number;
  @Input()
  public commentsActivity: number;
  @Input()
  public isGroupMember = true;
  public description = '';
  public inProgress: boolean;
  public isReplyInProgress: boolean;
  linkPreviewId: number;

  get isSubmitDisabled(): boolean {
    const isEmpty = this.stripHTML.isEmpty(this.description);

    return isEmpty
      ? true
      : false;
  }

  constructor(
    private commentsService: CommentsService,
    private stripHTML: RTEStripHTMLService,
    private translate: TranslateService,
    private RTEService: RichTextEditorService,
  ) { }

  public ngOnDestroy(): void {
    if (this.$deleteCommentSubscription) { this.$deleteCommentSubscription.unsubscribe(); }
    if (this.$createCommentSubscription) { this.$createCommentSubscription.unsubscribe(); }
  }

  public ngOnInit(): void { }

  public onCommentSubmit(replyData?): void {
    if (replyData) { this.isReplyInProgress = true; }
    this.inProgress = true;
    const data = {
      entityId: this.entityId,
      entityType: this.activityType,
      parentId: replyData ? replyData.parentId : null,
      text: replyData ? replyData.description : this.description,
      linkPreviewId: replyData ? replyData.linkPreviewId : this.linkPreviewId
    };
    this.$createCommentSubscription = this.commentsService.onCreate(data)
      .pipe(
        finalize(() => {
          this.inProgress = false;
          this.isReplyInProgress = false;
        }))
      .subscribe((next: any) => {
        this.comments = next.comments;
        this.description = '';
        this.RTEService.linkPreviewSource.next(null);
        this.RTEService.cleanLinksToSkip();
      });
  }

  public deleteComment(obj): void {
    if (confirm(this.translate.instant('common.AreYouSure'))) {
      this.$deleteCommentSubscription = this.commentsService.deleteComment(obj)
        .subscribe((next: any) => {
          this.comments = next.comments;
        });
    }
  }

  public editComment(comments): void {
    this.comments = comments;
  }

  addLinkPreview(linkPreviewId: number) {
    this.linkPreviewId = linkPreviewId;
  }
}
