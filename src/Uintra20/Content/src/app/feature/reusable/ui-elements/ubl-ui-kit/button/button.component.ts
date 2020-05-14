import { Component, Input } from '@angular/core';
import { UrlType } from 'src/app/shared/enums/url-type';
import { IButtonData } from 'src/app/shared/interfaces/panels/text/text-panel.interface';

@Component({
  selector: 'app-button',
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.less']
})
export class ButtonComponent {
  @Input() data: IButtonData;
  @Input() withArrow: Boolean;

  urlType = UrlType;

  ngOnInit()
  {
    if (!this.data) return;

    if (this.data.target) this.data.type = UrlType.External;

    if (this.data.type === UrlType.Media)
    {
      if (!this.data.target) this.data.target = '_blank';
    }
  }
}
