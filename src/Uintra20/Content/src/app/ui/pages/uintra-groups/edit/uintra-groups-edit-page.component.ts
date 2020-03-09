import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import ParseHelper from 'src/app/shared/utils/parse.helper';
import { AddButtonService } from 'src/app/ui/main-layout/left-navigation/components/my-links/add-button.service';
import { Observable } from 'rxjs';
import { HasDataChangedService } from 'src/app/shared/services/general/has-data-changed.service';
import { CanDeactivateGuard } from 'src/app/shared/services/general/can-deactivate.service';
import { RouterResolverService } from 'src/app/shared/services/general/router-resolver.service';

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
      if (!data.requiresRedirect.get()) {
        this.data = ParseHelper.parseUbaselineData(data);
        this.addButtonService.setPageId(data.id);
      } else {
        this.router.navigate([data.errorLink.get().originalUrl.get()]);
      }
    });
  }

  canDeactivate(): Observable<boolean> | boolean {
    if (this.hasDataChangedService.hasDataChanged) {
      return this.canDeactivateService.canDeacrivateConfirm();
    }

    return true;
  }
}
