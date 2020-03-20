import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ISearchRequestData } from './search.interface';

@Injectable({
  providedIn: 'root'
})
export class SearchService {

  constructor(
    private http: HttpClient,
  ) { }

  autocomplete(query: string) {
    return this.http.post("/ubaseline/api/search/autocomplete", {query: query})
  }

  search(data: ISearchRequestData) {
    return this.http.post("/ubaseline/api/search/search", data)
  }
}
