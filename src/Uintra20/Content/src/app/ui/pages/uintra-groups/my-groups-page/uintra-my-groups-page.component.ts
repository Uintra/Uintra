import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UintraMyGroupsInterface } from 'src/app/shared/interfaces/pages/uintra-groups/my/uintra-my-groups.interface';

@Component({
  selector: 'uintra-my-groups-page',
  templateUrl: './uintra-my-groups-page.html',
  styleUrls: ['./uintra-my-groups-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class UintraMyGroupsPage {

  public data: UintraMyGroupsInterface;

  constructor(
    private route: ActivatedRoute,

  ) {
    this.activatedRoute.data.subscribe((data: UintraMyGroupsInterface) => {
      this.data = data;
    });
  }
}
