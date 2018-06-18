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

    controller.$inject = ["$http", "authResource", "$scope", "$timeout", "eventsManagementConfig", "intranetUserService"];
    angular.module('umbraco').controller('EventsManagementController', controller);
})(angular);