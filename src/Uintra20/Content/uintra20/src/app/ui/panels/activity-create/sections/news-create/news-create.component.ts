import { Component, OnInit, Input } from '@angular/core';
import ParseHelper from 'src/app/feature/shared/helpers/parse.helper';
import { IActivityCreatePanel } from '../../activity-create-panel.interface';

@Component({
  selector: 'app-news-create',
  templateUrl: './news-create.component.html',
  styleUrls: ['./news-create.component.less']
})
export class NewsCreateComponent implements OnInit {
  @Input() data: IActivityCreatePanel;

  constructor() { }

  ngOnInit( ) {
  }

  onSubmit() {
    // const requestModel: INewsCreateModel = {

    // };
    // this.newsCreateService
    //   .submitNewsContent({
    //     //TODO Add model
    //   });
  }
}

