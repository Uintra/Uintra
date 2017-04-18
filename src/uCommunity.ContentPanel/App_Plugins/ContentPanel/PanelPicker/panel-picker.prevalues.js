(function () {
    var controller = function (gridService, $scope) {
        var self = this;
        self.gridEditors = [];

        gridService.getGridEditors().then(function (response) {
            self.gridEditors = response.data;
        });

        self.init = function (model) { self.model = model; self.model.value = self.model.value || { allowedEditors: [] } }

        self.selectAll = function () {
            self.model.value.allowedEditors = [];
            self.gridEditors.forEach(self.editorClickHandler);
        }

        self.removeAll = function () {
            self.model.value.allowedEditors = [];
        }

        self.editorClickHandler = function (editor) {
            if (self.isEditorSelected(editor)) {
                var index = self.model.value.allowedEditors.indexOf(editor.alias);
                self.model.value.allowedEditors.splice(index, 1);
            } else {
                self.model.value.allowedEditors.push(editor.alias);
            }
        }

        self.isEditorSelected = function (editor) {
            return self.model.value.allowedEditors.indexOf(editor.alias) > -1;
        }

    }

    controller.$inject = ["gridService", "$scope"];
    angular.module('umbraco').controller('panelPickerPrevaluesController', controller);
})();