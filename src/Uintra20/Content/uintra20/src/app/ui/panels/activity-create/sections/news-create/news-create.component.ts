/// <reference types="@types/googlemaps" />
import { Component, ViewChild, OnInit, ElementRef, NgZone, Input } from '@angular/core';
import { MapsAPILoader, MouseEvent } from '@agm/core';
import { IActivityCreatePanel } from '../../activity-create-panel.interface';
import ParseHelper from 'src/app/feature/shared/helpers/parse.helper';
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
  private geoCoder;
  

  @ViewChild('search', {static: false})
  public searchElementRef: ElementRef;

  public googleMapsModel: IGoogleMapsModel;
  public defaultCoordinates: ICoordinates = GOOGLE_MAPS_CONFIG.DEFAULT_COORDINATES;

  constructor(
    private mapsAPILoader: MapsAPILoader,
    private ngZone: NgZone
  ) {
    this.googleMapsModel = {
      coordinates: GOOGLE_MAPS_CONFIG.DEFAULT_COORDINATES,
      address: '',
      zoom: GOOGLE_MAPS_CONFIG.ZOOM,
      disableDefaultUI: GOOGLE_MAPS_CONFIG.DISABLE_DEFAULT_UI,
      zoomControl: GOOGLE_MAPS_CONFIG.ZOOM_CONTROL
    };
  }

  ngOnInit() {
    this.panelData = ParseHelper.parseUbaselineData(this.data);
    this.tags = Object.values(this.panelData.tags.userTagCollection);

    this.mapsAPILoader.load().then(() => {

      this.geoCoder = new google.maps.Geocoder;

      let autocomplete = new google.maps.places.Autocomplete(this.searchElementRef.nativeElement, {
        types: ["address"]
      });
      autocomplete.addListener("place_changed", () => {
        this.ngZone.run(() => {
          //get the place result
          let place: google.maps.places.PlaceResult = autocomplete.getPlace();

          //verify result
          if (place.geometry === undefined || place.geometry === null) {
            return;
          }

          this.googleMapsModel.coordinates.latitude = place.geometry.location.lat();
          this.googleMapsModel.coordinates.longitude = place.geometry.location.lng();
          this.googleMapsModel.zoom = 12;
        });
      });
    });
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

    this.googleMapsModel.coordinates = {
      latitude: $event.coords.lat,
      longitude: $event.coords.lng,
    };
    this.getAddress($event.coords.lat, $event.coords.lng);

  }

  getAddress(latitude, longitude) {
    this.geoCoder.geocode({ 'location': { lat: latitude, lng: longitude } }, (results, status) => {
      console.log(results);
      console.log(status);
      if (status === 'OK') {
        if (results[0]) {
          this.googleMapsModel.zoom = 12;
          this.googleMapsModel.address = results[0].formatted_address;
        } else {
          window.alert('No results found');
        }
      } else {
        window.alert('Geocoder failed due to: ' + status);
      }

    });
  }
}

export interface IGoogleMapsModel {
  coordinates: ICoordinates;
  address: string;
  zoom: number;
  disableDefaultUI: boolean;
  zoomControl: boolean;
}

export interface ICoordinates {
  latitude: number;
  longitude: number;
}
