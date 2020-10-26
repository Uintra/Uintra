(function (angular) {
    'use strict';

    const tableEditorColumnControl = function () {
        return {
            restrict: "A",
            link: function (scope, element, attrs) {

                //had to encapsulate all of the jquery in each function due to the dynamic nature of the prop editor

                element.bind('mouseover', function () {
                    var $td = jQuery(element);
                    var index = $td.index() + 1;
                    var $table = $td.closest("table");
                    var $tds = $table.find("tbody td:nth-child(" + index + ")");
                    var $th1 = $table.find("thead tr:nth-child(1) th:nth-child(" + (index) + ")");
                    var $th2 = $table.find("thead tr:nth-child(2) th:nth-child(" + (index) + ")");
                    $tds.addClass("col-highlighted");
                    $th1.addClass("col-highlighted");
                    $th2.addClass("col-highlighted");

                    $th1.css('visibility', 'visible');

                    if ($th1.find('option').length > 1) {
                        $th1.find('select').css('visibility', 'visible');
                    }
                });

                element.bind('mouseout', function () {
                    var $td = jQuery(element);
                    var index = $td.index() + 1;
                    var $table = $td.closest("table");
                    var $tds = $table.find("tbody td:nth-child(" + index + ")");
                    var $th1 = $table.find("thead tr:nth-child(1) th:nth-child(" + index + ")");
                    var $th2 = $table.find("thead tr:nth-child(2) th:nth-child(" + (index) + ")");
                    $tds.removeClass("col-highlighted");
                    $th1.removeClass("col-highlighted");
                    $th2.removeClass("col-highlighted");
                    $th1.css('visibility', 'hidden');
                    $th1.find('select').css('visibility', 'hidden');
                });
            }
        }
    }

    angular.module("umbraco").directive('tableEditorColumnControl', tableEditorColumnControl);
})(angular);