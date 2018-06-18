(function () {
    var editControl = function () {
        var controller = function ($scope) {

            $scope.$watch(function () { return $scope.config.mode; }, function (newValue, oldValue) {
                if (newValue != oldValue) {
                    $scope.triggerChangeMode(newValue);
                }
            });

            $scope.save = function (isSaveButton) {
                if (!$scope.isActiveEditMode) {
                    return;
                }

                if (!isSaveButton) {
                    $scope.config.onCancel();
                    $scope.triggerChangeMode(ControlMode.view);
                    return;
                }

                var saveResult = $scope.config.onSave();
                if (saveResult != false) {
                    $scope.triggerChangeMode(ControlMode.view);
                }
            }

            $scope.cancel = function () {
                if (!$scope.isActiveEditMode) {
                    return;
                }

                var cancelResult = $scope.config.onCancel();
                if (cancelResult != false) {
                    $scope.triggerChangeMode(ControlMode.view);
                }
            }

            $scope.activateEditMode = function () {
                if (!$scope.isActiveViewMode) {
                    return;
                }

                $scope.triggerChangeMode(ControlMode.edit);
            }

            $scope.triggerChangeMode = function (mode) {
                $scope.config.mode = mode;
                updateControlState();
            }

            function updateControlState() {
                if ($scope.config) {
                    $scope.isActiveEditMode = $scope.config.mode == ControlMode.edit;
                    $scope.isActiveViewMode = $scope.config.mode == ControlMode.view;
                    $scope.isActiveDisadledMode = $scope.config.mode == ControlMode.disable;
                    $scope.isActiveCreateMode = $scope.config.mode == ControlMode.create;
                }
            }

            function initControl() {
                updateControlState();
            }

            initControl();
        }

        return {
            restrict: 'E',
            templateUrl: '/App_Plugins/BaseControls/EditControl/edit-control.html',
            scope: { config: "="},
            transclude: true,
            replace: true,
            controller: ['$scope', controller]
        };
    }

    angular.module('umbraco').directive('editControl', [editControl]);
})();