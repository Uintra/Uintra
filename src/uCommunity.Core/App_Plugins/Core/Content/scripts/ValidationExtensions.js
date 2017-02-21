$(document).ready(function () {
    $.validator.addMethod('greaterthan', function (value, element, params) {
        var inputStartDate = $('#js-start-date');
        var inputEndDate = $('#js-end-date');
        if (Date.parse(value) < Date.parse(params.val())) {
            inputStartDate.addClass('input-validation-error');
            inputEndDate.addClass('input-validation-error');
            return false;
        }
        if (inputStartDate.hasClass('input-validation-error') && inputEndDate.hasClass('input-validation-error')) {
            inputStartDate.removeClass('input-validation-error');
            inputEndDate.removeClass('input-validation-error');
        }
        return true;
    }, '');

    $.validator.unobtrusive.adapters.add('greaterthan', ['startdate'], function (options) {
        console.log(options);
        options.rules['greaterthan'] = $('#' + options.params["startdate"]);
        options.messages['greaterthan'] = options.message;
    });
});