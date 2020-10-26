(function () {

    var contentPickerDirectiveFactory = function () {
        var defaultConfig = {
            startNode: {
                query: "",
                type: "media"                
            },
            filter: 'folder',
            minNumber: 0
        };

        return {
            restrict: "E",
            replace: false,
            transclude: true,
            template: '<div ng-transclude><div ng-include="\'views/propertyeditors/contentpicker/contentpicker.html\'" onload="contentPickerLoadedHandler()"></div></div>',
            scope: {
                value: "=ngModel",
                config: "="
            },
            link: function ($scope, $element, $attrs) {
                var contentPickerScope;
                $scope.model = {
                    value: $scope.value,
                    config: angular.extend(defaultConfig, $scope.config || {})
                }

                $scope.contentPickerLoadedHandler = function () {
                    getContentPickerScope();

                    $scope.$watch(function () { return $scope.model.value; }, function () {
                        $scope.value = $scope.model.value;
                    });
                }

                var getContentPickerScope = function () {
                    contentPickerScope = $element.find('[ng-controller]').scope();
                    if (contentPickerScope == null) {
                        throw new Error("ContentPicker: Can't access umbraco media picker scope!");
                    }
                }
            }
        }
    }

    contentPickerDirectiveFactory.$inject = [];
    angular.module('umbraco').directive('contentPicker', contentPickerDirectiveFactory);
})();