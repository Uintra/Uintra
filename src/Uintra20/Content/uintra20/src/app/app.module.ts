import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { environment } from 'src/environments/environment';

import { DYNAMIC_COMPONENTS, UmbracoSupportModule, UbaselineCoreModule } from '@ubaseline/next';
import { pages } from './ui/pages/pages';
import { panels } from './ui/panels/panels';
import { HeaderComponent } from './ui/main-layout/header/header.component';
import { UserNavigationModule } from './feature/project/specific/user-navigation/user-navigation.module';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent
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
    UbaselineCoreModule,
    UserNavigationModule
  ],
  providers: [
    { provide: DYNAMIC_COMPONENTS, useValue: panels }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
