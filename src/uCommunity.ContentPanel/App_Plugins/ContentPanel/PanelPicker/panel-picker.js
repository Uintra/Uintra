(function () {
    'use strict';

    var controller = function (gridService, $scope) {
        var self = this;
        self.gridEditors = [];

        self.init = function (model) {
            self.model = model;
            self.config = model.config.config;
            loadGridEditors();

            //FU umbraco. I know dirty hacks too.
            self.model.value = self.model.value || { value: {}, editor: {}, active: true, inGlobalPanelScope: true };
            $scope.control = self.model.value;
            $scope.$watch('control', function () { self.model.value = $scope.control; }, true);
        };

        self.selectPanel = function (editor) {
            self.model.value.value = null;
            self.model.value.editor = editor;
        }

        self.removePanel = function () {
            self.model.value.editor = null;
        }

        function loadGridEditors() {
            gridService.getGridEditors().then(function (response) {
                response.data.forEach(function (editor) {
                    if (self.config.allowedEditors.indexOf(editor.alias) > -1) {
                        self.gridEditors.push(editor);
                    }
                });
            });
        }
    }
    controller.$inject = ["gridService", "$scope"];
    angular.module('umbraco').controller('panelPickerController', controller);
})();