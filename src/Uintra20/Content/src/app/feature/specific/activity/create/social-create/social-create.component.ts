import {
  Component,
  OnInit,
  Input,
  ViewChild,
  HostListener
} from '@angular/core';
import { DropzoneComponent } from 'ngx-dropzone-wrapper';
import { ModalService } from 'src/app/shared/services/general/modal.service';
import { ITagData } from 'src/app/feature/reusable/inputs/tag-multiselect/tag-multiselect.interface';
import { IUserAvatar } from 'src/app/feature/reusable/ui-elements/user-avatar/user-avatar-interface';
import { ISocialCreate } from 'src/app/shared/interfaces/components/social/create/social-create.interface';
import { SocialPopUpComponent } from './components/social-pop-up/social-pop-up.component';
import { MqService } from 'src/app/shared/services/general/mq.service';

@Component({
  selector: 'app-social-create',
  templateUrl: './social-create.component.html',
  styleUrls: ['./social-create.component.less']
})
export class SocialCreateComponent implements OnInit {
  @Input() public data: ISocialCreate;
  @ViewChild('dropdownRef', { static: false }) public dropdownRef: DropzoneComponent;
  @HostListener('window:resize', ['$event']) public getScreenSize(event?) {
    this.deviceWidth = window.innerWidth;
  }
  
  public deviceWidth: number;
  public userAvatar: IUserAvatar;

  constructor(
    private modalService: ModalService,
    private mq: MqService,
  ) { }

  public ngOnInit() {
    this.getPlaceholder();
    this.userAvatar = {
      name: this.data.data.creator.displayedName,
      photo: this.data.data.creator.photo
    };
  }

  public onShowPopUp(): void {
    if (this.data.canCreate) {
      this.showPopUp();
    }
  }

  public showPopUp(): void {
    this.modalService.addClassToRoot('disable-scroll');
    this.modalService.appendComponentToBody(SocialPopUpComponent, {data: this.data});
  }

  public canCreatePosts(): boolean {
    if (this.data) {
      return (
        this.data.canCreate ||
        this.data.createEventsLink ||
        this.data.createNewsLink
      );
    }
  }
  
  public getPlaceholder(): string {
    return this.mq.isTablet(this.deviceWidth)
      ? 'socialsCreate.PopupPlaceholder.lbl'
      : 'socialsCreate.MobileBtn.lbl';
  }
}
