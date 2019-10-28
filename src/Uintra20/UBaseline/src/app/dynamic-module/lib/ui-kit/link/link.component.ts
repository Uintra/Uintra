import { Component, Input } from '@angular/core';
import { ILink } from '../../core/interface/link';

@Component({
  selector: 'ubl-link',
  templateUrl: './link.component.html',
  styleUrls: ['./link.component.less']
})
export class LinkComponent {
  @Input() data: ILink;
  @Input() download: boolean;
}
