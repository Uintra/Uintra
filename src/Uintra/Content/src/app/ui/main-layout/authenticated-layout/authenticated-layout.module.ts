import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthenticatedLayoutComponent } from './authenticated-layout.component';
import { TranslateModule } from '@ngx-translate/core';
import { SearchModule } from 'src/app/feature/reusable/inputs/search/search.module';
import { GoToTopButtonModule } from 'src/app/feature/reusable/ui-elements/go-to-top-button/go-to-top-button.module';
import { UserNavigationModule } from 'src/app/feature/specific/user-navigation/user-navigation.module';
import { HeaderModule } from '../header/header.module';
import { LeftNavigationModule } from '../left-navigation/left-navigation.module';



@NgModule({
  declarations: [AuthenticatedLayoutComponent],
  imports: [
    CommonModule,
    UserNavigationModule,
    GoToTopButtonModule,
    HeaderModule,
    SearchModule,
    LeftNavigationModule,
    TranslateModule
  ],
  exports: [AuthenticatedLayoutComponent]
})
export class AuthenticatedLayoutModule { }
