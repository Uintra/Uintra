import { NgModule } from '@angular/core';
import { SocialDetailsPanelComponent } from '../component/social-details-page.component';
import { CommonModule } from '@angular/common';
import { AS_DYNAMIC_COMPONENT } from '@ubaseline/next';
import { RouterModule } from '@angular/router';
import { UbaselineCoreModule } from '@ubaseline/next';

@NgModule({
  declarations: [
    SocialDetailsPanelComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([{ path: '', component: SocialDetailsPanelComponent }]),
    UbaselineCoreModule
  ],
  providers: [
    { provide: AS_DYNAMIC_COMPONENT, useValue: SocialDetailsPanelComponent }
  ],
  entryComponents: [
    SocialDetailsPanelComponent
  ]
})
export class SocialDetailsPageModule { }
