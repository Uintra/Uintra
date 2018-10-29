(function () {

    var controller = function ($scope, centralFeedService) {

        var self = this;
        self.availableActivityTypes = [];

        $scope.overlay = {
            show: false,
            view: "/App_Plugins/CentralFeed/backoffice/central-feed-overlay.html",
            title: "Central feed",
            close: function () {
                $scope.overlay.show = false;
                $scope.control.value = $scope.backupModel;
            },
            submit: function () {
                $scope.overlay.show = false;
            }
        }

        $scope.open = function () {
            $scope.overlay.show = true;
            $scope.backupModel = angular.copy($scope.control.value);
        }

        $scope.init = function (control, isCreatePanel) {
            $scope.control = control;

            centralFeedService.getActivityTypes().success(function (data) {
                if (isCreatePanel) {
                    data = data.filter(function (element) {
                        return element.Id !== 0;
                    });
                }
                self.availableActivityTypes = data;
            });
        }
    }
    controller.$inject = ["$scope", "centralFeedService"];
    angular.module('umbraco').controller('centralFeedController', controller);
})();