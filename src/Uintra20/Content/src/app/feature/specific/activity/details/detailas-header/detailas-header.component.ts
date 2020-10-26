import { Component, OnInit, Input } from '@angular/core';
import { ISocialDetails, ILocation } from '../../activity.interfaces';

@Component({
  selector: 'app-detailas-header',
  templateUrl: './detailas-header.component.html',
  styleUrls: ['./detailas-header.component.less']
})
export class DetailasHeaderComponent implements OnInit {
  @Input() details: ISocialDetails;
  @Input() activityName: string;
  @Input() location?: ILocation;

  googleMapUrl: string;

  constructor() { }

  public ngOnInit(): void {
    if (this.location && this.location.address) {
      this.googleMapUrl = `http://maps.google.co.uk/maps?q=${this.location.address}`;
    }
  }

  public getDates() {
    if (!this.details.headerInfo.dates) {

      return '';
    }

    return this.details.headerInfo.dates.join(' - ');
  }
}
