import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import ParseHelper from 'src/app/shared/utils/parse.helper';

@Component({
  selector: 'uintra-groups-page',
  templateUrl: './uintra-groups-page.html',
  styleUrls: ['./uintra-groups-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class UintraGroupsPage {
  data: any;

  constructor(
    private route: ActivatedRoute,
  ) {
    this.route.data.subscribe(data => {
      this.data = ParseHelper.parseUbaselineData(data);
    });
  }
}
