import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { UbaselineCoreModule } from 'ubaseline-next-for-uintra';
import { ArticlePage } from './article-page.component';
import { CanDeactivateGuard } from 'src/app/shared/services/general/can-deactivate.service';
import { GroupDetailsWrapperModule } from 'src/app/feature/specific/groups/group-details-wrapper/group-details-wrapper.module';
import { BreadcrumbsModule } from 'src/app/shared/ui-elements/breadcrumbs/breadcrumbs.module';


@NgModule({
  declarations: [ArticlePage],
  imports: [
    CommonModule,
    RouterModule.forChild([{ path: '', component: ArticlePage, canDeactivate: [CanDeactivateGuard] }]),
    UbaselineCoreModule,
    GroupDetailsWrapperModule,
    BreadcrumbsModule,

  ],
  exports: [
    ArticlePage
  ],
  entryComponents: [ArticlePage]
})
export class ArticlePageModule { }
