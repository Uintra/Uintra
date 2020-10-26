import { Component, OnInit, Input } from '@angular/core';
import { INoImageSpotData } from 'src/app/shared/interfaces/panels/spot/spot-panel.interface';


@Component({
  selector: 'app-no-image',
  templateUrl: './no-image.component.html',
  styleUrls: ['./no-image.component.less']
})
export class NoImageComponent {
  @Input() data: INoImageSpotData;
}
