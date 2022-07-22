(function () {
    function showApiError(response) {
        var errorMessage = document.getElementById('errorMessage');
        response.json().then(errorBody => {
            errorMessage.innerText = errorBody.title;
        }).catch(() => {
            errorMessage.innerText = 'Could not call the API: Status ' + response.status;
        });
        errorMessage.style.display = 'block';
        document.getElementById('responsePanel').style.display = 'none';
    }

    var logInButton = document.getElementById('logIn');
    logInButton.addEventListener('click', () => {
        authService.login().then(account => {
            document.getElementById('userName').innerText = `Welcome, ${account.name}!`;
            logInButton.style.display = 'none';
            document.getElementById('verifiedCredentialPanel').style.display = 'block';
        }).catch(error => {
            console.log('Could not login: ' + error);
        });
    });

    document.getElementById('issueCredential').addEventListener('click', () => {
        authService.getAccessToken().then(accessToken => {
            // Send an issuance request.
            var data = {
                'usePinCode': document.getElementById('usePinCode').checked
            };
            fetch('api/issuance/request', {
                method: 'POST',
                headers: {
                    'Authorization': 'Bearer ' + accessToken,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            }).then(response => {
                if (!response.ok) {
                    // Something went wrong while sending the presentation request, show an error message.
                    showApiError(response);
                } else {
                    // The issuance request was successful, show the response.
                    response.json().then(requestResponseBody => {
                        var statusMessage = document.getElementById('statusMessage');
                        var pinCodeMessage = '';
                        if (requestResponseBody.pinValue) {
                            pinCodeMessage = ` Use PIN code <kbd>${requestResponseBody.pinValue}</kbd>.`;
                        }
                        statusMessage.innerHTML = "Waiting..." + pinCodeMessage;
                        document.getElementById('requestId').innerText = requestResponseBody.requestId;
                        document.getElementById('deepLink').href = requestResponseBody.url;
                        document.getElementById('qrCodeImage').src = requestResponseBody.qrCode;
                        document.getElementById('errorMessage').style.display = 'none';
                        document.getElementById('responsePanel').style.display = 'block';
                        document.getElementById('scanPanel').style.display = 'block';

                        // Start polling the status of the request, as callback messages should arrive
                        // back from the Verifiable Credentials Service.
                        var checkStatus = setInterval(() => {
                            authService.getAccessToken().then(accessToken => {
                                fetch('api/issuance/status?requestId=' + requestResponseBody.requestId, {
                                    headers: {
                                        'Authorization': 'Bearer ' + accessToken
                                    }
                                }
                                ).then(response => {
                                    if (!response.ok) {
                                        // Something went wrong while polling, show an error message.
                                        showApiError(response);
                                    } else {
                                        response.json().then(statusResponseBody => {
                                            // Check the status code.
                                            if (statusResponseBody.status == 'pending') {
                                                statusMessage.innerHTML = 'Waiting...' + pinCodeMessage;
                                            } else if (statusResponseBody.status == 'request_retrieved') {
                                                // The QR code was scanned, hide it.
                                                document.getElementById('scanPanel').style.display = 'none';
                                                statusMessage.innerHTML = 'Please accept your verified credential.' + pinCodeMessage;
                                            } else if (statusResponseBody.status == 'issuance_error') {
                                                // An error occurred.
                                                statusMessage.innerText = `Something went wrong: ${statusResponseBody.message}.`;
                                            } else if (statusResponseBody.status == 'issuance_successful') {
                                                // Stop polling for changes.
                                                clearInterval(checkStatus);

                                                // The verified credential was issued.
                                                statusMessage.innerText = 'Your verified credential was successfully issued!';
                                            }
                                        })
                                    }
                                }).catch(error => {
                                    console.log(error.message);
                                });
                            });
                        }, 1000);
                    })
                };
            }).catch(error => {
                console.log(error.message);
            })
        }).catch(error => {
            console.log("Could not acquire access token: " + error);
        });
    });
})();