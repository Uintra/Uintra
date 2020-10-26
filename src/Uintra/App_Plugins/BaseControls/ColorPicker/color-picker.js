(function (angular) {
    // https://bgrins.github.io/spectrum/
    var defaults = {
        color: '#000000',
        disabled: false,
        showInitial: false,
        preferredFormat: 'hex',
        showInput: true,
        clickoutFiresChange: true,
        change: function ($scope, color) {
            $scope.value = color.toHexString().toUpperCase();
        }
    };

    var preloadAssets = function ($scope, $q, assetsService) {
        var queue = [];
        if (typeof (tinycolor) === 'undefined') {
            queue.push(assetsService.loadJs('/umbraco/lib/spectrum/spectrum.js', $scope));
        }
        queue.push(assetsService.loadCss('/umbraco/lib/spectrum/spectrum.css'));

        return $q.all(queue);
    };

    var factory = function ($q, assetsService) {
        var link = function ($scope, $element) {
            var buildConfig = function () {
                var config = angular.extend({}, defaults, $scope.config);
                angular.forEach(config, function (value, key) {
                    if (!angular.isFunction(value)) return;
                    config[key] = function () {
                        var params = [].slice.call(arguments);
                        params.unshift($scope);
                        var result = value.apply(null, params);
                        $scope.$root.$$phase !== '$apply' && $scope.$root.$$phase !== '$digest' && $scope.$apply();
                        return result;
                    };
                });
                return config;
            };

            var createPicker = function (config) {
                $scope.value = $scope.value || config.color;
                return $element.find('input').spectrum(config);
            };

            var subscribe = function ($input) {
                var modelListener = $scope.$watch('value', function (value) {
                    $input.spectrum('set', value);
                });

                $scope.$on('$destroy', function () {
                    modelListener();
                    $input.spectrum('destroy');
                });
            };

            preloadAssets($scope, $q, assetsService)
                .then(buildConfig)
                .then(createPicker)
                .then(subscribe);
        };

        return {
            restrict: 'E',
            template: '<input type="hidden" />',
            scope: {
                value: '=ngModel',
                config: '='
            },
            link: link
        };
    };

    factory.$inject = ['$q', 'assetsService'];
    angular.module('umbraco').directive('colorPicker', factory);
})(angular);
