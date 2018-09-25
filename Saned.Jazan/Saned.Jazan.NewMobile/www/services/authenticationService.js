

var authenticationService = (function () {

    var loadSideMenuLinks = function () {

        var visitorStatue = localStorage.getItem('Visitor');
        if (visitorStatue == 'true') {
            $('#linkTologout').css('display', 'none');
            $('#linkToadvertismentsProfile').css('display', 'none');
            $('#linkToaddAdvertisment').css('display', 'none');
            $('#linkToresetPass').css('display', 'none');
            $('#linkToadvertismentsProfile').css('display', 'none');
            $('#addAdvertisment').css('display', 'none');
            $('#notification').css('display', 'none');
            $('#btnTologout').css('display', 'none');
            $('#linkTonotification').css('display', 'none');
            $('#linkTologin').css('display', 'block');
            $('#btnTologin').css('display', 'block');
        }
        else {
            $('#linkTologout').css('display', 'block');
            $('#addAdvertisment').css('display', 'block');
            $('#notification').css('display', 'block');
            $('#linkToadvertismentsProfile').css('display', 'block');
            $('#linkToaddAdvertisment').css('display', 'block');
            $('#linkToresetPass').css('display', 'block');
            $('#btnTologout').css('display', 'block');
            $('#linkTonotification').css('display', 'block');
            $('#linkTologin').css('display', 'none');
            $('#btnTologin').css('display', 'none');
        }


    }

    function goToSignupVisitorPage(page) {
        if (typeof page != 'undefined') {
          
            $('#signup-txtFullName').val('');
            $('#signup-txtUserName').val('');
            $('#signup-txtEmail').val('');
            $('#signup-txtMobile').val('');
            $('#signup-txtPassword').val('');
            $('#signup-txtConfirmPassword').val('');

            $('#sign-up-form').parsley().reset();
            var type = page.query.type;
            var signUpType = 'signup';
            localStorage.setItem('userType', type);
        }
    }
    function goToActivationCodePage(page) {
        if (typeof page != 'undefined') {
            $('#txtCode').val('');
            $('#sendActivationCode').unbind().on('click', function () {
                //FValidation.ValidateAll('activation', function (res) {
                //    if (res == true) {

                if ($('#reset-activation-code').parsley().validate()) {
                    var txtCode = $('#txtCode').val();
                    var userId = localStorage.getItem('UserID');

                    CallService('activationCode', 'POST', 'api/Account/ConfirmEmail', { "userId": userId, "code": txtCode }, function (res) {
                        if (res != null) {
                            localStorage.setItem('UserEntersCode', true);
                            mainView.router.loadPage({ pageName: 'login' })
                        }
                        else {
                            notificationService.alert( 'خطا','خطا  اثناء عملية تفعيل الايميل', function () { });
                        }
                    });
                }
                //}
                //});
            });
        }
    }
    function goToForgetPasswordPage(page) {
        if (typeof page != 'undefined') {
            
            $('#forgot-password-page-form').parsley().reset();
            $('#txtForgetPasswordEmail').val('');
        }
    }
    function goToResetPasswordPage(page) {
        if (typeof page != 'undefined') {
            $('#reset-password-page-form').parsley().reset();
            $('#txtResetCode').val('');
            $('#txtResetPassword').val('');
            $('#txtResetConfirmPassword').val('');

        }
    }
    function goToChangePasswordPage(page) {
        if (typeof page != 'undefined') {
            $('#change-password-page').parsley().reset();
            $('#txtChangeOldPassword').val('');
            $('#txtChangeNewPassword').val('');
            $('#txtChangeConfirmNewPassword').val('');
        }
    }

    var signOut = function () {
        // CallService('', 'POST', 'api/Account/SignOut', null, function (viewsRes) {
        localStorage.removeItem('appToken');
        localStorage.removeItem('USName');
        localStorage.removeItem('refreshToken');
        localStorage.removeItem('userLoggedIn');
        localStorage.removeItem('Visitor');
        localStorage.removeItem('loginUsingSocial');
        localStorage.setItem('Visitor', true);
        mainView.router.loadPage({ pageName: 'login' });
        //  });
    }

    var service = {
        signOut: signOut,
        goToSignupVisitorPage: goToSignupVisitorPage,
        goToActivationCodePage: goToActivationCodePage,
        goToForgetPasswordPage: goToForgetPasswordPage,
        goToResetPasswordPage: goToResetPasswordPage,
        goToChangePasswordPage: goToChangePasswordPage
    };

    return service;

})();

