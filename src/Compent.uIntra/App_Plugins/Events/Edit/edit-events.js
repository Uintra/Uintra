import helpers from "./../../Core/Content/scripts/Helpers";
import fileUploadController from "./../../Core/Controls/FileUpload/file-upload";
import confirm from "./../../Core/Controls/Confirm/Confirm";
import pinActivity from "./../../Core/Content/scripts/PinActivity";

let alertify = require('alertifyjs/build/alertify.min');

let holder;
let userSelect;
let editor;
let form;

let initUserSelect = function () {
    userSelect = holder.find('.js-user-select').select2({});
}

let continueSubmit = function (notifyAllSubscribers) {
    $('#notifyAllSubscribers').val(notifyAllSubscribers);
    form.submit();
}

let initSubmitButton = function () {
    form = holder.find('#editForm');
    let btn = holder.find('.js-disable-submit');
    let descriptionElem = holder.find('#description');

    btn.click(function (event) {
        if (!form.valid()) {
            event.preventDefault();
            return;
        }

        if (!pinActivity.isPinAccepted(holder)) {
            event.preventDefault();
            return;
        }

        if (editor.getLength() <= 1) {
            event.preventDefault();
            descriptionElem.addClass('input-validation-error');
        }

        descriptionElem.removeClass('input-validation-error');
        event.preventDefault();

        let data = helpers.serialize(form[0]);

        $.post('/umbraco/surface/Events/HasConfirmation', data, function (result) {
            if (result && !result.HasConfirmation) {
                continueSubmit(false);
                return;
            }

            let callbacks = [
                function () {
                    continueSubmit(true);
                }, function () {
                    continueSubmit(false);
                },
                function () {
                    return true;
                }
            ];

            alertify.defaults.glossary.cancel = btn.data('cancel');
            alertify.defaults.glossary.yes = btn.data('yes');
            alertify.defaults.glossary.no = btn.data('no');

            confirm.showDialog(btn.data('title'), btn.data('text'), callbacks, confirm.defaultSettings);
        });
    });
}

let initDescriptionControl = function () {
    let dataStorage = holder.find('#js-hidden-description-container');
    if (!dataStorage) {
        throw new Error("EditNews: Hiden input field missing");
    }
    let descriptionElem = holder.find('#description');
    var toolbarOptions = [
        [{ 'header': [1, 2, 3, false] }],
        ['bold', 'italic', 'underline', 'link'],
        [{ 'list': 'ordered'}, { 'list': 'bullet' }],
        ['emoji'],
        ['clean']
    ];

    editor = helpers.initQuill(descriptionElem[0], dataStorage[0], { 
        modules: {
            toolbar: toolbarOptions
        },
        theme: 'snow'
    });

    let emojiContainer = editor.container.querySelector(".js-emoji");
    if(!emojiContainer){
        helpers.initSmiles(editor, editor.getModule('toolbar').container);
        emojiContainer = true;
    }

    editor.on('text-change', function () {
        if (!editor.getText().trim()) {
            dataStorage.val('');
            return;
        }
        dataStorage.val(editor.container.firstChild.innerHTML);

        if (editor.getLength() > 1 && descriptionElem.hasClass('input-validation-error')) {
            descriptionElem.removeClass('input-validation-error');
        }
    });
}

let initHideControl = function () {
    let hideControl = holder.find('.js-event-hide');
    let text = hideControl.data('text');
    let title = hideControl.data('title');
    let textOk = hideControl.data('ok');
    let textCancel = hideControl.data('cancel');

    alertify.defaults.glossary.cancel = textCancel;
    alertify.defaults.glossary.ok = textOk;

    hideControl.on('click', function () {
        confirm.showConfirm(title, text, function () {
            let eventId = hideControl.data('id');
            let redirectUrl = hideControl.data('return-url');
            let callbacks = [
                () => hideEvent(eventId, true, () => redirectToUrl(redirectUrl)),
                () => hideEvent(eventId, false, () => redirectToUrl(redirectUrl)),
                () => true
            ];

            let popupStringsHolder = holder.find(".js-notification-popup-strings-holder")

            alertify.defaults.glossary.cancel = popupStringsHolder.data('cancel');
            alertify.defaults.glossary.yes = popupStringsHolder.data('yes');
            alertify.defaults.glossary.no = popupStringsHolder.data('no');

            confirm.showDialog(popupStringsHolder.data('title'), popupStringsHolder.data('text'), callbacks, confirm.defaultSettings);
        }, () => { }, confirm.defaultSettings);

        return false;
    });
}

function redirectToUrl(url) {
    window.location.href = url;
}

function hideEvent(eventId, isNotificationNeeded, callback) {
    $.post('/umbraco/surface/Events/Hide?id=' + eventId + '&isNotificationNeeded=' + isNotificationNeeded, callback);
}

let initDatePickers = function () {
    let start = helpers.initDatePicker(holder, '#js-start-date', '#js-start-date-value');
    let end = helpers.initDatePicker(holder, '#js-end-date', '#js-end-date-value');
    let pin = pinActivity.initPinDate(holder);

    function startOnChange(newDates) {
        let newDate = newDates[0];
        let endDate = end.selectedDates[0];
        if (endDate != null && endDate < new Date(newDate)) {
            end.setDate(newDate);
            pin.setDate(newDate);
        }
        end.set('minDate', newDate);
        pin.set('minDate', newDate);
    }

    start.config.onChange.push(startOnChange);
}

let controller = {
    init: function () {
        holder = $('#js-events-edit-page');
        if (!holder.length) {
            return;
        }

        pinActivity.initPinControl(holder);
        initUserSelect();
        initDatePickers();
        initDescriptionControl();
        initSubmitButton();
        initHideControl();
        fileUploadController.init(holder);
    }
}

export default controller;