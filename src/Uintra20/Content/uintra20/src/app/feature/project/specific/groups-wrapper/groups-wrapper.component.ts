import { Component, OnInit } from '@angular/core';
import { GroupsService } from 'src/app/ui/main-layout/left-navigation/components/groups/groups.service';

@Component({
  selector: 'app-groups-wrapper',
  templateUrl: './groups-wrapper.component.html',
  styleUrls: ['./groups-wrapper.component.less']
})
export class GroupsWrapperComponent implements OnInit {
  tabs: any;

  constructor(private groupsService: GroupsService) { }

  ngOnInit() {
    this.groupsService.getGroupsLinks().subscribe(res => {
      this.tabs = [res.groupPageItem, ...res.items];
    })
  }

}
