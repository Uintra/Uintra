import ajax from './../../Core/Content/scripts/Ajax';
import helpers from "./../../Core/Content/scripts/Helpers";
import fileUploadController from "./../../Core/Controls/FileUpload/file-upload";
import umbracoAjaxForm from "./../../Core/Content/scripts/UmbracoAjaxForm";

const toolbarSelector = ".js-create-bulletin__toolbar";

let holder;
let dropzone;
let dataStorage;
let description;
let toolbar;
let sentButton;
//let header;
let editor;
let html;
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
let tagSelector;

function initElements() {
    dataStorage = holder.querySelector(".js-create-bulletin__description-hidden");
    description = holder.querySelector(".js-create-bulletin__description");
    toolbar = holder.querySelector(toolbarSelector);
    sentButton = document.querySelector(".js-toolbar__send-button");
    //header = holder.querySelector(".js-create-bulletin__user");
    html = document.querySelector("html");
    body = document.querySelector("body");
    bulletin = document.querySelector(".js-create-bulletin");
    confirmMessage = bulletin.dataset.message;
    createForm = bulletin.querySelector(".js-create-bulletin-form");
    tagSelector = createForm.querySelector('.js-user-tags-picker');
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
            isOneLinkDetected = true;
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
                isOneLinkDetected = false;
            });
    }

    function getImageElem(data) {
        var divElem = document.createElement('div');
        divElem.className += "link-preview";

        divElem.innerHTML =
            `<div class="link-preview__block"><button type="button" class="link-preview__close js-link-preview-remove-preview">X</button>
                <div class="link-preview__image">` +
            (data.imageUri ? `<img src="${data.imageUri}" />` : '') +
            `</div>
                <div class="link-preview__text">
                    <h3 class="link-preview__title">
                        <a href="${data.uri}">${data.title}</a>
                    </h3>` +
            (data.description ? `<p>${data.description}</p>` : "") +
            "</div></div>";

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

    //window.addEventListener("scroll", function (ev) {
    //    closeBulletin(ev);
    //});
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

function descriptionClickHandler() {
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

    let form = holder.querySelector('form');
    let formObj = umbracoAjaxForm(form);

    form.classList.add("submitted");

    let promise = formObj.submit();
    promise.then(function (response) {
        let data = response.data;
        if (data.IsSuccess) {
            window.location.hash = data.Id;

            cfReloadTab();
            hide();
        }
        form.classList.remove("submitted");
    });

    clearTagSelector();
}

function closeBulletin(event) {
    if (isEdited()) {
        if (showConfirmMessage(confirmMessage)) {
            hide(event);
            window.scrollTo(0, 0);
        }
        return;
    }

    clearTagSelector();
    hide(event);
}

function clearTagSelector() {
    $(tagSelector).val(null).trigger('change');
}

function beforeUnloadHander(event) {
    if (isEdited()) {
        let confirmationMessage = "\o/";
        event.returnValue = confirmationMessage;
        return confirmationMessage;
    }
}
// editor helpers

function show() {
    const headerAndButtonHeight = 130;
    setGlobalEventShow();
    bulletin.classList.add("_expanded");
    html.style.overflow = 'hidden';
    body.style.overflow = 'hidden';
    body.classList.add("bulletin_expanded");
    //toolbar.classList.remove("hidden");
    //header.classList.remove("hidden");
    //closeBulletinBtn.classList.remove("hidden");
    //editor.setSelection(0, 0);
}

function hide(event) {
    if (event && event.target == closeBulletinBtn) { event.preventDefault(); }
    setGlobalEventHide();
    bulletin.classList.remove("_expanded");
    html.style.overflow = '';
    body.style.overflow = '';
    body.classList.remove("bulletin_expanded");
    //toolbar.classList.add("hidden");
    //header.classList.add("hidden");
    //closeBulletinBtn.classList.add("hidden");
    hideLinkPreview();

    clear();
}

function hideLinkPreview() {
    linkPreviewContainer = holder.querySelector(".link-preview");
    var linkPreviewId = holder.querySelector("[name='linkPreviewId']");

    if (linkPreviewContainer) {
        linkPreviewContainer.parentNode.removeChild(linkPreviewContainer);
        linkPreviewId.parentNode.removeChild(linkPreviewId);
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
    if (el && !el.contains(target) && target != hiddenInput && target != expandBulletinBtn && isDescendant(wrapper, target)) {
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

        //initMobile();
        initElements();
        initEditor();
        initEventListeners();
        initFileUploader();
    }
}

export default controller;