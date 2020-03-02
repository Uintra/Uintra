import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from '@ubaseline/next';
import { ArticlePage } from './article-page.component';
import { CanDeactivateGuard } from 'src/app/shared/services/general/can-deactivate.service';
import { GroupDetailsWrapperModule } from 'src/app/feature/specific/groups/group-details-wrapper/group-details-wrapper.module';

@NgModule({
  declarations: [ArticlePage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: ArticlePage, canDeactivate: [CanDeactivateGuard]}]),
    UbaselineCoreModule,
    GroupDetailsWrapperModule,
  ],
  exports: [
    ArticlePage
  ],
  entryComponents: [ArticlePage]
})
export class ArticlePageModule {}
