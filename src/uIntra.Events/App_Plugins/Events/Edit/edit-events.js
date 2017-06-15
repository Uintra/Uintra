require("./../../Core/Content/libs/jquery.validate.min.js");
require("./../../Core/Content/libs/jquery.unobtrusive-ajax.min.js");
require("./../../Core/Content/libs/jquery.validate.unobtrusive.min.js");

import appInitializer from "./../../Core/Content/scripts/AppInitializer";
import helpers from "./../../Core/Content/scripts/Helpers";
import fileUploadController from "./../../Core/Controls/FileUpload/file-upload";
import confirm from "./../../Core/Controls/Confirm/Confirm";
import pinActivity from "./../../Core/Content/scripts/PinActivity";


var alertify = require('alertifyjs/build/alertify.min');
require('alertifyjs/build/css/alertify.min.css');
require('alertifyjs/build/css/themes/default.min.css');

require('select2');
require('./../../Core/Content/scripts/ValidationExtensions');

var holder;
var userSelect;
var editor;
var form;

var initUserSelect = function () {
    userSelect = holder.find('.js-user-select').select2({});
}

var continueSubmit = function (notifyAllSubscribers) {
    $('#notifyAllSubscribers').val(notifyAllSubscribers);
    form.submit();
}

var initSubmitButton = function () {
    form = holder.find('#editForm');
    var btn = holder.find('.form__btn._submit');
    var descriptionElem = holder.find('#description');   

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

        var data = helpers.serialize(form[0]);

        $.post('/umbraco/surface/Events/HasConfirmation', data, function (result) {
            if (result && !result.HasConfirmation) {
                continueSubmit(false);
                return;
            }

            var callbacks = [
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

            confirm.showDialog(btn.data('text'), callbacks, confirm.defaultSettings);
        });
    });
}

var initDescriptionControl = function () {
    var dataStorage = holder.find('#js-hidden-description-container');
    if (!dataStorage) {
        throw new Error("EditNews: Hiden input field missing");
    }
    var descriptionElem = holder.find('#description');

    editor = helpers.initQuill(descriptionElem[0], dataStorage[0], { theme: 'snow' });

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

var initHideControl = function () {
    var hideControl = holder.find('.js-event-hide');
    var text = hideControl.data('text');
    var textOk = hideControl.data('ok');
    var textCancel = hideControl.data('cancel');

    alertify.defaults.glossary.cancel = textCancel;
    alertify.defaults.glossary.ok = textOk;

    hideControl.on('click', function () {
        confirm.showConfirm(text, function () {
            $.post('/umbraco/surface/Events/Hide?id=' + hideControl.data('id'),function () {
                var url = hideControl.data('return-url');
                window.location.href = url;
            });
        }, function () { }, confirm.defaultSettings);

        return false;
    });
}

var initDatePickers = function () {
    var start = helpers.initDatePicker(holder, '#js-start-date', '#js-start-date-value');
    var end = helpers.initDatePicker(holder, '#js-end-date', '#js-end-date-value');
    var pin = pinActivity.initPinDate(holder);

    function startOnChange(newDates) {
        var newDate = newDates[0];
        var endDate = end.selectedDates[0];
        if (endDate != null && endDate < new Date(newDate)) {
            end.setDate(newDate);
            pin.setDate(newDate);
        }
        end.set('minDate', newDate);
        pin.set('minDate', newDate);
    }

    start.config.onChange.push(startOnChange);
}

var controller = {
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

appInitializer.add(controller.init);