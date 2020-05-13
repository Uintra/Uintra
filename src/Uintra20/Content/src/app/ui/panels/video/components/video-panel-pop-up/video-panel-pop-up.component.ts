import { Component, OnInit, Input } from '@angular/core';
import { ModalService } from 'src/app/shared/services/general/modal.service';

@Component({
  selector: 'app-video-panel-pop-up',
  templateUrl: './video-panel-pop-up.component.html',
  styleUrls: ['./video-panel-pop-up.component.less']
})
export class VideoPanelPopUpComponent implements OnInit {
  @Input() data: any;
  readonly UMEDIA_TYPE: number = 2;

  constructor(private modalService: ModalService) { }

  ngOnInit() {
  }

  close() {
    this.modalService.removeComponentFromBody();
  }
}
