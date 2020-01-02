import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from '@ubaseline/next';
import { NotificationsPage } from './notifications-page.component';
import { HeaderComponent } from '../../main-layout/header/header.component';

@NgModule({
  declarations: [NotificationsPage, HeaderComponent],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: NotificationsPage}]),
    UbaselineCoreModule,
  ],
  entryComponents: [NotificationsPage]
})
export class NotificationsPageModule {}
