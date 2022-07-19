document.getElementById('presentCredential').addEventListener('click', () => {
    fetch('api/presentation/request', { method: 'POST' }).then(function (response) {
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
            // The presentation request was successful, show the response.
            response.json().then(function (responseBody) {
                document.getElementById('requestId').innerText = responseBody.requestId;
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