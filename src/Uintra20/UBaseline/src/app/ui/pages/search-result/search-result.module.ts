import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { SearchResultComponent } from './search-result.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { SearchComponent } from './search/search.component';
import { TranslateModule } from '@ngx-translate/core';
import { HttpClient } from '@angular/common/http';
import { TranslationsLoader } from 'src/app/service/translations-loader';
import { NgxPaginationModule } from 'ngx-pagination';
import { ResultListComponent } from './result-list/result-list.component';
import { AliasQuestionComponent } from './alias-question/alias-question.component';

const routes: Routes = [{ path: "", component: SearchResultComponent }];
@NgModule({
  declarations: [SearchResultComponent, SearchComponent, ResultListComponent, AliasQuestionComponent],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild(routes),
    NgxPaginationModule,
    TranslateModule.forChild({
      loader: {
        provide: TranslateModule,
        deps: [HttpClient],
        useClass: TranslationsLoader
      },
    })
  ]
})
export class SearchResultModule { }
