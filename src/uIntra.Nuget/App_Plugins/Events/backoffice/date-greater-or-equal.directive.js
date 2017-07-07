(function () {
    var dateGreaterOrEqualDirectiveFactory = function () {

        return {
            require: 'ngModel',
            link: function ($scope, $element, $attrs, ctrl) {

                var validate = function (viewValue) {
                    var comparisonValue = $attrs.dateGreaterOrEqual;

                    if (!viewValue || !comparisonValue) {
                        // It's valid because we have nothing to compare
                        ctrl.$setValidity('dateGreaterOrEqual', true);
                    }

                    // It's valid if model is greater than or equal to the model we're comparing
                    ctrl.$setValidity('dateGreaterOrEqual', new Date(comparisonValue) <= new Date(viewValue));
                    return viewValue;
                };

                ctrl.$parsers.unshift(validate);
                ctrl.$formatters.push(validate);

                $attrs.$observe('dateGreaterOrEqual', function () {
                    // Whenever the comparison model changes we'll re-validate
                    return validate(ctrl.$viewValue);
                });
            }
        }
    }

    dateGreaterOrEqualDirectiveFactory.$inject = [];

    angular.module('umbraco').directive("dateGreaterOrEqual", dateGreaterOrEqualDirectiveFactory);
})();