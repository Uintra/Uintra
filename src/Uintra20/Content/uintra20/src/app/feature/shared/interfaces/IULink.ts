export interface IUlinkWithTitle {
  title: string;
  link: IULink;
}
export interface IULink {
  originalUrl: string;
  baseUrl: string;
  params: Array<object>;
}
