import {
  Component,
  ViewEncapsulation,
  Inject,
  forwardRef,
  Input,
  Output,
  EventEmitter,
  ChangeDetectorRef,
  NgZone
} from "@angular/core";
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from "@angular/forms";

import { QUILL_CONFIG_TOKEN, QuillConfig } from "ngx-quill";
import Quill from "quill";
import Counter from "./quill-modules/counter";
import "quill-mention";
import { MentionsService } from "./quill-modules/mentions.service";
import { RichTextEditorService } from './rich-text-editor.service';
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
  @Input() isEmoji: boolean = true;
  @Input() isEventsOrNews: boolean = false;
  @Output() addAttachment = new EventEmitter();
  @Output() linkPreview = new EventEmitter();

  config: QuillConfig;
  editor: Quill;
  isEmojiPalette: boolean = false;
  test: false;

  get value() {
    return this._value;
  }
  set value(val) {
    this._value = val;
    this.propagateChange(val);
  }

  constructor(
    @Inject(QUILL_CONFIG_TOKEN) config: QuillConfig,
    private richTextEditorService: RichTextEditorService,
    private mentionsService: MentionsService,
    private ngZone: NgZone,
  ) {
    config.modules = {
      ...config.modules,
      mention: this.mentionsService.getMentionsModule()
    };
  }

  suggestPeople(searchTerm) {
    const allPeople = [
      {
        id: 1,
        value: "Fredrik Sundqvist"
      },
      {
        id: 2,
        value: "Patrik Sjölin"
      }
    ];
    return allPeople.filter(person => person.value.includes(searchTerm));
  }

  initEditor(editor) {
    this.editor = editor;
    this.editor.linksToSkip = [];
    this.richTextEditorService.addOnTextChangeCallback(editor);
    this.richTextEditorService.addStylesToImages(editor);

    if (!this.isEventsOrNews) {
      editor.focus();
    }
    this.richTextEditorService.linkPreviewSource.subscribe(result => {
      if ((this.editor.firstLinkPreview && this.editor.firstLinkPreview.uri) === (result && result.uri)) {
        this.ngZone.run(() => {
          this.editor.firstLinkPreview === result;
          this.linkPreview.emit(result && result.id);
        });
      }
    });
    this.richTextEditorService.cleanLinksToSkipSubject.subscribe(() => {
      this.editor.linksToSkip = [];
    })
  }

  onShowDropdown() {
    this.addAttachment.emit();
  }

  onTouched(): any { }
  onChange(): any { }
  propagateChange(val) { }
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
    return { "top-mode": this.isEditing };
  }

  closeEmojiPalette() {
    this.isEmojiPalette = false;
  }

  toggleEmojiPalette() {
    this.isEmojiPalette = !this.isEmojiPalette;
  }

  addEmoji(emoji, index?) {
    if (index) {
      this.richTextEditorService.addEmoji(this.editor, emoji, index);
    }

    this.richTextEditorService.addEmoji(this.editor, emoji);

    this.closeEmojiPalette();
  }
  public closeLinkPreview(): void {
    this.editor.linksToSkip.push(this.editor.firstLinkPreview && this.editor.firstLinkPreview.url);
    this.richTextEditorService.getLinkPreview(this.editor);
  }
}
