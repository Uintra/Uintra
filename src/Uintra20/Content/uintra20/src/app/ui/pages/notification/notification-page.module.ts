import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from '@ubaseline/next';
import { NotificationPage } from './notification-page.component';

@NgModule({
  declarations: [NotificationPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: NotificationPage}]),
    UbaselineCoreModule,
  ],
  entryComponents: [NotificationPage]
})
export class NotificationPageModule {}
