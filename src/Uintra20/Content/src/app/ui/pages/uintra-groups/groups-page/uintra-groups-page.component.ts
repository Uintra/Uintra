import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AddButtonService } from '../../../main-layout/left-navigation/components/my-links/add-button.service';
import { UintraGroupsInterface } from '../../../../shared/interfaces/pages/uintra-groups/uintra-groups.interface';

@Component({
  selector: 'uintra-groups-page',
  templateUrl: './uintra-groups-page.html',
  styleUrls: ['./uintra-groups-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class UintraGroupsPage {
  public data: UintraGroupsInterface;

  constructor(
    private activatedRoute: ActivatedRoute,
    private addButtonService: AddButtonService
  ) {
    this.activatedRoute.data.subscribe((data: UintraGroupsInterface) => {
      this.data = data;
      this.addButtonService.setPageId(data.id.toString());
    });
  }
}
