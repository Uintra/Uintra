(function () {
    var controller = function ($scope) {

        $scope.activeQuestion = -1;
        $scope.rteConfigAlias = "faq-panel";

        $scope.addQuestion = function() {
            var emptyQuestion = {
                question: "",
                answer: ""
            }
            $scope.control.value.questions.push(emptyQuestion);
            $scope.activeQuestion = $scope.control.value.questions.length - 1;
        }

        $scope.changeActiveQuestion = function(index) {
            $scope.activeQuestion = index;
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