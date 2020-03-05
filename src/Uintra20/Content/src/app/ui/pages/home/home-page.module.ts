import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule, NotImplementedModule } from '@ubaseline/next';
import { HomePage } from './home-page.component';
import { CanDeactivateGuard } from 'src/app/shared/services/general/can-deactivate.service';
import { SocialCreateModule } from '../../../feature/specific/activity/create/social-create/social-create.module';

@NgModule({
  declarations: [HomePage],
  imports: [
    CommonModule,
      RouterModule.forChild([{ path: '', component: HomePage, canDeactivate: [CanDeactivateGuard]}]),
      UbaselineCoreModule,
      NotImplementedModule,
      SocialCreateModule,
  ],
  entryComponents: [HomePage]
})
export class HomePageModule { }
