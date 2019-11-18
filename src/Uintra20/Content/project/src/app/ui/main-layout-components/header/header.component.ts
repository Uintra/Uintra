import { Component, OnInit, Input } from '@angular/core';
import { ISiteHeaderSettings } from 'src/app/shared/interface/site-settings';
import { INavigationItem } from 'src/app/service/navigation.service';
import { PictureData } from 'src/app/dynamic-module/lib/helper/picture-data';
import { IPictureData } from 'src/app/shared/components/picture/picture.component';
import { DrawerService } from 'src/app/dynamic-module/lib/drawer/service/drawer.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.less']
})
export class HeaderComponent implements OnInit {
  @Input() navigation: INavigationItem[];
  @Input() settings: ISiteHeaderSettings;
  logo: IPictureData;

  constructor(
    public drawerService: DrawerService
  ) { }

  ngOnInit()
  {
    this.logo = PictureData.fromUrlWithRect('wwwroot/assets/img/logo.svg', {width: 92, height: 92});
  }
}
