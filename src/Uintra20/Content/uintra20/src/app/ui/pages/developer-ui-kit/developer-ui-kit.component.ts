import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'developer-ui-kit',
  templateUrl: './developer-ui-kit.html',
  styleUrls: ['./developer-ui-kit.less'],
  encapsulation: ViewEncapsulation.None
})
export class DeveloperUIKitPage implements OnInit{
  testValue: string = 'test value';
  testBoolean: boolean = false;

  reactiveForm = null;
  field1 = null;
  field2 = null;

  options1:any = {
    // defaultDate: new Date(),
    // minDate: new Date(),
    // maxDate: null

  };

  options2: any = {

  };

  constructor() {

  }
      ngOnInit(): void {
        this.field1 = new Date();
      }

  onSubmit() {
    this.field1.format();
    this.field2.format();
  }

  dp1Change() {
    this.options2 = {
      ...this.options2,
      minDate: this.field1,
    }
  }

  dp2Change() {
    this.options1 = {
      ...this.options1,
      maxDate: this.field2,
    }
  }
}
