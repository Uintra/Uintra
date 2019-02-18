var app = angular.module('umbraco');

app.controller('memberGroups.editController',
    function (memberGroupsService, $routeParams, notificationsService, $location, navigationService, $scope, $http, currentUserResource, usersResource, editorState, userService) {

        var vm = this;
        var inProgress = false;
        vm.memberGroup = null;
        var memberGroupId = $routeParams.id;

        if ($routeParams.create) {
            vm.memberGroup = {};
            vm.isCreate = true;
        } else {

            memberGroupsService.getPermissions(memberGroupId)
                .success(function (groupPermissionModel) {
                    vm.memberGroup = groupPermissionModel.memberGroup;
                    vm.permissions = groupByActivityTypeName(groupPermissionModel.permissions);
                    vm.isSuperUser = groupPermissionModel.isSuperUser;
                    syncTree(memberGroupId);
                });

            //$http.get('/umbraco/backoffice/api/MemberGroup/Get?id=' + memberGroupId).success(function (response) {
            //    vm.memberGroup = response;
            //});

            //$http.get('/umbraco/backoffice/api/MemberGroup/IsSuperUser')
            //    .success(function (response) {
            //        vm.isSuperUser = response;
            //        vm.permissions = [
            //            { actionId: 0, actionName: "read", activityTypeId: 100, activityTypeName: "Events", enabled: true, allowed: true },
            //            { actionId: 1, actionName: "create", activityTypeId: 101, activityTypeName: "Events", enabled: true, allowed: true },
            //            { actionId: 2, actionName: "delete", activityTypeId: 102, activityTypeName: "Events", enabled: false, allowed: true },
            //            { actionId: 3, actionName: "update", activityTypeId: 103, activityTypeName: "Events", enabled: true, allowed: true },
            //
            //            { actionId: 4, actionName: "read", activityTypeId: 110, activityTypeName: "News", enabled: true, allowed: true },
            //            { actionId: 5, actionName: "create", activityTypeId: 111, activityTypeName: "News", enabled: true, allowed: true },
            //            { actionId: 6, actionName: "delete", activityTypeId: 112, activityTypeName: "News", enabled: false, allowed: true },
            //            { actionId: 7, actionName: "update", activityTypeId: 113, activityTypeName: "News", enabled: false, allowed: true },
            //
            //            { actionId: 8, actionName: "read", activityTypeId: 120, activityTypeName: "Bulletins", enabled: true, allowed: true },
            //            { actionId: 9, actionName: "create", activityTypeId: 121, activityTypeName: "Bulletins", enabled: true, allowed: true },
            //            { actionId: 10, actionName: "delete", activityTypeId: 122, activityTypeName: "Bulletins", enabled: false, allowed: true },
            //            { actionId: 11, actionName: "update", activityTypeId: 123, activityTypeName: "Bulletins", enabled: true, allowed: true }
            //        ];
            //    });

        }

        function groupByActivityTypeName(items) {
            return _.groupBy(items, function (item) {
                return item.activityTypeName;
            });
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

        vm.toggleEnabled = function myfunction(permission) {
            if (inProgress) return;
            inProgress = true;

            var request = angular.copy(permission);
            request.enabled = !permission.enabled;

            memberGroupsService.toggle(request)
                .success(function (response) {
                    permission.enabled = !permission.enabled;
                    if (permission.enabled)
                        notificationsService.success("Success", "Permission has been enabled!");
                    else
                        notificationsService.warning("Success", "Permission has been disabled!");
                    inProgress = false;
                });
        };

        vm.toggleAllowed = function myfunction(permission) {
            if (inProgress) return;
            inProgress = true;

            var request = angular.copy(permission);
            request.allowed = !permission.allowed;

            memberGroupsService.toggle(request)
                .success(function (response) {
                    permission.allowed = !permission.allowed;
                    if (permission.allowed)
                        notificationsService.success("Success", "Permission has been allowed!");
                    else
                        notificationsService.warning("Success", "Permission has been disallowed!");
                    inProgress = false;
                });
        };

        vm.buttonState = "init";
        vm.save = function () {
            vm.buttonState = "busy";
            if (vm.isCreate) {
                memberGroupsService.create(vm.memberGroup.name)
                    .success(function (createdMemberGroupId) {
                        syncTree(createdMemberGroupId);
                        $location.url("/" + $routeParams.section + "/" + $routeParams.tree + "/" + $routeParams.method + "/" + response);
                    });
                return;
            }
            memberGroupsService.save(memberGroupId, vm.memberGroup.name)
                .success(function (response) {
                    vm.buttonState = "success";
                    notificationsService.success("Success", "Member group has been saved!");
                    syncTree(memberGroupId);
                });
        };

        function syncTree(id) {
            navigationService.syncTree({
                tree: $routeParams.tree, path: ["-1", id.toString()],
                forceReload: true,
                activate: true
            });
        }
    });