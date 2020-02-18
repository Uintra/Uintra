import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import ParseHelper from 'src/app/feature/shared/helpers/parse.helper';

@Component({
  selector: 'uintra-groups-room-page',
  templateUrl: './uintra-groups-room-page.html',
  styleUrls: ['./uintra-groups-room-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class UintraGroupsRoomPage {
  data: any;
  parsedData: any;
  leftPanels: any;

  constructor(
    private route: ActivatedRoute
  ) {
    this.route.data.subscribe(data => {
      this.data = data;
      this.parsedData = ParseHelper.parseUbaselineData(data);
    });
  }
}
