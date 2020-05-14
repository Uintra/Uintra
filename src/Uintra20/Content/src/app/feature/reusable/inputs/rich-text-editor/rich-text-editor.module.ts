import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RichTextEditorComponent } from './rich-text-editor.component';
import { QuillModule, QUILL_CONFIG_TOKEN, QuillConfig } from 'ngx-quill';
import { FormsModule } from '@angular/forms';
import { RichTextEditorEmojiComponent } from './rich-text-editor-emoji/rich-text-editor-emoji.component';
import { ClickOutsideModule } from 'src/app/shared/directives/click-outside/click-outside.module';
import { LinkPreviewModule } from 'src/app/feature/specific/link-preview/link-preview.module';
import { TagMultiselectModule } from '../tag-multiselect/tag-multiselect.module';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [RichTextEditorComponent, RichTextEditorEmojiComponent],
  imports: [
    CommonModule,
    FormsModule,
    ClickOutsideModule,
    LinkPreviewModule,
    QuillModule.forRoot(),
    TagMultiselectModule,
    TranslateModule,
  ],
  exports: [
    RichTextEditorComponent
  ]
})
export class RichTextEditorModule {
  static configure(config: QuillConfig): ModuleWithProviders  {
    return {
      ngModule: RichTextEditorModule,
      providers: [
        { provide: QUILL_CONFIG_TOKEN, useValue: config }
      ]
    };
  }
}

