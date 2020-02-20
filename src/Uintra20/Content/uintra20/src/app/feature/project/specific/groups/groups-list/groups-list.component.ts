import { Component, OnInit, NgZone, Input } from '@angular/core';
import { GroupsService } from '../groups.service';
import { IGroupsListItem } from '../groups.interface';

@Component({
  selector: 'groups-list',
  templateUrl: './groups-list.component.html',
  styleUrls: ['./groups-list.component.less']
})
export class GroupsListComponent implements OnInit {
  @Input('myGroups') myGroups: any;
  groups: IGroupsListItem[] = [];
  currentPage: number = 1;
  isGroupsLoading: boolean;
  isScrollDisabled: boolean;

  constructor(
    private groupsService: GroupsService,
    private ngZone: NgZone,
  ) { }

  ngOnInit() {
    this.myGroups = this.myGroups !== undefined;
    this.getGroups();
  }

  getGroups(): void {
    this.isGroupsLoading = true;

    this.groupsService
      .getGroups(this.myGroups, this.currentPage)
      .then((response: IGroupsListItem[]) => {
        this.isScrollDisabled = response.length === 0;
        this.concatWithCurrentList(response);
      })
      .finally(() => {
        this.isGroupsLoading = false;
      });
  }

  concatWithCurrentList(data): void {
    this.ngZone.run(() => {
      this.groups = this.groups.concat(data);
    });
  }

  onLoadMore(): void {
    this.currentPage += 1;
    this.getGroups();
  }

  onScroll(): void {
    this.onLoadMore();
  }
}
