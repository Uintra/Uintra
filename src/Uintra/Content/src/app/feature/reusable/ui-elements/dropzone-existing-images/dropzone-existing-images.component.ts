import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IMedia, IDocument } from '../../../specific/activity/activity.interfaces';

@Component({
  selector: 'app-dropzone-existing-images',
  templateUrl: './dropzone-existing-images.component.html',
  styleUrls: ['./dropzone-existing-images.component.less']
})
export class DropzoneExistingImagesComponent implements OnInit {
  @Input() medias: Array<IMedia> = [];
  @Input() otherFiles: Array<IDocument> = [];
  @Output() removeImage = new EventEmitter();
  @Output() removeFile = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  onImageRemove(image) {
    this.removeImage.emit(image);
  }

  handleFileRemove(file) {
    this.removeFile.emit(file);
  }
}
