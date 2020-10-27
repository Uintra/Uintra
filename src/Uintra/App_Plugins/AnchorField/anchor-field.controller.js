function anchorFieldController(
    $scope,
    editorState,
    notificationsService) {
    var vm = this;
    var culture = param("cculture") || param("mculture") || "";
    vm.pattern = "^[A-Za-z0-9]+$";

    if ($scope.model.value) {
        loadLinks();
    }

    vm.valueChanged = function () {
        vm.showValidationMessage = $scope.model.value === undefined;
        if ($scope.model.value) {
            loadLinks();
        }
        else {
            vm.links = [];
        }
    };

    vm.copyLink = function (link) {
        var el = document.createElement('textarea');
        el.value = link;
        document.body.appendChild(el);
        el.select();
        document.execCommand('copy');
        document.body.removeChild(el);
        notificationsService.success(`Url copied to clipboard ${link}`);
    };

    function loadLinks() {
        vm.links = getLinks();
    }

    function getLinks() {
        var links = editorState.current.urls.filter(function (i) {
            return i.isUrl && (!culture || i.culture.toLowerCase() === culture.toLowerCase());
        });

        return links.map(function (i) { return i.text + "#" + $scope.model.value; });
    }

    function param(name) {
        var arr = (window.location.href.split(name + '=')[1] || '').split('&');
        return arr && arr.length > 0 ? arr[0] : "";
    }
}
angular.module("umbraco").controller("UBaseline.AnchorFieldController", anchorFieldController);