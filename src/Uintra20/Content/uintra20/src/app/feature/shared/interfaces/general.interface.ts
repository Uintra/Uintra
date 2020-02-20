export interface IUlinkWithTitle {
  title: string;
  link: IULink;
}
export interface IULink {
  originalUrl: string;
  baseUrl: string;
  params: Array<{
    name: string;
    value: string;
  }>;
}
export interface ICreator {
  id: string;
  displayedName: string;
  photo: string;
  photoId: number;
  email: string;
  loginName: string;
  inactive: boolean;
}
