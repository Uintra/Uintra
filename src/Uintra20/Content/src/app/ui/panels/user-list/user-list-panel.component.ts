import { Component, ViewEncapsulation } from '@angular/core';
import { IUserListPanel } from './user-list-panel.interface';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, finalize } from 'rxjs/operators';
import { SearchService } from 'src/app/feature/specific/search/search.service';
import ParseHelper from 'src/app/shared/utils/parse.helper';

@Component({
  selector: 'user-list-panel',
  templateUrl: './user-list-panel.html',
  styleUrls: ['./user-list-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class UserListPanel {
  data: IUserListPanel;
  parsedData: any;
  inputValue: string = "";
  currentPage: number = 1;
  members: any[] = [];
  selectedColumns: any[] = [];
  canLoadMore: boolean;
  isName: boolean;
  isInfo: boolean;
  isLoadMoreDisabled: boolean;

  _query = new Subject<string>();

  constructor(
    private searchService: SearchService,
  ) {
    this._query.pipe(
      debounceTime(200),
      distinctUntilChanged(),
    ).subscribe((value: string) => {
      this.currentPage = 1;
      this.members = [];
      this.canLoadMore = false;
      this.getMembers();
    })
  }

  ngOnInit() {
    this.parsedData = ParseHelper.parseUbaselineData(this.data);
    this.members = Object.values(this.parsedData.details.members);
    this.selectedColumns = Object.values(this.parsedData.details.selectedColumns);
    this.getColumns();
    this.canLoadMore = !this.parsedData.details.isLastRequest;
  }

  onQueryChange(val) {
    this.inputValue = val;
    this._query.next(val);
  }

  getMembers() {
    this.isLoadMoreDisabled = true;

    this.searchService.userListSearch(this.requestDataBuilder()).pipe(
      finalize(() => {
        this.isLoadMoreDisabled = false;
      })
    ).subscribe((res: any) => {
      this.members = this.members.concat(res.members);
      this.selectedColumns = res.selectedColumns;
      this.getColumns();
      this.canLoadMore = !res.isLastRequest;
    })
  }

  loadMore() {
    this.currentPage += 1;
    this.getMembers();
  }

  getColumns() {
    this.isName = this.selectedColumns.indexOf(column => column.name == 'Name') !== -1;
    this.isInfo = this.selectedColumns.indexOf(column => column.name == 'Info') !== -1;
  }

  requestDataBuilder() {
    return {
      text: this.inputValue,
      page: this.currentPage,
      groupId: null,
      orderingString: null,
      isInvite: null
    }
  }
}
