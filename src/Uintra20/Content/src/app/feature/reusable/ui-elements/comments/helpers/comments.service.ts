import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CommentsService {

  private routePrefix = '/ubaseline/api/comments/';

  constructor(private http: HttpClient) { }

  public onCreate = (data) => {
    return this.http.post(`${this.routePrefix}add`, data);
  }

  public deleteComment = (data) => {
    return this.http.delete(`${this.routePrefix}delete?targetId=${data.targetId}&targetType=${data.targetType}&commentId=${data.commentId}`, {});
  }

  public editComment = (data) => {
    return this.http.put(`${this.routePrefix}edit`, data);
  }
}
