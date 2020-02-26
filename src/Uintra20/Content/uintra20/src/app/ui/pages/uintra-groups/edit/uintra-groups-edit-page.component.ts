import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import ParseHelper from 'src/app/feature/shared/helpers/parse.helper';
import { AddButtonService } from 'src/app/ui/main-layout/left-navigation/components/my-links/add-button.service';
import { Observable } from 'rxjs';
import { HasDataChangedService } from 'src/app/services/general/has-data-changed.service';
import { CanDeactivateGuard } from 'src/app/services/general/can-deactivate.service';
import { RouterResolverService } from 'src/app/services/general/router-resolver.service';

@Component({
  selector: 'uintra-groups-edit-page',
  templateUrl: './uintra-groups-edit-page.html',
  styleUrls: ['./uintra-groups-edit-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class UintraGroupsEditPage {
  data: any;

  constructor(
    private route: ActivatedRoute,
    private addButtonService: AddButtonService,
    private router: Router,
    private hasDataChangedService: HasDataChangedService,
    private canDeactivateService: CanDeactivateGuard,
    private routerResolverService: RouterResolverService,
  ) {
    this.route.data.subscribe(data => {
      this.data = ParseHelper.parseUbaselineData(data);
      this.addButtonService.setPageId(data.id);
      //TODO refactor it
      this.routerResolverService.removePageRouter(this.router.url);
    });
  }

  canDeactivate(): Observable<boolean> | boolean {
    if (this.hasDataChangedService.hasDataChanged) {
      this.canDeactivateService.canDeacrivateConfirm();
    }

    return true;
  }
}
