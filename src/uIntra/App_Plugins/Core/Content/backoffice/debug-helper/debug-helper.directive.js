(function () {
	'use strict';

	angular.module('umbraco').directive('debugHelper', function () {
		return {
			restrict: 'E',
			scope: {
				model: "=model"
			},
			replace: true,
			template: '<pre class="debug-helper"><span ng-bind-html="debugModel"></span></pre>',
			link: function ($scope, $elem, $attrs) {
				$scope.$watch('model', function () {
					try {
						$scope.debugModel = highlight($scope.model);
					} catch (ex) {
						console.error(ex);
					}
				}, true);

				function highlight(json) {
					if (typeof json != 'string') {
						json = JSON.stringify(json, propsHandler, 2);
					}
					json = json.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
					return json.replace(/("(\\u[a-zA-Z0-9]{4}|\\[^u]|[^\\"])*"(\s*:)?|\b(true|false|null)\b|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?)/g, function (match) {
						var cls = 'number';
						if (/^"/.test(match)) {
							if (/:$/.test(match)) {
								cls = 'key';
							} else {
								cls = 'string';
							}
						} else if (/true|false/.test(match)) {
							cls = 'boolean';
						} else if (/null/.test(match)) {
							cls = 'null';
						} else if (/undefined/.test(match)) {
							cls = 'undefined';
						}
						return '<span class="' + cls + '">' + match + '</span>';
					});
				}

				function propsHandler(key, value) {
					return value === undefined ? '~Undefined here~' : value;
				}
			}
		}
	})
})();