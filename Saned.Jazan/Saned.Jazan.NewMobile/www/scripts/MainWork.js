/// <reference path="../js/framework7.js" />
/// <reference path="CallService.js" />
/// <reference path="../js/jquery-2.1.0.js" />

var AdSubImageArray = [];
var myPhotoBrowserPopupDark;
function resetAddProjectDetailsForm() {
    localStorage.removeItem('profileImageURI');
    //localStorage.removeItem('subMainCategoryId')
    //localStorage.removeItem('packageId');
    localStorage.removeItem('tourist-details-address-latitude');
    localStorage.removeItem('tourist-details-address-address-longtitude');
    localStorage.removeItem('tourist-details-address');
    $('#txtAdName').val('');
    $('#txtCiName').val('');
    $('#textAreaAdvertisementDetails').val('');
    $('#txtTimingHours').val('');
    $('#txtMobile').val('');
    $('#txtEmail').val('');
    $('#txtWebsite').val('');
    $('#txtTwitter').val('');
    $('#txtFacebook').val('');
    $('#txtInstgram').val('');
    $('#txtSnapChat').val('');
    $('#mainPicSrc').attr('src', 'img/profile.jpg');
    AdSubImageArray = [];
    $('#subMainPicShow').html('');

    $('#add-project-details').parsley().reset();
}
function DrawMainCategoryOption(res, id) {
    var mainCategoryOption = document.getElementById(id);
    mainCategoryOption.innerHTML = "";

    if (res && res.length > 0) {
        localStorage.setItem('subMainCategoryId', res[0].categoryId);
    }

    for (var i = 0; i < res.length; i++) {
        var category = document.createElement('option');
        category.innerHTML = res[i].categoryName;

        category.setAttribute('value', res[i].categoryId);
        mainCategoryOption.appendChild(category);
    }


}
function DrawDalelPage(res) {
    var divDalelCategories = document.getElementById('dalelCategories');
    divDalelCategories.innerHTML = "";

    for (var i = 0; i < res.length; i++) {
        var divMainDrawed = document.createElement('div');
        var linkSubCategories = document.createElement('a');
        var divImageText = document.createElement('div');
        var imgcategory = document.createElement('img');
        var divText = document.createElement('div');
        var hCategoryText = document.createElement('h3');
        hCategoryText.innerHTML = res[i].categoryName;
        divText.className = "dalelOverlay";
        imgcategory.setAttribute('src', adminPanelURL + res[i].imageUrl);
        imgcategory.setAttribute('onerror', 'replaceURL(this,"inner")');

        divImageText.className = "card";
        linkSubCategories.setAttribute('id', 'linkDalelSubCategory_' + res[i].categoryId);
        linkSubCategories.setAttribute('data-category-name', res[i].categoryName);
        divMainDrawed.className = "col-50";
        divText.appendChild(hCategoryText);
        divImageText.appendChild(imgcategory);
        divImageText.appendChild(divText);
        linkSubCategories.appendChild(divImageText);
        divMainDrawed.appendChild(linkSubCategories);
        divDalelCategories.appendChild(divMainDrawed);
        $('#linkDalelSubCategory_' + res[i].categoryId).on('click', function () {

            var categoryId = $(this).attr('id').split('_')[1];
            localStorage.setItem('categotyId', categoryId);
            mainView.router.loadPage({ pageName: 'categoryDetails', query: { categoryId: categoryId, categoryName: $(this).attr('data-category-name') } });



        });
    }


}

function DrawAdvertisementsSubMainImages(imageURI, index) {
    var subMainPicShow = document.getElementById('subMainPicShow');
    var divDrawed = document.createElement('div');
    divDrawed.setAttribute('id', 'add-ads-image-' + index);
    var imageAdvertisementSubMain = document.createElement('img');
    divDrawed.className = "col-25";
    imageAdvertisementSubMain.setAttribute('src', 'data:image/jpeg;base64, ' + imageURI);
    imageAdvertisementSubMain.className = 'image-responsive';

    var deleteImageButton = document.createElement('a');
    deleteImageButton.className = 'button button-round red';
    deleteImageButton.setAttribute('onclick', ' notificationService.confirm("هل انت متأكد من حذف الصورة",function(buttonIndex){  if(buttonIndex==1){ AdSubImageArray.splice(' + index + ',1); $("#' + 'add-ads-image-' + index + '").remove(); }})');

    deleteImageButton.innerHTML = 'حذف';


    divDrawed.appendChild(imageAdvertisementSubMain);
    divDrawed.appendChild(deleteImageButton);
    subMainPicShow.insertBefore(divDrawed, subMainPicShow.firstChild);
}
function DrawQuizPage() {
    var divWinners = document.getElementById('winners');
    divWinners.innerHTML = "";
    for (var i = 0; i < 6; i++) {
        var divMinnerImageName = document.createElement('div');
        var imgWinner = document.createElement('img');
        var pName = document.createElement('p');
        divMinnerImageName.setAttribute('class', 'col-33');
        imgWinner.setAttribute('src', 'img/profile.jpg');
        pName.innerHTML = "اسم الفائز";

        divMinnerImageName.appendChild(imgWinner);
        divMinnerImageName.appendChild(pName);
        divWinners.appendChild(divMinnerImageName);
    }
}
function DrawComments(commentResult) {
    var comments = document.getElementById('comments');


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
        divChatTime.innerHTML = "3  minutes ago";
        imageUser.setAttribute('src', imgHost + commentResult[i].photoUrl);
        imageUser.setAttribute('alt', 'avatar');
        divCommentImage.appendChild(divchatText);
        divCommentImage.appendChild(divChatTime);
        divDrawed.appendChild(imageUser);
        lIComment.appendChild(divDrawed);
        lIComment.appendChild(divCommentImage);
        comments.appendChild(lIComment);
    }
}
function DrawNewsPage(res) {
    var divNews = document.getElementById('news');

    for (var i = 0; i < res.length; i++) {
        var divNewsLinkTitle = document.createElement('div');
        var linkNewsDetails = document.createElement('a');
        var divNewsTitle = document.createElement('div');
        var pNewstitle = document.createElement('p');
        divNewsLinkTitle.setAttribute('class', 'col-100 newsLength');
        linkNewsDetails.setAttribute('id', 'linkNewsDetails_' + res[i].newsId);
        divNewsTitle.setAttribute('class', 'card');
        pNewstitle.innerHTML = res[i].title;
        divNewsTitle.appendChild(pNewstitle);
        linkNewsDetails.appendChild(divNewsTitle);
        divNewsLinkTitle.appendChild(linkNewsDetails);
        divNews.appendChild(divNewsLinkTitle);

        $('#linkNewsDetails_' + res[i].newsId).on('click', function () {

            var newId = $(this).attr('id').split('_')[1];
            mainView.router.loadPage({ pageName: 'newsDetails', query: { newId: newId } });

        });
    }
}

function DrawUserVisits(res) {
    var divpersonalVisits = document.getElementById('usersVisits');

    for (var i = 0; i < res.touristVisits.length; i++) {
        var divMainElement = document.createElement('div');
        var linkVisitDetails = document.createElement('a');
        var divVisitPicureName = document.createElement('div');
        var imgVisit = document.createElement('img');
        var divVisitName = document.createElement('div');
        var hVisitName = document.createElement('h3');
        var linkDeleteproject = document.createElement('a');
        var labelLinkDelete = document.createElement('label');

        divMainElement.className = "col-50 visitsLength";
        linkVisitDetails.setAttribute('id', 'linkVisitDetails_' + res.touristVisits[i].id);
        divVisitPicureName.className = "card";
        imgVisit.setAttribute('src', imgHost + res.touristVisits[i].imageUrl);
        divVisitName.className = "dalelOverlay";
        hVisitName.innerHTML = res.touristVisits[i].name;
        linkDeleteproject.setAttribute('id', 'linkDeleteTouristVisit_' + res.touristVisits[i].id);
        linkDeleteproject.setAttribute('class', 'button button-round red');
        labelLinkDelete.innerHTML = "حذف";
        linkDeleteproject.appendChild(labelLinkDelete)
        divVisitName.appendChild(hVisitName);
        divMainElement.appendChild(linkVisitDetails);
        divMainElement.appendChild(linkDeleteproject);
        divVisitPicureName.appendChild(imgVisit);
        divVisitPicureName.appendChild(divVisitName);
        linkVisitDetails.appendChild(divVisitPicureName);
        divMainElement.appendChild(linkVisitDetails);
        divMainElement.appendChild(linkDeleteproject);
        divpersonalVisits.appendChild(divMainElement);
        $('#linkDeleteTouristVisit_' + res.touristVisits[i].id).on('click', function () {

            var elementVisitDeleted = $(this).attr('id').split('_')[1];
            alert(elementVisitDeleted);


            CallService('advertismentsProfileVisits', 'DELETE', 'api/TouristVisit/Delete/' + elementVisitDeleted, '', function (res) {
                if (res != null) {
                    notificationService.alert('تم', 'تم الحذف  بنجاح', function () { });
                }
                else {
                    notificationService.alert('تنبيه', 'لم يتم الحذف ', function () { });
                }


            });


        });
        $('#linkVisitDetails_' + res.touristVisits[i].id).on('click', function () {

            var elementVisit = $(this).attr('id').split('_')[1];
            mainView.router.loadPage({ pageName: 'advertismentsProfileDetails', query: { elementVisitId: elementVisit } });


        });
    }

}
function DrawPlacePage(res) {
    var divPlace = document.getElementById('place');

    for (var i = 0; i < res.advertisements.length; i++) {
        var LinkPlaceDetails = document.createElement('a');
        var divMain = document.createElement('div');
        var divSupMain = document.createElement('div');
        var divPlaceName = document.createElement('div');
        var hplaceName = document.createElement('h3');
        var labelAdvertisementType = document.createElement('label');
        var divMainViewsRates = document.createElement('div');
        var divViews = document.createElement('div');
        var pViews = document.createElement('p');
        var iViews = document.createElement('i');
        var space = document.createTextNode("\u00A0");
        var labelViews = document.createElement('label');
        var divRate = document.createElement('div');
        var divPlaceImage = document.createElement('div');
        var imgPlace = document.createElement('img');

        if (res.banner) {
            var divAdvertisementTourist = document.createElement('div');
            var linkAdvertisementDetails = document.createElement('a');
            var imgAdvertisement = document.createElement('img');

            imgAdvertisement.setAttribute('src', imgHost + res.banner.advertisementImageUrl);
            imgAdvertisement.setAttribute('class', 'inner_ads');
            linkAdvertisementDetails.setAttribute('id', 'linkBannerDalelDetails_' + res.banner.advertisementId);
            linkAdvertisementDetails.appendChild(imgAdvertisement);
            divAdvertisementTourist.appendChild(linkAdvertisementDetails);
        }






        LinkPlaceDetails.setAttribute('id', 'advertisementDetails_' + res.advertisements[i].advertisementId);
        divMain.setAttribute('class', 'card placeLength');
        divSupMain.setAttribute('class', 'row');
        divPlaceName.setAttribute('class', 'col-50');
        hplaceName.innerHTML = res.advertisements[i].advertisementName;
        labelAdvertisementType.innerHTML = res.advertisements[i].packageName;
        labelAdvertisementType.className = "free";

        divMainViewsRates.setAttribute('class', 'row det');
        divViews.setAttribute('class', 'col-100');
        iViews.setAttribute('class', 'ionicons ion-eye');

        iViews.innerHTML = res.advertisements[i].views;
        divRate.setAttribute('class', 'col-100');
        divRate.setAttribute('id', 'rateYo' + res.advertisements[i].advertisementId);
        divPlaceImage.setAttribute('class', 'col-50');
        imgPlace.setAttribute('src', imgHost + res.advertisements[i].advertisementImageUrl);
        divPlaceImage.appendChild(imgPlace);
        pViews.appendChild(iViews);
        pViews.appendChild(space);
        divViews.appendChild(pViews);
        divMainViewsRates.appendChild(divViews);
        divMainViewsRates.appendChild(divRate);
        divPlaceName.appendChild(hplaceName);
        divPlaceName.appendChild(labelAdvertisementType);

        divPlaceName.appendChild(divMainViewsRates);
        divSupMain.appendChild(divPlaceName);
        divSupMain.appendChild(divPlaceImage);
        divMain.appendChild(divSupMain);
        LinkPlaceDetails.appendChild(divMain);
        divPlace.appendChild(LinkPlaceDetails);

        $('#advertisementDetails_' + res.advertisements[i].advertisementId).on('click', function () {

            var placeDetails = $(this).attr('id').split('_')[1];
            localStorage.setItem('placeDetails', placeDetails);
            mainView.router.loadPage({ pageName: 'placeDetails', query: { placeDetails: placeDetails } });

        });


        if (res.advertisements[i].rating != null) {
            $("#rateYo" + res.advertisements[i].advertisementId).rateYo({

                rating: res.advertisements[i].rating,
                spacing: "1px",
                starWidth: "20px",
                readOnly: true
            });
        }
        else {

            $("#rateYo" + res.advertisements[i].advertisementId).rateYo({

                rating: 0,
                spacing: "1px",
                starWidth: "20px",
                readOnly: true
            });

        }




    }
    if (res.banner) {
        divPlace.appendChild(divAdvertisementTourist);


        $('#linkBannerDalelDetails_' + res.banner.advertisementId).on('click', function () {
            initDalel = true;

            var bannerDetailsId = $(this).attr('id').split('_')[1];
            localStorage.setItem('bannerDetailsId', bannerDetailsId);

            mainView.router.loadPage({ pageName: 'advertismentsDetails', query: { id: bannerDetailsId } });

        });
    }



}
function DrawWeatherSlider(result) {
    $('#fajrTime').text(result.data.timings.Asr);
    $('#shrouk').text(result.data.timings.Sunrise);
    $('#Dhuhr').text(result.data.timings.Dhuhr);
}
function DrawDalelSubCategories(res) {
    var divDalelSubCategories = document.getElementById('subCategories');
    divDalelSubCategories.innerHTML = "";

    for (var i = 0; i < res.length; i++) {
        var divMainDrawede = document.createElement('div');
        var linkAdvertisement = document.createElement('a');
        var divSubCategoryImageText = document.createElement('div');
        var imgsubCategory = document.createElement('img');
        var divSubCategoryText = document.createElement('div');
        var hSubCategoryText = document.createElement('h3');
        divMainDrawede.className = "col-50";
        linkAdvertisement.setAttribute('id', 'linkPlaceDetails_' + res[i].categoryId);
        linkAdvertisement.setAttribute('data-category-name', res[i].categoryName);
        divSubCategoryImageText.className = "card";
        imgsubCategory.setAttribute('src', adminPanelURL + res[i].imageUrl);
        imgsubCategory.setAttribute('onerror', 'replaceURL(this,"inner")');
        divSubCategoryText.className = "dalelOverlay";
        hSubCategoryText.innerHTML = res[i].categoryName;
        divSubCategoryText.appendChild(hSubCategoryText);
        divSubCategoryImageText.appendChild(imgsubCategory);
        divSubCategoryImageText.appendChild(divSubCategoryText);
        linkAdvertisement.appendChild(divSubCategoryImageText);
        divMainDrawede.appendChild(linkAdvertisement);
        divDalelSubCategories.appendChild(divMainDrawede);
        $('#linkPlaceDetails_' + res[i].categoryId).on('click', function () {
            var placeDetailsId = $(this).attr('id').split('_')[1];
            localStorage.setItem('subCategoryId', placeDetailsId);
            mainView.router.loadPage({ pageName: 'place', query: { placeId: placeDetailsId, categoryName: $(this).attr('data-category-name') } });
        });
    }
}
function DrawAddAdvertismentPage(res) {
    var packageType = document.getElementById('packageType');
    packageType.innerHTML = "";

    $('.popup-overlay').css('display', 'none');

    for (var i = 0; i < res.length; i++) {

        var pCategoryName = document.createElement('p');
        var linkCategoryDetails = document.createElement('a');

        linkCategoryDetails.innerHTML = res[i].arabicName;
        linkCategoryDetails.setAttribute('id', 'linkCategoryDetails_' + res[i].id);
        //linkCategoryDetails.setAttribute('data-popup', '.popup-diamond');
        linkCategoryDetails.setAttribute('class', 'button button-round blue');
        pCategoryName.appendChild(linkCategoryDetails);

        packageType.appendChild(pCategoryName);
        $('#linkCategoryDetails_' + res[i].id).on('click', function () {


            var packageId = $(this).attr('id').split('_')[1];
            if (packageId == 1) {
                $('#diamond').attr('class', 'popup popup-diamond');
            } else if (packageId == 2) {
                $('#diamond').attr('class', 'popup popup-gold');
            } else if (packageId == 3) {
                $('#diamond').attr('class', 'popup popup-silver');
            } else {
                $('#diamond').attr('class', 'popup popup-free');
            }
            myApp.popup('#diamond');

            var clickLink = this;

            localStorage.setItem('packageId', packageId);
            CallService('dalel', 'Get', 'api/Packages/' + packageId, '', function (res) {

                if (res != null) {

                    var featuresArray = [];
                    var packageFeatureQuantity = 0;
                    for (var i = 0; i < res.length; i++) {
                        featuresArray.push(res[i].featureId);
                        if (res[i].featureId == 15) {
                            packageFeatureQuantity = res[i].packageFeatureQuantity;
                        }

                    }


                    if (featuresArray.contains(1)) { localStorage.setItem('mainBanner', 'true'); }
                    else {
                        localStorage.setItem('mainBanner', 'false');
                    }

                    if (featuresArray.contains(2)) { localStorage.setItem('categoryBetweenBanner', 'true'); }
                    else {
                        localStorage.setItem('categoryBetweenBanner', 'false');
                    }

                    if (featuresArray.contains(3)) { localStorage.setItem('map', 'true'); }
                    else {
                        localStorage.setItem('map', 'false');
                    }

                    if (featuresArray.contains(4)) { localStorage.setItem('advertisementEdit', 'true'); }
                    else {
                        localStorage.setItem('advertisementEdit', 'false');
                    }

                    if (featuresArray.contains(5)) { localStorage.setItem('uploadImageNoLimit', 'true'); }
                    else {
                        localStorage.setItem('uploadImageNoLimit', 'false');
                    }

                    if (featuresArray.contains(6)) { localStorage.setItem('upCategoryBanner', 'true'); }
                    else {
                        localStorage.setItem('upCategoryBanner', 'false');
                    }

                    if (featuresArray.contains(7)) { localStorage.setItem('advertisementNotification', 'true'); }
                    else {
                        localStorage.setItem('advertisementNotification', 'false');
                    }

                    if (featuresArray.contains(8)) { localStorage.setItem('snapChat', 'true'); }
                    else {
                        localStorage.setItem('snapChat', 'false');
                    }

                    if (featuresArray.contains(9)) { localStorage.setItem('instgram', 'true'); }
                    else {
                        localStorage.setItem('instgram', 'false');
                    }

                    if (featuresArray.contains(10)) { localStorage.setItem('facebook', 'true'); }
                    else {
                        localStorage.setItem('facebook', 'false');
                    }

                    if (featuresArray.contains(11)) { localStorage.setItem('twitter', 'true'); }
                    else {
                        localStorage.setItem('twitter', 'false');
                    }

                    if (featuresArray.contains(12)) { localStorage.setItem('website', 'true'); }
                    else {
                        localStorage.setItem('website', 'false');
                    }

                    if (featuresArray.contains(13)) { localStorage.setItem('email', 'true'); }
                    else {
                        localStorage.setItem('email', 'false');
                    }

                    if (featuresArray.contains(14)) { localStorage.setItem('mobile', 'true'); }
                    else {
                        localStorage.setItem('mobile', 'false');
                    }

                    if (featuresArray.contains(15)) { localStorage.setItem('uploadImageLimited', 'true'); localStorage.setItem('uploadImageLimited-count', packageFeatureQuantity); }
                    else {
                        localStorage.setItem('uploadImageLimited', 'false'); localStorage.setItem('uploadImageLimited-count', 0);
                    }
                    //}
                    DrawBackageFeatures(res);

                }
                else {

                    //notificationService.alert('خطا في استرجاع مميزات الباقة', 'خطا', function () { });
                }

            });
        });
    }
}
function DrawBackageFeatures(res) {
    var categoryFeatures = document.getElementById('categoryFeatures');
    categoryFeatures.innerHTML = "";
    $('#packageArabicName').text(res[0].packageArabicName);
    $('#categoryPrice').text(res[0].price + "ريال فقط");
    $('#categoryDuration').text(res[0].period + "شهر");

    for (var i = 0; i < res.length ; i++) {

        if (res[i].featureId != 8
            && res[i].featureId != 9
            && res[i].featureId != 10
            && res[i].featureId != 11
            && res[i].featureId != 12
            && res[i].featureId != 13
            && res[i].featureId != 14) {
            pCategoryfeatures = document.createElement('p');
            labelCategoryName = document.createElement('label');

            labelCategoryName.innerHTML = res[i].packageFeatureQuantity ? res[i].featureArabicName + ' (' + res[i].packageFeatureQuantity + ')' : res[i].featureArabicName;
            pCategoryfeatures.appendChild(labelCategoryName);
            categoryFeatures.appendChild(pCategoryfeatures);
        }


    }



}
function GoToDalelPage(page) {
    if (typeof page != 'undefined') {
        //loadSideMenuLinks();
        params = {
            "LanguageId": 0,
            "PageNumber": 1,
            "PageSize": 10
        };
        CallService('dalel', 'POST', 'api/Categories/GetCategories/0', params, function (res) {
            if (res != null) {
                DrawDalelPage(res);
            }
            else {
                //notificationService.alert('خطا في استرجاع  الدليل ', 'تنبيه', function () { });
            }
        });


    }
}
function GoToAdvertismentsProfileDetailsPage() {
    if (typeof page != "undefined") {
        DrawAdvertisementsSliderPage('advertismentsSliderProfileDetails');
       
        $('#projectDetails #txtProjectName').text("");
        $('#projectDetails #labelView').text("");
        $('#projectDetails #txtCityName').text("");
        $('#projectDetails #txtAdvertisementDuration').text("");
        $('#projectDetails #labelBackageType').text("");
        var userProjectId = localStorage.getItem('userProjectId');

        CallService('advertismentsProfileDetails', 'GET', 'api/Advertisements/' + localStorage.getItem('userProjectId'), '', function (res) {

            if (res != null) {
                $('#projectDetails #txtProjectName').text(res.advertisementName);

                if (res.isApproved == true) {
                    $('#projectDetails #labelProjectApproved').show();
                    $('#projectDetails #labelProjectRefuse').hide();

                }
                else {

                    $('#projectDetails #labelProjectRefuse').show();
                    $('#projectDetails  #labelProjectApproved').hide();
                }

                $('#projectDetails #labelView').text();
                $('#projectDetails #txtCityName').text(res.cityName);
                $('#projectDetails #txtAdvertisementDuration').text();
                $('#projectDetails #labelBackageType').text(res.packageName);
                $('#projectDetails #padvertisementDetails').text(res.description);

                $('#projectDetails #userPhoneContact').text(res.mobile);
                $('#projectDetails #userMailContact').text(res.email);
                $('#projectDetails #userWebsiteContact').text(res.webSite);
                $('#projectDetails #userTwitterContact').text(res.instagram);
                $('#projectDetails #userFacebookContact').text(res.faceBook);
                $('#projectDetails #userInstgramContact').text(res.instagram);
                $('#projectDetails #userSnapChatContact').text(res.snapChat);


                if (res.images != null) {
                    DrawProjectDetailImage(res.images);
                }


                $('#btnDeleteUserProjectPicture').on('click', function () {

                    DrawProjectDetailImageForDeleted(res.images)

                });

            }
            else {


            }

        });


    }


}
function DrawProjectDetailImage(res) {
    var projectImages = document.getElementById('projectImages');
    projectImages.innerHTML = "";
    for (var i = 0; i < res.length; i++) {

        var divDrawed = document.createElement('div');
        var linkImageBrowsing = document.createElement('a');
        var imgProject = document.createElement('img');
        divDrawed.className = "col-25";
        imgProject.setAttribute('src', imgHost + res[i].imageUrl);
        linkImageBrowsing.className = "pb-popup-dark2";

        linkImageBrowsing.appendChild(imgProject);
        divDrawed.appendChild(linkImageBrowsing);
        projectImages.appendChild(divDrawed);
    }



};
function GoToUserProfilePage() {

}
function GoToProfileProjects(page) {
    myApp.attachInfiniteScroll($$('#projectsInfinite'));

    if (typeof page != 'undefined') {
        //loadSideMenuLinks();
        var loading = false;
        var maxItems = 0;
        var last = 0;
        var divpersonalProjects = document.getElementById('personalProjects');
        divpersonalProjects.innerHTML = "";
        var params = {
            "PageSize": 8,

            "PageNumber": 1,

            "UserId": "591ee08a-7f68-49cd-9a48-e74ec574c679"
        };



        CallService('advertismentsProfileProjects', 'POST', 'api/Advertisements/GetPagedAdvertisement', params, function (res) {
            if (res != null) {
                DrawUserProjects(res);

                localStorage.setItem('maxProfileAdvertisement', res.advertisements[0].overallCount);
            }
            else {
                //notificationService.alert('خطا اثناء استرجاع الاعلانات الشخصية', 'تنبيه', function () { });

            }


        });
        if (initUserProjectsInfinite == true) {
            initUserProjectsInfinite = false;
            $$('#projectsInfinite').on('infinite', function () {
                if (loading) return;

                // Set loading flag
                loading = true;

                $$('.loading img').css('display', '');


                last = $$('#personalProjects div.projectsLength').length;

                var maxItems = localStorage.getItem('maxProfileAdvertisement');


                if (last >= maxItems) {
                    myApp.detachInfiniteScroll($$('#projectsInfinite'));

                    $$('.loading img').css('display', 'none');
                    return;
                }
                else {
                    $$('.loading img').css('display', '');
                    var params = {
                        "PageSize": 8,

                        "PageNumber": parseInt(parseInt(last / 8) + 1),

                        "UserId": "591ee08a-7f68-49cd-9a48-e74ec574c679"
                    };



                    CallService('advertismentsProfileProjects', 'POST', 'api/Advertisements/GetPagedAdvertisement', params, function (res) {
                        if (res != null) {
                            loading = false;
                            DrawUserProjects(res);

                        }
                        else {
                            //notificationService.alert('خطا اثناء استرجاع الاعلانات الشخصية', 'تنبيه', function () { });

                        }


                    });

                    last = $$('#personalProjects div.projectsLength').length;

                }

            });

        }

    }
}
function GoToAddAdvertismentPage(page) {
    if (typeof page != 'undefined') {
        //loadSideMenuLinks();



        CallService('addAdvertisment', 'Get', 'api/Packages', '', function (res) {
            if (res != null) {
                DrawAddAdvertismentPage(res);

            }
            else {

                //notificationService.alert('خطا في استرجاع نوع الباقة', 'خطا', function () { })
            }
        });
    }
}
function GoToDalelSubCategoriesPage(page) {
    if (typeof page != 'undefined') {
        //loadSideMenuLinks();
        var parentId = localStorage.getItem('categotyId');

        $('#category-details-title').text(page.query.categoryName);

        params = {

            "LanguageId": 0,
            "PageNumber": 1,
            "PageSize": 10

        };
        CallService('dalel', 'POST', 'api/Categories/GetCategories/' + parentId, params, function (res) {

            if (res != null) {
                DrawDalelSubCategories(res);
            }
            else {

                //notificationService.alert('خطا في استرجاع اعلانات هذا الدليل', 'خطا', function () { });
            }

        });

    }
}
function GoToPlacePage(page) {
    if (typeof page != 'undefined') {
        $('#place-category-name').text(page.query.categoryName);
        myApp.detachInfiniteScroll($$('#placesInfinte'));

        localStorage.setItem('place-page-number', '1');


        var initInfinitePlace = false;
        var divPlace = document.getElementById('place');
        divPlace.innerHTML = "";
        var subCategoryId = localStorage.getItem('subCategoryId');
        CallService('place', 'Get', 'api/Advertisements/CalculateDynamicPageSize/' + subCategoryId, "",
        function (result) {
            if (result != null && result != "NaN" && result != 0) {
                localStorage.setItem('pageSizeInfinite', result);
                var params = {
                    "PageSize": result,
                    "PageNumber": 1,
                    "CategoryId": subCategoryId
                };
                CallService('place', 'POST', 'api/Advertisements/GetPagedAdvertisement', params, function (res) {
                    if (res != null) {
                        if (res.advertisements.length < localStorage.getItem('pageSizeInfinite')) {
                            myApp.detachInfiniteScroll($$('#placesInfinte'));
                        }
                        else {
                            myApp.attachInfiniteScroll($$('#placesInfinte'));
                            var maxItemsPlaces = res.advertisements[0].overallCount;
                            localStorage.setItem('maxItemsPlaces', maxItemsPlaces);
                            DrawPlacePage(res);

                        }

                    }
                });
            }
        });
        var placesInfiniteLoading = false;
        if (initPlacesInfinite == true) {
            initPlacesInfinite = false;
            $$('#placesInfinte').on('infinite', function () {
                if (placesInfiniteLoading) return;
                placesInfiniteLoading = true;
                var pageNumber = parseInt(localStorage.getItem('place-page-number')) + 1;
                var placesParams = {
                    "PageSize": localStorage.getItem('pageSizeInfinite'),
                    "PageNumber": pageNumber,
                    "CategoryId": localStorage.getItem('subCategoryId')
                };

                localStorage.setItem('place-page-number', pageNumber);

                CallService('place', 'POST', 'api/Advertisements/GetPagedAdvertisement', placesParams,
            function (places) {
                if (places) {

                    DrawPlacePage(places);
                    if (places.advertisements.length < localStorage.getItem('pageSizeInfinite')) {
                        myApp.detachInfiniteScroll($$('#placesInfinte'));


                    }

                } else {
                    myApp.detachInfiniteScroll($$('#placesInfinte'));
                }
                placesInfiniteLoading = false;
            });
            });
        }
    }
}
function DrawPlaceImages(res) {
    var divPlaceImages = document.getElementById('divPlaceImages');
    divPlaceImages.innerHTML = "";
    for (var i = 0; i < res.length; i++) {

        var divMain = document.createElement('div');
        linkImageSlider = document.createElement('a');
        imgPlace = document.createElement('img');

        imgPlace.setAttribute('src', imgHost + res[i].imageUrl);

        divPlaceImages.className = "row";
        divMain.className = "col-25";

        linkImageSlider.className = "pb-popup-dark";
        linkImageSlider.setAttribute('data-place-details-image-index', i);

        linkImageSlider.setAttribute('id', res[i].id);
        linkImageSlider.appendChild(imgPlace);
        divMain.appendChild(linkImageSlider);
        divPlaceImages.appendChild(divMain)
    }

}
function DrawPlaceDetails(res) {
    if (typeof page != 'undefined') {

       

        var placeImages = [];
        var comments = document.getElementById('comments');
        comments.innerHTML = "";


        $('#txtAreaComment').val("");



        $('#divPlaceDetailRate').rateYo().rateYo('destroy');
        // var placeDetails = page.query.placeDetails;

        if (res.rating != null) {

            $('#divPlaceDetailRate').rateYo({
                rating: res.rating,
                starWidth: "20px",
                readOnly: true

            });
        }
        else if (res.rating == null) {
            $('#divPlaceDetailRate').rateYo({
                rating: 0,
                starWidth: "20px",
                readOnly: true


            });

        }
        $("#placeRate").rateYo({
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


                    var params = {

                        "RateValues": '1' + ':' + rating,
                        "RelatedId": localStorage.getItem('placeDetails'),
                        "RelatedType": 1,


                    };

                    CallService('placeDetails', "POST", "api/Rating/SaveRating", params, function (res) {
                        if (res != null) {


                            notificationService.alert('نجاح', 'شكرا لك علي التقييم .', function () { });
                        }
                    });
                }
            }
        });

        // $('#placeRate').rateYo("destroy");
        var rateyo = $('#placeRate').rateYo({
            fullStar: true
        });



        $('#txtPlaceName').text(res.cityName);
        $('#labelPackageType').text(res.packageName);
        if (res.images != null) {
            DrawPlaceImages(res.images);
            for (var i = 0; i < res.images.length; i++) {
                placeImages.push(imgHost + res.images[i].imageUrl);
             

            }

            myPhotoBrowserPopupDark = myApp.photoBrowser({
                photos: placeImages,
                theme: 'dark',
                type: 'popup',
                backLinkText: 'إغلاق',
                ofText: 'من'
            });

            $$('.pb-popup-dark').on('click', function () {
              

                var imageIndex = $(this).attr('data-place-details-image-index');
                myPhotoBrowserPopupDark.open(imageIndex);
            });


        } else {
            var divPlaceImages = document.getElementById('divPlaceImages');
            divPlaceImages.innerHTML = '';

        }
        $('#btnSendComment').unbind().on('click', function () {
            if (isUserLoggedIn()) {
                notificationService.alert("دليل جازان", "يجب عليك تسجيل الدخول حتى تتمكن من اضافة تعليق", function () { })
            } else {

                var txtAreaComment = $('#txtAreaComment').val();
                if (txtAreaComment != '') {
                    var params = {

                        "CommentText": txtAreaComment,
                        "RelatedId": localStorage.getItem('placeDetails'),
                        "commentTypeId": '1'

                    }

                    CallService('placeDetails', 'POST', 'api/Comments/SaveComment/', params, function (res) {
                        if (res != null) {
                            appendComment(res, "comments");
                            //$('#divCommentNotification').hide();
                            $('#txtAreaComment').val('');
                        }



                    });


                }
                else {

                    notificationService.alert('تنبيه', 'من فضلك قم بادخال تعليق', function () { });
                }

            }
        });


        $('#placeDetails').text(res.description);

        if (res.latitude && res.longitude) {

            initMapWithCoordinates('place-details-map', res.latitude, res.longitude, res.cityName, true);
        } else {
            $('#place-details-map').html('');
        }
        if (res.workingHours) {
            $("#place-details-working-hours").html('<i class="ionicons ion-clock"></i> ' + res.workingHours);
            $("#place-details-working-hours").show();
        } else {
            $("#place-details-working-hours").hide();
        }

        if (res.email) {
            $("#place-details-email").html('<i class="ionicons ion-android-mail"></i> ' + res.email);
            $("#place-details-email").unbind().click(function () {
                var ref = cordova.InAppBrowser.open("mailto:" + res.email, '_system', 'location=no');
            });
            $("#place-details-email").show();
        } else {
            $("#place-details-email").hide();
        }

        if (res.webSite) {
            $("#place-details-website").html('<i class="ionicons ion-earth"></i> ' + res.webSite);

            $("#place-details-website").show();

            $("#place-details-website").unbind().click(function () {
                var ref = cordova.InAppBrowser.open(res.webSite, '_system', 'location=no');
            });

        } else {
            $("#place-details-website").hide();
        }

        if (res.mobile) {
            $("#place-details-phone-number").show();
            $("#place-details-phone-number").html('<i class="ionicons ion-android-phone-portrait"></i> ' + res.mobile);
            $("#place-details-phone-number").unbind().click(function () {
                window.plugins.CallNumber.callNumber(onCallPlaceNumberSuccess, onCallPlaceNumberError, res.mobile, true);

            });

            function onCallPlaceNumberSuccess(result) {
                console.log("Success:" + result);
            }

            function onCallPlaceNumberError(result) {
                console.log("Error:" + result);
            }
        } else {
            $("#place-details-phone-number").hide();
        }

        if (res.faceBook) {
            $("#place-details-facebook").show();

            $("#place-details-facebook").unbind().click(function () {
                var ref = cordova.InAppBrowser.open(res.faceBook, '_system', 'location=no');
            });


        } else {
            $("#place-details-facebook").hide();
        }

        if (res.twitter) {
            $("#place-details-twitter").show();
            $("#place-details-twitter").unbind().click(function () {
                var ref = cordova.InAppBrowser.open(res.twitter, '_system', 'location=no');
            });

        } else {
            $("#place-details-twitter").hide();
        }

        if (res.instagram) {
            $("#place-details-instagram").show();
            $("#place-details-instagram").unbind().click(function () {
                var ref = cordova.InAppBrowser.open(res.instagram, '_system', 'location=no');
            });
        } else {
            $("#place-details-instagram").hide();
        }


    }


}
function GoToPlaceDetailsPage(page) {
    if (typeof page != 'undefined') {
        $('#comments').html('');
        myApp.detachInfiniteScroll($$('#commentsInfinite'));
        $$('.infinite-scroll-preloader').remove();
        localStorage.setItem('place-comment-page-number', 1);


        //loadSideMenuLinks();
        //$('#divCommentNotification').hide();
        var placeId = localStorage.getItem('placeDetails');
        CallService('placeDetails', 'Get', 'api/Advertisements/' + placeId, '', function (res) {
            if (res != null) {
                DrawPlaceDetails(res);
                CallService('placeDetails', 'Get', 'api/Comments/GetPagedComments/1/5/' + placeId + '/1', "", function (commentResult) {
                    getDeviceId();
                    viewsParamter = {
                        "RelatedTypeId": 1,
                        "RelatedId": placeId,
                        "DeviceId": localStorage.getItem('deviceId')
                    }
                    CallService('placeDetails', 'POST', 'api/Views/SaveView', viewsParamter, function (viewsRes) {
                        if (viewsRes != null) {

                        }
                        else {


                        }
                    });
                    if (commentResult != null) {
                        if (commentResult.length == 0) {
                            //$('#divCommentNotification').show();
                            myApp.detachInfiniteScroll($$('#commentsInfinite'));

                        }
                        else {
                            //$('#divCommentNotification').hide();
                            drawComments(commentResult, "comments");
                            maxItemComments = commentResult[0].overallCount;
                            localStorage.setItem('maxItemComments', maxItemComments);
                            lastPlace = $$('#comments div.commentLength').length;
                            myApp.attachInfiniteScroll($$('#commentsInfinite'));
                        }
                    }

                });

            }
            else {

                //notificationService.alert('خطا في استجاع الكومنتات', 'تنبيه', function () { });
            }

        });
        var loading = false;
        if (initCommentsInfinite == true) {
            initCommentsInfinite = false;
            $$('#commentsInfinite').on('infinite', function () {
                if (lastPlace > 0) {


                    if (loading) return;
                    loading = true;
                    var pageNumber = parseInt(localStorage.getItem('place-comment-page-number')) + 1;


                    CallService('placeDetails',
                           'Get',
                           'api/Comments/GetPagedComments/' + pageNumber + '/' + 5 + '/' + localStorage.getItem('placeDetails') + '/1',
                            "",
                            function (commentResult) {
                                if (commentResult) {
                                    loading = false;
                                    localStorage.setItem('place-comment-page-number', parseInt(localStorage.getItem('place-comment-page-number')) + 1);
                                    drawComments(commentResult, 'comments');
                                    if (commentResult && commentResult.length < 5) {

                                        myApp.detachInfiniteScroll($$('#commentsInfinite'));

                                        $$('.infinite-scroll-preloader').remove();
                                        return;
                                    }

                                }
                            });

                }
            });
        }
    }
}
function GoToNewsDetailsPage() { }
function GoToQuizPage(page) {
    if (typeof page != 'undefined') {
        //loadSideMenuLinks();
        DrawAdvertisementsSliderPage("advertisementSliderQuiz");
      
        DrawQuizPage();
    }

}
function GoToAddProjectDetails(page) {
    if (typeof page != 'undefined') {

        AdSubImageArray = [];
        resetAddProjectDetailsForm();
        params = {
            "LanguageId": 0,
            "PageNumber": 1,
            "PageSize": 1000
        };
        CallService('addProjectDetails', 'POST', 'api/Categories/GetCategories/0', params, function (res) {
            if (res != null) {
                DrawMainCategoryOption(res, "mainCategoryOption");
                if (res && res.length > 0) {
                    CallService('addProjectDetails', 'POST', 'api/Categories/GetCategories/' + res[0].categoryId, params, function (result) {
                        if (result != null && result.length > 0) {
                            DrawMainCategoryOption(result, "subMainCategoryOption");
                            localStorage.setItem('subMainCategoryId', result[0].categoryId);
                        }
                        else {

                            notificationService.alert('خطا', 'خطا في استرجاع التصنيف الفرعي', function () {
                            })

                        }
                    });
                }
            }
            else {

                notificationService.alert('خطا', 'خطا في استرجاع التصنيف الرئيسي', function () {
                });
            }
        });
        if (localStorage.getItem('uploadImageNoLimit') == 'true' || localStorage.getItem('uploadImageLimited') == 'true') {
            $("#btnAddAdvertisementSubPicture").show();
        } else {
            $("#btnAddAdvertisementSubPicture").hide();
        }


        if (localStorage.getItem('instgram') == 'true') {

            $('#instgramShow').show();
            localStorage.setItem('instgram', 'false');
        }
        else {
            $('#instgramShow').hide();
        }
        if (localStorage.getItem('map') == 'true') {
            //$('#mapShow').html('<input id="pac-input" class="controls" type="text" placeholder="اسم الموقع السياحى"><div id="add-project-details-map" style="width:100%;height:200px;display:block"></div>');
            //intializeSearchMapLocation('Jazan', '24.713552', '46.675296', 'Jazan', 'add-project-details-map');
            $('#mapShow').html('<div id="add-project-details-map" style="width:100%;height:200px;"></div>');
            initMapWithCoordinates('add-project-details-map', '16.889359', '42.570567', 'Jazan', false);

            localStorage.setItem('map', 'false');


        }
        else {
            $('#add-project-details-map').hide();
        }
        if (localStorage.getItem('facebook') == 'true') {
            $('#facebookShow').show();
            localStorage.setItem('facebook', 'false');
        }
        else {

            $('#facebookShow').hide();
        }
        if (localStorage.getItem('twitter') == 'true') {
            $('#twitterShow').show();
            localStorage.setItem('twitter', 'false');
        }
        else {

            $('#twitterShow').hide();
        }
        if (localStorage.getItem('mobile') == 'true') {
            $('#mobileShow').show();
            localStorage.setItem('mobile', 'false');
        }
        if (localStorage.getItem('email') == 'true') {

            $('#emailShow').show();
            localStorage.setItem('email', 'false');
        }
        else {
            $('#emailShow').hide();
        }
        if (localStorage.getItem('website') == 'true') {
            $('#websiteShow').show();
            localStorage.setItem('website', 'false');
        }
        else {
            $('#websiteShow').hide();
        }
        if (localStorage.getItem('snapChat') == 'true') {
            $('#snapShow').show();
            localStorage.setItem('snapChat', 'false');
        }
        else {
            $('#snapShow').hide();
        }
    }
}

myApp.onPageAfterAnimation('addProjectDetails', function (page) {
    $('#btnAddAdvertisementProfilePic').unbind().on('click', function () {

        navigator.camera.getPicture(function (profileImageURI) {
            localStorage.setItem('profileImageURI', profileImageURI);

            var imgUserAccount = document.getElementById('mainPicSrc');

            imgUserAccount.setAttribute('src', 'data:image/jpeg;base64, ' + profileImageURI);

        }
        ,
            function (message) {
                //notificationService.alert('تنبيه','خطا اثناء تحميل الصورة', function () { });
            },
        {
            quality: 100,
            destinationType: navigator.camera.DestinationType.DATA_URL,
            sourceType: navigator.camera.PictureSourceType.PHOTOLIBRARY,
            encodingType: Camera.EncodingType.JPEG,
            targetWidth: 300,
            targetHeight: 300,
            popoverOptions: CameraPopoverOptions,
            saveToPhotoAlbum: false,
            correctOrientation: true
        });



    });
    $('#btnAddAdvertisementSubPicture').unbind().on('click', function () {

        navigator.camera.getPicture(function (subImageURI) {

            if (localStorage.getItem('uploadImageNoLimit') == 'false' || localStorage.getItem('uploadImageNoLimit') == null) {
                if (AdSubImageArray && (AdSubImageArray.length < parseInt(localStorage.getItem('uploadImageLimited-count')))) {
                    DrawAdvertisementsSubMainImages(subImageURI, AdSubImageArray.length);
                    AdSubImageArray.push({ 'base64': subImageURI, 'imageExtension': 'jpg' });

                } else {
                    var maxImagesCount = localStorage.getItem('uploadImageLimited-count') == "null" ? '0' : localStorage.getItem('uploadImageLimited-count');
                    notificationService.alert('نجاح',
                                             'لقد تم إستنفاذ العدد المسموح للصور المرفقة ..بإمكانك حذف صورة مصاحبة قبل التمكن من رفع صورة اخرى',
                                             function () {

                                             });
                }
            } else {
                DrawAdvertisementsSubMainImages(subImageURI, AdSubImageArray.length);
                AdSubImageArray.push({ 'base64': subImageURI, 'imageExtension': 'jpg' });
            }

        }
      ,
          function (message) {
              //notificationService.alert('خطا اثناء تحميل الصورة', 'تنبيه', function () { });
          },
      {
          quality: 100,
          destinationType: navigator.camera.DestinationType.DATA_URL,
          sourceType: navigator.camera.PictureSourceType.PHOTOLIBRARY,
          encodingType: Camera.EncodingType.JPEG,
          targetWidth: 300,
          targetHeight: 300,
          popoverOptions: CameraPopoverOptions,
          saveToPhotoAlbum: false,
          correctOrientation: true
      });


    });
    $('#btnAddAdvertisement').unbind().on('click', function () {
        if ($('#add-project-details').parsley().validate()) {

            $('#btnAddAdvertisement').attr('disabled', true);

            var latitude = localStorage.getItem('tourist-details-address-latitude');
            var longtitude = localStorage.getItem('tourist-details-address-address-longtitude');

            if ((!latitude || !longtitude) && $("#add-project-details-map").is(":visible")) {
                notificationService.alert('خطأ', 'يجب اختيار احداثيات الموقع');
                $('#btnAddAdvertisement').attr('disabled', false);
            } else {

                if (!localStorage.getItem('profileImageURI')) {
                    notificationService.alert('خطأ', 'يجب اختيار  الصورة الرئيسية');
                    $('#btnAddAdvertisement').attr('disabled', false);
                } else {
                    var txtAdName = $('#txtAdName').val();
                    var txtCiName = $('#txtCiName').val();
                    var textAreaAdvertisementDetails = $('#textAreaAdvertisementDetails').val();
                    var txtTimingHours = $('#txtTimingHours').val();
                    var txtMobile = $('#txtMobile').val();
                    var txtEmail = $('#txtEmail').val();
                    var txtWebsite = $('#txtWebsite').val();
                    var txtTwitter = $('#txtTwitter').val();
                    var txtFacebook = $('#txtFacebook').val();
                    var txtInstgram = $('#txtInstgram').val();
                    var txtSnapChat = $('#txtSnapChat').val();

                    var params = {
                        "name": txtAdName,
                        "cityName": txtCiName,
                        "categoryId": localStorage.getItem('subMainCategoryId'),
                        "packageId": localStorage.getItem('packageId'),
                        "description": textAreaAdvertisementDetails,
                        "latitude": latitude,
                        "longitude": longtitude,
                        "workingHours": txtTimingHours,
                        "mobile": txtMobile,
                        "email": txtEmail,
                        "webSite": txtWebsite,
                        "twitter": txtTwitter,
                        "faceBook": txtFacebook,
                        "instagram": txtInstgram,
                        "snapchat": txtSnapChat,
                        "mainImageBase64": localStorage.getItem('profileImageURI'),
                        "mainImageExtension": "jpg",
                        "advertisementImages": AdSubImageArray

                    };

                    CallService('addProjectDetails', 'POST', 'api/Advertisements/Add', params, function (res) {
                        $('#btnAddAdvertisement').attr('disabled', false);
                        if (res != null) {
                            if (res.packageId != 4) {
                                notificationService.alert('نجاخ', 'في انتظار صاحب التطبيق علي الموافقه علي الاعلان',
                                                                             function () {
                                                                                 resetAddProjectDetailsForm();
                                                                                 myApp.popup('.popup-thanks');
                                                                                 localStorage.setItem('userProjectId', res.id);
                                                                             });
                            } else {
                                resetAddProjectDetailsForm();
                                localStorage.setItem('userProjectId', res.id);
                                mainView.router.loadPage({ pageName: 'advertismentsProfileDetails', query: { elementProjectId: res.id } });
                            }


                        }
                        else {
                            notificationService.alert('خطأ', 'خطا اثناء اضافة اعلان', function () { });
                        }


                    });
                }

            }


        }


    });
    $('#mainCategoryOption').unbind().on('change', function () {
        var mainCategoryId = $(this).val();
        $('#subMainCategoryOption').text('');
        myApp.closeModal();
        CallService('addProjectDetails', 'POST', 'api/Categories/GetCategories/' + mainCategoryId, params, function (result) {
            if (result != null) {

                DrawMainCategoryOption(result, "subMainCategoryOption");
            }
            else {

                notificationService.alert('خطا', 'خطا في استرجاع التصنيف الفرعي', function () {
                })

            }
        });

    });
    $('#subMainCategoryOption').unbind().on('change', function () {
        var subMainCategoryId = $(this).val();
        localStorage.setItem('subMainCategoryId', subMainCategoryId);
        myApp.closeModal();

    });
}).trigger();
$$('.popup-thanks').on('opened', function () {
    mainView.router.loadPage({ pageName: 'advertismentsProfileDetails', query: { elementProjectId: localStorage.getItem('userProjectId') } });

});

myApp.onPageAfterAnimation('dalel', function (page) {
    if (page) {
        GoToDalelPage(page);
        dataContextService.advertisement.getMainBannerAdvertisement("1", function (mainAdvertisements) {
            if (mainAdvertisements) {
                drawMainSliderAdvertisements(mainAdvertisements, 'dalel-swiper-container');
                
            }
        });
    }
}).trigger();
myApp.onPageAfterAnimation('news', function (page) {
    if (page) {
        newsController.goToNewsPage(page);
        dataContextService.advertisement.getMainBannerAdvertisement("1", function (mainAdvertisements) {
            if (mainAdvertisements) {
                drawMainSliderAdvertisements(mainAdvertisements, 'news-swiper-container');
               
            }
        });
    }

}).trigger();
myApp.onPageAfterAnimation('advertismentsProfile', function (page) {
    if (page) {
        personalPageController.goToPersonalPage(page);

        dataContextService.advertisement.getMainBannerAdvertisement("1", function (mainAdvertisements) {
            if (mainAdvertisements) {
                drawMainSliderAdvertisements(mainAdvertisements, 'advertismentsProfile-swiper-container');
              
            }
        });
    }
}).trigger();
myApp.onPageAfterAnimation('advertismentsProfileDetails', function (page) {
    if (page) {
        personalPageProjectsDetailsController.goToPersonalPageProjectsDetails(page);

        dataContextService.advertisement.getMainBannerAdvertisement("1", function (mainAdvertisements) {
            if (mainAdvertisements) {
                drawMainSliderAdvertisements(mainAdvertisements, 'advertismentsProfileDetails-swiper-container');
               
            }
        });
    }

}).trigger();
myApp.onPageAfterAnimation('touristVisitProfileDetails', function (page) {
    if (page) {
        personalPageTouristDetailController.goToPersonalPageTouristDetails(page);

        dataContextService.advertisement.getMainBannerAdvertisement("1", function (mainAdvertisements) {
            if (mainAdvertisements) {
                drawMainSliderAdvertisements(mainAdvertisements, 'touristVisitProfileDetails-swiper-container');
                if (init_touristVisitProfileDetails) {
                    init_touristVisitProfileDetails = false;
                    initSwiper('touristVisitProfileDetails-swiper-container');
                }
            }
        });
    }

}).trigger();
myApp.onPageAfterAnimation('userProfile', function (page) {

    if (page) {
        GoToUserProfilePage(page);
        dataContextService.advertisement.getMainBannerAdvertisement("1", function (mainAdvertisements) {
            if (mainAdvertisements) {
                drawMainSliderAdvertisements(mainAdvertisements, 'userProfile-swiper-container');
                
            }
        });
    }

}).trigger();
myApp.onPageAfterAnimation('advertismentsProfileProjects', function (page) {

    if (page) {
        personalPageProjectsController.goToPersonalPageProject(page);

        dataContextService.advertisement.getMainBannerAdvertisement("1", function (mainAdvertisements) {
            if (mainAdvertisements) {
                drawMainSliderAdvertisements(mainAdvertisements, 'advertismentsProfileProjects-swiper-container');
                
            }
        });
    }

}).trigger();
myApp.onPageAfterAnimation('advertismentsProfileVisits', function (page) {
    if (page) {
        personalPageTouristVisitController.goToPersonalPageTouristVisit(page);

        dataContextService.advertisement.getMainBannerAdvertisement("1", function (mainAdvertisements) {
            if (mainAdvertisements) {
                drawMainSliderAdvertisements(mainAdvertisements, 'advertismentsProfileVisits-swiper-container');
                
            }
        });
    }

}).trigger();
myApp.onPageAfterAnimation('addAdvertisment', function (page) {
    if (page) {
        loadSideMenuLinks();
        GoToAddAdvertismentPage(page);
        dataContextService.advertisement.getMainBannerAdvertisement("1", function (mainAdvertisements) {
            if (mainAdvertisements) {
                drawMainSliderAdvertisements(mainAdvertisements, 'addAdvertisment-swiper-container');
                
            }
        });
    }

}).trigger();
myApp.onPageAfterAnimation('categoryDetails', function (page) {

    if (page) {
        GoToDalelSubCategoriesPage(page);
        dataContextService.advertisement.getMainBannerAdvertisement("1", function (mainAdvertisements) {
            if (mainAdvertisements) {
                drawMainSliderAdvertisements(mainAdvertisements, 'categoryDetails-swiper-container');
              
            }
        });
    }

}).trigger();
myApp.onPageAfterAnimation('place', function (page) {
    if (page) {
        GoToPlacePage(page);

        dataContextService.advertisement.getMainBannerAdvertisement("1", function (mainAdvertisements) {
            if (mainAdvertisements) {
                drawMainSliderAdvertisements(mainAdvertisements, 'place-swiper-container');
                
            }
        });
    }

}).trigger();
myApp.onPageAfterAnimation('placeDetails', function (page) {
    if (page) {
        GoToPlaceDetailsPage(page);
        dataContextService.advertisement.getMainBannerAdvertisement("1", function (mainAdvertisements) {
            if (mainAdvertisements) {
                drawMainSliderAdvertisements(mainAdvertisements, 'placeDetails-swiper-container');
                
            }
        });
    }
}).trigger();
myApp.onPageAfterAnimation('newsDetails', function (page) {
    if (page) {
        newsController.goToNewsDetailsPage(page);
        dataContextService.advertisement.getMainBannerAdvertisement("1", function (mainAdvertisements) {
            if (mainAdvertisements) {
                drawMainSliderAdvertisements(mainAdvertisements, 'newsDetails-swiper-container');
               
            }
        });
    }
}).trigger();
myApp.onPageAfterAnimation('quiz', function (page) {
    quizPageController.goToCultureCompetitionPage(page);
}).trigger();
myApp.onPageAfterAnimation('addProjectDetails', function (page) {
    GoToAddProjectDetails(page);
}).trigger();

// side main work 1 js files 
myApp.onPageAfterAnimation('tourist', function (page) {
    if (page) {
        GoToTouristPage(page);
        dataContextService.advertisement.getMainBannerAdvertisement("1", function (mainAdvertisements) {
            if (mainAdvertisements) {
                drawMainSliderAdvertisements(mainAdvertisements, 'tourist-swiper-container');
               
            }
        });
    }

}).trigger();
myApp.onPageAfterAnimation('about', function (page) {
    if (page) {
        mobileSettingController.goToAboutUsPage(page);
        dataContextService.advertisement.getMainBannerAdvertisement("1", function (mainAdvertisements) {
            if (mainAdvertisements) {
                drawMainSliderAdvertisements(mainAdvertisements, 'about-swiper-container');
               
            }
        });
    }
}).trigger();
myApp.onPageAfterAnimation('touristDetails', function (page) {
    if (page) {
        GoToTouristDetailsPage(page);
        dataContextService.advertisement.getMainBannerAdvertisement("1", function (mainAdvertisements) {
            localStorage.setItem('pageNumberForMainBanner', 1);
            if (mainAdvertisements) {
                drawMainSliderAdvertisements(mainAdvertisements, 'touristDetails-swiper-container');
               
            }
        });
    }

}).trigger();
myApp.onPageAfterAnimation('notification', function (page) {
    if (page) {
        notificationController.goToNotificationPage(page);
        dataContextService.advertisement.getMainBannerAdvertisement("1", function (mainAdvertisements) {
            if (mainAdvertisements) {
                drawMainSliderAdvertisements(mainAdvertisements, 'notification-swiper-container');
                
            }
        });

    }

}).trigger();


// side main work 2 js files    

myApp.onPageBeforeAnimation('intro', function (page) {
    if (page) {
        GoToIntroductionPage(page);

        dataContextService.news.getPaged(1, 5, function (result) {
            drawNewsSlider(result);
        
        });

        dataContextService.advertisement.getMainBannerAdvertisement("1", function (mainAdvertisements) {
            if (mainAdvertisements) {
                drawMainSliderAdvertisements(mainAdvertisements, 'intro-swiper-container');
               
            }
        });
    }
}).trigger();
myApp.onPageAfterAnimation('login', function (page) {
    if (page) {
        loadSideMenuLinks();
        $('#loginEmail').val('');
        $('#Loginpassword').val('');
    }
}).trigger();
myApp.onPageAfterAnimation('signupVisitor', function (page) {
    if (page) {
        authenticationService.goToSignupVisitorPage(page);
    }
}).trigger();
myApp.onPageAfterAnimation('activationCode', function (page) {
    if (page) {
        authenticationService.goToActivationCodePage(page);
    }
}).trigger();
myApp.onPageAfterAnimation('forgetPassword', function (page) {
    if (page) {
        authenticationService.goToForgetPasswordPage(page);
    }
}).trigger();
myApp.onPageAfterAnimation('resetPassword', function (page) {
    if (page) {
        authenticationService.goToResetPasswordPage(page);
    }
}).trigger();
myApp.onPageAfterAnimation('changePassword', function (page) {
    if (page) {
        authenticationService.goToChangePasswordPage(page);
    }
}).trigger();
myApp.onPageAfterAnimation('advertisments', function (page) {
    if (page) {
        advertismentController.GoToadvertismentsPage(page);

        dataContextService.advertisement.getMainBannerAdvertisement("1", function (mainAdvertisements) {
            if (mainAdvertisements) {
                drawMainSliderAdvertisements(mainAdvertisements, 'advertisments-swiper-container');
               
            }
        });
    }
}).trigger();
myApp.onPageAfterAnimation('advertismentsDetails', function (page) {
    if (page) {
        advertismentController.GoToadvertismentsDetailsPage(page);
        dataContextService.advertisement.getMainBannerAdvertisement("1", function (mainAdvertisements) {
            if (mainAdvertisements) {

                drawMainSliderAdvertisements(mainAdvertisements, 'advertismentsDetails-swiper-container');
               
            }
        });
    }
}).trigger();

myApp.init();

document.addEventListener("offline", onOffline, false);

function onOffline() {
    // Handle the offline event
    navigator.notification.alert('انت غير متصل بالانترنت , من فضلك أعد تشغيل البرنامج بعد التأكد من الإنترنت .', function () {
        ExitApplication();
    }, 'خطأ', 'موافق');
}


//window.document.addEventListener('backbutton', function (event) {
//    myApp.closeModal();
//    if ($('.modal-in').length > 0) {
//        return false
//    }
//}, false);

//myApp.onPageAfterBack('', function (page) { });