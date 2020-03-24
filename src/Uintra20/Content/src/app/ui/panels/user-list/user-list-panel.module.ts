import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AS_DYNAMIC_COMPONENT } from '@ubaseline/next';
import { UserListPanel } from './user-list-panel.component';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [UserListPanel],
  imports: [
    CommonModule,
    TranslateModule,
  ],
  providers: [{provide: AS_DYNAMIC_COMPONENT, useValue: UserListPanel}],
  entryComponents: [UserListPanel]
})
export class UserListPanelModule {}
