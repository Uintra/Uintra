import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AS_DYNAMIC_COMPONENT } from '@ubaseline/next';
import { UserTagsPanel } from './user-tags-panel.component';
import { TranslateModule } from '@ngx-translate/core';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [UserTagsPanel],
  imports: [
    CommonModule,
    TranslateModule,
    RouterModule
  ],
  providers: [{ provide: AS_DYNAMIC_COMPONENT, useValue: UserTagsPanel }],
  entryComponents: [UserTagsPanel]
})
export class UserTagsPanelModule { }
