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

  public getPublications = (data: IFeedListRequest) =>
    this.http.post(`/ubaseline/api/centralFeedApi/FeedList`, data)


  public addLike({ entityId, entityType }: IAddLikeRequest) {
    return this.http.post(`/ubaseline/api/likes/AddLike?entityId=${entityId}&entityType=${entityType}`, {}).toPromise();
  }

  public removeLike({ entityId, entityType }: IAddLikeRequest) {
    return this.http.post(`/ubaseline/api/likes/RemoveLike?entityId=${entityId}&entityType=${entityType}`, {}).toPromise();
  }
}
