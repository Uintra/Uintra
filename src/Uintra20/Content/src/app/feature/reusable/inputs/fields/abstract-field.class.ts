import {Input } from '@angular/core';
import {ControlValueAccessor, FormControl} from '@angular/forms';

export abstract class AbstractFieldClass implements ControlValueAccessor {
    @Input('value') _value: any = '';
    @Input() id: string;
    @Input() name: string;
    @Input() required: boolean;

    get value() { return this._value; }
    set value(val) {
        this._value = val;
        this.propagateChange(val);
    }

    onTouched(): any { }
    onChange(): any {}

    abstract validator(value: any): boolean;

    writeValue(value) {
        if (value) {
            this.value = value;
        }
    }

    propagateChange: any = () => {};
    registerOnChange(fn) { this.propagateChange = fn; }
    registerOnTouched(fn: any): void {
      this.onTouched = fn;
    }
    validate(c: FormControl) { return this.validator(c); }
}
