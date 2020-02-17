import { IOwner } from 'src/app/feature/project/specific/activity/activity.interfaces';
import { IULink } from 'src/app/feature/shared/interfaces/IULink';

export interface INavigationData {
  menuItems: INavigationItem[];
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
