import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { UbaselineCoreModule } from '@ubaseline/next';
import { UintraGroupsPage } from './uintra-groups-page.component';

@NgModule({
  declarations: [UintraGroupsPage],
  imports: [
    CommonModule,
    RouterModule.forChild([{ path: '', component: UintraGroupsPage }]),
    UbaselineCoreModule,
  ],
  entryComponents: [UintraGroupsPage]
})
export class UintraGroupsPageModule { }

