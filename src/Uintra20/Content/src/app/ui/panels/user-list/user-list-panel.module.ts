import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AS_DYNAMIC_COMPONENT } from '@ubaseline/next';
import { UserListPanel } from './user-list-panel.component';
import { UserListModule } from 'src/app/feature/specific/search/user-list/user-list.module';

@NgModule({
  declarations: [UserListPanel],
  imports: [
    CommonModule,
    UserListModule,
  ],
  providers: [{provide: AS_DYNAMIC_COMPONENT, useValue: UserListPanel}],
  entryComponents: [UserListPanel]
})
export class UserListPanelModule {}
