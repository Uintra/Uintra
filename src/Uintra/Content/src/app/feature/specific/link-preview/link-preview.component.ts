import { Component, OnInit, Input, NgZone } from '@angular/core';
import { ILinkPreview } from '../../reusable/inputs/rich-text-editor/rich-text-editor.interface';
import { RichTextEditorService } from '../../reusable/inputs/rich-text-editor/rich-text-editor.service';

@Component({
  selector: 'app-link-preview',
  templateUrl: './link-preview.component.html',
  styleUrls: ['./link-preview.component.less']
})
export class LinkPreviewComponent implements OnInit {
  @Input() firstLinkPreview: ILinkPreview;
  @Input() editor: any;

  constructor(private richTextEditorService: RichTextEditorService) { }

  ngOnInit() {
  }

  public closeLinkPreview(): void {
    this.editor.linksToSkip.push(this.editor.firstLinkPreview && this.editor.firstLinkPreview.uri);
    this.richTextEditorService.getLinkPreview(this.editor);
  }
}
