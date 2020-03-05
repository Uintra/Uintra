import { Component, OnInit, Input } from '@angular/core';
import { GroupsService } from '../groups.service';
import { IBreadcrumbsItem } from '../groups.interface';
import { IUlinkWithTitle } from 'src/app/shared/interfaces/general.interface';

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

  breadcrumbs: IBreadcrumbsItem[];
  data: IGroupDetailsHeaderMapedData;

  constructor(private groupsService: GroupsService) { }

  ngOnInit() {
    this.groupsService.getBreadcrumbs().subscribe((res: IBreadcrumbsItem[]) => {
      this.breadcrumbs = res;
    });
    this.groupsService.getGroupDetailsLinks(this.id).subscribe(res => {
      this.data = {
        title: {link: {...res.groupLinks.groupRoomPage}, title: res.title},
        groupLinks: [
          {link: {...res.groupLinks.groupRoomPage}, title: 'All'},
          {link: {...res.groupLinks.groupDocumentsPage}, title: 'Group Documents'},
          {link: {...res.groupLinks.groupMembersPage}, title: 'Group Members'},
        ],
        groupEditPageLink: res.groupLinks.groupEditPage ? {link: {...res.groupLinks.groupEditPage}, title: 'Settings'} : null,
      }
    });
  }

}
