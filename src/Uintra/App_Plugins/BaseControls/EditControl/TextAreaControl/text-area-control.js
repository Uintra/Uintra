﻿(function () {
    var textAreaControl = function () {
        var controller = function ($scope) {
            $scope.config.triggerRefresh = function () {
                $scope.includeValidation = false;
                $scope.config.triggerCopySavedData();
                $scope.editControlConfig.mode = $scope.config.mode;
            }

            $scope.config.triggerValidate = function () {
                if (!$scope.config.isValidationRequired) {
                    return true;
                }

                $scope.includeValidation = true;

                $scope.isTextLengthNotValid = $scope.config.value && $scope.config.value.length > $scope.config.maxLength;
                $scope.isValidValue = angular.isDefined($scope.config.value)
                    && $scope.config.value !== null
                    && $scope.config.value !== ''
                    && $scope.config.value.length <= $scope.config.maxLength;

                return $scope.isValidValue;
            }

            $scope.config.triggerCopySavedData = function () {
                $scope.savedValue = $scope.config.value;
                updateDisplayValue();
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

            function updateDisplayValue() {
                $scope.config.displayValue = ($scope.config.value || "").replace(/\n/gi, "<br/>");
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
            templateUrl: '/App_Plugins/BaseControls/EditControl/TextAreaControl/text-area-control.html',
            replace: true,
            scope: { config: "=" },
            controller: ['$scope', controller]
        };
    }

    angular.module('umbraco').directive('textAreaControl', [textAreaControl]);
})();