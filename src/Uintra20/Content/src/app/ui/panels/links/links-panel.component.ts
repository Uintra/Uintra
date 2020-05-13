import { Component, ViewEncapsulation, HostBinding } from '@angular/core';
import { ILinksPanel } from '../../../shared/interfaces/panels/links/links-panel.interface';
import { Link } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/core/interface/link';
import { resolveThemeCssClass } from 'src/app/feature/reusable/ui-elements/ubl-ui-kit/core/helpers/panel-settings';

@Component({
  selector: 'links-panel',
  templateUrl: './links-panel.html',
  styleUrls: ['./links-panel.less']//,
  //encapsulation: ViewEncapsulation.None
})
export class LinksPanel {
  data: ILinksPanel;
  links: Link[] = [];
  @HostBinding('class') hostClasses;

  ngOnInit() {
    this.hostClasses = resolveThemeCssClass(this.data.panelSettings);
    if (this.data && this.data.links && this.data.links.length) {
      this.links = this.data.links.map(i => Link.fromButtonData(i));
    }
  }
}
