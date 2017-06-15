import appInitializer from "./../../Core/Content/scripts/AppInitializer";
import helpers from "./../../Core/Content/scripts/Helpers";
import fileUploadController from "./../../Core/Controls/FileUpload/file-upload";
import ajax from "./../../Core/Content/scripts/Ajax";

const toolbarSelector = ".js-create-bulletin__toolbar";
const bulletinsTabNumber = 4;

var cfReloadTabEvent;
var cfShowBulletinsEvent;
let holder;
let dropzone;
let dataStorage;
let description;
let toolbar;
let sentButton;
let closeButton;
let header;
let editor; 

function initElements() {
    dataStorage = holder.querySelector(".js-create-bulletin__description-hidden");
    description = holder.querySelector(".js-create-bulletin__description");
    toolbar = holder.querySelector(toolbarSelector);
    sentButton = document.querySelector(".js-toolbar__send-button");
    closeButton = holder.querySelector(".js-create-bulletin__description-close");
    header = holder.querySelector(".js-create-bulletin__user");
}

function initEditor() {
    editor = helpers.initQuill(description, dataStorage,{
        theme: 'snow',
        placeholder: description.dataset["placeholder"],
        modules: {
            toolbar: {
                container: toolbarSelector
            }
        }
    });

    editor.on('text-change', function () {
        sentButton.disabled = !isEdited();
    });
}

function initEventListeners() {    
    description.addEventListener("click", descriptionClickHandler);
    sentButton.addEventListener("click", sentButtonClickHandler);
    closeButton.addEventListener("click", closeBtnClickHandler);
    window.addEventListener("beforeunload", beforeUnloadHander);

    cfReloadTabEvent = new CustomEvent("cfReloadTab", {
        detail: {
            isReinit: true
        }
    });

    cfShowBulletinsEvent = new CustomEvent("cfShowBulletins",
    {
        detail: {
            isReinit: true
        }
    });
}

function initFileUploader() {
    let previewContainer = document.querySelector(".js-dropzone-previews");
    let options = {
        previewsContainer: previewContainer
    };

    if ("undefined" === typeof dropzone) {
        dropzone = fileUploadController.init(holder, options);

        dropzone.on('success', function (file, fileId) {
            previewContainer.classList.remove("hidden");

            sentButton.disabled = !isEdited();
        });

        dropzone.on('removedfile', function (file) {
            if (this.files.length === 0) {
                previewContainer.classList.add("hidden");
            }

            sentButton.disabled = !isEdited();
        });
    }
}

function descriptionClickHandler(event) {
    show();
}

function isBulletinsTab() {
    var currentTabNumber=document.querySelector('.js-feed-links .js-feed-type._active').dataset['type'];
    return currentTabNumber == bulletinsTabNumber;
}

function sentButtonClickHandler(event) {
    let newMedia = holder.querySelector(".js-new-media");
    let data = {
        description: dataStorage.value,
        newMedia: newMedia.value
    };

    ajax.PostJson("/umbraco/api/BulletinsApi/Create", data).then(function(response) {
        if (response.isSuccess) {
            isBulletinsTab()? cfReloadTab():cfShowBulletins();            
            hide();
            dropzone.removeAllFiles(true);

        }
    });
}

function closeBtnClickHandler(event) {
    close(event);
}

function beforeUnloadHander(event) {
    if (isEdited()) {
        let confirmationMessage = "\o/";
        event.returnValue = confirmationMessage;
        return confirmationMessage;
    }
}

// editor helpers

function close(event) {
    if (isEdited()) {
        if (showConfirmMessage()) {
            hide();
        } else {
            event.preventDefault();
        }
    } else {
        hide();
    }
}

function show() {
    toolbar.classList.remove("hidden");
    header.classList.remove("hidden");
    closeButton.classList.remove("hidden");
}

function hide() {
    toolbar.classList.add("hidden");
    header.classList.add("hidden");
    closeButton.classList.add("hidden");

    clear();
}

function clear() {
    editor.setText("");
    dropzone.removeAllFiles(true);
}

function isEdited() {
    let isDescriptionEdited = editor.getLength() > 1;
    let isFilesUploaded = dropzone.files.length;
    return isDescriptionEdited || isFilesUploaded;
}

function showConfirmMessage() {
    return window.confirm("TODO: are you sure ?");
}

let controller = {
    init: function () {
        holder = getBulletinHolder();
        if (!holder) {
            return;
        }

        initElements();
        initEditor();
        initEventListeners();
        initFileUploader();
    }
}

function getBulletinHolder() {
    return document.querySelector(".js-create-bulletin");
}

function cfReloadTab() {
    document.body.dispatchEvent(cfReloadTabEvent);
}

function cfShowBulletins() {    
    document.body.dispatchEvent(cfShowBulletinsEvent);
}

function cfTabReloadedEventHandler(e) {
    let bulletinHolder = getBulletinHolder();
    if (!bulletinHolder || !e.detail.isReinit) {
        return;
    }

    controller.init();
}

appInitializer.add(() => {
    controller.init();

    document.body.addEventListener('cfTabReloaded', cfTabReloadedEventHandler);
});