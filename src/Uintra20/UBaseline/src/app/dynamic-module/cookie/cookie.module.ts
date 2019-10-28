import { NgModule, InjectionToken } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CookieComponent } from './cookie.component';
import { DYNAMIC_COMPONENT } from '../../shared/dynamic-component-loader/dynamic-component.manifest';

@NgModule({
  declarations: [CookieComponent],
  imports: [
    CommonModule
  ],
  providers: [
    {provide: DYNAMIC_COMPONENT, useValue: CookieComponent}
  ],
  exports: [CookieComponent],
  entryComponents: [CookieComponent]
})
export class CookieModule { }
