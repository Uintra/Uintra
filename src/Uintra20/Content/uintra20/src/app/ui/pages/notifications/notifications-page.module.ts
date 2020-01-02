import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from '@ubaseline/next';
import { NotificationsPage } from './notifications-page.component';
import { NavNotificationsModule } from 'src/app/feature/project/specific/nav-notifications/nav-notifications.module';

@NgModule({
  declarations: [NotificationsPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{path: "", component: NotificationsPage}]),
    UbaselineCoreModule,
    NavNotificationsModule
  ],
  entryComponents: [NotificationsPage]
})
export class NotificationsPageModule {}
