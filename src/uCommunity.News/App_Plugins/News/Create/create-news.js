import appInitializer from "./../../Core/Content/scripts/AppInitializer";
import helpers from "./../../Core/Content/scripts/Helpers";
import fileUploadController from "./../../Core/Controls/FileUpload/file-upload";

require('select2');

require('flatpickr/dist/flatpickr.min.css');
require('dropzone/dist/min/dropzone.min.css');
require("./../_news.css");

var holder;
var userSelect;
var editor;

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

var controller = {
    init: function () {
        holder = $('#js-news-create-page');

        if (!holder.length) {
            return;
        }

        initUserSelect();
        helpers.initPublishDatePicker(holder);
        initDescriptionControl();
        fileUploadController.init(holder);
    }
}

appInitializer.add(controller.init);