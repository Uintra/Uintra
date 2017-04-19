import appInitializer from "./../../Core/Content/scripts/AppInitializer";
import helpers from "./../../Core/Content/scripts/Helpers";
import fileUploadController from "./../../Core/Controls/FileUpload/file-upload";

require('select2');

require('flatpickr/dist/flatpickr.min.css');
require('dropzone/dist/min/dropzone.min.css');
require("./../_news.css");

var initUserSelect = function (holder) {
    holder.find('#js-user-select').select2({});
}

var initDescriptionControl = function (holder) {
    var dataStorage = holder.find('#js-hidden-description-container');
    if (!dataStorage) {
        throw new Error("EditNews: Hiden input field missing");
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


var controller = {
    init: function () {
        var holder = $('#js-news-create-page');

        if (!holder.length) {
            return;
        }

        initUserSelect(holder);
        helpers.initPublishDatePicker(holder);
        initDescriptionControl(holder);
        fileUploadController.init(holder);
    }
}

appInitializer.add(controller.init);