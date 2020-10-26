import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IUserLikeData } from './like-button.interface';

export interface ILike {
  entityId: string;
  entityType: string;
}
export interface IAddLikeRequest extends ILike { }

export interface IRemoveLikeRequest extends ILike { }

@Injectable({
  providedIn: 'root'
})
export class LikeButtonService {

  private routePrefix = '/ubaseline/api/likes/';

  constructor(private httpClient: HttpClient) { }

  public addLike = ({ entityId, entityType }: ILike): Observable<Array<IUserLikeData>> =>
    this.httpClient.post<Array<IUserLikeData>>(`${this.routePrefix}AddLike?entityId=${entityId}&entityType=${entityType}`, {})

  public removeLike = ({ entityId, entityType }: ILike): Observable<Array<IUserLikeData>> =>
    this.httpClient.post<Array<IUserLikeData>>(`${this.routePrefix}RemoveLike?entityId=${entityId}&entityType=${entityType}`, {})
}
