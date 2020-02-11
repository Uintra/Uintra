import { Component, OnInit, ViewEncapsulation, HostBinding, Input, Output, EventEmitter, HostListener, ViewChild, ElementRef } from '@angular/core';

export interface IDropzoneConfig {
  maxFiles?: number;
}

@Component({
  selector: 'app-dropzone-wrapper',
  templateUrl: './dropzone-wrapper.component.html',
  styleUrls: ['./dropzone-wrapper.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class DropzoneWrapperComponent implements OnInit {
  @Input() customMessage: string;
  @Input() filesLength: number;
  @Input() maxFiles: number;
  @Output() success = new EventEmitter();
  @Output() removedFile = new EventEmitter();
  @HostBinding('class') className: string;

  message: string;
  config: IDropzoneConfig = {};

  constructor() { }

  ngOnInit() {
    if (this.maxFiles) {
      this.config.maxFiles = this.maxFiles;
    }
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
