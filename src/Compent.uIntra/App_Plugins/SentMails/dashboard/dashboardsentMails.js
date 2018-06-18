$(function () {

    function loadInitialData() {

        ajax("GetInitialDataSettingsDashboard", {}, null, function (response) {

            var $sentMailsCreate = $("#sent-mails-create"),
                $sentMailsDelete = $("#sent-mails-delete"),
                $sentMailsCreateSmtp = $("#sent-mails-create-smtp"),
                $sentMailsFixDb = $("#sent-mails-fix-db"),
                $sentMailsDeleteMails = $("#sent-mails-delete-mails");

            unDisable($sentMailsCreate);
            unDisable($sentMailsDelete);
            unDisable($sentMailsCreateSmtp);
            unDisable($sentMailsFixDb);
            unDisable($sentMailsDeleteMails);

            if (response.isMailTypeExists) {
                disable($sentMailsCreate);
            } else {
                disable($sentMailsDelete);
            }

            if (!response.isSmtpOrColumnsSettingsEmpty) {
                disable($sentMailsCreateSmtp);
            }

            if (!response.isSomeColumnsEmpty) {
                disable($sentMailsFixDb);
            }

            if (!response.isMailsExists) {
                disable($sentMailsDeleteMails);
            }

            setPrimaryClassIfNotDisabled($sentMailsCreate);
            setPrimaryClassIfNotDisabled($sentMailsDelete);
            setPrimaryClassIfNotDisabled($sentMailsCreateSmtp);
            setPrimaryClassIfNotDisabled($sentMailsDeleteMails);

            setWarningClassIfNotDisabled($sentMailsFixDb);

        });
    }

    loadInitialData();

    $("#sent-mails-create").on("click", function () {
        var $sentMailsCreate = $("#sent-mails-create");

        ajax("CreateDocTypes", {
            parentIdOrAlias: $(this).closest(".sentMails").find(".mail-template-id-or-alias").val()
        }, $sentMailsCreate, function (response) {

            if (response.result) {
                disable($sentMailsCreate);
                setSuccessClass($sentMailsCreate);

                var $sentMailsDelete = $("#sent-mails-delete");
                unDisable($sentMailsDelete);
                setPrimaryClassIfNotDisabled($sentMailsDelete);
            } else {

                setDangerClass($sentMailsCreate);
            }

            showResultMessage(response.messages);
            $sentMailsCreate.show();
        });
    });

    $("#sent-mails-delete").on("click", function () {
        var $sentMailsDelete = $(this);

        ajax("DeleteDocTypes", {}, $sentMailsDelete, function (response) {

            if (response.result) {
                disable($sentMailsDelete);
                setSuccessClass($sentMailsDelete);

                var $sentMailsCreate = $("#sent-mails-create");
                unDisable($sentMailsCreate);
                setPrimaryClassIfNotDisabled($sentMailsCreate);
            } else {

                setDangerClass($sentMailsDelete);
            }

            showResultMessage(response.messages);
            $sentMailsDelete.show();
        });
    });

    $("#sent-mails-create-smtp").on("click", function () {
        var $sentMailsCreateSmtp = $(this);

        ajax("CreateSmtpDefaultSettings", {}, $sentMailsCreateSmtp, function (response) {

            if (response.result) {
                disable($sentMailsCreateSmtp);
                setSuccessClass($sentMailsCreateSmtp);
            } else {
                setDangerClass($sentMailsCreateSmtp);
            }

            showResultMessage(response.result);
            $sentMailsCreateSmtp.show();
        });
    });

    $("#sent-mails-fix-db").on("click", function () {
        var $sentmailsFixDb = $(this);

        ajax("FixDb", {}, $sentmailsFixDb, function (response) {

            if (response.result) {
                disable($sentmailsFixDb);
                setSuccessClass($sentmailsFixDb);
            } else {
                setDangerClass($sentmailsFixDb);
            }

            showResultMessage(response.result);

            $sentmailsFixDb.show();
        });
    });

    $("#sent-mails-delete-mails").on("click", function () {
        var $sentmailsDeleteMails = $(this);

        ajax("DeleteMails", {}, $sentmailsDeleteMails, function (response) {

            if (response.result) {
                setSuccessClass($sentmailsDeleteMails);
            } else {
                setDangerClass($sentmailsDeleteMails);
            }

            showResultMessage(response.result);

            $sentmailsDeleteMails.show();
        });
    });

    $("#sent-mails-send-test-email").on("click", function () {
        var $sentmailsSendTestEmail = $(this);

        ajax("SendTestEmail", {}, $sentmailsSendTestEmail, function (response) {

            if (response.result) {
                setSuccessClass($sentmailsSendTestEmail);
            } else {
                setDangerClass($sentmailsSendTestEmail);
            }

            showResultMessage(response.message);

            $sentmailsSendTestEmail.show();
        });
    });

    function disable($this) {
        $this.prop('disabled', true);
    }

    function unDisable($this) {
        $this.prop('disabled', false);
    }

    function setPrimaryClassIfNotDisabled($this) {
        if (!$this.prop('disabled')) {
            setPrimaryClass($this);
        }
    }

    function setWarningClassIfNotDisabled($this) {
        if (!$this.prop('disabled')) {
            setWarningClass($this);
        }
    }

    function setSuccessClass($this) {
        $this.removeClass("btn-primary-custom").removeClass("btn-warning-custom").removeClass("btn-info-custom").addClass("btn-success-custom");
    }

    function setPrimaryClass($this) {
        $this.removeClass("btn-info-custom").addClass("btn-primary-custom");
    }

    function setDangerClass($this) {
        $this.removeClass("btn-info-custom").removeClass("btn-primary-custom").addClass("btn-danger-custom");
    }

    function setWarningClass($this) {
        $this.removeClass("btn-info-custom").removeClass("btn-primary-custom").addClass("btn-warning-custom");
    }

    function showResultMessage(message) {
        $("#result-messages").html(message);
    }

    function ajax(action, data, $currentItem, successCallback) {
        var url = "/umbraco/backoffice/api/SentMailsDashboardApi/" + action;

        if ($currentItem != null) {
            $currentItem.hide();
        }

        $.ajax({
            url: url,
            data: data,
            type: "POST"
        }).done(successCallback)
          .fail(function (response) {
              if ($currentItem != null) {
                  setDangerClass($currentItem);
                  $currentItem.show();
              }
              showResultMessage(response.responseText.substr(response.responseText.indexOf("<body"), response.responseText.indexOf("</body>")));
          });
    }
});