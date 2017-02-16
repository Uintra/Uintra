(function () {
    var datePickerDirectiveFactory = function () {

        var defaultOptions = {
            pickDate: true,
            pickTime: false,
            useSeconds: true,
            format: "YYYY-MM-DD", //If left empty then the format is YYYY-MM-DD. (see momentjs.com for supported formats)
            offsetTime: "1" //"1" = true, "0" = false When enabled the time displayed will be offset with the server's timezone, this is useful for scenarios like scheduled publishing when an editor is in a different timezone than the hosted server
        }

        return {
            restrict: 'E',
            replace: true,
            transclude: true,
            template: '<div ng-transclude><div ng-include="\'views/propertyeditors/datepicker/datepicker.html\'" onload="datePicketLoaded()"></div></div>',
            scope: {
                value: "=ngModel",
                config: "=",
                placeholder: "@"
            },
            link: function ($scope, $element, $attrs) {
                var datePickerScope;

                $scope.model = {
                    value: $scope.value || null,
                    config: angular.extend(defaultOptions, $scope.config || {}),
                    validation: {
                        mandatory: 'required' in $attrs
                    }
                };

                $scope.datePicketLoaded = function () {
                    getDatePickerScope();

                    $scope.$watch(function () { return datePickerScope.datetimePickerValue; }, function (newVal) {
                        $scope.value = $scope.model.value;
                    });

                    if ($scope.placeholder) {
                        $element.find('.datepickerinput').attr('placeholder', $scope.placeholder);
                    }

                }

                var getDatePickerScope = function () {
                    datePickerScope = $element.find('[ng-controller]').scope();

                    if (datePickerScope == null) {
                        throw new Error("DatePickerDorective: Can't access umbraco datepicker scope!");
                    }
                }
            }
        }
    }

    datePickerDirectiveFactory.$inject = [];

    angular.module('umbraco').directive("datePicker", datePickerDirectiveFactory);
})();