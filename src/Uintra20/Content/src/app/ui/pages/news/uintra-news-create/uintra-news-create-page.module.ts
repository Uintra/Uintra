import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from '@ubaseline/next';
import { UintraNewsCreatePage } from './uintra-news-create-page.component';
import { NewsCreateModule } from 'src/app/feature/specific/activity/create/news-create/news-create.module';
import { GroupDetailsWrapperModule } from 'src/app/feature/specific/groups/group-details-wrapper/group-details-wrapper.module';
import { CanDeactivateGuard } from 'src/app/shared/services/general/can-deactivate.service';

@NgModule({
  declarations: [UintraNewsCreatePage],
  imports: [
    CommonModule,
    RouterModule.forChild([{ path: "", component: UintraNewsCreatePage, canDeactivate: [CanDeactivateGuard] }]),
    UbaselineCoreModule,
    NewsCreateModule,
    GroupDetailsWrapperModule,
  ],
  entryComponents: [UintraNewsCreatePage]
})
export class UintraNewsCreatePageModule { }
