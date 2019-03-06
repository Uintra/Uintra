var app = angular.module('umbraco');

app.controller('memberGroups.editController',
    function (memberGroupsService, $routeParams, notificationsService, $location, navigationService, $scope, $http, currentUserResource, usersResource, editorState, userService) {

        var vm = this;
        var inProgress = false;
        vm.memberGroup = null;
        var memberGroupId = $routeParams.id;
        vm.isButtonDisabled = true;

        if ($routeParams.create) {
            vm.memberGroup = {};
            vm.isCreate = true;
            vm.isButtonDisabled = false;
        } else {
            memberGroupsService.getPermissions(memberGroupId)
                .success(function (groupPermissionModel) {
                    vm.memberGroup = groupPermissionModel.memberGroup;
                    vm.permissions = groupPermissionModel.permissions;
                    vm.isSuperUser = groupPermissionModel.isSuperUser;

                    vm.groupedPermissions = groupByResourceTypeName(groupPermissionModel.permissions);

                    var currentGroupName = groupPermissionModel.memberGroup.name;
                    $scope.$watch(function (scope) { return vm.memberGroup.name; }, function (newValue, oldValue) {
                        vm.isButtonDisabled = currentGroupName === newValue || !newValue;
                    });

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

        vm.isParentDisabled = function (permission) {
            if (permission.parentActionId === null)
                return false;
            var parent = vm.permissions.find(function (item) {
                return item.actionId === permission.parentActionId && item.resourceTypeId === permission.resourceTypeId;
            });
            return !parent.allowed;
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
                .success(function (groupPermissionModel) {
                    vm.permissions = groupPermissionModel.permissions;
                    vm.groupedPermissions = groupByResourceTypeName(groupPermissionModel.permissions);
                    if (!permission.allowed) {
                        notificationsService.success("Success", "Permission has been allowed!");
                    }
                    else {
                        notificationsService.warning("Success", "Permission has been disallowed!");
                    }
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
                        if (createdMemberGroupId > 0) {
                            syncTree(createdMemberGroupId);
                            $location.url("/" + $routeParams.section + "/" + $routeParams.tree + "/" + $routeParams.method + "/" + createdMemberGroupId);
                        } else {
                            notificationsService.error("Error", "Invalid group name!");
                            vm.buttonState = "success";
                        }
                    }).error(function (error) {
                        vm.buttonState = "success";
                    });
                return;
            }
            memberGroupsService.save(memberGroupId, vm.memberGroup.name)
                .success(function (result) {
                    if (result === 'true') {
                        notificationsService.success("Success", "Member group has been saved!");
                        syncTree(memberGroupId);
                    } else {
                        notificationsService.error("Error", "The group with the same name already exist!");
                    }
                }).always(function () {
                    vm.buttonState = "success";
                });
        };

        function groupByResourceTypeName(items) {
            var grouped = _.groupBy(items, function (item) {
                return item.resourceTypeName;
            });
            return Object.entries(grouped); // used for sorting from backend
        }

        function syncTree(id) {
            navigationService.syncTree({
                tree: $routeParams.tree, path: ["-1", id.toString()],
                forceReload: true,
                activate: true
            });
        }
    });