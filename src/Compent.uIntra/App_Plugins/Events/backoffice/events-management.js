(function (angular) {
    'use strict';

    var onError = function (error) { console.error(error); }

    var controller = function ($http, authResource, $scope, $timeout, eventsManagementConfig, intranetUserService) {
        var self = this;
        self.eventsList = [];
        self.currentUser = null;
        self.dateFormat = "dd/MM/yyyy";
        self.selected = null;
        self.selectedIndex = null;
        self.filterModel = {};
        self.users = [];
        self.startDatePicker = null;
        self.endDatePicker = null;
        self.publishDatePicker = null;

        self.filter = function (item) {
            var checkList = [];

            var compareText = function (left, right) {
                return left.toLowerCase().indexOf(right.toLowerCase()) > -1;
            }

            var compareDates = function (itemDate, filter) {
                var date = new Date(itemDate);
                date.setHours(0, 0, 0, 0);

                if (filter) {
                    if (filter.from) {
                        checkList.push(date >= new Date(filter.from));
                    }
                    if (filter.to) {
                        checkList.push(date <= new Date(filter.to));
                    }
                }
            }

            if (self.filterModel.id) {
                checkList.push(compareText(item.id, self.filterModel.id));
            }
            if (self.filterModel.title) {
                checkList.push(compareText(item.title, self.filterModel.title));
            }
            if (self.filterModel.startDate) {
                compareDates(item.startDate, self.filterModel.startDate);
            }
            if (self.filterModel.endDate) {
                compareDates(item.endDate, self.filterModel.endDate);
            }
            if (self.filterModel.createdDate) {
                compareDates(item.createdDate, self.filterModel.createdDate);
            }
            if (self.filterModel.modifyDate) {
                compareDates(item.modifyDate, self.filterModel.modifyDate);
            }

            return checkList.filter(function (item) { return !!item; }).length == checkList.length;
        }

        self.getHeadClasses = function () {
            return {
                '_admin-mode': self.currentUser != null && self.currentUser.id == 0,
                '_events-selected': self.selected != null
            };
        }

        self.selectEventsToEdit = function (events, index) {
            if (events == null) {
                var currentOwner = self.users.filter(function (user) { return user.umbracoId === self.currentUser.id })[0];
                events = { owner: currentOwner };
            } else {
                events.owner = self.users.filter(function (user) { return user.id === events.ownerId })[0];
            }

            self.selectedIndex = index;
            self.selected = angular.copy(events);
            self.selected.startDate = self.selected.startDate || new Date().toISOString();
            self.selected.endDate = self.selected.endDate || new Date().toISOString();
            self.selected.publishDate = self.selected.publishDate || new Date().toISOString();

            self.config.endDate.minDate = self.selected.startDate;
        }

        self.save = function () {
            if ($scope.editForm.$invalid) {
                $scope.editForm.$setDirty();
                return;
            }

            self.selected.ownerId = self.selected.owner.id;

            if (self.selected.id == null) {
                create(self.selected);
            } else {
                save(self.selected, self.selectedIndex);
            }
        }

        self.publish = function (events, $index) {
            self.clearSelected();
            var editedEvents = angular.copy(events);
            editedEvents.isHidden = false;
            save(editedEvents, $index);
        }

        self.unpublish = function (events, $index) {
            self.clearSelected();
            var editedEvents = angular.copy(events);
            editedEvents.isHidden = true;
            save(editedEvents, $index);
        }

        self.delete = function (events, $index) {
            self.clearSelected();
            if (!confirm('Are you sure?')) {
                return;
            }

            $http.delete('/Umbraco/backoffice/Api/EventsSection/Delete?id=' + events.id).then(function (response) {
                self.eventsList.splice($index, 1);
                self.clearSelected();
            }, onError);
        }

        self.clearSelected = function () {
            self.selectedIndex = self.selected = null;
        }

        self.startDateChanged = function () {
            $timeout(function () {
                var startDate = self.startDatePicker.selectedDates[0];
                if (startDate == null) {
                    return;
                }

                startDate.setHours(0, 0, 0, 0);
                setEndMinDate(startDate)

                var endDate = self.endDatePicker.selectedDates[0];
                if (endDate != null) {
                    endDate.setHours(0, 0, 0, 0);
                }

                if (endDate == null || endDate < startDate) {
                    self.endDatePicker.setDate(startDate);
                }
            })
        }

        function setEndMinDate(minDate) {
            self.endDatePicker.set('minDate', minDate);
        }

        self.endDateChanged = function () {
            $timeout(function () {
                var endDate = self.endDatePicker.selectedDates[0];
                if (endDate == null) {
                    return;
                }

                var startDate = self.startDatePicker.selectedDates[0];
                if (startDate == null) {
                    return;
                }
                startDate.setHours(0, 0, 0, 0);
                self.startDatePicker.setDate(startDate);
            })
        }

        var create = function (events) {
            $http.post('/Umbraco/backoffice/Api/EventsSection/Create', events).then(function (response) {
                self.eventsList.push(response.data);
                self.clearSelected();
            }, onError);
        }

        var save = function (events, index) {
            $http.post('/Umbraco/backoffice/Api/EventsSection/Save', events).then(function (response) {
                self.eventsList.splice(index, 1, response.data);
                self.clearSelected();
            }, onError);
        }

        var loadAll = function () {
            var promise = $http.get('/Umbraco/backoffice/Api/EventsSection/GetAll');
            var success = function (response) {
                self.eventsList = response.data || [];
            }
            promise.then(success, onError);
        }

        var loadUsers = function () {
            var promise = intranetUserService.getAll();
            var success = function (response) {
                self.users = response.data || [];
            }
            promise.then(success, onError);
        }

        var activate = function () {
            loadAll();
            loadUsers();
            authResource.getCurrentUser().then(function (data) {
                self.currentUser = data;
            });
            self.config = eventsManagementConfig;
        }

        activate();
    }


    var init = function () {
        //debugger;
        var locationContainer = document.querySelector(".js-location-edit");
        if (!locationContainer) {
            return;
        }

        var defaultMapOptions = {
            center: { lat: 55.676098, lng: 12.568337 },
            zoom: 10,
            disableDoubleClickZoom: true
        };

        const denmarkRegionCode = "dk";

        var dataStorage = locationContainer.querySelector("#js-map-container");

        var map = new google.maps.Map(dataStorage, defaultMapOptions);
        var marker = new google.maps.Marker();

        var address = locationContainer.querySelector("#js-map-address").value;
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
            locationContainer.querySelector("#js-map-address").value = geocoderData.formatted_address;

            for (var i = 0; i < geocoderData.address_components.length; i++) {
                var address = geocoderData.address_components[i];
                if (address.types[0] == "locality")
                    locationContainer.querySelector("#js-map-short").value = address.long_name;
            }
        }

        locationContainer.querySelector("#js-map-address").addEventListener("keypress", function (event) {
            if (event.keyCode == 13) {
                mapAddressChanged();
            }
        });

        locationContainer.querySelector("#js-map-address").addEventListener("blur", function () {
            mapAddressChanged();
        });

        function mapAddressChanged() {
            var mapAddress = locationContainer.querySelector("#js-map-address").value;
            geocodeAddress(mapAddress, function (results, status) {
                if (status == 'OK') {
                    fillAddress(results[0]);
                    placeMarkerAndPanTo(results[0].geometry.location);
                }
            });
        }
    }

    init();

    controller.$inject = ["$http", "authResource", "$scope", "$timeout", "eventsManagementConfig", "intranetUserService"];
    angular.module('umbraco').controller('EventsManagementController', controller);
})(angular);