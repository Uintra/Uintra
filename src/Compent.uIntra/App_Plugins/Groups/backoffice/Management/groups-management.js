(function () {

    var controller = function ($http, notificationsService, interpolate) {
        var self = this;
        self.showHidden = false;
        self.directions = {
            asc: 1,
            desc: 2,
            '1': "asc",
            '2': "desc"
        }
        self.sortFields = {
            title: "title",
            createDate: "createDate",
            creator: "creator",
            updateDate: "updateDate"
        }

        self.sortField = self.sortFields.title;
        self.direction = self.directions.asc;

        self.sort = function (field) {
            self.direction = self.direction == self.directions.asc ? self.directions.desc : self.directions.asc;
            self.sortField = field;
            loadGroups();
        }

        self.getSortClass = function (field) {
            if (field == self.sortField) {
                return "sortable _" + self.directions[self.direction];
            }
            return "sortable";
        }

        self.showHiddenChanged = function () {
            loadGroups();
        }

        self.hide = function (group) {
            openConfirm(function () {
                hide(group.id, true);
            });
        }

        self.show = function (group) {
            openConfirm(function () {
                hide(group.id, false);
            });
        }

        function openConfirm(onSuccess) {
            self.confirmOverlay = {
                show: true,
                title: "Are you sure?",
                submitButtonLabel: "Yes",
                closeButtonLabel: "No",
                submit: function (model) {
                    self.confirmOverlay.show = false;
                    self.confirmOverlay = null;
                    onSuccess && onSuccess();
                },
                close: function (oldModel) {
                    self.confirmOverlay.show = false;
                    self.confirmOverlay = null;
                }
            }
        }

        function hide(groupId, hide) {
            $http.post("/Umbraco/backoffice/Api/GroupsSection/Hide?groupId=" + groupId + "&hide=" + hide).then(loadGroups);
        }

        function loadGroups() {
            function success(response) {
                self.itemsList = response.data;
            }
            function fail(response) { console.error(response); }
            var params = {
                showHidden: self.showHidden + "",
                field: self.sortField,
                direction: self.direction
            }
            var urlPattern = "/Umbraco/backoffice/Api/GroupsSection/GetAll?showHidden={showHidden}&field={field}&direction={direction}";
            $http.get(interpolate(urlPattern, params)).then(success, fail);
        }



        loadGroups();
    }
    controller.$inject = ["$http", "notificationsService", "interpolateFilter"];
    angular.module('umbraco').controller('groupsController', controller);
})();