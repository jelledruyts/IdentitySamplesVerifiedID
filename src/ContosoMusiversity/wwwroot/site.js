var qrCodeGenerator = new QRCode('qrCodeCanvas', { width: 300, height: 300 });

var usePinCodeCheckbox = document.getElementById('usePinCode');
var getCredentialButton = document.getElementById('getCredential');
var errorMessageElement = document.getElementById('errorMessage');
var responsePanel = document.getElementById('responsePanel');
var qrCodeImage = document.getElementById('qrCodeImage');
var qrCodeCanvas = document.getElementById('qrCodeCanvas');
var deepLink = document.getElementById('deepLink');
var requestIdElement = document.getElementById('requestId');
var pinPanelElement = document.getElementById('pinPanel');
var pinCodeElement = document.getElementById('pinCode');

getCredentialButton.addEventListener('click', () => {
    var data = { 'usePinCode': usePinCodeCheckbox.checked };
    fetch('api/issuer/issuance-request', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    }).then(function (response) {
        if (!response.ok) {
            // Something went wrong, show an error message.
            response.json().then(function (errorBody) {
                errorMessageElement.innerText = errorBody.title;
            }).catch(error => {
                errorMessageElement.innerText = error.message;
            });
            errorMessageElement.style.display = 'block';
            responsePanel.style.display = 'none';
        } else {
            // The request was successful, show the response.
            response.json().then(function (responseBody) {
                requestIdElement.innerText = responseBody.requestId;
                pinPanel.style.display = 'none';
                if (responseBody.pinValue) {
                    pinCodeElement.innerText = responseBody.pinValue;
                    pinPanel.style.display = 'block';
                }
                deepLink.href = responseBody.url;
                if (responseBody.qrCode) {
                    // The QR code is already available as a base64 encoded image.
                    qrCodeImage.src = responseBody.qrCode;
                    qrCodeImage.style.display = 'block';
                } else {
                    // Create a QR code from the URL.
                    qrCodeGenerator.makeCode(responseBody.url);
                    qrCodeCanvas.style.display = 'block';
                }
                errorMessageElement.style.display = 'none';
                responsePanel.style.display = 'block';
            })
        };
    }).catch(error => {
        console.log(error.message);
    })
});