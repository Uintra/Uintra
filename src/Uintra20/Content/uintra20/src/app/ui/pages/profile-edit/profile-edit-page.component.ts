import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import ParseHelper from 'src/app/feature/shared/helpers/parse.helper';
import { IProfileEditPage } from './profile-edit-page.interface';
import { Validators, FormGroup, FormControl } from '@angular/forms';
import { ProfileService } from './services/profile.service';
import { finalize } from 'rxjs/operators';
import { NotifierTypeEnum } from 'src/app/feature/shared/enums/notifier-type.enum';

@Component({
  selector: 'profile-edit-page',
  templateUrl: './profile-edit-page.html',
  styleUrls: ['./profile-edit-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class ProfileEditPage implements OnInit {
  files = [];
  private data: any;
  public profileEdit: IProfileEditPage;
  public profileEditForm: FormGroup;
  public inProgress = false;
  public isUploaded = false;
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private profileService: ProfileService
  ) {
    this.route.data.subscribe(data => this.data = data);
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
      .pipe(finalize(() => this.inProgress = false))
      .subscribe(
        () => this.router.navigate([this.profileEdit.url])
      );
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

  processAvatarUpload(fileArray: Array<any> = []): void {
    this.files.push(fileArray);
    this.isUploaded = true;
    this.profileEdit.member.newMedia = fileArray[1];
  }

  processAvatarDelete() {
    this.profileService.deletePhoto(this.profileEdit.member.photoId)
      .subscribe(
        () => {
          this.files = [];
          this.profileEdit.member.photo = null;
        }
      );
  }
}
