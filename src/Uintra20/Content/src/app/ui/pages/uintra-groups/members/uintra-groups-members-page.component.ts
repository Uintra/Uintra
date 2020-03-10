import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import ParseHelper from 'src/app/shared/utils/parse.helper';
import { AddButtonService } from 'src/app/ui/main-layout/left-navigation/components/my-links/add-button.service';

@Component({
  selector: 'uintra-groups-members-page',
  templateUrl: './uintra-groups-members-page.html',
  styleUrls: ['./uintra-groups-members-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class UintraGroupsMembersPage {
  data: any;

  constructor(
    private route: ActivatedRoute,
    private addButtonService: AddButtonService,
    private router: Router,
  ) {
    this.route.data.subscribe(data => {
      if (!data.requiresRedirect.get()) {
        this.data = ParseHelper.parseUbaselineData(data);
        this.addButtonService.setPageId(data.id.get());
      } else {
        this.router.navigate([data.errorLink.get().originalUrl.get()]);
      }
    });
  }
}
