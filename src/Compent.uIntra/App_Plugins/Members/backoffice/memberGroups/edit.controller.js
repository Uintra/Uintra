var app = angular.module('umbraco');
//app.requires.push('angular.filter');
//var app = angular.module('umbraco', ['angular.filter']);

app.filter('groupBy', function ($parse) {
    return _.memoize(function (items, field) {
        var getter = $parse(field);
        return _.groupBy(items, function (item) {
            return getter(item);
        });
    });
});

app.controller('memberGroups.editController',
    function ($scope, $routeParams, $http, notificationsService, $location, navigationService) {

        var vm = this;
        vm.memberGroup = null;
        var memberGroupId = $routeParams.id;

        if ($routeParams.create) {
            vm.memberGroup = {};
            vm.isCreate = true;
        } else {
            $http.get('/umbraco/backoffice/api/MemberGroup/Get?id=' + memberGroupId).success(function (response) {
                vm.memberGroup = response;
            });

            //TODO get from backend
            vm.permissions = [
                { actionId: 0, actionName: "read", activityTypeId: 100, activityTypeName: "Events", exists: true },
                { actionId: 1, actionName: "create", activityTypeId: 101, activityTypeName: "Events", exists: true },
                { actionId: 2, actionName: "delete", activityTypeId: 102, activityTypeName: "Events", exists: false },
                { actionId: 3, actionName: "update", activityTypeId: 103, activityTypeName: "Events", exists: true },

                { actionId: 4, actionName: "read", activityTypeId: 110, activityTypeName: "News", exists: true },
                { actionId: 5, actionName: "create", activityTypeId: 111, activityTypeName: "News", exists: true },
                { actionId: 6, actionName: "delete", activityTypeId: 112, activityTypeName: "News", exists: false },
                { actionId: 7, actionName: "update", activityTypeId: 113, activityTypeName: "News", exists: false },

                { actionId: 8, actionName: "read", activityTypeId: 120, activityTypeName: "Bulletin", exists: true },
                { actionId: 9, actionName: "create", activityTypeId: 121, activityTypeName: "Bulletin", exists: true },
                { actionId: 10, actionName: "delete", activityTypeId: 122, activityTypeName: "Bulletin", exists: false },
                { actionId: 11, actionName: "update", activityTypeId: 123, activityTypeName: "Bulletin", exists: true }
            ];
        }

        vm.property = {
            label: "Name",
            description: "Member group name"
        };
        vm.permissionsProperty = {
            label: "",
            description: "Activity type name"
        };

        vm.getProperty = function (activityTypeName) {
            vm.permissionsProperty.label = activityTypeName;
            return vm.permissionsProperty;
        };

        vm.toggle = function myfunction(permission) {
            //TODO backend support
            permission.exists = !permission.exists;
        };

        vm.buttonState = "init";
        vm.save = function () {
            vm.buttonState = "busy";
            if (vm.isCreate) {
                $http.post('/umbraco/backoffice/api/MemberGroup/Create', { name: vm.memberGroup.name })
                    .success(function (response) {
                        navigationService.syncTree({ tree: $routeParams.tree, path: ["-1", response.toString()], forceReload: true, activate: false });
                        $location.url("/" + $routeParams.section + "/" + $routeParams.tree + "/" + $routeParams.method + "/" + response);
                    });
                return;
            }
            $http.post('/umbraco/backoffice/api/MemberGroup/Save', { id: memberGroupId, name: vm.memberGroup.name })
                .success(function (response) {
                    vm.buttonState = "success";
                    navigationService.syncTree({ tree: $routeParams.tree, path: ["-1", memberGroupId.toString()], forceReload: true, activate: false });
                });
        };
    });