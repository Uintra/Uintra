import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GroupsListComponent } from './groups-list.component';



@NgModule({
  declarations: [GroupsListComponent],
  imports: [
    CommonModule
  ],
  exports: [GroupsListComponent]
})
export class GroupsListModule { }
