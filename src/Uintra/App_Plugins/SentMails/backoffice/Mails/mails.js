angular.module('umbraco').controller('mailsController', ['$scope', '$routeParams', 'sentMailsService',
    function ($scope, $routeParams, sentMailsService) {
        //Enum for determine current node
        var nodeEnum = Object.freeze({ SENT: 'SENT', OUTPUT: 'OUTPUT' });

        $scope.currentNode = null;
        $scope.isSaveBtnDisable = false;

        if (typeof $routeParams.id === 'string' && $routeParams.id === 'edit-sent') {
            $scope.currentNode = nodeEnum.SENT;
            getMails();
        } else if (typeof $routeParams.id === 'string' && $routeParams.id === 'edit-outbox') {
            $scope.currentNode = nodeEnum.OUTPUT;
            getMails();
        }

        function getMails() {

            var mailType;

            if ($scope.currentNode === nodeEnum.SENT) {
                mailType = true;
            }
            else {
                mailType = false;
            }
            //page - current page
            //pages - total pages
            //displayPerPage - items for one page
            //displayPages count button in paging system
            //displaybtn array paging button
            $scope.pagination = {
                page: 1,
                pages: 1,
                displayPerPage: 20,
                displayPages: 5,
                displaybtn: []
            };

            $scope.nextPage = function () {
                if ($scope.pagination.page < $scope.pagination.pages) {
                    $scope.pagination.page++;
                }
            };

            $scope.previousPage = function () {
                if ($scope.pagination.page > 1) {
                    $scope.pagination.page--;
                }
            };

            $scope.firstPage = function () {
                $scope.pagination.page = 1;
            };

            $scope.lastPage = function () {
                $scope.pagination.page = $scope.pagination.pages;
            };

            $scope.moveToCurrent = function (pageItem) {
                $scope.pagination.page = pageItem;
            };


            function sortNumber(a, b) {
                return a - b;
            }


            $scope.filters = {
                Subject: '',
                Body: '',
                TypeId: '',
                FromEmail: '',
                FromName: '',
                ToEmail: '',
                ToName: '',
                CcEmails: '',
                BccEmails: '',
                StartCreateDate: $scope.date,
                EndCreateDate: $scope.date,
                StartSentUtcDate: $scope.date,
                EndSentUtcDate: $scope.date,
                StartSentInFutureDate: $scope.date,
                EndSentInFutureDate: $scope.date
            };

            $scope.showFiltersValue = function () {
                getAll();
            };


            $scope.$watch('pagination.page', function () {
                getAll();
            });

            function getAll() {

                var data = {
                    IsSentMails: mailType,
                    CurrentPage: $scope.pagination.page,
                    DisplayPerPage: $scope.pagination.displayPerPage
                };

                sentMailsService.getAllSentMails($.extend(data, $scope.filters)).then(function (response) {
                    $scope.pagination.pages = Math.ceil(response.data.TotalItems / $scope.pagination.displayPerPage);
                    $scope.mails = response.data.SentMails;
                    $scope.columnSettings = response.data.ColumnSettings;
                    $scope.pagination.displaybtn = [];

                    var i;
                    var nearBtn = Math.floor($scope.pagination.displayPages / 2);


                    if ($scope.pagination.page > (1 + nearBtn) && $scope.pagination.page < ($scope.pagination.pages - nearBtn)) {
                        for (i = $scope.pagination.page; i >= $scope.pagination.page - nearBtn; i--) {
                            if ($scope.pagination.displaybtn.indexOf(i) <= 0) {
                                $scope.pagination.displaybtn.push(i);
                            }
                        }

                        for (i = $scope.pagination.page; i <= $scope.pagination.page + nearBtn; i++) {
                            if ($scope.pagination.displaybtn.indexOf(i) < 0) {
                                $scope.pagination.displaybtn.push(i);
                            }
                        }

                        $scope.pagination.displaybtn.sort(sortNumber);
                    }
                    else if ($scope.pagination.page == 1 || $scope.pagination.page < $scope.pagination.displayPages) {
                        for (i = 1; i <= $scope.pagination.displayPages; i++) {
                            if (i <= $scope.pagination.pages) {
                                $scope.pagination.displaybtn.push(i);
                            }
                        }
                    }
                    else if ($scope.pagination.page == $scope.pagination.pages || ($scope.pagination.page + $scope.pagination.displayPages) > $scope.pagination.pages) {
                        var temp = ($scope.pagination.page + $scope.pagination.displayPages);
                        for (i = $scope.pagination.pages; i <= temp; i--) {
                            $scope.pagination.displaybtn.push(i);
                            if ($scope.pagination.displaybtn.length == $scope.pagination.displayPages) {
                                break;
                            }
                        }

                        $scope.pagination.displaybtn.sort(sortNumber);
                    }

                }, function (error) {
                    console.log(error);
                });
            }
        }

        $scope.downloadAttachment = function (id) {
            window.open('/umbraco/backoffice/SentMails/SentMailsApi/DownloadAttachment/' + id, '_blank', '');
        };

        $scope.getFileExtension = function (extension) {
            if (extension === 'jpeg') {
                return 'jpg';
            }

            if (extension !== '7z' &&
                extension !== 'bmp' &&
                extension !== 'csv' &&
                extension !== 'doc' &&
                extension !== 'html' &&
                extension !== 'jpg' &&
                extension !== 'log' &&
                extension !== 'pdf' &&
                extension !== 'png' &&
                extension !== 'ppt' &&
                extension !== 'pptx' &&
                extension !== 'psd' &&
                extension !== 'rar' &&
                extension !== 'rtf' &&
                extension !== 'tiff' &&
                extension !== 'txt' &&
                extension !== 'xlsx' &&
                extension !== 'xml' &&
                extension !== 'zip') {
                return 'file';
            }

            return extension;
        };
        
        $scope.config = {
            enableTime: true,
            dateFormat: 'Y-m-d H:i',
            time_24hr: true
        };

        $scope.onChangeStartCreateDate = function(selected, date, instance) {

            var dateQueue = instance.selectedDates;

            var latestDate = dateQueue[instance.selectedDates.length - 1];

            $scope.filters.StartCreateDate = latestDate;
        };

        $scope.onChangeEndCreateDate = function(selected, date, instance) {

            var dateQueue = instance.selectedDates;

            var latestDate = dateQueue[instance.selectedDates.length - 1];

            $scope.filters.EndCreateDate = latestDate;
        };

        $scope.onChangeStartSentUtcDate = function(selected, date, instance) {

            var dateQueue = instance.selectedDates;

            var latestDate = dateQueue[instance.selectedDates.length - 1];

            $scope.filters.StartSentUtcDate = latestDate;
        };

        $scope.onChangeEndSentUtcDate = function(selected, date, instance) {

            var dateQueue = instance.selectedDates;

            var latestDate = dateQueue[instance.selectedDates.length - 1];

            $scope.filters.EndSentUtcDate = latestDate;
        };

        $scope.onChangeStartSentInFutureDate = function(selected, date, instance) {

            var dateQueue = instance.selectedDates;

            var latestDate = dateQueue[instance.selectedDates.length - 1];

            $scope.filters.StartSentInFutureDate = latestDate;
        };

        $scope.onChangeEndSentInFutureDate = function(selected, date, instance) {

            var dateQueue = instance.selectedDates;

            var latestDate = dateQueue[instance.selectedDates.length - 1];

            $scope.filters.EndSentInFutureDate = latestDate;
        };
    }
]);