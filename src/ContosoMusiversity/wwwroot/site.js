var signIn = document.getElementById('sign-in');
var signOut = document.getElementById('sign-out');
var display = document.getElementById('display');
var qrcode = new QRCode("qrcode", { width: 300, height: 300 });

signIn.addEventListener('click', () => {
    fetch('issue-request')
        .then(function (response) {
            response.text().then(function (message) {
                if (/Android/i.test(navigator.userAgent)) {
                    console.log(`Android device! Using deep link (${message}).`);
                    window.location.href = message; setTimeout(function () {
                        window.location.href = "https://play.google.com/store/apps/details?id=com.azure.authenticator";
                    }, 2000);
                } else if (/iPhone/i.test(navigator.userAgent)) {
                    console.log(`iOS device! Using deep link (${message}).`);
                    window.location.replace(message);
                } else {
                    console.log(`Not Android or IOS. Generating QR code encoded with ${message}`);
                    qrcode.makeCode(message);
                    document.getElementById('sign-in').style.display = "none";
                    document.getElementById('qrText').style.display = "block";
                }
            }).catch(error => {
                console.log(error.message);
            })
        }).catch(error => {
            console.log(error.message);
        })
})
