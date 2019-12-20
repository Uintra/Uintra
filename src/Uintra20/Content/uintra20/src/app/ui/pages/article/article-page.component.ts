import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'article-page',
  templateUrl: './article-page.html',
  styleUrls: ['./article-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class ArticlePage {
  data: any;

  constructor(
    private route: ActivatedRoute
  ) {
    this.route.data.subscribe(data => this.data = data);

    debugger;
  }
}
