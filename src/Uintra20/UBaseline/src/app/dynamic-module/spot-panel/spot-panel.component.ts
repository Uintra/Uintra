import { Component, OnInit, Input, HostBinding } from '@angular/core';
import { IUProperty } from 'src/app/shared/interface/umbraco-property';
import mapValues from 'lodash/mapValues';
import pick from 'lodash/pick';

enum ViewMode {
  SINGLE = 1,
  SINGLE_NO_IMAGE,
  SLIDER,
  DEFAULT
}
interface ISpotPanelData {
  title?: IUProperty<string>;
  description?: IUProperty<string>;
  media?: IUProperty<any>;
  link?: IUProperty<Object>;
  panelSettings?: any;
  items: IUProperty<any[]>;
  columnsCount: IUProperty<number>;
}
@Component({
  selector: 'app-spot-panel',
  templateUrl: './spot-panel.component.html',
  styleUrls: ['./spot-panel.component.less']
})
export class SpotPanelComponent implements OnInit {
  @Input() data: ISpotPanelData;
  @HostBinding('class') rootClasses;

  viewMode: ViewMode;
  ViewModes = ViewMode;
  vm: any;

  ngOnInit() 
  {
    this.viewMode = this.getTypeOfViewModel(this.data);
    this.vm = this.mapData2ViewMode(this.viewMode, this.data);
    this.rootClasses = `
      ${ this.data.panelSettings.theme.value.alias || 'default-theme' }
      ${ this.data.panelSettings.behaviour.value || 'full-content' }
    `;
  }

  private getTypeOfViewModel(data: ISpotPanelData)
  {
    if (data.items.value.length === 1 && data.items.value[0].media.value.image === null && data.items.value[0].media.value.video === null)
      return ViewMode.SINGLE_NO_IMAGE;
    
    if (data.items.value.length === 1) return ViewMode.SINGLE;

    if (data.columnsCount.value < data.items.value.length) return ViewMode.DEFAULT;
    
    return ViewMode.DEFAULT;
  }

  private mapData2ViewMode(mode: ViewMode, data: ISpotPanelData)
  {
    switch(mode) {
      case (ViewMode.SINGLE_NO_IMAGE): return this.getValueByKey(data.items.value[0], ['title', 'description', 'link']);
      case (ViewMode.SINGLE): return this.getValueByKey(data.items.value[0], ['title', 'media', 'description', 'link']);
      case (ViewMode.DEFAULT): return this.getValueByKey(data, ['items']);
    }
  }

  private getValueByKey(from, what: string[])
  {
    return mapValues(pick(from, what), item => item && item.value);
  }
}

