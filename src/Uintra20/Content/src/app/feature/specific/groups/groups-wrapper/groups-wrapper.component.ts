import { Component, OnInit } from '@angular/core';
import { IGroupsData, IBreadcrumbsItem } from '../groups.interface';
import { IUlinkWithTitle } from 'src/app/shared/interfaces/general.interface';
import { GroupsService } from '../groups.service';

@Component({
  selector: 'app-groups-wrapper',
  templateUrl: './groups-wrapper.component.html',
  styleUrls: ['./groups-wrapper.component.less']
})
export class GroupsWrapperComponent implements OnInit {
  breadcrumbs: IBreadcrumbsItem[];
  tabs: IUlinkWithTitle[];

  constructor(private groupsService: GroupsService) { }

  ngOnInit() {
    this.groupsService.getBreadcrumbs().subscribe((res: IBreadcrumbsItem[]) => {
      this.breadcrumbs = res;
    })
    this.groupsService.getGroupsLinks().subscribe((res: IGroupsData) => {
      this.tabs = [res.groupPageItem, ...res.items];
    })
  }

}
