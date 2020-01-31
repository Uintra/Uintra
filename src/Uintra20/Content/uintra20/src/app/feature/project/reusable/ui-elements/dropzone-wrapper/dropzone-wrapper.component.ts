import { Component, OnInit, ViewEncapsulation, HostBinding, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-dropzone-wrapper',
  templateUrl: './dropzone-wrapper.component.html',
  styleUrls: ['./dropzone-wrapper.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class DropzoneWrapperComponent implements OnInit {
  @Input() customMessage: string;
  @Input() filesLength: number;
  @Output() success = new EventEmitter();
  @Output() removedFile = new EventEmitter();
  @HostBinding('class') className: string;

  message: string;

  constructor() { }

  ngOnInit() {
    this.className = 'dropzone-wrapper';
    this.message = `<span class='custom-message icon-upload'>${this.customMessage ? this.customMessage : 'Insert image'}</span>`;
  }

  onUploadSuccess(event) {
    this.success.emit(event);
  }

  onFileRemoved(event) {
    this.removedFile.emit(event);
  }
}
