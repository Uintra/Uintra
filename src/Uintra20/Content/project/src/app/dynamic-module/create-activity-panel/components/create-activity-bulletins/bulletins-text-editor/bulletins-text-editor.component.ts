import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { DropzoneComponent } from 'ngx-dropzone-wrapper';

@Component({
  selector: 'app-bulletins-text-editor',
  templateUrl: './bulletins-text-editor.component.html',
  styleUrls: ['./bulletins-text-editor.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class BulletinsTextEditorComponent implements OnInit {
  @ViewChild('dropdownRef', { static: false }) dropdownRef: DropzoneComponent;

  constructor() { }

  ngOnInit() {
  }

  openDropdown() {
    this.dropdownRef.directiveRef.dropzone().clickableElements[0].click();
  }

}
