import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { UmbracoFlatPropertyModel } from "@ubaseline/next";
import { CentralFeedFiltersService } from "./central-feed-filters.service";

@Component({
  selector: "app-central-feed-filters",
  templateUrl: "./central-feed-filters.component.html",
  styleUrls: ["./central-feed-filters.component.less"]
})
export class CentralFeedFiltersComponent implements OnInit {
  @Input() tabs: Array<UmbracoFlatPropertyModel>;
  @Output() selectFilters = new EventEmitter();

  selectedTab: UmbracoFlatPropertyModel = null;
  selectTabFilters: Array<any>;
  selectedTabType: number;
  isOpen: boolean = false;
  filtersState: Array<any> = [];

  constructor(private centralFeedFiltersService: CentralFeedFiltersService) {}

  ngOnInit() {
    this.setInitValues();
    this.emitFilterState();
  }

  handleOpen(value: boolean) {
    this.centralFeedFiltersService.setOpeningState(this.isOpen);
  }

  setSelectedTab(event) {
    this.selectedTabType = event;
    this.selectedTab = this.tabs.find(tab => tab.get().type.get() === event);
    this.selectTabFilters = this.getTabFilters();
    this.setSelectedFiltersFromCookie();
    this.emitFilterState();
  }

  changeFilters() {
    this.setFiltersState();
    this.emitFilterState();
  }

  clearFilters() {
    this.centralFeedFiltersService.setFilteringState("");
    this.filtersState = [];
    this.setInitValues();

    this.selectTabFilters = this.selectTabFilters.map(filter => ({
      ...filter,
      isActive: false
    }));
    this.setSelectedTab(
      this.tabs.find(tab => tab.get().isActive.get() === true).get().type.get()
    );
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
      return Object.values(JSON.parse(JSON.stringify(tab.get().filters.get())));
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
      this.selectedTab = this.tabs.find(
        tab => tab.get().type == selectedTabType
      );
    } else {
      this.selectedTab = this.tabs.find(tab => tab.get().isActive);
      this.selectedTabType = this.selectedTab.get().type.get();
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
        const foundFilter = selectTabFilters.find(
          cur => cur.key === filter.key
        );

        return foundFilter
          ? { ...filter, isActive: foundFilter.isActive }
          : filter;
      });
    }

    this.setFiltersState();
  }

  private getTabFilters() {
    return Object.values(
      JSON.parse(JSON.stringify(this.selectedTab.get().filters.get()))
    );
  }

  private setFiltersState() {
    this.filtersState = this.filtersState.map(filter => {
      const foundFilter = this.selectTabFilters.find(
        cur => cur.key === filter.key
      );
      if (foundFilter) {
        filter.isActive = foundFilter.isActive;
      }
      return filter;
    });
  }
}
