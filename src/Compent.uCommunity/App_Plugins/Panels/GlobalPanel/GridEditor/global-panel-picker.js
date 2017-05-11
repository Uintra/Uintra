(function () {
    'use strict';

    var controller = function (dialogService, entityResource, $q) {
        var self = this;
        self.hi = "Hi there!";
        self.init = function (control) {
            self.control = control;
            !self.control.value && openNodePicker();
        }

        self.selectPanel = function() {
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

        function nodeSelected(content) {
            self.control.value = {
                id: content.id,
                name: content.name
            }
        }

        function getStartNodeId() {
            return entityResource.getByQuery(self.control.editor.config.startNodeXPath, -1, "Document");
        }
    }
    controller.$inject = ['dialogService', 'entityResource', '$q'];
    angular.module("umbraco").controller('globalPanelPickerController', controller);
})();