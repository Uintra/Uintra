import { IULink, ICreator, IUlinkWithTitle } from 'src/app/feature/shared/interfaces/general.interface';

export interface IGroupsListItem {
  id: string;
  hasImage: boolean;
  groupUrl: IULink;
  groupImageUrl: string;
  title: string;
  description: string;
  creator: ICreator;
  isMember: boolean;
  membersCount: number;
}
export interface IGroupsData {
  groupPageItem: IUlinkWithTitle;
  items: IUlinkWithTitle[];
}
export interface IGroupModel {
  title: string;
  description: string;
  newMedia: string;
  media: string[] | null;
  id?: string;
}
export interface IGroupDetailsHeaderData {
  title: string;
  groupLinks: {
    groupRoomPage: IULink;
    groupDocumentsPage: IULink;
    groupMembersPage: IULink;
    groupEditPage?: IULink;
  }
}
