import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RichTextEditorComponent } from './rich-text-editor.component';
import { QuillModule, QUILL_CONFIG_TOKEN, QuillConfig } from 'ngx-quill';
import { FormsModule } from '@angular/forms';
import { RichTextEditorEmojiComponent } from './rich-text-editor-emoji/rich-text-editor-emoji.component';
import { ClickOutsideModule } from 'src/app/shared/directives/click-outside/click-outside.module';

@NgModule({
  declarations: [RichTextEditorComponent, RichTextEditorEmojiComponent],
  imports: [
    CommonModule,
    FormsModule,
    ClickOutsideModule,
    QuillModule.forRoot(),
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

