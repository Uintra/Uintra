import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-publication-header',
  templateUrl: './publication-header.component.html',
  styleUrls: ['./publication-header.component.less']
})
export class PublicationHeaderComponent implements OnInit {

  @Input() avatar: string;
  @Input() title: string;
  @Input() originalUrl?: string;
  @Input() params?: Array<any>;

  ngOnInit(): void {

  }

}
