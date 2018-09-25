var userPersonlAdvertisementImagesArray = [];

var personalPageProjectsDetailsController = (function () {
    var advertisementImages = [];
    initDelete = true;
    var deletedImagesArray = [];
    var projectImagesArrayWithId = [];
    var projectImagesArrayAfterDeletedImages = [];

    $('#edit-personal-project-main-image-button').unbind().click(function () {
        navigator.camera.getPicture(onSuccess, onFail, {
            quality: 50,
            destinationType: Camera.DestinationType.DATA_URL,
            sourceType: Camera.PictureSourceType.SAVEDPHOTOALBUM,
        });
        function onSuccess(imageURI) {
            var image = document.getElementById('edit-personal-project-main-image');
            image.src = 'data:image/png;base64,' + imageURI;
        }
        function onFail(message) {

        }
    });

    var drawAdvertisementsImages = function drawAdvertisementsImages(imageURI, index) {

        var subMainPicShow = document.getElementById('edit-personal-advertisement');
        var divDrawed = document.createElement('div');
        divDrawed.setAttribute('id', 'personal-project-details-edit-image-' + index);
        var imageAdvertisementSubMain = document.createElement('img');
        divDrawed.className = "col-25";
        imageAdvertisementSubMain.setAttribute('src', 'data:image/jpeg;base64, ' + imageURI);
        imageAdvertisementSubMain.className = 'image-responsive';

        var deleteImageButton = document.createElement('a');
        deleteImageButton.className = 'button button-round red';
        deleteImageButton.setAttribute('onclick', ' notificationService.confirm("هل انت متأكد من حذف الصورة",function(buttonIndex){  if(buttonIndex==1){ userPersonlAdvertisementImagesArray.splice(' + index + ',1); $("#' + 'personal-project-details-edit-image-' + index + '").remove(); }})');

        deleteImageButton.innerHTML = 'حذف';


        divDrawed.appendChild(imageAdvertisementSubMain);
        divDrawed.appendChild(deleteImageButton);
        subMainPicShow.insertBefore(divDrawed, subMainPicShow.firstChild);
    }
    var drawPersonalPageProjectsDetailsImages = function drawPersonalPageProjectsDetailsImages(advertisementDetailsImages, id) {
        if (id) {
            var projectImages = document.getElementById(id);
            projectImages.innerHTML = "";
            for (var i = 0; i < advertisementDetailsImages.length; i++) {
                var divDrawed = document.createElement('div');
                var linkImageBrowsing = document.createElement('a');
                var imgProject = document.createElement('img');
                divDrawed.className = "col-40";
                divDrawed.setAttribute('id', advertisementDetailsImages[i].id);
                imgProject.setAttribute('src', imgHost + advertisementDetailsImages[i].imageUrl);
                linkImageBrowsing.setAttribute('id', 'linkImageBrowing_' + advertisementDetailsImages[i].id);
                linkImageBrowsing.className = "pb-popup-dark2";
                linkImageBrowsing.appendChild(imgProject);
                divDrawed.appendChild(linkImageBrowsing);
                projectImages.appendChild(divDrawed);
                $('#linkImageBrowing_' + advertisementDetailsImages[i].id).on('click', function () {
                    //var myPhotoBrowserPopupDark = myApp.photoBrowser({
                    //    photos: advertisementImages,
                    //    theme: 'dark',
                    //    type: 'popup',
                    //    backLinkText: 'إغلاق',
                    //    ofText: 'من'
                    //});
                    //myPhotoBrowserPopupDark.open();
                });
                if (deletedImagesArray.length > 0) {
                    for (var d = 0; d < deletedImagesArray.length; d++) {
                        if (deletedImagesArray[d].id == advertisementDetailsImages[i].id) {
                            projectImagesArrayAfterDeletedImages.push(advertisementDetailsImages[i].id);
                            $('#' + advertisementDetailsImages[i].id).remove();
                            for (var t = 0; t < projectImagesArrayAfterDeletedImages.length; t++) {
                                for (var l = 0; l < projectImagesArrayWithId.length; l++) {
                                    if (projectImagesArrayAfterDeletedImages[t] == projectImagesArrayWithId[l].id) {
                                        projectImagesArrayWithId.pop(projectImagesArrayWithId[l].id);
                                    }
                                }
                            }
                            advertisementImages = [];
                            for (var h = 0; h < projectImagesArrayWithId.length; h++) {
                                advertisementImages.push(projectImagesArrayWithId[h].image);
                            }

                        }
                    }
                }
            }
        }
    }
    var drawProjectDetailImageForDeleted = function DrawProjectDetailImageForDeleted(res, id) {
        if (id) {
            if (res.length == 0) {
                $('#btnDeleteUserProjectPicture').text('حذف الصور');
                $('#btnDeleteUserProjectPicture').removeClass('green');
                $('#btnDeleteUserProjectPicture').addClass('red');
            }
            var projectImages = document.getElementById(id);
            projectImages.innerHTML = "";
            for (var i = 0; i < res.length; i++) {
                var divDrawed = document.createElement('div');
                var linkImageBrowsing = document.createElement('a');
                var imgProject = document.createElement('img');
                var iDeleteIcon = document.createElement('i');
                var divImageContent = document.createElement('div');
                var divOverlay = document.createElement('div');
                divDrawed.setAttribute('id', res[i].id);
                divDrawed.className = "col-40";
                divOverlay.className = "delOverlay";
                iDeleteIcon.className = "ionicons ion-android-delete";
                imgProject.setAttribute('src', imgHost + res[i].imageUrl);
                divImageContent.className = "imgContent";
                linkImageBrowsing.className = "pb-popup-dark2";
                linkImageBrowsing.setAttribute('id', 'linkDeletedImage_' + res[i].id);
                divOverlay.appendChild(iDeleteIcon);
                divImageContent.appendChild(divOverlay);
                divImageContent.appendChild(imgProject);
                linkImageBrowsing.appendChild(divImageContent);
                divDrawed.appendChild(linkImageBrowsing);
                projectImages.appendChild(divDrawed);
                $('#linkDeletedImage_' + res[i].id).on('click', function () {
                    var imageDeletedId = $(this).attr('id').split('_')[1];
                    localStorage.setItem('personal-project-details-image-deleted-id', imageDeletedId);
                    notificationService.confirm('هل انت متأكد من حذف الصورة ؟', function (buttonIndex) {
                        if (buttonIndex == 1) {
                            dataContextService.advertisement.deleteUserProjectsImage(localStorage.getItem('personal-project-details-image-deleted-id'), function (imageDeleted) {
                                if (imageDeleted != null) {
                                    notificationService.alert('تنبيه', 'تم حذف الصورة بنجاح', function () {

                                        for (var i = 0; i < res.length; i++) {
                                            if (localStorage.getItem('personal-project-details-image-deleted-id') == res[i].id) {
                                                deletedImagesArray.push({ 'id': imageDeletedId });
                                                $('#' + res[i].id).remove();
                                            }
                                        }


                                        if ($('#projectImages div.col-40') && $('#projectImages div.col-40').length == 0) {
                                            $("#btnDeleteUserProjectPicture").hide();
                                        }

                                        localStorage.removeItem('personal-project-details-image-deleted-id');
                                    });

                                }
                                else {
                                    notificationService.alert('تنبيه', 'لم يتم حذف الصورة', function () {
                                        localStorage.removeItem('personal-project-details-image-deleted-id');
                                    });
                                }
                            });
                        }
                    });
                });
                if (deletedImagesArray.length > 0) {
                    for (var d = 0; d < deletedImagesArray.length; d++) {
                        if (deletedImagesArray[d].id == res[i].id) {
                            $('#' + res[i].id).remove();

                        }
                    }
                }

            }
        }
    }
    var goToPersonalPageProjectsDetails = function (page) {
        if (typeof page != 'undefined') {

            advertisementImages = [];
            deletedImagesArray = [];
            projectImagesArrayAfterDeletedImages = [];
            projectImagesArrayWithId = [];
            var projectImages = document.getElementById('projectImages');
            projectImages.innerHTML = "";
            $('#btnDeleteUserProjectPicture').text('حذف الصور');
            $('#btnDeleteUserProjectPicture').removeClass('green');
            $('#btnDeleteUserProjectPicture').addClass('red');
            deletedImagesArray = [];
            //loadSideMenuLinks();
            $('#projectDetails #txtProjectName').text("");
            $('#projectDetails #labelView').text("");
            $('#projectDetails #txtCityName').text("");
            $('#projectDetails #txtAdvertisementDuration').text("");
            $('#projectDetails #labelBackageType').text("");
            $('#labelCategoryName').text();
            var userProjectId = localStorage.getItem('userProjectId');
            var userLoged = JSON.parse(localStorage.getItem('userLoggedIn'));

            dataContextService.advertisement.getAdvertisementDetails(localStorage.getItem('userProjectId'), userLoged.userId, function (advertisementDetails) {
                if (advertisementDetails != null) {
                    localStorage.setItem('personal-user-project-features', advertisementDetails.features);


                    // if the project doesn't contains edit feature we will disable edit ads 
                    if (advertisementDetails.features.indexOf('4') == -1) {
                        $('#btnEditPersonalAdvertisementDetail').attr('disabled', true);
                        $('#btnDeleteUserProjectPicture').attr('disabled', true);
                    } else {
                        $('#btnEditPersonalAdvertisementDetail').attr('disabled', false);
                        $('#btnDeleteUserProjectPicture').attr('disabled', false);
                    }

                    if (advertisementDetails.features.indexOf('8') == -1) {
                        $('#user-project-details-snap').val('');
                        $('#user-project-details-snap-show').hide();
                    } else {
                        $('#user-project-details-snap').val(advertisementDetails.snapchat);
                        $('#user-project-details-snap-show').show();
                    }

                    if (advertisementDetails.features.indexOf('9') == -1) {
                        $('#user-project-details-instgram').val('');
                        $('#user-project-details-instgram-show').hide();
                    } else {
                        $('#user-project-details-instgram').val(advertisementDetails.instagram);
                        $('#user-project-details-instgram-show').show();
                    }

                    if (advertisementDetails.features.indexOf('10') == -1) {
                        $('#user-project-details-facebook').val('');
                        $('#user-project-details-facebook-show').hide();
                    } else {
                        $('#user-project-details-facebook').val(advertisementDetails.faceBook);
                        $('#user-project-details-facebook-show').show();
                    }

                    if (advertisementDetails.features.indexOf('11') == -1) {
                        $('#user-project-details-twitter').val('');
                        $('#user-project-details-twitter-show').hide();
                    } else {
                        $('#user-project-details-twitter').val(advertisementDetails.twitter);
                        $('#user-project-details-twitter-show').show();
                    }

                    if (advertisementDetails.features.indexOf('13') == -1) {
                        $('#user-project-details-email').val('');
                        $('#user-project-details-email-show').hide();
                    } else {
                        $('#user-project-details-email').val(advertisementDetails.email);
                        $('#user-project-details-email-show').show();
                    }

                    if (advertisementDetails.features.indexOf('12') == -1) {
                        $('#user-project-details-website').val('');
                        $('#user-project-details-website-show').hide();
                    } else {
                        $('#user-project-details-website').val(advertisementDetails.webSite);
                        $('#user-project-details-website-show').show();
                    }

                    if (advertisementDetails.features.indexOf('14') == -1) {
                        $('#user-project-details-mobile').val('');
                        $('#user-project-details-mobile-show').hide();
                    } else {
                        $('#user-project-details-mobile').val(advertisementDetails.mobile);
                        $('#user-project-details-mobile-show').show();
                    }


                    //dataContextService.advertisement.selectMaximumImagesCountByAdvertisementId(advertisementDetails.advertisementId, function (result) {
                    //    if (result) {
                    //        localStorage.setItem('edit-tourist-details-visit-maximum-images-count', result);
                    //    } else {
                    //        localStorage.setItem('edit-tourist-details-visit-maximum-images-count', 0);
                    //    }

                    //});


                    $('#projectDetails #txtProjectName').text(advertisementDetails.advertisementName);
                    if (advertisementDetails.isApproved == true) {
                        $('#projectDetails #labelProjectApproved').show();
                        $('#projectDetails #labelProjectRefuse').hide();
                    }
                    else {
                        $('#projectDetails #labelProjectRefuse').show();
                        $('#projectDetails  #labelProjectApproved').hide();
                    }
                    if (advertisementDetails.mobile == "" || advertisementDetails.mobile == null) {
                        $('#pUserPhone').hide();
                    }
                    else {
                        $('#pUserPhone').show();
                    }
                    if (advertisementDetails.email == "" || advertisementDetails.email == null) {
                        $('#pUserEmailContact').hide();
                    }
                    else {
                        $('#pUserEmailContact').show();
                    }
                    if (advertisementDetails.webSite == "" || advertisementDetails.webSite == null) {
                        $('#pUserWebsite').hide();
                    }
                    else {
                        $('#pUserWebsite').show();
                    }
                    if (advertisementDetails.faceBook == "" || advertisementDetails.faceBook == null) {
                        $('#pUSerFaceBook').hide();
                    }
                    else {
                        $('#pUSerFaceBook').show();
                    }
                    if (advertisementDetails.instagram == "" || advertisementDetails.instagram == null) {
                        $('#pUserInstgram').hide();
                    }
                    else {
                        $('#pUserInstgram').show();
                    }

                    if (advertisementDetails.snapchat == "" || advertisementDetails.snapchat == null) {
                        $('#pUserSnapChat').hide();
                    }
                    else {
                        $('#pUserSnapChat').show();
                    }
                    if (advertisementDetails.twitter == "" || advertisementDetails.twitter == null) {
                        $('#pUserTwitter').hide();
                    }
                    else {
                        $('#pUserTwitter').show();
                    }
                    $('#projectDetails #labelView').text();
                    $('#projectDetails #txtCityName').html('<i class="ionicons ion-home"><i> ' + advertisementDetails.cityName);
                    $('#projectDetails #txtAdvertisementDuration').text();
                    $('#projectDetails #labelBackageType').text(advertisementDetails.packageName);
                    $('#labelCategoryName').html('<i class="ionicons ion-pricetags"><i> ' + advertisementDetails.categoryName);
                    $('#projectDetails #padvertisementDetails').text(advertisementDetails.description);
                    $('#projectDetails #userPhoneContact').text(advertisementDetails.mobile);
                    $('#projectDetails #userMailContact').text(advertisementDetails.email);
                    $('#projectDetails #userWebsiteContact').text(advertisementDetails.webSite);
                    $('#projectDetails #userTwitterContact').text(advertisementDetails.twitter);
                    $('#projectDetails #userFacebookContact').text(advertisementDetails.faceBook);
                    $('#projectDetails #userInstgramContact').text(advertisementDetails.instagram);
                    $('#projectDetails #userSnapChatContact').text(advertisementDetails.snapchat);
                    if (advertisementDetails.workingHours == "" || advertisementDetails.workingHours == null) {
                        $("#personal-user-project-details-working-time-wrapper").hide();
                    } else {
                        $('#personal-user-project-details-working-time').text(advertisementDetails.workingHours)
                        $("#personal-user-project-details-working-time-wrapper").show();
                    }

                    if (advertisementDetails.startDate) {
                        var month = new Date(advertisementDetails.startDate).getUTCMonth() + 1;
                        var day = new Date(advertisementDetails.startDate).getUTCDate();
                        var year = new Date(advertisementDetails.startDate).getUTCFullYear();

                        $('#personal-user-project-details-project-start-date').text("تاريخ بداية المشروع " + day + '/' + month + '/' + year);
                        $('#personal-user-project-details-project-start-date-wrapper').show();
                    } else {
                        $('#personal-user-project-details-project-start-date-wrapper').hide();
                    }

                    if (advertisementDetails.endDate) {
                        var month = new Date(advertisementDetails.endDate).getUTCMonth() + 1;
                        var day = new Date(advertisementDetails.endDate).getUTCDate();
                        var year = new Date(advertisementDetails.endDate).getUTCFullYear();

                        $('#personal-user-project-details-project-end-date').text("تاريخ نهاية المشروع " + day + '/' + month + '/' + year);
                        $('#personal-user-project-details-project-end-date-wrapper').show();
                    } else {
                        $('#personal-user-project-details-project-end-date-wrapper').hide();
                    }

                    if (advertisementDetails.images != null) {
                        drawPersonalPageProjectsDetailsImages(advertisementDetails.images, 'projectImages');
                        for (var i = 0; i < advertisementDetails.images.length; i++) {
                            projectImagesArrayWithId.push({ 'id': advertisementDetails.images[i].id, 'image': imgHost + advertisementDetails.images[i].imageUrl });
                            advertisementImages.push(projectImagesArrayWithId[i].image);
                            $('#btnDeleteUserProjectPicture').show();
                        }
                    }
                    else {
                        $('#btnDeleteUserProjectPicture').hide();
                    }

                    if (advertisementDetails.latitude && advertisementDetails.longitude) {
                        initMapWithCoordinates('personal-advertisement-map', advertisementDetails.latitude, advertisementDetails.longitude, advertisementDetails.cityName, true);
                    } else {
                        $('#personal-advertisement-map-wrapper').html('');

                    }

                    $('#btnDeleteUserProjectPicture').unbind().on('click', function () {

                        if (initDelete) {
                            initDelete = false;
                            $('#btnDeleteUserProjectPicture').text('تراجع');
                            $('#btnDeleteUserProjectPicture').removeClass('red');
                            $('#btnDeleteUserProjectPicture').addClass('green');
                            drawProjectDetailImageForDeleted(advertisementDetails.images, "projectImages");
                        }
                        else {
                            initDelete = true;
                            $('#btnDeleteUserProjectPicture').text('حذف الصور');
                            $('#btnDeleteUserProjectPicture').removeClass('green');
                            $('#btnDeleteUserProjectPicture').addClass('red');
                            drawPersonalPageProjectsDetailsImages(advertisementDetails.images, "projectImages");
                        }



                    });
                    $('#btnEditPersonalAdvertisementDetail').unbind().on('click', function () {
                        $('#advertismentDetailsEdit').parsley().reset();

                        dataContextService.advertisement.selectMaximumImagesCountByAdvertisementId(advertisementDetails.advertisementId, function (result) {
                            if (result) {
                                localStorage.setItem('edit-tourist-details-visit-maximum-images-count', result);
                            } else {
                                localStorage.setItem('edit-tourist-details-visit-maximum-images-count', 0);
                            }

                        });


                        userPersonlAdvertisementImagesArray = [];
                        $('#edit-personal-advertisement').html('');
                        $('#personalUserAdId').val(advertisementDetails.advertisementId);
                        $('#personalUserCategoryId').val(advertisementDetails.categoryId);
                        $('#personalUserPackageId').val(advertisementDetails.packageId);
                        $('#txtEditPersonalAdName').val(advertisementDetails.advertisementName);
                        $('#personalUserLocationLatitude').val(advertisementDetails.latitude);
                        $('#personalUserLocationLongtitude').val(advertisementDetails.longitude);
                        $('#txtEditPersonalAdCity').val(advertisementDetails.cityName);
                        $('#txtEditPersonalAdDescription').val(advertisementDetails.description);
                        $('#txtEditPersonalAdWorkingHours').val(advertisementDetails.workingHours);

                        $('#edit-personal-project-main-image').attr('src', imgHost + advertisementDetails.advertisementImageUrl)

                        editUserPersonalAd();
                        $$('.popup-advertismentDetailsEdit').on('opened', function () {
                            if (advertisementDetails.latitude && advertisementDetails.longitude) {
                                //$('#edit-advertisement-details-map-wrapper').html('<input id="pac-input" class="controls" type="text" placeholder="اسم الموقع السياحى"><div id="edit-advertisement-details-map" style="width:100%;height:200px;"></div>');
                                //intializeSearchMapLocation(advertisementDetails.cityName, advertisementDetails.latitude, advertisementDetails.longitude, advertisementDetails.cityName, 'edit-advertisement-details-map');

                                $('#edit-advertisement-details-map-wrapper').html('<div id="edit-advertisement-details-map" style="width:100%;height:200px;"></div>');
                                initMapWithCoordinates('edit-advertisement-details-map', '16.889359', '42.570567', 'Jazan', false);

                            }
                            else {
                                $('#edit-advertisement-details-map-wrapper').html('');
                            }

                        });
                        myApp.popup('.popup-advertismentDetailsEdit');
                    });

                }
                else {
                    //notificationService.alert('خطا في استرجاع تفاصيل هذا المشروع', 'تنبيه', function () { });
                }
            });
        }
    };
    var editUserPersonalAd = function editUserPersonalAd() {
        $('#addUserPersonalAdvertisementPicture').unbind().on('click', function () {



            navigator.camera.getPicture(function (subImageURI) {
                //userPersonlAdvertisementImagesArray.push({ 'base64': advertisementImageURI, 'imageExtension': 'jpg' });
                //drawAdvertisementsImages(advertisementImageURI);


                if (localStorage.getItem('personal-user-project-features').split(',').contains(' 5') == false) {
                    if (userPersonlAdvertisementImagesArray && (userPersonlAdvertisementImagesArray.length
                        < parseInt(localStorage.getItem('edit-tourist-details-visit-maximum-images-count')))) {
                        drawAdvertisementsImages(subImageURI, userPersonlAdvertisementImagesArray.length);
                        userPersonlAdvertisementImagesArray.push({ 'base64': subImageURI, 'imageExtension': 'jpg' });

                    }
                    else {
                        var maxImagesCount = localStorage.getItem('edit-tourist-details-visit-maximum-images-count') == "null" ? '0' : localStorage.getItem('edit-tourist-details-visit-maximum-images-count');
                        notificationService.alert('نجاخ', 'لقد تم إستنفاذ العدد المسموح للصور المرفقة ..بإمكانك حذف صورة مصاحبة قبل التمكن من رفع صورة اخرى', function () { });
                    }
                }
                else {
                    //DrawAdvertisementsSubMainImages(subImageURI, userPersonlAdvertisementImagesArray.length);
                    drawAdvertisementsImages(subImageURI, userPersonlAdvertisementImagesArray.length);
                    userPersonlAdvertisementImagesArray.push({ 'base64': subImageURI, 'imageExtension': 'jpg' });
                }
            }, function (message) { },
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
        $('#btnEditUserPersonaAd').unbind().on('click', function () {
            if ($('#advertismentDetailsEdit').parsley().validate()) {
                var personalUserLocationLatitude = localStorageService.getValue(localStorageService.getAllKeys().touristDetailsAddressLatitude);
                var personalUserLocationLongtitude = localStorageService.getValue(localStorageService.getAllKeys().touristDetailsAddressLongtitude);
                var personalUserAdvertisementId = $('#personalUserAdId').val();
                var personalUserAdvertisementCategoryId = $('#personalUserCategoryId').val();
                var personalUserAdvertisementPackageId = $('#personalUserPackageId').val();
                var PersonalUserAdvertisementName = $('#txtEditPersonalAdName').val();
                var PersonalUserAdvertisementCity = $('#txtEditPersonalAdCity').val();
                var PersonalUserAdvertisementDescription = $('#txtEditPersonalAdDescription').val();
                var PersonalUserAdvertisementWorkingHours = $('#txtEditPersonalAdWorkingHours').val();

                //8	سناب شات
                //9	انستجرام
                //10	فيسبوك
                //11	تويتر
                //12	الموقع الإلكترونى
                //13	البريد الإلكترونى
                //14	رقم الجوال
                //15	إرفاق صور

                var personalUserAdvertisementMobile = $('#user-project-details-mobile-show').is(":visible") ? $('#user-project-details-mobile').val().trim() : null;
                var personalUserAdvertisementSnap = $('#user-project-details-snap-show').is(":visible") ? $('#user-project-details-snap').val().trim() : null;
                var personalUserAdvertisementInstgram = $('#user-project-details-instgram-show').is(":visible") ? $('#user-project-details-instgram').val().trim() : null;
                var personalUserAdvertisementFacebook = $('#user-project-details-facebook-show').is(":visible") ? $('#user-project-details-facebook').val().trim() : null;
                var personalUserAdvertisementTwitter = $('#user-project-details-twitter-show').is(":visible") ? $('#user-project-details-twitter').val().trim() : null;
                var personalUserAdvertisementWebSite = $('#user-project-details-website-show').is(":visible") ? $('#user-project-details-website').val().trim() : null;
                var personalUserAdvertisementEmail = $('#user-project-details-email-show').is(":visible") ? $('#user-project-details-email').val().trim() : null;

                var mainImageBase64 = null;
                if ($('#edit-personal-project-main-image').attr('src').indexOf('data:image/png;base64,') != -1) {
                    var mainImageBase64 = $('#edit-personal-project-main-image').attr('src').replace('data:image/png;base64,', '');
                }


                dataContextService.advertisement.editUserAdvertisement(personalUserAdvertisementId, PersonalUserAdvertisementName,
                 PersonalUserAdvertisementCity, personalUserAdvertisementCategoryId, personalUserAdvertisementPackageId, PersonalUserAdvertisementDescription,
                 personalUserLocationLatitude, personalUserLocationLongtitude, PersonalUserAdvertisementWorkingHours, personalUserAdvertisementMobile, personalUserAdvertisementEmail, personalUserAdvertisementWebSite,
                 userPersonlAdvertisementImagesArray, personalUserAdvertisementMobile, personalUserAdvertisementTwitter, personalUserAdvertisementFacebook,
                 personalUserAdvertisementInstgram, personalUserAdvertisementSnap, mainImageBase64,
                 function (advertisementDetails) {
                     if (advertisementDetails) {
                         notificationService.alert('تم', 'تم التعديل بنجاح', function () { });
                         localStorageService.removeByKey(localStorageService.getAllKeys().touristDetailsAddressLatitude);
                         localStorageService.removeByKey(localStorageService.getAllKeys().touristDetailsAddressLongtitude);
                         mainView.router.loadPage({ pageName: 'advertismentsProfileProjects' });
                         userPersonlAdvertisementImagesArray = [];
                         $('#personalUserAdId').val("");
                         $('#personalUserCategoryId').val("");
                         $('#personalUserPackageId').val("");
                         $('#txtEditPersonalAdName').val("");
                         $('#txtEditPersonalAdCity').val("");
                         $('#personalUserLocationLatitude').val("");
                         $('#personalUserLocationLongtitude').val("");
                         $('#txtEditPersonalAdDescription').val("");
                         $('#txtEditPersonalAdWorkingHours').val("");
                         myApp.closeModal()

                     }
                 });
            }

        });
    }
    var service = {
        drawPersonalPageProjectsDetailsImages: drawPersonalPageProjectsDetailsImages,
        goToPersonalPageProjectsDetails: goToPersonalPageProjectsDetails,
        DrawProjectDetailImageForDeleted: drawProjectDetailImageForDeleted,
        editUserPersonalAd: editUserPersonalAd,
        drawAdvertisementsImages, drawAdvertisementsImages
        };
    return service;

        }) ();




