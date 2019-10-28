import { Component, Input, HostBinding, OnInit } from '@angular/core';
import { resolveThemeCssClass } from '../lib/helper/panel-settings';

@Component({
  selector: 'app-autosuggest-panel',
  templateUrl: './autosuggest-panel.component.html',
  styleUrls: ['./autosuggest-panel.component.less']
})
export class AutosuggestPanelComponent implements OnInit {
  @Input() data: any;
  @HostBinding('class') get hostClasses() {
    return resolveThemeCssClass(this.data);
  }

  constructor() { }

  ngOnInit() {
    this.data = this.tempMap(this.data);
    if (!this.data) { return; }
  }

  private tempMap(data: any) {
    let res = {};

    res['tip'] = {
      value: 'Search...'
    };

    return res;
  }
}
