(function () {

    var controller = function ($scope) {
        $scope.overlay = {
            show: false,
            view: "/App_Plugins/Panels/ContentPanel/backoffice/overlay.html",
            title: "Content panel",
            close: function() {
                $scope.overlay.show = false;
                $scope.control.value = $scope.backupModel;
            },
            submit: function () {
                $scope.overlay.show = false;
            }
        }

        $scope.open = function() {
            $scope.overlay.show = true;
            $scope.control.value.type = $scope.control.value.type || "vertical";
            $scope.backupModel = angular.copy($scope.control.value);
        }

        $scope.init = function(control) {
            $scope.control = control;
        }
    }
    controller.$inject = ["$scope"];
    angular.module('umbraco').controller('contentPanelController', controller);
})();