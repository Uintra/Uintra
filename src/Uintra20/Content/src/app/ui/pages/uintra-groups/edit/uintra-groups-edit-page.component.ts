import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AddButtonService } from 'src/app/ui/main-layout/left-navigation/components/my-links/add-button.service';
import { Observable } from 'rxjs';
import { HasDataChangedService } from 'src/app/shared/services/general/has-data-changed.service';
import { CanDeactivateGuard } from 'src/app/shared/services/general/can-deactivate.service';
import { UintraGroupEditInterface } from '../../../../shared/interfaces/pages/uintra-groups/edit/uintra-groups-edit.interface';

@Component({
  selector: 'uintra-groups-edit-page',
  templateUrl: './uintra-groups-edit-page.html',
  styleUrls: ['./uintra-groups-edit-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class UintraGroupsEditPage {
  data: UintraGroupEditInterface;

  constructor(
    private route: ActivatedRoute,
    private addButtonService: AddButtonService,
    private router: Router,
    private hasDataChangedService: HasDataChangedService,
    private canDeactivateService: CanDeactivateGuard,
  ) {
    this.route.data.subscribe(data => {
      if (!data.requiresRedirect) {
        this.data = data;
        this.addButtonService.setPageId(data.id.toString());
      } else {
        this.router.navigate([data.errorLink.originalUrl]);
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
