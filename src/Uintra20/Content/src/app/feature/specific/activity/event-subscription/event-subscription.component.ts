import { Component, OnInit, Input, EventEmitter, Output, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, NG_VALIDATORS } from '@angular/forms';

@Component({
  selector: 'app-event-subscription',
  templateUrl: './event-subscription.component.html',
  styleUrls: ['./event-subscription.component.less'],
  providers: [
    { provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => EventSubscriptionComponent), multi: true },
    { provide: NG_VALIDATORS, useExisting: forwardRef(() => EventSubscriptionComponent), multi: true }
  ]
})
export class EventSubscriptionComponent implements OnInit {
  @Input() isChecked: boolean;
  @Output() checkboxChange = new EventEmitter<boolean>();

  inputValue: string;

  constructor() { }

  ngOnInit() {
  }

  onCheckboxChange() {
    this.checkboxChange.emit(this.isChecked);
  }

  onChangeInput(val) {
    debugger
    this.propagateChange(val);
  }

  onTouched(): any { }
  onChange(e): any {}
  propagateChange: any = () => { };
  writeValue(value) {
    this.onChange(this.inputValue);}
  registerOnChange(fn) { this.propagateChange = fn; }
  registerOnTouched(fn) { this.onTouched = fn; }
}
