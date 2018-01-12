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

        hasEventConfirmation().then(function (result) {
            if (!result.HasConfirmation) {
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

            showNotifyAllSubscribersDialog(callbacks);
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
        [{ 'list': 'ordered' }, { 'list': 'bullet' }],
        ['emoji'],
        ['clean']
    ];

    editor = helpers.initQuill(descriptionElem[0], dataStorage[0], {
        placeholder: dataStorage.first().data('placeholder') || '',
        modules: {
            toolbar: toolbarOptions
        },
        theme: 'snow'
    });

    let emojiContainer = editor.container.querySelector(".js-emoji");
    if (!emojiContainer) {
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
    var hideControl = holder.find('.js-event-hide');
    var text = hideControl.data('text');
    var title = hideControl.data('title');
    var returnUrl = hideControl.data('return-url');

    alertify.defaults.glossary.cancel = hideControl.data('cancel');
    alertify.defaults.glossary.ok = hideControl.data('ok');

    hideControl.on('click', function () {
        confirm.showConfirm(title, text, function () {

            hasEventConfirmation().then(function (result) {
                if (!result.HasConfirmation) {
                    hideEvent(false, returnUrl);
                    return;
                }

                let callbacks = [
                    function () {
                        hideEvent(true, returnUrl);
                    }, function () {
                        hideEvent(false, returnUrl);
                    },
                    function () {
                        return true;
                    }
                ];

                showNotifyAllSubscribersDialog(callbacks);
            });
        }, function () { }, confirm.defaultSettings);

        return false;
    });
}

function hideEvent(notifyAllSubscribers, returnUrl) {
    let eventId = holder.find('[name="id"]').val();

    $.post('/umbraco/surface/Events/Hide?id=' + eventId + '&isNotificationNeeded=' + notifyAllSubscribers, function () {
        window.location.href = returnUrl;
    });
}

function hasEventConfirmation() {
    let eventId = holder.find('[name="id"]').val();

    return $.get('/umbraco/surface/Events/HasConfirmation?id=' + eventId);
}

function showNotifyAllSubscribersDialog(callbacks) {
    let popupTranslations = holder.find('.js-notification-popup-translations');

    alertify.defaults.glossary.cancel = popupTranslations.data('cancel');
    alertify.defaults.glossary.yes = popupTranslations.data('yes');
    alertify.defaults.glossary.no = popupTranslations.data('no');

    confirm.showDialog(popupTranslations.data('title'), popupTranslations.data('text'), callbacks, confirm.defaultSettings);
}

let initDatePickers = function () {
    let start = helpers.initDatePicker(holder, '#js-start-date', '#js-start-date-value');
    let end = helpers.initDatePicker(holder, '#js-end-date', '#js-end-date-value');
    let publish = helpers.initDatePicker(holder, '#js-publish-date', '#js-publish-date-value');
    let pin = pinActivity.initPinDate(holder);

    function startOnChange(newDates) {
        let newDate = newDates[0];
        let endDate = end.selectedDates[0];
        if (endDate != null && endDate < new Date(newDate)) {
            end.setDate(newDate);
            pin.setDate(newDate);
        }
        end.set('minDate', newDate);
        pin.setDate(newDate);
        pin.set('minDate', newDate);
    }

    function endOnChange(newDates) {
        var newDate = newDates[0];
        pin.setDate(newDate);
    }

    start.config.onChange.push(startOnChange);
    end.config.onChange.push(endOnChange);
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