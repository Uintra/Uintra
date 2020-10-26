import { Component, OnInit, Input } from '@angular/core';
import { IDocument } from '../../activity.interfaces';

@Component({
  selector: 'app-attachments',
  templateUrl: './attachments.component.html',
  styleUrls: ['./attachments.component.less']
})
export class AttachmentsComponent implements OnInit {
  @Input() documents: Array<IDocument> = new Array<IDocument>();

  constructor() { }

  ngOnInit() {
  }

}
