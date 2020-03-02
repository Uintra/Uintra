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
export interface ILocationResult {
    address: string;
    shortAddress: string;
}

