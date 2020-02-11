import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'uintra-groups-create-page',
  templateUrl: './uintra-groups-create-page.html',
  styleUrls: ['./uintra-groups-create-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class UintraGroupsCreatePage {
  data: any;
  title: string = "";
  description: string = "";
  files: any[] = [];
  isShowValidation: boolean = false;
  inProgress: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient
  ) {
    this.route.data.subscribe(data => this.data = data);
  }

  get isSubmitDisabled() {
    return this.inProgress;
  }

  ngOnInit() {

  }

  onUploadSuccess(fileArray: Array<any> = []): void {
    this.files.push(fileArray);
  }

  onFileRemoved(removedFile: object) {
    this.files = this.files.filter(file => file[0] !== removedFile);
  }

  onSubmit() {
    if (this.validate()) {
      this.inProgress = true;

      const groupCreateModel = {
        title: this.title,
        description: this.description,
        newMedia: this.getMediaIdsForResponse(),
        media: null
      }

      this.http.post('/ubaseline/api/Group/Create', groupCreateModel).pipe(
        finalize(() => this.inProgress = false)
      ).subscribe(res => {});
    }
  }

  validate() {
    if (this.title && this.description && this.files.length < 2) {
      return true;
    }

    this.isShowValidation = true;
    return false;
  }

  getMediaIdsForResponse(): string {
    return this.files.map(file => file[1]).join(',');
  }
}
