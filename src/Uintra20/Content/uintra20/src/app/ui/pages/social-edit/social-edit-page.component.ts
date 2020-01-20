import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import ParseHelper from '../../../feature/shared/helpers/parse.helper';
import { ISocialEdit } from './social-edit-page.interface';

@Component({
  selector: 'social-edit',
  templateUrl: './social-edit-page.component.html',
  styleUrls: ['./social-edit-page.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class SocialEditPageComponent {

  private data: any;
  public socialEdit: ISocialEdit;

  constructor(
    private route: ActivatedRoute
  ) {
    this.route.data.subscribe(data => this.data = data);
    this.onParse();
  }

  private onParse = (): void => {
    this.socialEdit = ParseHelper.parseUbaselineData(this.data);
    this.socialEdit.tags = Object.values(this.socialEdit.tags);
    this.socialEdit.availableTags = Object.values(this.socialEdit.availableTags);
  }

  public handleUpload(fileArray: Array<any> = []): void {
    console.log('uploaded');
  }

  public handleRemove(removedFile: object): void {
    console.log('removed');
  }

  public handleSave(): void {
    console.log('save');
  }

  public handleDelete(): void {
    console.log('delete');
  }
}
