(function () {
    alert()
    var controller = function ($scope, editorConfigService) {
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

        $scope.init = function (control) { debugger
            $scope.control = control;
            editorConfigService
                .getConfig(control.editor.alias, control.editor.config)
                .then(function (config) {
                    $scope.linksPickerConfig = config
                });
        };
    }

    function getDefaultModel() {
        return {
            type: "vertical",
        };
    }

    controller.$inject = ["$scope", "editorConfigService"];
    angular.module('umbraco').controller('contentPanelController', controller);
})();