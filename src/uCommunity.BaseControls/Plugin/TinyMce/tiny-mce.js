(function (angular) {
    var defaultConfig = {
        mode: "exact",
        skin: "umbraco",
        menubar: false,
        statusbar: false,
        relative_urls: false,
        toolbar: ["styleselect", "bold", "italic", "alignleft", "aligncenter", "alignright", "bullist", "numlist", "helloWorld"].join(" | "),
        //style_formats: [], // https://www.tinymce.com/docs/configure/content-formatting/#style_formats
        autoresize_bottom_margin: 0,
        extraButtons: {
            helloWorld: {
                icon: 'custom icon-tv',
                tooltip: 'Hello world',
                onclick: function (editor, $scope) {
                    editor.insertContent('<br /> <h1>Hello world</h1>');
                }
            }
        }
    };

    var buildConfig = function ($scope, valueUpdater) {
        var config = angular.extend(defaultConfig, $scope.config || {});
        config.elements = 'tiny-mce-' + $scope.$id;
        config.setup = function (editor) {
            var updateValue = function (e) { valueUpdater(editor); };
            editor.on('blur', updateValue); // when leave
            editor.on('ExecCommand', updateValue); // when buttons modify content
            editor.on('KeyUp', updateValue); // when keypress
            editor.on('SetContent', function (e) {
                if (!e.initial) {
                    updateValue(e);
                }
            }); // when: copy/pasted text, plugins altering content

            angular.forEach(config.extraButtons, function (value, key) {
                var originalOnClick = value.onclick || angular.noop;
                value.onclick = function (e) { originalOnClick(editor, $scope); };
                editor.addButton(key, value);
            });

            var onFormSubmitting = $scope.$on("formSubmitting", function () {
                $scope.value = editor.getContent();
            });
            $scope.$on('$destroy', onFormSubmitting);
        };
        return config;
    };

    var preloadAssets = function ($scope, $q, assetsService) {
        var queue = [];
        if (typeof (tinymce) === "undefined") {
            queue.push(assetsService.loadJs("lib/tinymce/tinymce.min.js", $scope));
        }
        return $q.all(queue);
    };

    var valueUpdaterFactory = function ($scope, angularHelper) {
        return function (editor) {
            editor.save();
            var valueSetter = function () { $scope.value = editor.getContent(); };
            angularHelper.safeApply($scope, valueSetter);
        };
    };

    var factory = function ($q, $timeout, angularHelper, assetsService) {
        return {
            scope: {
                value: '=ngModel',
                config: '='
            },
            restrict: 'E',
            templateUrl: '/App_Plugins/BaseControls/TinyMce/tiny-mce.html',
            link: function ($scope) {
                var valueUpdater = valueUpdaterFactory($scope, angularHelper);
                var config = buildConfig($scope, valueUpdater);
                var init = function () {
                    $timeout(function () {
                        tinymce.DOM.events.domLoaded = true;
                        tinymce.init(config);
                    }, 150, false);
                };
                preloadAssets($scope, $q, assetsService).then(init);
            }
        };
    };

    factory.$inject = ['$q', '$timeout', 'angularHelper', 'assetsService'];
    angular.module("umbraco").directive('tinyMce', factory);
})(angular);