import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AS_DYNAMIC_COMPONENT } from '@ubaseline/next';
import { RouterModule } from '@angular/router';
import { UbaselineCoreModule } from '@ubaseline/next';
import { SocialDetailsPanelComponent } from './social-details-page.component';
import { LikeButtonModule } from 'src/app/feature/project/reusable/ui-elements/like-button/like-button.module';

@NgModule({
  declarations: [
    SocialDetailsPanelComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([{ path: '', component: SocialDetailsPanelComponent }]),
    UbaselineCoreModule,
    LikeButtonModule,
  ],
  providers: [
    { provide: AS_DYNAMIC_COMPONENT, useValue: SocialDetailsPanelComponent }
  ],
  entryComponents: [
    SocialDetailsPanelComponent
  ]
})
export class SocialDetailsPageModule { }
