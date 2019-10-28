import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.less']
})
export class ModalComponent {
  @Input() isShow: boolean = false;
  @Output() close: EventEmitter<any> = new EventEmitter();

  isShowContent: boolean = false;

  constructor() {
    const ANIMATION_DELAY = 1;

    setTimeout(() => {
      this.isShowContent = this.isShow;
    }, ANIMATION_DELAY);
  }

  handleClose(): void {
    return this.close.emit(null);
  }
}
