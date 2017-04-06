import appInitializer from "./../../Core/Content/scripts/AppInitializer";
import helpers from "./../../Core/Content/scripts/Helpers";
import fileUploadController from "./../../Core/Controls/FileUpload/file-upload";

var Flatpickr = require('flatpickr');
var FlatpickrLang = require('flatpickr/dist/l10n/da');

require('select2');

require('flatpickr/dist/flatpickr.min.css');
require('dropzone/dist/min/dropzone.min.css');

'use strict';
var CreateEventsController = (function () {
    var holder;
    var userSelect;
    var editor;

    var initUserSelect = function () {
        userSelect = holder.find('#js-user-select').select2({});
    }

    var initDatePicker = function (containerSelector, valueSelector) {
        var dateElem = holder.find(containerSelector);
        var dateFormat = dateElem.data('dateFormat');
        var dateElemValue = holder.find(valueSelector);
        var defaultDate = new Date(dateElem.data('defaultDate'));

        var datePicker = new Flatpickr(dateElem[0], {
            minDate: new Date().setHours(0, 0, 0, 0),
            enableTime: true,
            time_24hr: true,
            allowInput: false,
            weekNumbers: true,
            dateFormat: dateFormat,
            locale: FlatpickrLang.da,
            onChange: function (selectedDates) {
                if (selectedDates.length === 0) {
                    dateElemValue.val('');
                    return;
                }

                var selectedDate = selectedDates[0].toISOString();
                dateElemValue.val(selectedDate);
            }
        });

        datePicker.setDate(helpers.removeOffset(defaultDate), true);
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

    var controller = {
        init: function () {
            holder = $('#js-events-create-page');

            if (!holder.length) {
                return;
            }

            initUserSelect();
            initDatePicker('#js-start-date', '#js-start-date-value');
            initDatePicker('#js-end-date', '#js-end-date-value');
            initDescriptionControl();
            fileUploadController.init(holder);
        }
    }
    appInitializer.add(controller.init);

    return controller;
})();

window.App = window.App || {};
window.App.CreateEventsController = CreateEventsController;