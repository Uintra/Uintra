import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { config } from 'src/app/app.config';

export interface IAutosuggestResponse {
  searchPageUrl: string;
  items: IAutosuggestResponseItem[];
  totalResultCount: number;
  pageCount: number;
  initialRequest: boolean;
}

export interface IAutosuggestResponseItem {
  title: string;
  description: string;
  url: string;
  highlighted?: boolean;
}
@Injectable({
  providedIn: 'root'
})
export class AutosuggestService {

  private results$: BehaviorSubject<IAutosuggestResponse> = new BehaviorSubject(
    { items: [], totalResultCount: 0, pageCount: 0, initialRequest: true, searchPageUrl: null }
  );

  headers: HttpHeaders;

  constructor(private http: HttpClient) {
    this.headers = new HttpHeaders({
      'Cache-Control': 'no-cache',
      'Pragma': 'no-cache',
      'Expires': 'Sat, 01 Jan 2000 00:00:00 GMT'
    });
  }

  getResults() {
    return this.results$;
  }

  async getSuggestionsFor(query: string) {
    if (typeof query !== 'string' || query.length <= 2) {
      return this.clear();
    }

    const results = await this.http.post<IAutosuggestResponse>(
      config.searchApi + 'autocomplete',
      { query, page: 0 },
      { headers: this.headers }
    ).toPromise()
      .catch(err => {
        if (err.status === 404) {
          console.error(`
            Dear developer, please check or specify the property 'searchApi:
            "/your/search/api"' in the project's app.config.ts
          `);
        }
        return null;
      });

    results ? this.results$.next(results) : this.clear();
  }

  clear() {
    this.results$.next({ items: [], totalResultCount: 0, pageCount: 0, initialRequest: true, searchPageUrl: null });
  }
}
