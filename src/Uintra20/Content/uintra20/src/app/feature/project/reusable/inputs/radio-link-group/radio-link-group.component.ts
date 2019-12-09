import { Component, OnInit, Input, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

interface IRadioLink {
  type: string;
}

@Component({
  selector: 'app-radio-link-group',
  templateUrl: './radio-link-group.component.html',
  styleUrls: ['./radio-link-group.component.less'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => RadioLinkGroupComponent),
      multi: true
    }
  ]
})
export class RadioLinkGroupComponent implements ControlValueAccessor {
  @Input() links: Array<IRadioLink> = [];
  selectedLink: number;

  constructor() { }

  onRadioChange() {
    this.propagateChange(this.selectedLink);
  }

  onTouched(): any { }
  onChange(): any {}
  propagateChange: any = () => { };
  writeValue(value) { this.selectedLink = value; }
  registerOnChange(fn) { this.propagateChange = fn; }
  registerOnTouched(fn) { this.onTouched = fn; }
}
