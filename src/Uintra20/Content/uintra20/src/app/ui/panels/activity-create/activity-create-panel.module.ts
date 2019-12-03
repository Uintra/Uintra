import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { AS_DYNAMIC_COMPONENT, NotImplementedModule } from '@ubaseline/next';
import { ActivityCreatePanel } from './activity-create-panel.component';
import { RichTextEditorModule } from 'src/app/feature/project/reusable/inputs/rich-text-editor/rich-text-editor.module';

@NgModule({
  declarations: [ActivityCreatePanel],
  imports: [
    CommonModule,
    NotImplementedModule,
    FormsModule,
    RichTextEditorModule.configure({
      modules: {
        counter: {
          maxLength: 2000
        }
      }
    })
  ],
  providers: [{provide: AS_DYNAMIC_COMPONENT, useValue: ActivityCreatePanel}],
  entryComponents: [ActivityCreatePanel]
})
export class ActivityCreatePanelModule {}
