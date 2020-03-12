import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import ParseHelper from 'src/app/shared/utils/parse.helper';

@Component({
  selector: 'event-create-page',
  templateUrl: './event-create-page.html',
  styleUrls: ['./event-create-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class EventCreatePage {
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
