(function () {
    var richTextEditor = function () {
        var controller = function ($scope) {

            $scope.config.triggerRefresh = function () {
                $scope.includeValidation = false;
                $scope.config.triggerCopySavedData();
                $scope.editControlConfig.mode = $scope.config.mode;
            }

            $scope.config.triggerValidate = function () {
                if ($scope.config.value) {
                    $scope.includeValidation = true;
                    $scope.isValidValue = $scope.config.value.length <= $scope.config.maxLength;
                    $scope.maxDescriptionLength = $scope.config.maxLength - $scope.config.value.length; /*+ ' ' + $filter('translate')('characters left')*/;
                    return $scope.isValidValue;
                } else {
                    return true;
                }
            }

            $scope.config.triggerCopySavedData = function () {
                $scope.savedValue = $scope.config.value;
            }

            $scope.config.triggerCheckControlChanged = function () {
                return $scope.savedValue !== $scope.config.value;
            }

            $scope.isEditableMode = function () {
                return $scope.editControlConfig.mode == ControlMode.edit;
            }

            $scope.summernoteOptions = {
                height: 100,
                toolbar: [
                  ['style', ['bold', 'italic']],
                  ['para', ['ul', 'ol']],
                  ['height', ['height']]
                ],
                disableDragAndDrop: true
            };

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
            templateUrl: '/App_Plugins/BaseControls/EditControl/RichTextEditorControl/rich-text-editor.html',
            replace: true,
            scope: { config: "=" },
            controller: ['$scope', controller]
        };
    }

    angular.module('umbraco').directive('richTextEditor', [richTextEditor]);
})();