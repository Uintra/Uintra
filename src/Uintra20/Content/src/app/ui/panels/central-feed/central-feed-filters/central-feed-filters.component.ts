import { Component, OnInit, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import { CentralFeedFiltersService } from './central-feed-filters.service';

@Component({
  selector: 'app-central-feed-filters',
  templateUrl: './central-feed-filters.component.html',
  styleUrls: ['./central-feed-filters.component.less']
})
export class CentralFeedFiltersComponent implements OnInit, OnDestroy {

  @Input()
  public tabs: Array<any>;
  @Output()
  public selectFilters = new EventEmitter();

  public selectedTab: any = null;
  public selectTabFilters: Array<any>;
  public selectedTabType: number;
  public isOpen = false;
  public filtersState: Array<any> = [];
  public filtersAllowed: boolean;
  public filterSubscription: any;

  constructor(private centralFeedFiltersService: CentralFeedFiltersService) { }

  public ngOnInit(): void {
    this.setInitValues();
    this.emitFilterState();
    this.resolveFilterPermissions();
    this.filterSubscription = this.centralFeedFiltersService.filter.subscribe((val: number) => {
      this.setSelectedTab(val);
    });
  }

  public ngOnDestroy(): void {
    this.filterSubscription.unsubscribe();
  }

  public handleOpen(value: boolean) {
    this.centralFeedFiltersService.setOpeningState(this.isOpen);
  }

  public setSelectedTab(event) {
    if (event === 0) {
      event = '0';
    }
    this.selectedTabType = event;
    this.selectedTab = this.tabs.find(tab => tab.type === event);
    this.selectTabFilters = this.getTabFilters();
    this.setSelectedFiltersFromCookie();
    this.emitFilterState();
  }

  public changeFilters() {
    this.setFiltersState();
    this.emitFilterState();
  }

  public clearFilters() {
    this.centralFeedFiltersService.setFilteringState('');
    this.filtersState = [];
    this.setInitValues();

    this.selectTabFilters = this.selectTabFilters.map(filter => ({
      ...filter,
      isActive: false
    }));
    this.setSelectedTab(this.tabs.find(tab => tab.isActive === true).type);
  }

  private setInitValues(): void {
    this.isOpen = this.centralFeedFiltersService.getOpeningState();

    this.setUniqueFiltersState();
    this.setSelectedTabFromCookie();

    this.selectTabFilters = this.getTabFilters();
    this.setSelectedFiltersFromCookie();
  }

  private setUniqueFiltersState() {
    const filtersState = this.tabs.map(tab => {
      return tab.filters;
    });
    filtersState.forEach(filter => {
      filter.forEach((item: any) => {
        if (!this.filtersState.find(cur => cur.key === item.key)) {
          this.filtersState.push(item);
        }
      });
    });
  }

  private setSelectedTabFromCookie() {
    const selectedTabType = this.centralFeedFiltersService.getTabState();

    if (selectedTabType) {
      this.selectedTabType = selectedTabType;
      this.selectedTab = this.tabs.find(tab => tab.type === selectedTabType);
      if (!this.selectedTab) {
        this.selectedTab = this.tabs[0];
      }
    } else {
      this.selectedTab = this.tabs.find(tab => tab.isActive);
      this.selectedTabType = this.selectedTab.type;
    }
  }

  private emitFilterState() {
    this.centralFeedFiltersService.setTabState(this.selectedTabType);
    this.centralFeedFiltersService.setFilteringState(this.filtersState);

    this.selectFilters.emit({
      selectedTabType: this.selectedTabType,
      selectTabFilters: this.selectTabFilters
    });
  }

  private setSelectedFiltersFromCookie() {
    const selectTabFilters = this.centralFeedFiltersService.getFilteringState();

    if (selectTabFilters && selectTabFilters.length) {
      this.selectTabFilters = this.selectTabFilters.map(filter => {
        const foundFilter = selectTabFilters.find(cur => cur.key === filter.key);

        return foundFilter
          ? { ...filter, isActive: foundFilter.isActive }
          : filter;
      });
    }

    this.setFiltersState();
  }

  private getTabFilters() {
    return this.selectedTab.filters;
  }

  private setFiltersState() {
    this.filtersState = this.filtersState.map(filter => {
      const foundFilter = this.selectTabFilters.find(cur => cur.key === filter.key);
      if (foundFilter) {
        filter.isActive = foundFilter.isActive;
      }
      return filter;
    });
  }

  private resolveFilterPermissions(): void {
    if (this.tabs.length > 1) {
      this.filtersAllowed = true;
    }
  }
}

