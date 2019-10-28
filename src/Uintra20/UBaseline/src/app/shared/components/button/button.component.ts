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
  domain: string;
  urlType = UrlType;

  constructor(private appConfigService: AppConfigService) {
    this.domain = appConfigService.getHostName();
  }
}
