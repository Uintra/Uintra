import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { INewsPanelItem } from '../news-panel.component';
import { config } from 'src/app/app.config';

interface INewsRequestModel {
  index: number;
  step: number;
  year: number;
}
export interface INewsResponseModel {
  totalCount: number;
  items: INewsPanelItem[];
  years: number[];
}

@Injectable({
  providedIn: 'root'
})
export class NewsOverviewService {
  constructor(private http: HttpClient) { }

  getNews(data: INewsRequestModel) {
    return this.http.post<INewsResponseModel>(
      `${config.api}/NewsPreview/GetNewsPreviews`, data
    );
  }
}
