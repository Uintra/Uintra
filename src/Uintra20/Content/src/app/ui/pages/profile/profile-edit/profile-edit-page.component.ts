import { Component, ViewEncapsulation, OnInit, HostListener, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Validators, FormGroup, FormControl } from '@angular/forms';
import { ProfileService } from './services/profile.service';
import { NotifierTypeEnum } from 'src/app/shared/enums/notifier-type.enum';
import { HasDataChangedService } from 'src/app/shared/services/general/has-data-changed.service';
import { Observable, Subscription } from 'rxjs';
import { CanDeactivateGuard } from 'src/app/shared/services/general/can-deactivate.service';
import { IProfileEditPage } from 'src/app/shared/interfaces/pages/profile/profile-edit-page.interface';
import { AddButtonService } from 'src/app/ui/main-layout/left-navigation/components/my-links/add-button.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'profile-edit-page',
  templateUrl: './profile-edit-page.html',
  styleUrls: ['./profile-edit-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class ProfileEditPage implements OnInit, OnDestroy {

  private $updateSubscription: Subscription;
  private $notificationSubscription: Subscription;

  public data: IProfileEditPage;
  public profileEditForm: FormGroup;
  public inProgress = false;
  public isUploaded = false;

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private profileService: ProfileService,
    private addButtonService: AddButtonService,
    private hasDataChangedService: HasDataChangedService,
    private canDeactivateService: CanDeactivateGuard,
    private translate: TranslateService,
  ) {
    this.activatedRoute.data.subscribe((data: IProfileEditPage) => {
      this.data = data;
      this.addButtonService.setPageId(data.id.toString());
    });
  }

  @HostListener('window:beforeunload') checkIfDataChanged() {
    return !this.hasDataChangedService.hasDataChanged || !this.checkIfdataChanged();
  }

  public ngOnInit(): void {
    this.onInitForm();
  }

  public ngOnDestroy(): void {
    this.$updateSubscription.unsubscribe();
    this.$notificationSubscription.unsubscribe();
  }

  private onInitForm = (): void => {
    this.profileEditForm = new FormGroup(
      {
        firstName: new FormControl(this.data.profile.firstName, Validators.required),
        lastName: new FormControl(this.data.profile.lastName, Validators.required),
        phone: new FormControl(this.data.profile.phone),
        department: new FormControl(this.data.profile.department)
      }
    );
  }

  public handleSave(): void {
    this.inProgress = true;
    const profile = {
      id: this.data.profile.id,
      firstName: this.profileEditForm.value.firstName,
      lastName: this.profileEditForm.value.lastName,
      phone: this.profileEditForm.value.phone,
      department: this.profileEditForm.value.department,
      photo: this.data.profile.photo,
      photoId: this.data.profile.photoId,
      email: this.data.profile.email,
      profileUrl: this.data.profile.profileUrl,
      mediaRootId: this.data.profile.mediaRootId,
      newMedia: this.data.profile.newMedia,
      memberNotifierSettings: this.data.profile.memberNotifierSettings,
      tagIdsData: this.data.profile.tags.map(t => t.id)
    };

    this.$updateSubscription = this.profileService.update(profile)
      .subscribe(
        (next: any) => {
          this.hasDataChangedService.reset();
          this.resetDataChecker();
          this.router.navigate([next.originalUrl]);
        },
        (err: any) => this.inProgress = false);
  }

  public handleUpdateNotificationSettings(event): void {
    event.preventDefault();
    if (confirm(this.translate.instant('profile.NotifierSettings.Confirmation.lbl'))) {
      this.$notificationSubscription = this.profileService.updateNotificationSettings({
        notifierTypeEnum: NotifierTypeEnum[NotifierTypeEnum.EmailNotifier],
        isEnabled: event.target.checked
      }).subscribe(
        (next) => { },
        (error) => {
          this.data.profile.memberNotifierSettings.emailNotifier =
            !this.data.profile.memberNotifierSettings.emailNotifier;
        }
      );
    } else {
      this.data.profile.memberNotifierSettings.emailNotifier = !event.target.checked;
    }
  }

  public processAvatarUpload(fileArray: Array<any> = []): void {
    this.isUploaded = true;
    this.data.profile.newMedia = fileArray[1];
    this.hasDataChangedService.onDataChanged();
  }

  public processAvatarDelete(): void {
    if (this.data.profile.newMedia) {
      this.data.profile.newMedia = null;
    } else {
      if (confirm(this.translate.instant('profile.DeletePhotoConfirm.lbl'))) {
        const currentPhoto = this.data.profile.photo;
        this.data.profile.photo = null;
        this.profileService.deletePhoto(this.data.profile.photoId).subscribe(
          (res) => {
            this.hasDataChangedService.onDataChanged();
          },
          (err) => {
            this.data.profile.photo = currentPhoto;
          }
        );
      }
    }
  }

  public onTagsChange(e) {
    if (this.data.profile.tags !== e) {
      this.hasDataChangedService.onDataChanged();
    }
    this.data.profile.tags = e;
  }

  public checkIfdataChanged(): boolean {
    return this.data.profile.firstName !== this.profileEditForm.value.firstName ||
      this.data.profile.lastName !== this.profileEditForm.value.lastName ||
      this.data.profile.phone !== this.profileEditForm.value.phone ||
      this.data.profile.department !== this.profileEditForm.value.department;
  }

  public resetDataChecker(): void {
    this.data.profile.firstName = this.profileEditForm.value.firstName;
    this.data.profile.lastName = this.profileEditForm.value.lastName;
    this.data.profile.phone = this.profileEditForm.value.phone;
    this.data.profile.department = this.profileEditForm.value.department;
  }

  public canDeactivate(): Observable<boolean> | boolean {
    if (this.hasDataChangedService.hasDataChanged || this.checkIfdataChanged()) {
      return this.canDeactivateService.canDeacrivateConfirm();
    }

    return true;
  }
}
