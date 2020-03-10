import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'page-not-found-page',
  templateUrl: './page-not-found-page.html',
  styleUrls: ['./page-not-found-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class PageNotFoundPage {
  data: any;

  constructor(
    private route: ActivatedRoute
  ) {
    this.route.data.subscribe(data => this.data = data);
  }
}