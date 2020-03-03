import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GroupDetailsWrapperComponent } from './group-details-wrapper.component';
import { UlinkModule } from 'src/app/shared/pipes/link/ulink.module';
import { RouterModule } from '@angular/router';



@NgModule({
  declarations: [GroupDetailsWrapperComponent],
  imports: [
    CommonModule,
    RouterModule,
    UlinkModule,
  ],
  exports: [GroupDetailsWrapperComponent]
})
export class GroupDetailsWrapperModule { }
