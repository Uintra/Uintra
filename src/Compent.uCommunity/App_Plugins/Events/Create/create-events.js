require("./../../Core/Content/libs/jquery.validate.min.js");
require("./../../Core/Content/libs/jquery.unobtrusive-ajax.min.js");
require("./../../Core/Content/libs/jquery.validate.unobtrusive.min.js");

import appInitializer from "./../../Core/Content/scripts/AppInitializer";
import helpers from "./../../Core/Content/scripts/Helpers";
import fileUploadController from "./../../Core/Controls/FileUpload/file-upload";
import tagsController from "./../../Tagging/tags";

require('select2');
require('./../../Core/Content/scripts/ValidationExtensions');

var holder;
var userSelect;
var editor;

var initSubmitButton = function () {
    var form = holder.find('#createForm');
    var btn = holder.find('._submit');
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
    });
}

var initPinControl = function () {
    var pinControl = holder.find('#pin-control');
    var pinInfoHolder = holder.find('#pin-info');
    $(pinInfoHolder).hide();

    pinControl.change(function () {
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
        holder = $('#js-events-create-page');

        if (!holder.length) {
            return;
        }

        initSubmitButton();
        initPinControl();
        initUserSelect();
        initDatePickers();
        initDescriptionControl();
        fileUploadController.init(holder);
        tagsController.init();
    }
}

appInitializer.add(controller.init);