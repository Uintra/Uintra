import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TextPanel } from './text-panel.component';
import { AS_DYNAMIC_COMPONENT } from 'ubaseline-next-for-uintra';
import { TextModule } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/text/text.module';
import { RouterModule } from '@angular/router';
import { ButtonModule } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/button/button.module';

@NgModule({
  declarations: [TextPanel],
  imports: [
    CommonModule,
    TextModule,
    RouterModule,
    ButtonModule
  ],
  providers: [{provide: AS_DYNAMIC_COMPONENT, useValue: TextPanel}],
  entryComponents: [TextPanel]
})
export class TextPanelModule {}