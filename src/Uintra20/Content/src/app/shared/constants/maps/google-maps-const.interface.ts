import { ICoordinates } from 'src/app/feature/reusable/ui-elements/location-picker/location-picker.interface';

export interface GoogleMapsConfig {
    API_KEY: string;
    DEFAULT_COORDINATES: ICoordinates;
    ZOOM: number;
    DISABLE_DEFAULT_UI: boolean;
    ZOOM_CONTROL: boolean;
}

