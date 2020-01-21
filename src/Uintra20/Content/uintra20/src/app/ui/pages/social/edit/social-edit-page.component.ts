import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import ParseHelper from '../../../../feature/shared/helpers/parse.helper';
import { ISocialEdit } from './social-edit-page.interface';
import { SocialService } from '../services/social.service';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'social-edit',
  templateUrl: './social-edit-page.component.html',
  styleUrls: ['./social-edit-page.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class SocialEditPageComponent {

  private data: any;
  public inProgress = false;
  public socialEdit: ISocialEdit;

  constructor(
    private route: ActivatedRoute,
    private socialService: SocialService,
    private router: Router
  ) {
    this.route.data.subscribe(data => this.data = data);
    this.onParse();
  }

  private onParse = (): void => {
    this.socialEdit = ParseHelper.parseUbaselineData(this.data);
    this.socialEdit.tags = Object.values(this.socialEdit.tags);
    this.socialEdit.availableTags = Object.values(this.socialEdit.availableTags);
    this.socialEdit.lightboxPreviewModel = Object.values(this.socialEdit.lightboxPreviewModel);
  }

  public handleUpload(fileArray: Array<any> = []): void {
    console.log('uploaded');
  }

  public handleRemove(removedFile: object): void {
    console.log('removed');
  }

  public handleSocialUpdate(): void {
    const populated = this.clone(this.socialEdit);
    populated.tagIdsData = this.socialEdit.tags.map( t => t.id);

    this.inProgress = true;
    this.socialService.update(populated)
      .pipe(finalize(() => this.inProgress = false))
      .subscribe(
        (next) => {
          const route = `social-details?id=${this.socialEdit.id}`; // TODO Fix after adding linkService on backend

          this.router.navigate([route]);
        },
        (error) => { },
        () => { }
      );
  }

  // TODO: Add usage of alertify or smth similiar
  public handleSocialDelete(): void {
    this.inProgress = true;
    this.socialService.delete(this.socialEdit.id)
      .pipe(finalize(() => this.inProgress = false))
      .subscribe(
        (next) => {
          // this.router.navigate(['/socials']); // TODO: socials doesnt exist, uncomment code when it will be done.
        },
        (error) => { },
        () => { }
      );
  }

  //TODO: Research about deep copy then
  private clone(obj: any): any {
    return Object.assign({}, obj);
  }
}
