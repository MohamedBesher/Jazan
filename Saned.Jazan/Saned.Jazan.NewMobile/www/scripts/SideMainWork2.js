/// <reference path="../js/framework7.js" />
/// <reference path="../js/jquery-2.1.0.js" />
/// <reference path="CallService.js" />
/// <reference path="MainWork.js" />


function DrawIntroductionPage() {

    callWeatherService("intro", "GET", "http://api.aladhan.com/timingsByAddress?address=Jazan,%20Saudi%20Arabia&method=2", "", function (result) {
        if (result != null) {
            var fajr = document.getElementById('fajr');
            fajr.innerHTML = result.data.timings.Fajr;
            $('#shroukTime').text(result.data.timings.Sunrise);
            $('#DhuhrTime').text(result.data.timings.Dhuhr);
            $('#AsrTime').text(result.data.timings.Asr);
            $('#MaghribTime').text(result.data.timings.Maghrib);
            $('#IshaTime').text(result.data.timings.Isha);
        }
    });
}




//function initSwiper(id)
//{ }



function DrawPersonalAdImages(res) {
    var divPlaceImages = document.getElementById('adImgsDiv');
    divPlaceImages.innerHTML = "";
    for (var i = 0; i < res.length; i++) {

        var divMain = document.createElement('div');
        linkImageSlider = document.createElement('a');
        imgPlace = document.createElement('img');

        imgPlace.setAttribute('src', imgHost + res[i].imageUrl);

        divPlaceImages.className = "row";
        divMain.className = "col-25";

        linkImageSlider.className = "pb-popup-dark";
        linkImageSlider.setAttribute('id', res[i].id);
        linkImageSlider.appendChild(imgPlace);
        divMain.appendChild(linkImageSlider);
        divPlaceImages.appendChild(divMain)
    }

}



function onImgError(e) {
    e.src = "img/adv1.jpg";
}
function onBannerError(e) {
    e.src = "img/adv1.jpg";
}
function GoToIntroductionPage(page) {
    loadSideMenuLinks();
    getDeviceId();
    DrawIntroductionPage();

}
function GoToLoginPage(page) {
    if (typeof page != 'undefined') {

    }
}
var notificationOpenedCallback = function (jsonData) {
    mainView.router.loadPage({ pageName: 'notification' });
};
function loadSideMenuLinks() {

    var visitorStatue = localStorage.getItem('Visitor');
    if (visitorStatue == 'true' || visitorStatue == null) {

        $('#linkTologout').css('display', 'none');
        $('#linkTologin').css('display', '');
        $('#linkToadvertismentsProfile').css('display', 'none');
        $('#linkToaddAdvertisment').css('display', 'none');
        $('#linkToresetPass').css('display', 'none');
        $('#linkToadvertismentsProfile').css('display', 'none');
        $('#addAdvertisment').css('display', 'none');
        $('#notification').css('display', 'none');
        $('#btnTologin').css('display', 'block');
        $('#btnTologout').css('display', 'none');
        $('#linkTonotification').css('display', 'none');
    }
    else {
        $('#linkTologout').css('display', '');
        $('#linkTologin').css('display', 'none');
        $('#addAdvertisment').css('display', '');
        $('#notification').css('display', '');
        $('#linkToadvertismentsProfile').css('display', '');
        $('#linkToaddAdvertisment').css('display', 'block');
        $('#linkToresetPass').css('display', 'block');
        $('#btnTologin').css('display', 'none');
        $('#btnTologout').css('display', 'block');
        $('#linkTonotification').css('display', 'block');
    }
}
function drawMainSliderAdvertisements(advertiseMentResult, id) {

    if (advertiseMentResult && advertiseMentResult.length > 0) {
        $('#' + id).html(' <div class="owl-carousel" id="' + id + '-carousel"></div>');
        for (var i = 0; i < advertiseMentResult.length; i++) {
            var onClickMainBannerAd = '  mainView.router.loadPage({ pageName: \'advertismentsDetails\', query: { id: ' + advertiseMentResult[i].advertisementId + ' } });'
            $('#' + id + '-carousel').append(' <div class="item"><a onclick="' + onClickMainBannerAd + '"><img src="' + imgHost + advertiseMentResult[i].advertisementImageUrl + '"></a></div>');
        }


        $('#' + id + '-carousel').unbind().owlCarousel({
            rtl: true,
            loop: true,
            autoplay: true,
            autoplayTimeout: 1500,
            items: 1,
            margin: 0,
            nav: false,
        });


    } else {
        document.getElementById(id).innerHTML = "";
    }
}
function drawNewsSlider(result) {
   
    if (result && result.length > 0) {
        $('#newsSlider').html(' <div class="owl-carousel" id="news-carousel"></div>');
        for (var i = 0; i < result.length; i++) {
            var onClickFunction = 'mainView.router.loadPage({ pageName: \'newsDetails\', query: { id: ' + result[i].id + ' } });';
            $('#news-carousel').append(' <div class="item"><div class="swiper-slide"><a class="owl-img" onclick="' + onClickFunction + '"><p>' + result[i].title + '</p></a></div></div>');
        }


        $('#news-carousel').unbind().owlCarousel({
            rtl: true,
            loop: true,
            autoplay: true,
            autoplayTimeout: 1500,
            items: 1,
            margin: 0,
            nav: false,
        });


    } else {
        document.getElementById('newsSlider').innerHTML = "";
    }
}


function isUserLoggedIn() {
    var visitorStatue = localStorage.getItem('Visitor');
    return visitorStatue == 'true' || visitorStatue == null;
}