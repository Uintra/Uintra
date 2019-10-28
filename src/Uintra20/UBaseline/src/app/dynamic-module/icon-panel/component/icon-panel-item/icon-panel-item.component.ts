import { Component, OnInit, Input, HostBinding } from '@angular/core';
import { IIconPanelItem } from '../../icon-panel.component';
import { UrlType } from 'src/app/shared/enum/url-type';

@Component({
  selector: 'app-icon-panel-item',
  templateUrl: './icon-panel-item.component.html',
  styleUrls: ['./icon-panel-item.component.less']
})
export class IconPanelItemComponent {
  @Input() data: IIconPanelItem;
  contentVariant: UrlType = null;
  urlType = UrlType;
  @HostBinding('class') get className() { return 'icon-panel-item'};
  
  constructor() { }

  ngOnInit() {
    if (this.data.link && this.data.link.value) this.contentVariant = this.data.link.value.type
  }

}
