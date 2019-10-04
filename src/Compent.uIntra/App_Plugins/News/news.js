import helpers from './../Core/Content/scripts/Helpers';
import fileUploadController from './../Core/Controls/FileUpload/file-upload';
import pinActivity from './../Core/Content/scripts/PinActivity';

require('select2');

var marker = {
    HIDDEN_DESCRIPTION_CONTAINER: '#js-hidden-description-container',
    INPUT_VALIDATION_ERROR: '.input-validation-error',
    DESCRIPTION_ELEMENT: '#description',
    USER_SELECT: '.js-user-select',
    FORM: '#form',
    FORM_SUBMIT: '._submit',
    HEADER: '#header',
    PUBLISH_DATE: '#js-publish-date',
    PUBLISH_DATE_VALUE: '#js-publish-date-value',
    UNPUBLISH_DATE: '#js-unpublish-date',
    UNPUBLISH_DATE_VALUE: '#js-unpublish-date-value',
    NEWS_CREATE_PAGE: '#js-news-create-page',
    NEWS_EDIT_PAGE: '#js-news-edit-page',
    LOADING: '_loading'
};

var initUserSelect = function (holder) {
    holder.find(marker.USER_SELECT).select2({});
};

var initSubmitButton = function (holder) {
    var form = holder.find(marker.FORM);
    var btn = holder.find(marker.FORM_SUBMIT);

    btn.click(function (event) {
        if (!form.valid()) {
            event.preventDefault();
            const labelHeight = 26;
            const header = $(marker.HEADER);
            const additionalHeight = header.length > 0 ? header.outerHeight() + labelHeight : labelHeight;
            const invalidELPos = $(marker.INPUT_VALIDATION_ERROR).first().offset().top;
            window.scrollTo(0, invalidELPos - additionalHeight);
            return;
        }

        if (!pinActivity.isPinAccepted(holder)) {
            event.preventDefault();
            return;
        }

        $(this).addClass(marker.LOADING);
        form.submit();
    });
};

var initDescriptionControl = function (holder) {
    var dataStorage = holder.find(marker.HIDDEN_DESCRIPTION_CONTAINER);
    if (!dataStorage) {
        throw new Error(holder.attr('id') + ': Hiden input field missing');
    }
    var descriptionElem = holder.find(marker.DESCRIPTION_ELEMENT);
    var btn = holder.find('.js-submit');

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
        }
    });

    editor.on('text-change', function () {
        if (editor.getLength() > 1 && descriptionElem.hasClass(marker.INPUT_VALIDATION_ERROR)) {
            descriptionElem.removeClass(marker.INPUT_VALIDATION_ERROR);
        }
    });

    btn.click(function () {
        descriptionElem.toggleClass(marker.INPUT_VALIDATION_ERROR, editor.getLength() <= 1);
    });
};

var initDates = function (holder) {
    let publish = helpers.initDatePicker(holder,  marker.PUBLISH_DATE, marker.PUBLISH_DATE_VALUE);
    let unpublish = helpers.initDatePicker(holder, marker.UNPUBLISH_DATE, marker.UNPUBLISH_DATE_VALUE);
    let pin = pinActivity.initPinDate(holder, publish.selectedDates[0]);
    publish.config.onChange.push(publishDateChanged);

    function setMinDate(minDate) {
        if (unpublish) {
            minDate && unpublish.set('minDate', minDate);
        }

        if (pin) {
            pin.setDate(minDate, true);
            pin.set('minDate', minDate);    
        }
    }

    function publishDateChanged(newDates) {
        setMinDate(newDates[0]);
    }
};

var controller = {
    init: function () {
        this.initItem($(marker.NEWS_CREATE_PAGE));
        this.initItem($(marker.NEWS_EDIT_PAGE));
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