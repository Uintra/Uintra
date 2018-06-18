(function (angular) {

    var linksPickerFactory = function (interpolate, config) {

        var getLinkElement = function (model) {
            var linkModel = {
                id: model.id,
                link: model.link,
                target: model.target || "",
                prefix: (model.additionalType || {}).value || "",
                caption: model.caption,
                type: model.type
            }

            var elem = interpolate('<a href="{prefix}{link}" data-id="{id}" data-type="{type}" target="{target}">{caption}</a>', linkModel);
            return elem;
        }

        var getLinkInfo = function (editor) {
            var node = editor.selection.getNode();
            return {
                id: node.dataset["id"],
                caption: node.innerHTML,
                target: node.target,
                type: node.dataset["type"],
                link: node.href
            }
        }

        var isLinkSelected = function (editor) {
            var node = editor.selection.getNode();
            return node && node.nodeName == "A";
        }

        var insertContent = function (editor, content) {
            if (isLinkSelected(editor)) {
                var node = editor.selection.getNode();
                node.remove();
                editor.insertContent(content);
            } else {
                editor.insertContent(content);
            }
        }

        return {
            icon: 'custom icon-link',
            tooltip: 'Link picker',
            onclick: function (editor, $scope) {
                $scope.linksPickerOverlay = {
                    view: "/App_Plugins/BaseControls/TinyMce/link-picker-overlay.html",
                    title: "Pick link",
                    position: "right",
                    show: true,
                    config: config.linksPicker,
                    selectedText: "",
                    submit: function (model) {
                        var linkContent = getLinkElement(model.value);
                        insertContent(editor, linkContent);
                        $scope.linksPickerOverlay.show = false;
                    },
                    close: function () {
                        $scope.linksPickerOverlay.show = false;
                    }
                };

                if (isLinkSelected(editor)) {
                    $scope.linksPickerOverlay.value = getLinkInfo(editor);
                } else {
                    var selectedText = editor.selection.getContent({ format: 'text' });
                    $scope.linksPickerOverlay.caption = selectedText || "";
                }
            }
        }
    };

    var umbmediapickerFactory = function (dialogService, interpolate) {
        return {
            icon: 'custom icon-picture',
            tooltip: 'Media Picker',
            onclick: function (editor) {
                dialogService.mediaPicker({
                    disableFolderSelect: true,
                    multiPicker: false,
                    filterCssClass: "not-allowed not-published",
                    callback: function (content) {
                        if (content.contentTypeAlias == "Image") {
                            var img = '<img src="' + content.image + '" alt="' + content.name + '" ></img>';
                            editor.insertContent(img);
                        } else {
                            var linkModel = {
                                link: content.image,
                                caption: content.name
                            }
                            var elem = interpolate('<a href="{link}" target="_blank">{caption}</a>', linkModel);
                            editor.insertContent(elem);
                        }
                    }
                });
            }
        }
    }

    var defaultConfig = {
        plugins: "paste code media lists",
        paste_preprocess: function (plugin, args) {
            var content = args.content.replace(/&lt;/gi, '<').replace(/&gt;/gi, '>').replace(/&quot;/gi, '"').replace(/<\/br>/gi, ''); // ¯\_(ツ)_/¯
            args.content = angular.element('<div>' + content + '</div>').text();
        },
        mode: "exact",
        skin: "umbraco",
        menubar: false,
        statusbar: false,
        relative_urls: false,
        toolbar: ["styleselect", "bold", "italic", "alignleft", "aligncenter", "alignright", "bullist", "numlist", "helloWorld", "linksPicker"].join(" | "),
        //style_formats: [], // https://www.tinymce.com/docs/configure/content-formatting/#style_formats
        autoresize_bottom_margin: 0,
        linksPicker: {
            mode: "single" //or multiple
        },
        extraButtons: {
            helloWorld: {
                icon: 'custom icon-tv',
                tooltip: 'Hello world',
                onclick: function (editor, $scope) {
                    editor.insertContent('<br /> <h1>Hello world</h1>');
                }
            }
        },
        content_css: [
            '/App_Plugins/BaseControls/TinyMce/tiny-mce.css'
        ]
    };

    var buildConfig = function ($scope, customConfig, valueUpdater, interpolateFilter, dialogService) {
        var config = angular.extend({}, angular.copy(defaultConfig), customConfig);
        config.extraButtons.linksPicker = linksPickerFactory(interpolateFilter, config);
        config.extraButtons.umbmediapicker = umbmediapickerFactory(dialogService, interpolateFilter);

        var thisEditorId = config.editorId;
        
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

            // dirty hack to reset editor when data is outdated
            $(document).on('editorDataReset', function (event, model, editorId) {
                if (editorId === thisEditorId) {
                    editor.setContent(model);
                }
            });

            angular.forEach(config.extraButtons, function (value, key) {
                var originalOnClick = value.onclick || angular.noop;
                value.onclick = function (e) { originalOnClick(editor, $scope); };
                editor.addButton(key, value);
            });

            var onFormSubmitting = $scope.$on("formSubmitting", function () {
                $scope.model = editor.getContent();
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
            var valueSetter = function () {
                $scope.model = editor.getContent();
            };
            angularHelper.safeApply($scope, valueSetter);
        };
    };

    var factory = function ($q, $timeout, $http, angularHelper, assetsService, interpolateFilter, dialogService) {
        return {
            scope: { model: '=', config: '=' },
            restrict: 'E',
            templateUrl: '/App_Plugins/BaseControls/TinyMce/tiny-mce.html',
            link: function ($scope) {
                var valueUpdater = valueUpdaterFactory($scope, angularHelper);

                var getConfig = function () {
                    if (typeof $scope.config === 'string') {
                        return $http
                            .get('/umbraco/backoffice/api/RteConfig/GetConfig?rteAlias=' + $scope.config)
                            .then(function (res) {
                                var config = res.data.data;
                                $scope.customConfig = buildConfig($scope,
                                    config,
                                    valueUpdater,
                                    interpolateFilter,
                                    dialogService);
                            });
                    }

                    var config = $scope.config ? $scope.config : {};
                    $scope.customConfig = buildConfig($scope, config, valueUpdater, interpolateFilter, dialogService);
                }

                var init = function () {
                    $timeout(function () {
                        tinymce.DOM.events.domLoaded = true;
                        tinymce.init($scope.customConfig);
                    }, 150, false);
                };

                preloadAssets($scope, $q, assetsService)
                    .then(getConfig)
                    .then(init);
            }
        };
    };

    factory.$inject = ['$q', '$timeout', '$http', 'angularHelper', 'assetsService', 'interpolateFilter', 'dialogService'];
    angular.module("umbraco").directive('tinyMce', factory);
})(angular);