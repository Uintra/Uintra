import { DropzoneConfigInterface } from 'ngx-dropzone-wrapper';

export const DEFAULT_DROPZONE_CONFIG: DropzoneConfigInterface = {
    // TODO: Change this to your upload POST address:
    url: '/umbraco/api/file/UploadSingle',
    maxFiles: 10,
    maxFilesize: 50,
    addRemoveLinks: true,
    createImageThumbnails: true
};
export const MAX_FILES_FOR_SINGLE = 1;
