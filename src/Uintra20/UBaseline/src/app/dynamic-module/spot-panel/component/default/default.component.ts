import { Component, OnInit, Input } from '@angular/core';
import { IUProperty } from 'src/app/shared/interface/umbraco-property';
import { ILink } from 'src/app/shared/interface/link';

export interface IDefaultSpotData {
  date: IUProperty<string>;
  description: IUProperty<string>;
  link: IUProperty<ILink>;
  media: IUProperty<{image: IUProperty<any>, video: IUProperty<any>}>;
  title: IUProperty<string>;
}
@Component({
  selector: 'app-default',
  templateUrl: './default.component.html',
  styleUrls: ['./default.component.less']
})
export class DefaultComponent {
  @Input() data: {items: IDefaultSpotData[]};
  ngOnInit()
  {
    // debugger
  }
}
