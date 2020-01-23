import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import ParseHelper from 'src/app/feature/shared/helpers/parse.helper';
import { IProfileEditPage } from './profile-edit-page.interface';
import { Validators, FormGroup, FormControl } from '@angular/forms';
import { ProfileService } from './services/profile.service';

@Component({
  selector: 'profile-edit-page',
  templateUrl: './profile-edit-page.html',
  styleUrls: ['./profile-edit-page.less'],
  encapsulation: ViewEncapsulation.None
})
export class ProfileEditPage implements OnInit {

  private data: any;
  public profileEdit: IProfileEditPage;
  public profileEditForm: FormGroup;

  constructor(
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
      }
    );
  }

  private onParse = (): void => {
    const parsed = ParseHelper.parseUbaselineData(this.data);
    this.profileEdit = {
      title: parsed.name,
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
        memberNotifierSettings: parsed.profile.memberNotifierSettings
      }
    };
  }

  //TODO: pass data
  public handleSave(): void {
    const profile = {
     id: null,
     firstName: null,
     lastName: null,
     phone: null,
     department: null,
     photo: null,
     photoId: null,
     email : null,
     profileUrl: null,
     MediaRootId: null,
     NewMedia: null,
     MemberNotifierSettings: null
    };

    // this.profileService.update(profile);
  }
}
