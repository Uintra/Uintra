import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CanDeactivateGuard } from 'src/app/shared/services/general/can-deactivate.service';
import { HasDataChangedService } from 'src/app/shared/services/general/has-data-changed.service';
import { Observable } from 'rxjs';
import { AddButtonService } from '../../main-layout/left-navigation/components/my-links/add-button.service';
import { IArcticlePage } from 'src/app/shared/interfaces/pages/article/article-page.interface';

@Component({
  selector: 'article-page',
  templateUrl: './article-page.html',
  styleUrls: ['./article-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class ArticlePage {

  public data: IArcticlePage;

  constructor(
    private activatedRoute: ActivatedRoute,
    private addButtonService: AddButtonService,
    private hasDataChangedService: HasDataChangedService,
    private canDeactivateService: CanDeactivateGuard,
  ) {
    this.activatedRoute.data.subscribe((data: IArcticlePage) => {
      this.data = data;
      this.addButtonService.setPageId(data.id.toString());
    });
  }

  public canDeactivate(): Observable<boolean> | boolean {
    if (this.hasDataChangedService.hasDataChanged) {
      return this.canDeactivateService.canDeacrivateConfirm();
    }

    return true;
  }
}
