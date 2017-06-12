import appInitializer from "./../Core/Content/scripts/AppInitializer";
import initGreaterThan from "./../Core/Content/scripts/ValidationExtensions";
import helpers from "./../Core/Content/scripts/Helpers";
import fileUploadController from "./../Core/Controls/FileUpload/file-upload";
import pinActivity from "./../Core/Content/scripts/PinActivity";

require('./style.css');
require('select2');

var initUserSelect = function (holder) {
    holder.find('.js-user-select').select2({});
}

var initSubmitButton = function (holder) {    
    var form = holder.find('#form');
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

var initDescriptionControl = function (holder) {
    var dataStorage = holder.find('#js-hidden-description-container');
    if (!dataStorage) {
        throw new Error(holder.attr("id") + ": Hiden input field missing");
    }
    var descriptionElem = holder.find('#description');
    var btn = holder.find('.form__btn._submit');
    var editor = helpers.initQuill(descriptionElem[0], dataStorage[0], { theme: 'snow' });

    editor.on('text-change', function () {
        if (editor.getLength() > 1 && descriptionElem.hasClass('input-validation-error')) {
            descriptionElem.removeClass('input-validation-error');
        }
    });

    btn.click(function () {
        descriptionElem.toggleClass("input-validation-error", editor.getLength() <= 1);

    });
}

var initDates = function (holder) {

    var publish = helpers.initDatePicker(holder, "#js-publish-date", "#js-publish-date-value");
    var unpublish = helpers.initDatePicker(holder, "#js-unpublish-date", "#js-unpublish-date-value");
    var pin = pinActivity.initPinDate(holder);

    var initialMinDate = publish.selectedDates[0] || null;
    setMinDate(initialMinDate);

    publish.config.onChange.push(publishDateChanged);

    function setMinDate(minDate) {
        minDate && unpublish.set('minDate', minDate);
        pin.set('minDate', minDate);
    }

    function publishDateChanged(newDates) {
        setMinDate(newDates[0]);

    }
    
    var clearUnpublishDateBtn = holder.find('.js-clear-unpublish-date');
    clearUnpublishDateBtn.click(function () {
        unpublish.clear();
    });
}

var controller = {
    init: function (holder) {
        if (!holder.length) {
            return;
        }
        initSubmitButton(holder);
        initDates(holder);
        pinActivity.initPinControl(holder);
        initUserSelect(holder);
        initDescriptionControl(holder);
        fileUploadController.init(holder);
    }
}

appInitializer.add(() => {
    controller.init($('#js-news-create-page'));
});

appInitializer.add(() => {
    controller.init($('#js-news-edit-page'));
});