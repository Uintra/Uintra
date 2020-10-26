import {
  Component,
  ViewChild,
  OnInit,
  ElementRef,
  NgZone,
  Output,
  EventEmitter,
  Input
} from "@angular/core";
import { MapsAPILoader, MouseEvent } from "@agm/core";
import { IGoogleMapsModel, ICoordinates } from "./location-picker.interface";
import { GoogleGeolocationService } from "./services/google-geolocation.service";
import { GOOGLE_MAPS_CONFIG } from 'src/app/shared/constants/maps/google-maps.const';
import { LOCATION_TITLE_MAX_LENGTH } from 'src/app/shared/constants/activity/activity-create.const';
import {ILocation} from "../../../specific/activity/activity.interfaces";

@Component({
  selector: "app-location-picker",
  templateUrl: "./location-picker.component.html",
  styleUrls: ["./location-picker.component.less"]
})
export class LocationPickerComponent implements OnInit {
  @Input() initialValues: ILocation;

  address: ILocation;
  @Output() handleChange = new EventEmitter<ILocation>();

  @ViewChild("search", { static: false })
  public searchElementRef: ElementRef;
  public googleMapsModel: IGoogleMapsModel;
  public defaultCoordinates: ICoordinates =
    GOOGLE_MAPS_CONFIG.DEFAULT_COORDINATES;
  public locationTitleMaxLength: number = LOCATION_TITLE_MAX_LENGTH;

  constructor(
    private mapsAPILoader: MapsAPILoader,
    private ngZone: NgZone,
    private googleGeolocationService: GoogleGeolocationService
  ) {}

  public ngOnInit(): void {
    this.address={ address:"",shortAddress:""};
    this.onInit();
    this.setupInputListener();
  }

  mapReady() {
    if (this.initialValues){
      this.address = this.initialValues;
    }

    this.googleGeolocationService.getLatLng(this.address.address, (results) => {
      if (results.length && results[0].geometry) {
        const lat = results[0].geometry.location.lat();
        const lng = results[0].geometry.location.lng();

        this.updateDefaultCoordinates(lat, lng);
      }
    });
  }

  public handleMapClicked($event: MouseEvent): void {
    const latitude = $event.coords.lat;
    const longitude = $event.coords.lng;

    this.googleMapsModel.coordinates = {
      latitude,
      longitude
    };
    this.updateDefaultCoordinates(latitude, longitude);
    this.googleGeolocationService.getAddress(latitude, longitude, result => {
      this.address = result;
      this.handleChange.emit(result);
    });
  }

  private onInit(): void {

    this.googleMapsModel = {
      coordinates: GOOGLE_MAPS_CONFIG.DEFAULT_COORDINATES,
      zoom: GOOGLE_MAPS_CONFIG.ZOOM,
      disableDefaultUI: GOOGLE_MAPS_CONFIG.DISABLE_DEFAULT_UI,
      zoomControl: GOOGLE_MAPS_CONFIG.ZOOM_CONTROL
    };
  }

  private setupInputListener(): void {
    this.mapsAPILoader.load().then(() => {
      const autocomplete = new google.maps.places.Autocomplete(
        this.searchElementRef.nativeElement,
        {
          types: ["address"]
        }
      );
      autocomplete.addListener("place_changed", () => {
        this.ngZone.run(() => {
          const place: google.maps.places.PlaceResult = autocomplete.getPlace();

          if (place.geometry === undefined || place.geometry === null) {
            return;
          }

          const lat = place.geometry.location.lat();
          const lng = place.geometry.location.lng();

          this.googleMapsModel.coordinates.latitude = lat;
          this.googleMapsModel.coordinates.longitude = lng;
          this.updateDefaultCoordinates(lat, lng);

          this.googleGeolocationService.getAddress(lat, lng, result => {
            this.address = result;
            this.handleChange.emit(this.address);
          });
        });
      });
    });
  }

  private updateDefaultCoordinates(lat, long): void {
    this.defaultCoordinates.latitude = lat;
    this.defaultCoordinates.longitude = long;
  }
}
