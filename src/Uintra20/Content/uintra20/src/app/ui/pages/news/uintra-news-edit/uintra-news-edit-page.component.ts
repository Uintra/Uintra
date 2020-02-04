import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import ParseHelper from 'src/app/feature/shared/helpers/parse.helper';
import { INewsCreateModel, IOwner } from 'src/app/feature/project/specific/activity/activity.interfaces';
import { ITagData } from 'src/app/feature/project/reusable/inputs/tag-multiselect/tag-multiselect.interface';

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
  members: IOwner[];
  creator: IOwner;
  tags: ITagData[];

  constructor(
    private route: ActivatedRoute
  ) {
    this.route.data.subscribe(data => this.data = data);
  }

  ngOnInit(): void {
    this.panelData = ParseHelper.parseUbaselineData(this.data);
    this.details = this.panelData.details;
    this.members = (Object.values(this.panelData.members) as Array<any>) || [];
    this.creator = this.details.headerInfo.owner;
    this.tags = Object.values(this.details.availableTags) || [];

    this.newsData = {
      ownerId: this.details.ownerId,
      title: this.details.headerInfo.title,
      description: this.details.description,
      publishDate: this.details.publishDate,

      unpublishDate: this.details.unpublishDate,
      media: {
        medias: (Object.values(this.details.lightboxPreviewModel.medias) as Array<any>) || [],
        otherFiles: (Object.values(this.details.lightboxPreviewModel.otherFiles) as Array<any>) || [],
      },

      endPinDate: this.details.endPinDate,
      // mediaRootId: number;
      // tagIdsData?: string[];

      isPinned: this.details.isPinned,
      location: {
        address: this.details.location.address,
        shortAddress:  this.details.location.shortAddress,
      },
      tags: (Object.values(this.details.tags) as Array<any>) || []
    }
  }

  onSubmit(event) {
    // debugger;
  }
}
