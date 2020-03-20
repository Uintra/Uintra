import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import ParseHelper from 'src/app/shared/utils/parse.helper';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'search-page',
  templateUrl: './search-page.html',
  styleUrls: ['./search-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class SearchPage {
  data: any;
  parsedData: any;
  inputValue: string = "";
  filters: any[] = [];
  availableTags: any[] = [];
  resultsList: any[] = [];
  isOnlyPinned: boolean = false;
  _query = new Subject<string>();

  constructor(
    private route: ActivatedRoute
  ) {
    this.route.data.subscribe(data => {
      this.data = data;
      this.parsedData = ParseHelper.parseUbaselineData(this.data);
    });
    // this._query.pipe(
    //   debounceTime(200),
    //   distinctUntilChanged(),
    // ).subscribe((value: string) => {
    //   if (value && value.length > 2) {
    //     this.searchService.autocomplete(value).subscribe((res: any[]) => {
    //       this.autocompleteList = res.map(suggestion => ({
    //         ...suggestion,
    //         isActive: false
    //       }));
    //       this.hasResults = res.length > 0;
    //     })
    //   } else {
    //     this.autocompleteList = [];
    //   }
    // })
  }

  ngOnInit() {
    this.availableTags = Object.values(this.parsedData.filterItems).map((item: any) => ({id: item.id, text: item.name}));
    this.resultsList = this.parsedData.results || [];
  }

  onQueryChange(val: string) {
    this.inputValue = val;
    this._query.next(val);
  }

  onTagsChange(val) {

  }
}
