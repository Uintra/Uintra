import { Injectable, Inject, Optional } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { AUTOSUGGEST_CONFIG, IAutosuggestModuleConfig } from '../config';

export interface IAutosuggestResponse {
  items: IAutosuggestResponseItem[];
  totalResultCount: number;
  pageCount: number;
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

  private results$: BehaviorSubject<IAutosuggestResponse> = new BehaviorSubject({ items: [], totalResultCount: 0, pageCount: 0 });

  constructor(
    private http: HttpClient,
    @Optional() @Inject(AUTOSUGGEST_CONFIG) private config: IAutosuggestModuleConfig
    ) {
      if (!this.config) throw new Error('Please provide the `CONFIG` token of this module in the AppModule.');
    }

  getResults() { return this.results$; }

  async getSuggestionsFor(query: string) {
    if (typeof query !== 'string' || query.length <= 2) return this.clear();

    let results = await this.http.post<IAutosuggestResponse>(this.config.endPoint, { query, page: this.config.page }).toPromise()
      .catch(err => {
        if (err.status === 404)
          console.error(`Dear developer, please check or specify the property 'searchApi: "/your/search/api"' in the project's app.config.ts`);
        return null;
      });

    results ? this.results$.next(results) : this.clear();
  }

  clear() {
    this.results$.next({ items: [], totalResultCount: 0, pageCount: 0 });
  }
}