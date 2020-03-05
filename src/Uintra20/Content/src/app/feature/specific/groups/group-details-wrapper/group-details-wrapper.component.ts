import { Component, OnInit, Input } from '@angular/core';
import { GroupsService } from '../groups.service';
import { IUlinkWithTitle } from 'src/app/shared/interfaces/general.interface';
import { IGroupDetailsHeaderData } from '../groups.interface';

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
  @Input() data: IGroupDetailsHeaderData;

  mapedData: IGroupDetailsHeaderMapedData;

  constructor(private groupsService: GroupsService) { }

  ngOnInit() {
      this.mapedData = {
        title: {link: {...this.data.groupLinks.groupRoomPage}, title: this.data.title},
        groupLinks: [
          {link: {...this.data.groupLinks.groupRoomPage}, title: 'All'},
          {link: {...this.data.groupLinks.groupDocumentsPage}, title: 'Group Documents'},
          {link: {...this.data.groupLinks.groupMembersPage}, title: 'Group Members'},
        ],
        groupEditPageLink: this.data.groupLinks.groupEditPage ? {link: {...this.data.groupLinks.groupEditPage}, title: 'Settings'} : null,
      }
  }

}
