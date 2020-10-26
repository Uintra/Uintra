import { Component, Input, HostListener, OnInit } from '@angular/core';
import { MAX_FILES_FOR_SINGLE } from 'src/app/shared/constants/dropzone/drop-zone.const';
import { IMedia } from '../../activity/activity.interfaces';
import { Router } from '@angular/router';
import { HasDataChangedService } from 'src/app/shared/services/general/has-data-changed.service';
import { TITLE_MAX_LENGTH } from 'src/app/shared/constants/activity/activity-create.const';
import { GroupsService } from '../groups.service';
import { TranslateService } from '@ngx-translate/core';
import { IGroupsForm } from 'src/app/shared/interfaces/components/groups-form/groups-form.interface';

export interface IEditGroupData {
  id?: string;
  title?: string;
  description?: string;
  media?: { 0: string };
  mediaPreview?: {
    medias: Array<IMedia>;
  };
}

@Component({
  selector: 'groups-form',
  templateUrl: './groups-form.component.html',
  styleUrls: ['./groups-form.component.less']
})
export class GroupsFormComponent implements OnInit {
  @Input()
  public data: IGroupsForm;
  @Input()
  public allowedExtensions: string;
  @Input('edit')
  public edit: any;

  public title = '';
  public description = '';
  public medias: string[] = [];
  public mediasPreview: IMedia[] = [];
  public isShowValidation = false;
  public inProgress = false;
  public hidingInProgress = false;
  public TITLE_MAX_LENGTH: number = TITLE_MAX_LENGTH;
  public files: any[] = [];

  constructor(
    private groupsService: GroupsService,
    private router: Router,
    private hasDataChangedService: HasDataChangedService,
    public translate: TranslateService,
  ) { }

  public ngOnInit(): void {
    this.edit = this.edit !== undefined;
    this.setDataToEdit();
  }

  @HostListener('window:beforeunload')
  checkIfDataChanged() {
    return !this.hasDataChangedService.hasDataChanged;
  }

  public get isSubmitDisabled() {
    return this.inProgress;
  }

  public onUploadSuccess(fileArray: Array<any> = []): void {
    this.files.push(fileArray);
    this.hasDataChangedService.onDataChanged();
  }

  public onFileRemoved(removedFile: object): void {
    this.files = this.files.filter(file => file[0] !== removedFile);
  }

  public onImageRemove(): void {
    this.medias = [];
    this.mediasPreview = [];
    this.hasDataChangedService.onDataChanged();
  }

  public onTitleChange(e): void {
    if (this.title !== e) {
      this.hasDataChangedService.onDataChanged();
    }
    this.title = e;
  }

  public onDescriptionChange(e): void {
    if (this.description !== e) {
      this.hasDataChangedService.onDataChanged();
    }
    this.description = e;
  }

  public onSubmit(): void {
    if (this.validate()) {
      this.inProgress = true;
      const groupModel = {
        title: this.title,
        description: this.description,
        newMedia: this.getMediaIdsForResponse(),
        media: null,
        id: this.data ? this.data.id : null,
      };

      if (!this.edit) {
        this.groupsService.createGroup(groupModel)
          .subscribe(res => {
            this.hasDataChangedService.reset();
            this.router.navigate([res.originalUrl]);
          },
            (err: any) => {
              this.inProgress = false;
            });
      } else {
        if (this.medias && this.medias.length) {
          groupModel.media = this.medias[0];
        }
        this.groupsService.editGroup(groupModel)
          .subscribe(res => {
            this.hasDataChangedService.reset();
            this.router.navigate([res.originalUrl]);
          },
            (err: any) => {
              this.inProgress = false;
            });
      }
    }
  }

  public onHide(): void {
    this.hidingInProgress = true;
    this.groupsService.hideGroup(this.data.id).subscribe(res => {
      this.hasDataChangedService.reset();
      this.router.navigate([res.originalUrl]);
      this.hidingInProgress = false;
    });
  }

  public validate(): boolean {
    if (this.title && this.description && this.files.length <= MAX_FILES_FOR_SINGLE) {
      return true;
    }

    this.isShowValidation = true;
    return false;
  }

  public getMediaIdsForResponse(): string {
    return this.files.map(file => file[1]).join(',');
  }

  public setDataToEdit(): void {
    if (this.data) {
      this.title = this.data.title;
      this.description = this.data.description;
      this.mediasPreview = this.data.mediaPreview ? this.data.mediaPreview.medias : [];
      this.medias = this.data.media;
    }
  }

  public getTitleValidationState() {
    return (this.isShowValidation && !this.title) || (this.isShowValidation && this.title.length > TITLE_MAX_LENGTH)
  }

  public getTitleLengthValidationMessage() {
    return this.translate.instant('groupEdit.TitleLengthValidation.lbl').replace('{{0}}', TITLE_MAX_LENGTH);
  }
}
