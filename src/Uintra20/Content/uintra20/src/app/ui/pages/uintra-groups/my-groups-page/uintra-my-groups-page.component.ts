import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'uintra-my-groups-page',
  templateUrl: './uintra-my-groups-page.html',
  styleUrls: ['./uintra-my-groups-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class UintraMyGroupsPage {
  data: any;

  constructor(
    private route: ActivatedRoute
  ) {
    this.route.data.subscribe(data => this.data = data);
  }
}