import { Component, ViewEncapsulation } from '@angular/core';
import { ITextPanelData } from 'src/app/shared/interfaces/panels/text/text-panel.interface';

@Component({
  selector: 'text-panel',
  templateUrl: './text-panel.html',
  styleUrls: ['./text-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class TextPanel {
  data: ITextPanelData;
}