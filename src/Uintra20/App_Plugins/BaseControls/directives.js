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

    var focusFactory = function ($parse) {
        return function (scope, element, attr) {
            var fn = $parse(attr['cFocus']);
            element.bind('focus', function (event) {
                scope.$apply(function () {
                    fn(scope, { $event: event });
                });
            });
        }
    }

    var blurFactory = function ($parse) {
        return function (scope, element, attr) {
            var fn = $parse(attr['cBlur']);
            element.bind('blur', function (event) {
                scope.$apply(function () {
                    fn(scope, { $event: event });
                });
            });
        }
    }

    var clickOutsideFactory = function ($document, $parse) {
        return {
            restrict: 'A',
            link: function ($scope, elem, attr) {
                var classList = (attr.outsideIfNot !== undefined) ? attr.outsideIfNot.replace(', ', ',').split(',') : [],
                    fn = $parse(attr['cClickOutside']);

                // add the elements id so it is not counted in the click listening
                if (attr.id !== undefined) {
                    classList.push(attr.id);
                }

                // assign the document click handler to a variable so we can un-register it when the directive is destroyed
                $document.on('click', function (e) {
                    var i = 0,
                        element;

                    // if there is no click target, no point going on
                    if (!e || !e.target) {
                        return;
                    }

                    // loop through the available elements, looking for classes in the class list that might match and so will eat
                    for (element = e.target; element; element = element.parentNode) {
                        var id = element.id,
                            classNames = element.className,
                            l = classList.length;

                        // loop through the elements id's and classnames looking for exceptions
                        for (i = 0; i < l; i++) {
                            // check for id's or classes, but only if they exist in the first place
                            if ((id !== undefined && id.indexOf(classList[i]) > -1) || (classNames && classNames.indexOf(classList[i]) > -1)) {
                                // now let's exit out as it is an element that has been defined as being ignored for clicking outside
                                return;
                            }
                        }
                    }

                    // if we have got this far, then we are good to go with processing the command passed in via the click-outside attribute
                    return $scope.$apply(function () {
                        return fn($scope);
                    });
                });

                // when the scope is destroyed, clean up the documents click handler as we don't want it hanging around
                $scope.$on('$destroy', function () {
                    $document.off('click');
                });
            }
        };
    }

    angular.module('umbraco').directive('stopPropagation', stopPropagationFactory);
    angular.module('umbraco').directive('validatePanel', panelValidationFactory);
    angular.module('umbraco').directive('cRequired', controlRequiredFactory);
    angular.module('umbraco').directive('cFocus', ['$parse', focusFactory]);
    angular.module('umbraco').directive('cBlur', ['$parse', blurFactory]);
    angular.module('umbraco').directive('cClickOutside', ['$document', '$parse', clickOutsideFactory]);
})();