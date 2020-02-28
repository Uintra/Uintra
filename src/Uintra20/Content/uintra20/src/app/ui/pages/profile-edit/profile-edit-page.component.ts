import { Component, ViewEncapsulation, OnInit, HostListener } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import ParseHelper from 'src/app/feature/shared/helpers/parse.helper';
import { IProfileEditPage } from '../../../feature/shared/interfaces/pages/profile/edit/profile-edit-page.interface';
import { Validators, FormGroup, FormControl } from '@angular/forms';
import { ProfileService } from './services/profile.service';
import { NotifierTypeEnum } from 'src/app/feature/shared/enums/notifier-type.enum';
import { AddButtonService } from '../../main-layout/left-navigation/components/my-links/add-button.service';
import { HasDataChangedService } from 'src/app/services/general/has-data-changed.service';
import { Observable } from 'rxjs';
import { CanDeactivateGuard } from 'src/app/services/general/can-deactivate.service';

@Component({
  selector: 'profile-edit-page',
  templateUrl: './profile-edit-page.html',
  styleUrls: ['./profile-edit-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class ProfileEditPage implements OnInit {
  @HostListener('window:beforeunload') checkIfDataChanged() {
    return !this.hasDataChangedService.hasDataChanged || !this.checkIfdataChanged();
  }
  files = [];
  private data: any;
  public profileEdit: IProfileEditPage;
  public profileEditForm: FormGroup;
  public inProgress = false;
  public isUploaded = false;
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private profileService: ProfileService,
    private addButtonService: AddButtonService,
    private hasDataChangedService: HasDataChangedService,
    private canDeactivateService: CanDeactivateGuard,
  ) {
    this.route.data.subscribe(data => {
      this.data = data;
      this.addButtonService.setPageId(data.id);
    });
  }

  public ngOnInit(): void {
    this.onParse();
    this.onInitForm();
  }

  private onInitForm = (): void => {
    this.profileEditForm = new FormGroup(
      {
        firstName: new FormControl(this.profileEdit.member.firstName, Validators.required),
        lastName: new FormControl(this.profileEdit.member.lastName, Validators.required),
        phone: new FormControl(this.profileEdit.member.phone),
        department: new FormControl(this.profileEdit.member.department)
      }
    );
  }

  private onParse = (): void => {
    const parsed = ParseHelper.parseUbaselineData(this.data);
    this.profileEdit = {
      title: parsed.name,
      url: parsed.url,
      member: {
        id: parsed.profile.id,
        firstName: parsed.profile.firstName,
        lastName: parsed.profile.lastName,
        phone: parsed.profile.phone,
        department: parsed.profile.department,
        photo: parsed.profile.photo,
        photoId: parsed.profile.photoId,
        email: parsed.profile.email,
        profileUrl: parsed.profile.profileUrl,
        mediaRootId: parsed.profile.mediaRootId,
        newMedia: parsed.profile.newMedia,
        memberNotifierSettings: ParseHelper.parseUbaselineData(this.data.profile.data.memberNotifierSettings),
        tags: Object.values(parsed.tags),
        availableTags: Object.values(parsed.availableTags)
      }
    };
  }

  public handleSave(): void {
    this.inProgress = true;
    const profile = {
      id: this.profileEdit.member.id,
      firstName: this.profileEditForm.value.firstName,
      lastName: this.profileEditForm.value.lastName,
      phone: this.profileEditForm.value.phone,
      department: this.profileEditForm.value.department,
      photo: this.profileEdit.member.photo,
      photoId: this.profileEdit.member.photoId,
      email: this.profileEdit.member.email,
      profileUrl: this.profileEdit.member.profileUrl,
      mediaRootId: this.profileEdit.member.mediaRootId,
      newMedia: this.profileEdit.member.newMedia,
      memberNotifierSettings: this.profileEdit.member.memberNotifierSettings,
      tagIdsData: this.profileEdit.member.tags.map(t => t.id)
    };

    this.profileService.update(profile)
      .subscribe((next: any) => {
        this.hasDataChangedService.reset();
        this.resetDataChecker();
        this.router.navigate([next.originalUrl]);
      },
      (err) => {
        this.inProgress = false;
      });
  }

  public handleUpdateNotificationSettings(event): void {
    event.preventDefault();
    if (confirm('Are you sure')) {
      this.profileService.updateNotificationSettings({
        notifierTypeEnum: NotifierTypeEnum[NotifierTypeEnum.EmailNotifier],
        isEnabled: event.target.checked
      }).subscribe(
        (next) => { },
        (error) => {
          this.profileEdit.member.memberNotifierSettings.emailNotifier =
            !this.profileEdit.member.memberNotifierSettings.emailNotifier;
        }
      );
    } else {
      this.profileEdit.member.memberNotifierSettings.emailNotifier = !event.target.checked;
    }
  }

  public processAvatarUpload(fileArray: Array<any> = []): void {
    this.files.push(fileArray);
    this.isUploaded = true;
    this.profileEdit.member.newMedia = fileArray[1];
    this.hasDataChangedService.onDataChanged();
  }

  public processAvatarDelete(): void {
    this.profileService.deletePhoto(this.profileEdit.member.photoId)
      .subscribe(
        () => {
          this.files = [];
          this.profileEdit.member.photo = null;
          this.hasDataChangedService.onDataChanged();
        }
      );
  }

  onTagsChange(e) {
    if (this.profileEdit.member.tags != e) {
      this.hasDataChangedService.onDataChanged();
    }
    this.profileEdit.member.tags = e;
  }

  checkIfdataChanged() {
    return this.profileEdit.member.firstName !== this.profileEditForm.value.firstName ||
      this.profileEdit.member.lastName !== this.profileEditForm.value.lastName ||
      this.profileEdit.member.phone !== this.profileEditForm.value.phone ||
      this.profileEdit.member.department !== this.profileEditForm.value.department;
  }

  resetDataChecker() {
    this.profileEdit.member.firstName = this.profileEditForm.value.firstName;
    this.profileEdit.member.lastName = this.profileEditForm.value.lastName;
    this.profileEdit.member.phone = this.profileEditForm.value.phone;
    this.profileEdit.member.department = this.profileEditForm.value.department;
  }

  canDeactivate(): Observable<boolean> | boolean {
    if (this.hasDataChangedService.hasDataChanged || this.checkIfdataChanged()) {
      return this.canDeactivateService.canDeacrivateConfirm();
    }

    return true;
  }
}
