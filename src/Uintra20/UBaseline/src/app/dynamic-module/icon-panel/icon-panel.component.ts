import { Component, Input, HostBinding } from '@angular/core';
import { IUProperty } from 'src/app/shared/interface/umbraco-property';
import { IPanelSettings } from 'src/app/shared/interface/panel-settings';
import { IPictureData } from 'src/app/shared/components/picture/picture.component';
import { IButtonData } from 'src/app/shared/components/button/button.component';
import { UrlType } from 'src/app/shared/enum/url-type';
import { resolveThemeCssClass } from '../lib/helper/panel-settings';

export interface IconPanelData {
  title: IUProperty<string>,
  centerText: IUProperty<boolean>;
  items: IUProperty<IIconPanelItem[]>;
  panelSettings: IPanelSettings;
}

export interface IIconPanelItem {
  icon: IUProperty<IPictureData>;
  iconScale: IUProperty<number>;
  link: IUProperty<IButtonData>;
  title: IUProperty<string>;
}

@Component({
  selector: 'app-icon-panel',
  templateUrl: './icon-panel.component.html',
  styleUrls: ['./icon-panel.component.less'],
})
export class IconPanelComponent {
  @Input() data: Partial<IconPanelData>;
  @HostBinding('class') get hostClasses() {return resolveThemeCssClass(this.data.panelSettings)}

  urlType = UrlType;

  // ngOnInit() { debugger }
}