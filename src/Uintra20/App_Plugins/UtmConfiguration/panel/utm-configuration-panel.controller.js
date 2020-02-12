(function (angular) {
    'use strict';

    const controller = function ($scope, utmConfigurationService) {
        var vm = this;

        var utmConfigurationModel = {
            isVisible: true,
            parameters: []
        };
        $scope.model.value = $scope.model.value || utmConfigurationModel;

        vm.addParameter = function () {
            if (vm.utmParameter && vm.utmValue) {
                var model = {
                    parameterName: vm.utmParameter,
                    parameterValue: vm.utmValue,
                    isVisible: vm.utmVisible
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
                vm.utmParameter = response.data[0].value;
                vm.utmParameters = response.data;
            }).finally(function () {
                vm.working = false;
            });
        }
    };

    controller.$inject = ["$scope", "utmConfigurationService"];

    angular.module("umbraco").controller("utmConfigurationController", controller);

})(angular);