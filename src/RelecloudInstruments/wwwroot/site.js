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

document.getElementById('presentCredential').addEventListener('click', () => {
    // Send a presentation request.
    fetch('api/presentation/request', { method: 'POST' }).then(response => {
        if (!response.ok) {
            // Something went wrong while sending the presentation request, show an error message.
            showApiError(response);
        } else {
            response.json().then(requestResponseBody => {
                // The presentation request was successful, show the response.
                document.getElementById('requestId').innerText = requestResponseBody.requestId;
                document.getElementById('deepLink').href = requestResponseBody.url;
                document.getElementById('qrCodeImage').src = requestResponseBody.qrCode;
                document.getElementById('errorMessage').style.display = 'none';
                document.getElementById('responsePanel').style.display = 'block';
                document.getElementById('scanPanel').style.display = 'block';

                // Start polling the status of the request, as callback messages should arrive
                // back from the Verifiable Credentials Service.
                var checkStatus = setInterval(() => {
                    fetch('api/presentation/status?requestId=' + requestResponseBody.requestId).then(response => {
                        if (!response.ok) {
                            // Something went wrong while polling, show an error message.
                            showApiError(response);
                        } else {
                            response.json().then(statusResponseBody => {
                                var statusMessage = document.getElementById('statusMessage');
                                statusMessage.style.display = 'block';
                                // Check the status code.
                                if (statusResponseBody.status == 'pending') {
                                    statusMessage.innerText = 'Waiting...';
                                } else if (statusResponseBody.status == 'request_retrieved') {
                                    // The QR code was scanned, hide it.
                                    document.getElementById('scanPanel').style.display = 'none';
                                    statusMessage.innerText = 'Please share your verified credential with us.';
                                } else if (statusResponseBody.status == 'presentation_verified') {
                                    // Stop polling for changes.
                                    clearInterval(checkStatus);

                                    // The verified credential was presented.
                                    var isStudent = statusResponseBody.credentialTypes.some(t => t == 'Verified Student');
                                    var customerType = isStudent ? 'verified student' : 'valued customer';
                                    var discount = isStudent ? 5 : 0;
                                    statusMessage.innerText = `Thank you for proving that you are a ${customerType}, you get a ${discount}% discount!`;
                                }
                            })
                        }
                    }).catch(error => {
                        console.log(error.message);
                    });
                }, 1000);
            })
        };
    }).catch(error => {
        console.log(error.message);
    });
});