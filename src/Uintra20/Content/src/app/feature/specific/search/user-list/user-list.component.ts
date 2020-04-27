import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { SearchService } from '../search.service';
import { Subject, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged, finalize } from 'rxjs/operators';
import { IUserListData } from '../search.interface';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.less']
})
export class UserListComponent implements OnInit, OnDestroy {

  private $searchSubscription: Subscription;
  private $changeMemberStatusSubscription: Subscription;
  private $deleteMemberSubscription: Subscription;

  @Input()
  public data: IUserListData;
  public inputValue = '';
  public currentPage = 1;
  public canLoadMore: boolean;
  public isNameColumn: boolean;
  public isInfoColumn: boolean;
  public isGroupColumn: boolean;
  public isDeleteColumn: boolean;
  public isLoadMoreDisabled: boolean;
  public isNoMembers: boolean;

  public _query = new Subject<string>();

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
    });
  }
  public ngOnDestroy(): void {
    if (this.$searchSubscription) { this.$searchSubscription.unsubscribe(); }
    if (this.$changeMemberStatusSubscription) { this.$changeMemberStatusSubscription.unsubscribe(); }
    if (this.$deleteMemberSubscription) { this.$deleteMemberSubscription.unsubscribe(); }
  }

  public ngOnInit(): void {
    this.getColumns();
    this.canLoadMore = !this.data.details.isLastRequest;
  }

  public onQueryChange(val): void {
    this.inputValue = val;
    this._query.next(val);
  }

  public getMembers(): void {
    this.isLoadMoreDisabled = true;
    this.isNoMembers = false;

    this.$searchSubscription = this.searchService.userListSearch(this.requestDataBuilder()).pipe(
      finalize(() => {
        this.isLoadMoreDisabled = false;
      })
    ).subscribe((res: any) => {
      this.data.details.members = this.currentPage === 1
        ? res.members
        : this.data.details.members.concat(res.members);

      this.data.details.selectedColumns = res.selectedColumns;
      // this.getColumns();
      this.canLoadMore = !res.isLastRequest;
      this.isNoMembers = this.data.details.members.length === 0;
    });
  }

  public loadMore() {
    this.currentPage += 1;
    this.getMembers();
  }

  public getColumns() {
    this.isNameColumn = this.data.details.selectedColumns.findIndex(column => column.name === 'Name') !== -1;
    this.isInfoColumn = this.data.details.selectedColumns.findIndex(column => column.name === 'Info') !== -1;
    this.isGroupColumn = this.data.details.selectedColumns.findIndex(column => column.name === 'Group') !== -1;
    this.isDeleteColumn = this.data.details.selectedColumns.findIndex(column => column.name === 'Management') !== -1;
  }

  public requestDataBuilder() {
    return {
      text: this.inputValue,
      page: this.currentPage,
      groupId: this.data.details.groupId || null,
      orderingString: null,
      isInvite: null
    };
  }

  public changeMemberStatus(memberId: string, select: HTMLSelectElement) {
    const selectedMember = this.data.details.members.find(member => member.member.id === memberId);
    const requestData = { memberId, groupId: this.data.details.groupId };

    this.$changeMemberStatusSubscription = this.searchService.changeMemberStatus(requestData).subscribe(res => {
      selectedMember.isGroupAdmin = !selectedMember.isGroupAdmin;
      select.value = selectedMember.isGroupAdmin ? '1' : '0';
    },
      (error) => {
        select.value = selectedMember.isGroupAdmin ? '1' : '0';
      });
  }

  public deleteMember(userId: string) {
    const requestData = { userId, groupId: this.data.details.groupId };

    this.$deleteMemberSubscription = this.searchService.deleteMember(requestData).subscribe(res => {
      this.data.details.members = this.data.details.members.filter(member => member.member.id !== userId);
    });
  }

  public index = (index, item) => {
    return index;
  }
}
