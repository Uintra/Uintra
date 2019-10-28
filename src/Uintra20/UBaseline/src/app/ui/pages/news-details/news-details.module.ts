import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NewsDetailsComponent } from './news-details.component';
import { Routes, RouterModule } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';
import { DynamicComponentLoaderModule } from 'src/app/shared/dynamic-component-loader/dynamic-component-loader.module';
import { TranslateModule } from '@ngx-translate/core';
import { HttpClient } from '@angular/common/http';
import { TranslationsLoader } from 'src/app/service/translations-loader';

const routes: Routes = [{path: "", component: NewsDetailsComponent}];

@NgModule({
  declarations: [NewsDetailsComponent],
  imports: [
    RouterModule.forChild(routes),
    CommonModule,
    SharedModule,
    DynamicComponentLoaderModule,
    TranslateModule.forChild({
      loader: {
        provide: TranslateModule,
        deps: [HttpClient],
        useClass: TranslationsLoader
      },
    }),
  ]
})
export class NewsDetailsModule { }
