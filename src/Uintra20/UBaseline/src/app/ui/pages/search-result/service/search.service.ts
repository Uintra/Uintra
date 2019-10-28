import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { config } from 'src/app/app.config';
import { BehaviorSubject, Subject } from 'rxjs';

export interface ISearchParams {
    query: string,
    page: number;
}

export interface ISearchResponse {
    items: ISearchItem[];
    pageCount: number;
    totalResultCount: number;
}

export interface ISearchItem {
    isSuggestion: boolean;
    title: string;
    description: string;
    url: string;
}

@Injectable({
    providedIn: 'root'
})


export class SearchService {
  private query: BehaviorSubject<string> = new BehaviorSubject('');
  private _currentPage: number;

  currentPage: number = 0;
  searchResult: Subject<ISearchItem[]> = new Subject();
  total: number = 0;

  headers: HttpHeaders;

  constructor(private http: HttpClient) {
      this.query.subscribe(query => {
          if (query) {
              this.search({ query, page: this.currentPage }).subscribe(this.responseHandler);
          }
      });

      this.headers = new HttpHeaders({
          'Cache-Control': 'no-cache',
          'Pragma': 'no-cache',
          'Expires': 'Sat, 01 Jan 2000 00:00:00 GMT'
      });
  }

  searchBy(query: string) {
      this.resetCurrentPage();
      this.query.next(query);
  }

  changePageTo(page: number) {
      this.currentPage = page;
      this.query.next(this.query.value);
  }

  private resetCurrentPage() {
      this.currentPage = 0;
  }

  private responseHandler = (res: ISearchResponse) => {
      this.total = res.totalResultCount;
      this.searchResult.next(res.items);
  }

  private search(params: ISearchParams) {
      return this.http.post(config.searchApi + 'search', params, { headers: this.headers });
  }
}


