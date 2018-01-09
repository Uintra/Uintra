require('./styles.css');
import initEdit from './../activityLocationEdit';

var init = function () {
    var locationContainer = document.querySelector(".js-location-edit");
    if (!locationContainer) {
        return;
    }

    let addressHolder = locationContainer.querySelector("#js-map-address");
    let shortAddressHolder = locationContainer.querySelector("#js-map-short");
    let mapContainer = locationContainer.querySelector("#js-map-container");

    initEdit(addressHolder, shortAddressHolder, mapContainer);
}

export default init;