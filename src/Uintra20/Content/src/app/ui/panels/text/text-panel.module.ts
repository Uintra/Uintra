import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TextPanel } from './text-panel.component';
import { AS_DYNAMIC_COMPONENT } from 'ubaseline-next-for-uintra';
import { TextModule } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/text/text.module';
import { ButtonComponent } from './components/button/button.component';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [TextPanel, ButtonComponent],
  imports: [
    CommonModule,
    TextModule,
    RouterModule,
  ],
  providers: [{provide: AS_DYNAMIC_COMPONENT, useValue: TextPanel}],
  entryComponents: [TextPanel]
})
export class TextPanelModule {}