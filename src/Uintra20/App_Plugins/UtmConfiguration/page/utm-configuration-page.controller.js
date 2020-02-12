(function (angular) {
    'use strict';

    const controller = function ($scope, utmConfigurationService) {
        var vm = this;

        var utmConfigurationModel = {
            utmSource: '',
            parameters: []
        };

        $scope.model.value = $scope.model.value || utmConfigurationModel;

        vm.addParameter = function () {
            if (vm.utmParameter && vm.utmValue) {
                var model = {
                    parameterName: vm.utmParameter,
                    parameterValue: vm.utmValue
                };
                $scope.model.value.parameters.push(model);
            }
        };

        vm.removeParameter = function (index) {
            $scope.model.value.parameters.splice(index, 1);
        };

        init();

        function init() {
            vm.working = true;

            utmConfigurationService.getUtmParameters().then(function (response) {
                var parameters = response.data.filter(x => x.value !== "utm_source");
                vm.utmParameter = parameters[0].value;
                vm.utmParameters = parameters;
            }).finally(function () {
                vm.working = false;
            });
        }
    };

    controller.$inject = ["$scope", "utmConfigurationService"];

    angular.module("umbraco").controller("utmConfigurationController", controller);

})(angular);