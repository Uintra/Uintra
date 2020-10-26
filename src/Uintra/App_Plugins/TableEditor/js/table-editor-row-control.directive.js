(function (angular) {
    'use strict';

    const tableEditorRowControl = function () {
        return {
            restrict: "A",
            link: function (scope, element, attrs) {

                var $rowControls = jQuery(element).find("td.row-buttons-td div");
                var $rowStyle = jQuery(element).find("td.row-style");
                var selectActive = false;

                $rowStyle.find("select").focus(function () {
                    selectActive = true;
                });

                $rowStyle.find("select").blur(function () {
                    selectActive = false;
                });

                element.bind('mouseover', function () {
                    selectActive = false;
                    $rowControls.show();

                    element.addClass("row-highlighted");

                    if ($rowStyle.find('option').length > 1) {
                        $rowStyle.css('visibility', 'visible');
                    }
                });

                element.bind('mouseout', function () {
                    if (selectActive == false) {
                        $rowControls.hide();
                        $rowStyle.css('visibility', 'hidden');
                        //$rowStyle.find('select').hide();
                        element.removeClass("row-highlighted");
                    }
                });
            }
        }
    }

    angular.module("umbraco").directive('tableEditorRowControl', tableEditorRowControl);
})(angular);