import { Component, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'forbidden-page',
  templateUrl: './forbidden-page.html',
  styleUrls: ['./forbidden-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class ForbiddenPage {

  public data: any;

  constructor() {}
}
