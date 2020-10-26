import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ICommentHeader } from 'src/app/shared/interfaces/components/comments/header/comment-header.interface';

@Component({
  selector: 'app-comment-header',
  templateUrl: './comment-header.component.html',
  styleUrls: ['./comment-header.component.less']
})
export class CommentHeaderComponent implements OnInit {
  @Input() data: ICommentHeader;
  @Output() toggleReply = new EventEmitter();
  @Output() toggleEditingMode = new EventEmitter();
  @Output() commentDelete = new EventEmitter();

  constructor() { }

  public ngOnInit(): void { }

  onToggleReply() {
    this.toggleReply.emit();
  }

  onToggleEditingMode() {
    this.toggleEditingMode.emit();
  }

  onCommentDelete() {
    this.commentDelete.emit();
  }
}
