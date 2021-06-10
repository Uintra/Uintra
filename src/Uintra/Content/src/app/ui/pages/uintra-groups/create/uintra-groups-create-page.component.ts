import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HasDataChangedService } from 'src/app/shared/services/general/has-data-changed.service';
import { Observable } from 'rxjs';
import { CanDeactivateGuard } from 'src/app/shared/services/general/can-deactivate.service';
import { UintraGroupCreate } from '../../../../shared/interfaces/pages/uintra-groups/create/uintra-groups-create.interface';

@Component({
  selector: 'uintra-groups-create-page',
  templateUrl: './uintra-groups-create-page.html',
  styleUrls: ['./uintra-groups-create-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class UintraGroupsCreatePage {
  public data: UintraGroupCreate;

  constructor(
    private hasDataChangedService: HasDataChangedService,
    private canDeactivateService: CanDeactivateGuard,
    private router: Router,
  ) {}

  ngOnInit(): void {
    //TODO: move this logic to guard
    if (this.data.requiresRedirect) {
      this.router.navigate([this.data.errorLink.originalUrl]);
    }
  }

  canDeactivate(): Observable<boolean> | boolean {
    if (this.hasDataChangedService.hasDataChanged) {
      return this.canDeactivateService.canDeacrivateConfirm();
    }

    return true;
  }
}

