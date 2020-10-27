import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UintraGroups } from '../../../../shared/interfaces/pages/uintra-groups/uintra-groups.interface';

@Component({
  selector: 'uintra-groups-page',
  templateUrl: './uintra-groups-page.html',
  styleUrls: ['./uintra-groups-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class UintraGroupsPage {
  public data: UintraGroups;

  constructor(
    private activatedRoute: ActivatedRoute,
  ) {
    this.activatedRoute.data.subscribe((data: UintraGroups) => {
      this.data = data;
    });
  }
}
