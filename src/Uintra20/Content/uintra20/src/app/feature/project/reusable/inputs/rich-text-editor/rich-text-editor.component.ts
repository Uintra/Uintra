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
  editor: any;
  container: any;
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
    console.log(editor);
    this.editor = editor;
    this.container = editor.container;
    editor.focus();
  }

  onShowDropdown() {
    this.addAttachment.emit();
  }

  onTouched(): any {}
  onChange(): any {}
  propagateChange: any = () => {};
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

  addEmoji(emoji) {
    if (this.editor.getSelection()) {
      this.editor.insertEmbed(this.editor.getSelection().index, 'image', emoji.src);
      this.editor.setSelection(this.editor.getSelection().index + 1);
      this.editor.container.querySelectorAll("img").forEach(img => {
        img.setAttribute('width', '20');
        img.setAttribute('height', '20');
        img.setAttribute('style', 'margin: 0 4px; vertical-align: middle');
      });
    }

    this.closeEmojiPalette();
  }

  closeEmojiPalette() {
    this.isEmojiPalette = false;
  }

  toggleEmojiPalette() {
    this.isEmojiPalette = !this.isEmojiPalette;
  }
}
