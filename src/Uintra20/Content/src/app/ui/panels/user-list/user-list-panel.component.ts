import { Component, ViewEncapsulation } from '@angular/core';
import { IUserListPanel } from './user-list-panel.interface';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { SearchService } from 'src/app/feature/specific/search/search.service';

@Component({
  selector: 'user-list-panel',
  templateUrl: './user-list-panel.html',
  styleUrls: ['./user-list-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class UserListPanel {
  data: IUserListPanel;
  inputValue: string = "";
  currentPage: number = 1;

  _query = new Subject<string>();

  constructor(
    private searchService: SearchService,
  ) {
    this._query.pipe(
      debounceTime(200),
      distinctUntilChanged(),
    ).subscribe((value: string) => {
      this.searchService.userListSearch(this.requestDataBuilder()).subscribe((res: any[]) => {
        // this.autocompleteList = res.map(suggestion => ({
        //   ...suggestion,
        //   isActive: false
        // }));
        // this.hasResults = res.length > 0;
      })
    })
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
