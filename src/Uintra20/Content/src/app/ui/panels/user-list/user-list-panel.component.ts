import { Component, ViewEncapsulation } from '@angular/core';
import { IUserListPanel } from './user-list-panel.interface';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, finalize } from 'rxjs/operators';
import { SearchService } from 'src/app/feature/specific/search/search.service';
import ParseHelper from 'src/app/shared/utils/parse.helper';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'user-list-panel',
  templateUrl: './user-list-panel.html',
  styleUrls: ['./user-list-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class UserListPanel {
  //TODO: Change data interface from any to IUserListPanel once you remove UFP from this panel and remove first three lines in ngOnInit()
  data: any;
  // data: IUserListPanel;
  parsedData: any;

  constructor() {}

  ngOnInit() {
    if (this.data.get) {
      this.data = this.data.get();
    }
    this.parsedData = ParseHelper.parseUbaselineData(this.data);
    this.parsedData.details.members = Object.values(this.parsedData.details.members);
    this.parsedData.details.selectedColumns = Object.values(this.parsedData.details.selectedColumns);
  }
}
