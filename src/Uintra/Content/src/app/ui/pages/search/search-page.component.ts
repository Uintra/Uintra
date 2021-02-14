import { Component, ViewEncapsulation, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subject, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged, finalize } from 'rxjs/operators';
import { SearchService } from 'src/app/feature/specific/search/search.service';
import { ISearchRequestData, IMapedFilterData, ISearchResult } from 'src/app/feature/specific/search/search.interface';
import { TranslateService } from '@ngx-translate/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ISearchPage } from 'src/app/shared/interfaces/pages/search/search-page.interface';
import { Indexer } from '../../../shared/abstractions/indexer';

@Component({
  selector: 'search-page',
  templateUrl: './search-page.html',
  styleUrls: ['./search-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class SearchPage extends Indexer<number> implements OnInit, OnDestroy {

  private $searchSubscription: Subscription;
  public data: ISearchPage;

  public inputValue = '';
  public availableFilters: IMapedFilterData[] = [];
  public selectedFilters: IMapedFilterData[] = [];
  public resultsList: ISearchResult[] = [];
  public isOnlyPinned = false;
  public currentPage = 1;
  public query = '';
  public resultsCount: number;
  public isResultsLoading = false;
  public isScrollDisabled = false;
  public _query = new Subject<string>();

  constructor(
    private route: ActivatedRoute,
    private searchService: SearchService,
    private translate: TranslateService,
    private sanitizer: DomSanitizer,
  ) {
    super();
    this.route.data.subscribe((data: ISearchPage) =>  { 
      return this.data = data 
    });

    this._query.pipe(
      debounceTime(200),
      distinctUntilChanged(),
    ).subscribe(value => {
      this.getResults();
    });
  }

  public ngOnInit(): void {
    if(this.data.filterItems) {
      this.availableFilters = this.data.filterItems.map((item: any) => ({
        id: item.id,
        text: this.translate.instant(item.name)
      }));
    }
    
    if(this.data.results) {
      this.resultsList = this.data.results.map(this.checkSocialTitle);
    }
    this.inputValue = this.data.query;

    const paramsSubscription = this.route.queryParams.subscribe(params => {
      const query = params && params.query ? params.query : '';
      this.inputValue = query;
    });

    paramsSubscription.unsubscribe();
  }

  private checkSocialTitle(item: ISearchResult): ISearchResult {
    const SOCIAL_TYPE = 'Social';

    if (item.type === SOCIAL_TYPE) {
      item.title = '';
    }

    return item;
  }

  public ngOnDestroy(): void {
    if (this.$searchSubscription) { this.$searchSubscription.unsubscribe(); }
  }

  public onQueryChange(val: string): void {
    this.inputValue = val;
    this._query.next(val);
  }

  public onTagsChange(val): void {
    this.selectedFilters = val;
    this._query.next(val);
  }

  public onCbxChange(val): void {
    this._query.next(val);
  }

  public requestDataBuilder(): ISearchRequestData {
    return {
      query: this.inputValue,
      page: this.currentPage,
      types: this.selectedFilters.map(filter => filter.id),
      onlyPinned: this.isOnlyPinned
    };
  }

  public getResultsTitle(): string {
    const firstPart = this.translate.instant('searchResult.YouSearchedFor.lbl').replace('{0}', this.query);
    const secondPart = this.translate.instant('searchResult.Count.lbl').replace('{0}', this.resultsCount);
    return this.resultsCount !== undefined ? firstPart + ' ' + secondPart : '';
  }

  public getType(item): string {
    switch (item.type) {
      case 'Socials':
        return `Socials ${item.publishedDate}`;
      case 'News':
        return `News ${item.publishedDate}`;
      case 'Event':
        return `Event ${item.startDate} - ${item.endDate}`;
      default:
        return item.type;
    }
  }

  public getResults(): void {
    this.isResultsLoading = true;

    this.searchService.search(this.requestDataBuilder()).pipe(
      finalize(() => {
        this.isResultsLoading = false;
      })
    ).subscribe((res: any) => {
      this.isScrollDisabled = res.results.length === res.resultsCount;
      this.resultsList = res.results.map(this.checkSocialTitle).map(result => ({
        ...result,
        title: this.sanitizer.bypassSecurityTrustHtml(result.title),
        description: this.sanitizer.bypassSecurityTrustHtml(result.description),
      }));
      this.query = res.query;
      if(res.filterItems) {
        this.availableFilters = res.filterItems.map((item: any) => ({ id: item.id, text: item.name }));
      }
      this.data.allTypesPlaceholder = res.allTypesPlaceholder;
      this.resultsCount = res.resultsCount;
    });
  }

  public onLoadMore(): void {
    this.currentPage += 1;
    this.getResults();
  }

  public onScroll(): void {
    this.onLoadMore();
  }
}
