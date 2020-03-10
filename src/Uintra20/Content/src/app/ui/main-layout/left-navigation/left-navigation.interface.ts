import { IULink } from 'src/app/shared/interfaces/general.interface';
import { IOwner } from 'src/app/feature/specific/activity/activity.interfaces';
import { IGroupsData } from 'src/app/feature/specific/groups/groups.interface';
import { ISharedNavData } from './components/shared-links/shared-links.service';
import { IMyLink } from './components/my-links/my-links.service';

export interface INavigationData {
  menuItems: INavigationItem[];
  groupItems: IGroupsData;
  sharedLinks: ISharedNavData;
  myLinks: Array<IMyLink>;
}
export interface INavigationItem {
  id: number;
  name: string;
  url: string;
  isActive: boolean;
  isHomePage: boolean;
  isClickable: boolean;
  isHeading: boolean;
  children: INavigationItem[];
  level: number;
  isSelected: boolean;
}
export interface IMobileUserNavigation {
  currentMember: IOwner,
  items: ITopNavigationLink[]
}
export interface ITopNavigationLink {
  name: string;
  url: IULink;
  type: number;
}
