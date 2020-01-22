import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { environment } from 'src/environments/environment';

import { DYNAMIC_COMPONENTS, UmbracoSupportModule, UbaselineCoreModule } from '@ubaseline/next';
import { pages } from './ui/pages/pages';
import { panels } from './ui/panels/panels';
import { NavNotificationsModule } from './feature/project/specific/nav-notifications/nav-notifications.module';
import { HeaderComponent } from './ui/main-layout/header/header.component';
import { UserNavigationModule } from './feature/project/specific/user-navigation/user-navigation.module';
import { ImageGalleryModule } from './feature/project/reusable/ui-elements/image-gallery/image-gallery.module';
import { GoToTopButtonModule } from './feature/project/reusable/ui-elements/go-to-top-button/go-to-top-button.module';
import { LeftNavigationModule } from './ui/main-layout/left-navigation/left-navigation.module';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    UserNavigationModule,
    GoToTopButtonModule,
    ImageGalleryModule,
    UmbracoSupportModule.configure({
      apiPrefix: '/ubaseline/api',
      pages: pages,
      environment: environment
    }),
    NavNotificationsModule,
    LeftNavigationModule,
    UbaselineCoreModule,
  ],
  providers: [
    { provide: DYNAMIC_COMPONENTS, useValue: panels }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
