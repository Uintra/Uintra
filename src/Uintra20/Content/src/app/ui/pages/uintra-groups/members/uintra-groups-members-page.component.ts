import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UintraGroupsMembersInterface } from '../../../../shared/interfaces/pages/uintra-groups/members/uintra-groups-members.interface';

@Component({
  selector: 'uintra-groups-members-page',
  templateUrl: './uintra-groups-members-page.html',
  styleUrls: ['./uintra-groups-members-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class UintraGroupsMembersPage {
  public data: UintraGroupsMembersInterface;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
  ) {
    this.activatedRoute.data.subscribe((data: UintraGroupsMembersInterface) => {
      if (!data.requiresRedirect) {
        this.data = data;
      } else {
        this.router.navigate([data.errorLink.originalUrl]);
      }
    });
  }
}
