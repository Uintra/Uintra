import helpers from "./Helpers";

var pinActivity = {
    initPinControl: function(holder) {    
        var pinControl = holder.find('.pin-control');
        var pinInfoHolder = holder.find('.pin-info');
        if (pinControl.is(":unchecked")) {
            pinInfoHolder.hide();
        }
        else {
            var pinAccept = holder.find('.pin-accept');
            pinAccept.prop('checked', true);
        }
        pinControl.change(function() {
            if ($(this).is(":checked")) {
                pinInfoHolder.show();
            } else {
                pinInfoHolder.hide();
            }
        });
    },
    initPinDate: function (holder) {
        var pinDate = helpers.initDatePicker(holder, ".js-endpin-date", ".js-endpin-date-value");

        var initialMinDate = new Date();
        setMinDate(initialMinDate);
    
        function setMinDate(minDate) {
            minDate && pinDate.set('minDate', minDate);
        }

        var clearEndPinDateBtn = holder.find('.js-clear-endpin-date');
        clearEndPinDateBtn.click(function () {
            pinDate.clear();
        });

        return pinDate;
    },
    isPinAccepted: function(holder) {
        var pinControl = holder.find('.pin-control');
        if (pinControl.is(":checked")) {
            var pinAccept = holder.find('.pin-accept');
            if (pinAccept.is(":unchecked")) {
                pinAccept.closest(".check__label").addClass('input-validation-error');
                return false;
            }
        }

        return true;
    }
}

export default pinActivity;