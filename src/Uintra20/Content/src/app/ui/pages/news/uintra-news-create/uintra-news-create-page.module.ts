import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from '@ubaseline/next';
import { UintraNewsCreatePage } from './uintra-news-create-page.component';
import { NewsCreateModule } from 'src/app/ui/panels/activity-create/sections/news-create/news-create.module';

@NgModule({
  declarations: [UintraNewsCreatePage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: UintraNewsCreatePage}]),
    UbaselineCoreModule,
    NewsCreateModule,
  ],
  entryComponents: [UintraNewsCreatePage]
})
export class UintraNewsCreatePageModule {}
