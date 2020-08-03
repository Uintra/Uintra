import { Component, Input/*, ViewEncapsulation*/ } from '@angular/core';
import { ISingleSpotData, IDefaultSpotData } from 'src/app/shared/interfaces/panels/spot/spot-panel.interface';

@Component({
  selector: 'app-single',
  templateUrl: './single.component.html',
  styleUrls: ['./single.component.less']//,
  // encapsulation: ViewEncapsulation.None
})
export class SingleComponent {
  @Input() data: ISingleSpotData;

  constructor() {}

  isVideo(item: IDefaultSpotData) {
    return item && item.media && item.media.video;
  }

  isImage(item: IDefaultSpotData) {
    return item && item.media && item.media.image;
  }
}
