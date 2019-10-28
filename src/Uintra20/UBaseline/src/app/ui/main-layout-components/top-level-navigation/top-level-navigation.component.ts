import { Component, OnInit, Input } from '@angular/core';
import { INavigationItem } from 'src/app/service/navigation.service';

@Component({
  selector: 'app-top-level-navigation',
  templateUrl: './top-level-navigation.component.html',
  styleUrls: ['./top-level-navigation.component.less']
})
export class TopLevelNavigationComponent implements OnInit {
  @Input() items: INavigationItem[];

  ngOnInit() 
  {
  }
}
