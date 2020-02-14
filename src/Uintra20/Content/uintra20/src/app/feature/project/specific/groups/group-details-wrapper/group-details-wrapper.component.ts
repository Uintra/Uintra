import { Component, OnInit, Input } from '@angular/core';
import { GroupsService } from '../groups.service';

@Component({
  selector: 'app-group-details-wrapper',
  templateUrl: './group-details-wrapper.component.html',
  styleUrls: ['./group-details-wrapper.component.less']
})
export class GroupDetailsWrapperComponent implements OnInit {
  @Input() id: string;

  data: any;

  constructor(private groupsService: GroupsService) { }

  ngOnInit() {
    this.groupsService.getGroupDetailsLinks(this.id).subscribe(res => {
      console.log(res);
      this.data = {
        title: res.title,
        groupLinks: [
          {...res.groupLinks.groupRoomPage, title: 'All'},
          {...res.groupLinks.groupDocumentsPage, title: 'Group Documents'},
          {...res.groupLinks.groupMembersPage, title: 'Group Members'},
        ],
        groupEditPageLink: {...res.groupLinks.groupEditPage, title: 'Settings'},
      }
    })
  }

}
