import helpers from "./../../Core/Content/scripts/Helpers";
import fileUploadController from "./../../Core/Controls/FileUpload/file-upload";
import umbracoAjaxForm from "./../../Core/Content/scripts/UmbracoAjaxForm";

const toolbarSelector = ".js-create-bulletin__toolbar";

let mobileMediaQuery = window.matchMedia("(max-width: 899px)");

let holder;
let dropzone;
let dataStorage;
let description;
let mobileBtn;
let toolbar;
let sentButton;
let header;
let editor; 
let body;
let bulletin;
let confirmMessage;

function initElements() {
    dataStorage = holder.querySelector(".js-create-bulletin__description-hidden");
    description = holder.querySelector(".js-create-bulletin__description");
    toolbar = holder.querySelector(toolbarSelector);
    sentButton = document.querySelector(".js-toolbar__send-button");
    header = holder.querySelector(".js-create-bulletin__user");
    mobileBtn = document.querySelector(".js-expand-bulletin");
    body = document.querySelector("body");
    bulletin = document.querySelector(".js-create-bulletin");
    confirmMessage = bulletin.dataset.message;
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
    mobileMediaQuery.matches ? 
        mobileBtn.addEventListener("click", descriptionClickHandler) : 
        description.addEventListener("click", descriptionClickHandler);

    sentButton.addEventListener("click", sentButtonClickHandler);
    window.addEventListener("beforeunload", beforeUnloadHander);
    body.addEventListener("click", function(ev) {
        isOutsideClick(bulletin, ev.target, function() {
            closeBulletin();
        });
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

function sentButtonClickHandler(event) {
    event.preventDefault();
    let form = umbracoAjaxForm(holder.querySelector('form'));
  
    let promise = form.submit();
    promise.then(function(data) {
        if (data.IsSuccess) {
            window.location.hash = data.Id;

            cfReloadTab();
            hide(); 
        }
    });
}

function closeBulletin() {
    if (isEdited()) {
        if (showConfirmMessage(confirmMessage)) {
            hide();
        } 
        return;
    } 
    hide();
}

function beforeUnloadHander(event) {
    if (isEdited()) {
        let confirmationMessage = "\o/";
        event.returnValue = confirmationMessage;
        return confirmationMessage;
    }
}

function initMobile(){
    if(mobileMediaQuery.matches){
        holder = getBulletinHolder();
        holder.classList.add("hidden");
    }
}

// editor helpers

function show() {
    toolbar.classList.remove("hidden");
    header.classList.remove("hidden");

    if(mobileMediaQuery.matches){
        let bulletinHolder = getBulletinHolder();
        bulletinHolder.classList.remove("hidden");
        mobileBtn.classList.add("hide");
    }
}

function hide() {
    toolbar.classList.add("hidden");
    header.classList.add("hidden");

    if(mobileMediaQuery.matches){
        let bulletinHolder = getBulletinHolder();
        bulletinHolder.classList.add("hidden");
        mobileBtn.classList.remove("hide");
    }

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

function showConfirmMessage(message) {
    return window.confirm(message);
}

function getBulletinHolder() {
    return document.querySelector(".js-create-bulletin");
}

function cfReloadTab() {
    uIntra.events.cfReloadTab.dispatch();
}

function isOutsideClick (el, target, callback) {
    let hiddenInput = document.querySelector(".dz-hidden-input");
    if (el && !el.contains(target) && target != hiddenInput && target != mobileBtn) {
        if (typeof callback === "function") {
            callback();
        }
    }
};

let controller = {
    init: function () {
        holder = getBulletinHolder();
        if (!holder) {
            return;
        }

        initMobile();
        initElements();
        initEditor();
        initEventListeners();
        initFileUploader();
    }
}

export default controller;