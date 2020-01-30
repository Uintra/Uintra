export interface ISocialCreateModel {
  description: string;
  ownerId: string;
  newMedia: string;
  tagIdsData: string[];
}

export interface INewsCreateModel {
  ownerId: string;
  title: string;
  description: string;
  publishDate: string;

  unpublishDate?: string;
  media?: Array<any>;
  mediaRootId?: number;
  endPinDate?: string;
  tagIdsData?: string[];

  isPinned?: boolean;
  activityLocationEditModel?: {
    address: string;
    shortAddress: string;
  };
  newMedia?: string;
}
