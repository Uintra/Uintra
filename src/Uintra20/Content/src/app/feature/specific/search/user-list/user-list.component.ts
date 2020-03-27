import { Component, OnInit, Input } from '@angular/core';
import { SearchService } from '../search.service';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, finalize } from 'rxjs/operators';
import { IUserListData } from '../search.interface';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.less']
})
export class UserListComponent implements OnInit {
  @Input() data: IUserListData;
  inputValue: string = "";
  currentPage: number = 1;
  canLoadMore: boolean;
  isNameColumn: boolean;
  isInfoColumn: boolean;
  isGroupColumn: boolean;
  isDeleteColumn: boolean;
  isLoadMoreDisabled: boolean;
  isNoMembers: boolean;

  _query = new Subject<string>();

  constructor(
    private searchService: SearchService,
  ) {
    this._query.pipe(
      debounceTime(200),
      distinctUntilChanged(),
    ).subscribe((value: string) => {
      this.currentPage = 1;
      this.data.details.members = [];
      this.canLoadMore = false;
      this.getMembers();
    })
  }

  ngOnInit() {
    this.getColumns();
    this.canLoadMore = !this.data.details.isLastRequest;
  }

  onQueryChange(val) {
    this.inputValue = val;
    this._query.next(val);
  }

  getMembers() {
    this.isLoadMoreDisabled = true;
    this.isNoMembers = false;

    this.searchService.userListSearch(this.requestDataBuilder()).pipe(
      finalize(() => {
        this.isLoadMoreDisabled = false;
      })
    ).subscribe((res: any) => {
      this.data.details.members = this.currentPage == 1 ? res.members : this.data.details.members.concat(res.members);
      this.data.details.selectedColumns = res.selectedColumns;
      this.getColumns();
      this.canLoadMore = !res.isLastRequest;
      this.isNoMembers = this.data.details.members.length == 0;
    })
  }

  loadMore() {
    this.currentPage += 1;
    this.getMembers();
  }

  getColumns() {
    this.isNameColumn = this.data.details.selectedColumns.findIndex(column => column.alias == 'Name') !== -1;
    this.isInfoColumn = this.data.details.selectedColumns.findIndex(column => column.alias == 'Info') !== -1;
    this.isGroupColumn = this.data.details.selectedColumns.findIndex(column => column.alias == 'Group') !== -1;
    this.isDeleteColumn = this.data.details.selectedColumns.findIndex(column => column.alias == 'Management') !== -1;
  }

  requestDataBuilder() {
    return {
      text: this.inputValue,
      page: this.currentPage,
      groupId: this.data.details.groupId || null,
      orderingString: null,
      isInvite: null
    }
  }

  changeMemberStatus(memberId: string, select: HTMLSelectElement) {
    const selectedMember = this.data.details.members.find(member => member.member.id == memberId);
    const requestData = {memberId: memberId, groupId: this.data.details.groupId};

    this.searchService.changeMemberStatus(requestData).subscribe(res => {
      selectedMember.isGroupAdmin = !selectedMember.isGroupAdmin;
      select.value = selectedMember.isGroupAdmin ? '1' : '0';
    },
    (error) => {
      select.value = selectedMember.isGroupAdmin ? '1' : '0';
    });
  }

  deleteMember(userId: string) {
    const requestData = {userId: userId, groupId: this.data.details.groupId};

    this.searchService.deleteMember(requestData).subscribe(res => {
      console.log(res);
    })
  }
}
