import ajax from './../../Core/Content/scripts/Ajax';
import helpers from "./../../Core/Content/scripts/Helpers";
import fileUploadController from "./../../Core/Controls/FileUpload/file-upload";
import confirm from "./../../Core/Controls/Confirm/Confirm";
import alertify from 'alertifyjs/build/alertify.min';

let holder;
let descriptionElem;
let editor;
let editForm;
let isOneLinkDetected = false;
let linkPreviewId;

function initEditor() {    
    descriptionElem = holder.querySelector(".js-edit-bulletin__description");
    let dataStorage = holder.querySelector(".js-edit-bulletin__description-hidden");

    editor = helpers.initQuill(descriptionElem, dataStorage);

    editor.on('text-change', function () {
        if (editor.getLength() > 1 && descriptionElem.classList.contains('input-validation-error')) {
            descriptionElem.classList.remove('input-validation-error');
        }
    });

    editor.onLinkDetected(function (link) {        
        if (!isOneLinkDetected) {
            showLinkPreview(link);            
        }
    });

    linkPreviewId = holder.querySelector("[name='linkPreviewId']");
    if (linkPreviewId) {
        isOneLinkDetected = true;        
    }

    editForm = holder.querySelector("form");

    function showLinkPreview(link) {
        ajax.get('/umbraco/api/LinkPreview/Preview?url=' + link)
            .then(function (response) {
                var data = response.data;
                var imageElem = getImageElem(data);
                var hiddenSaveElem = getHiddenSaveElem(data);
                $(descriptionElem).after(imageElem);
                $(descriptionElem).after(hiddenSaveElem);
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

function initFileUploader() {
    fileUploadController.init(holder);
}

function initEventListeners() {
    let deleteButton = holder.querySelector(".js-edit-bulletin__delete");
    deleteButton.addEventListener("click", deleteClickHandler);

    let submitButton = holder.querySelector(".js-disable-submit");
    submitButton.addEventListener("click", submitClickHandler);
}

function submitClickHandler(event) {
    $(descriptionElem).toggleClass("input-validation-error", editor.getLength() <= 1);

    if (!isModelValid()) {
        holder.querySelector('.form__required').style.display = 'block';
        event.preventDefault();
        event.stopPropagation();
    }
}

function isModelValid() {
    let decription = holder.querySelector('input[name="description"]').value;
    let media = holder.querySelector('input[name="newMedia"]').value;
    let newMedia = holder.querySelector('input[name="media"]').value;
    return decription || media || newMedia ? true : false;
}

function deleteClickHandler(event) {
    let button = event.target;
    let id = button.dataset["id"];
    let text = button.dataset["text"];
    let returnUrl = button.dataset["returnUrl"];
    let deleteUrl = button.dataset["deleteUrl"];

    alertify.defaults.glossary.cancel = button.dataset["cancel"];
    alertify.defaults.glossary.ok = button.dataset["ok"];

    confirm.showConfirm('', text, function () {
        $.post(deleteUrl + '?id=' + id, function (data) {
            if (data.IsSuccess) {
                window.location.href = returnUrl;
            }
        });
    }, function () { }, confirm.defaultSettings);

    return false;
}

function getBulletinHolder() {
    return document.querySelector(".js-edit-bulletin");
}

var initEditLinkPreview = function (holder) {

    var removeLinkPreviewButton = findControl(holder, '.js-link-preview-remove-preview');
    var linkPreviewIdContainer = findControl(holder, 'input[name="linkPreviewId"]')[0];
    var linkPreviewEditContainer = findControl(holder, '.js-link-preview-edit-preview-container');
    
    removeLinkPreviewButton.on('click', function () {
        linkPreviewIdContainer.value = null;
        linkPreviewEditContainer.hide();
        isOneLinkDetected = false;
    });
};

function findControl(holder, selector) {
    return holder.find(selector).filter(function () {
        var $this = $(this);
        var parent = $this.closest('.js-edit-bulletin');
        return parent.data('id') === holder.data('id');
    });
}

let controller = {
    init: function () {
        holder = getBulletinHolder();
        if (!holder) {
            return;
        }

        initEditor();
        initFileUploader();
        initEventListeners();
        initEditLinkPreview($(holder));
    }
}

export default controller;