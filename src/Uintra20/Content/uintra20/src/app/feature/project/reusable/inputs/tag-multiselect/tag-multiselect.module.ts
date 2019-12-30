import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TagMultiselectComponent } from './tag-multiselect.component';
import { TagItemComponent } from './tag-item/tag-item.component';

@NgModule({
  declarations: [TagMultiselectComponent, TagItemComponent],
  imports: [
    CommonModule
  ],
  exports: [
    TagMultiselectComponent
  ]
})
export class TagMultiselectModule { }
