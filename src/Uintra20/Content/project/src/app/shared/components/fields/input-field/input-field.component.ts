import { Component, Input, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, NG_VALIDATORS, FormControl } from '@angular/forms';
import { AbstractFieldClass } from '../abstract-field.class';

type InputType = 'text' | 'password' | 'search';

@Component({
  selector: 'input-field',
  templateUrl: 'input-field.template.html',
  styles: [`
    :host {
      display: flex;
      align-items: center;
      width: 100%;
    }
    .input {
      display: block;
      flex: 1 1 auto;
      width: 100%;
      border: none;
      background-image: none;
      background-color: transparent;
      -webkit-box-shadow: none;
      -moz-box-shadow: none;
      box-shadow: none;
    }
  `],
  providers: [
    { provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => InputFieldComponent), multi: true },
    { provide: NG_VALIDATORS, useExisting: forwardRef(() => InputFieldComponent), multi: true }
  ]
})
export class InputFieldComponent extends AbstractFieldClass {
  @Input() type: InputType = 'text';
  @Input() ariaLabel = '';
  @Input() role = 'textbox';
  @Input() placeholder: string;

  constructor() {
    super();
  }

  public validator(value: FormControl): boolean {
    return true;
  }
}
