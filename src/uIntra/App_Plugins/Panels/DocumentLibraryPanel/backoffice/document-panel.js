(function () {

    var controller = function ($scope) {
        var defaultDocumentsCount = 5;

        $scope.overlay = {
            show: false,
            view: "/App_Plugins/Panels/DocumentLibraryPanel/backoffice/overlay.html",
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
            if (!$scope.control.value) {
                $scope.control.value = getDefaultModel();
            }
            $scope.control = control;
        }

        function getDefaultModel() {
            return {
                count: defaultDocumentsCount
            }
        }
    }
    controller.$inject = ["$scope"];
    angular.module('umbraco').controller('documentPanelController', controller);
})();