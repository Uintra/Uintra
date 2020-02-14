import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GroupsWrapperComponent } from './groups-wrapper.component';
import { RouterModule } from '@angular/router';
import { UlinkModule } from 'src/app/services/pipes/link/ulink.module';



@NgModule({
  declarations: [GroupsWrapperComponent],
  imports: [
    CommonModule,
    RouterModule,
    UlinkModule
  ],
  exports: [GroupsWrapperComponent]
})
export class GroupsWrapperModule { }
