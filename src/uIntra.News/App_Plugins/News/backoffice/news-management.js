(function (angular) {
    'use strict';

    var onError = function (error) { console.error(error); }

    var controller = function ($http, authResource, $scope, newsManagementConfig, intranetUserService) {
        var self = this;
        self.newsList = [];
        self.currentUser = null;
        self.dateFormat = "dd/MM/yyyy";
        self.selected = null;
        self.selectedIndex = null;
        self.filterModel = {};
        self.users = [];

        $scope.$watch(function () { return (self.selected || {}).publishDate; }, function () {
            if (self.selected != null) {
                var date = new Date(self.selected.publishDate);
                if (self.unpublishDatePicker) {
                    self.unpublishDatePicker.set('minDate', date);
                } else {
                    self.config.unpublishDate.minDate = date;
                }
            }
        });

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
            if (self.selected != null) {
                self.clearSelected();
            }

            if (news == null) {
                var currentOwner = self.users.filter(function (user) { return user.umbracoId === self.currentUser.id })[0];
                news = { owner: currentOwner };
            } else {
                news.owner = self.users.filter(function (user) { return user.id === news.ownerId })[0];
            }

            self.selectedIndex = index;
            self.selected = angular.copy(news);
            self.selected.publishDate = self.selected.publishDate || new Date().toISOString();
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

            $http.delete('/Umbraco/backoffice/Api/NewsSection/Delete?id=' + news.id).then(function (response) {
                self.newsList.splice($index, 1);
                self.clearSelected();
            }, onError);
        }

        self.clearSelected = function () {
            self.selectedIndex = self.selected = self.unpublishDatePicker = null;
        }

        var create = function (news) {
            $http.post('/Umbraco/backoffice/Api/NewsSection/Create', news).then(function (response) {
                self.newsList.push(response.data);
                self.clearSelected();
            }, onError);
        }

        var save = function (news, index) {
            $http.post('/Umbraco/backoffice/Api/NewsSection/Save', news).then(function (response) {
                self.newsList.splice(index, 1, response.data);
                self.clearSelected();
            }, onError);
        }

        var loadAll = function () {
            var promise = $http.get('/Umbraco/backoffice/Api/NewsSection/GetAll');
            var success = function (response) {
                self.newsList = response.data || [];
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
            self.config = newsManagementConfig;
            self.config.unpublishDate = angular.copy(self.config.publishDate);
        }

        activate();
    }

    controller.$inject = ["$http", "authResource", "$scope", "newsManagementConfig", "intranetUserService"];
    angular.module('umbraco').controller('NewManagementController', controller);
})(angular);