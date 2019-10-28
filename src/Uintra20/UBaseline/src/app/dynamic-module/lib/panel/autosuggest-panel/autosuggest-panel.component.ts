import { Component, Input, HostBinding } from '@angular/core';
import { resolveThemeCssClass } from '../../helper/panel-settings';

@Component({
  selector: 'ubl-autosuggest-panel',
  templateUrl: './autosuggest-panel.component.html',
  styleUrls: ['./autosuggest-panel.component.less']
})
export class AutosuggestPanelComponent {

  @Input() data: any;
  @HostBinding('class') get hostClasses() 
  {
    return resolveThemeCssClass(this.data);
  }
}
