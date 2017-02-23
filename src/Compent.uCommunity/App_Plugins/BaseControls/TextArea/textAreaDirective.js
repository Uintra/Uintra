(function () {

    function textAreaDirective() {
        var defaultConfig = {
            editor: {
                toolbar: ['bold italic underline paste'],
                dimensions: {
                    height: 200,
                    width: 500
                }
            }
        };

        return {
            scope: {
                value: "=ngModel",
                config: "="
            },
            restrict: 'E',
            replace: false,
            transclude: true,
            template: '<ng-form name="rteForm"><div ng-transclude><div ng-include="\'views/propertyeditors/rte/rte.html\'" onload="rteLoadedHandler()"></div></div></ng-form>',
            link: function ($scope, $element, attrs) {
                var rteScope;

                $scope.model = {
                    value: $scope.value,
                    config: angular.extend(defaultConfig, $scope.config || {})
                }

                $scope.rteLoadedHandler = function () {
                    getRteScope();
                    $scope.$watch(function () { return $scope.model.value; }, function () {
                        $scope.value = $scope.model.value;
                    });
                }

                var getRteScope = function () {
                    rteScope = $element.find('[ng-controller]').scope();
                    if (rteScope == null) {
                        throw new Error("RteDirective: Can't access umbraco rte scope!");
                    }
                }
            }
        };
    };

    textAreaDirective.$inject = [];
    angular.module("umbraco").directive('textArea', textAreaDirective);
})();