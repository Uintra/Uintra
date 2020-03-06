import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import ParseHelper from 'src/app/shared/utils/parse.helper';

@Component({
  selector: 'uintra-news-create-page',
  templateUrl: './uintra-news-create-page.html',
  styleUrls: ['./uintra-news-create-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class UintraNewsCreatePage {
  data: any;
  parsedData: any;

  constructor(
    private route: ActivatedRoute
  ) {
    this.route.data.subscribe(data => {
      this.data = data;
      this.parsedData = ParseHelper.parseUbaselineData(this.data);
    });
  }
}
