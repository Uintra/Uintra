import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { CanDeactivateGuard } from 'src/app/shared/services/general/can-deactivate.service';
import { HasDataChangedService } from 'src/app/shared/services/general/has-data-changed.service';
import { UintraNewsCreate } from 'src/app/shared/interfaces/pages/news/create/uintra-news-create.interface';
import { AppService } from 'src/app/app.service';

@Component({
  selector: 'uintra-news-create-page',
  templateUrl: './uintra-news-create-page.html',
  styleUrls: ['./uintra-news-create-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class UintraNewsCreatePage {
  public data: UintraNewsCreate;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private hasDataChangedService: HasDataChangedService,
    private canDeactivateService: CanDeactivateGuard,
    private appService: AppService
  ) {
    this.activatedRoute.data.subscribe((data: UintraNewsCreate) => {
      if (!data.requiresRedirect) {
        this.data = data;
        this.appService.setPageAccess(data.allowAccess);
      } else {
        this.router.navigate([data.errorLink.originalUrl]);
      }
    });
  }

  public canDeactivate(): Observable<boolean> | boolean {
    if (this.hasDataChangedService.hasDataChanged) {
      return this.canDeactivateService.canDeacrivateConfirm();
    }

    return true;
  }
}
