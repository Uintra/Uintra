import { Component, Output, Input, EventEmitter, HostListener, HostBinding } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'ubl-checkbox',
  templateUrl: './checkbox.component.html',
  styleUrls: ['./checkbox.component.less'],
  exportAs: 'ublCheckbox',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: CheckboxComponent,
      multi: true
    }
  ]
})
export class CheckboxComponent implements ControlValueAccessor {
  @Input() mixed: boolean;
  @Input() set disabled(flag: boolean) {this.isDisabled = flag};
  @Input() noChanges: boolean;

  @Output() ngModelChange = new EventEmitter();

  model: boolean;

  changeHandler: any;
  touchedHandler: any;

  @HostBinding('class.disabled') isDisabled: boolean;

  @HostListener('click') onClick()
  {
    if (this.isDisabled || this.noChanges) return;

    this.model = !this.model;
    this.changeHandler(this.model);
  }

  ngOnInit()
  {
    this.isDisabled = this.disabled;
  }

  writeValue(checked: boolean | null)
  {
    this.model = checked
  }

  registerOnChange(fn: any)
  {
    this.changeHandler = fn;
  }

  registerOnTouched(fn: any)
  {
    this.touchedHandler = fn;
  }

  setDisabledState(isDisabled: boolean)
  {
    this.isDisabled = isDisabled;
  }
}
