
function render() {
    window.recaptchaVerifier = new firebase.auth.RecaptchaVerifier('recaptcha-container');
    recaptchaVerifier.render();
}

function phoneAuth(number) {
    //phone number authentication function of firebase
    //it takes two parameter first one is number,,,second one is recaptcha
    firebase.auth().signInWithPhoneNumber(number, window.recaptchaVerifier).then(function (confirmationResult) {
        //s is in lowercase
        window.confirmationResult = confirmationResult;
        coderesult = confirmationResult;
        console.log(coderesult);
        showSuccessAlert("Verify Phone", "A sms with 6-digit verification code has been sent to your number " + number, "Ok", "Cancel");
        invokeMethod("VerificationSentSuccessfully");
    }).catch(function (error) {
        showErrorAlert("Error", error.message, "Ok", "Cancel");
    });
}
function codeverify(code) {
    coderesult.confirm(code).then(function (result) {
        showSuccessAlert("Verification Successfull", "Well done! your phone number has been verified successfuly", "Ok", "Cancel");
        var user = result.user.uid;
        DotNet.invokeMethodAsync('TechFlurry.SparkLedger.PWA', "AuthenticateUser", user);
    }).catch(function (error) {
        showErrorAlert("Error", error.message, "Ok", "Cancel");
    });
}