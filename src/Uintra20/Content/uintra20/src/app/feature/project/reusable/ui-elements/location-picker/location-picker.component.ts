/// <reference types="@types/googlemaps" />
import { Component, ViewChild, OnInit, ElementRef, NgZone, Input } from '@angular/core';
import { MapsAPILoader, MouseEvent } from '@agm/core';
import { GOOGLE_MAPS_CONFIG } from 'src/app/constants/maps/google-maps.const';
import { IGoogleMapsModel, ICoordinates } from './location-picker.interface';

@Component({
  selector: 'app-location-picker',
  templateUrl: './location-picker.component.html',
  styleUrls: ['./location-picker.component.less']
})
export class LocationPickerComponent implements OnInit {

  private geoCoder;

  @Input() address: string;
  @ViewChild('search', { static: false })
  public searchElementRef: ElementRef;
  public googleMapsModel: IGoogleMapsModel;
  public defaultCoordinates: ICoordinates = GOOGLE_MAPS_CONFIG.DEFAULT_COORDINATES;

  constructor(
    private mapsAPILoader: MapsAPILoader,
    private ngZone: NgZone
  ) { }

  public ngOnInit(): void {
    this.address = '';
    this.googleMapsModel = {
      coordinates: GOOGLE_MAPS_CONFIG.DEFAULT_COORDINATES,
      zoom: GOOGLE_MAPS_CONFIG.ZOOM,
      disableDefaultUI: GOOGLE_MAPS_CONFIG.DISABLE_DEFAULT_UI,
      zoomControl: GOOGLE_MAPS_CONFIG.ZOOM_CONTROL
    };

    this.mapsAPILoader.load().then(() => {

      this.geoCoder = new google.maps.Geocoder;

      let autocomplete = new google.maps.places.Autocomplete(this.searchElementRef.nativeElement, {
        types: ["address"]
      });
      autocomplete.addListener("place_changed", () => {
        this.ngZone.run(() => {
          const place: google.maps.places.PlaceResult = autocomplete.getPlace();

          if (place.geometry === undefined || place.geometry === null) {
            return;
          }

          this.googleMapsModel.coordinates.latitude = place.geometry.location.lat();
          this.googleMapsModel.coordinates.longitude = place.geometry.location.lng();
          this.defaultCoordinates.latitude = place.geometry.location.lat();
          this.defaultCoordinates.longitude = place.geometry.location.lng();
        });
      });
    });
  }

  public handleMapClicked($event: MouseEvent): void {
    this.googleMapsModel.coordinates = {
      latitude: $event.coords.lat,
      longitude: $event.coords.lng,
    };
    this.getAddress($event.coords.lat, $event.coords.lng);
  }

  getAddress(lat, lng) {
    this.geoCoder.geocode(
      {
        location: {
          lat,
          lng
        }
      }, (results = [], status) => {
        if (status === 'OK' && results.length) {
          this.address = results[0].formatted_address;
          this.defaultCoordinates.latitude = lat;
          this.defaultCoordinates.longitude = lng;
        }
      });
  }
}
