import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'uintra-news-edit-page',
  templateUrl: './uintra-news-edit-page.html',
  styleUrls: ['./uintra-news-edit-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class UintraNewsEditPage {
  data: any;

  constructor(
    private route: ActivatedRoute
  ) {
    this.route.data.subscribe(data => this.data = data);
  }
}
