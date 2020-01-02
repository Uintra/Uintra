import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule } from '@ubaseline/next';
import { HomePage } from './home-page.component';
<<<<<<< HEAD
import { HeaderComponent } from '../../main-layout/header/header.component';
import { UserNavigationComponent } from 'src/app/feature/project/specific/user-navigation/user-navigation.component';
import { NavNotificationsModule } from 'src/app/feature/project/specific/nav-notifications/nav-notifications.module';
=======
>>>>>>> release/2.0

@NgModule({
  declarations: [HomePage],
  imports: [
    CommonModule,
<<<<<<< HEAD
    RouterModule.forChild([{ path: '', component: HomePage }]),
    UbaselineCoreModule,
    NavNotificationsModule
=======
      RouterModule.forChild([{ path: '', component: HomePage }]),
      UbaselineCoreModule
>>>>>>> release/2.0
  ],
  entryComponents: [HomePage]
})
export class HomePageModule { }
