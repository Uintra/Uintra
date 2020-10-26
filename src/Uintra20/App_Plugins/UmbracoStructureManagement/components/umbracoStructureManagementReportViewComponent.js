(function () {
    'use strict';

    var umbracoStructureManagementReportView = {
        templateUrl: Umbraco.Sys.ServerVariables.application.applicationPath + 'App_Plugins/UmbracoStructureManagement/Components/umbracoStructureManagementReportView.html',
        bindings: {
            typeChanges: '<',
            dependingTypeChanges: '<'
        },
        controllerAs: 'vm',
        controller: umbracoStructureManagementReportViewController
    };

    function umbracoStructureManagementReportViewController($scope) {

        var vm = this;

        vm.showChange = showChange;
        vm.getTypeName = getTypeName;
        vm.getChangeName = getChangeName;
        vm.countChanges = countChanges;
        vm.getChangeClass = getChangeClass;
        vm.getTypes = getTypes;
        vm.onNeverShowAgainCheck = onNeverShowAgainCheck;
        vm.onChangeToApplySelect = onChangeToApplySelect;
        vm.applyChangeTypeToAll = applyChangeTypeToAll;
        vm.ignoreAllChanges = ignoreAllChanges;
        vm.isTypeVisible = isTypeVisible;
        vm.isChangeVisible = isChangeVisible;
        vm.countVisibleTypes = countVisibleTypes;

        vm.changeToApply = {
            local: 0,
            external: 1
        };

        vm.changeType = {
            create: "Create",
            update: "Update",
            delete: "Delete",
            noChange: "NoChange"
        };

        vm.changesAreIgnored = false;

        /////////

        function getChangeClass(change) {
            switch (change) {
                case 'Create':
                    return 'color-green';
                case 'Delete':
                    return 'color-red';
                case 'Update':
                    return 'color-orange';
                default:
                    return 'color-orange';
            }
        }

        function showChange(change) {
            return (change !== vm.changeType.noChange && change !== 'Removed');
        }

        function countVisibleTypes() {
            return vm.typeChanges.filter(function(type) { return isTypeVisible(type); }).length;
        }

        function countChanges() {
            var count = 0;
            angular.forEach(vm.typeChanges, function (type, typeKey) {
                angular.forEach(type.changes, function (change, changeKey) {
                    if (isChangeVisible(change)) {
                        count++;
                    }
                });
            });

            return count;
        }

        function getTypes(changeType) {
            var result = vm.typeChanges.map(x => {
                x.changes = x.changes.filter(y => {
                    return y.change === changeType;
                });

                return x;
            });

            return result;
        }

        function getTypeName(type) {
            if (!type.alias && type.changes[0].change === vm.changeType.delete && !type.changes[0].newValue)
                return type.changes[0].oldValue;

            return type.name ? `${type.name} (${type.alias})` : type.alias;
        }

        function getChangeName(name) {
            if (name === vm.changeType.delete)
                return "";

            return name;
        }

        function onNeverShowAgainCheck(item) {
            item.changeDisabled = item.neverShowAgain;

            if (item.neverShowAgain) {
                item.changeToApply = vm.changeToApply.local;

                var countIgnored = 0;
                angular.forEach(vm.typeChanges, function (type, typeKey) {
                    angular.forEach(type.changes, function (val, changeKey) {
                        if (val.neverShowAgain) {
                            countIgnored++;
                        }
                    });
                });

                if (countChanges(vm.typeChanges) === countIgnored) {
                    vm.changesAreIgnored = true;
                }
            } else {
                vm.changesAreIgnored = false;
            }
        }

        function onChangeToApplySelect(change) {
            if (change.key) {
                modifyDependingContentType(vm.typeChanges, change, change.key);
                modifyDependingContentType(vm.dependingTypeChanges, change, change.definitionKey);
            }
        }

        function modifyDependingContentType(typeChanges, change, key) {
            var dependingType = typeChanges.find(function (typeChange) {
                return typeChange.key === key || typeChange.newValue === key;
            });

            if (dependingType && dependingType.changes.length === 1) {
                var dependingChange = dependingType.changes[0];

                if (dependingChange.change === vm.changeType.create || (dependingChange.change === vm.changeType.update && dependingChange.path === "/")) {
                    var nowDisabled = change.changeToApply === vm.changeToApply.external;

                    dependingChange.changeToApply = nowDisabled ? vm.changeToApply.external : change.changeToApply;
                    dependingChange.changeDisabled = nowDisabled;
                    dependingChange.ignoreDisabled = nowDisabled;
                }
            }
        }

        function applyChangeTypeToAll(changeType) {
            angular.forEach(vm.typeChanges, function (type) {
                angular.forEach(type.changes, function (change) {
                    if (!change.changeDisabled) {
                        change.changeToApply = changeType;
                        onChangeToApplySelect(change);
                    }
                });
            });
        }

        function ignoreAllChanges() {
            angular.forEach(vm.typeChanges, function (type) {
                angular.forEach(type.changes, function (val) {
                    val.neverShowAgain = vm.changesAreIgnored;
                    onNeverShowAgainCheck(val);
                });
            });
        }

        function isTypeVisible(type) {
            return type.changes.length === type.changes.filter(function (change) { return isChangeVisible(change); }).length;
        }

        function isChangeVisible(change) {
            return !change.hidden;
        }
    }

    angular.module('umbraco')
        .component('umbracoStructureManagementReportView', umbracoStructureManagementReportView);
})();