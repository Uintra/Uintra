import { Component, OnInit, Input } from '@angular/core';
import { IActivityCreatePanel } from '../../activity-create-panel.interface';
import { CreateActivityService } from 'src/app/feature/project/specific/activity/create-activity.service';
import { INewsCreateModel } from 'src/app/feature/project/specific/activity/create-activity.interface';
import ParseHelper from 'src/app/feature/shared/helpers/parse.helper';

@Component({
  selector: 'app-news-create',
  templateUrl: './news-create.component.html',
  styleUrls: ['./news-create.component.less']
})
export class NewsCreateComponent implements OnInit {
  @Input() data: IActivityCreatePanel;
  newsData: INewsCreateModel;
  members: Array<any>;
  creator: any;
  tags: Array<any>;

  panelData;

  constructor(private createActivityService: CreateActivityService) { }

  ngOnInit() {
    this.panelData = ParseHelper.parseUbaselineData(this.data);
    this.members = Object.values(this.panelData.members) as Array<any> || [];
    this.creator = this.panelData.creator;
    this.tags = Object.values(this.panelData.tags.userTagCollection);

    this.newsData = {
      ownerId: this.creator.id,
      title: null,
      description: null,
      publishDate: null,
    };
  }

  onSubmit(data) {
    this.createActivityService.submitNewsContent(data).subscribe((r) => {
      console.log(r);
    })
  }
}
