import { Component, OnInit, Input } from '@angular/core';
import { IGroupsData, IBreadcrumbsItem } from '../groups.interface';
import { IUlinkWithTitle } from 'src/app/shared/interfaces/general.interface';
import { GroupsService } from '../groups.service';

@Component({
  selector: 'app-groups-wrapper',
  templateUrl: './groups-wrapper.component.html',
  styleUrls: ['./groups-wrapper.component.less']
})
export class GroupsWrapperComponent implements OnInit {

  @Input() public data: IGroupsData;
  public breadcrumbs: IBreadcrumbsItem[];
  public tabs: IUlinkWithTitle[];

  constructor(private groupsService: GroupsService) { }

  public ngOnInit(): void {
    this.groupsService.getBreadcrumbs().subscribe((res: IBreadcrumbsItem[]) => {
      this.breadcrumbs = res;
    });
    this.tabs = [this.data.groupPageItem, ...this.data.items];
  }
}
