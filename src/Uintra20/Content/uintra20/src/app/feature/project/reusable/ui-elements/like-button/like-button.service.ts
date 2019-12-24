import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface IAddLikeRequest {
  entityId: string;
  entityType: string;
}

@Injectable({
  providedIn: 'root'
})
export class LikeButtonService {

  constructor(
    private http: HttpClient
  ) { }

  addLike({entityId, entityType }: IAddLikeRequest) {
    return this.http.post(`/ubaseline/api/likes/AddLike?entityId=${entityId}&entityType=${entityType}`, {}).toPromise();
  }

  removeLike({entityId, entityType }: IAddLikeRequest) {
    return this.http.post(`/ubaseline/api/likes/RemoveLike?entityId=${entityId}&entityType=${entityType}`, {}).toPromise();
  }
}
