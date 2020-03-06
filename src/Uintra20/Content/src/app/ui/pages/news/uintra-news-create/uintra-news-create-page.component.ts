import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
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
    private route: ActivatedRoute,
    private router: Router,
  ) {
    this.route.data.subscribe(data => {
      if (!data.requiresRedirect.get()) {
        this.data = data;
        this.parsedData = ParseHelper.parseUbaselineData(this.data);
      } else {
        this.router.navigate([data.errorLink.get().originalUrl.get()]);
      }
    });
  }
}
