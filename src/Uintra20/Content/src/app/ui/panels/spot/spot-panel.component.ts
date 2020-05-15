import { Component, ViewEncapsulation, HostBinding } from '@angular/core';
import { ISpotPanel } from '../../../shared/interfaces/panels/spot/spot-panel.interface';

enum ViewMode {
  SINGLE = 1,
  SINGLE_NO_IMAGE,
  SLIDER,
  DEFAULT
}

@Component({
  selector: 'spot-panel',
  templateUrl: './spot-panel.html',
  styleUrls: ['./spot-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class SpotPanel {
  data: ISpotPanel;
  @HostBinding('class') rootClasses;

  viewMode: ViewMode;
  ViewModes = ViewMode;
  vm: any;

  ngOnInit()
  {
    this.viewMode = this.getTypeOfViewModel(this.data);
    this.vm = this.mapData2ViewMode(this.viewMode, this.data);
    this.rootClasses = `
      ${ this.data.panelSettings.theme.alias || 'default-theme' }
      ${ this.data.panelSettings.behaviour || 'full-content' }
    `;
  }

  private getTypeOfViewModel(data: ISpotPanel)
  {
    if (data.items.length === 1 && data.items[0].media === null) return ViewMode.SINGLE_NO_IMAGE;

    if (data.columnsCount === 1) return ViewMode.SINGLE;

    if (data.columnsCount < data.items.length) return ViewMode.DEFAULT;

    return ViewMode.DEFAULT;
  }

  private mapData2ViewMode(mode: ViewMode, data: ISpotPanel)
  {
    switch(mode) {
      case (ViewMode.SINGLE_NO_IMAGE): return data.items[0];
      case (ViewMode.SINGLE): return data;
      case (ViewMode.DEFAULT): return data;
    }
  }
}