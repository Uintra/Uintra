angular.module('umbraco').controller('mailsController', ['$scope', '$routeParams', 'sentMailsService',
    function ($scope, $routeParams, sentMailsService) {
        var self = this;
        //Enum for determine current node
        var nodeEnum = Object.freeze({ SENT: "SENT", OUTPUT: "OUTPUT" });

        self.currentNode = null;
        self.isSaveBtnDisable = false;

        if (typeof $routeParams.id === "string" && $routeParams.id == "edit-sent") {
            self.currentNode = nodeEnum.SENT;
            getMails();
        } else if (typeof $routeParams.id === "string" && $routeParams.id == "edit-outbox") {
            self.currentNode = nodeEnum.OUTPUT;
            getMails();
        }

        function getMails() {

            var mailType;
            if (self.currentNode == nodeEnum.SENT) {
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
            self.pagination = {
                page: 1,
                pages: 1,
                displayPerPage: 20,
                displayPages: 5,
                displaybtn: []
            };

            self.nextPage = function () {
                if (self.pagination.page < self.pagination.pages) {
                    self.pagination.page++;
                }
            };

            self.previousPage = function () {
                if (self.pagination.page > 1) {
                    self.pagination.page--;
                }
            };

            self.firstPage = function () {
                self.pagination.page = 1;
            };

            self.lastPage = function () {
                self.pagination.page = self.pagination.pages;
            };

            self.moveToCurrent = function (pageItem) {
                self.pagination.page = pageItem;
            };


            function sortNumber(a, b) {
                return a - b;
            }


            self.filters = {
                Subject: '',
                Body: '',
                TypeId: '',
                FromEmail: '',
                FromName: '',
                ToEmail: '',
                ToName: '',
                CcEmails: '',
                BccEmails: '',
                StartCreateDate: null,
                EndCreateDate: null,
                StartSentUtcDate: null,
                EndSentUtcDate: null,
                StartSentInFutureDate: null,
                EndSentInFutureDate: null
            };

            self.datepickerStartCreateDate = getDatePickerObj(self.filters.StartCreateDate);            
            self.datepickerEndCreateDate = getDatePickerObj(self.filters.EndCreateDate);
            self.datepickerStartSentUtcDate = getDatePickerObj(self.filters.StartSentUtcDate);
            self.datepickerEndSentUtcDate = getDatePickerObj(self.filters.EndSentUtcDate);
            self.datepickerStartSentInFutureDate = getDatePickerObj(self.filters.StartSentInFutureDate);
            self.datepickerEndSentInFutureDate = getDatePickerObj(self.filters.EndSentInFutureDate);

            self.showFiltersValue = function (startCreateDate, endCreateDate, startSentDate, endSentDate, startSentInFutureDate, endSentInFutureDate) {
                self.filters.StartCreateDate = startCreateDate;
                self.filters.EndCreateDate = endCreateDate;
                self.filters.StartSentUtcDate = startSentDate;
                self.filters.EndSentUtcDate = endSentDate;
                self.filters.StartSentInFutureDate = startSentInFutureDate;
                self.filters.EndSentInFutureDate = endSentInFutureDate;

                getAll();
            };


            $scope.$watch('mailsCtrl.pagination.page', function () {
                getAll();
            });

            function getAll() {

                var data = {
                    IsSentMails: mailType,
                    CurrentPage: self.pagination.page,
                    DisplayPerPage: self.pagination.displayPerPage
                };

                sentMailsService.getAllSentMails($.extend(data, self.filters)).then(function (response) {
                    self.pagination.pages = Math.ceil(response.data.TotalItems / self.pagination.displayPerPage);
                    self.mails = response.data.SentMails;
                    self.columnSettings = response.data.ColumnSettings;
                    self.pagination.displaybtn = [];

                    var i;
                    var nearBtn = Math.floor(self.pagination.displayPages / 2);


                    if (self.pagination.page > (1 + nearBtn) && self.pagination.page < (self.pagination.pages - nearBtn)) {
                        for (i = self.pagination.page; i >= self.pagination.page - nearBtn ; i--) {
                            if (self.pagination.displaybtn.indexOf(i) <= 0) {
                                self.pagination.displaybtn.push(i);
                            }
                        }

                        for (i = self.pagination.page; i <= self.pagination.page + nearBtn ; i++) {
                            if (self.pagination.displaybtn.indexOf(i) < 0) {
                                self.pagination.displaybtn.push(i);
                            }
                        }

                        self.pagination.displaybtn.sort(sortNumber);
                    }
                    else if (self.pagination.page == 1 || self.pagination.page < self.pagination.displayPages) {
                        for (i = 1; i <= self.pagination.displayPages; i++) {
                            if (i <= self.pagination.pages) {
                                self.pagination.displaybtn.push(i);
                            }
                        }
                    }
                    else if (self.pagination.page == self.pagination.pages || (self.pagination.page + self.pagination.displayPages) > self.pagination.pages) {
                        var temp = (self.pagination.page + self.pagination.displayPages);
                        for (i = self.pagination.pages; i <= temp; i--) {
                            self.pagination.displaybtn.push(i);
                            if (self.pagination.displaybtn.length == self.pagination.displayPages) {
                                break;
                            }
                        }

                        self.pagination.displaybtn.sort(sortNumber);
                    }

                }, function (error) {
                    console.log(error);
                });
            }
        }

        self.downloadAttachment = function (id) {
            window.open("/umbraco/backoffice/SentMails/SentMailsApi/DownloadAttachment/" + id, "_blank", "");
        }

        self.getFileExtension = function (extension) {
            if (extension === "jpeg") {
                return "jpg";
            }

            if (extension !== "7z" && extension !== "bmp" && extension !== "csv" && extension !== "doc" &&
                extension !== "html" && extension !== "jpg" && extension !== "log" && extension !== "pdf" &&
                extension !== "png" && extension !== "ppt" && extension !== "pptx" && extension !== "psd" &&
                extension !== "rar" && extension !== "rtf" && extension !== "tiff" && extension !== "txt" &&
                extension !== "xlsx" && extension !== "xml" && extension !== "zip") {
                return "file";
            }
            return extension;
        }

        function getDatePickerObj(objValue) {
            return  {
                view: 'datepicker',
                config: {
                    pickDate: true,
                    pickTime: true,
                    pick12HourFormat: false
                },
                value: objValue
            };
        }
    }
    ]);