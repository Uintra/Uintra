(function (angular) {
    'use strict';

    var onError = function (error) { console.error(error); }

    var controller = function ($http, authResource, $scope, newsManagementConfig) {
        var self = this;
        self.newsList = [];
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
            if (self.filterModel.title) {
                checkList.push(compareText(item.title, self.filterModel.title));
            }
            if (self.filterModel.teaser) {
                checkList.push(compareText(item.teaser, self.filterModel.teaser));
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
                '_news-selected': self.selected != null
            };
        }

        self.selectNewsToEdit = function (news, index) {
            self.selectedIndex = index;
            self.selected = angular.copy(news);
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

        self.publish = function (news, $index) {
            self.clearSelected();
            var editedNews = angular.copy(news);
            editedNews.isHidden = false;
            save(editedNews, $index);
        }

        self.unpublish = function (news, $index) {
            self.clearSelected();
            var editedNews = angular.copy(news);
            editedNews.isHidden = true;
            save(editedNews, $index);
        }

        self.delete = function (news, $index) {
            self.clearSelected();
            if (!confirm('Are you sure?')) {
                return;
            }

            $http.delete('/Umbraco/backoffice/Api/NewsSection/Delete?id='+ news.id).then(function (response) {
                self.newsList.splice($index, 1);
                self.clearSelected();
            }, onError);
        }

        self.clearSelected = function () {
            self.selectedIndex = self.selected = null;
        }

        var create = function (news) {
            $http.post('/Umbraco/backoffice/Api/NewsSection/Create', news).then(function (response) {
                self.newsList.push(response.data);
                self.clearSelected();
            }, onError);
        }

        var save = function(news, index) {
            $http.post('/Umbraco/backoffice/Api/NewsSection/Save', news).then(function (response) {
                self.newsList.splice(index, 1, response.data);
                self.clearSelected();
            }, onError);
        }

        var loadAll = function () {
            var promise = $http.get('/Umbraco/backoffice/Api/NewsSection/GetAll');
            var success = function (response) {
                self.newsList = response.data || [];
                console.debug(self.newsList);
            }
            promise.then(success, onError);
        }

        var activate = function () {
            loadAll();
            authResource.getCurrentUser().then(function (data) {
                self.currentUser = data;
            });
            self.config = newsManagementConfig;
        }

        activate();
    }

    controller.$inject = ["$http", "authResource", "$scope", "newsManagementConfig"];
    angular.module('umbraco').controller('NewManagementController', controller);
})(angular);