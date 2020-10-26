import { Component, OnInit, Input } from '@angular/core';
import { ModalService } from 'src/app/shared/services/general/modal.service';

@Component({
  selector: 'app-modal-video',
  templateUrl: './modal-video.component.html',
  styleUrls: ['./modal-video.component.less']
})
export class ModalVideoComponent implements OnInit {
  @Input() data: any;
  readonly UMEDIA_TYPE: number = 2;

  constructor(private modalService: ModalService) { }

  ngOnInit() {
  }

  close() {
    this.modalService.removeComponentFromBody();
  }
}
