import { Component, OnInit, Input } from '@angular/core';
import { IActivityCreatePanel } from '../../activity-create-panel.interface';
import ParseHelper from 'src/app/feature/shared/helpers/parse.helper';

@Component({
  selector: 'app-news-create',
  templateUrl: './news-create.component.html',
  styleUrls: ['./news-create.component.less']
})
export class NewsCreateComponent implements OnInit {
  @Input() data: IActivityCreatePanel;
  panelData: any; //TODO create interface
  constructor() { }

  ngOnInit() {
    this.panelData = ParseHelper.parseUbaselineData(this.data);
  }

}
