import { Component, OnInit } from '@angular/core';
import { NewsService, INewsItem, INewsResponseModel } from 'src/app/service/news.service';

const itemsPerPage = 12; //TODO get from server

@Component({
  selector: 'news-list',
  templateUrl: './news-list.component.html',
  styleUrls: ['./news-list.component.less']
})
export class NewsListComponent {
  newsList: INewsItem[] = [];
  paginateOptions = {
    itemsPerPage,
    currentPage: 0,
    totalItems: 0
  };
  chekedYear: number = null;
  totalCount: number;

  constructor(private newsService: NewsService) {
    this.getNews();
  }

  getNews() {
    this.newsService.getNews(
      {
        year: this.chekedYear,
        index: this.paginateOptions.currentPage,
        step: itemsPerPage
      })
        .subscribe((res: INewsResponseModel) => {
          this.newsList = this.newsList.concat(res.items);
          this.paginateOptions.totalItems = res.totalCount;
          this.totalCount = res.totalCount;
        });
  }

  loadMore() {
    this.paginateOptions.currentPage += 1;
    this.getNews();
  }
}
