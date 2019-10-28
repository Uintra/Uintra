import { Component, OnInit, Input } from '@angular/core';

export interface IFooterSettings {
  footerLogo: any;
  footerItems: any[];
}
@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.less']
})
export class FooterComponent implements OnInit {
  @Input() settings: IFooterSettings;
  constructor() { }

  ngOnInit() {
  }

}
