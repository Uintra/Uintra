import { Component, OnInit, Input } from '@angular/core';
import { IGroupsData } from 'src/app/feature/specific/groups/groups.interface';
import { GroupsService } from 'src/app/feature/specific/groups/groups.service';

@Component({
  selector: 'left-nav-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.less']
})
export class GroupsComponent implements OnInit {
  @Input() data: IGroupsData;
  isOpen: boolean;

  constructor(private groupsService: GroupsService) { }

  ngOnInit() {
    this.isOpen = this.groupsService.getOpenState();
  }

  onToggle() {
    this.isOpen = !this.isOpen;
    this.groupsService.setOpenState(this.isOpen);
  }
}
