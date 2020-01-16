import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TagMultiselectComponent } from './tag-multiselect.component';
import { TagItemComponent } from './tag-item/tag-item.component';
import { ClickOutsideDirective } from './helpers/click-outside.directive';


@NgModule({
  declarations: [TagMultiselectComponent, TagItemComponent, ClickOutsideDirective],
  imports: [
    CommonModule
  ],
  exports: [
    TagMultiselectComponent
  ]
})
export class TagMultiselectModule { }
