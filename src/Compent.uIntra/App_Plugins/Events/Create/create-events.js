import helpers from "./../../Core/Content/scripts/Helpers";
import fileUploadController from "./../../Core/Controls/FileUpload/file-upload";
import pinActivity from "./../../Core/Content/scripts/PinActivity";

var holder;
var userSelect;
var editor;

var initUserSelect = function () {
    userSelect = holder.find('.js-user-select').select2({});
}

var initSubmitButton = function () {
    var form = holder.find('#createForm');
    var btn = holder.find('._submit');

    btn.click(function (event) {
        if (!form.valid()) {
            event.preventDefault();
            const header = $("#header");
            const headerAndLabelHeight = header.length > 0 ? header.outerHeight() + 26 : 26;
            const invalidELPos = $(".input-validation-error").first().offset().top

            window.scrollTo(0, invalidELPos - headerAndLabelHeight);
            return;
        }

        if (!pinActivity.isPinAccepted(holder)) {
            event.preventDefault();
            return;
        }

        $(this).addClass('_loading');
        form.submit();
    });
}

var initDescriptionControl = function () {
    var dataStorage = holder.find('#js-hidden-description-container');
    var descriptionElem = holder.find('#description');
    var btn = holder.find('.js-submit');
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
        }
    });

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
    var publish = helpers.initDatePicker(holder, '#js-publish-date', '#js-publish-date-value');
    var pin = pinActivity.initPinDate(holder);

    function startOnChange(newDates) {
        var newDate = newDates[0];
        var endDate = end.selectedDates[0];
        if (endDate != null && endDate < new Date(newDate)) {
            end.setDate(newDate, true);
        }

     //   end.set('minDate', newDate);
    }

    function publishOnChange(newDates) {
        var newDate = newDates[0];
        pin.setDate(newDate, true);
        pin.set('minDate', newDate);
    }

    start.config.onChange.push(startOnChange);
    publish.config.onChange.push(publishOnChange);
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

export default controller;