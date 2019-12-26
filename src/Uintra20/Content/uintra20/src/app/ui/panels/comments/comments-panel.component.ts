import { Component, ViewEncapsulation, OnInit, ViewChild } from '@angular/core';
import { ICommentsPanel } from './comments-panel.interface';
import { DropzoneComponent } from 'ngx-dropzone-wrapper';

@Component({
  selector: 'comments-panel',
  templateUrl: './comments-panel.html',
  styleUrls: ['./comments-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class CommentsPanel implements OnInit {
  @ViewChild("dropdownRef", { static: false }) dropdownRef: DropzoneComponent;

  data: ICommentsPanel;
  description: string = "";
  files: Array<any> = [];

  constructor() {
  }

  ngOnInit(): void {
    console.log(this.data);
  }

  addAttachment() {
    this.dropdownRef.directiveRef.dropzone().clickableElements[0].click();
    debugger
  }

  onUploadSuccess(fileArray: Array<any> = []): void {
    this.files.push(fileArray);
  }

  onFileRemoved(removedFile: object) {
    this.files = this.files.filter(file => {
      const fileElement = file[0];
      return fileElement !== removedFile;
    });
  }

  onCommentSubmit() {
    console.log(this.data);
    // const data = {
    //   EntityId: this.data.activityId;
    //   EntityType: this.data.
    //   ParentId: 
    //   Text: 
    // }
  }
}
