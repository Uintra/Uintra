export class LoginModel {
    constructor(
        login: string,
        password: string,
        clientTimeZoneId: string,
        returnUrl: string) {
        this.login = login;
        this.password = password;
        this.clientTimeZoneId = clientTimeZoneId;
        this.returnUrl = returnUrl;
    }

    private login: string;
    private password: string;
    private returnUrl: string;
    private clientTimeZoneId: string;
}
