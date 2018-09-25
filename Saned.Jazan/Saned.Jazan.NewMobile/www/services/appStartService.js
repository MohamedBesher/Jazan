var $$ = Dom7;
var lang = 'En';

//var serviceURL = 'http://192.168.0.99:1993/';
//var imgHost = 'http://192.168.0.99:1993/uploads/';

var serviceURL = 'http://jazanapi.saned-projects.com/';
var adminPanelURL = 'http://jazanadmin.saned-projects.com';
var imgHost = 'http://jazanapi.saned-projects.com/uploads/';


Array.prototype.contains = function (obj) {
    var i = this.length;
    while (i--) {
        if (this[i] === obj) {
            return true;
        }
    }
    return false;
}

var myApp = new Framework7({
    pushState: true,
    swipeBackPage: false,
    swipePanel: false,
    panelsCloseByOutside: true,
    init: false,
    animateNavBackIcon: true,
    modalButtonOk: 'تم',
    modalButtonCancel: 'إلغاء'
});

$$('.popup-contact').on('opened', function () {
    mobileSettingController.goToContactUs();
});

$('#linkMenuProfile').unbind().on("click", function () {
    mainView.router.loadPage({ pageName: 'profile' });
});

$('#linkTologout').unbind().on("click", function () {
    authenticationService.signOut();
});

$('#btnTologout').unbind().on("click", function () {
    authenticationService.signOut();
});

$('#sendActivateCodeAgain').unbind().on('click', function () {

    var userId = localStorage.getItem('UserID');

    CallService('activationCode', "POST", "api/Account/ReSendConfirmationCode/" + userId, '', function (res) {
        if (res != null) {
            notificationService.alert('نجاح', 'تم إعادة إرسال الكود بنجاح .', function () { });
            localStorage.getItem('Visitor', false);
        }
    });
});

$("#btnLogin").unbind().on("click", function () {

    var loginEmail = $("#loginEmail").val();
    var loginPassword = $('#Loginpassword').val();

    loginEmail = loginEmail.trim();

    if ((loginEmail != '' && loginPassword != null)) {

        GetToken('login', "POST", "token", loginEmail, loginPassword, function (res) {

            if (res != null) {
                localStorage.setItem('appToken', res.access_token);
                localStorage.setItem('usrPhoto', res.PhotoUrl);
                localStorage.setItem('USName', res.userName);
                localStorage.setItem('refreshToken', res.refresh_token);
                localStorage.setItem('Visitor', false);
                localStorage.setItem('loginUsingSocial', false);
                CallService('login', "POST", "api/User/FindByUserId/" + res.userName, '', function (res) {
                    if (res != null) {
                        localStorage.setItem('userLoggedIn', JSON.stringify(res));
                        if (typeof res.photoUrl != 'undefined' && res.photoUrl != null && res.photoUrl != '' && res.photoUrl != ' ') {
                            localStorage.setItem('usrPhoto', res.photoUrl);
                        }
                        else {
                            localStorage.removeItem('usrPhoto');
                        }

                        dataContextService.notifications.addDevice(localStorage.getItem('deviceId'), function () {

                        });


                        if (typeof localStorage.getItem('userType') == 'undefined' || localStorage.getItem('userType') == null || localStorage.getItem('userType') == 'visitor') {
                            mainView.router.loadPage({ pageName: 'intro' });
                        }
                        else {
                            localStorage.removeItem('userType');
                            mainView.router.loadPage({ pageName: 'addAdvertisment' });
                        }
                    }
                    else {
                        notificationService.alert('خطأ', 'لا يمكن التحقق من البيانات .', function () { });
                    }
                });
            }
            else {
                notificationService.alert('خطا', 'خطا اثناء عمليه تسجيل الدخول', function () { })
            }
        });
    }
    else {
        notificationService.alert('تنبيه', 'من فضلك أدخل إسم الدخول وكلمة المرور', function () { });
    }
});

$('#visitorSignUp').unbind().on('click', function () {
    mainView.router.loadPage({ pageName: 'signupVisitor', query: { type: 'visitor' } });
});

$('#projectOwnerSignUp').unbind().on('click', function () {
    mainView.router.loadPage({ pageName: 'signupVisitor', query: { type: 'projectOwner' } });
});

$('#linkForgetPassword').unbind().on('click', function () {
    mainView.router.loadPage({ pageName: 'forgetPassword' });
});

$('#facebookLogin').unbind().on('click', function () {

    var fbLoginFailed = function (error) {
        console.log(error);
        facebookConnectPlugin.login(["public_profile", "email", "user_about_me"], function (userData) {
            var userFullName = '';
            if (userData.authResponse) {
                var accessToken = userData.authResponse.accessToken;
                facebookConnectPlugin.api(userData.authResponse.userID + "/?fields=id,email,first_name,last_name", ["public_profile"],
               function (result) {
                   userFullName = result.first_name + ' ' + result.last_name;
                   CallService('login', "POST", "api/Account/RegisterExternal", { "Provider": "Facebook", "ExternalAccessToken": accessToken, "userId": userData.authResponse.userID, "name": userFullName }, function (res) {
                       if (res != null) {
                           localStorage.setItem('USName', res.userName);
                           localStorage.setItem('appToken', res.access_token);
                           localStorage.setItem('loginUsingSocial', true);

                           CallService('login', "POST", "api/User/GetUserInfo", '', function (res1) {
                               if (res1 != null) {
                                   res1.userName = res1.name;
                                   localStorage.setItem('userLoggedIn', JSON.stringify(res1));
                                   if (typeof res1.photoUrl != 'undefined' && res1.photoUrl != null && res1.photoUrl != '' && res1.photoUrl != ' ') {
                                       localStorage.setItem('usrPhoto', res1.photoUrl);
                                   }
                                   else {
                                       localStorage.removeItem('usrPhoto');
                                   }

                                   mainView.router.loadPage({ pageName: 'statistics' });

                               }
                           });
                           //mainView.router.loadPage({ pageName: 'categories' });
                       }
                   });
               },
                function (error) {
                    console.log('ERROR:' + error);
                });
            }
        }, function () { });
    }

    var fbLoginSuccess = function (userData) {
        if (userData.authResponse) {

            var accessToken = userData.authResponse.accessToken;
            var userFullName = '';
            facebookConnectPlugin.api(userData.authResponse.userID + "/?fields=id,email,first_name,last_name", ["public_profile"],
                function (result) {
                    userFullName = result.first_name + ' ' + result.last_name;
                    CallService('login', "POST", "api/Account/RegisterExternal", { "Provider": "Facebook", "ExternalAccessToken": accessToken, "userId": userData.authResponse.userID, "name": userFullName }, function (res) {
                        if (res != null) {
                            localStorage.setItem('USName', res.userName);
                            localStorage.setItem('appToken', res.access_token);
                            localStorage.setItem('loginUsingSocial', true);
                            localStorage.setItem('Visitor', false);
                            CallService('login', "POST", "api/User/FindByUserId/" + res.userName, '', function (res) {
                                if (res != null) {
                                    localStorage.setItem('userLoggedIn', JSON.stringify(res));
                                    if (typeof res.photoUrl != 'undefined' && res.photoUrl != null && res.photoUrl != '' && res.photoUrl != ' ') {
                                        localStorage.setItem('usrPhoto', res.photoUrl);
                                    }
                                    else {
                                        localStorage.removeItem('usrPhoto');
                                    }

                                    dataContextService.notifications.addDevice(localStorage.getItem('deviceId'), function () {

                                    });

                                    mainView.router.loadPage({ pageName: 'intro' });
                                }
                                else {
                                    notificationService.alert('خطأ', 'لا يمكن التحقق من البيانات .', function () { });
                                }
                            });
                        }
                    });
                },
                function (error) {
                    console.log('ERROR:' + error);
                });


        }
    }

    facebookConnectPlugin.login(["public_profile", "email"], fbLoginSuccess, fbLoginFailed);


});

$('#googlePlusLogin').unbind().on('click', function () {
    window.plugins.googleplus.login(
        {
            'scopes': 'profile email',
            'webClientId': '800609633559-l4ndqjvjc7dn280akgj0i2t8oosg0k52.apps.googleusercontent.com',
            'offline': false,
        },
        function (obj) {
            var userName = obj.displayName;
            var userEmail = obj.email;
            var userId = obj.userId;
            var userToken = obj.idToken;

            localStorage.setItem('socialEmail', userEmail);

            var registerObj = {
                "Provider": "Google",
                "userId": userId,
                "name": userName,
                "email": userEmail,
                "ExternalAccessToken": userToken
            };

            CallService('login', "POST", "api/Account/RegisterExternal", registerObj, function (res) {
                if (res != null) {
                    localStorage.setItem('USName', res.userName);
                    localStorage.setItem('appToken', res.access_token);
                    localStorage.setItem('loginUsingSocial', true);
                    localStorage.setItem('Visitor', false);
                    CallService('login', "POST", "api/User/FindByUserId/" + res.userName, '', function (res) {
                        if (res != null) {
                            localStorage.setItem('userLoggedIn', JSON.stringify(res));
                            if (typeof res.photoUrl != 'undefined' && res.photoUrl != null && res.photoUrl != '' && res.photoUrl != ' ') {
                                localStorage.setItem('usrPhoto', res.photoUrl);
                            }
                            else { localStorage.removeItem('usrPhoto'); }

                            dataContextService.notifications.addDevice(localStorage.getItem('deviceId'), function () { });
                            mainView.router.loadPage({ pageName: 'intro' });
                        }
                        else {
                            notificationService.alert('خطأ', 'لا يمكن التحقق من البيانات .', function () { });
                        }
                    });
                }
            });
        },
        function (msg) {
            console.log('error: ' + msg);
        });

});

$('#twitterLogin').unbind().on('click', function () {
    TwitterConnect.login(function (data) {
        var accessToken = data.token;
        var userFullName = '';

        TwitterConnect.showUser(function (result) {
            var userFullName = result.name;
            CallService('login', "POST", "api/Account/RegisterExternal", { "Provider": "Twitter", "ExternalAccessToken": accessToken, "userId": data.userId, "name": data.userName }, function (res) {
                if (res != null) {
                    localStorage.setItem('USName', res.userName);
                    localStorage.setItem('appToken', res.access_token);
                    localStorage.setItem('loginUsingSocial', true);
                    localStorage.setItem('Visitor', false);
                    CallService('login', "POST", "api/User/FindByUserId/" + res.userName, '', function (res) {
                        if (res != null) {
                            localStorage.setItem('userLoggedIn', JSON.stringify(res));
                            if (typeof res.photoUrl != 'undefined' && res.photoUrl != null && res.photoUrl != '' && res.photoUrl != ' ') {
                                localStorage.setItem('usrPhoto', res.photoUrl);
                            }
                            else { localStorage.removeItem('usrPhoto'); }

                            dataContextService.notifications.addDevice(localStorage.getItem('deviceId'), function () { });
                            mainView.router.loadPage({ pageName: 'intro' });
                        }
                        else {
                            notificationService.alert('خطأ', 'لا يمكن التحقق من البيانات .', function () { });
                        }
                    });

                }
            });
        }
        ,
        function (error) {
            //notificationService.alert('Error', 'Error retrieving user profile', null);
        });


    }, function (error) {
        console.log('Error in Login');
    });
});

$('#userSignUpBack').unbind().on('click', function () {
    $('#completeName').val('');
    $('#userName').val('');
    $('#email').val('');
    $('#mobileNumber').val('');
    $('#password').val('');
    $('#passwordConfirm').val('');
    mainView.router.back();
});

$('#userSignUp').unbind().on('click', function () {
    if (localStorage.getItem('userLoggedIn')) {
        var user = JSON.parse(localStorage.getItem('userLoggedIn'));
        signUpType = 'editProfile';
    }
    else {
        signUpType = 'signup';
    }

    //FValidation.ValidateAll(signUpType, function (res) {
    //    if (res == true) {
    if ($('#sign-up-form').parsley().validate()) {
        var params = {
            Name: $('#signup-txtFullName').val(),
            UserName: $('#signup-txtUserName').val(),
            Email: $('#signup-txtEmail').val(),
            MobileNumber: $('#signup-txtMobile').val(),
            Password: $('#signup-txtPassword').val(),
            ConfirmPassword: $('#signup-txtConfirmPassword').val()
        };

        CallService('signupVisitor', 'POST', 'api/Account/Register', params, function (res) {
            if (res != null) {
                localStorage.setItem('USName', 'مستخدم');
                localStorage.setItem('UserID', res);
                localStorage.setItem('UserEntersCode', false);
                notificationService.alert('نجاح', 'تم تسجيل المستخدم بنجاح', function () {
                    mainView.router.loadPage({ pageName: 'activationCode' });
                });
            }
            else {
                notificationService.alert('خطا', 'خطا اثناء عملية التسجيل', function () { });
            }
        });
    }
});

$('#btnPasswordBackToHome').unbind().on('click', function () {
    mainView.router.loadPage({ pageName: 'userAccount' });
});

$('#btnChangePassword').unbind().on('click', function () {

    if ($('#change-password-page').parsley().validate()) {
        var params = {
            'oldPassword': $('#txtChangeOldPassword').val(),
            'newPassword': $('#txtChangeNewPassword').val(),
            'ConfirmPassword': $('#txtChangeConfirmNewPassword').val()
        }

        CallService('changePassword', "POST", "Api/Account/ChangePassword", params, function (res) {
            if (res != null) {
                notificationService.alert('نجاح', 'تم تعديل كلمة السر بنجاح .', function () {
                    mainView.router.loadPage({ pageName: 'intro' });
                });
            }
            else {
                notificationService.alert('خطأ', 'خطأ في تعديل كلمة السر.', function () {

                });
            }
        });


    }

});

$('#btnResetPasswordBackToHome').unbind().on('click', function () {
    mainView.router.loadPage({ pageName: 'login' });
});

$('#btnResetPassword').unbind().on('click', function () {

    if ($('#reset-password-page-form').parsley().validate()) {
        var params = {
            'code': $('#txtResetCode').val(),
            'email': localStorage.getItem('confirmationMail'),
            'password': $('#txtResetPassword').val(),
            'confirmPassword': $('#txtResetConfirmPassword').val()
        }

        CallService('resetPassword', "POST", "api/Account/ResetPassword", params, function (res) {
            if (res != null) {
                localStorage.removeItem('confirmationMail');
                notificationService.alert('نجاح', 'تم تغيير كلمة المرور القديمة بنجاح .', function () {
                    mainView.router.loadPage({ pageName: 'login' });
                });
            }
            else {
                notificationService.alert('خطأ', 'خطأ في تغيير كلمة المرور القديمة.', function () {

                });
            }
        });
    }

});

$('#btnBackFromForgetPassword').unbind().on('click', function () {
    mainView.router.loadPage({ pageName: 'login' });
});

$('#btnSendEmail').unbind().on('click', function () {

    if ($('#forgot-password-page-form').parsley().validate()) {
        var params = {
            'email': $('#txtForgetPasswordEmail').val()
        }

        CallService('forgetPassword', "POST", "api/Account/ForgetPassword", params, function (res) {
            if (res != null) {
                localStorage.setItem('confirmationMail', $('#txtForgetPasswordEmail').val());
                notificationService.alert('نجاح', 'تم إرسال الكود لبريدك الإليكتروني بنجاح .', function () {
                    mainView.router.loadPage({ pageName: 'resetPassword' });
                });
            }
            else {
                notificationService.alert('خطأ', 'خطأ في إرسال الكود لبريدك الإليكتروني .', function () {
                    mainView.router.loadPage({ pageName: 'resetPassword' });
                });
            }
        });
    }

});
