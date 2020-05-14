import { Component, Input } from '@angular/core';
import { IDefaultSpotData } from 'src/app/shared/interfaces/panels/spot/spot-panel.interface';

@Component({
  selector: 'app-default',
  templateUrl: './default.component.html',
  styleUrls: ['./default.component.less']
})
export class DefaultComponent {
  @Input() data: {items: IDefaultSpotData[]};

  constructor() { }

  isVideo(item: IDefaultSpotData) {
    return item && item.media && item.media.video;
  }

  isImage(item: IDefaultSpotData) {
    return item && item.media && item.media.image;
  }
}
