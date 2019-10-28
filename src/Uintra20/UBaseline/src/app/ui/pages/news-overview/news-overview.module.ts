import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';
import { DynamicComponentLoaderModule } from 'src/app/shared/dynamic-component-loader/dynamic-component-loader.module';
import { NewsOverviewPageComponent } from './news-overview-page/news-overview-page.component';
import { NewsListComponent } from './news-list/news-list.component';
import { NewsItemComponent } from './news-item/news-item.component';
import { NgxPaginationModule } from 'ngx-pagination';

import { TranslateModule } from '@ngx-translate/core';
import { HttpClient } from '@angular/common/http';
import { TranslationsLoader } from 'src/app/service/translations-loader';

const routes: Routes = [{path: "", component: NewsOverviewPageComponent}];

@NgModule({
  declarations: [NewsOverviewPageComponent, NewsListComponent, NewsItemComponent],
  imports: [
    RouterModule.forChild(routes),
    CommonModule,
    SharedModule,
    DynamicComponentLoaderModule,
    NgxPaginationModule,
    TranslateModule.forChild({
      loader: {
        provide: TranslateModule,
        deps: [HttpClient],
        useClass: TranslationsLoader},
    })
  ]
})
export class NewsOverviewModule { }
