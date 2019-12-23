import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CreateSocialContentService {

  constructor(
    private http: HttpClient
  ) { }

  // TODO: add interface to data
  submitSocialContent(data) {
    return this.http.post(`/ubaseline/api/bulletins/createExtended`, data).toPromise();
  }
}
