require("./../../Core/Content/libs/jquery.validate.min.js");
require("./../../Core/Content/libs/jquery.unobtrusive-ajax.min.js");
require("./../../Core/Content/libs/jquery.validate.unobtrusive.min.js");

import appInitializer from "./../../Core/Content/scripts/AppInitializer";
import helpers from "./../../Core/Content/scripts/Helpers";
import fileUploadController from "./../../Core/Controls/FileUpload/file-upload";
import confirm from "./../../Core/Controls/Confirm/Confirm";

var alertify = require('alertifyjs/build/alertify.min');

require('select2');
require('./../../Core/Content/scripts/ValidationExtensions');

var holder;
var userSelect;
var editor;
var form;

var initPinControl=function() {    
    var pinControl = holder.find('#pin-control');
    var pinInfoHolder = holder.find('#pin-info');
    if (pinControl.is(":unchecked")) {
        pinInfoHolder.hide();
    } else {
        var pinAccept = holder.find('#pin-accept');
        pinAccept.prop('checked', true);
    }
    pinControl.change(function() {
        if ($(this).is(":checked")) {
            pinInfoHolder.show();
        } else {
            pinInfoHolder.hide();
        }
    });
}

var initUserSelect = function () {
    userSelect = holder.find('#js-user-select').select2({});
}

var continueSubmit = function (value) {
    $('#notifyAllSubscribers').val(value);
    form.submit();
}

var initSubmitButton = function () {
    form = holder.find('#editForm');
    var btn = holder.find('.form__btn._submit');
    var descriptionElem = holder.find('#description');
    var pinControl = holder.find('#pin-control');    

    btn.click(function (event) {
        if (!form.valid()) {
            event.preventDefault();
            return;
        }

        if (pinControl.is(":checked")) {
            var pinAccept = holder.find('#pin-accept');
            if (pinAccept.is(":unchecked")) {
                pinAccept.closest(".check__label").addClass('input-validation-error');
                event.preventDefault();
                return;
            }
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
            $.post('/umbraco/surface/Events/Hide?id=' + hideControl.data('id'),
                function (data) {
                    window.location.href = data.Url;
                });
        }, function () { }, confirm.defaultSettings);

        return false;
    });
}

var initDatePickers = function () {
    var start = helpers.initDatePicker(holder, '#js-start-date', '#js-start-date-value');
    var end = helpers.initDatePicker(holder, '#js-end-date', '#js-end-date-value');

    function startOnChange(newDates) {
        var newDate = newDates[0];
        var endDate = end.selectedDates[0];
        if (endDate != null && endDate < new Date(newDate)) {
            end.setDate(newDate);
        }
        end.set('minDate', newDate);
    }

    start.config.onChange.push(startOnChange);
}

var controller = {
    init: function () {
        holder = $('#js-events-edit-page');
        if (!holder.length) {
            return;
        }

        initPinControl();
        initUserSelect();
        initDatePickers();
        initDescriptionControl();
        initSubmitButton();
        initHideControl();
        fileUploadController.init(holder);
    }
}

appInitializer.add(controller.init);