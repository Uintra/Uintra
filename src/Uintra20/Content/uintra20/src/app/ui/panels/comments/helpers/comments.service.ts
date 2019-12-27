import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CommentsService {

  constructor(private http: HttpClient) { }

  onCreate(data) {
    return this.http.post('/ubaseline/api/comments/add', data).toPromise();
  }

  deleteComment(data) {
    return this.http.delete(`ubaseline/api/comments/delete?targetId=${data.targetId}&targetType=${data.targetType}&commentId=${data.commentId}`, {}).toPromise();
  }

  editComment(data) {
    return this.http.put('/ubaseline/api/comments/edit', data).toPromise();
  }
}
