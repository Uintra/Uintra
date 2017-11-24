import helpers from "./Helpers";

var pinControl;
var pinInfoHolder;
var pinAccept;
var pinValue;

var pinActivity = {
    initPinControl: function (holder) {
        pinControl = holder.find('.pin-control');
        pinInfoHolder = holder.find('.pin-info');
        pinAccept = holder.find('.pin-accept');
        pinValue = holder.find('.js-endpin-date-value');

        if (pinControl.is(":unchecked")) {
            pinInfoHolder.hide();
        }
        else {
            pinAccept.prop('checked', true);
        }

        pinControl.on('change', switchPinControlDisabledProp);

        function switchPinControlDisabledProp() {
            if (pinControl.is(":checked")) {
                pinInfoHolder.show();
                pinValue.prop('disabled', false);

            } else {
                pinInfoHolder.hide();
                pinValue.prop('disabled', true);
            }
        }

        switchPinControlDisabledProp();
    },
    initPinDate: function (holder) {
        var pinDate = helpers.initDatePicker(holder, ".js-endpin-date", ".js-endpin-date-value");
        pinDate.set('minDate', new Date());

        var clearEndPinDateBtn = holder.find('.js-clear-endpin-date');
        clearEndPinDateBtn.click(function () {
            pinDate.clear();
        });

        return pinDate;
    },
    isPinAccepted: function (holder) {
        pinControl = holder.find('.pin-control');
        if (pinControl.is(":checked")) {
            if (pinAccept.is(":unchecked")) {
                pinAccept.closest(".check__label").addClass('input-validation-error');
                return false;
            }
        }

        return true;
    }
}

export default pinActivity;