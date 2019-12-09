import { Component, OnInit, forwardRef, Input } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-checkbox-input',
  templateUrl: './checkbox-input.component.html',
  styleUrls: ['./checkbox-input.component.less'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CheckboxInputComponent),
      multi: true
    }
  ]
})
export class CheckboxInputComponent implements OnInit {
  @Input() isChecked: boolean = false;

  constructor() { }

  onChangeInput() {
    this.propagateChange(this.isChecked);
  }

  ngOnInit() {
  }

  onTouched(): any { }
  onChange(): any {}
  propagateChange: any = () => { };
  writeValue(value) { this.isChecked = value; }
  registerOnChange(fn) { this.propagateChange = fn; }
  registerOnTouched(fn) { this.onTouched = fn; }
}
