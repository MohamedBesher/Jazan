

//var userId = 0;
//userId = localStorage.getItem("UID");
//var markers = [];
//var user;
//var item = 0;
//var allCities = [];
//var allCategories = [];
//var initNotification = false;
//var BackIsClicked = false;
//var scrollLoadsBefore = false;
//var initLandingPage = true;
//var initIntroPage = true;
//var initLoginPage = true;
//var initSignupVisitorPage = true;
//var initForgetPassPage = true;
//var initActivationCodePage = true;
//var initResetPassPage = true;
//var initDalelPage = true;
//var initAdvertismentsPage = true;
//var initNewsPage = true;
//var initAdvertismentsDetailsPage = true;
//var initAdvertismentsProfilePage = true;
//var initAdvertismentsProfileDetailsPage = true;
//var initUserProfilePage = true;
//var initAddAdvertismentPage = true;
//var initCategoryDetailsPage = true;
//var initPlacePage = true;
//var iniTouristPage = true;
//var initTouristInfinite = true;
//var initUserProjectsInfinite = true;
//var initCommentsInfinite = true;
//var initUserVisitsInfinite = true;
//var initNewsInfinite = true;
//var initCommentsInfinite = true;
//var initPlaceDetailsPage = true;
//var initTouristDetailsPage = true;
//var initNewsDetailsPage = true;
//var initQuizPage = true;
//var initAdvertismentDetailsPage = true;
//var categoriesBackClicked = false;
//var initForgetPassword = true;
//var initResetPassword = true;
//var initChangePassword = true;
//var loaderPage;
//var clientId = 'consoleApp';
//var clientSecret = '123@abc';
//var linkBackSearch = false;
//var linkBackAddProduct = false;
//var initBooksTest = true;
//var adIsFristLoading = true


////side main work 2 
//var initAdPage = true;
//var initDalel;
//var initPersonalAds;
//var initAllAds;
//var initReachEnd = true;
//var initMainBanner;
//var initSwiperPage;


var mainView = myApp.addView('.view-main', {
    dynamicNavbar: true,
    domCache: true
});

//old init
var initLandingPage = true;
var initSwiperPage = true;
var initTouristInfinite = true
var initPlacesInfinite = true;
var initCommentsInfinite = true;
var initUserProjectsInfinite = true;
var initUserVisitsInfinite = true;
var initNewsInfinite = true;
var initForgetPassword = true;
var initResetPassword = true;
var initChangePassword = true;
var initTouristDetailsPage = true;
var initAdvertismentsPage = true;
var iniTouristPage = true;
var initNotification = true;
var initIntroPage = true;


// new init
var init_landing = true;
var init_intro = true;
var init_login = true;
var init_signupVisitor = true;
var init_activationCode = true;
var init_forgetPassword = true;
var init_changePassword = true;
var init_resetPassword = true;
var init_dalel = true;
var init_notification = true;
var init_about = true;
var init_advertisments = true;
var init_news = true;
var init_advertismentsDetails = true;
var init_advertismentsProfile = true;
var init_advertismentsProfileProjects = true;
var init_advertismentsProfileVisits = true;
var init_touristVisitProfileDetails = true;
var init_advertismentsProfileDetails = true;
var init_addAdvertisment = true;
var init_categoryDetails = true;
var init_place = true;
var init_tourist = true;
var init_placeDetails = true;
var init_touristDetails = true;
var init_newsDetails = true;
var init_quiz = true;
var init_addProjectDetails = true;
var init_news_slider = true;
var init_prayer_slider = true;

var clientId = 'consoleApp';
var clientSecret = '123@abc';

$$(document).on('pageInit', function (e) {
    page = e.detail.page;
    if (page.name === 'landing') {

        init_landing = true;
        initLandingPage = true;

        init_prayer_slider = true;

        setTimeout(function () {
            $(".landing").delay(3000).fadeOut("slow");

            window.plugins.OneSignal.startInit("70bc392b-584e-47a4-95e2-82074e7616f9")
                                  .inFocusDisplaying(window.plugins.OneSignal.OSInFocusDisplayOption.Notification)
                                  .handleNotificationOpened(notificationOpenedCallback)
                                  .endInit();

            window.plugins.OneSignal.getIds(function (ids) {
                localStorage.setItem('deviceId', ids.userId);
            });

            if (localStorage.getItem('USName')) {
                if (localStorage.getItem('UserEntersCode') == "false") {
                    if (localStorage.getItem('loginUsingSocial') == true) {
                        mainView.router.loadPage({ pageName: 'intro' });
                    } else {
                        mainView.router.loadPage({ pageName: 'activationCode' });
                    }
                }
                else { mainView.router.loadPage({ pageName: 'intro' }); }
            }
            else {
                if (localStorage.getItem('UID')) {
                    mainView.router.loadPage({ pageName: 'signupVisitor' });
                }   //else if (!localStorage.getItem('Visitor') || localStorage.getItem('Visitor') == null) { mainView.router.loadPage({ pageName: 'intro' }); }
                else { mainView.router.loadPage({ pageName: 'login' }); }
            }
        }, 3000);
    }
    if (page.name === 'intro') {
        init_news_slider = true;
        init_intro = true;
        initSwiperPage = true;
        initIntroPage = true;

        $('#prayerSwiper').owlCarousel({
            rtl: true,
            loop: true,
            autoplay: true,
            autoplayTimeout: 5000,
            items: 1,
            margin: 0,
            nav: false,
        });
    }
    if (page.name === 'login') { init_login = true; }
    if (page.name === 'signupVisitor') { init_signupVisitor = true; }
    if (page.name === 'activationCode') { init_activationCode = true; }
    if (page.name === 'forgetPassword') { init_forgetPassword = true; initForgetPassword = true; }
    if (page.name === 'changePassword') { init_changePassword = true; initChangePassword = true; }
    if (page.name === 'resetPassword') { init_resetPassword = true; initResetPassword = true; }
    if (page.name === 'dalel') { init_dalel = true; }
    if (page.name === 'notification') { init_notification = true; initNotification = true; }
    if (page.name === 'about') { init_about = true; }
    if (page.name === 'advertisments') { init_advertisments = true; initAdvertismentsPage = true; }
    if (page.name === 'news') { init_news = true; initNewsInfinite = true; }
    if (page.name === 'advertismentsDetails') { init_advertismentsDetails = true; }
    if (page.name === 'advertismentsProfile') { init_advertismentsProfile = true; }
    if (page.name === 'advertismentsProfileProjects') { init_advertismentsProfileProjects = true; initUserProjectsInfinite = true; }
    if (page.name === 'advertismentsProfileVisits') { init_advertismentsProfileVisits = true; initUserVisitsInfinite = true; }
    if (page.name === 'touristVisitProfileDetails') { init_touristVisitProfileDetails = true; }
    if (page.name === 'advertismentsProfileDetails') { init_advertismentsProfileDetails = true; }
    if (page.name === 'addAdvertisment') { init_addAdvertisment = true; }
    if (page.name === 'categoryDetails') { init_categoryDetails = true; }
    if (page.name === 'place') { init_place = true; initPlacesInfinite = true; }
    if (page.name === 'tourist') { init_tourist = true; initTouristInfinite = true; iniTouristPage = true; }
    if (page.name === 'placeDetails') { init_placeDetails = true; initCommentsInfinite = true; }
    if (page.name === 'touristDetails') { init_touristDetails = true; initTouristDetailsPage = true; }
    if (page.name === 'newsDetails') { init_newsDetails = true; }
    if (page.name === 'quiz') { init_quiz = true; }
    if (page.name === 'addProjectDetails') { init_addProjectDetails = true; }
});

window.document.addEventListener('backbutton', function (event) {


    if ($('.modal-in').length > 0) {
        myApp.closeModal();
    } else {
        var currentPage = mainView.activePage.name;

        if (currentPage == 'intro') {
            navigator.notification.confirm("هل تريد الخروج من التطبيق ؟", function (buttonIndex) {
                if (buttonIndex == 1) {
                    ExitApplication();
                }
            }, 'دليل جازان', ['نعم', 'لا']);

        }
        else {
            mainView.router.back();
        }
    }

});

function remove() {
    var frame = document.getElementById("videoSrc");
    frame.parentNode.removeChild(frame);
}

function ExitApplication() {
    if (navigator.app) {
        navigator.app.exitApp();
    }
    else if (navigator.device) {
        navigator.device.exitApp();
    }
}

function onConfirmExit(buttonIndex) {
    if (buttonIndex == 1) {
        if (device.platform.indexOf('dr') > 0) {
            ExitApplication();
        }
    }
}

function checkConnection() {
    var networkState = navigator.connection.type;
    return networkState;
}

function callWeatherService(pageName, CallType, serverUrl, dataVariables, callback) {

    $.ajax({
        pageName: pageName,
        type: CallType,
        url: serverUrl,
        headers: {
            "content-type": "application/json",
            "cache-control": "no-cache"
        },
        data: dataVariables,
        async: true,
        crossDomain: true
    }).done(function (result) {
        callback(result);

    }).fail(function (error, textStatus) {

        var responseText = JSON.parse(error.responseText);

    });






}

function ShowLoader(pageName) {
    //var divPage = document.getElementById(pageName + 'Page');
    //var divLoader = document.createElement('div');
    //var hdrWait = document.createElement('h3');
    //var loadImage = document.createElement('img');

    //divLoader.className += 'loader divLoader';
    //loadImage.src = 'img/load.svg';
    ////hdrWait.innerHTML = 'برجاء الإنتظار';

    //divLoader.appendChild(loadImage);
    //divLoader.appendChild(hdrWait);
    //divPage.appendChild(divLoader);
    //$(".loader").fadeIn("slow");

    if ($('.popup.modal-in').length) {
        $('.popup.modal-in').block({
            css: {
                backgroundColor: 'transparent',
                border: 'none'
            },
            message: '<div class="preloader-indicator-modal"><span class="preloader preloader-white"><span class="preloader-inner"><span class="preloader-inner-gap"></span><span class="preloader-inner-left"><span class="preloader-inner-half-circle"></span></span><span class="preloader-inner-right"><span class="preloader-inner-half-circle"></span></span></span></span></div><div class="preloader-indicator-modal"><span class="preloader preloader-white"><span class="preloader-inner"><span class="preloader-inner-gap"></span><span class="preloader-inner-left"><span class="preloader-inner-half-circle"></span></span><span class="preloader-inner-right"><span class="preloader-inner-half-circle"></span></span></span></span></div>',
            overlayCSS: { backgroundColor: 'transparent' }
        });
    } else {
        //$('.page-content').css('visibility', 'hidden');
        $('[data-page].page-on-center').block({
            css: {
                backgroundColor: 'transparent',
                border: 'none'
            },
            message: '<div class="preloader-indicator-modal"><span class="preloader preloader-white"><span class="preloader-inner"><span class="preloader-inner-gap"></span><span class="preloader-inner-left"><span class="preloader-inner-half-circle"></span></span><span class="preloader-inner-right"><span class="preloader-inner-half-circle"></span></span></span></span></div><div class="preloader-indicator-modal"><span class="preloader preloader-white"><span class="preloader-inner"><span class="preloader-inner-gap"></span><span class="preloader-inner-left"><span class="preloader-inner-half-circle"></span></span><span class="preloader-inner-right"><span class="preloader-inner-half-circle"></span></span></span></span></div>',
            overlayCSS: { backgroundColor: 'transparent' }
        });
    }


}

function HideLoader(pageName) {
    $('[data-page]').each(function () { $(this).unblock() });
    $('.popup').each(function () { $(this).unblock() });
    //$('.page-content').css('visibility','visible')
}

function RefreshToken(pageName, CallType, MethodName, callback) {
    var token = localStorage.getItem('refreshToken');
    ShowLoader(pageName);
    $.ajax({
        type: CallType,
        url: serviceURL + MethodName,
        headers: {
            "content-type": "application/x-www-form-urlencoded",
            "cache-control": "no-cache",
            "postman-token": "a7924ea4-7d97-e2f6-5b56-33cbffb586a7"
        },
        data: { 'refresh_token': token, 'grant_type': 'refresh_token', 'client_id': clientId, 'client_secret': clientSecret },
        async: true,
        crossDomain: true,
        success: function (result) {

        },
        error: function (error) {

        }
    }).done(function (result) {
        var res = result;
        HideLoader();
        callback(res);
    })
        .fail(function (error, textStatus) {
            HideLoader();
            console.log("Error in (" + MethodName + ") , Error text is : [" + error.responseText + "] , Error json is : [" + error.responseJSON + "] .");

            var responseText = JSON.parse(error.responseText);

            //if (error.status === 401) {
            notificationService.alert('خطأ', 'لا يمكن إعادة تنشيط رمز التحقق الخاص بك لإنتهاء صلاحيته .', function () {
                localStorage.removeItem('appToken');
                localStorage.removeItem('USName');
                localStorage.removeItem('refreshToken');
                localStorage.removeItem('userLoggedIn');
                localStorage.removeItem('Visitor');
                localStorage.removeItem('loginUsingSocial');
                localStorage.setItem('Visitor', true);
                myApp.closeModal();
                mainView.router.loadPage({ pageName: 'login' });
            });
            //}
        });
}

function GetToken(pageName, CallType, MethodName, userName, password, callback) {
    ShowLoader(pageName);
    $.ajax({
        type: CallType,
        url: serviceURL + MethodName,
        headers: {
            "content-type": "application/x-www-form-urlencoded",
            "cache-control": "no-cache",
            "postman-token": "a7924ea4-7d97-e2f6-5b56-33cbffb586a7"
        },
        data: { 'userName': userName, 'Password': password, 'grant_type': 'password', 'client_id': clientId, 'client_secret': clientSecret },
        async: true,
        crossDomain: true,
        success: function (result) {

        },
        error: function (error) {

        }
    }).done(function (result) {
        var res = result;
        HideLoader();
        callback(res);
    })
        .fail(function (error, textStatus) {
            HideLoader();
            console.log("Error in (" + MethodName + ") , Error text is : [" + error.responseText + "] , Error json is : [" + error.responseJSON + "] .");

            var responseText = JSON.parse(error.responseText);

            if (typeof responseText.error_description != 'undefined') {
                var error_description = responseText.error_description;
                if (error_description === 'The user name or password is incorrect.') {
                    notificationService.alert('خطأ', 'خطأ في اسم المستخدم أو كلمة المرور .', function () { });
                }
                else if (error_description === '1-User are Arhieve') {
                    notificationService.alert('خطأ', 'تمت أرشفة حسابك, من فضلك اتصل بإدارة التطبيق .', function () { });
                }
                else if (error_description.indexOf('2-Email Need To Confirm') > -1) {
                    notificationService.alert('خطأ', 'حسابك غير مفعل...من فضلك فعل حسابك من خلال إدخال الكود الخاص ببريدك الإليكتروني .', function () {
                        localStorage.setItem('UserEntersCode', 'false');
                        if (error_description.indexOf('/') > -1) {
                            var usrId = error_description.split('/')[1];
                            localStorage.setItem('UserID', usrId);
                        }
                        mainView.router.loadPage({ pageName: 'activationCode' });
                    });
                }
                else if (error_description === '3-Wating To Approve') {
                    notificationService.alert('خطأ', 'تم ايقاف حسابك من فضلك اتصل بادارة التطبيق', function () { });
                }
                else {
                    notificationService.alert('خطأ', 'يوجد خطأ في عملية التسجيل .', function () { });
                }
            }
            else if (typeof responseText.message != 'undefined') {
                var message = responseText.message;
                callback(null);
            }
        });
}






function getDeviceId() {
    //window.plugins.OneSignal
    //   .startInit("21624373-9d13-4bd5-9c93-9564218ad7bc")
    //   .handleNotificationOpened(notificationOpenedCallback)
    //   .endInit();

    //window.plugins.OneSignal.getIds(function (ids) {
    //    console.log('getIds: ' + JSON.stringify(ids));
    //    localStorage.setItem('deviceId', ids.userId);
    //    alert(deviceId);


    //    callBack(ids.userId);
    //});

}

function validateEmail(email) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

function GetCommentDuration(date1, date2) {
    //Get 1 day in milliseconds
    var one_day = 1000 * 60 * 60 * 24;

    // Convert both dates to milliseconds
    var date1_ms = date1.getTime();
    var date2_ms = date2.getTime();

    // Calculate the difference in milliseconds
    var difference_ms = date2_ms - date1_ms;
    //take out milliseconds
    difference_ms = difference_ms / 1000;
    var seconds = Math.floor(difference_ms % 60);
    difference_ms = difference_ms / 60;
    var minutes = Math.floor(difference_ms % 60);
    difference_ms = difference_ms / 60;
    var hours = Math.floor(difference_ms % 24);
    var days = Math.floor(difference_ms / 24);
    var txtDuration = '';

    if (days > 30) {
        txtDuration = 'منذ أكثر من شهر مضي .';
    } else {
        if (days > 0) {
            txtDuration = days + ' يوم ' + hours + ' ساعة ' + minutes + ' دقيقة ';
        }
        else {
            if (hours > 0) {
                txtDuration = hours + ' ساعة ' + minutes + ' دقيقة ';
            }
            else {
                if (minutes > 0) {
                    txtDuration = minutes + ' دقيقة ' + seconds + ' ث ';
                }
                else {
                    txtDuration = 'منذ قليل .';
                }
            }
        }
    }

    return txtDuration;
}

function drawComments(commentResult, divId) {
    var comments = document.getElementById(divId);


    for (var i = 0; i < commentResult.length; i++) {
        var lIComment = document.createElement('li');
        var divDrawed = document.createElement('div');
        var divCommentImage = document.createElement('div');
        var divchatText = document.createElement('div');
        var divChatTime = document.createElement('div');
        var imageUser = document.createElement('img');
        var divComment = document.createElement('div');
        divDrawed.setAttribute('class', 'chatavatar  commentLength');
        divCommentImage.className = "column-right chat";
        divchatText.className = "chat-text";
        divChatTime.className = "chat-time";
        divchatText.innerHTML = commentResult[i].commentText;
        divChatTime.innerHTML = commentResult[i].commentPeriod;
        imageUser.setAttribute('src', imgHost + commentResult[i].photoUrl);
        imageUser.setAttribute('onerror', 'replaceURL(this,"inner")');
        imageUser.setAttribute('alt', 'avatar');
        divCommentImage.appendChild(divchatText);
        divCommentImage.appendChild(divChatTime);
        divDrawed.appendChild(imageUser);
        lIComment.appendChild(divDrawed);
        lIComment.appendChild(divCommentImage);
        comments.appendChild(lIComment);
    }
}

function appendComment(comment, divId) {
    var comments = document.getElementById(divId);
    var lIComment = document.createElement('li');
    var divDrawed = document.createElement('div');
    var divCommentImage = document.createElement('div');
    var divchatText = document.createElement('div');
    var divChatTime = document.createElement('div');
    var imageUser = document.createElement('img');
    var divComment = document.createElement('div');

    divDrawed.setAttribute('class', 'chatavatar  commentLength');
    divCommentImage.className = "column-right chat";
    divchatText.className = "chat-text";
    divChatTime.className = "chat-time";
    divchatText.innerHTML = comment.commentText;
    divChatTime.innerHTML = comment.commentPeriod;
    imageUser.setAttribute('src', imgHost + localStorage.getItem('usrPhoto'));
    imageUser.setAttribute('onerror', 'replaceURL(this,"inner")');
    imageUser.setAttribute('alt', 'avatar');
    divCommentImage.appendChild(divchatText);
    divCommentImage.appendChild(divChatTime);
    divDrawed.appendChild(imageUser);
    lIComment.appendChild(divDrawed);
    lIComment.appendChild(divCommentImage);
    comments.insertBefore(lIComment, comments.firstChild);
}

function replaceURL(elem, name) {
    var imgElem = elem;
    if (name == "inner") {
        elem.src = 'img/logo.png';
    }
    else if (name == "head") {
        elem.src = 'img/logo.png';
    }
    else {
        elem.src = 'img/logo.png';
    }
}


//function CallService(pageName, CallType, MethodName, dataVariables, callback) {

//    if (pageName) {
//        ShowLoader(pageName);
//    }

//    var token = localStorage.getItem('appToken');
//    var contentType = "application/json";

//    if (MethodName.indexOf('api/User/FindByUserId') > -1
//        || MethodName.indexOf('api/TouristVisit/CalculateDynamicPageSize') > -1
//        || MethodName.indexOf('api/TouristVisit/GetById') > -1
//        || MethodName.indexOf('api/Account/ReSendConfirmationCode') > -1
//        || MethodName.indexOf('api/Account/LogOff') > -1) {
//        contentType = "application/x-www-form-urlencoded";
//    }

//    if (dataVariables != '' && dataVariables != null) {
//        dataVariables = JSON.stringify(dataVariables);
//    }

//    $.ajax({
//        type: CallType,
//        url: serviceURL + MethodName,
//        headers: {
//            "content-type": contentType,
//            "cache-control": "no-cache",
//            "authorization": "bearer " + token
//        },
//        data: dataVariables,
//        async: true,
//        crossDomain: true,
//    }).done(function (result) {
//        var res = result;
//        if (pageName) { HideLoader(pageName); }
//        callback(res);
//    }).fail(function (error, textStatus) {
//        callback(null);
//        if (pageName) { HideLoader(pageName); }
//        var pp = pageName;
//        console.log("Error in (" + MethodName + ") , Error text is : [" + error.responseText + "] , Error json is : [" + error.responseJSON + "] .");
//        if (typeof error.responseText != 'undefined') {
//            var responseText = JSON.parse(error.responseText);
//            if (error.status === 401) {
//                notificationService.alert('مدة صلاحية رمز التحقق الخاص بك قد انتهت , جاري تنشيط رمز التحقق  .', 'خطأ', function () {
//                    RefreshToken(pageName, CallType, 'Token', function (result) {
//                        localStorage.setItem('appToken', result.access_token);
//                        localStorage.setItem('refreshToken', result.refresh_token);
//                        notificationService.alert('تم تنشيط رمز التحقق الخاص بك , يرجي تسجيل دخولك مرة أخري  .', 'نجاح', function () {
//                            //localStorage.removeItem('USName');
//                            //localStorage.removeItem('userLoggedIn');
//                            //localStorage.removeItem('Visitor');
//                            //localStorage.removeItem('loginUsingSocial');
//                            //mainView.router.loadPage({ pageName: 'login' });
//                            mainView.router.loadPage({ pageName: 'intro' });
//                        });
//                    });
//                });
//            }
//            else {
//                if (typeof responseText.error_description != 'undefined') {
//                    var error_description = responseText.error_description;
//                    if (error_description === 'The user name or password is incorrect.') {
//                        notificationService.alert('خطأ في أسم المستخدم أو كلمة المرور .', 'خطأ', function () { });
//                    }
//                    else if (error_description === '1-User are Arhieve') {
//                        notificationService.alert('تمت أرشفة حسابك, من فضلك اتصل بإدارة التطبيق .', 'خطأ', function () { });
//                    }
//                    else if (error_description.indexOf('2-Email Need To Confirm') > -1) {
//                        notificationService.alert('حسابك غير مفعل...من فضلك فعل حسابك من خلال إدخال الكود الخاص ببريدك الإليكتروني .', 'خطأ', function () {
//                            localStorage.setItem('UserEntersCode', 'false');
//                            if (error_description.indexOf('/') > -1) {
//                                var usrId = error_description.split('/')[1];
//                                localStorage.setItem('UserID', usrId);
//                            }
//                            mainView.router.loadPage({ pageName: 'userCode' });
//                        });
//                    }
//                    else if (error_description === '3-Wating To Approve') {
//                        notificationService.alert('يجب تأكيدك من خلال المندوب اذا كنت طبيبا أو من خلال أدمن الموقع اذا كنت مندوبا.', 'خطأ', function () { });
//                    }
//                    else if (error_description.indexOf('Login') > -1 || error_description.indexOf('login') > -1) {
//                        notificationService.alert("يجب عليك تسجيل الدخول مجددا .", 'خطأ', function () {
//                            localStorage.removeItem('appToken');
//                            localStorage.removeItem('USName');
//                            localStorage.removeItem('refreshToken');
//                            localStorage.removeItem('userLoggedIn');
//                            localStorage.removeItem('Visitor');
//                            localStorage.removeItem('loginUsingSocial');
//                            mainView.router.loadPage({ pageName: 'login' });
//                        });
//                    }
//                    else {
//                        notificationService.alert('يوجد خطأ في عملية التسجيل .', 'خطأ', function () { });
//                    }
//                }
//                else if (typeof responseText.message != 'undefined' && responseText.message != "The request is invalid.") {
//                    var message = responseText.message;
//                    if (MethodName == 'Api/Account/ChangePassword' && message == 'An error has occurred.') {
//                        message = 'كلمة السر القديمة غير صحيحة .';
//                        notificationService.alert(message, 'خطأ', function () { });
//                    }
//                    else if (MethodName == 'api/Account/ConfirmEmail' && message == 'An error has occurred.') {
//                        message = 'كود التفعيل غير صحيح .';
//                        notificationService.alert(message, 'خطأ', function () { });
//                    }
//                    else if (message.indexOf('Login') > -1 || message.indexOf('login') > -1) {
//                        notificationService.alert("يجب عليك تسجيل الدخول مجددا .", 'خطأ', function () {
//                            localStorage.removeItem('appToken');
//                            localStorage.removeItem('USName');
//                            localStorage.removeItem('refreshToken');
//                            localStorage.removeItem('userLoggedIn');
//                            localStorage.removeItem('Visitor');
//                            localStorage.removeItem('loginUsingSocial');
//                            mainView.router.loadPage({ pageName: 'login' });
//                        });
//                    }
//                }
//                else if (typeof responseText.modelState != 'undefined') {
//                    if (typeof responseText.modelState.email != 'undefined') {
//                        notificationService.alert('هذا البريد الإليكتروني مستخدم من قبل .', 'خطأ', function () { });
//                    }
//                    else {
//                        var messages = responseText.modelState[""];
//                        var message = "";
//                        if (messages && messages.length > 0) {
//                            if (messages[0] === 'Incorrect password.') {
//                                notificationService.alert('كلمة السر القديمة غير صحيحة .', 'خطأ', function () { });
//                            }
//                            else if (messages[0] === "Email Must Be Unique") {
//                                notificationService.alert("البريد الإلكتروني مسجل من قبل", 'خطأ', function () { });
//                            }
//                            else if (messages[0] === "The Password must be at least 6 characters long.") {
//                                notificationService.alert("كلمة السر 6 حروف على الاقل", 'خطأ', function () { });
//                            }
//                            else if (messages[0] === "The password and confirmation password do not match.") {
//                                notificationService.alert("لا تتطابق كلمة السر مع تأكيد كلمة السر", 'خطأ', function () { });
//                            }
//                            else if (messages[0].startsWith('Name') && messages[0].endsWith('is already taken.')) {
//                                notificationService.alert("اسم المستخدم مسجل من قبل", 'خطأ', function () { });
//                            }
//                            else if (messages[0].startsWith('Phone Number') && messages[0].endsWith('is already taken.')) {
//                                notificationService.alert("رقم الجوال مسجل من قبل", 'خطأ', function () { });
//                            }
//                            else if (messages[0].startsWith('User name') && messages[0].endsWith('can only contain letters or digits.')) {
//                                notificationService.alert("اسم المستخدم يمكن ان يحتوي فقط على حروف وارقام", 'خطأ', function () { });
//                            }
//                            else if (messages[0].startsWith('Invalid token') > -1) {
//                                notificationService.alert("كود التفعيل غير صحيح  , برجاء تفقد البريد الالكترونى", 'خطأ', function () { });
//                            }
//                            else if (messages[0].indexOf('Login') > -1 || messages[0].indexOf('login') > -1) {
//                                notificationService.alert("يجب عليك تسجيل الدخول مجددا .", 'خطأ', function () {
//                                    localStorage.removeItem('appToken');
//                                    localStorage.removeItem('USName');
//                                    localStorage.removeItem('refreshToken');
//                                    localStorage.removeItem('userLoggedIn');
//                                    localStorage.removeItem('Visitor');
//                                    localStorage.removeItem('loginUsingSocial');
//                                    mainView.router.loadPage({ pageName: 'login' });
//                                });
//                            }
//                            else {
//                                for (var m = 0; m < messages.length; m++) {
//                                    message += "- [" + messages[m] + " ] - ";
//                                }

//                                notificationService.alert(message, 'خطأ', function () { });
//                            }
//                        }
//                        else {
//                            //callback(null);
//                        }
//                    }
//                }
//                else {
//                }
//            }
//        }
//        else {
//            notificationService.alert('خطأ في الخدمة .', 'خطأ', function () { });
//        }



//    });
//}






// new call service // 

function CallService(pageName, CallType, MethodName, dataVariables, callback) {

    ShowLoader();

    var token = localStorage.getItem('appToken');
    var contentType = "application/json";

    if (MethodName.indexOf('api/User/FindByUserId') > -1
        || MethodName.indexOf('api/TouristVisit/CalculateDynamicPageSize') > -1
        || MethodName.indexOf('api/TouristVisit/GetById') > -1
        || MethodName.indexOf('api/Account/ReSendConfirmationCode') > -1) {
        contentType = "application/x-www-form-urlencoded";
    }

    if (dataVariables != '' && dataVariables != null) {
        dataVariables = JSON.stringify(dataVariables);
    }

    $.ajax({
        type: CallType,
        url: serviceURL + MethodName,
        headers: {
            "content-type": contentType,
            "cache-control": "no-cache",
            "authorization": "bearer " + token
        },
        data: dataVariables,
        async: true,
        crossDomain: true,
        success: function (result) {

        },
        error: function (error) {

        }
    }).done(function (result, status, headers) {
        var res = result;
        HideLoader();
        return callback(res);
    })
        .fail(function (error, textStatus) {

            HideLoader();

            var pp = pageName;
            console.log(JSON.stringify(dataVariables));
            console.log("Error in (" + MethodName + ") , Error text is : [" + error.responseText + "] , Error json is : [" + error.responseJSON + "] .");
            if (typeof error.responseText != 'undefined' && error.responseText.indexOf("<!DOCTYPE html>") == -1) {
                var responseText = JSON.parse(error.responseText);

                if (error.status === 401) {
                    if (localStorage.getItem('refreshToken')) {
                        notificationService.alert('خطأ', 'مدة صلاحية رمز التحقق الخاص بك قد انتهت , جاري تنشيط رمز التحقق  .', function () {
                            RefreshToken(pageName, CallType, 'Token', function (result) {
                                localStorage.setItem('appToken', result.access_token);
                                localStorage.setItem('refreshToken', result.refresh_token);
                                notificationService.alert('نجاح', 'تم تنشيط رمز التحقق الخاص بك ', function () {
                                    //localStorage.removeItem('USName');
                                    //localStorage.removeItem('userLoggedIn');
                                    //localStorage.removeItem('Visitor');
                                    //localStorage.removeItem('loginUsingSocial');
                                    //mainView.router.loadPage({ pageName: 'login' });
                                    mainView.router.loadPage({ pageName: 'intro' });
                                });
                            });
                        });

                    } else {
                        notificationService.alert('دليل جزان', 'الرجاء القيام بالتسجيل او تسجيل الدخول اولا ', function () {
                            localStorage.removeItem('appToken');
                            localStorage.removeItem('USName');
                            localStorage.removeItem('refreshToken');
                            localStorage.removeItem('userLoggedIn');
                            localStorage.removeItem('Visitor');
                            localStorage.removeItem('loginUsingSocial');
                            localStorage.setItem('Visitor', true);
                            mainView.router.loadPage({ pageName: 'login' });
                        });
                    }
                }
                else if (error.status == 500) {
                    //notificationService.alert('خطأ', 'خطأ في الخدمة', function () {
                    //    notificationService.alert('دليل جزان', 'الرجاء القيام بالتسجيل او تسجيل الدخول اولا ', function () {
                    //        localStorage.removeItem('appToken');
                    //        localStorage.removeItem('USName');
                    //        localStorage.removeItem('refreshToken');
                    //        localStorage.removeItem('userLoggedIn');
                    //        localStorage.removeItem('Visitor');
                    //        localStorage.removeItem('loginUsingSocial');
                    //        localStorage.setItem('Visitor', true);
                    //        mainView.router.loadPage({ pageName: 'login' });
                    //    });
                    //});
                }
                else {
                    if (typeof responseText.error_description != 'undefined') {
                        var error_description = responseText.error_description;
                        if (error_description === 'The user name or password is incorrect.') {
                            notificationService.alert('خطأ', 'خطأ في اسم الدخول أو كلمة المرور .', function () { });
                        }
                        else if (error_description === '1-User are Arhieve') {
                            notificationService.alert('خطأ', 'تمت أرشفة حسابك, من فضلك اتصل بإدارة التطبيق .', function () { });
                        }
                        else if (error_description === '2-Email Need To Confirm') {
                            notificationService.alert('خطأ', 'حسابك غير مفعل...من فضلك فعل حسابك من خلال إدخال الكود الخاص ببريدك الإليكتروني .', function () {
                                localStorage.setItem('UserEntersCode', 'false');
                                mainView.router.loadPage({ pageName: 'activationCode' });
                            });
                        }
                        else {
                            notificationService.alert('خطأ', 'يوجد خطأ في عملية التسجيل .', function () { });
                        }
                    }
                    else if (typeof responseText.message != 'undefined' && responseText.message != "The request is invalid.") {
                        var message = responseText.message;
                        if (MethodName == 'Api/Account/ChangePassword' && message == 'An error has occurred.') {
                            message = 'كلمة السر القديمة غير صحيحة .';
                        }
                        notificationService.alert('خطأ', message, function () { });
                    }
                    else if (typeof responseText.modelState != 'undefined') {
                        if (typeof responseText.modelState.email != 'undefined') {
                            if (responseText.modelState.email[0] == 'Not Found') {
                                notificationService.alert('خطأ', 'هذا البريد الإلكتروني غير مسجل لدينا .', function () { });
                            }
                            else {
                                notificationService.alert('خطأ', 'هذا البريد الإليكتروني مستخدم من قبل .', function () { });
                            }

                        }
                        else if (typeof responseText.modelState.phoneNumber != 'undefined') {
                            notificationService.alert('خطأ', 'هذا الجوال مستخدم من قبل .', function () { });

                        }
                        else {
                            var messages = responseText.modelState[""];
                            var message = "";
                            if (messages.length > 0) {
                                if (messages[0] === 'Incorrect password.') {
                                    notificationService.alert('خطأ', 'كلمة السر القديمة غير صحيحة .', function () { });
                                }
                                else if (messages[0] === "Email Must Be Unique") {
                                    notificationService.alert('خطأ', "البريد الإلكتروني مسجل من قبل", function () { });
                                }
                                else if (messages[0] === "The Password must be at least 6 characters long.") {
                                    notificationService.alert('خطأ', "كلمة السر 6 حروف على الاقل", function () { });
                                }
                                else if (messages[0] === "The password and confirmation password do not match.") {
                                    notificationService.alert('خطأ', "لا تتطابق كلمة السر مع تأكيد كلمة السر", function () { });
                                }
                                else if (messages[0].startsWith('Name') && messages[0].endsWith('is already taken.')) {
                                    notificationService.alert('خطأ', "اسم المستخدم مسجل من قبل", function () { });
                                }
                                else if (messages[0].startsWith('Phone Number') && messages[0].endsWith('is already taken.')) {
                                    notificationService.alert('خطأ', "رقم الجوال مسجل من قبل", function () { });
                                }
                                else if (messages[0].startsWith('User name') && messages[0].endsWith('can only contain letters or digits.')) {
                                    notificationService.alert('خطأ', "اسم المستخدم يمكن ان يحتوي فقط على حروف وارقام", function () { });
                                }
                                else if (messages[0].indexOf('Invalid token') > -1) {
                                    notificationService.alert('خطأ', "كود التفعيل غير صحيح  , برجاء تفقد البريد الالكترونى", function () { });
                                }
                            }
                        }
                    }
                }
            }
            callback(null);
        });
}
