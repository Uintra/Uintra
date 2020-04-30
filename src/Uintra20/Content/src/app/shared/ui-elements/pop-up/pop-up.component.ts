import { Component, OnInit, HostBinding } from '@angular/core';
import { ModalService } from '../../services/general/modal.service';

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

  constructor(private modalService: ModalService) { }

  ngOnInit() {
    this.modalService.closePopUpSubject.subscribe(() => {
      this.className = this.needsShadowBackground ? 'pop-up pop-up--with-shadow' : 'pop-up';
    })
    this.className = this.needsShadowBackground ? 'pop-up pop-up--with-shadow' : 'pop-up';
  }

  closePopUp(e) {
    this.modalService.removeComponentFromBody(this.id);
    this.modalService.closePopUpSubject.next();
  }
}
