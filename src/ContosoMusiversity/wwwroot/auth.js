var authService;
(function () {
    // Get the authentication information from the server and use it to configure MSAL.js.
    fetch('api/configuration/authentication').then(response => {
        if (!response.ok) {
            console.error('Could not get authentication information: Status ' + response.status);
        }
        else {
            response.json().then(authenticationConfiguration => {
                var msalConfig = {
                    auth: {
                        clientId: authenticationConfiguration.clientId,
                        authority: authenticationConfiguration.authority
                    }
                };
                var msalClientApplication = new msal.PublicClientApplication(msalConfig);
                var scopes = [authenticationConfiguration.clientId + '/.default'];

                authService = {
                    login: function () {
                        return msalClientApplication.loginPopup({ scopes: scopes })
                            .then(loginResponse => {
                                return loginResponse.account;
                            });
                    },
                    getAccessToken: function () {
                        return msalClientApplication.acquireTokenSilent({ scopes: scopes, account: msalClientApplication.getAllAccounts()[0] }).then(accessTokenResponse => {
                            return accessTokenResponse.accessToken;
                        });
                    }
                };
            });
        }
    });
})();