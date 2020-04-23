import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import ParseHelper from 'src/app/shared/utils/parse.helper';

@Component({
  selector: 'uintra-groups-members-page',
  templateUrl: './uintra-groups-members-page.html',
  styleUrls: ['./uintra-groups-members-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class UintraGroupsMembersPage {
  data: any;
  parsedData: any;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
  ) {
    this.route.data.subscribe(data => {
      if (!data.requiresRedirect.get()) {
        this.data = data;
        this.parsedData = ParseHelper.parseUbaselineData(data);
      } else {
        this.router.navigate([data.errorLink.get().originalUrl.get()]);
      }
    });
  }
}
