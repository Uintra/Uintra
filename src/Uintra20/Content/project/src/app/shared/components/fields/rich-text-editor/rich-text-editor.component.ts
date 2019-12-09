import { Component, ViewEncapsulation, ViewChild } from '@angular/core';
import { DropzoneComponent } from 'ngx-dropzone-wrapper';
import Quill from 'quill';
import Counter from './helpers/counterQuillModule';
import 'quill-emoji/dist/quill-emoji';

Quill.register('modules/counter', Counter);

@Component({
  selector: 'app-rich-text-editor',
  templateUrl: './rich-text-editor.component.html',
  styleUrls: ['./rich-text-editor.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class RichTextEditorComponent {
  @ViewChild('dropdownRef', { static: false }) dropdownRef: DropzoneComponent;

  bulletinContent: string;

  quillConfig = {
    toolbar: {
      container: [
        ['bold', 'italic', 'link'],
        ['emoji']
      ],
      handlers: {'emoji': function() {}}
    },
    'emoji-toolbar': true,
    counter: {
      container: '#counter',
      unit: 'character',
      // TODO: get maxLength value from server
      maxLength: 2000
    }
  };

  constructor() { }

  setFocus(editor) {
    editor.focus();
  }

  openDropdown() {
    this.dropdownRef.directiveRef.dropzone().clickableElements[0].click();
  }
}
