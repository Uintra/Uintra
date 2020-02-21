import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AddButtonService } from '../../main-layout/left-navigation/components/my-links/add-button.service';

@Component({
  selector: 'article-page',
  templateUrl: './article-page.html',
  styleUrls: ['./article-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class ArticlePage {
  data: any;

  constructor(
    private route: ActivatedRoute,
    private addButtonService: AddButtonService
  ) {
    this.route.data.subscribe(data => {
      this.data = data;
      this.addButtonService.setPageId(data.id);
    });
  }
}
