import helpers from "./../Core/Content/scripts/Helpers";
import fileUploadController from "./../Core/Controls/FileUpload/file-upload";
import pinActivity from "./../Core/Content/scripts/PinActivity";

require('select2');

var initUserSelect = function (holder) {
    holder.find('.js-user-select').select2({});
};

var initSubmitButton = function (holder) {
    var form = holder.find('#form');
    var btn = holder.find('._submit');

    btn.click(function (event) {
        if (!form.valid()) {
            event.preventDefault();
            const labelHeight = 26;
            const header = $("#header");
            const additionalHeight = header.length > 0 ? header.outerHeight() + labelHeight : labelHeight;
            const invalidELPos = $(".input-validation-error").first().offset().top
            window.scrollTo(0, invalidELPos - additionalHeight);
            return;
        }

        if (!pinActivity.isPinAccepted(holder)) {
            event.preventDefault();
            return;
        }

        $(this).addClass('_loading');
        form.submit();
    });
};

var initDescriptionControl = function (holder) {
    var dataStorage = holder.find('#js-hidden-description-container');
    if (!dataStorage) {
        throw new Error(holder.attr("id") + ": Hiden input field missing");
    }
    var descriptionElem = holder.find('#description');
    var btn = holder.find('.form__btn._submit');

    var toolbarOptions = [
        [{ 'header': [1, 2, 3, false] }],
        ['bold', 'italic', 'underline', 'link'],
        [{ 'list': 'ordered' }, { 'list': 'bullet' }],
        ['emoji'],
        ['clean']
    ];
    var editor = helpers.initQuill(descriptionElem[0], dataStorage[0], {
        modules: {
            toolbar: toolbarOptions
        },
        theme: 'snow'
    });

    let emojiContainer = editor.container.querySelector(".js-emoji");
    if (!emojiContainer) {
        helpers.initSmiles(editor, editor.getModule('toolbar').container);
        emojiContainer = true;
    }

    editor.on('text-change', function () {
        if (editor.getLength() > 1 && descriptionElem.hasClass('input-validation-error')) {
            descriptionElem.removeClass('input-validation-error');
        }
    });

    btn.click(function () {
        descriptionElem.toggleClass("input-validation-error", editor.getLength() <= 1);

    });
};

var initDates = function (holder) {
    let publish = helpers.initDatePicker(holder, "#js-publish-date", "#js-publish-date-value");
    let unpublish = helpers.initDatePicker(holder, "#js-unpublish-date", "#js-unpublish-date-value");
    let pin = pinActivity.initPinDate(holder);

    publish.config.onChange.push(publishDateChanged);

    function setMinDate(minDate) {
        if (unpublish) {
            minDate && unpublish.set('minDate', minDate);
        }

        pin.setDate(minDate, true);
        pin.set('minDate', minDate);
    }

    function publishDateChanged(newDates) {
        setMinDate(newDates[0]);
    }
};

var controller = {
    init: function () {
        this.initItem($('#js-news-create-page'));
        this.initItem($('#js-news-edit-page'));
    },
    initItem: function (holder) {
        if (!holder.length) {
            return;
        }

        pinActivity.initPinControl(holder);
        initSubmitButton(holder);
        initDates(holder);
        initUserSelect(holder);
        initDescriptionControl(holder);
        fileUploadController.init(holder);
    }
};

export default controller;