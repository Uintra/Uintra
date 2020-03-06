import { Component, OnInit, Input } from '@angular/core';
import { IGroupsData, IBreadcrumbsItem } from '../groups.interface';
import { IUlinkWithTitle } from 'src/app/shared/interfaces/general.interface';
import { GroupsService } from '../groups.service';

@Component({
  selector: 'app-groups-wrapper',
  templateUrl: './groups-wrapper.component.html',
  styleUrls: ['./groups-wrapper.component.less']
})
export class GroupsWrapperComponent implements OnInit {
  @Input() data: IGroupsData;
  breadcrumbs: IBreadcrumbsItem[];
  tabs: IUlinkWithTitle[];

  constructor(private groupsService: GroupsService) { }

  ngOnInit() {
    this.groupsService.getBreadcrumbs().subscribe((res: IBreadcrumbsItem[]) => {
      this.breadcrumbs = res;
    })
    this.tabs = [this.data.groupPageItem, ...Object.values(this.data.items)];
  }

}
