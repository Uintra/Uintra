(function () {
    'use strict';

    var stopPropagationFactory = function () {
        return {
            restrict: 'A',
            link: function ($scope, $elem, $attrs) {
                $elem.on($attrs.stopPropagation, function (e) {
                    e.stopPropagation();
                });
            }
        }
    }

    var panelValidationFactory = function () {
        function link($scope, $elem, $attrs) {
            function validate() {
                var els = $elem.find('[required].ng-invalid');
                var control = $elem.closest('.umb-control');
                els.length ? control.addClass('_invalid') : control.removeClass('_invalid');
            }
            $scope.$watch(function () { return $scope.$eval($attrs.validatePanel) }, function () { setTimeout(validate) }, true);
            $scope.$on('formSubmitting', validate);
        }
        return { restrict: 'A', link: link }
    }

    var controlRequiredFactory = function () {
        var validator = '<input type="hidden" ng-model="validation.model" required />';
        var __defaultModelProperty = 'model';
        var ngInvalid = 'ng-invalid';
        function compile($elem) {
            $elem.append(validator);
            return {
                post: function ($scope, $elem, $attrs) {
                    var modelProp = $attrs.cRequired || __defaultModelProperty;
                    if (!(modelProp in $scope)) {
                        console.error("Try validate elem: ", $elem[0]);
                        throw new Error("Can't find model to validate");
                    }

                    $scope.validation = {
                        get model() {
                            var val = angular.copy($scope[modelProp]);
                            val == null || val.length == 0 && (val = null);

                            var hdnInpt = $elem.find('input[type="hidden"]')[0];
                            ;
                            if (hdnInpt.classList.contains(ngInvalid)) {
                                $elem[0].classList.add(ngInvalid);
                            } else {
                                $elem[0].classList.remove(ngInvalid);
                            }
                            return val;
                        },
                        set model(v) { }
                    }
                }
            }
        }
        return {
            restrict: 'A',
            compile: compile
        }
    }

    angular.module('umbraco').directive('stopPropagation', stopPropagationFactory);
    angular.module('umbraco').directive('validatePanel', panelValidationFactory);
    angular.module('umbraco').directive('cRequired', controlRequiredFactory);
})();