import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import ParseHelper from 'src/app/shared/utils/parse.helper';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { SearchService } from 'src/app/feature/specific/search/search.service';
import { ISearchRequestData, IFilterData } from 'src/app/feature/specific/search/search.interface';

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
  availableFilters: IFilterData[] = [];
  selectedFilters: IFilterData[] = [];
  resultsList: any[] = [];
  isOnlyPinned: boolean = false;
  currentPage: number = 1;
  _query = new Subject<string>();

  constructor(
    private route: ActivatedRoute,
    private searchService: SearchService,
  ) {
    this.route.data.subscribe(data => {
      this.data = data;
      this.parsedData = ParseHelper.parseUbaselineData(this.data);
    });
    this._query.pipe(
      debounceTime(200),
      distinctUntilChanged(),
    ).subscribe(value => {
      this.searchService.search(this.requestDataBuilder()).subscribe((res: any) => {
        this.resultsList = res.results;
      })
    })
  }

  ngOnInit() {
    this.availableFilters = Object.values(this.parsedData.filterItems).map((item: any) => ({id: item.id, text: item.name}));
    this.resultsList = this.parsedData.results || [];
  }

  onQueryChange(val: string) {
    this.inputValue = val;
    this._query.next('query');
  }

  onTagsChange(val) {
    this.selectedFilters = val;
    this._query.next('tags');
  }

  onCbxChange() {
    this._query.next('cbx');
  }

  requestDataBuilder(): ISearchRequestData {
    return {
      query: this.inputValue,
      page: this.currentPage,
      types: this.selectedFilters.map(filter => filter.id),
      onlyPinned: this.isOnlyPinned
    }
  }
}
