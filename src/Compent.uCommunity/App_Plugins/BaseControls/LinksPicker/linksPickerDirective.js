(function () {
    'use strict';

    angular
        .module('umbraco')
        .directive('linksPicker', linksPickerDirective);

    linksPickerDirective.$inject = ["$routeParams", "entityResource", "dialogService"];

    function linksPickerDirective($routeParams, entityResource, dialogService) {
        return {
            scope: {
                shouldbeonelink:"=",
                startNodeQuery: "@",
                allowedDocTypes: "=",
                links:"="
            },
            restrict: 'E',
            templateUrl: '/App_Plugins/BaseControls/LinksPicker/linksPicker.html',
            link: function (scope, element, attrs) {

                scope.hideAddSection = scope.shouldbeonelink === "1" && scope.links.length > 0;
                if (!scope.startNodeQuery) {
                    scope.startNodeQuery = "$current/ancestor-or-self::*[@isDoc]";
                }

                if (!scope.links) {
                    scope.links = [];
                }

                scope.typesEnum = {
                    Internal: 0,
                    External: 1,
                    Media: 2
                };

                scope.newCaption = '';
                scope.newLink = 'http://';
                scope.newNewWindow = false;
                scope.newInternal = null;
                scope.newInternalName = '';
                scope.newMedia = null;
                scope.newMediaName = '';
                scope.addType = scope.typesEnum.Internal;
                scope.currentEditLink = null;
                scope.hasError = false;
                scope.hasExternalError = false;
                scope.hasInternalError = false;
                scope.hasMediaError = false;

                function getElementIndexByUrl(url) {
                    for (var i = 0; i < scope.links.length; i++) {
                        if (scope.links[i].link == url) {
                            return i;
                        }
                    }

                    return -1;
                }

                function chooseContent() {
                    var rootId = $routeParams.id;
                    entityResource.getByQuery(scope.startNodeQuery, rootId, "Document").then(function (ent) {
                        dialogService.contentPicker({
                            multiPicker: false,
                            startNodeId: ent.id,
                            filterCssClass: "not-allowed not-published",
                            filter: scope.allowedDocTypes,
                            callback: function (data) {
                                if (scope.currentEditLink != null) {
                                    scope.currentEditLink.link = data.id;
                                    scope.currentEditLink.name = data.name;
                                } else {
                                    scope.newInternal = data.id;
                                    scope.newInternalName = data.name;
                                }
                            }
                        });
                    });
                }

                function chooseMedia() {
                    dialogService.mediaPicker({
                        callback: function (data) {
                            if (scope.currentEditLink != null) {
                                scope.currentEditLink.link = data.id;
                                scope.currentEditLink.name = data.name;
                            } else {
                                scope.newMedia = data.id;
                                scope.newMediaName = data.name;
                            }
                        }
                    });
                }

                scope.internal = function () {
                    scope.currentEditLink = null;

                    chooseContent();
                };

                scope.media = function () {
                    scope.currentEditLink = null;

                    chooseMedia();
                }

                scope.selectInternal = function (link) {
                    scope.currentEditLink = link;
                    chooseContent();
                };

                scope.selectMedia = function (link) {
                    scope.currentEditLink = link;
                    chooseMedia();
                };

                scope.edit = function (idx) {
                    for (var i = 0; i < scope.links.length; i++) {
                        scope.links[i].edit = false;
                    }
                    scope.links[idx].edit = true;
                };

                scope.saveEdit = function (idx) {
                    var link = scope.links[idx];
                    link.hasError = !link.caption;

                    if (!link.hasError) {
                        scope.links[idx].edit = false;
                    }
                };

                scope.delete = function (idx) {
                    scope.links.splice(idx, 1);
                    scope.hideAddSection = scope.shouldbeonelink === "1" && scope.links.length > 0;
                };

                scope.add = function ($event) {
                    if (scope.links === "") {
                        scope.links = [];
                    }

                    if (scope.newCaption === "") {
                        scope.hasError = true;
                    }
                    else if (scope.addType === scope.typesEnum.External && (scope.newLink === "http://" || scope.newLink === "")) {
                        scope.hasExternalError = true;
                    }
                    else if (scope.addType === scope.typesEnum.Internal && scope.newInternal === null) {
                        scope.hasInternalError = true;
                    }
                    else if (scope.addType === scope.typesEnum.Media && scope.newMedia === null) {
                        scope.hasMediaError = true;
                    }
                    else {
                        if (scope.addType === scope.typesEnum.External) {
                            var newExtLink = new function () {
                                this.caption = scope.newCaption;
                                this.link = scope.newLink;
                                this.newWindow = true;
                                this.edit = false;
                                this.type = scope.typesEnum.External;
                            };
                            scope.links.push(newExtLink);
                        } else if (scope.addType === scope.typesEnum.Internal) {
                            var newIntLink = new function () {
                                this.caption = scope.newCaption;
                                this.link = scope.newInternal;
                                this.newWindow = scope.newNewWindow;
                                this.edit = false;
                                this.name = scope.newInternalName;
                                this.type = scope.typesEnum.Internal;
                            };
                            scope.links.push(newIntLink);
                        } else {
                            var newMediaLink = new function () {
                                this.caption = scope.newCaption;
                                this.link = scope.newMedia;
                                this.newWindow = true;
                                this.edit = false;
                                this.name = scope.newMediaName;
                                this.type = scope.typesEnum.Media;
                            };
                            scope.links.push(newMediaLink);
                        }

                        scope.newCaption = '';
                        scope.newLink = 'http://';
                        scope.newInternal = null;
                        scope.newInternalName = '';
                        scope.newMedia = null;
                        scope.newMediaName = '';

                        scope.hideAddSection = scope.shouldbeonelink === "1" && scope.links.length > 0;
                    }
                    $event.preventDefault();
                };

                scope.switch = function (type) {
                    scope.newNewWindow = type === scope.typesEnum.External || type === scope.typesEnum.Media;
                    scope.addType = type;
                };

                scope.switchLinkToInternal = function (link) {
                    link.type = scope.typesEnum.Internal;
                    link.link = null;
                    link.name = "";
                };

                scope.switchLinkToExternal = function (link) {
                    link.type = scope.typesEnum.External;
                    link.link = scope.newLink;
                };

                scope.switchLinkToMedia = function (link) {
                    link.type = scope.typesEnum.Media;
                    link.link = null;
                    link.Name = "";
                };

                scope.move = function (index, direction) {
                    var temp = scope.links[index];
                    scope.links[index] = scope.links[index + direction];
                    scope.links[index + direction] = temp;
                };

                scope.sortableOptions = {
                    containment: 'parent',
                    cursor: 'move',
                    helper: function (e, ui) {
                        // When sorting <trs>, the cells collapse.  This helper fixes that: http://www.foliotek.com/devblog/make-table-rows-sortable-using-jquery-ui-sortable/
                        ui.children().each(function () {
                            $(this).width($(this).width());
                        });
                        return ui;
                    },
                    items: '> tr',
                    tolerance: 'pointer',
                    update: function (e, ui) {
                        // Get the new and old index for the moved element (using the URL as the identifier)
                        var newIndex = ui.item.index();
                        var movedLinkUrl = ui.item.attr('data-link');
                        var originalIndex = getElementIndexByUrl(movedLinkUrl);

                        // Move the element in the model
                        var movedElement = scope.links[originalIndex];
                        scope.links.splice(originalIndex, 1);
                        scope.links.splice(newIndex, 0, movedElement);
                    }
                };

            }
        };
    }

})();