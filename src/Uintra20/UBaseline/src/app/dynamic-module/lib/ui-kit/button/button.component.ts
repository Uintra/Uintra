import { Component, HostBinding } from '@angular/core';

@Component({
  selector: 'button[ubl-button]',
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.less'],
  exportAs: 'ublButton'
})
export class ButtonComponent {
  toggled: boolean;

  @HostBinding('attr.disable') disableAttr: boolean;

  constructor() { }
}
