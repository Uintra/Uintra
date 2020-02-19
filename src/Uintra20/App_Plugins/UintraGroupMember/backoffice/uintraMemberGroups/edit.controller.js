var app = angular.module('umbraco');

app.controller('memberGroups.editController',
    function (
        memberGroupsService,
        $routeParams,
        notificationsService,
        $location,
        navigationService,
        $scope) {

        var vm = this;
        var inProgress = false;
        vm.memberGroup = null;
        var memberGroupId = $routeParams.id;
        vm.isButtonDisabled = true;
        var notification = {
            SUCCESS: 'Success',
            ERROR: 'Error',
            INVALID_GROUP_NAME: 'Invalid group name!',
            MEMBER_SAVED: 'Member group has been saved!',
            GROUP_EXIST: 'The group with the same name already exist!',
            PERMISSION: {
                ENABLED: 'Permission has been enabled!',
                DISABLED: 'Permission has been disabled!',
                ALLOWED: 'Permission has been allowed!',
                DISALLOWED: 'Permission has been disallowed!',
            }
        };

        control = {
            button: {
                state: {
                    INIT: 'init',
                    BUSY: 'busy',
                    SUCCESS: 'success'
                }
            }
        };

        if ($routeParams.create) {
            vm.memberGroup = {};
            vm.isCreate = true;
            vm.isButtonDisabled = false;
        } else {
            memberGroupsService
                .getPermissions(memberGroupId)
                .then(function (groupPermissionModel) {
                    let data = groupPermissionModel.data;
                    vm.memberGroup = data.memberGroup;
                    vm.permissions = data.permissions;
                    vm.isSuperUser = data.isSuperUser;

                    vm.groupedPermissions = groupByResourceTypeName(data.permissions);

                    var currentGroupName = data.memberGroup.name;
                    $scope.$watch(function (scope) { return vm.memberGroup.name; }, function (newValue, oldValue) {
                        vm.isButtonDisabled = currentGroupName === newValue || !newValue;
                    });

                    syncTree(memberGroupId);
                });
        }

        vm.property = {
            label: 'Name',
            description: 'Member group name'
        };
        vm.permissionsProperty = {
            label: '',
            description: 'Activity type name'
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

        vm.toggleEnabled = function (permission) {
            if (inProgress) return;
            inProgress = true;

            var request = angular.copy(permission);
            request.enabled = !permission.enabled;

            memberGroupsService
                .toggle(request)
                .then(function (response) {
                    permission.enabled = !permission.enabled;
                    if (permission.enabled)
                        notificationsService.success(notification.SUCCESS, notification.PERMISSION.ENABLED);
                    else
                        notificationsService.warning(notification.SUCCESS, notification.PERMISSION.DISABLED);
                    inProgress = false;
                });
        };

        vm.toggleAllowed = function (permission) {
            if (inProgress) return;

            inProgress = true;

            var request = angular.copy(permission);
            request.allowed = !permission.allowed;
            memberGroupsService
                .toggle(request)
                .then(function (groupPermissionModel) {
                    vm.permissions = groupPermissionModel.data.permissions;
                    vm.groupedPermissions = groupByResourceTypeName(groupPermissionModel.data.permissions);
                    if (!permission.allowed) {
                        notificationsService.success(notification.SUCCESS, notification.PERMISSION.ALLOWED);
                    } else {
                        notificationsService.warning(notification.SUCCESS, notification.PERMISSION.DISALLOWED);
                    }
                    inProgress = false;
                });
        };

        changeButtonState(control.button.state.INIT);


        vm.save = function () {
            changeButtonState(control.button.state.BUSY);
            if (vm.isCreate) {
                memberGroupsService
                    .create(vm.memberGroup.name)
                    .then(function (response) {
                        const createdMemberGroupId = response.data.memberGroup.id;
                        if (createdMemberGroupId > 0) {
                            syncTree(createdMemberGroupId);
                            $location.url('/' + $routeParams.section + '/' + $routeParams.tree + '/' + $routeParams.method + '/' + createdMemberGroupId);
                        } else {
                            notificationsService.error(notification.ERROR, notification.INVALID_GROUP_NAME);
                            changeButtonState(control.button.state.SUCCESS);
                        }
                    },
                    function (error) {
                        changeButtonState(control.button.state.SUCCESS);
                    });

                return;
            }
            memberGroupsService
                .save(memberGroupId, vm.memberGroup.name)
                .then(function (result) {
                    if (result.data === 'true') {
                        notificationsService.success(notification.SUCCESS, notification.MEMBER_SAVED);
                        syncTree(memberGroupId);
                    } else {
                        notificationsService.error(notification.ERROR, notification.GROUP_EXIST);
                    }
                    changeButtonState(control.button.state.SUCCESS);
                });
        };

        function groupByResourceTypeName(items) {
            var grouped = _.groupBy(items, function (item) {
                return item.resourceTypeName;
            });

            objectEntriesCheck();

            return Object.entries(grouped);
        }

        function syncTree(id) {
            navigationService.syncTree({
                tree: $routeParams.tree, path: ['-1', id.toString()],
                forceReload: true,
                activate: true
            });
        }

        function changeButtonState(state) {
            vm.buttonState = state;
        }

        //Used due to support IE11
        function objectEntriesCheck() {
            if (!Object.entries) {
                Object.entries = function (obj) {
                    var ownProps = Object.keys(obj),
                        i = ownProps.length,
                        resArray = new Array(i);
                    while (i--)
                        resArray[i] = [ownProps[i], obj[ownProps[i]]];

                    return resArray;
                };
            }
        }
    });