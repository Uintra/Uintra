var Dropzone = require("dropzone");
Dropzone.autoDiscover = false;

require("./file-upload.css");

var fileUploader = (function () {
    var separator = ';';

    var getUploadedFiles = function (container) {
        var val = container.val();
        if (!val) {
            return [];
        }
        return val.split(separator);
    }

    var addFile = function (container, fileId) {
        var files = getUploadedFiles(container);
        files.push(fileId);
        container.val(files.join(separator));
    }

    var removeFile = function (container, fileId) {
        container.val(container.val().replace(fileId + separator));
    }

    return {
        init: function (holder) {
            holder = $(holder);
            var dropzoneElem = holder.find('.js-dropzone');
            var hiddenInput = holder.find('input[type=hidden].js-new-media');

            if (!holder || !hiddenInput || !dropzoneElem) {
                throw new Error("FileUpload: Can't find elements to work with");
            }

            var allowedExtentions = dropzoneElem.data('allowed').replace(/\s/g, '') || "";
            var maxCount = dropzoneElem.data('maxCount');

            var dropzone = new Dropzone(dropzoneElem[0], {
                url: "/Umbraco/Api/File/UploadSingle",
                maxFiles:maxCount,
                addRemoveLinks: true,
                maxFilesize: 50,
                acceptedFiles: allowedExtentions,
                dictDefaultMessage: dropzoneElem.data('defaultText'),
                dictRemoveFile: dropzoneElem.data('removeText')
            });

            //disable upload file
            var editHolder = holder.find('.js-file-edit');
            var filesElem = editHolder.find('input[type="hidden"]');
            if (filesElem.length) {
                var existedFiles=filesElem.val().replace(/\s/g, '').split(',').filter(function (s){ return s != null && s != ''});   
                if (dropzone.options.maxFiles <= existedFiles.length) {
                    dropzone.removeEventListeners();
                }    
            }            

            dropzone.on('success', function (file, fileId) {
                file.uuid = fileId;
                addFile(hiddenInput, fileId);
            });

            dropzone.on('maxfilesreached',
              function() {
                  dropzone.removeEventListeners();
              });
            
            dropzone.on('removedfile', function (file) {
                removeFile(hiddenInput, file.uuid);
                if (dropzone.options.maxFiles>this.files.length) {
                    dropzone.setupEventListeners();
                }
            });
        }
    }
})();

var fileEditor = (function () {
    var modelInput;
    var controlHolder;

    var removeFileView = function (targetId) {
        var fileHolder = controlHolder.find('li[data-id="' + targetId + '"]');
        if (!fileHolder) {
            throw new Error("FileEdit: Can't file holder with target id: " + targetId);
        }

        fileHolder.css('display', 'none');
        fileHolder.hidden = true;
    }

    var bindRemove = function () {
        var btn = $(this);
        btn.on("click", function () {
            var targetId = btn.data('targetId');
            if (!targetId) {
                throw new Error("FileEdit: Can't find target id for element");
            }

            var modelValue = modelInput.val().replace(/\s/g, '').split(',').filter(function (s){ return s != null && s != ''});
            var newModelValue = modelValue.filter(s => s != targetId);
            modelInput.val(newModelValue.join(',') || "");
            removeFileView(targetId);

            // allow upload 
            var dropzoneElem = controlHolder.siblings('div').find('.dropzone');            
            if (dropzoneElem[0].dropzone.options.maxFiles <= modelValue.length) {
                dropzoneElem[0].dropzone.setupEventListeners();
            }  
        });
    }

    return {
        init: function (holder) {
            holder = $(holder);
            controlHolder = holder.find('.js-file-edit');
            var removeBtns = controlHolder.find('.js-remove-file-btn');
            modelInput = controlHolder.find('input[type="hidden"]');

            if (!modelInput) {
                throw new Error("FileEdit: Can't find model input");
            }

            removeBtns.each(bindRemove);
        }
    }
})();

var FileUploadController = {
    init: function (holder) {
        fileUploader.init(holder);
        fileEditor.init(holder);
    }
}

export default FileUploadController;

/*window.App = window.App || {};
window.App.FileUploadController = FileUploadController;*/
