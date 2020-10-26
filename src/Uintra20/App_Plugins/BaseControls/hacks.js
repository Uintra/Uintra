(function () {
    //This file contains fixed umbraco directive 'outsideClick'.
    //If umbraco version updated please review this file and make sure we dont loose umbraco changes

    if (Umbraco.Sys.ServerVariables.application.version != '7.5.7') { //update version to current after review
        console.debug("======================================================================================================================================");
        console.debug("UMBRACO VERSION WAS UPDATED!! PLEASE REVIEW THIS FILE (please check comments below) ༼ つ ◕_◕ ༽つ ");
        console.debug("======================================================================================================================================");
    }


    var outsideClickDirective = function ($timeout) {
        return function (scope, element, attrs) {
            var eventBindings = [];
            function oneTimeClick(event) {
                var el = event.target;
                //ignore link and button clicks
                var els = ["INPUT", "A", "BUTTON"];
                if (els.indexOf(el.nodeName) >= 0) { return; }


                el = $(el);

                // ignore children of links and buttons
                // ignore clicks on new overlay
                var parents = el.parents("a,button,.umb-overlay");
                if (parents.length > 0) {
                    return;
                }

                // ignore clicks on dialog from old dialog service
                var oldDialog = el.parents("#old-dialog-service");
                if (oldDialog.length === 1) {
                    return;
                }

                // ignore clicks in tinyMCE dropdown(floatpanel)
                var floatpanel = el.parents(".mce-floatpanel");
                if (floatpanel.length === 1) {
                    return;
                }

                //CUSTOM CHECK. MAKE SURE WE DONT LOOSE THIS
                var flatpickr = el.parents('.flatpickr-calendar');
                if (flatpickr.length === 1) {
                    return;
                }

                //ignore clicks inside this element
                if ($(element).has(el).length > 0) {
                    return;
                }

                scope.$apply(attrs.onOutsideClick);
            }
            $timeout(function () {
                if ("bindClickOn" in attrs) {

                    eventBindings.push(scope.$watch(function () {
                        return attrs.bindClickOn;
                    }, function (newValue) {
                        if (newValue === "true") {
                            $(document).on("click", oneTimeClick);
                        } else {
                            $(document).off("click", oneTimeClick);
                        }
                    }));

                } else {
                    $(document).on("click", oneTimeClick);
                }

                scope.$on("$destroy", function () {
                    $(document).off("click", oneTimeClick);
                    // unbind watchers
                    for (var e in eventBindings) {
                        eventBindings[e]();
                    }

                });
            }); // Temp removal of 1 sec timeout to prevent bug where overlay does not open. We need to find a better solution.
        };
    }
    outsideClickDirective.$inject = ['$timeout'];
    angular.module('umbraco.directives').directive('onOutsideClick', outsideClickDirective);


    function config($provide) {
        $provide.decorator('onOutsideClickDirective', function ($delegate) {
            $delegate.shift();
            return $delegate;
        });
    }
    angular.module('umbraco.directives').config(['$provide', config]);

    //IF YOU NEED LIST OF ALL DIRECTIVES
    //var moduleName = 'umbraco.directives';
    //angular.module(moduleName).config(function ($provide) {
    //    var invokeQueue = angular.module(moduleName)._invokeQueue;
    //    invokeQueue.forEach(function (service) {
    //        if (service[1] === 'directive') {
    //            var directive = service[2][0];
    //            console.log(directive);
    //            $provide.decorator(directive + 'Directive', function ($delegate) {
    //                return $delegate;
    //            });
    //        }
    //    });
    //});
})();