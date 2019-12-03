import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from '@ubaseline/next';
import { ArticlePage } from './article-page.component';

@NgModule({
  declarations: [ArticlePage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: ArticlePage}]),
    UbaselineCoreModule,
  ],
  entryComponents: [ArticlePage]
})
export class ArticlePageModule {}