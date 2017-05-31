import appInitializer from "./../Core/Content/scripts/AppInitializer";
import helpers from "./../Core/Content/scripts/Helpers";
import fileUploadController from "./../Core/Controls/FileUpload/file-upload";
import ajax from "./../Core/Content/scripts/Ajax";

require('./style.css');

const toolbarSelector = ".js-create-bulletin__toolbar";

var cfReloadTabEvent;
let holder;
let dropzone; // TODO: ?
let description;
let toolbar;
let sentButton;
let closeButton;
let header;
let editor; 

function initElements() {
    dropzone = holder.querySelector(".js-create-bulletin__dropzone");
    description = holder.querySelector(".js-create-bulletin__description");
    toolbar = holder.querySelector(toolbarSelector);
    sentButton = document.querySelector(".js-toolbar__send-button");
    closeButton = holder.querySelector(".js-create-bulletin__description-close");
    header = holder.querySelector(".js-create-bulletin__user");
}

function initEditor() {
    let dataStorage = holder.querySelector(".js-create-bulletin__description-hidden");
    editor = helpers.initQuill(description, dataStorage,{
        theme: 'snow',
        placeholder: "TODO: Create Bulletin activity",
        modules: {
            toolbar: {
                container: toolbarSelector,
                handlers: {
                    //'custom-file': function() {
                    //    if (dropzone.classList.contains("_hide")) {
                    //        dropzone.classList.remove("_hide");
                    //    } else {
                    //        dropzone.classList.add("_hide");
                    //    }
                    //}
                }
            }
        }
    });
}

function initEditorEventListeners() {
    editor.on('text-change', function () {
        //if (isEdited()) {
        //    sentButton.disabled = false;
        //} else {
        //    sentButton.disabled = true;
        //}

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
}

function descriptionClickHandler(event) {
    show();
}

function sentButtonClickHandler(event) {
    let data = {
        description: editor.getText()
    };

    ajax.PostJson("/umbraco/api/BulletinsApi/Create", data).then(function(response) {
        if (response.isSuccess) {
            //holder = dropzone = description = toolbar = sentButton= closeButton = header = editor = null;
            cfReloadTab();
            hide();
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
}

function isEdited() {
    return editor.getLength() > 1;
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
        initEditorEventListeners();
        initEventListeners();
    }
}

function getBulletinHolder() {
    return document.querySelector(".js-create-bulletin");
}

function cfReloadTab() {
    document.body.dispatchEvent(cfReloadTabEvent);
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