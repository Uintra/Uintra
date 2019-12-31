function ublTree() {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: '/App_Plugins/PanelContainer/directives/tree/tree.html',
        scope: {
           model: '<',
           selected: '&'
        },
        controller: ['$scope', function(scope) {

            this.select = (panel) => {
                scope.selected({panel: panel});
            }
        }]
    }
}

function ublTreeItem() {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: "/App_Plugins/PanelContainer/directives/tree/tree-item.html",
        scope: {
           model: '<',
           level: '<',
        },
        require: '^^ublTree',
        link: function link(scope, element, attrs, treeCtrl) {
            scope.toggleChildren = function() {
                scope.model.expanded = !scope.model.expanded;
            }

            scope.select = function(model) {
                treeCtrl.select(model);
            }
        }
    }
}

angular.module('umbraco.directives')
    .directive("ublTree", ublTree)
    .directive("ublTreeItem", ublTreeItem);