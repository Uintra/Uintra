(function (angular) {
    var factory = function ($q, assetsService) {
        var preloadAssets = function ($scope, $q, assetsService) {
            var queue = [];
            if (typeof (ace) === "undefined") {
                queue.push(assetsService.loadJs("~/App_Plugins/BaseControls/ConfigEditor/aceeditor/ace.js", $scope));
            }
            return $q.all(queue);
        };

        var tryParseJson = function (jsonString) {
            try {
                return JSON.parse(jsonString);
            }
            catch (e) {
                return null;
            }
        }

        var link = function ($scope) {
            var init = function () {
                var editor = ace.edit("editor");
                editor.setTheme("ace/theme/monokai");
                var session = editor.getSession();
                session.setMode("ace/mode/json");
                session.setUseWrapMode(true);

                if ($scope.ngModel) {
                    editor.setValue(JSON.stringify($scope.ngModel, null, 4), 1);
                }

                session.on("changeAnnotation",
                    function () {
                        var annotations = session.getAnnotations();
                        var errors = annotations.find(function (annotation) {
                            return annotation.type === "error";
                        });

                        if (errors) {
                            $scope.isNoAceError = "";
                        } else {
                            $scope.isNoAceError = true;
                        }
                    });

                session.on("change",
                    function (e) {
                        var jsonModel = tryParseJson(editor.getValue());
                        if (jsonModel) {
                            $scope.ngModel = jsonModel;
                        }
                    });

                editor.commands.addCommand({
                    name: 'format',
                    bindKey: { win: 'Ctrl-B', mac: 'Command-B' },
                    exec: function (editor) {
                        var value = JSON.parse(editor.getValue());
                        var valueString = JSON.stringify(value, null, 4);
                        editor.setValue(valueString, 1);
                    }
                });

                editor.gotoLine(0);
            };

            preloadAssets($scope, $q, assetsService).then(init);
        };

        return {
            restrict: 'E',
            templateUrl: '/App_Plugins/BaseControls/ConfigEditor/rte-config-editor.html',
            scope: { ngModel: '=' },
            link: link
        };
    }
    factory.$inject = ['$q', 'assetsService'];
    angular.module('umbraco').directive('configEditor', factory);
})(angular);