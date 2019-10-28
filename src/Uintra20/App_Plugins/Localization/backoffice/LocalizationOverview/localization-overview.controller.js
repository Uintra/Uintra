(function (angular) {
    'use strict';

    var controller = function ($timeout, notificationsService, editorService, assetsService,
        userService, localizationResourceService) {
        var self = this;

        // Don't move it to package.manifest
        assetsService.load([
            "/App_Plugins/Localization/backoffice/Shared/polyfills/findIndex.polyfill.js",
            "/App_Plugins/Localization/backoffice/Shared/polyfills/find.polyfill.js"
        ]).then(function () { });

        var defaultDateTime = "";
        var defaultResource = {};

        self.title = 'Localization';

        self.filter = {
            text: "",
            showActive: false,
            showAll: false,
            itemsCount: 0
        };

        self.resources = [];
        self.languages = [];
        self.listSettings = {
            isShowUpdateDate: false,
            isShowDescription: false
        };
        self.workspaceDisabled = true;

        self.permissions = {
            isCanDelete: false
        };

        self.activate = function (resource) {
            resource.isActive = !resource.isActive;
        }

        self.edit = function (resource, $event) {
            if (resource.isActive) {
                $event.stopPropagation();
            }

            //after clone resource.disabled = true;
            var dialogSettings = getDialogSettings(angular.copy(resource));
            editorService.open(dialogSettings);
        }

        self.delete = function (resource, $event) {
            if (resource.isActive) {
                $event.stopPropagation();
            }

            if (!confirm("Are you sure?")) return;

            resource.disabled = true;
            localizationResourceService.deleteResource(resource.key).then(function () {
                var index = self.resources.indexOf(resource);
                self.resources.splice(index, 1);

                notificationsService.success("Success", "Resource " + resource.key + " was deleted successfully");
            }, function () {
                errorHandler(resource.key, "was not deleted");
            });
        }

        self.create = function () {
            var dialogSettings = getDialogSettings(angular.copy(defaultResource));
            editorService.open(dialogSettings);
        }

        self.refresh = function () {
            self.workspaceDisabled = true;
            self.filter = {
                showActive: false,
                text: ""
            };

            init();
        }

        var _timeout;
        self.handleFilter = function () {
            if (_timeout) {
                $timeout.cancel(_timeout);
            }
            _timeout = $timeout(function () {
                const resources = self.allResources.filter(function (resource) {
                    return self.filterResources(resource) && self.filterActive(resource);
                });

                self.filter.itemsCount = resources.length;
                self.resources = limitResources(resources);

                _timeout = null;
            }, 350);
        }

        function limitResources(resources) {
            return self.filter.showAll ? resources : resources.slice(0, self.resourcesLimit);
        }

        self.filterResources = function (resource) {
            var filterValue = self.filter.text.toLowerCase();

            function isKeyContains() {
                var result = resource.key.toLowerCase().indexOf(filterValue) >= 0;
                return result;
            }

            function isTranslationsContains() {
                var result = false;

                for (var transaction in resource.translations) {
                    if (resource.translations.hasOwnProperty(transaction) && resource.translations[transaction].toLowerCase().indexOf(filterValue) >= 0) {
                        result = true;
                        break;
                    }
                }

                return result;
            }

            function isDescriptionContains() {
                var result = resource.description && resource.description.toLowerCase().indexOf(filterValue) >= 0;
                return result;
            }

            function isParentContains() {
                var result = resource.parentKey && resource.parentKey.toLowerCase().indexOf(filterValue) >= 0;
                return result;
            }

            if (!self.filter.text.trim()) {
                return true;
            }

            var result = isKeyContains() || isTranslationsContains() || isDescriptionContains() || isParentContains();
            return result;
        }

        self.filterActive = function (resource) {
            var result = !self.filter.showActive || resource.isActive;
            return result;
        }

        function saveResource(resource) {
            localizationResourceService.saveResource(resource.oldKey, resource).then(function (response) {
                var index = self.resources.findIndex(function (sResource) {
                    return sResource.key === resource.oldKey;
                });

                var oldResource = self.resources[index];
                response.data.isActive = oldResource.isActive;
                self.resources[index] = response.data;

                notificationsService.success("Success", "Resource " + resource.key + " was saved successfully");
                closeEditor();
            }, function () {
                errorHandler(resource.key, "was not saved");
            });
        }

        function createResource(resource) {
            localizationResourceService.createResource(resource).then(function (response) {
                var createdResource = response.data;
                createdResource.isNew = true;
                self.resources.unshift(createdResource);
                notificationsService.success("Success", "Resource " + resource.key + " was created successfully");
                closeEditor();
            }, function () {
                errorHandler(resource.key, "was not created");
            });
        }

        function errorHandler(resourceKey, message) {
            notificationsService.error("Resource - " + resourceKey, message);

            var resource = self.resources.find(function (sResource) {
                return sResource.key === resourceKey;
            });

            if (resource) {
                resource.disabled = false;
            }
        }

        function getDialogSettings(initialResource) {

            function dialogSubmittedCallback(resource) {
                resource.disabled = true;

                if (resource.isDefault) {
                    createResource(resource);
                } else {
                    saveResource(resource);
                }
            }

            function checkKey(resource) {
                if (resource.key.length === 0) {
                    resource.invalid = false;
                    return;
                }

                var storedResource = self.resources.find(function (sResource) {
                    return sResource.key.toLowerCase() === resource.key.toLowerCase();
                });

                resource.invalid = typeof storedResource !== "undefined";
            }

            function getStoredKeys() {
                var result = [];
                self.resources.forEach(function (resource) {
                    if (resource.key != initialResource.key) {
                        var storedKey = resource.key;
                        result.push(storedKey);
                    }
                });

                return result;
            }

            var result = {
                view: "/App_Plugins/Localization/backoffice/LocalizationOverview/localization-creation.dialog.html",
                size: "small",
                title: 'Resource creation',
                position: 'right',
                submit: dialogSubmittedCallback,
                close: function () {
                    closeEditor();
                },
                dialogData: {
                    languages: self.languages,
                    storedKeys: getStoredKeys(),
                    isShowDescription: self.listSettings.isShowDescription,
                    resource: initialResource,
                    checkKey: checkKey,
                    chooseParent: function (key, resource) {
                        resource.parentKey = key;
                    }
                }
            };

            return result;
        }

        function closeEditor() {
            editorService.close();
        }

        function init() {
            localizationResourceService.getLocalizationOverview().then(function (response) {
                self.resourcesLimit = response.data.resourcesLimit;
                self.allResources = _.clone(response.data.resources);
                self.filter.itemsCount = self.allResources.length;
                self.resources = limitResources(self.allResources);

                defaultResource = response.data.defaultResourceModel;
                defaultResource.isDefault = true;

                self.languages = response.data.languages;

                defaultDateTime = response.data.defaultDateTime;
                self.listSettings.isShowUpdateDate = self.resources.some(function (resource) {
                    return resource.updateDate !== defaultDateTime;
                });

                self.listSettings.isShowDescription = self.resources.some(function (resource) {
                    return resource.description !== null;
                });

                self.workspaceDisabled = false;
            }, function () {
                notificationsService.error("Error", "Resources were not loaded");
            });

            userService.getCurrentUser().then(function (currentUserData) {
                self.permissions.isCanDelete = currentUserData.userGroups.indexOf("admin") > -1;
            });
        }

        init();
    }

    controller.$inject = ["$timeout", "notificationsService", "editorService", "assetsService",
        "userService", "localizationResourceService"];

    angular.module('umbraco').controller('localizationOverviewController', controller);
})(angular);