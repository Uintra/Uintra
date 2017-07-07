(function () {

    var mediaPickerDirectiveFactory = function () {
        var defaultConfig = {
            disableFolderSelect: "1",
            multiPicker: "0",
            onlyImages: "0",
            startNodeId: null
        };

        return {
            restrict: "E",
            replace: false,
            transclude: true,
            template: '<div ng-transclude><div ng-include="\'views/propertyeditors/mediapicker/mediapicker.html\'" onload="mediaPickerLoadedHandler()"></div></div>',
            scope: {
                value: "=ngModel",
                config: "="
            },
            link: function ($scope, $element, $attrs) {
                var mediaPickerScope;
                $scope.model = {
                    value: $scope.value,
                    config: angular.extend({}, defaultConfig, $scope.config || {})
                }

                $scope.mediaPickerLoadedHandler = function () {
                    getMediaPickerScope();

                    $scope.$watch(function () { return $scope.model.value; }, function () {
                        $scope.value = $scope.model.value;
                    });
                }

                var getMediaPickerScope = function () {
                    mediaPickerScope = $element.find('[ng-controller]').scope();
                    if (mediaPickerScope == null) {
                        throw new Error("MediaPickerDirective: Can't access umbraco media picker scope!");
                    }
                }
            }
        }
    }

    mediaPickerDirectiveFactory.$inject = [];
    angular.module('umbraco').directive('mediaPicker', mediaPickerDirectiveFactory);
})();