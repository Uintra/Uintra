import centralFeed from "./../../App_Plugins/CentralFeed/centralFeed.js";

var holders;

function init() {
    holders = $(document).find('.js-latest-activities').toArray();
    if (!holders.length) return;
    holders.forEach(function (holder) {
        var seeAll = holder.querySelector('.js-see-all');
        var activityType = holder.querySelector("input[type='hidden']");
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