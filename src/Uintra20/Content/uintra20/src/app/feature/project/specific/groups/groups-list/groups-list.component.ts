import { Component, OnInit } from '@angular/core';
import { GroupsService } from '../groups.service';

@Component({
  selector: 'groups-list',
  templateUrl: './groups-list.component.html',
  styleUrls: ['./groups-list.component.less']
})
export class GroupsListComponent implements OnInit {

  constructor(
    private groupsService: GroupsService,
  ) { }

  ngOnInit() {
    this.groupsService.getGroups(false, 1).subscribe(res => {
      console.log(res);
    })
  }

}
