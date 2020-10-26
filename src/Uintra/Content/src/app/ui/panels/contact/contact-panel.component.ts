import { Component, HostBinding } from '@angular/core';
import { IContactPanel } from '../../../shared/interfaces/panels/contact/contact-panel.interface';

@Component({
  selector: 'contact-panel',
  templateUrl: './contact-panel.html',
  styleUrls: ['./contact-panel.less']
})
export class ContactPanel {
  data: IContactPanel;

  @HostBinding('class') rootClasses;

  ngOnInit() {
    this.rootClasses = `
      ${ this.data.panelSettings.theme.value.alias || 'default-theme' }
    `;
  }
}
