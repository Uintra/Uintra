import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header.component';
import { SearchModule } from 'src/app/feature/project/reusable/inputs/search/search.module';
import { SearchComponent } from 'src/app/feature/project/reusable/inputs/search/search.component';
import { RouterModule } from '@angular/router';
import { NavNotificationsModule } from 'src/app/feature/project/specific/nav-notifications/nav-notifications.module';
import { UserNavigationModule } from 'src/app/feature/project/specific/user-navigation/user-navigation.module';

@NgModule({
  declarations: [HeaderComponent],
  imports: [
    CommonModule,
    RouterModule,
    SearchModule,
    NavNotificationsModule,
    UserNavigationModule
  ],
  exports: [HeaderComponent]
})
export class HeaderModule { }
