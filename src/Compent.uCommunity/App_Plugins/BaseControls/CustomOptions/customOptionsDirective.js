(function () {
    'use strict';

    angular
        .module('umbraco')
        .directive('customOptions', customOptionsDirective);

    customOptionsDirective.$inject = [];

    function customOptionsDirective() {
        return {
            scope: {
                optionTypes: "=",
                optionValue: "="
            },
            restrict: 'E',
            templateUrl: '/App_Plugins/BaseControls/CustomOptions/customOptions.html',
            link: function (scope, element, attrs) {
                scope.selectedType = { optionValue: scope.optionValue };

                scope.selectType = function (type) {
                       scope.optionValue = type;
                }
            }
        };
    }

})();