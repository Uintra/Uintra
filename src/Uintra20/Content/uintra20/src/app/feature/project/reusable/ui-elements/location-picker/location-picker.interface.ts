export interface IGoogleMapsModel {
    coordinates: ICoordinates;
    zoom: number;
    disableDefaultUI: boolean;
    zoomControl: boolean;
}

export interface ICoordinates {
    latitude: number;
    longitude: number;
}
