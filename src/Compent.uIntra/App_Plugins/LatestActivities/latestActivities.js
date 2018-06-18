import centralFeed from "./../../App_Plugins/CentralFeed/centralFeed.js";

var holders;

function init() {
    holders = document.querySelectorAll('.js-latest-activities');
    if (!holders.length) return;
    holders.forEach(function (holder) {
        var seeAll = holder.querySelector('.js-see-all');
        var activityType = holder.querySelector('input[name="typeId"]');
        var activityTypeId = activityType.value;
        if (seeAll && activityTypeId) {
            seeAll.addEventListener('click', function () {
                centralFeed.goActivityTab(activityTypeId);
            });
        }
    });
}

export default {
    init
}