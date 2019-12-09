import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-publication-header',
  templateUrl: './publication-header.component.html',
  styleUrls: ['./publication-header.component.less']
})
export class PublicationHeaderComponent implements OnInit {
  // TODO: use IPictureData instead of string
  @Input() avatar: string;
  @Input() title: string;

  constructor() { }

  ngOnInit() {

  }

}
