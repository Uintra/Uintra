import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import ParseHelper from 'src/app/feature/shared/helpers/parse.helper';
import { INewsCreateModel } from 'src/app/feature/project/specific/activity/activity.interfaces';

@Component({
  selector: 'uintra-news-edit-page',
  templateUrl: './uintra-news-edit-page.html',
  styleUrls: ['./uintra-news-edit-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class UintraNewsEditPage implements OnInit{

  data: any;
  newsData: INewsCreateModel;
  panelData: any;
  details: any;
  members: any;

  constructor(
    private route: ActivatedRoute
  ) {
    this.route.data.subscribe(data => this.data = data);
  }

  ngOnInit(): void {
    this.panelData = ParseHelper.parseUbaselineData(this.data);
    this.details = this.panelData.details;
    this.members = (Object.values(this.data.members) as Array<any>) || [];

    this.newsData = {
      ownerId: this.details.ownerId,
      title: this.details.headerInfo.title,
      description: this.details.description,
      publishDate: this.details.publishDate,

      unpublishDate: this.details.unpublishDate,
      media: this.details.media,

      endPinDate: this.details.endPinDate,
      // mediaRootId: number;
      // tagIdsData?: string[];

      isPinned: this.details.isPinned
      // activityLocationEditModel?: {
      //   address?: string;
      //   shortAddress?: string;
      // };
    }
  }

  onSubmit(event) {

  }
}
