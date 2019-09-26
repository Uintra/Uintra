import ajax from "./../../Core/Content/scripts/Ajax";
import confirm from "./../../Core/Controls/Confirm/Confirm";

var alertify = require('alertifyjs/build/alertify.min');

require('alertifyjs/build/css/alertify.min.css');
require("./user-list.css");

const searchBoxElement = $(".js-user-list-filter");
const searchButton = $(".js-search-button");
const table = $(".js-user-list-table");
const tableBody = $(".js-user-list-table .js-tbody");
const button = $(".js-user-list-button"); // Load More Button
const displayedRows = $(".js-user-list-row");
const emptyResultLabel = $(".js-user-list-empty-result");
const openModalPageListener = $(".js-open-search-modal-page");
const searchActivationDelay = 256;

const routePrefix = '/umbraco/surface/UserList/';

var url = {
    GET_USERS: routePrefix + 'GetUsers',
    EXCLUDE_USER_FROM_GROUP: routePrefix + 'ExcludeUserFromGroup',
    TOGGLE_ADMIN_RIGHTS: routePrefix + 'Assign',
    INVITE_USER: routePrefix + 'InviteMember',
    GET_NOT_INVITED_USERS: routePrefix + 'GetNotInvitedUsers'
};

/**
 * Search values initiates when modal page opens.
 */
var SEARCH_USER_ELEMENT; 
var SEARCH_USER_RESULT_ELEMENT;
var INVITE_USER_ELEMENT;


let lastRequestClassName = "last";

let searchTimeout;
let request;
let displayedAmount;
let amountPerRequest;
let confirmTitle;
let confirmText;

let controller = {
    init: function() {

        if (tableBody.length === 0) return;

        init();
        button.click(onButtonClick);

        searchBoxElement.on("input", onSearchStringChanged); 
        searchBoxElement.on("keypress", onKeyPress);

        var inviteUserSearch = {
            keyPress: (e) => {
                if (e.which === 13 || e.KeyCode === 13 || e.charCode === 13) {
                    search(SEARCH_USER_ELEMENT.val());
                    eventPreprocessing(e);
                }
            },
            searchStringChanged: () => {
                clearTimeout(searchTimeout);
                const searchString = SEARCH_USER_ELEMENT.val();

                if (searchString.length === 0) {
                    SEARCH_USER_RESULT_ELEMENT.children().remove();
                    return;
                }

                searchTimeout = setTimeout(() => inviteUserSearch.searchUser(searchString), searchActivationDelay);
            },
            searchUser: (searchString) => {
                request.skip = 0;
                request.take = displayedAmount;
                request.text = searchString;
                request.isInvite = true;
                ajax.post(url.GET_NOT_INVITED_USERS, request)
                    .then(result => {
                        var rows = $(result.data).filter("div");
                        SEARCH_USER_RESULT_ELEMENT.children().remove();
                        SEARCH_USER_RESULT_ELEMENT.append(rows);
                        INVITE_USER_ELEMENT = $(".js-user-invite-member");
                        INVITE_USER_ELEMENT.on("click", inviteUserSearch.inviteUser);
                        
                        updateUI(rows);
                    });
            },
            inviteUser: (e) => {

                var row = $(e.target).closest(".js-user-list-row");
                var groupId = row.data("group-id");
                var userId = row.data("id");
                ajax.post(url.INVITE_USER, buildGroupMemberModel(userId, groupId))
                    .then(
                        function (resolve) {
                        
                        },
                        function (reject) {
                        
                        }
                    );
            }
        };

        searchButton.click(onSearchClick);
        addDetailsHandler(displayedRows);
        addRemoveUserFromGroupHandler(displayedRows);
        toggleAdminRights(displayedRows);
        openSearchModalPage(openModalPageListener);

        function init() {
            request = window.userListConfig.request;
            displayedAmount = window.userListConfig.displayedAmount;
            amountPerRequest = window.userListConfig.amountPerRequest;
            request.groupId = new URL(window.location.href).searchParams.get("groupId");
            confirmTitle = table.data("title");
            confirmText = table.data("text");
        }

        function onSearchClick(e) {
            const query = searchBoxElement.val();
            if (query) {
                search(query);
            }
        }

        function onKeyPress(e) {
            if (e.which === 13 || e.KeyCode === 13 || e.charCode === 13) {
                search(searchBoxElement.val());
                eventPreprocessing(e);
            }
        }

        function onButtonClick(event) {
            request.skip = tableBody.children("div").length;
            request.take = amountPerRequest;

            ajax.post(url.GET_USERS, request)
                .then(result => {
                    var rows = $(result.data).filter("div");
                    tableBody.append(rows);
                    addDetailsHandler(rows);
                    addRemoveUserFromGroupHandler(rows);
                    updateUI(rows);
                });
        }

        function onSearchStringChanged() {
            clearTimeout(searchTimeout);
            const searchString = searchBoxElement.val();
            searchTimeout = setTimeout(() => search(searchString), searchActivationDelay);
        }

        function search(searchString) {
            request.skip = 0;
            request.take = displayedAmount;
            request.text = searchString;
            ajax.post(url.GET_USERS, request)
                .then(result => {
                    var rows = $(result.data).filter("div");
                    tableBody.children().remove();
                    tableBody.append(rows);
                    addDetailsHandler(rows);
                    addRemoveUserFromGroupHandler(rows);
                    updateUI(rows);
                });
        }

        function updateUI(rows) {
            if (tableBody.children("div").length === 0) emptyResultLabel.show();
            else emptyResultLabel.hide();
            if (rows.hasClass(lastRequestClassName) || rows.length === 0) button.hide();
            else button.show();
        }

        function addDetailsHandler(rows) {
            rows.click(function() {
                var profileUrl = $(this).data("profile");
                location.href = profileUrl;
            });
        }

        function addRemoveUserFromGroupHandler(rows) {
            var deleteButtons = rows.find(".js-user-list-delete");
            deleteButtons.click(function(e) {
                eventPreprocessing(e);

                confirm.showConfirm(confirmTitle,
                    confirmText,
                    () => {
                        var row = $(this).closest(".js-user-list-row");
                        var groupId = row.data("group-id");
                        var userId = row.data("id");
                        ajax.post(url.EXCLUDE_USER_FROM_GROUP, { groupId: groupId, userId: userId })
                            .then(function(result) {
                                if (result.data) {
                                    row.remove();
                                    request.skip = request.skip - 1;
                                }
                            });
                    },
                    () => {},
                    confirm.defaultSettings);
            });
        }

        function toggleAdminRights(rows) {
            var checkboxes = rows.find(".js-user-list-toggle-admin-rights");
            checkboxes.click(function(e) {
                eventPreprocessing(e);

                var row = $(this).closest(".js-user-list-row");
                var groupId = row.data("group-id");
                var userId = row.data("id");

                ajax.put(url.TOGGLE_ADMIN_RIGHTS, { groupId: groupId, memberId: userId })
                    .then(function(result) {
                        if (result.status === 200) {
                            var checkbox = $(row[0]).find(".js-user-list-toggle-admin-rights");
                            if (checkbox.is(':checked')) {
                                $(row[0]).find("span").last().text("Group Member");
                                checkbox.prop('checked', false);
                            } else {
                                $(row[0]).find("span").last().text("Group Admin");
                                checkbox.prop('checked', true);
                            }
                        }
                    });
            });
        }

        function eventPreprocessing(e) {
            e.preventDefault();
            e.stopPropagation();
        }

        function getParameterByName(name, url) {
            if (!url) url = window.location.href;
            name = name.replace(/[\[\]]/g, '\\$&');
            var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, ' '));
        }
        
        function openSearchModalPage(openSearchModalButton) {
            openSearchModalButton.click(
                function(event) {
                    eventPreprocessing(event);
                    alertify.alert(
                        'Users Search', 
                        '<input type="text" name="search" class="form-control js-user-search" placeholder="Enter users name" />' +
                        '<ul class="list-group js-user-search-result"></ul>', 
                        function() { alertify.success('Ok'); }
                    );
                    SEARCH_USER_ELEMENT = $(".js-user-search");
                    SEARCH_USER_ELEMENT.on("input", inviteUserSearch.searchStringChanged);
                    SEARCH_USER_RESULT_ELEMENT = $(".js-user-search-result");
                    SEARCH_USER_RESULT_ELEMENT.on("keypress", inviteUserSearch.keyPress);
                }
            );
        }

        function buildGroupMemberModel(memberId, groupId) {
            return { memberId: memberId, groupId: groupId };
        }
    }
};

export default controller;