import { Component, OnInit, Input } from '@angular/core';
import { IBreadcrumbsItem } from 'src/app/feature/specific/groups/groups.interface';

@Component({
  selector: 'app-breadcrumbs',
  templateUrl: './breadcrumbs.component.html',
  styleUrls: ['./breadcrumbs.component.less']
})
export class BreadcrumbsComponent implements OnInit {
  @Input() breadcrumbs: IBreadcrumbsItem[];

  constructor() { }

  ngOnInit() {
  }

}
