import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface IFeedListRequest {
  TypeId: number;
  FilterState?: IFilterState;
  Page?: number;
}

interface IFilterState {
  ShowSubscribed?: boolean;
  ShowPinned?: boolean;
  IncludeBulletin?: boolean;
}

export interface IAddLikeRequest {
  entityId: string;
  entityType: string;
}

@Injectable({
  providedIn: 'root'
})
export class PublicationsService {

  constructor(
    private http: HttpClient
  ) { }

  getPublications(data: IFeedListRequest) {
    return this.http.post(`/ubaseline/api/centralFeedApi/FeedList`, data).toPromise();
  }

  addLike({entityId, entityType }: IAddLikeRequest) {
    return this.http.post(`/ubaseline/api/likes/AddLike?entityId=${entityId}&entityType=${entityType}`, {}).toPromise();
  }

  removeLike({entityId, entityType }: IAddLikeRequest) {
    return this.http.post(`/ubaseline/api/likes/RemoveLike?entityId=${entityId}&entityType=${entityType}`, {}).toPromise();
  }
}
