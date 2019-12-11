import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'developer-ui-kit',
  templateUrl: './developer-ui-kit.html',
  styleUrls: ['./developer-ui-kit.less'],
  encapsulation: ViewEncapsulation.None
})
export class DeveloperUIKitPage {
  data: any;
  testValue: string = 'test value';
  testBoolean: boolean = false;

  constructor(
    private route: ActivatedRoute
  ) {
    this.route.data.subscribe(data => this.data = data);
  }
}
