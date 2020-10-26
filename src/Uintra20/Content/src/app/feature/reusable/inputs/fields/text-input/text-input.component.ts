import { Component, Input, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, NG_VALIDATORS, FormControl } from '@angular/forms';
import { AbstractFieldClass } from '../abstract-field.class';

type InputType = 'text' | 'password' | 'search';

@Component({
  selector: 'text-input',
  templateUrl: 'text-input.component.html',
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
      outline: 0;
      margin: 0;
    }
  `],
  providers: [
    { provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => TextInputComponent), multi: true },
    { provide: NG_VALIDATORS, useExisting: forwardRef(() => TextInputComponent), multi: true }
  ]
})
export class TextInputComponent extends AbstractFieldClass {
  @Input() type: InputType = 'text';
  @Input() ariaLabel = '';
  @Input() role = 'textbox';
  @Input() placeholder: string;
  @Input() maxLength: number;

  constructor() {
    super();
  }

  public validator(value: FormControl): boolean {
    return true;
  }
}
