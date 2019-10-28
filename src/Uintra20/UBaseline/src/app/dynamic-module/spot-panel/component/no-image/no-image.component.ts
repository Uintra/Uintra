import { Component, OnInit, Input } from '@angular/core';
import { ILink } from 'src/app/shared/interface/link';

export interface INoImageSpotData {
  title: string;
  description: string;
  link: ILink;
}
@Component({
  selector: 'app-no-image',
  templateUrl: './no-image.component.html',
  styleUrls: ['./no-image.component.less']
})
export class NoImageComponent {
  @Input() data: INoImageSpotData;
}
