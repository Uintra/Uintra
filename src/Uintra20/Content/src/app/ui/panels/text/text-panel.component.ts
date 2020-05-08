import { Component, ViewEncapsulation, HostBinding } from '@angular/core';
import { ITextPanelData } from 'src/app/shared/interfaces/panels/text/text-panel.interface';
import { resolveThemeCssClass } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/core/helpers/panel-settings';

@Component({
  selector: 'text-panel',
  templateUrl: './text-panel.html',
  styleUrls: ['./text-panel.less']
})
export class TextPanel {
  data: ITextPanelData;
  @HostBinding('class') hostClasses;


  ngOnInit(){
    this.hostClasses = resolveThemeCssClass(this.data.panelSettings);
  }
}
