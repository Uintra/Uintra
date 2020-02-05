import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-comment-header',
  templateUrl: './comment-header.component.html',
  styleUrls: ['./comment-header.component.less']
})
export class CommentHeaderComponent implements OnInit {
  @Input() data: any;
  @Input() isReply: any;
  @Output() toggleReply = new EventEmitter();
  @Output() toggleEditingMode = new EventEmitter();
  @Output() commentDelete = new EventEmitter();

  constructor() { }

  ngOnInit() {
    console.log(this.data);
  }

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
