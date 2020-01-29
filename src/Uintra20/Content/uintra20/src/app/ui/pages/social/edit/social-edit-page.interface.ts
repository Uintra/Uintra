import { ITagData } from 'src/app/feature/project/reusable/inputs/tag-multiselect/tag-multiselect.interface';
import { IMedia, IDocument } from '../details/social-details.interface';

export interface ISocialEdit {
    description: string ;
    tags: Array<ITagData>;
    availableTags: Array<ITagData>;
    lightboxPreviewModel: ILightBoxPreviewModel;
    id: string;
    name: string;
    tagIdsData: Array<string>;
}

export interface ILightBoxPreviewModel {
    medias: Array<IMedia>;
    otherFiles: Array<IDocument>;
    hiddenImagesCount: number;
    additionalImages: number;
    filesToDisplay: number;
}
