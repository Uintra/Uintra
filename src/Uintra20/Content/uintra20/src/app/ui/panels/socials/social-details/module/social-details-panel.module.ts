import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AS_DYNAMIC_COMPONENT } from '@ubaseline/next';
import { SocialDetailsPanelComponent } from '../component/social-details-panel.component';

@NgModule({
  declarations: [
    SocialDetailsPanelComponent
  ],
  imports: [
    CommonModule
  ],
  providers: [
    { provide: AS_DYNAMIC_COMPONENT, useValue: SocialDetailsPanelComponent }
  ],
  entryComponents: [
    SocialDetailsPanelComponent
  ]
})
export class SocialDetailsPanelModule { }
