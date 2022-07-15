var signIn = document.getElementById('sign-in');
var qrcode = new QRCode("qrcode", { width: 300, height: 300 });

signIn.addEventListener('click', () => {
    fetch('api/issuer/issuance-request', { method: 'POST' })
        .then(function (response) {
            var errorMessageElement = document.getElementById('errorMessage');
            if (!response.ok) {
                errorMessageElement.style.display = "block";
                response.json().then(function (errorBody) {
                    errorMessageElement.innerText = errorBody.title;
                }).catch(error => {
                    errorMessageElement.innerText = error.message;
                });
            } else {
                errorMessageElement.style.display = "none";
                response.json().then(function (responseBody) {
                    if (/Android/i.test(navigator.userAgent)) {
                        console.log(`Android device! Using deep link (${responseBody}).`);
                        window.location.href = responseBody;
                        setTimeout(function () {
                            window.location.href = "https://play.google.com/store/apps/details?id=com.azure.authenticator";
                        }, 2000);
                    } else if (/iPhone/i.test(navigator.userAgent)) {
                        console.log(`iOS device! Using deep link (${responseBody}).`);
                        window.location.replace(responseBody);
                    } else {
                        console.log(`Not Android or IOS. Generating QR code encoded with ${responseBody}`);
                        qrcode.makeCode(responseBody);
                        document.getElementById('sign-in').style.display = "none";
                        document.getElementById('qrText').style.display = "block";
                    }
                })
            };
        }).catch(error => {
            console.log(error.message);
        })
})
