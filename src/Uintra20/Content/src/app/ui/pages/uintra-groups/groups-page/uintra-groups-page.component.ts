import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AddButtonService } from '../../../main-layout/left-navigation/components/my-links/add-button.service';
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
    private addButtonService: AddButtonService
  ) {
    this.route.data.subscribe(data => {
      this.data = ParseHelper.parseUbaselineData(data);
      this.addButtonService.setPageId(data.id);
    });
  }
}
