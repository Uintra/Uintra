import { Component, Input, EventEmitter, Output } from '@angular/core';
import { IUProperty } from '../../interface/umbraco-property';

export interface IModalVideo {
  title: IUProperty<string>;
  description: IUProperty<string>;
  video: any;
  params?: string;
}
@Component({
  selector: 'app-modal-video',
  templateUrl: './modal-video.component.html',
  styleUrls: ['./modal-video.component.less']
})
export class ModalVideoComponent {
  @Input() data: IModalVideo;
  @Input() isShow: boolean = false;
  @Output() close: EventEmitter<any> = new EventEmitter();

  constructor() { }

  readonly UMEDIA_TYPE: number = 2;

  handleClose(): void {
    return this.close.emit(null);
  }
}
