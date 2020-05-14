import { Component, ViewEncapsulation, HostBinding } from '@angular/core';
import { IContactPanel } from '../../../shared/interfaces/panels/contact/contact-panel.interface';
import { resolveThemeCssClass } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/core/helpers/panel-settings';

@Component({
  selector: 'contact-panel',
  templateUrl: './contact-panel.html',
  styleUrls: ['./contact-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class ContactPanel {
  data: IContactPanel;
  @HostBinding('class') hostClasses;

  ngOnInit() {
    this.hostClasses = resolveThemeCssClass(this.data.panelSettings);
  }
}