import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AS_DYNAMIC_COMPONENT } from 'ubaseline-next-for-uintra';
import { ContactPanel } from './contact-panel.component';
import { PictureModule } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/picture/picture.module';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [ContactPanel],
  imports: [
    CommonModule,
    PictureModule,
    TranslateModule
  ],
  providers: [{provide: AS_DYNAMIC_COMPONENT, useValue: ContactPanel}],
  entryComponents: [ContactPanel]
})
export class ContactPanelModule {}
