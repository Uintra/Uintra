import { Component, Input } from '@angular/core';
import { UrlType } from '../../enum/url-type';
import { LinkTargetType } from '../../interface/link';
import { AppConfigService } from '../../../service/app-config.service';

export interface IButtonData {
  type?: UrlType;
  name?: string;
  queryString?: string;
  target?: LinkTargetType;
  url?: string;
}

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
