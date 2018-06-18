(function (angular) {
    'use strict';    
    const controller = function ($scope) {

        var emptyCellModel = '{"value": ""}';
        var defaultModel = {
            tableId: uuidv4(),
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

        $scope.overlay = {
            show: false,
            view: "/App_Plugins/Panels/TableEditor/views/overlay.html",
            title: "Table panel",
            close: function () {
                $scope.overlay.show = false;
                $scope.control.value = $scope.backupModel;
            },
            submit: function () {
                $scope.overlay.show = false;
            }
        }

        $scope.open = function () {
            $scope.overlay.show = true;
            $scope.control.value = $scope.control.value;
            $scope.backupModel = angular.copy($scope.control.value);
        }

        $scope.init = function (control) {            
            $scope.control = control;       
            $scope.control.config = {
                maxRows: 10,
                maxColumns:10
            }                       
            $scope.control.value = $scope.control.value || defaultModel;     
        };

        $scope.canAddRow = function () {
            if (isNaN(parseInt($scope.control.config.maxRows, 10))) {
                return true;
            }

            return ($scope.control.config.maxRows > $scope.control.value.cells.length);
        }

        $scope.addRow = function ($index) {
            if ($scope.canAddRow()) {
                var newRow = [];

                for (var i = 0; i < getColumnCount(); i++) {
                    newRow.push(emptyCellModel);
                }

                $scope.control.value.cells.splice($index + 1, 0, JSON.parse("[" + newRow.join(',') + "]"));
            }
        }

        $scope.canAddColumn = function () {
            
            if (isNaN(parseInt($scope.control.config.maxColumns, 10))) {
                return true;
            }

            return ($scope.control.config.maxColumns > getColumnCount());
        }

        $scope.addColumn = function ($index) {
            if ($scope.canAddColumn()) {

                //style
                $scope.control.value.columnStylesSelected.splice($index + 1, 0, null);

                //cells
                for (var i in $scope.control.value.cells) {
                    $scope.control.value.cells[i].splice($index + 1, 0, JSON.parse(emptyCellModel));
                }
            }
        }

        $scope.canRemoveRow = function () {
            return ($scope.control.value.cells.length > 1);
        }

        $scope.removeRow = function ($index) {
            if ($scope.canRemoveRow()) {
                if (confirm("Are you sure you'd like to remove this row?")) {
                    $scope.control.value.cells.splice($index, 1);
                }
            }
        }

        $scope.canRemoveColumn = function () {
            return getColumnCount() > 1;
        }

        $scope.removeColumn = function ($index) {
            if ($scope.canRemoveColumn()) {
                if (confirm("Are you sure you'd like to remove this column?")) {
                    $scope.control.value.columnStylesSelected.splice($index, 1);

                    for (var i in $scope.control.value.cells) {
                        $scope.control.value.cells[i].splice($index, 1);
                    }
                }
            }
        }

        $scope.canSort = function () {
            return ($scope.control.value.cells.length > 1);
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
                $scope.control.value = defaultModel;
            }
        }

        function uuidv4() {
            return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
                return v.toString(16);
            });
        }

        function getColumnCount() {
            return $scope.control.value.cells[0].length;
        }

        function parseStyleConfig(configString) {            
            if (!configString)
                return [];

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