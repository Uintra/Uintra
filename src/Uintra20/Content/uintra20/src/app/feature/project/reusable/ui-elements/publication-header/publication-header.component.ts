import { Component, Input, OnInit } from '@angular/core';
import { IUlinkWithTitle } from 'src/app/feature/shared/interfaces/IULink';

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
  @Input() groupInfo?: IUlinkWithTitle;

  ngOnInit(): void {

  }

}
