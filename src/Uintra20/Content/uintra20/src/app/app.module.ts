import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { environment } from 'src/environments/environment';

import { DYNAMIC_COMPONENTS, UmbracoSupportModule } from '@ubaseline/next';
import { pages } from './ui/pages/pages';
import { panels } from './ui/panels/panels';
import { NavNotificationsModule } from './feature/project/specific/nav-notifications/nav-notifications.module';
import { ClickOutsideDirective } from './feature/project/reusable/inputs/tag-multiselect/helpers/click-outside.directive';

@NgModule({
  declarations: [
    AppComponent,
    ClickOutsideDirective
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    UmbracoSupportModule.configure({
      apiPrefix: '/ubaseline/api',
      pages: pages,
      environment: environment
    }),
    NavNotificationsModule
  ],
  providers: [
    { provide: DYNAMIC_COMPONENTS, useValue: panels }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
