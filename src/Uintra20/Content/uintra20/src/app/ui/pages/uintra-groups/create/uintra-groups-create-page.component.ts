import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import ParseHelper from 'src/app/feature/shared/helpers/parse.helper';
import { AddButtonService } from 'src/app/ui/main-layout/left-navigation/components/my-links/add-button.service';

@Component({
  selector: 'uintra-groups-create-page',
  templateUrl: './uintra-groups-create-page.html',
  styleUrls: ['./uintra-groups-create-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class UintraGroupsCreatePage {
  data: any;

  constructor(private route: ActivatedRoute, private addButtonService: AddButtonService) {
    this.route.data.subscribe(data => {
      this.data = ParseHelper.parseUbaselineData(data);
      this.addButtonService.setPageId(data.id);
    });
  }
}
