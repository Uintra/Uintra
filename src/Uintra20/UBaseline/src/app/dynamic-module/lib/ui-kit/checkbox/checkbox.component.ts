import { Component, Output, Input, EventEmitter } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'label[ubl-checkbox]',
  templateUrl: './checkbox.component.html',
  styleUrls: ['./checkbox.component.less'],
  exportAs: 'ublCheckbox',
  host: {
    'attr.ngDefaultControl': ''
  },
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: CheckboxComponent,
      multi: true
    }
  ]
})
export class CheckboxComponent implements ControlValueAccessor {
  @Input() ngModel: boolean;
  @Output() ngModelChange = new EventEmitter();
  @Input() value: boolean;
  @Input() name: string;
  @Input() partlyChecked: boolean;

  isDisabled: boolean;
  model: boolean;

  changeHandler: any;
  touchedHandler: any;
  writeValue(checked: boolean | null): void {
    if (checked === null) return;

    this.model = checked
  }

  registerOnChange(fn: any): void {
    this.changeHandler = fn;
  }
  registerOnTouched(fn: any): void {
    this.touchedHandler = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    this.isDisabled = isDisabled;
  }
}
