import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ISocialDetails, IUserTag } from './social-details.interface';
import { ActivityEnum } from 'src/app/feature/shared/enums/activity-type.enum';
import { ILikeData } from 'src/app/feature/project/reusable/ui-elements/like-button/like-button.interface';
import { ImageGalleryService } from 'src/app/feature/project/reusable/ui-elements/image-gallery/image-gallery.service';

@Component({
  selector: 'social-details',
  templateUrl: './social-details-page.component.html',
  styleUrls: ['./social-details-page.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class SocialDetailsPanelComponent implements OnInit {

  data: any;
  details: ISocialDetails;
  tags: Array<IUserTag>;
  activityName: string;
  medias: Array<string>;

  constructor(
    private route: ActivatedRoute,
    private imgService: ImageGalleryService
  ) {
    this.route.data.subscribe(data => this.data = this.addActivityTypeProperty(data));
   }

  private addActivityTypeProperty(data) {
    // TODO investigate UmbracoFlatProperty and refactor code below
    data.panels.data.value = data.panels.get().map(panel => {
      panel.data.value.activityType = data.details.get().activityType.get();
      return panel
    })

    return data;
  }

  public ngOnInit(): void {
    const parsedData = JSON.parse(JSON.stringify(this.data));

    this.details = parsedData.details;
    this.activityName = this.parseActivityType(this.details.activityType);
    this.tags = Object.values(parsedData.tags);
    this.medias = Object.values(parsedData.details.media);
  }

  public parseActivityType(activityType: number): string {
    return ActivityEnum[activityType];
  }

  openGallery() {
    this.imgService.open();
  }
}
