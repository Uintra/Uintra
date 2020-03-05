import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TagMultiselectComponent } from './tag-multiselect.component';
import { TagItemComponent } from './tag-item/tag-item.component';
import { ClickOutsideModule } from 'src/app/shared/directives/click-outside/click-outside.module';
import { TranslateModule } from '@ngx-translate/core';


@NgModule({
  declarations: [TagMultiselectComponent, TagItemComponent],
  imports: [
    CommonModule,
    ClickOutsideModule,
    TranslateModule,
  ],
  exports: [
    TagMultiselectComponent
  ]
})
export class TagMultiselectModule { }
