import helpers from "./Helpers";

var initPinDate = function (holder) {
    var pinDate = helpers.initDatePicker(holder, "#js-endpin-date", "#js-endpin-date-value");

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
}

export default initPinDate;