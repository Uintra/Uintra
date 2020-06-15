import { Component, HostBinding } from '@angular/core';
import { ITextPanelData } from 'src/app/shared/interfaces/panels/text/text-panel.interface';
import { resolveThemeCssClass } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/core/helpers/panel-settings';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'text-panel',
  templateUrl: './text-panel.html',
  styleUrls: ['./text-panel.less']
})
export class TextPanel {
  data: ITextPanelData;
  @HostBinding('class') hostClasses;

  constructor(private sanitized: DomSanitizer) { }

  get description() {
    return this.sanitized.bypassSecurityTrustHtml(this.data.description);
  }

  ngOnInit(){
    this.hostClasses = resolveThemeCssClass(this.data.panelSettings);
  }
}
