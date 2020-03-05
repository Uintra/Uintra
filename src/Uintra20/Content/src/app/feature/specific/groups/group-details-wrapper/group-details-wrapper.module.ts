import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GroupDetailsWrapperComponent } from './group-details-wrapper.component';
import { UlinkModule } from 'src/app/shared/pipes/link/ulink.module';
import { RouterModule } from '@angular/router';
import { BreadcrumbsModule } from 'src/app/shared/ui-elements/breadcrumbs/breadcrumbs.module';



@NgModule({
  declarations: [GroupDetailsWrapperComponent],
  imports: [
    CommonModule,
    RouterModule,
    UlinkModule,
    BreadcrumbsModule,
  ],
  exports: [GroupDetailsWrapperComponent]
})
export class GroupDetailsWrapperModule { }
