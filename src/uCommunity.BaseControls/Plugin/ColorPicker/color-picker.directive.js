(function () {
    var link = function ($scope, $element) {
        $scope.model = {
            value: $scope.value,
            config: $scope.config || {},
            validation: { mandatory: false }
        };

        $scope.onLoadHandler = function () {
            var elementScope = $element.find('[ng-controller]').scope();
            if (elementScope == null) {
                throw new Error("ColorPickerDirective: Can't access umbraco scope!");
            }

            $scope.$watch(function () { return $scope.model.value; }, function () {
                $scope.value = $scope.model.value;
            });
        };
    };

    var directiveFactory = function () {
        return {
            restrict: "E",
            replace: false,
            transclude: true,
            template: '<ng-form name="propertyForm"><div ng-transclude><div ng-include="\'views/propertyeditors/colorpicker/colorpicker.html\'" onload="onLoadHandler()"></div></div></ng-form>',
            scope: {
                value: "=ngModel",
                config: "="
            },
            link: link
        };
    };

    directiveFactory.$inject = [];
    angular.module('umbraco').directive('colorPicker', directiveFactory);
})();
