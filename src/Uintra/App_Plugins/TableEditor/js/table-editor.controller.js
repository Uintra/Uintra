(function (angular) {
    'use strict';

    const controller = function ($scope) {

        var emptyCellModel = '{"value": ""}';
        var defaultModel = {
            useFirstRowAsHeader: true,
            makeFirstColumnBold: false,
            tableStyle: null,
            columnStylesSelected: [
                null,
                null
            ],
            rowStylesSelected: [
                null,
                null,
                null
            ],
            cells: [
                [{ value: "" }, { value: "" }],
                [{ value: "" }, { value: "" }],
                [{ value: "" }, { value: "" }]
            ]
        }

        $scope.model.value = $scope.model.value || defaultModel;
        $scope.model.config.tableStyles = parseStyleConfig($scope.model.config.tableStyles);
        $scope.model.config.columnStyles = parseStyleConfig($scope.model.config.columnStyles);
        $scope.model.config.rowStyles = parseStyleConfig($scope.model.config.rowStyles);

        $scope.canAddRow = function () {
            if (isNaN(parseInt($scope.model.config.maxRows, 10))) {
                return true;
            }

            return ($scope.model.config.maxRows > $scope.model.value.cells.length);
        }

        $scope.addRow = function ($index) {
            if ($scope.canAddRow()) {
                var newRow = [];

                for (var i = 0; i < getColumnCount(); i++) {
                    newRow.push(emptyCellModel);
                }

                $scope.model.value.cells.splice($index + 1, 0, JSON.parse("[" + newRow.join(',') + "]"));
            }
        }

        $scope.canAddColumn = function () {

            if (isNaN(parseInt($scope.model.config.maxColumns, 10))) {
                return true;
            }

            return ($scope.model.config.maxColumns > getColumnCount());
        }

        $scope.addColumn = function ($index) {
            if ($scope.canAddColumn()) {

                //style
                $scope.model.value.columnStylesSelected.splice($index + 1, 0, null);

                //cells
                for (var i in $scope.model.value.cells) {
                    $scope.model.value.cells[i].splice($index + 1, 0, JSON.parse(emptyCellModel));
                }
            }
        }

        $scope.canRemoveRow = function () {
            return ($scope.model.value.cells.length > 1);
        }

        $scope.removeRow = function ($index) {
            if ($scope.canRemoveRow()) {
                if (confirm("Are you sure you'd like to remove this row?")) {
                    $scope.model.value.cells.splice($index, 1);
                }
            }
        }

        $scope.canRemoveColumn = function () {
            return getColumnCount() > 1;
        }

        $scope.removeColumn = function ($index) {
            if ($scope.canRemoveColumn()) {
                if (confirm("Are you sure you'd like to remove this column?")) {
                    $scope.model.value.columnStylesSelected.splice($index, 1);

                    for (var i in $scope.model.value.cells) {
                        $scope.model.value.cells[i].splice($index, 1);
                    }
                }
            }
        }

        $scope.canSort = function () {
            return ($scope.model.value.cells.length > 1);
        }

        //sort config
        $scope.sortableOptions = {
            axis: 'y',
            cursor: "move",
            handle: ".handle",
            update: function (ev, ui) {

            },
            stop: function (ev, ui) {

            }
        };

        $scope.clearTable = function () {
            if (confirm("Are you sure you wish to remove everything from the table?")) {
                $scope.model.value = defaultModel;
            }
        }

        function getColumnCount() {
            return $scope.model.value.cells[0].length;
        }

        function parseStyleConfig(configString) {
            if (!configString)
                return;

            //Col Style 1,col-style-1

            var lines = configString.split('\n');
            var styles = [{ label: "None", value: null }];

            for (var i in lines) {
                var style = {};
                var temp = lines[i].split(',');

                if (temp[0].trim() != "" && temp[1].trim() != "") {
                    style.label = temp[0].trim();
                    style.value = temp[1].trim();

                    styles.push(style);
                }
            }

            return styles;
        }
    }

    angular.module('umbraco').controller('TableEditorController', ["$scope", controller]);
})(angular);