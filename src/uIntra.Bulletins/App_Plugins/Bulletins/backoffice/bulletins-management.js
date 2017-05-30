(function (angular) {
    'use strict';

    var onError = function (error) { console.error(error); }

    var controller = function ($http, authResource, $scope, bulletinsManagementConfig) {
        var self = this;
        self.bulletinsList = [];
        self.currentUser = null;
        self.dateFormat = "dd MMMM yyyy";
        self.selected = null;
        self.selectedIndex = null;
        self.filterModel = {};

        self.filter = function (item) {
            var checkList = [];

            var compareText = function (left, right) {
                return left.toLowerCase().indexOf(right.toLowerCase()) > -1;
            }

            var compareDates = function (itemDate, filter) {
                if (filter) {
                    if (filter.from) {
                        checkList.push(itemDate >= filter.from);
                    }
                    if (filter.to) {
                        checkList.push(itemDate <= filter.to);
                    }
                }
            }

            if (self.filterModel.id) {
                checkList.push(compareText(item.id, self.filterModel.id));
            }
            if (self.filterModel.description) {
                checkList.push(compareText(item.description, self.filterModel.description));
            }
            if (self.filterModel.publishDate) {
                compareDates(item.publishDate, self.filterModel.publishDate);
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
                '_bulletins-selected': self.selected != null
            };
        }

        self.selectBulletinsToEdit = function (bulletin, index) {
            self.selectedIndex = index;
            self.selected = angular.copy(bulletin);
            self.selected.publishDate = self.selected.publishDate || new Date().toISOString();
            self.selected.umbracoCreatorId = self.selected.umbracoCreatorId || self.currentUser.id;
        }

        self.save = function () {
            if ($scope.editForm.$invalid) {
                $scope.editForm.$setDirty();
                return;
            }

            if (self.selected.id == null) {
                create(self.selected);
            } else {
                save(self.selected, self.selectedIndex);
            }
        }

        self.publish = function (bulletin, $index) {
            self.clearSelected();
            var editedBulletin = angular.copy(bulletin);
            editedBulletin.isHidden = false;
            save(editedBulletin, $index);
        }

        self.unpublish = function (bulletin, $index) {
            self.clearSelected();
            var editedBulletin = angular.copy(bulletin);
            editedBulletin.isHidden = true;
            save(editedBulletin, $index);
        }

        self.delete = function (bulletins, $index) {
            self.clearSelected();
            if (!confirm('Are you sure?')) {
                return;
            }

            $http.delete('/Umbraco/backoffice/Api/BulletinsSection/Delete?id=' + bulletins.id).then(function (response) {
                self.bulletinsList.splice($index, 1);
                self.clearSelected();
            }, onError);
        }

        self.clearSelected = function () {
            self.selectedIndex = self.selected = null;
        }

        var create = function (bulletin) {
            $http.post('/Umbraco/backoffice/Api/BulletinsSection/Create', bulletin).then(function (response) {
                self.bulletinsList.push(response.data);
                self.clearSelected();
            }, onError);
        }

        var save = function (bulletins, index) {
            $http.post('/Umbraco/backoffice/Api/BulletinsSection/Save', bulletins).then(function (response) {
                self.bulletinsList.splice(index, 1, response.data);
                self.clearSelected();
            }, onError);
        }

        var loadAll = function () {
            var promise = $http.get('/Umbraco/backoffice/Api/BulletinsSection/GetAll');
            var success = function (response) {
                self.bulletinsList = response.data || [];
            }
            promise.then(success, onError);
        }

        var activate = function () {
            loadAll();
            authResource.getCurrentUser().then(function (data) {
                self.currentUser = data;
            });
            self.config = bulletinsManagementConfig;
        }

        activate();
    }

    controller.$inject = ["$http", "authResource", "$scope", "bulletinsManagementConfig"];
    angular.module('umbraco').controller('BulletinsManagementController', controller);
})(angular);