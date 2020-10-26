import { Component, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'developer-ui-kit',
  templateUrl: './developer-ui-kit.html',
  styleUrls: ['./developer-ui-kit.less'],
  encapsulation: ViewEncapsulation.None
})
export class DeveloperUIKitPage {
  testValue: string = 'test value';
  testBoolean: boolean = false;
}
