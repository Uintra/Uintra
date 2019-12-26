import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CommentsService {

  constructor(private http: HttpClient) { }

  onCreate(data) {
    this.http.post('ubaseline/api/comments/add', data)
  }
}
