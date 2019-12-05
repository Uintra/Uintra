(function (angular) {
    'use strict';

    const controller = function ($scope, ubaselineMailchimpService) {
        var vm = this;

        vm.lists = [];
        vm.categories = [];
        vm.groups = [];

        if (!$scope.model.value) {
            $scope.model.value = [];
        }

        vm.listChanged = function (index) {
            var listId = $scope.model.value[index].listId;

            $scope.model.value[index].availableCategories = getList(listId).categories;
        };

        vm.categoryChanged = function (index) {
            var listId = $scope.model.value[index].listId;
            var categoryId = $scope.model.value[index].categoryId;

            $scope.model.value[index].availableGroups = getCategory(listId, categoryId).groups;
        };

        vm.toggleGroupSelection = function (index, groupId) {
            var groupIndex = $scope.model.value[index].groups.indexOf(groupId);

            if (groupIndex > - 1)
                $scope.model.value[index].groups.splice(groupIndex, 1);
            else
                $scope.model.value[index].groups.push(groupId);
        };

        vm.addNewList = function() {
            $scope.model.value.push(getDefaultValue());
        };

        init();

        function getList(listId) {
            return vm.lists.find(function (list) { return list.id === listId; });
        }

        function getCategory(listId, categoryId) {
            return getList(listId).categories.find(function (category) { return category.id === categoryId; });
        }

        function getDefaultValue() {
            return {
                listName: '',
                listId: '',
                categoryId: '',
                groups: [

                ]
            };
        }

        function init() {
            vm.working = true;

            ubaselineMailchimpService.getInfo().then(function (response) {
                vm.lists = response.data;

                if ($scope.model.value.length === 0) {
                    $scope.model.value = [getDefaultValue()];
                } else {
                    $scope.model.value.forEach(function(item, index) {
                        vm.listChanged(index);
                        vm.categoryChanged(index);
                    });
                }
            }).finally(function () {
                vm.working = false;
            });
        }
    };

    controller.$inject = ["$scope", "mailchimpListContainerService"];

    angular.module("umbraco").controller("mailchimpListContainerController", controller);

})(angular);