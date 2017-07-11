(function () {

    var controller = function ($scope) {
        $scope.overlay = {
            show: false,
            view: "/App_Plugins/Panels/ContentPanel/backoffice/overlay.html",
            title: "Content panel",
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
            $scope.control.value = $scope.control.value || getDefaultModel();
            $scope.backupModel = angular.copy($scope.control.value);
        }

        $scope.init = function (control) {
            $scope.control = control;
        }

        function getDefaultModel() {
            return {
                type: "vertical"
            };
        }
    }
    controller.$inject = ["$scope"];
    angular.module('umbraco').controller('contentPanelController', controller);
})();