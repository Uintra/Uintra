import appInitializer from "./../Core/Content/scripts/AppInitializer";
import helpers from "./../Core/Content/scripts/Helpers";
import fileUploadController from "./../Core/Controls/FileUpload/file-upload";

require('select2');

var initUserSelect = function (holder) {
    holder.find('#js-user-select').select2({});
}

var initDescriptionControl = function (holder, isEdit) {
    var dataStorage = holder.find('#js-hidden-description-container');
    if (!dataStorage) {
        throw new Error(holder.attr("id") + ": Hiden input field missing");
    }
    var descriptionElem = holder.find('#description');
    var btn = holder.find('.form__btn._submit');
    var editor = helpers.initQuill(descriptionElem[0], dataStorage[0], { theme: 'snow' });

    editor.on('text-change', function () {
        if(isEdit){
            dataStorage.val(editor.container.firstChild.innerHTML);
        }

        if (editor.getLength() > 1 && descriptionElem.hasClass('input-validation-error')) {
            descriptionElem.removeClass('input-validation-error');
        }
    });

    btn.click(function () {
        descriptionElem.toggleClass("input-validation-error", editor.getLength() <= 1);
    });
}

var controller = {
    init: function (holder, isEdit) {
        if (!holder.length) {
            return;
        }

        initUserSelect(holder);
        helpers.initPublishDatePicker(holder);
        initDescriptionControl(holder, isEdit);
        fileUploadController.init(holder);
    }
}

appInitializer.add(() => {
    controller.init($('#js-news-create-page'));
});

appInitializer.add(() => {
    controller.init($('#js-news-edit-page'), true);
});