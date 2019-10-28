import { Component, OnInit, Input, HostBinding } from '@angular/core';
import * as less from 'less';

interface FaqPanelData {
  title?: any,
  description?: any;
  items?: any;
  panelSettings?: any;
}
@Component({
  selector: 'app-faq-panel',
  templateUrl: './faq-panel.component.html',
  styleUrls: ['./faq-panel.component.less']
})
export class FaqPanelComponent implements OnInit {
  @Input() data: FaqPanelData;
  @HostBinding('class') rootClasses;

  constructor() { }

  ngOnInit() {
    this.rootClasses = `
      ${ this.data.panelSettings.theme.value.alias || 'default-theme' }
      ${ this.data.panelSettings.behaviour.value || 'full-content' }
    `;

    this.setUniqueToItems();
  }

  setUniqueToItems() {
    this.data.items.value = this.data.items.value.map(item => ({
      ...item,
      id: '_' + Math.random().toString(36).substr(2, 9)
    }));
  }

}
