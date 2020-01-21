import {
  Component,
  ViewEncapsulation,
  Inject,
  forwardRef,
  Input,
  Output,
  EventEmitter
} from "@angular/core";
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from "@angular/forms";

import { QUILL_CONFIG_TOKEN, QuillConfig } from "ngx-quill";
import Quill from "quill";
import Counter from "./quill-modules/counter";
import { emojiList } from './rich-text-editor-emoji/helpers/emoji-list';
Quill.register("modules/counter", Counter);


@Component({
  selector: "app-rich-text-editor",
  templateUrl: "./rich-text-editor.component.html",
  styleUrls: ["./rich-text-editor.component.less"],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => RichTextEditorComponent),
      multi: true
    }
  ],
  encapsulation: ViewEncapsulation.None
})
export class RichTextEditorComponent implements ControlValueAccessor {
  @Input("value") _value: string = "";
  @Input() placeholder: string;
  @Input() isDropzone: boolean = true;
  @Input() isUnderline: boolean = true;
  @Input() isEditing: boolean = false;
  @Output() addAttachment = new EventEmitter();

  config: QuillConfig;
  editor: Quill;
  isEmojiPalette: boolean = false;

  get value() {
    return this._value;
  }
  set value(val) {
    this._value = val;
    this.propagateChange(val);
  }

  constructor(@Inject(QUILL_CONFIG_TOKEN) config: QuillConfig) { }

  initEditor(editor) {
    this.editor = editor;
    this.editor.on('text-change', (delta, source) => {
      let stringFromDelta = '';
      this.editor.getContents().ops.forEach(op => {
        if (typeof op.insert === 'string') {
          stringFromDelta += op.insert;
        } else {
          //instead of images we insert '1' just to know right index
          stringFromDelta += '1';
        }
      });

      emojiList.forEach(emoji => {
        while (stringFromDelta.includes(emoji.shortcut)) {
          const index = stringFromDelta.indexOf(emoji.shortcut);
          this.editor.deleteText(index, emoji.shortcut.length);
          this.addEmoji(emoji, index);
          stringFromDelta = stringFromDelta.slice(0, index) + '1' + stringFromDelta.slice(index + emoji.shortcut.length);
        }
      })
    });
    editor.focus();
  }

  onShowDropdown() {
    this.addAttachment.emit();
  }

  onTouched(): any { }
  onChange(): any { }
  propagateChange(val) {
  };
  writeValue(value) {
    this.value = value;
  }
  registerOnChange(fn) {
    this.propagateChange = fn;
  }
  registerOnTouched(fn) {
    this.onTouched = fn;
  }

  getToolbarClass() {
    return { 'top-mode': this.isEditing };
  }

  addEmoji(emoji, index?) {
    this.editor.insertEmbed(index || this.getCursorOrContentLength(), 'image', emoji.src);
    this.editor.setSelection(index + 1 || this.getCursorOrContentLength() + 1);
    this.editor.container.querySelectorAll("img").forEach(img => {
      img.setAttribute('width', '20');
      img.setAttribute('height', '20');
      img.setAttribute('style', 'margin: 0 4px; vertical-align: middle');
    });

    this.closeEmojiPalette();
  }

  closeEmojiPalette() {
    this.isEmojiPalette = false;
  }

  toggleEmojiPalette() {
    this.isEmojiPalette = !this.isEmojiPalette;
  }

  getCursorOrContentLength() {
    const cursor = this.editor.getSelection();
    if (cursor) {
      if (cursor.length) {
        this.editor.deleteText(cursor.index, cursor.length);
      }
      return cursor.index;
    }

    return this.editor.getContents().ops.length > 1 ? this.editor.getLength() - 1 : 0;
  }


}
