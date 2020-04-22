import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'forbidden-page',
  templateUrl: './forbidden-page.html',
  styleUrls: ['./forbidden-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class ForbiddenPage {

  public data: any;

  constructor(
    private route: ActivatedRoute
  ) {
    this.route.data.subscribe(data => this.data = data);
  }
}
