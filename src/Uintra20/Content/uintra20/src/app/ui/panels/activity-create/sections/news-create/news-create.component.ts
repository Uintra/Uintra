import { Component, OnInit, Input } from '@angular/core';
import { IActivityCreatePanel } from '../../activity-create-panel.interface';
import ParseHelper from 'src/app/feature/shared/helpers/parse.helper';
import { MouseEvent } from '@agm/core';
import { GOOGLE_MAPS_CONFIG } from 'src/app/constants/maps/google-maps.const';

@Component({
  selector: 'app-news-create',
  templateUrl: './news-create.component.html',
  styleUrls: ['./news-create.component.less']
})
export class NewsCreateComponent implements OnInit {
  @Input() data: IActivityCreatePanel;
  panelData: any; //TODO create interface
  files: Array<any> = [];
  isPinCheked: boolean;
  tags: any[];

  public googleMapsModel: IGoogleMapsModel;
  public defaultCoordinates: ICoordinates = GOOGLE_MAPS_CONFIG.DEFAULT_COORDINATES;

  constructor() {
    this.googleMapsModel = {
      coordinates: GOOGLE_MAPS_CONFIG.DEFAULT_COORDINATES,
      locationName: 'SELO ',
      zoom: GOOGLE_MAPS_CONFIG.ZOOM,
      disableDefaultUI: GOOGLE_MAPS_CONFIG.DISABLE_DEFAULT_UI,
      zoomControl: GOOGLE_MAPS_CONFIG.ZOOM_CONTROL
    };
  }

  ngOnInit() {
    this.panelData = ParseHelper.parseUbaselineData(this.data);
    this.tags = Object.values(this.panelData.tags.userTagCollection);
  }

  onUploadSuccess(fileArray: Array<any> = []): void {
    this.files.push(fileArray);
  }

  onFileRemoved(removedFile: object) {
    this.files = this.files.filter(file => {
      const fileElement = file[0];
      return fileElement !== removedFile;
    });
  }

  setValue(value) {
    // debugger;
  }

  onSubmit() {
    // debugger;
  }

  public handleMapClicked($event: MouseEvent): void {
    console.log($event);
    this.googleMapsModel.locationName = 'SELO ' + Math.random();

    this.googleMapsModel.coordinates = {
      latitude: $event.coords.lat,
        longitude: $event.coords.lng,
    };

    console.log(this.googleMapsModel);
  }
}

export interface IGoogleMapsModel {
  coordinates: ICoordinates;
  locationName: string;
  zoom: number;
  disableDefaultUI: boolean;
  zoomControl: boolean;
}

export interface ICoordinates {
  latitude: number;
  longitude: number;
}
