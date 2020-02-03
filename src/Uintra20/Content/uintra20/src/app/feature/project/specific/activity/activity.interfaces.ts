import { ITagData } from '../../reusable/inputs/tag-multiselect/tag-multiselect.interface';
import { IMedia, IDocument } from 'src/app/ui/pages/social/details/social-details.interface';

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
    address?: string;
    shortAddress?: string;
  };
  newMedia?: string;
}

export interface ISocialEdit {
  ownerId: string;
  description: string ;
  tags: Array<ITagData>;
  availableTags: Array<ITagData>;
  lightboxPreviewModel: ILightBoxPreviewModel;
  id: string;
  name: string;
  tagIdsData: Array<string>;
  newMedia: string;
}

export interface ILightBoxPreviewModel {
  medias: Array<IMedia>;
  otherFiles: Array<IDocument>;
  hiddenImagesCount: number;
  additionalImages: number;
  filesToDisplay: number;
}
