import appInitializer from "./../../Core/Content/scripts/AppInitializer";
import helpers from "./../../Core/Content/scripts/Helpers";
import fileUploadController from "./../../Core/Controls/FileUpload/file-upload";
import umbracoAjaxForm from "./../../Core/Content/scripts/UmbracoAjaxForm";

var Flatpickr = require('flatpickr');
var FlatpickrLang = require('flatpickr/dist/l10n/da');
var Alertify = require('alertifyjs/build/alertify.min');

require('select2');
require('./../../Core/Controls/Confirm/Confirm');

require('flatpickr/dist/flatpickr.min.css');
require('dropzone/dist/min/dropzone.min.css');

'use strict';
var EditEventsController = (function () {
    var holder;
    var userSelect;
    var editor;
    var form;

    var initUserSelect = function () {
        userSelect = holder.find('#js-user-select').select2({});
    }

    var initDatePicker = function (containerSelector, valueSelector) {
        var dateElem = holder.find(containerSelector);
        var dateFormat = dateElem.data('dateFormat');
        var dateElemValue = holder.find(valueSelector);
        var defaultDate = new Date(dateElem.data('defaultDate'));

        var datePicker = new Flatpickr(dateElem[0], {
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

                dateElemValue.val(selectedDates[0].toISOString());
            }
        });

        defaultDate = helpers.removeOffset(defaultDate);
        datePicker.setDate(defaultDate, true);
        var minDate = new Date();
        if (defaultDate < minDate) {
            minDate = defaultDate;
        }

        datePicker.set('minDate', minDate.setHours(0));
    }

    var continueSubmit = function (value) {
        $('#notifyAllSubscribers').val(value);
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

            if (editor.getLength() <= 1) {
                event.preventDefault();
                descriptionElem.addClass('input-validation-error');
            }

            descriptionElem.removeClass('input-validation-error');
            event.preventDefault();

            var data = umbracoAjaxForm()(form[0]).serialize();
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

                var textQuestion = btn.data('text');
                var textYes = btn.data('yes');
                var textNo = btn.data('no');
                var textCancel = btn.data('cancel');
                Alertify.defaults.glossary.cancel = textCancel;
                Alertify.defaults.glossary.yes = textYes;
                Alertify.defaults.glossary.no = textNo;

                window.App.Confirm.showDialog(textQuestion, callbacks, App.Confirm.defaultSettings);
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
        Alertify.defaults.glossary.cancel = textCancel;
        Alertify.defaults.glossary.ok = textOk;

        hideControl.on('click', function () {
            window.App.Confirm.showConfirm(text,
                 function () {
                     $.post('/umbraco/surface/Events/Hide?id=' + hideControl.data('id'),
                         function (data) {
                             window.location.href = data.Url;
                         });
                 }, function () { }, App.Confirm.defaultSettings);

            return false;
        });
    }

    var controller = {
        init: function () {
            holder = $('#js-events-edit-page');
            if (!holder.length) {
                return;
            }

            initUserSelect();
            initDatePicker('#js-start-date', '#js-start-date-value');
            initDatePicker('#js-end-date', '#js-end-date-value');
            initDescriptionControl();
            initSubmitButton();
            initHideControl();
            fileUploadController.init(holder);
        }
    }
    appInitializer.add(controller.init);
    return controller;
})();

window.App = window.App || {};
window.App.EditEventsController = EditEventsController;
