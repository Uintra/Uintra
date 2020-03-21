import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import ParseHelper from 'src/app/shared/utils/parse.helper';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, finalize } from 'rxjs/operators';
import { SearchService } from 'src/app/feature/specific/search/search.service';
import { ISearchRequestData, IMapedFilterData, ISearchResult, ISearchData } from 'src/app/feature/specific/search/search.interface';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'search-page',
  templateUrl: './search-page.html',
  styleUrls: ['./search-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class SearchPage {
  data: any;
  parsedData: ISearchData;
  inputValue: string = "";
  availableFilters: IMapedFilterData[] = [];
  selectedFilters: IMapedFilterData[] = [];
  resultsList: ISearchResult[] = [];
  isOnlyPinned: boolean = false;
  currentPage: number = 1;
  query: string = "";
  resultsCount: number;
  isResultsLoading: boolean = false;
  isScrollDisabled: boolean = false;
  _query = new Subject<string>();

  constructor(
    private route: ActivatedRoute,
    private searchService: SearchService,
    private translate: TranslateService,
  ) {
    this.route.data.subscribe(data => {
      this.data = data;
      this.parsedData = ParseHelper.parseUbaselineData(this.data);
    });
    this._query.pipe(
      debounceTime(200),
      distinctUntilChanged(),
    ).subscribe(value => {
      this.getResults();
    })
  }

  ngOnInit() {
    this.availableFilters = Object.values(this.parsedData.filterItems).map((item: any) => ({id: item.id, text: item.name}));
    this.resultsList = this.parsedData.results || [];
    this.inputValue = this.parsedData.query;
  }

  onQueryChange(val: string): void {
    this.inputValue = val;
    this._query.next(val);
  }

  onTagsChange(val): void {
    this.selectedFilters = val;
    this._query.next(val);
  }

  onCbxChange(val): void {
    this._query.next(val);
  }

  requestDataBuilder(): ISearchRequestData {
    return {
      query: this.inputValue,
      page: this.currentPage,
      types: this.selectedFilters.map(filter => filter.id),
      onlyPinned: this.isOnlyPinned
    }
  }

  getResultsTitle(): string {
    const firstPart = this.translate.instant('searchResult.YouSearchedFor.lbl').replace('{0}', this.query)
    const secondPart = this.translate.instant('searchResult.Count.lbl').replace('{0}', this.resultsCount)
    return firstPart + " " + secondPart;
  }

  getType(item): string {
    switch (item.type) {
      case '##Search.Member##':
        return this.translate.instant('##Search.Member##');
      case '##Search.Socials##':
        return `${this.translate.instant('##Search.Socials##')} ${item.publishedDate}`;
      case 'News':
        return `${this.translate.instant('News')} ${item.publishedDate}`;
      case 'Event':
        return `${this.translate.instant('Event')} ${item.startDate} - ${item.endDate}`;
      case 'Content':
        return this.translate.instant('Content');
      case 'Document':
        return this.translate.instant('Document');
      case 'Tag':
        return this.translate.instant('Tag');
      default:
        return null;
    }
  }

  getResults(): void {
    this.isResultsLoading = true;

    this.searchService.search(this.requestDataBuilder()).pipe(
      finalize(() => {
        this.isResultsLoading = false;
      })
    ).subscribe((res: any) => {
      this.isScrollDisabled = res.results.length == res.resultsCount;
      this.resultsList = res.results;
      this.query = res.query;
      this.resultsCount = res.resultsCount;
    });
  }

  onLoadMore(): void {
    this.currentPage += 1;
    this.getResults();
  }

  onScroll(): void {
    this.onLoadMore();
  }
}
