import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CanDeactivateGuard } from 'src/app/shared/services/general/can-deactivate.service';
import { HasDataChangedService } from 'src/app/shared/services/general/has-data-changed.service';
import { Observable } from 'rxjs';
import { IArcticlePage, ISubNavigation } from 'src/app/shared/interfaces/pages/article/article-page.interface';

@Component({
  selector: 'article-page',
  templateUrl: './article-page.html',
  styleUrls: ['./article-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class ArticlePage implements OnInit {

  public data: IArcticlePage;
  public subNavigation: ISubNavigation[];

  constructor(
    private hasDataChangedService: HasDataChangedService,
    private canDeactivateService: CanDeactivateGuard,
  ) {}

  public ngOnInit(): void {
    this.setSubNavigation();
  }

  public canDeactivate(): Observable<boolean> | boolean {
    if (this.hasDataChangedService.hasDataChanged) {
      return this.canDeactivateService.canDeacrivateConfirm();
    }

    return true;
  }

  private setSubNavigation(): void {
    const isRootNode = this.data.subNavigation.currentItem;
    if (isRootNode) {
      this.subNavigation = this.data.subNavigation.subItems;
      return;
    }
    this.findSubNavigation(this.data.subNavigation);
  }

  private findSubNavigation(subNavigation: ISubNavigation): void {
    const hasCurrentChild = subNavigation.subItems.some(item => item.currentItem);
    if (hasCurrentChild) {
      this.subNavigation = subNavigation.subItems;
      return;
    }
    subNavigation.subItems.forEach(item => {
      this.findSubNavigation(item);
    })
  }
}
