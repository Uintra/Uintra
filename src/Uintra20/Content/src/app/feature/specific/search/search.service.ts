import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

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
}
