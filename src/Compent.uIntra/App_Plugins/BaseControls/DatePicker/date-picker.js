(function () {
    'use strict';
    //https://chmln.github.io/flatpickr/options/

    //WARNING: DO NOT USE 'APPEND TO' OPTION
    var defaultOptions = {
        weekNumbers: true,
        allowInput: false, // allowInput + dateFormat cause bug
        clickOpens: true,
        mode: "single" //"single", "multiple", or "range"
    }

    function isSingleMode(mode) { return mode == "single"; }

    function factory($timeout) {
        function wrapWithMagic(cb) { return function (o) { $timeout(function () { cb(o) }) } };

        function getSetter($scope, config) {
            return function (dates) {
                var value = dates && dates.length ? (isSingleMode(config.mode) ? dates[0] : dates) : null;
                $scope.$apply(function () {
                    $scope.model = value ? value.toIsoDateTimeString() : null;

                    if ($scope.change) {
                        $scope.change();
                    }
                });
            }
        }

        function createPicker($scope, $elem) {
            var config = angular.extend({}, defaultOptions, $scope.config);
            config.onChange = wrapWithMagic(getSetter($scope, config));

            var flatpickr = new Flatpickr($elem.find('.input-holder input')[0], config);
            $scope.$on('$destroy', wrapWithMagic(function () { flatpickr.destroy(flatpickr) }));
            return flatpickr;
        }

        function link($scope, $elem, $attrs) {
            var picker = createPicker($scope, $elem);
            picker.$$id = $scope.$id;
            "picker" in $attrs && ($scope.picker = picker); // allow direct access to flatpickr instance from parent scope

            $scope.toggle = wrapWithMagic(picker.toggle);
            $scope.clear = wrapWithMagic(picker.clear);

            if ($scope.model) {
                $timeout(function () {
                    picker.setDate($scope.model, false);
                });
            }
        }

        return {
            restrict: "E",
            templateUrl: "/App_Plugins/BaseControls/DatePicker/date-picker.html",
            scope: { model: '=', config: '=', picker: '=', placeholder: '@', change: '&' },
            link: link
        }
    }
    factory.$inject = ['$timeout'];
    angular.module('umbraco').directive('datePicker', factory);
})();