import { Component, OnInit } from '@angular/core';
import { IUlinkWithTitle } from 'src/app/shared/interfaces/general.interface';
import { IGroupsData } from '../groups.interface';
import { GroupsService } from '../groups.service';

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
