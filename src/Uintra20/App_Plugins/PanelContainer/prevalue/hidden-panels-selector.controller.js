function hiddenPanelsSelectorController($scope, panelContainerResource)
{
    var vm = this;
    vm.loading = true;

    if (!$scope.model.value) {
        $scope.model.value = [];
    }

    vm.isChecked = function (alias) {
        return vm.getIndex(alias) !== -1;
    };

    vm.getIndex = function (alias) {
        return $scope.model.value.indexOf(alias);
    };

    vm.toggleCheck = function(alias) {
        const index = vm.getIndex(alias);

        if (index === -1)
            $scope.model.value.push(alias);
        else
            $scope.model.value.splice(index, 1);
    };

    function init() {

        panelContainerResource.getAllPanelTypes()
            .then(panels => {
                vm.allPanels = panels;
            })
            .finally(() => vm.loading = false);
    }

    init();
}
angular.module("umbraco").controller("UBaseline.HiddenPanelsSelectorController", hiddenPanelsSelectorController);