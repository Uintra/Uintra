require("./../../Core/Content/libs/jquery.validate.min.js");
require("./../../Core/Content/libs/jquery.unobtrusive-ajax.min.js");
require("./../../Core/Content/libs/jquery.validate.unobtrusive.min.js");

import appInitializer from "./../../Core/Content/scripts/AppInitializer";
import helpers from "./../../Core/Content/scripts/Helpers";
import fileUploadController from "./../../Core/Controls/FileUpload/file-upload";
import pinActivity from "./../../Core/Content/scripts/PinActivity";

require('select2');
require('./../../Core/Content/scripts/ValidationExtensions');

var holder;
var userSelect;
var editor;

var initSubmitButton = function () {
    var form = holder.find('#createForm');
    var btn = holder.find('._submit');

    btn.click(function (event) {
        if (!form.valid()) {
            event.preventDefault();
            return;
        }

        if (!pinActivity.isPinAccepted(holder)) {
            event.preventDefault();
            return;
        }

        form.submit();
    });
}

var initUserSelect = function () {
    userSelect = holder.find('.js-user-select').select2({});
}

var initDescriptionControl = function () {
    var dataStorage = holder.find('#js-hidden-description-container');
    var descriptionElem = holder.find('#description');
    var btn = holder.find('.form__btn._submit');

    editor = helpers.initQuill(descriptionElem[0], dataStorage[0], { theme: 'snow' });

    editor.on('text-change', function () {
        if (editor.getLength() > 1 && descriptionElem.hasClass('input-validation-error')) {
            descriptionElem.removeClass('input-validation-error');
        }
    });

    btn.click(function () {
        editor.getLength() <= 1 ?
            descriptionElem.addClass('input-validation-error') :
            descriptionElem.removeClass('input-validation-error');
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
        holder = $('#js-events-create-page');

        if (!holder.length) {
            return;
        }

        initSubmitButton();
        pinActivity.initPinControl(holder);
        initUserSelect();
        initDatePickers();
        initDescriptionControl();
        fileUploadController.init(holder);
    }
}

appInitializer.add(controller.init);