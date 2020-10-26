import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header.component';
import { RouterModule } from '@angular/router';
import { SearchModule } from 'src/app/feature/reusable/inputs/search/search.module';
import { NavNotificationsModule } from 'src/app/feature/specific/nav-notifications/nav-notifications.module';
import { UserNavigationModule } from 'src/app/feature/specific/user-navigation/user-navigation.module';
import { AutocompleteModule } from 'src/app/feature/specific/search/autocomplete/autocomplete.module';

@NgModule({
  declarations: [HeaderComponent],
  imports: [
    CommonModule,
    RouterModule,
    SearchModule,
    NavNotificationsModule,
    UserNavigationModule,
    AutocompleteModule,
  ],
  exports: [HeaderComponent]
})
export class HeaderModule { }
