import { Component, OnInit } from '@angular/core';
import { GroupsService, IGroupsdata } from 'src/app/ui/main-layout/left-navigation/components/groups/groups.service';
import { IUlinkWithTitle } from 'src/app/feature/shared/interfaces/IULink';

@Component({
  selector: 'app-groups-wrapper',
  templateUrl: './groups-wrapper.component.html',
  styleUrls: ['./groups-wrapper.component.less']
})
export class GroupsWrapperComponent implements OnInit {
  tabs: IUlinkWithTitle[];

  constructor(private groupsService: GroupsService) { }

  ngOnInit() {
    this.groupsService.getGroupsLinks().subscribe((res: IGroupsdata) => {
      this.tabs = [res.groupPageItem, ...res.items];
    })
  }

}
