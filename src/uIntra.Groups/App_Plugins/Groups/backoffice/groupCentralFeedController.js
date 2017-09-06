(function () {

    var controller = function ($scope, groupCentralFeedService) {
        var self = this;
        self.availableActivityTypes = [];

        $scope.overlay = {
            show: false,
            view: "/App_Plugins/Groups/CentralFeed/backoffice/group-central-feed-overlay.html",
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

        $scope.init = function (control) {
            $scope.control = control;

            groupCentralFeedService.getActivityTypes().success(function (data) {
                self.availableActivityTypes = data;
            });
        }
    }
    controller.$inject = ["$scope", "groupCentralFeedService"];
    angular.module('umbraco').controller('groupCentralFeedController', controller);
})();