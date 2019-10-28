import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { config } from '../app.config';
import { Observable } from 'rxjs';

interface INewsRequestModel {
  index: number;
  step: number;
  year: number;
}

export interface INewsResponseModel {
  totalCount: number;
  items: INewsItem[];
  years: number[];
}

export interface INewsItem {
  title: string;
  description: string;
  previewImage: any;
  publishDate: string;
  publishDateString: string;
  url: string;
}

@Injectable({
  providedIn: 'root'
})
export class NewsService {

  constructor(private http: HttpClient) { }

  getNews(data: INewsRequestModel) {
    return this.http.post<INewsResponseModel>(`${config.api}/NewsPreview/GetNewsPreviews`, data);
  }
}
