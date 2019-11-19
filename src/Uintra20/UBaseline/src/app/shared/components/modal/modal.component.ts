import { Component, Input, Output, EventEmitter, Inject, OnInit } from '@angular/core';
import { DOCUMENT } from '@angular/common';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.less']
})
export class ModalComponent implements OnInit {
  @Input() isShow: boolean;
  @Output() close = new EventEmitter();

  isShowContent: boolean = false;
  rightOffset: string;

  constructor(@Inject(DOCUMENT) private document: Document)
  {}

  ngOnInit()
  {
    const ANIMATION_DELAY = 1;

    setTimeout(() => {
      this.isShowContent = this.isShow;
    }, ANIMATION_DELAY);

    this.rightOffset = `${this.document.body.offsetWidth - this.document.body.clientWidth}px`;
  }
}
