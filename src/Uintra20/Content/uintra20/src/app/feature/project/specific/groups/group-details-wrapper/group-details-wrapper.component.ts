import { Component, OnInit, Input } from '@angular/core';
import { GroupsService } from '../groups.service';
import { IUlinkWithTitle } from 'src/app/feature/shared/interfaces/IULink';

export interface IGroupDetailsHeaderMapedData {
  title: IUlinkWithTitle;
  groupLinks: IUlinkWithTitle[];
  groupEditPageLink: IUlinkWithTitle;
}

@Component({
  selector: 'app-group-details-wrapper',
  templateUrl: './group-details-wrapper.component.html',
  styleUrls: ['./group-details-wrapper.component.less']
})
export class GroupDetailsWrapperComponent implements OnInit {
  @Input() id: string;

  data: IGroupDetailsHeaderMapedData;

  constructor(private groupsService: GroupsService) { }

  ngOnInit() {
    this.groupsService.getGroupDetailsLinks(this.id).subscribe(res => {
      this.data = {
        title: {link: {...res.groupLinks.groupRoomPage}, title: res.title},
        groupLinks: [
          {link: {...res.groupLinks.groupRoomPage}, title: 'All'},
          {link: {...res.groupLinks.groupDocumentsPage}, title: 'Group Documents'},
          {link: {...res.groupLinks.groupMembersPage}, title: 'Group Members'},
        ],
        groupEditPageLink: {link: {...res.groupLinks.groupEditPage}, title: 'Settings'},
      }
    })
  }

}
