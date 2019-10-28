import { Component, OnInit, Input } from '@angular/core';
import { SearchService, ISearchResponse, ISearchItem } from '../service/search.service';
import { Subject, Observable } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import get from 'lodash/get';


@Component({
    selector: 'app-search',
    templateUrl: './search.component.html',
    styleUrls: ['./search.component.less']
})
export class SearchComponent implements OnInit {
    @Input() minSymbols: number = 2;
    @Input() itemsPerPage: number = 10;

    private subTitleTranslation: string;

    subTitle: string;
    currentPage: number;
    resultItems: ISearchItem[];
    advancedItems: ISearchItem[];
    total: number;
    searchQuery: string;

    constructor(
        private router: Router,
        private route: ActivatedRoute,
        private searchService: SearchService,
        private translateService: TranslateService,

    ) {
        translateService.getTranslation('').subscribe(val => {
            this.subTitleTranslation = val['searchResult.SubTitle'];
        });

        this.searchService.searchResult.subscribe(this.onSearchResultChanged);
    }

    ngOnInit() {
        this.route.queryParams.subscribe(params => {
            let query = get(params, 'query');
            this.searchQuery = query;
            this.searchByQuery(query);
        });
    }

    private addQueryParamsToUrl(query) {
        const queryParams: Params = { query: query };

        this.router.navigate(
          [],
          {
            relativeTo: this.route,
            queryParams: queryParams,
            queryParamsHandling: 'merge',
          });
    }

    searchByQuery(query: string = '') {
        if (query.length <= this.minSymbols) return;
        this.searchService.searchBy(query);
        this.addQueryParamsToUrl(query);
    }

    onPageChange(newPage) {
        this.searchService.changePageTo(newPage);
    }

    private onSearchResultChanged = (res) => {
        const total = this.searchService.total;

        this.currentPage = this.searchService.currentPage;

        this.resultItems = res.filter(item => !item.isSuggestion) || [];
        this.advancedItems = res.filter(item => item.isSuggestion === true) || [];
        this.total = total;

        this.subTitle = this.subTitleTranslation.replace('${0}', this.total.toString());
    }
}
