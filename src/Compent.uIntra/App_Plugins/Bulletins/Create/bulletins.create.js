import ajax from './../../Core/Content/scripts/Ajax';
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
let createForm;
let expandBulletinBtn;
let closeBulletinBtn;
let wrapper;
let dimmedBg;
let isOneLinkDetected = false;
let linkPreviewContainer;

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
    createForm = bulletin.querySelector(".js-create-bulletin-form");
    expandBulletinBtn = document.querySelector(".js-bulletin-open");
    closeBulletinBtn = holder.querySelector(".js-create-bulletin__close");    
    wrapper = document.getElementById("wrapper");
    uIntra.events.add("setBulletinCreateMode");
    uIntra.events.add("removeBulletinCreateMode");
    dimmedBg = holder.querySelector(".js-create-bulletin__dimmed-bg");
}

function initEditor() {
    editor = helpers.initQuill(description, dataStorage, {
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

    editor.onLinkDetected(function (link) {
        
        if (!isOneLinkDetected) {
            showLinkPreview(link);            
        }
    });

    function showLinkPreview(link) {
        ajax.get('/umbraco/api/LinkPreview/Preview?url=' + link)
            .then(function (response) {
                var data = response.data;
                var imageElem = getImageElem(data);
                var hiddenSaveElem = getHiddenSaveElem(data);
                $(createForm).append(imageElem);
                $(createForm).append(hiddenSaveElem);
                isOneLinkDetected = true;

                var removeLinkPreview = function (e) {
                    if (e.target.classList.contains('js-link-preview-remove-preview')) {
                        imageElem.parentNode.removeChild(imageElem);
                        imageElem.removeEventListener('click', removeLinkPreview);
                        imageElem = null;
                        hiddenSaveElem.parentNode.removeChild(hiddenSaveElem);
                        isOneLinkDetected = false;
                    }
                };

                imageElem.addEventListener('click', removeLinkPreview);

            })
            .catch(err => {
                // Ignore error and do not crash if server returns non-success code
            });
    }

    function getImageElem(data) {
        var divElem = document.createElement('div');
        divElem.className += "link-preview";

        divElem.innerHTML =
            `<button type="button" class="link-preview__close js-link-preview-remove-preview">X</button>
                <div class="link-preview__image">
                     <img src="${data.imageUri}" />
                 </div>
                <div class="link-preview__text">
                    <h3 class="link-preview__title">
                        <a href="${data.uri}">${data.title}</a>
                    </h3>
                    <p>${data.description}</p>
                </div>`;

        return divElem;
    }

    function getHiddenSaveElem(data) {
        return createHiddenInput('linkPreviewId', data.id);
    }

    function createHiddenInput(name, value) {
        var input = document.createElement('input');

        input.setAttribute('type', 'hidden');
        input.setAttribute('name', name);
        input.setAttribute('value', value);

        return input;
    }
}

function initEventListeners() {
    mobileMediaQuery.matches ?
        mobileBtn.addEventListener("click", descriptionClickHandler) :
        description.addEventListener("click", descriptionClickHandler);

    sentButton.addEventListener("click", sentButtonClickHandler);
    window.addEventListener("beforeunload", beforeUnloadHander);
    expandBulletinBtn.addEventListener("click", descriptionClickHandler);
    closeBulletinBtn.addEventListener("click", function (ev) {
        closeBulletin(ev);
    });
    dimmedBg.addEventListener("click", function (ev) {
        closeBulletin(ev);
    })
    body.addEventListener("click", function (ev) {
        if (bulletin.classList.contains("_expanded")) {
            isOutsideClick(bulletin, ev.target, function () {
                closeBulletin(ev);
            });
        }
    });

    if (!mobileMediaQuery.matches) {
        window.addEventListener("scroll", function (ev) {
            closeBulletin(ev);
        });
    }
}

function initFileUploader() {
    let previewContainer = document.querySelector(".js-dropzone-previews");
    let options = {
        previewsContainer: previewContainer
    };

    if ("undefined" === typeof dropzone) {
        dropzone = fileUploadController.init(holder, options);

        dropzone.on('success', function (file, fileId) {
            sentButton.disabled = !isEdited();
        });

        dropzone.on('sending', function (file) {
            previewContainer.classList.remove("hidden");
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

function setGlobalEventShow() {
    uIntra.events.setBulletinCreateMode.dispatch();
}

function setGlobalEventHide() {
    uIntra.events.removeBulletinCreateMode.dispatch();
}

function sentButtonClickHandler(event) {
    event.preventDefault();
    let form = umbracoAjaxForm(holder.querySelector('form'));

    let promise = form.submit();
    promise.then(function (response) {
        let data = response.data;
        if (data.IsSuccess) {
            window.location.hash = data.Id;

            cfReloadTab();
            hide();
        }
    });
}

function closeBulletin(event) {
    if (isEdited()) {
        if (showConfirmMessage(confirmMessage)) {
            hide(event);
        }
        return;
    }
    hide(event);
}

function beforeUnloadHander(event) {
    if (isEdited()) {
        let confirmationMessage = "\o/";
        event.returnValue = confirmationMessage;
        return confirmationMessage;
    }
}

function initMobile() {
    if (mobileMediaQuery.matches) {
        holder = getBulletinHolder();
        holder.classList.add("hidden");
    }
}

// editor helpers

function show() {
    const headerAndButtonHeight = 130;
    setGlobalEventShow();
    bulletin.classList.add("_expanded");
    body.style.overflow = 'hidden';
    toolbar.classList.remove("hidden");
    header.classList.remove("hidden");
    closeBulletinBtn.classList.remove("hidden");
    sentButton.classList.remove("hidden");

    if (mobileMediaQuery.matches) {
        let bulletinHolder = getBulletinHolder();
        bulletinHolder.classList.remove("hidden");
        mobileBtn.classList.add("hide");
        window.scrollTo(0, (window.pageYOffset + bulletin.getBoundingClientRect().top - headerAndButtonHeight));
    }
}

function hide(event) {    
    if (event && event.target == closeBulletinBtn) { event.preventDefault(); }
    setGlobalEventHide();
    bulletin.classList.remove("_expanded");
    body.style.overflow = '';
    toolbar.classList.add("hidden");
    header.classList.add("hidden");
    closeBulletinBtn.classList.add("hidden");
    sentButton.classList.add("hidden");
    hideLinkPreview();

    if (mobileMediaQuery.matches) {
        let bulletinHolder = getBulletinHolder();
        bulletinHolder.classList.add("hidden");
        mobileBtn.classList.remove("hide");
    }

    clear();
}

function hideLinkPreview() {
    linkPreviewContainer = holder.querySelector(".link-preview");
    if (linkPreviewContainer) {
        $(linkPreviewContainer).hide();
        isOneLinkDetected = false;
    }
}

function clear() {
    editor.setText("");
    editor.blur();
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

function isOutsideClick(el, target, callback) {
    let hiddenInput = document.querySelector(".dz-hidden-input");
    if (el && !el.contains(target) && target != hiddenInput && target != expandBulletinBtn && target != mobileBtn && isDescendant(wrapper, target)) {
        if (typeof callback === "function") {
            callback();
        }
    }
};

function isDescendant(parent, child) {
    var node = child.parentNode;
    while (node != null) {
        if (node == parent) {
            return true;
        }
        node = node.parentNode;
    }
    return false;
}

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