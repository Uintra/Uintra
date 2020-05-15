import { Component, ViewEncapsulation, HostBinding } from '@angular/core';
import { IImagePanel } from '../../../shared/interfaces/panels/image/image-panel.interface';
import { resolveThemeCssClass } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/core/helpers/panel-settings';

@Component({
  selector: 'image-panel',
  templateUrl: './image-panel.html',
  styleUrls: ['./image-panel.less']//,
  //encapsulation: ViewEncapsulation.None
})
export class ImagePanel {
  data: IImagePanel;
  // @HostBinding('class') rootClasses;

  // ngOnInit() {
  //   this.rootClasses = `
  //     ${ this.data.panelSettings.theme.value.alias || 'default-theme' }
  //   `;
  // }
}
