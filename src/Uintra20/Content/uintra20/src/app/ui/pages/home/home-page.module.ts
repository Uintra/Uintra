import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from '@ubaseline/next';
import { HomePage } from './home-page.component';
import { HeaderComponent } from '../../main-layout/header/header.component';
import { UserNavigationComponent } from 'src/app/feature/project/specific/user-navigation/user-navigation.component';
import { NavNotificationsModule } from 'src/app/feature/project/specific/nav-notifications/nav-notifications.module';

@NgModule({
  declarations: [HomePage, HeaderComponent,
    UserNavigationComponent],
  imports: [
    CommonModule,
    RouterModule.forChild([{ path: '', component: HomePage }]),
    UbaselineCoreModule,
    NavNotificationsModule
  ],
  entryComponents: [HomePage]
})
export class HomePageModule { }
