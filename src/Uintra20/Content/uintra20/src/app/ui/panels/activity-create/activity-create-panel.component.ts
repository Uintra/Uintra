import { Component, ViewEncapsulation, ViewChild } from '@angular/core';
import { IActivityCreatePanel } from './activity-create-panel.interface';
import { ITagData } from 'src/app/feature/project/reusable/inputs/tag-multiselect/tag-multiselect.interface';
import { DropzoneComponent } from 'ngx-dropzone-wrapper';
import { CreateSocialContentService } from './services/create-social-content.service';

@Component({
  selector: 'activity-create-panel',
  templateUrl: './activity-create-panel.html',
  styleUrls: ['./activity-create-panel.less'],
  encapsulation: ViewEncapsulation.None
})
export class ActivityCreatePanel {
  @ViewChild('dropdownRef', { static: false }) dropdownRef: DropzoneComponent;

  data: IActivityCreatePanel;
  availableTags: Array<ITagData> = [];
  isPopupShowing: boolean = false;

  tags: any = [];
  description: string;

  constructor(private socialContentService: CreateSocialContentService) {}

  ngOnInit() {
    this.availableTags = [
      { id: '1', text: 'test' },
      { id: '2', text: 'testtest' },
      { id: '3', text: 'test2' },
      { id: '4', text: 'testtest2' },
      { id: '5', text: 'test3' },
      { id: '6', text: 'testtest3' }
    ];
  }

  onShowPopUp() {
    this.isPopupShowing = true;
  }
  onHidePopUp() {
    this.isPopupShowing = false;
  }

  addAttachment() {
    this.dropdownRef.directiveRef.dropzone().clickableElements[0].click();
  }

  onSubmit() {
    this.socialContentService.submitSocialContent({
      description: this.description,
      OwnerId: 'cb6969e1-ac68-4cae-88a3-8b1cbc453ef7',
      title: ''
    })
  }
}
