import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule, NotImplementedModule, AS_DYNAMIC_COMPONENT } from 'ubaseline-next-for-uintra';
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
  providers: [
    { provide: AS_DYNAMIC_COMPONENT, useValue: HomePage }
  ],
  entryComponents: [HomePage]
})
export class HomePageModule { }
