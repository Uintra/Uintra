(function () {
    var textControl = function () {
        var controller = function ($scope) {
            $scope.config.triggerRefresh = function () {
                $scope.config.triggerCopySavedData();
                $scope.editControlConfig.mode = $scope.config.mode;
            }

            $scope.config.triggerValidate = function () {
                if (!$scope.config.isValidationRequired) {
                    return true;
                }

                isTextLengthExceeded();
                $scope.includeValidation = true;
                if ($scope.config && $scope.config.inputType == InputType.text) {
                    $scope.isValidValue = angular.isDefined($scope.config.value) &&
                        $scope.config.value !== null &&
                        $scope.config.value.length &&
                        $scope.config.value.length <= $scope.config.maxLength;
                } else {
                    $scope.isValidValue = angular.isDefined($scope.config.value) &&
                        $scope.config.value !== null &&
                        $scope.config.value !== 0 &&
                        $scope.config.value !== '';
                }

                return $scope.isValidValue;
            }

            $scope.config.triggerCopySavedData = function () {
                $scope.savedValue = $scope.config.value;
            }

            $scope.config.triggerCheckControlChanged = function () {
                return $scope.savedValue !== $scope.config.value;
            }

            $scope.isEditableMode = function () {
                return $scope.editControlConfig.mode == ControlMode.edit || $scope.editControlConfig.mode == ControlMode.create;
            }

            $scope.isConfigValueValid = function () {
                return !$scope.includeValidation || $scope.isValidValue;
            }

            $scope.isTextRequiredButEmpty = function () {
                return $scope.config.isRequired && (!$scope.config.value || $scope.config.value.length === 0);
            }

            $scope.setValue = function () {
                if ($scope.editControlConfig.mode == ControlMode.create) {
                    $scope.config.onSave($scope.config.value);
                }
            }

            function isTextLengthExceeded() {
                if ($scope.config && $scope.config.inputType == InputType.text) {
                    $scope.isTextLengthNotValid = $scope.config.value && $scope.config.value.length > $scope.config.maxLength;
                }
            }

            function saveHandler() {
                if (!$scope.config.triggerValidate()) {
                    return false;
                }

                if ($scope.config.triggerCheckControlChanged()) {
                    $scope.config.onSave($scope.config.value);
                    $scope.config.triggerCopySavedData();
                }

                return true;
            }

            function cancelHandler() {
                $scope.config.value = $scope.savedValue;
                $scope.includeValidation = false;
                $scope.isValidValue = true;
            }

            function initControl() {
                $scope.includeValidation = false;

                $scope.editControlConfig = new EditControlModel();
                $scope.editControlConfig.mode = $scope.config.mode;
                $scope.editControlConfig.onSave = saveHandler;
                $scope.editControlConfig.onCancel = cancelHandler;

                $scope.config.triggerCopySavedData();
            }

            initControl();
        };

        return {
            restrict: 'E',
            templateUrl: '/App_Plugins/BaseControls/EditControl/TextControl/text-control.html',
            replace: true,
            scope: { config: "=" },
            controller: ['$scope', controller]
        };
    }

    angular.module('umbraco').directive('textControl', [textControl]);
})();