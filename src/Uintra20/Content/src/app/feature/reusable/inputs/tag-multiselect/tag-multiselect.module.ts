import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TagMultiselectComponent } from './tag-multiselect.component';
import { TagItemComponent } from './tag-item/tag-item.component';
import { ClickOutsideModule } from 'src/app/shared/directives/click-outside/click-outside.module';


@NgModule({
  declarations: [TagMultiselectComponent, TagItemComponent],
  imports: [
    CommonModule,
    ClickOutsideModule
  ],
  exports: [
    TagMultiselectComponent
  ]
})
export class TagMultiselectModule { }
