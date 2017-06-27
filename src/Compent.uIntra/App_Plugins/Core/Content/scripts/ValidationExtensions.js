export default function () {
    document.addEventListener('DOMContentLoaded', function () {
        $.validator.addMethod('greaterthan',
            function (value, element, params) {
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
            },
            function (params, element) {
                return $(element).data('val-greaterthan');
            });

        $.validator.unobtrusive.adapters.add('greaterthan', ['startdate'], function (options) {
            options.rules['greaterthan'] = $('#' + options.params["startdate"]);
            options.messages['greaterthan'] = options.message;

            $(options.element).rules("add", options.rules);
        });

        $.validator.unobtrusive.parse('form');
    })
}