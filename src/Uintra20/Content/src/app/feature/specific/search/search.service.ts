import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ISearchRequestData, IAutocompleteItem, ISearchData } from './search.interface';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SearchService {

  constructor(
    private http: HttpClient,
  ) { }

  autocomplete(query: string): Observable<IAutocompleteItem[]> {
    return this.http.post<IAutocompleteItem[]>("/ubaseline/api/search/autocomplete", {query: query});
  }

  search(data: ISearchRequestData): Observable<ISearchData> {
    return this.http.post<ISearchData>("/ubaseline/api/search/search", data)
  }
}
