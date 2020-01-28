import { ICoordinates } from 'src/app/ui/panels/activity-create/sections/news-create/news-create.component';

export interface GoogleMapsConfig {
    API_KEY: string;
    DEFAULT_COORDINATES: ICoordinates;
    ZOOM: number;
    DISABLE_DEFAULT_UI: boolean;
    ZOOM_CONTROL: boolean;
}

