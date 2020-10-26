import { Component, OnInit, HostBinding } from '@angular/core';
import { ModalService } from '../../services/general/modal.service';
import { NavNotificationsService } from 'src/app/feature/specific/nav-notifications/nav-notifications.service';

@Component({
  selector: 'app-pop-up',
  templateUrl: './pop-up.component.html',
  styleUrls: ['./pop-up.component.less']
})
export class PopUpComponent implements OnInit {
  @HostBinding('class') className: string;

  data: any;
  id: string;
  needsShadowBackground: boolean;

  constructor(private modalService: ModalService, private notificationsService: NavNotificationsService) { }

  ngOnInit() {
    this.className = 'pop-up';
  }

  closePopUp(e) {
    this.modalService.removeComponentFromBody(this.id);
    this.modalService.closePopUpSubject.next();
    this.notificationsService.markPopUpAsClosed(this.data.Id).subscribe(res => console.log(res));
  }
}
