import { Component, OnInit } from '@angular/core';
import { GroupsService, IGroupsData } from 'src/app/feature/project/specific/groups/groups.service';
import { IUlinkWithTitle } from 'src/app/feature/shared/interfaces/general.interface';

@Component({
  selector: 'app-groups-wrapper',
  templateUrl: './groups-wrapper.component.html',
  styleUrls: ['./groups-wrapper.component.less']
})
export class GroupsWrapperComponent implements OnInit {
  tabs: IUlinkWithTitle[];

  constructor(private groupsService: GroupsService) { }

  ngOnInit() {
    this.groupsService.getGroupsLinks().subscribe((res: IGroupsData) => {
      this.tabs = [res.groupPageItem, ...res.items];
    })
  }

}
