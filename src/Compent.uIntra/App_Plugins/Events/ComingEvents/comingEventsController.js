(function () {

    var controller = function ($scope) {

        var defaultEventsAmount = 5;
        $scope.overlay = {
            show: false,
            view: "/App_Plugins/Events/ComingEvents/coming-events-overlay.html",
            title: "Coming events",
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
            if (!$scope.control.value) {
                $scope.control.value = { eventsAmount: defaultEventsAmount };
            }
            $scope.control = control;
        }
    }
    controller.$inject = ["$scope"];
    angular.module('umbraco').controller('comingEventsController', controller);
})();