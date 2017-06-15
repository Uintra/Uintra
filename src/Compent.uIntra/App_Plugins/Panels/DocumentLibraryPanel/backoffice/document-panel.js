(function () {

    var controller = function ($scope) {
        $scope.overlay = {
            show: false,
            view: "/App_Plugins/DocumentLibraryPanel/backoffice/overlay.html",
            title: "Document panel",
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
        }
    }
    controller.$inject = ["$scope"];
    angular.module('umbraco').controller('documentPanelController', controller);
})();