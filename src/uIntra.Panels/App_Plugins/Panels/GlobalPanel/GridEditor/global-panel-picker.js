(function () {
    'use strict';

    var controller = function (dialogService, entityResource, contentResource, $q) {
        var self = this;

        self.init = function (control) {
            self.control = control;

            if (!self.control.value) {
                openNodePicker();
            } else {
                fillPanelName(self.control.value.id);
            }
        }

        self.selectPanel = function () {
            openNodePicker();
        }

        function openDialog(response) {
            var config = self.control.editor.config;
            var options = {
                multiPicker: false,
                filterCssClass: "not-allowed not-published",
                filter: config.allowedAliases.join(','),
                startNodeId: response.id,
                callback: nodeSelected
            }
            dialogService.contentPicker(options);
        }

        function openNodePicker() {
            getStartNodeId().then(openDialog);
        }

        function fillPanelName(contentId) {
            contentResource.getById(contentId).then(function (response) {
                var panelTab = response.tabs.filter(function (tab) {
                    return tab.alias === "Panel";
                })[0];

                var panelConfigProperty = panelTab.properties.filter(function (property) {
                    return property.alias === "panelConfig";
                })[0];

                self.panelName = panelConfigProperty.value.editor.name;
            });
        }

        function nodeSelected(content) {
            fillPanelName(content.id);

            self.control.value = {
                id: content.id,
                name: content.name
            }
        }

        function getStartNodeId() {
            return entityResource.getByQuery(self.control.editor.config.startNodeXPath, -1, "Document");
        }
    }
    controller.$inject = ['dialogService', 'entityResource', 'contentResource', '$q'];
    angular.module("umbraco").controller('globalPanelPickerController', controller);
})();