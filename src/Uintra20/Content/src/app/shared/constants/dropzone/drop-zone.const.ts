import { DropzoneConfigInterface } from 'ngx-dropzone-wrapper';
import { TranslateService } from '@ngx-translate/core';

export const MAX_FILES_FOR_SINGLE = 1;
export const DEFAULT_DROPZONE_CONFIG: DropzoneConfigInterface = {
    url: '/umbraco/api/file/UploadSingle',
    maxFiles: 10,
    maxFilesize: 50,
    addRemoveLinks: true,
    createImageThumbnails: true,
    dictRemoveFile: 'Remove file',
    dictMaxFilesExceeded: 'You can not upload any more files',
};
