var mobileSettingController = (function () {





    var drawAboutUsPage = function (data) {

        var mainWrapper = $('#about-us-wrapper');

        mainWrapper.html('<div class="card"><p>' + data + '</p></div>');


    };


    var drawContactUsPage = function (data) {

        var mainWrapper = $('#contact-us-wrapper');
        if (data && data.length > 0) {
            $.each(data, function (i) {
                if (data[i].id == appSettingService.mobileSettingEnum.mobileNumber.id) {
                    mainWrapper.append('<h4 id="contact-us-wrapper-whatsup"><img src="img/' + appSettingService.mobileSettingEnum.mobileNumber.icon + '"> <span>' + data[i].value + '</span></h4>');
                } else if (data[i].id == appSettingService.mobileSettingEnum.line.id) {
                    mainWrapper.append('<h4 id="contact-us-wrapper-line"><img src="img/' + appSettingService.mobileSettingEnum.line.icon + '"> <span>' + data[i].value + '</span></h4>');
                } else if (data[i].id == appSettingService.mobileSettingEnum.twitter.id) {
                    mainWrapper.append('<h4 id="contact-us-wrapper-twitter"><img src="img/' + appSettingService.mobileSettingEnum.twitter.icon + '"> <span>' + data[i].value + '</span></h4>');
                } else if (data[i].id == appSettingService.mobileSettingEnum.instgram.id) {
                    mainWrapper.append('<h4 id="contact-us-wrapper-instgram"><img src="img/' + appSettingService.mobileSettingEnum.instgram.icon + '"> <span>' + data[i].value + '</span></h4>');
                } else if (data[i].id == appSettingService.mobileSettingEnum.snapchat.id) {
                    mainWrapper.append('<h4 id="contact-us-wrapper-snapchat"><img src="img/' + appSettingService.mobileSettingEnum.snapchat.icon + '"> <span>' + data[i].value + '</span></h4>');
                } else if (data[i].id == appSettingService.mobileSettingEnum.blackBerry.id) {
                    mainWrapper.append('<h4 id="contact-us-wrapper-blackberry"><img src="img/' + appSettingService.mobileSettingEnum.blackBerry.icon + '"> <span>' + data[i].value + '</span></h4>');
                } else if (data[i].id == appSettingService.mobileSettingEnum.email.id) {
                    mainWrapper.append('<h4 id="contact-us-wrapper-email"><img src="img/' + appSettingService.mobileSettingEnum.email.icon + '"> <span>' + data[i].value + '</span></h4>');
                }

            });
        } else {
            //mainWrapper.append('<div class="divAlert"><p class="pNotification"> لا يوجد بيانات </p></div>');
        }
    };

    var goToAboutUsPage = function (page) {
        if (typeof page != 'undefined') {
            dataContextService.mobileSetting.getAboutUs(function (result) {
                if (result && result.length > 0) {
                    drawAboutUsPage(result[0].value);
                } else {
                    var mainWrapper = $('#about-us-wrapper');
                    //mainWrapper.append('<div class="divAlert"><p class="pNotification"> لا يوجد بيانات </p></div>');
                }
            });
        }
    };

    var goToContactUs = function () {
        dataContextService.mobileSetting.getContactUs(function (result) {
            $('#contact-us-wrapper').html('');
            drawContactUsPage(result);
        });
    };



    var service = {
        goToAboutUsPage: goToAboutUsPage,
        goToContactUs: goToContactUs
    };

    return service;

})();

