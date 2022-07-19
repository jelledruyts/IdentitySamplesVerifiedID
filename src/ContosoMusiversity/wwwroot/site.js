document.getElementById('getCredential').addEventListener('click', () => {
    var data = {
        'usePinCode': document.getElementById('usePinCode').checked
    };
    fetch('api/issuer/issuance-request', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    }).then(function (response) {
        if (!response.ok) {
            // Something went wrong, show an error message.
            var errorMessageElement = document.getElementById('errorMessage');
            response.json().then(function (errorBody) {
                errorMessageElement.innerText = errorBody.title;
            }).catch(error => {
                errorMessageElement.innerText = error.message;
            });
            errorMessageElement.style.display = 'block';
            document.getElementById('responsePanel').style.display = 'none';
        } else {
            // The issuance request was successful, show the response.
            response.json().then(function (responseBody) {
                document.getElementById('requestId').innerText = responseBody.requestId;
                var pinPanel = document.getElementById('pinPanel');
                pinPanel.style.display = 'none';
                if (responseBody.pinValue) {
                    document.getElementById('pinCode').innerText = responseBody.pinValue;
                    pinPanel.style.display = 'block';
                }
                document.getElementById('deepLink').href = responseBody.url;
                var qrCodeImage = document.getElementById('qrCodeImage');
                qrCodeImage.src = responseBody.qrCode;
                qrCodeImage.style.display = 'block';
                document.getElementById('errorMessage').style.display = 'none';
                document.getElementById('responsePanel').style.display = 'block';
            })
        };
    }).catch(error => {
        console.log(error.message);
    })
});