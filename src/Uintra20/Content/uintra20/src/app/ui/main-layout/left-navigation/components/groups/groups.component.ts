import { Component, OnInit } from '@angular/core';
import { GroupsService, IGroupsData } from './groups.service';

@Component({
  selector: 'left-nav-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.less']
})
export class GroupsComponent implements OnInit {
  data: IGroupsData;
  isOpen: boolean;

  constructor(private groupsService: GroupsService) { }

  ngOnInit() {
    this.groupsService.getGroupsLinks().subscribe(res => {
      this.data = res;
    });
    this.isOpen = this.groupsService.getOpenState();
  }

  onToggle() {
    this.isOpen = !this.isOpen;
    this.groupsService.setOpenState(this.isOpen);
  }
}
