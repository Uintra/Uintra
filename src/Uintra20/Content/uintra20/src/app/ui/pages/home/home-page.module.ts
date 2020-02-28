import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { UbaselineCoreModule, NotImplementedModule } from '@ubaseline/next';
import { HomePage } from './home-page.component';
import { CanDeactivateGuard } from 'src/app/services/general/can-deactivate.service';

@NgModule({
  declarations: [HomePage],
  imports: [
    CommonModule,
      RouterModule.forChild([{ path: '', component: HomePage, canDeactivate: [CanDeactivateGuard]}]),
      UbaselineCoreModule,
      NotImplementedModule
  ],
  entryComponents: [HomePage]
})
export class HomePageModule { }
