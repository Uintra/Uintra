window.initActivityLocationEdit = function (addressHolder, shortAddressHolder, mapContainer) {

    var defaultMapOptions = {
        center: { lat: 55.676098, lng: 12.568337 },
        zoom: 10,
        disableDoubleClickZoom: true
    };

    const denmarkRegionCode = "dk";

    var map = new google.maps.Map(mapContainer, defaultMapOptions);
    var marker = new google.maps.Marker();

    var address = addressHolder.value;
    if (address) {
        geocodeAddress(address, function (results, status) {
            if (status == 'OK') {
                placeMarkerAndPanTo(results[0].geometry.location);
            }
        });
    } else {
        placeMarkerAndPanTo(defaultMapOptions.center);
    }

    map.addListener('dblclick', function (e) {
        placeMarkerAndPanTo(e.latLng);
        placeNewAddress(e.latLng);
    });

    function placeMarkerAndPanTo(latLng) {
        marker.setMap(null);
        marker = new google.maps.Marker({
            position: latLng,
            map: map
        });
        map.panTo(latLng);
    }

    function placeNewAddress(latLng) {
        var latlng = { lat: parseFloat(latLng.lat()), lng: parseFloat(latLng.lng()) };
        geocodeLocation(latlng, function (results, status) {
            if (status == 'OK') {
                fillAddress(results[0]);
            }
        });
    }

    function geocodeAddress(address, callback) {
        var geocoder = new google.maps.Geocoder;
        geocoder.geocode({ 'address': address, 'region': denmarkRegionCode }, callback);
    }

    function geocodeLocation(latLng, callback) {
        var geocoder = new google.maps.Geocoder;
        geocoder.geocode({ 'location': latLng }, callback);
    }

    function fillAddress(geocoderData) {
        addressHolder.value = geocoderData.formatted_address;

        for (var i = 0; i < geocoderData.address_components.length; i++) {
            var address = geocoderData.address_components[i];
            if (address.types[0] == "locality")
                shortAddressHolder.value = address.long_name;
        }
    }

    addressHolder.addEventListener("keypress", function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            mapAddressChanged();
        }
    });

    addressHolder.addEventListener("blur", function () {
        mapAddressChanged();
    });

    function mapAddressChanged() {
        var mapAddress = addressHolder.value;
        geocodeAddress(mapAddress, function (results, status) {
            if (status == 'OK') {
                fillAddress(results[0]);
                placeMarkerAndPanTo(results[0].geometry.location);
            }
        });
    }
}