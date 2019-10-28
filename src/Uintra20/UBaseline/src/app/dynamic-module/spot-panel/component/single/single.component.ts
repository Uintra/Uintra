import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';
import { ILink } from 'src/app/shared/interface/link';
import { IUProperty } from 'src/app/shared/interface/umbraco-property';
import { IPictureData } from 'src/app/shared/components/picture/picture.component';

export interface ISingleSpotData {
  title: string;
  description: string;
  link: ILink;
  media:{image: IPictureData};
}

@Component({
  selector: 'app-single',
  templateUrl: './single.component.html',
  styleUrls: ['./single.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class SingleComponent {
  @Input() data: ISingleSpotData;
}
