(function () {
    var onError = function (error) { console.error(error); }

    var controller = function ($scope, $routeParams, contentResource, usersTagsService) {
        var self = this;

        $scope.overlay = {
            show: false,
            title: "Users tags",
            view: "/App_Plugins/UsersTags/backoffice/users-tags.overlay.html",
            close: function () {
                $scope.overlay.show = false;
            },
            submit: function () {
                $scope.overlay.show = false;
            }
        };

        self.open = function () {
            usersTagsService.getAll($routeParams.id).then(function (response) {
                $scope.control.value.usersTags = response.data;
                $scope.overlay.show = true;
            }, onError);
        }

        function init() {
            if (!$scope.control.value) {
                $scope.control.value = {
                    entityId: "",
                    usersTags: []
                };
            }

            if (!$scope.control.value.entityId) {
                contentResource.getById($routeParams.id).then(function (content) {
                    $scope.control.value.entityId = content.key;
                }, onError);
            }
        }

        init();
    };

    controller.$inject = ["$scope", "$routeParams", "contentResource", "usersTagsService"];
    angular.module('umbraco').controller('usersTagsController', controller);
})();