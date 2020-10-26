import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { emojiList, IEmoji } from './helpers/emoji-list';


@Component({
  selector: 'app-rich-text-editor-emoji',
  templateUrl: './rich-text-editor-emoji.component.html',
  styleUrls: ['./rich-text-editor-emoji.component.less']
})
export class RichTextEditorEmojiComponent implements OnInit {
  @Output() emojiClick = new EventEmitter();

  constructor() { }

  emojiList: IEmoji[] = emojiList;

  ngOnInit() {
  }

  onEmojiClick(emoji) {
    this.emojiClick.emit(emoji);
  }
}
