import { Component, OnInit, Input, HostBinding } from '@angular/core';
import { resolveThemeCssClass } from '../lib/helper/panel-settings';

@Component({
  selector: 'app-contact-panel',
  templateUrl: './contact-panel.component.html',
  styleUrls: ['./contact-panel.component.less']
})
export class ContactPanelComponent implements OnInit {
  @Input() data: any;
  @HostBinding('class') get hostClasses() { return resolveThemeCssClass(this.data.panelSettings) }

  constructor() { }

  ngOnInit() {

  }
}

