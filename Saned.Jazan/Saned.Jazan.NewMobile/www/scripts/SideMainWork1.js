
function resetTouristDetailsControls() {

    localStorageService.removeByKey(localStorageService.getAllKeys().touristDetailsAddressLatitude);
    localStorageService.removeByKey(localStorageService.getAllKeys().touristDetailsAddressLongtitude);
    localStorageService.removeByKey(localStorageService.getAllKeys().touristDetailsAddress);

    $('#txtTouristDetailsLocationName').val('');
    $("#add-tourist-details-multiple-videos-wrapper").html('');
    $('#txtTouristDetailsCity').val('');
    $('#dtpTouristDetailsDate').val('');
    $('#txtAreaLocationInformation').val('');
    $('#add-tourist-details-multiple-images-wrapper').html('');
    $('#imgMainVisit').attr('src', 'img/profile.jpg');

    $('#add-tourist-details').parsley().reset();
}

$$('.popup-addTourist').on('opened', function () {

    ////Intialize google api map 
    //$('#add-tourist-details-map-wrapper').html('<input id="pac-input" class="controls" type="text" placeholder="اسم الموقع السياحى"><div id="add-tourist-details-map" style="width:100%;height:200px;"></div>');

    //intializeSearchMapLocation('Jazan', '24.713552', '46.675296', 'Jazan', 'add-tourist-details-map');
    $('#add-tourist-details-map-wrapper').html('<div id="add-tourist-details-map" style="width:100%;height:200px;"></div>');
    initMapWithCoordinates('add-tourist-details-map', '16.889359', '42.570567', 'Jazan', false);
    //reset controls 
    resetTouristDetailsControls();


});

$('#add-tourist-details-multiple-videos').unbind().click(function () {
    //var videoLink = $('#txt-tourist-details-video-link').val();
    //var regExp = /^((?:https?:)?\/\/)?((?:www|m)\.)?((?:youtube\.com|youtu.be))(\/(?:[\w\-]+\?v=|embed\/|v\/)?)([\w\-]+)(\S+)?$/;
    //var matchingResult = videoLink.match(regExp);
    //if (matchingResult) {
    //    var html = '<div class="img_content"><img src="https://i.ytimg.com/vi/' + matchingResult[5] + '/hqdefault.jpg" /></div>';
    //    $('#add-tourist-details-multiple-videos-wrapper').append(html);
    //} else {
    //    notificationService.alert('خطا', 'ادخل رابط يوتيوب صحيح');
    //}

    //$('#txt-tourist-details-video-link').val('');

    var videoLink = $('#txt-tourist-details-video-link').val();
    var regExp = /^((?:https?:)?\/\/)?((?:www|m)\.)?((?:youtube\.com|youtu.be))(\/(?:[\w\-]+\?v=|embed\/|v\/)?)([\w\-]+)(\S+)?$/;
    var matchingResult = videoLink.match(regExp);
    if (matchingResult) {
        var index = $('#add-tourist-details-multiple-videos-wrapper div.col-25').length;

        var subMainPicShow = document.getElementById('add-tourist-details-multiple-videos-wrapper');
        var divDrawed = document.createElement('div');
        divDrawed.setAttribute('id', 'add-tourist-visit-video-' + index);
        var imageAdvertisementSubMain = document.createElement('img');
        divDrawed.className = "col-25";
        imageAdvertisementSubMain.setAttribute('src', 'https://i.ytimg.com/vi/' + matchingResult[5] + '/hqdefault.jpg');
        imageAdvertisementSubMain.className = 'image-responsive';

        var deleteImageButton = document.createElement('a');
        deleteImageButton.className = 'button button-round red';
        deleteImageButton.setAttribute('onclick', 'notificationService.confirm("هل انت متأكد من حذف الفيديو",function(buttonIndex){  if(buttonIndex==1){ $("#add-tourist-visit-video-' + index + '").remove(); }})');

        deleteImageButton.innerHTML = 'حذف';


        divDrawed.appendChild(imageAdvertisementSubMain);
        divDrawed.appendChild(deleteImageButton);
        subMainPicShow.insertBefore(divDrawed, subMainPicShow.firstChild);
    } else {
        notificationService.alert('خطأ', 'ادخل رابط يوتيوب صحيح');
    }

    $('#txt-tourist-details-video-link').val('');

});

$('#add-tourist-details-image').unbind().click(function () {
    navigator.camera.getPicture(onSuccess, onFail, {
        quality: 50,
        destinationType: Camera.DestinationType.DATA_URL,
        sourceType: Camera.PictureSourceType.SAVEDPHOTOALBUM,
    });
    function onSuccess(imageURI) {
        var image = document.getElementById('imgMainVisit');
        image.src = 'data:image/png;base64,' + imageURI;
    }
    function onFail(message) {
        //notificationService.alert('حدث خطأ :' + message);
    }
});
$('#add-tourist-details-multiple-images').unbind().click(function () {

    navigator.camera.getPicture(onSuccess, onFail, {
        quality: 50,
        destinationType: Camera.DestinationType.DATA_URL,
        sourceType: Camera.PictureSourceType.SAVEDPHOTOALBUM,
    });

    function onSuccess(imageURI) {
        var index = $('#add-tourist-details-multiple-images-wrapper div.col-25').length;

        var subMainPicShow = document.getElementById('add-tourist-details-multiple-images-wrapper');
        var divDrawed = document.createElement('div');
        divDrawed.setAttribute('id', 'add-tourist-visit-image-' + index);
        var imageAdvertisementSubMain = document.createElement('img');
        divDrawed.className = "col-25";
        imageAdvertisementSubMain.setAttribute('src', 'data:image/jpeg;base64, ' + imageURI);
        imageAdvertisementSubMain.className = 'image-responsive';

        var deleteImageButton = document.createElement('a');
        deleteImageButton.className = 'button button-round red';
        deleteImageButton.setAttribute('onclick', 'notificationService.confirm("هل انت متأكد من حذف الصورة",function(buttonIndex){  if(buttonIndex==1){ $("#add-tourist-visit-image-' + index + '").remove(); }})');

        deleteImageButton.innerHTML = 'حذف';


        divDrawed.appendChild(imageAdvertisementSubMain);
        divDrawed.appendChild(deleteImageButton);
        subMainPicShow.insertBefore(divDrawed, subMainPicShow.firstChild);

    }
    function onFail(message) {
        //notificationService.alert('حدث خطأ :' + message);
    }
});
$('#addTourist').unbind().click(function () {
    if (isUserLoggedIn()) {
        notificationService.alert("دليل جازان", "يجب عليك تسجيل الدخول حتى تتمكن من اضافة زيارة سياحية", function () { })
    } else {
        myApp.popup('.popup-addTourist');
        $('#btn-add-tourist-details').unbind().click(function () {

            if ($('#add-tourist-details').parsley().validate()) {
                $('#btn-add-tourist-details').attr('disabled', true);

                var latitude = localStorageService.getValue(localStorageService.getAllKeys().touristDetailsAddressLatitude);
                var longtitude = localStorageService.getValue(localStorageService.getAllKeys().touristDetailsAddressLongtitude);

                if (!latitude || !longtitude) {
                    notificationService.alert('خطا', 'الرجاء اختيار موقع الزيارة');
                    $('#btn-add-tourist-details').attr('disabled', false);

                } else {
                    if ($('#imgMainVisit').attr('src') == 'img/profile.jpg') {
                        notificationService.alert('خطا', 'الرجاء اختيار صورة اساسية للزيارة');
                        $('#btn-add-tourist-details').attr('disabled', false);
                    } else {


                        var mainImageBase64 = $('#imgMainVisit').attr('src').replace('data:image/png;base64,', '');

                        var secondaryImages = $('#add-tourist-details-multiple-images-wrapper div.col-25 img');
                        var secondaryImagesArray = [];
                        var touristVisitsVideos = [];
                        for (var i = 0; i < secondaryImages.length; i++) {
                            secondaryImagesArray.push(
                                {
                                    "base64": $(secondaryImages[i]).attr('src').replace('data:image/jpeg;base64, ', ''),
                                    "imageExtension": "jpg"
                                });
                        }

                        $("#add-tourist-details-multiple-videos-wrapper div img").each(
                            function () {
                                touristVisitsVideos.push($(this).attr('src')
                                    .replace('https://i.ytimg.com/vi/', '').replace('/hqdefault.jpg', ''));
                            });


                        dataContextService.touristVisits.add($('#txtTouristDetailsLocationName').val().trim(),
                             $('#txtTouristDetailsCity').val().trim(),
                              $('#dtpTouristDetailsDate').val(),
                              latitude,
                              longtitude,
                              $('#txtAreaLocationInformation').val().trim(),
                              mainImageBase64,
                              touristVisitsVideos,
                              secondaryImagesArray,
                               function (viewsRes) {
                                   $('#btn-add-tourist-details').attr('disabled', false);
                                   if (viewsRes) {
                                       myApp.closeModal();
                                       notificationService.alert('خطا', 'تم اضافة زيارة سياحية بنجاح', function () {
                                           mainView.router.reloadPage('tourist');
                                       });
                                   }
                               });
                    }
                }
            }

        });
    }


});

function DrawTouristPage(res) {


    var divTouristVisit = document.getElementById('touristVisit');

    for (var i = 0; i < res.touristVisits.length; i++) {

        var divMainViewsRates = document.createElement('div');
        var divRate = document.createElement('div');
        divMainViewsRates.setAttribute('class', 'row det');
        divRate.setAttribute('class', 'col-100');
        divRate.setAttribute('id', 'rateYo' + res.touristVisits[i].id);

        var LinkTouristDetails = document.createElement('a');
        var divMain = document.createElement('div');
        var divSupMain = document.createElement('div');
        var divPlaceName = document.createElement('div');
        var hplaceName = document.createElement('h3');
        var pVisitDetails = document.createElement('p');
        var divViews = document.createElement('div');
        var pViews = document.createElement('p');
        var iViews = document.createElement('i');
        var space = document.createTextNode("\u00A0");
        var labelViews = document.createElement('label');

        var divVisitImage = document.createElement('div');
        var imgVisit = document.createElement('img');




        LinkTouristDetails.setAttribute('id', 'linkTouristDetails_' + res.touristVisits[i].id)
        divMain.setAttribute('class', 'card touristLength');
        divSupMain.setAttribute('class', 'row');
        divPlaceName.setAttribute('class', 'col-60');
        hplaceName.innerHTML = res.touristVisits[i].name;
        pVisitDetails.setAttribute('class', 'des');
        pVisitDetails.innerHTML = res.touristVisits[i].description;
        divViews.setAttribute('class', 'col-50');
        iViews.setAttribute('class', 'ionicons ion-eye');
        divVisitImage.setAttribute('class', 'col-40');
        imgVisit.setAttribute('src', imgHost + res.touristVisits[i].imageUrl);

        divVisitImage.appendChild(imgVisit);



        pViews.appendChild(iViews);
        pViews.appendChild(space);
        labelViews.innerHTML = res.touristVisits[i].views;
        pViews.appendChild(labelViews);
        divViews.appendChild(pViews);

        divMainViewsRates.appendChild(divViews);
        divMainViewsRates.appendChild(divRate);

        divPlaceName.appendChild(hplaceName);
        divPlaceName.appendChild(pVisitDetails);
        divPlaceName.appendChild(divMainViewsRates);
        divSupMain.appendChild(divPlaceName);
        divSupMain.appendChild(divVisitImage);
        divMain.appendChild(divSupMain);
        LinkTouristDetails.appendChild(divMain);
        divTouristVisit.appendChild(LinkTouristDetails);

        $("#rateYo" + res.touristVisits[i].id).rateYo({
            rating: res.touristVisits[i].rating,
            spacing: "1px",
            starWidth: "15px",
            readOnly: true
        });



        $('#linkTouristDetails_' + res.touristVisits[i].id).unbind().on('click', function () {

            var touristDetailsId = $(this).attr('id').split('_')[1];
            mainView.router.loadPage({ pageName: 'touristDetails', query: { id: touristDetailsId } });
        });
    }


    if (res.banner && res.banner[0]) {

        var divAdvertisementTourist = document.createElement('div');
        var linkAdvertisementDetails = document.createElement('a');
        var imgAdvertisement = document.createElement('img');

        imgAdvertisement.setAttribute('src', imgHost + res.banner[0].advertisementImageUrl);
        imgAdvertisement.setAttribute('class', 'inner_ads');
        linkAdvertisementDetails.setAttribute('id', 'linkPlaceDetails_' + res.banner[0].advertisementId);
        linkAdvertisementDetails.appendChild(imgAdvertisement);
        divAdvertisementTourist.appendChild(linkAdvertisementDetails);
        divTouristVisit.appendChild(divAdvertisementTourist);



        $('#linkPlaceDetails_' + res.banner[0].advertisementId).on('click', function () {
            initDalel = true;
            var bannerDetailsId = $(this).attr('id').split('_')[1];
            localStorageService.setValue(localStorageService.getAllKeys().bannerDetailsId, bannerDetailsId);
            mainView.router.loadPage({ pageName: 'advertismentsDetails', query: { id: bannerDetailsId } });
        });
    }


}

function GoToTouristPage(page) {
    if (typeof page != 'undefined') {


        myApp.detachInfiniteScroll($$('#touristInfinite'));
        $$('.infinite-scroll-preloader').remove();
        var divTouristVisit = document.getElementById('touristVisit');
        divTouristVisit.innerHTML = '';
        localStorage.setItem('tourist-page-number', 1);
        localStorage.setItem('tourist-comment-page-number', 1);

      

        if (iniTouristPage == true) {
            iniTouristPage = false;
            var touristInfiniteLoading = false;

            $$('#touristInfinite').on('infinite', function () {
                if (touristInfiniteLoading) return;
                touristInfiniteLoading = true;

                dataContextService.touristVisits.getPaged(
                    localStorage.getItem('tourist-page-size'),
                    localStorage.getItem('tourist-page-number'),
                    null,
                    true,
                    function (result) {
                        if (result) {
                            touristInfiniteLoading = false;
                            localStorage.setItem('tourist-page-number', parseInt(localStorage.getItem('tourist-page-number')) + 1);

                            if (result && result.touristVisits && result.touristVisits.length > 0) {
                                DrawTouristPage(result);
                            }


                            if (result && result.touristVisits.length < localStorage.getItem('tourist-page-size')) {
                                myApp.detachInfiniteScroll($$('#touristInfinite'));
                                $$('.infinite-scroll-preloader').remove();
                                return;
                            }
                        }
                    }
                )
            });
        }

        dataContextService.touristVisits.calculateDynamicPageSize(function (pageSize) {
            if (pageSize) {
                localStorage.setItem('tourist-page-size', pageSize);


                dataContextService.touristVisits.getPaged(
                     localStorage.getItem('tourist-page-size'),
                     localStorage.getItem('tourist-page-number'), null, true, function (result) {
                         if (result) {
                             localStorage.setItem('tourist-page-number', parseInt(localStorage.getItem('tourist-page-number')) + 1);

                             DrawTouristPage(result);

                             if (result && result.touristVisits.length < localStorage.getItem('tourist-page-size')) {
                                 myApp.detachInfiniteScroll($$('#touristInfinite'));
                                 $$('.infinite-scroll-preloader').remove();
                                 return;
                             } else {
                                 myApp.attachInfiniteScroll($$('#touristInfinite'));
                             }
                         }
                     });

            } else {
                $$('.infinite-scroll-preloader').remove();
            }
        });





    }
}

function GoToTouristDetailsPage(page) {


    $('#tourist-visits-comment').html('');
    myApp.detachInfiniteScroll($$('#tourist-comments-infinite'));
    $$('.infinite-scroll-preloader').remove();


    if (typeof page != 'undefined') {
        var touristDetailsId = page.query.id;
        if (initTouristDetailsPage == true) {
            initTouristDetailsPage = false;

            var loading = false;
            $$('#tourist-comments-infinite').on('infinite', function () {
                if (loading) return;
                loading = true;

                dataContextService.comments.getPaged(localStorage.getItem('tourist-comment-page-number'),
                                                     localStorage.getItem('tourist-visit-id'),
                                                     2,
                        function (commentResult) {
                            if (commentResult) {
                                loading = false;
                                localStorage.setItem('tourist-comment-page-number', parseInt(localStorage.getItem('tourist-comment-page-number')) + 1);

                                drawComments(commentResult, 'tourist-visits-comment');

                                if (commentResult && commentResult.length < 5) {
                                    myApp.detachInfiniteScroll($$('#tourist-comments-infinite'));
                                    $$('.infinite-scroll-preloader').remove();
                                    return;
                                }


                            }
                        });

            });
        }


        dataContextService.view.add(touristDetailsId, 2, function () { });
        dataContextService.touristVisits.getById(touristDetailsId, function (res) {
            if (res != null) {
                $('#tourist-details-page-location-visit-rating').rateYo().rateYo("destroy");
                $("#tourist-details-page-location-calc-visit-rating").rateYo().rateYo("destroy");
                $('#tourist-details-page-location-name').text('');
                $('#tourist-details-page-location-comment-text-area').val('');
                $('#tourist-visits-comment').html('');
                $('#tourist-details-page-location-visit-date').text('');
                $('#tourist-details-page-location-visit-visitor-name').text('');
                $('#tourist-details-page-location-visit-map').attr('src', '');
                $('#tourist-details-page-images-wrapper').html('');
                $('#tourist-details-page-location-visit-description').text('');
                $('#tourist-details-page-location-visit-visitor-name').text('');
                $('#tourist-details-page-location-name').text(res.name);
                $('#tourist-details-page-location-visit-visitor-name').text(res.userName);
                $('#tourist-details-page-location-city-name').text(res.cityName);

                if (res.createdOn) {
                    var month = new Date(res.createdOn).getUTCMonth() + 1;
                    var day = new Date(res.createdOn).getUTCDate();
                    var year = new Date(res.createdOn).getUTCFullYear();

                    $('#tourist-details-page-location-visit-date').text(day + '/' + month + '/' + year);
                }

                initMapWithCoordinates('tourist-details-map', res.latitude, res.longitude, res.name, true);

                var placePictureDiv = document.createElement('div');
                placePictureDiv.setAttribute('class', 'row');

                var placeImages = [];

                for (var i = 0; i < res.imagesUrl.length; i++) {

                    var col25Div = document.createElement('div');
                    col25Div.setAttribute('class', 'col-25');

                    var imageAnchor = document.createElement('a');
                    imageAnchor.setAttribute('href', '#');
                    imageAnchor.setAttribute('class', 'pb-popup-dark');
                    imageAnchor.setAttribute('data-tourist-details-image-index', i);


                    var imageAnchorImage = document.createElement('img');
                    imageAnchorImage.setAttribute('src', imgHost + res.imagesUrl[i].mediaUrl);

                    imageAnchor.appendChild(imageAnchorImage);

                    imageAnchor.appendChild(imageAnchorImage);

                    col25Div.appendChild(imageAnchor);

                    placePictureDiv.appendChild(col25Div);

                    placeImages.push(imgHost + res.imagesUrl[i].mediaUrl);
                }


                if (res.youTubeUrls != null && res.youTubeUrls.length > 0) {
                    for (var i = 0; i < res.youTubeUrls.length; i++) {
                        var col25Div = document.createElement('div');
                        col25Div.setAttribute('class', 'col-25');

                        var imageAnchor = document.createElement('a');
                        imageAnchor.setAttribute('href', '#');
                        imageAnchor.setAttribute('class', 'pb-popup-dark');

                        var imageAnchorImage = document.createElement('img');
                        imageAnchorImage.setAttribute('src', 'https://i.ytimg.com/vi/' + res.youTubeUrls[i].mediaUrl + '/hqdefault.jpg');

                        imageAnchor.appendChild(imageAnchorImage);

                        imageAnchor.appendChild(imageAnchorImage);

                        col25Div.appendChild(imageAnchor);

                        placePictureDiv.appendChild(col25Div);


                        placeImages.push({ html: '<iframe style="height: 200px !important; width: 100%;"  width="560" height="315" src="https://www.youtube.com/embed/' + res.youTubeUrls[i].mediaUrl + '" frameborder="0" allowfullscreen></iframe>' });
                    }
                }

                $('#tourist-details-page-images-wrapper').html(placePictureDiv.outerHTML);

                $('#tourist-details-page-location-visit-description').text(res.description);

                $('#tourist-details-page-location-visit-rating').rateYo({
                    rating: parseInt(res.rating),
                    spacing: "1px",
                    starWidth: "20px",
                    readOnly: true
                });



                if (placeImages.length > 0) {

                    myPhotoBrowserPopupDark = myApp.photoBrowser({
                        photos: placeImages,
                        theme: 'dark',
                        type: 'popup',
                        backLinkText: 'إغلاق',
                        ofText: 'من'
                    });

                    $$('.pb-popup-dark').on('click', function () {
                        var imageIndex = $(this).attr('data-tourist-details-image-index');
                        myPhotoBrowserPopupDark.open(imageIndex);
                    });
                }

                localStorage.setItem('tourist-visit-id', res.id);

                $("#tourist-details-page-location-calc-visit-rating").rateYo({
                    rating: 0,
                    starWidth: "30px",
                    numStars: 5,
                    halfStar: false,
                    fullStar: true,
                    readOnly: false,
                    spacing: "5px",
                    onSet: function (rating, rateYoInstance) {
                        if (isUserLoggedIn()) {
                            notificationService.alert("دليل جازان", "يجب عليك تسجيل الدخول حتى تتمكن من اضافة تقييم", function () { })
                        } else {
                            dataContextService.rating.add(rating, localStorageService.getValue(localStorageService.getAllKeys().touristVisitId), 2,
                                function (res) {
                                    if (res != null) {
                                        notificationService.alert('نجاح', 'شكرا لك علي التقييم .', function () { });
                                    }
                                });
                        }
                    }
                });

                var rateyo = $('#tourist-details-page-location-calc-visit-rating').rateYo({
                    fullStar: true
                });

                $('#tourist-details-page-location-send-comment').unbind().on('click', function () {
                    if (isUserLoggedIn()) {
                        notificationService.alert("دليل جازان", "يجب عليك تسجيل الدخول حتى تتمكن من اضافة تعليق", function () { })
                    } else {
                        var txtAreaComment = $('#tourist-details-page-location-comment-text-area').val();
                        if (txtAreaComment != '') {

                            dataContextService.comments.add(txtAreaComment,
                                localStorageService.getValue(localStorageService.getAllKeys().touristVisitId), 2, function (res) {
                                    if (res != null) {
                                        appendComment(res, 'tourist-visits-comment');
                                        $('#tourist-details-page-location-comment-text-area').val('');
                                    }
                                });

                        } else {
                            notificationService.alert('تنبيه', 'من فضلك قم بادخال تعليق', function () { });
                        }
                    }
                });


                dataContextService.comments.getPaged(localStorage.getItem('tourist-comment-page-number'),
                                                    localStorage.getItem('tourist-visit-id'),
                                                    2,
                             function (commentResult) {
                                 drawComments(commentResult, 'tourist-visits-comment');
                                 localStorage.setItem('tourist-comment-page-number', parseInt(localStorage.getItem('tourist-comment-page-number')) + 1);

                                 if (commentResult && commentResult.length < 5) {
                                     myApp.detachInfiniteScroll($$('#tourist-comments-infinite'));
                                     $$('.infinite-scroll-preloader').remove();
                                     return;
                                 } else {
                                     myApp.attachInfiniteScroll($$('#tourist-comments-infinite'));
                                 }
                             });

            }
            else {
                notificationService.alert('خطأ', 'خطأ في إسترجاع بيانات الزيارة السياحية');
            }
        });
    }
}




