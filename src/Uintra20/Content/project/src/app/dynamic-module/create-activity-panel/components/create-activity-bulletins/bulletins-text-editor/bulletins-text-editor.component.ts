import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { DropzoneComponent } from 'ngx-dropzone-wrapper';
import 'quill-emoji/dist/quill-emoji';
import Quill from 'quill';
import Counter from './counterQuillModule';

Quill.register('modules/counter', Counter);

@Component({
  selector: 'app-bulletins-text-editor',
  templateUrl: './bulletins-text-editor.component.html',
  styleUrls: ['./bulletins-text-editor.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class BulletinsTextEditorComponent implements OnInit {
  @ViewChild('dropdownRef', { static: false }) dropdownRef: DropzoneComponent;

  bulletinContent: string;

  quillConfig = {
    toolbar: {
      container: [
        ['emoji', 'bold', 'italic', 'link']
      ],
      handlers: {'emoji': function() {}}
    },
    'emoji-toolbar': true,
    counter: {
      container: '#counter',
      unit: 'character',
      maxLenght: 2000
    }
  };

  constructor() { }

  ngOnInit() {

  }

  setFocus(editor) {
    editor.focus();
  }

  openDropdown() {
    this.dropdownRef.directiveRef.dropzone().clickableElements[0].click();
  }
}
