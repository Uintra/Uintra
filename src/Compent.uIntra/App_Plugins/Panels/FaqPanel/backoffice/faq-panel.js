(function () {
    var controller = function ($scope) {

        $scope.activeQuestion = -1;
        $scope.rteConfigAlias = "faq-panel";

        $scope.addQuestion = function () {
            var emptyQuestion = {
                question: "",
                answer: ""
            }
            $scope.control.value.questions.push(emptyQuestion);
            $scope.activeQuestion = $scope.control.value.questions.length - 1;
        }

        $scope.changeActiveQuestion = function (index) {
            $scope.activeQuestion = index;
        }

        $scope.overlay = {
            show: false,
            view: "/App_Plugins/Panels/FaqPanel/backoffice/overlay.html",
            title: "FAQ panel",
            close: function () {
                $scope.overlay.show = false;
                $scope.control.value = $scope.backupModel;
            },
            submit: function () {
                $scope.overlay.show = false;
            }
        }

        $scope.open = function () {
            $scope.overlay.show = true;
            $scope.control.value = $scope.control.value || getDefaultModel();
            $scope.backupModel = angular.copy($scope.control.value);
        }

        $scope.init = function (control) {
            $scope.control = control;
            if (!$scope.control.value) {
                $scope.control.value = {};
            }
            if (!$scope.control.value.questions) {
                $scope.control.value.questions = [];
            }
        }

        $scope.removeQuestion = function (question) {
            var index = $scope.control.value.questions.indexOf(question);
            $scope.control.value.questions.splice(index, 1);
        }
    }

    controller.$inject = ['$scope'];
    angular.module('umbraco').controller('faqPanelController', controller);
})();