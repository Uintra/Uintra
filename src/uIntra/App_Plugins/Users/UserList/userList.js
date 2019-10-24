import ajax from './../../Core/Content/scripts/Ajax';
import confirm from './../../Core/Controls/Confirm/Confirm';
import alertify from 'alertifyjs/build/alertify.min';
import 'alertifyjs/build/css/alertify.min.css';
import './user-list.css';

var marker = {
    ROWS: '.js-user-list-row',
    LIST_FILTER: '.js-user-list-filter',
    LIST_TABLE: '.js-user-list-table',
    TABLE_BODY: '.js-user-list-table .js-tbody',
    LIST_BUTTON: '.js-user-list-button',
    LIST_EMPTY_RESULT: '.js-user-list-empty-result',
    SEARCH_BUTTON: '.js-search-button',
    OPEN_MODAL_PAGE: '.js-open-search-modal-page',
    INVITE_MEMBER: '.js-user-invite-member',
    DELETE_MEMBER: '.js-user-list-delete',
    TOGGLE_ADMIN_RIGHTS: '.js-user-list-toggle-admin-rights',
    INVITE_SEARCH: '.js-user-search',
    INVITE_SEARCH_RESULT: '.js-user-search-result',
    INVITE_NOT_FOUND_RESULT: '.js-invite-user-list-empty-result',
    ALERTIFY: {
        SELECTOR: '.alertify',
        STYLE: {
            INVITE_POPUP: 'alertify--custom'
        },
        ACTIONS: {
            TOGGLE_STYLES: function () {
                $(marker.ALERTIFY.SELECTOR).hasClass(marker.ALERTIFY.STYLE.INVITE_POPUP)
                    ? $(marker.ALERTIFY.SELECTOR).removeClass(marker.ALERTIFY.STYLE.INVITE_POPUP)
                    : $(marker.ALERTIFY.SELECTOR).addClass(marker.ALERTIFY.STYLE.INVITE_POPUP);
            }
        }
    },
    SELECT_CONTAINER: '.select2-container'
};

const SEARCH_MEMBER_INPUT = $(marker.LIST_FILTER);
const TABLE = $(marker.LIST_TABLE);
const TABLE_BODY = $(marker.TABLE_BODY);
const LOAD_MORE_BUTTON = $(marker.LIST_BUTTON);
let DISPLAYED_ROWS;
const EMPTY_RESULT_LABEL = $(marker.LIST_EMPTY_RESULT);
const MEMBER_SEARCH_SUBMIT_BUTTON = $(marker.SEARCH_BUTTON);
const OPEN_INVITE_MODAL_ELEMENT = $(marker.OPEN_MODAL_PAGE);
const SEARCH_ACTIVATION_DELAY = 256;
let INVITE_LABEL_NOT_FOUND;

var hook = {
    rows: function () {
        if (DISPLAYED_ROWS) {

            var newest = $(marker.ROWS);

            if (DISPLAYED_ROWS !== newest) DISPLAYED_ROWS = newest;

            return DISPLAYED_ROWS;
        } else {
            DISPLAYED_ROWS = $(marker.ROWS);

            return DISPLAYED_ROWS;
        }
    }
};

const ROUTE_PREFIX = '/umbraco/surface/UserList/';

var routes = {
    GET_USERS: ROUTE_PREFIX + 'GetUsers',
    EXCLUDE_USER_FROM_GROUP: ROUTE_PREFIX + 'ExcludeUserFromGroup',
    TOGGLE_ADMIN_RIGHTS: ROUTE_PREFIX + 'Assign',
    INVITE_USER: ROUTE_PREFIX + 'InviteMember',
    GET_NOT_INVITED_USERS: ROUTE_PREFIX + 'ForInvitation'
};

/**
 * Search values initiates when modal page opens.
 */
let SEARCH_USER_ELEMENT;
let SEARCH_USER_RESULT_ELEMENT;
let INVITE_USER_ELEMENT;
let ROW_TO_DELETE;

let lastRequestClassName = 'last';
let searchTimeout;
let request;
let displayedAmount;
let amountPerRequest;
let confirmTitle;
let confirmText;
var buttonToDeleteMember = null;

let controller = {
    init: function () {

        if (TABLE_BODY.length === 0) return;

        init();

        function init() {
            hook.rows();
            request = window.userListConfig.request;
            displayedAmount = window.userListConfig.displayedAmount;
            amountPerRequest = window.userListConfig.amountPerRequest;
            request.groupId = new URL(window.location.href).searchParams.get('groupId');
            confirmTitle = TABLE.data('title');
            confirmText = TABLE.data('text');
            LOAD_MORE_BUTTON.click(onButtonClick);
            SEARCH_MEMBER_INPUT.on('input', onSearchStringChanged);
            SEARCH_MEMBER_INPUT.on('keypress', onKeyPress);
            MEMBER_SEARCH_SUBMIT_BUTTON.click(onSearchClick);
            addRemoveUserFromGroupHandler(hook.rows());
            toggleAdminRights(hook.rows());
            addDetailsHandler(hook.rows());
            openSearchModalPage(OPEN_INVITE_MODAL_ELEMENT);
        }

        var invite = {
            keyPress: function (e) {
                if (shared.isEnterClicked(e)) return;

                invite.searchUser(SEARCH_USER_ELEMENT.val());
            },
            searchStringChanged: function () {
                clearTimeout(searchTimeout);
                const searchString = SEARCH_USER_ELEMENT.val();

                if (searchString.length === 0) {
                    invite.updateUI(SEARCH_USER_RESULT_ELEMENT.children());
                    SEARCH_USER_RESULT_ELEMENT.children().remove();

                    return;
                }

                searchTimeout = setTimeout(function () { invite.searchUser(searchString); }, SEARCH_ACTIVATION_DELAY);
            },
            searchUser: function (searchString) {
                invite.preSearchUser(searchString);
                ajax.post(routes.GET_NOT_INVITED_USERS, request)
                    .then(function (result) {
                        var rows = $(result.data).filter('div');
                        SEARCH_USER_RESULT_ELEMENT.children().remove();
                        SEARCH_USER_RESULT_ELEMENT.append(rows);
                        INVITE_USER_ELEMENT = $(marker.INVITE_MEMBER);
                        INVITE_USER_ELEMENT.on('click', invite.inviteUser);
                        invite.updateUI(rows);
                    });
            },
            inviteUser: function (e) {
                var row = $(e.target).closest(marker.ROWS);
                var groupId = row.data('group-id');
                var userId = row.data('id');
                invite.disableInviteButton(row);
                ajax.post(routes.INVITE_USER, shared.buildGroupMemberModel(userId, groupId));
            },
            disableInviteButton: function (row) {
                $(row).find(marker.INVITE_MEMBER).prop('disabled', true);
            },
            preSearchUser: function (searchString) {
                request.skip = 0;
                request.take = displayedAmount;
                request.text = searchString;
                request.isInvite = true;
            },
            updateUI: function () {
                var searchResultLength = $(marker.INVITE_SEARCH_RESULT).children('div').length;
                if (searchResultLength === 0) {
                    INVITE_LABEL_NOT_FOUND.show();
                } else {
                    INVITE_LABEL_NOT_FOUND.hide();
                }

                var inputLength = SEARCH_USER_ELEMENT.val().length;

                if (inputLength === 0 || searchResultLength !== 0) {
                    INVITE_LABEL_NOT_FOUND.hide();
                } else {
                    INVITE_LABEL_NOT_FOUND.show();
                }
            }
        };

        function onSearchClick(e) {
            const query = SEARCH_MEMBER_INPUT.val();
            if (query) {
                search(query);
            }
        }

        function onKeyPress(e) {
            if (shared.isEnterClicked(e)) return;

            search(SEARCH_MEMBER_INPUT.val());
        }

        function onButtonClick(event) {
            request.skip = TABLE_BODY.children('div').length;
            request.take = amountPerRequest;

            ajax.post(routes.GET_USERS, request)
                .then(function (result) {
                    var rows = $(result.data).filter('div');
                    TABLE_BODY.append(rows);
                    addDetailsHandler(rows);
                    addRemoveUserFromGroupHandler(rows);
                    updateUI(rows);
                    toggleAdminRights(hook.rows());
                });
        }

        function onSearchStringChanged() {
            clearTimeout(searchTimeout);
            const searchString = SEARCH_MEMBER_INPUT.val();
            searchTimeout = setTimeout(function () { search(searchString); }, SEARCH_ACTIVATION_DELAY);
        }

        function search(searchString) {
            preSearch(searchString);
            ajax.post(routes.GET_USERS, request)
                .then(function (result) {
                    var rows = $(result.data).filter('div');
                    ROW_TO_DELETE = $(marker.ROWS);
                    $(ROW_TO_DELETE).remove();
                    TABLE_BODY.append(rows);
                    addDetailsHandler(rows);
                    toggleAdminRights(rows);
                    addRemoveUserFromGroupHandler(rows);
                    updateUI(rows);
                });
        }

        function preSearch(searchString) {
            request.skip = 0;
            request.take = displayedAmount;
            request.text = searchString;
            request.isInvite = false;
        }

        function updateUI(loadedRows) {

            var length = TABLE_BODY.children('div').length;

            var isLastRequest = loadedRows.hasClass(lastRequestClassName);

            if (length === 1) {
                EMPTY_RESULT_LABEL.show();
            } else {
                EMPTY_RESULT_LABEL.hide();
            }
            if (length === 1  || isLastRequest) {
                LOAD_MORE_BUTTON.hide();
                
            } else {
                LOAD_MORE_BUTTON.show();
            }
        }

        function addDetailsHandler(rows) {
            rows.click(function () {
                var profileUrl = $(this).data('profile');
                location.href = profileUrl;
            });
        }

        function addRemoveUserFromGroupHandler(rows) {
            var deleteButtons = rows.find(marker.DELETE_MEMBER);
            deleteButtons.click(function (e) {
                shared.eventSuppress(e);
                buttonToDeleteMember = e.target;
                confirm.showConfirm(confirmTitle,
                    confirmText,
                    function () {
                        var row = $(buttonToDeleteMember).closest(marker.ROWS);
                        var groupId = row.data('group-id');
                        var userId = row.data('id');
                        $(buttonToDeleteMember).prop('disabled', true);
                        ajax.post(routes.EXCLUDE_USER_FROM_GROUP, { groupId: groupId, userId: userId })
                            .then(function (result) {
                                if (result.data) {
                                    row.remove();
                                    request.skip = request.skip - 1;
                                    buttonToDeleteMember = null;
                                }
                            }, function () {
                                buttonToDeleteMember = null;
                            });
                    },
                    function () {
                        $(buttonToDeleteMember).prop('disabled', false);
                        buttonToDeleteMember = null;
                    },
                    confirm.defaultSettings);
            });
        }

        function toggleAdminRights(rows) {
            rows.find(marker.TOGGLE_ADMIN_RIGHTS).select2({ minimumResultsForSearch: -1 });

            var SELECT_ELEMENT = $(SELECT_CONTAINER);

            SELECT_ELEMENT.click(function (e) {
                shared.eventSuppress(e);
            });
            SELECT_ELEMENT.change(function (e) { 
                shared.eventSuppress(e);
                var row = $(this).closest(marker.ROWS);
                var groupId = row.data('group-id');
                var userId = row.data('id');

                ajax.put(routes.TOGGLE_ADMIN_RIGHTS, { groupId: groupId, memberId: userId })
                    .then(function (result) { });
            });
        }

        function openSearchModalPage(openSearchModalButton) {
            openSearchModalButton.click(
                function (e) {
                    shared.eventSuppress(e);
                    alertify.alert(
                        'People',
                        '<h4 class="user-search__subtitle">Search people</h4>' +
                        '<form class="user-search__form">' +
                        '<input type="text" name="search" class="user-search__input js-user-search" placeholder="Search for name, phone number, e-mail or anything else" />' +
                        '<button class="user-search__button js-search-button" type="button">' +
                        '<span class="icon-search">' +
                        '<svg class="svg-icon" viewBox="0 0 32 32" width="30px" height="30px">' +
                        '<use xlink: href="#icon-search" x="0" y="0"></use>' +
                        '</svg>' +
                        '</span > ' +
                        '</button > ' +
                        '</form >' +
                        '<ul class="list-group js-user-search-result"></ul>' +
                        '<div class="js-invite-user-list-empty-result" style="display:none;">No results, try other keywords</div>',
                        function () { }
                    ).set({ transition: 'fade', movable: false })
                    .set({
                        onclosing: function () {
                            marker.ALERTIFY.ACTIONS.TOGGLE_STYLES();
                            search('');
                        }
                    });
                    postOpenSearchModalPage();
                    INVITE_LABEL_NOT_FOUND = $(marker.INVITE_NOT_FOUND_RESULT);
                }
            );
        }

        function postOpenSearchModalPage() {
            SEARCH_USER_ELEMENT = $(marker.INVITE_SEARCH);
            SEARCH_USER_ELEMENT.on('input', invite.searchStringChanged);
            SEARCH_USER_ELEMENT.val('');
            SEARCH_USER_ELEMENT.on('keypress', invite.keyPress);
            SEARCH_USER_RESULT_ELEMENT = $(marker.INVITE_SEARCH_RESULT);
            SEARCH_USER_RESULT_ELEMENT.children().remove();
            marker.ALERTIFY.ACTIONS.TOGGLE_STYLES();
        }

        var shared = {
            isEnterClicked: function (e) {
                if (e.which === 13 || e.KeyCode === 13 || e.charCode === 13) {
                    shared.eventSuppress(e);

                    return true;
                }

                return false;
            },
            eventSuppress: function (e) {
                e.preventDefault();
                e.stopPropagation();
            },
            buildGroupMemberModel: function (memberId, groupId) {
                return {
                    memberId: memberId,
                    groupId: groupId
                };
            }
        };
    }
};

export default controller;