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
                }).always(function () {
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
                }).always(function () {
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
                        $location.url("/" + $routeParams.section + "/" + $routeParams.tree + "/" + $routeParams.method + "/" + createdMemberGroupId);
                    }).error(function (error) {
                        vm.buttonState = "success";
                    });
                return;
            }
            memberGroupsService.save(memberGroupId, vm.memberGroup.name)
                .success(function (response) {
                    notificationsService.success("Success", "Member group has been saved!");
                    syncTree(memberGroupId);
                }).always(function () {
                    vm.buttonState = "success";
                });
        };

        function groupByActivityTypeName(items) {
            return _.groupBy(items, function (item) {
                return item.activityTypeName;
            });
        }

        function syncTree(id) {
            navigationService.syncTree({
                tree: $routeParams.tree, path: ["-1", id.toString()],
                forceReload: true,
                activate: true
            });
        }
    });