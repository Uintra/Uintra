import centralFeed from "./../../App_Plugins/CentralFeed/centralFeed.js";

var holder;

function init() {
    debugger;
    holder = document.querySelector('.js-latest-activities');
    if (!holder) return;
    var seeAll = holder.querySelector('.js-see-all');
    var activityType = holder.querySelector("input[type='hidden']");

    if (seeAll && activityType) {
        var activityTypeId = activityType.value;
        seeAll.addEventListener('click', function () {
            centralFeed.goActivityTab(activityTypeId);
        });
    }
}

export default {
    init
}