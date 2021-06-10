import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UintraGroupsMembers } from '../../../../shared/interfaces/pages/uintra-groups/members/uintra-groups-members.interface';
import { SearchService } from 'src/app/feature/specific/search/search.service';
import { ResolveService } from 'ubaseline-next-for-uintra';
import { Subscription } from 'rxjs';

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
    private router: Router,
    private searchService: SearchService,
    private resolveService: ResolveService,
  ) {}

  ngOnInit() {
    this.refreshSubscription = this.searchService.groupMembersRefreshTrigger.subscribe(async (res: UintraGroupsMembers) => {
      this.data = await this.resolveService.resolveDataOnSameUrl(this.router.url);
    })
  }

  ngOnDestroy() {
    this.refreshSubscription.unsubscribe();
  }
}
