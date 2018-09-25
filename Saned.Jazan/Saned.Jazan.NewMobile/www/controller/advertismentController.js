var advertismentController = (function () {

    function drawSingleAdvertisement(singleAdvertisement) {

        $('#AdscontainerDiv').append('<div class="col-50">' +
                                '<a href="#advertismentsDetails" data-advertisement-id="' + singleAdvertisement.advertisementId + '">' +
                                    '<div class="card"> <img src="' + imgHost + singleAdvertisement.advertisementImageUrl + '">' +
                                       '<div class="dalelOverlay">' +
                                            '<h3>' + singleAdvertisement.advertisementName + '</h3> </div>' +
                                    '</div>' +
                                '</a>' +
                           ' </div>');
        $('[data-advertisement-id="' + singleAdvertisement.advertisementId + '"]').unbind().on('click', function () {

            var id = $(this).attr('data-advertisement-id');
            mainView.router.loadPage({ pageName: 'advertismentsDetails', query: { id: id } });
        });
    }

    function GoToadvertismentsPage(page) {

        $('#AdscontainerDiv').html('');
        myApp.detachInfiniteScroll($$('#AdsScrollDiv'));



        localStorageService.setValue(localStorageService.getAllKeys().advertismentsPageNumber, 1);

        var params = {
            'pageSize': 4,
            'pageNumber': localStorage.getItem("advertisments-page-number")
        };

        var adsPerentDiv = document.getElementById('AdscontainerDiv');

        CallService('advertisments', 'POST', 'api/Advertisements/GetPagedAdvertisement', params, function (result) {


            var newPageNumber = parseInt(localStorageService.getValue(localStorageService.getAllKeys().advertismentsPageNumber)) + 1;
            localStorageService.setValue(localStorageService.getAllKeys().advertismentsPageNumber, newPageNumber);

            if (result && result.advertisements && result.advertisements.length > 0) {
                drawSingleAdvertisementsAndBannerList(result);
            }

            if (result && result.length < localStorageService.getValue(localStorageService.getAllKeys().advertismentsPageNumber)) {
                myApp.detachInfiniteScroll($$('#AdsScrollDiv'));
                return;
            } else {
                myApp.attachInfiniteScroll($$('#AdsScrollDiv'));
            }

        });

        if (initAdvertismentsPage) {
            initAdvertismentsPage = false;

            var advertismentsInfiniteLoading = false;

            $$('#AdsScrollDiv').on('infinite', function () {

                if (advertismentsInfiniteLoading) return;
                advertismentsInfiniteLoading = true;

                var params = {
                    pageNumber: parseInt(localStorageService.getValue(localStorageService.getAllKeys().advertismentsPageNumber)),
                    pageSize: 4
                };

                var newPageNumber = parseInt(localStorageService.getValue(localStorageService.getAllKeys().advertismentsPageNumber)) + 1;
                localStorageService.setValue(localStorageService.getAllKeys().advertismentsPageNumber, newPageNumber);



                CallService('advertisments', 'POST', 'api/Advertisements/GetPagedAdvertisement', params, function (result) {

                    if (result && result.advertisements && result.advertisements.length > 0) {
                        drawSingleAdvertisementsAndBannerList(result);
                    }

                    advertismentsInfiniteLoading = false;
                    if (result && result.advertisements.length < 4) {
                        myApp.detachInfiniteScroll($$('#AdsScrollDiv'));
                        return;
                    }
                });
            });
        }
    }
    function GoToadvertismentsDetailsPage(page) {
        if (typeof page != 'undefined') {

            $('#advertisments-details-wrapper').html('');







            CallService('advertismentsDetails', 'GET', 'api/Advertisements/' + page.query.id, '', function (res) {

                if (res != null) {

                    var placeImages = [];
                    var html = '<div class="row"><div class="col-100"><p><b>' + res.advertisementName + '</b></p></div><div class="col-100"><p> <i class="ionicons ion-person"></i>' +
                                  res.createdByUserName + '</p></div> <div class="col-100"><p> <i class="ionicons ion-home"></i>' +
                                  res.cityName + '</p></div>';

                    if (res.images) {
                       
                    
                        for (var i = 0; i < res.images.length; i++) {
                            html = html + '<div class="col-25"><a href="#" class="pb-popup-dark" data-advertisement-details-image-index="' + i + '"><img src="' + imgHost + res.images[i].imageUrl + '"></a></div>';
                            placeImages.push(imgHost + res.images[i].imageUrl);
                        }
                    }





                    html = html + '<div class="col-100"><p class="">' + res.description + '</p></div><div class="col-100" id="advertisement-details-map" style="height:200px !important;width:100%" ></div>';

                    if (res.workingHours) {
                        html = html + '<div class="col-100">' + '<p> <i class="ionicons ion-clock"></i> ' + res.workingHours + ' </p> </div>';
                    }

                    if (res.mobile) {
                        html = html + '<div class="col-100"> <p id="advertisement-phone-number" class="phone-number"> <i class="ionicons ion-android-phone-portrait"></i> ' + res.mobile + ' </p> </div>';
                    }

                    if (res.email) {
                        html = html + '<div class="col-100"> <p id="advertisement-email" class="phone-number"> <i class="ionicons ion-android-mail"></i> ' + res.email + ' </p>  </div>';
                    }

                    if (res.webSite) {
                        html = html + ' <div class="col-100">  <p id="advertisement-details-website" class="phone-number"> <i class="ionicons ion-earth"></i>' + res.webSite + ' </p> </div>';
                    }

                    html = html + '<div class="col-100"><div class="social">';
                    if (res.faceBook) {
                        html = html + '<a id="advertisement-details-facebook" href="' + res.faceBook + '"><img src="img/facebook.png"></a>';


                    }
                    if (res.twitter) {
                        html = html + '<a id="advertisement-details-twitter" href="' + res.twitter + '"><img src="img/twitter.png"></a>';


                    }
                    if (res.instagram) {
                        html = html + '<a id="advertisement-details-instagram" href="' + res.instagram + '"><img src="img/instagram.png"></a>';


                    }


                    html = html + '</div></div></div>';

                    $('#advertisments-details-wrapper').html(html);

                    if (res.mobile) {

                        $("#advertisement-phone-number").unbind().click(function () {
                            window.plugins.CallNumber.callNumber(onAdvertisementNumberSuccess, onAdvertisementNumberError, res.mobile, true);
                        });

                        function onAdvertisementNumberSuccess(result) {
                            console.log("Success:" + result);
                        }

                        function onAdvertisementNumberError(result) {
                            console.log("Error:" + result);
                        }
                    }

                    if (res.faceBook) {

                        $("#advertisement-details-facebook").unbind().click(function () {
                            var ref = cordova.InAppBrowser.open(res.faceBook, '_system', 'location=no');
                        });
                    }
                    if (res.twitter) {

                        $("#advertisement-details-twitter").unbind().click(function () {
                            var ref = cordova.InAppBrowser.open(res.twitter, '_system', 'location=no');
                        });
                    }
                    if (res.instagram) {

                        $("#advertisement-details-instagram").unbind().click(function () {
                            var ref = cordova.InAppBrowser.open(res.instagram, '_system', 'location=no');
                        });
                    }

                    if (res.webSite) {
                        $("#advertisement-details-website").unbind().click(function () {
                            var ref = cordova.InAppBrowser.open(res.webSite, '_system', 'location=no');
                        });
                    }

                    if (res.email) {
                        $("#advertisement-email").unbind().click(function () {
                            var ref = cordova.InAppBrowser.open("mailto:" + res.email, '_system', 'location=no');
                        });
                    }

                    if (res.latitude && res.longitude) {
                        initMapWithCoordinates('advertisement-details-map', res.latitude, res.longitude, res.advertisementName, true);
                    }

                    if (placeImages.length > 0) {
                        var myPhotoBrowserPopupDark = myApp.photoBrowser({
                            photos: placeImages,
                            theme: 'dark',
                            type: 'popup',
                            backLinkText: 'إغلاق',
                            ofText: 'من'
                        });

                       
                        $$('.pb-popup-dark').on('click', function () {
                            var imageIndex = $(this).attr('data-advertisement-details-image-index');
                            myPhotoBrowserPopupDark.open(imageIndex);
                        });
                    }


                    //placeImages = [];
                    // 
                    //document.getElementById('adTitle').innerHTML = res.advertisementName;
                    //document.getElementById('adOwner').innerHTML += res.createdByUserName;
                    //document.getElementById('adCity').innerHTML += res.cityName;
                    //document.getElementById('adDetails').innerHTML = res.description;
                    ////document.getElementById('adMap').innerHTML = res.latitude + res.longitude;
                    //document.getElementById('adphone').innerHTML += res.mobile;
                    //document.getElementById('adEmail').innerHTML += res.email;
                    //document.getElementById('adWebSite').innerHTML += res.webSite;
                    //document.getElementById('AdWorkHours').innerHTML += res.workingHours;
                    //document.getElementById('adFbUl').src = res.faceBook;
                    //document.getElementById('adTwitterUrl').src = res.twitter;
                    //document.getElementById('adInstgramUrl').src = res.instagram;

                    //if (res.images != null) {
                    //    DrawPersonalAdImages(res.images);
                    //    for (var i = 0; i < res.images.length; i++) {
                    //        placeImages.push(imgHost + res.images[i].imageUrl);
                    //        $('#' + res.images[i].id).on('click', function () {
                    //            var myPhotoBrowserPopupDark = myApp.photoBrowser({
                    //                photos: placeImages,
                    //                theme: 'dark',
                    //                type: 'popup',
                    //                backLinkText: 'إغلاق',
                    //                ofText: 'من'
                    //            });
                    //            myPhotoBrowserPopupDark.open();

                    //        });
                    //    }
                    //}

                }

            });

            // //   
            // var map = new google.maps.Map(document.getElementById('ad-location-map'), {
            //     center: { lat: adDetailsQuery.latitude, lng: adDetailsQuery.longitude },
            //     zoom: 13,
            //     mapTypeId: 'roadmap'
            // });

        }
    }
    function DrawBanner(banner) {
        //var bannerhref = document.createElement("a");
        //bannerhref.className = '';
        //var bannerImg = document.createElement("img");
        //bannerImg.setAttribute('onerror', 'onBannerError(this);');
        //bannerImg.className = 'inner_ads';
        //bannerhref.setAttribute('id', 'linkBannerPersonalAdDetails_' + adsAndBannerList.banner.advertisementId);
        //bannerImg.src = imgHost + imgUrl;
        //bannerhref.appendChild(bannerImg);
        //advertisementDiv.appendChild(bannerhref);

        $('#AdscontainerDiv').append('<div class="col-100"><a data-banner-advertisement-id =' + banner.advertisementId
            + '> <img src="' + imgHost + banner.advertisementImageUrl + '" class="inner_ads"> </a> </div>');


        $('[data-banner-advertisement-id="' + banner.advertisementId + '"]').unbind().on('click', function () {

            var id = $(this).attr('data-banner-advertisement-id');

            mainView.router.loadPage({ pageName: 'advertismentsDetails', query: { id: id } });

        });
    }
    function drawSingleAdvertisementsAndBannerList(result) {

        if (result) {
            if (result.advertisements) {
                for (var index = 0; index < result.advertisements.length; index++) {
                    var adDetails = result.advertisements[index];
                    drawSingleAdvertisement(adDetails);
                }
            }
            if (result.banner) {

                DrawBanner(result.banner);
            }
        }



    }

    var service = {
        GoToadvertismentsPage: GoToadvertismentsPage,
        GoToadvertismentsDetailsPage: GoToadvertismentsDetailsPage

    };

    return service;

})();

