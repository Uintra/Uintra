import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UintraGroupsMembers } from '../../../../shared/interfaces/pages/uintra-groups/members/uintra-groups-members.interface';
import { SearchService } from 'src/app/feature/specific/search/search.service';
import { ResolveService } from 'ubaseline-next-for-uintra';
import { Subscription } from 'rxjs';
import { AppService } from 'src/app/app.service';

@Component({
  selector: 'uintra-groups-members-page',
  templateUrl: './uintra-groups-members-page.html',
  styleUrls: ['./uintra-groups-members-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class UintraGroupsMembersPage {
  public data: UintraGroupsMembers;
  private refreshSubscription: Subscription;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private searchService: SearchService,
    private resolveService: ResolveService,
    private appService: AppService
  ) {
    this.activatedRoute.data.subscribe((data: UintraGroupsMembers) => {
      this.data = data;
      this.appService.setPageAccess(data.allowAccess);
    });
  }

  ngOnInit() {
    this.refreshSubscription = this.searchService.groupMembersRefreshTrigger.subscribe(async (res: UintraGroupsMembers) => {
      this.data = await this.resolveService.resolveDataOnSameUrl(this.router.url);
    })
  }

  ngOnDestroy() {
    this.refreshSubscription.unsubscribe();
  }
}
