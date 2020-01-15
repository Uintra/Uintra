import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-publication-header',
  templateUrl: './publication-header.component.html',
  styleUrls: ['./publication-header.component.less']
})
export class PublicationHeaderComponent {
  @Input() avatar: string;
  @Input() title: string;
}
