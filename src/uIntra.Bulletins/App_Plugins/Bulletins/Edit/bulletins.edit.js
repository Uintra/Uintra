import helpers from "./../../Core/Content/scripts/Helpers";
import fileUploadController from "./../../Core/Controls/FileUpload/file-upload";
import confirm from "./../../Core/Controls/Confirm/Confirm";
import alertify from 'alertifyjs/build/alertify.min';

let holder;

function initEditor() {
    let description = holder.querySelector(".js-edit-bulletin__description");
    let dataStorage = holder.querySelector(".js-edit-bulletin__description-hidden");

    let editor = helpers.initQuill(description, dataStorage,{
        theme: 'snow',
        modules: {
            toolbar: [
                ['bold', 'italic', 'link']
            ]
        }
    });

    editor.on('text-change', function () {
        if (editor.getLength() > 1 && description.classList.contains('input-validation-error')) {
            description.classList.remove('input-validation-error');
        }
    });
}

function initFileUploader() {
    fileUploadController.init(holder);
}

function initEventListeners() {
    let deleteButton = holder.querySelector(".js-edit-bulletin__delete");
    deleteButton.addEventListener("click", deleteClickHandler);
}

function deleteClickHandler(event) {
    let button = event.target;
    let id = button.dataset["id"];
    let text = button.dataset["text"];
    var returnUrl = button.dataset["return-url"];

    alertify.defaults.glossary.cancel = button.dataset["cancel"];
    alertify.defaults.glossary.ok = button.dataset["ok"];

    confirm.showConfirm(text, function () {
        $.post('/umbraco/surface/Bulletins/Delete?id=' + id,function (data) {
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

let controller = {
    init: function () {
        holder = getBulletinHolder();
        if (!holder) {
            return;
        }

        initEditor();
        initFileUploader();
        initEventListeners();
    }
}

export default controller;