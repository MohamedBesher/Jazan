var userPersonlTouristImagesArray = [];

var personalPageTouristDetailController = (function () {

    var initDeleteVideo = true;
    var touristVideosArray = [];
    var deletedVideosArray = [];
    var touristVideosArrayAfterDeletedImages = [];
    var touristVideosArrayWithId = [];



    var touristImagesArray = [];
    var deletedTouristImagesArray = [];
    var touristImagesArrayWithId = [];
    var touristImagesArrayAfterDeletedImages = [];
    var initDeleteTourist = true;
    var initDeleted = false;
    var touristDate;
    var touristVisitImageDeleted;
    $('#edit-tourist-visit-main-image-button').unbind().click(function () {
        navigator.camera.getPicture(onSuccess, onFail, {
            quality: 50,
            destinationType: Camera.DestinationType.DATA_URL,
            sourceType: Camera.PictureSourceType.SAVEDPHOTOALBUM,
        });
        function onSuccess(imageURI) {
            var image = document.getElementById('edit-tourist-visit-main-image');
            image.src = 'data:image/png;base64,' + imageURI;
        }
        function onFail(message) {

        }
    });
    var drawTouristImages = function drawTouristImages(imageURI, index) {

        //var subMainPicShow = document.getElementById('add-personal-tourist-images');
        //var divDrawed = document.createElement('div');
        //var imageAdvertisementSubMain = document.createElement('img');
        //divDrawed.className = "img_content";
        //imageAdvertisementSubMain.setAttribute('src', 'data:image/jpeg;base64, ' + imageURI);
        //divDrawed.appendChild(imageAdvertisementSubMain);

        //subMainPicShow.insertBefore(divDrawed, subMainPicShow.firstChild);

        var subMainPicShow = document.getElementById('add-personal-tourist-images');
        var divDrawed = document.createElement('div');
        divDrawed.setAttribute('id', 'tourist-visit-edit-image-' + index);
        var imageAdvertisementSubMain = document.createElement('img');
        divDrawed.className = "col-25";
        imageAdvertisementSubMain.setAttribute('src', 'data:image/jpeg;base64, ' + imageURI);
        imageAdvertisementSubMain.className = 'image-responsive';

        var deleteImageButton = document.createElement('a');
        deleteImageButton.className = 'button button-round red';
        deleteImageButton.setAttribute('onclick', ' notificationService.confirm("هل انت متأكد من حذف الصورة",function(buttonIndex){  if(buttonIndex==1){ userPersonlTouristImagesArray.splice(' + index + ',1); $("#' + 'tourist-visit-edit-image-' + index + '").remove(); }})');

        deleteImageButton.innerHTML = 'حذف';


        divDrawed.appendChild(imageAdvertisementSubMain);
        divDrawed.appendChild(deleteImageButton);
        subMainPicShow.insertBefore(divDrawed, subMainPicShow.firstChild);
    }

    var drawPersonalPageTouristDetailsVideos = function drawPersonalPageTouristDetailsImages(touristDetailsImages, id) {

        if (id) {
            var projectImages = document.getElementById(id);
            projectImages.innerHTML = "";
            for (var i = 0; i < touristDetailsImages.length; i++) {
                var divDrawed = document.createElement('div');
                var linkImageBrowsing = document.createElement('a');
                var imgProject = document.createElement('img');
                divDrawed.setAttribute('id', touristDetailsImages[i].touristVisitImageId);
                divDrawed.className = "col-40";
                imgProject.setAttribute('src', 'https://i.ytimg.com/vi/' + touristDetailsImages[i].mediaUrl + '/hqdefault.jpg');
                linkImageBrowsing.setAttribute('id', 'linkImageBrowing_' + touristDetailsImages[i].touristVisitImageId);
                linkImageBrowsing.className = "pb-popup-dark2";
                linkImageBrowsing.appendChild(imgProject);
                divDrawed.appendChild(linkImageBrowsing);
                projectImages.appendChild(divDrawed);
                $('#linkImageBrowing_' + touristDetailsImages[i].touristVisitImageId).on('click', function () {
                });
                if (deletedVideosArray.length > 0) {
                    for (var d = 0; d < deletedVideosArray.length; d++) {
                        if (deletedVideosArray[d].id == touristDetailsImages[i].touristVisitImageId) {
                            touristVideosArrayAfterDeletedImages.push(touristDetailsImages[i].touristVisitImageId);
                            $('#' + touristDetailsImages[i].touristVisitImageId).remove();
                            for (var t = 0; t < touristVideosArrayAfterDeletedImages.length; t++) {
                                for (var l = 0; l < touristVideosArrayWithId.length; l++) {
                                    if (touristVideosArrayAfterDeletedImages[t] == touristVideosArrayWithId[l].id) {
                                        touristVideosArrayWithId.pop(touristVideosArrayWithId[l].id);
                                    }
                                }
                            }
                            touristVideosArray = [];
                            for (var h = 0; h < touristVideosArrayWithId.length; h++) {
                                touristVideosArray.push(touristVideosArrayWithId[h].image);
                            }

                        }
                    }
                }
            }
        }
    }

    var drawPersonalPageTouristDetailsImages = function drawPersonalPageTouristDetailsImages(touristDetailsImages, id) {


        if (id) {
            var projectImages = document.getElementById(id);
            projectImages.innerHTML = "";
            for (var i = 0; i < touristDetailsImages.length; i++) {
                var divDrawed = document.createElement('div');
                var linkImageBrowsing = document.createElement('a');
                var imgProject = document.createElement('img');
                divDrawed.setAttribute('id', touristDetailsImages[i].touristVisitImageId);
                divDrawed.className = "col-40";
                imgProject.setAttribute('src', imgHost + touristDetailsImages[i].mediaUrl);
                linkImageBrowsing.setAttribute('id', 'linkImageBrowing_' + touristDetailsImages[i].touristVisitImageId);
                linkImageBrowsing.className = "pb-popup-dark2";
                linkImageBrowsing.appendChild(imgProject);
                divDrawed.appendChild(linkImageBrowsing);
                projectImages.appendChild(divDrawed);
                $('#linkImageBrowing_' + touristDetailsImages[i].touristVisitImageId).on('click', function () {
                    //var myPhotoBrowserPopupDark = myApp.photoBrowser({
                    //    photos: touristImagesArray,
                    //    theme: 'dark',
                    //    type: 'popup',
                    //    backLinkText: 'إغلاق',
                    //    ofText: 'من'
                    //});
                    //myPhotoBrowserPopupDark.open();
                });
                if (deletedTouristImagesArray.length > 0) {
                    for (var d = 0; d < deletedTouristImagesArray.length; d++) {
                        if (deletedTouristImagesArray[d].id == touristDetailsImages[i].touristVisitImageId) {
                            touristImagesArrayAfterDeletedImages.push(touristDetailsImages[i].touristVisitImageId);
                            $('#' + touristDetailsImages[i].touristVisitImageId).remove();
                            for (var t = 0; t < touristImagesArrayAfterDeletedImages.length; t++) {
                                for (var l = 0; l < touristImagesArrayWithId.length; l++) {
                                    if (touristImagesArrayAfterDeletedImages[t] == touristImagesArrayWithId[l].id) {
                                        touristImagesArrayWithId.pop(touristImagesArrayWithId[l].id);
                                    }
                                }
                            }
                            touristImagesArray = [];
                            for (var h = 0; h < touristImagesArrayWithId.length; h++) {
                                touristImagesArray.push(touristImagesArrayWithId[h].image);
                            }

                        }
                    }
                }
            }
        }
    }

    var drawTouristDetailVideoForDeleted = function drawTouristDetailVideoForDeleted(res) {

        var projectImages = document.getElementById('touristVideos');
        projectImages.innerHTML = "";
        for (var i = 0; i < res.length; i++) {
            var divDrawed = document.createElement('div');
            var linkImageBrowsing = document.createElement('a');
            var imgProject = document.createElement('img');
            var iDeleteIcon = document.createElement('i');
            var divImageContent = document.createElement('div');
            var divOverlay = document.createElement('div');
            divDrawed.setAttribute('id', res[i].touristVisitImageId);
            divDrawed.className = "col-40";
            divOverlay.className = "delOverlay";
            iDeleteIcon.className = "ionicons ion-android-delete";
            imgProject.setAttribute('src', 'https://i.ytimg.com/vi/' + res[i].mediaUrl + '/hqdefault.jpg');
            divImageContent.className = "imgContent";
            linkImageBrowsing.className = "pb-popup-dark2";
            linkImageBrowsing.setAttribute('id', 'linkDeletedImage_' + res[i].touristVisitImageId);
            divOverlay.appendChild(iDeleteIcon);
            divImageContent.appendChild(divOverlay);
            divImageContent.appendChild(imgProject);
            linkImageBrowsing.appendChild(divImageContent);
            divDrawed.appendChild(linkImageBrowsing);
            projectImages.appendChild(divDrawed);
            $('#linkDeletedImage_' + res[i].touristVisitImageId).on('click', function () {
                initDeleted = true;
                var imageDeletedId = $(this).attr('id').split('_')[1];
                localStorage.setItem('personal-tourist-details-video-deleted-id', imageDeletedId);

                // from here
                notificationService.confirm('هل انت متأكد من حذف الفيديو ؟', function (buttonIndex) {
                    if (buttonIndex == 1) {
                        dataContextService.touristVisits.deleteUserTouristImages(localStorage.getItem('personal-tourist-details-video-deleted-id'),
                            function (imageDeleted) {
                                if (imageDeleted != null) {
                                    notificationService.alert('تنبيه', 'تم حذف الفيديو بنجاح', function () {
                                        for (var i = 0; i < res.length; i++) {
                                            if (localStorage.getItem('personal-tourist-details-video-deleted-id') == res[i].touristVisitImageId) {
                                                deletedVideosArray.push({ 'id': imageDeletedId });
                                                $('#' + res[i].touristVisitImageId).remove();
                                            }
                                        }

                                        if ($('#touristVideos div.col-40') && $('#touristVideos div.col-40').length == 0) {
                                            $("#btnDeleteUserVideo").hide();
                                        }

                                        localStorage.removeItem('personal-tourist-details-video-deleted-id');
                                    });
                                }
                                else {
                                    notificationService.alert('تنبيه', 'لم يتم حذف الفيديو', function () {
                                        localStorage.removeItem('personal-tourist-details-video-deleted-id');
                                    });

                                }
                            });
                    }
                });
                //to here
            });
            if (deletedVideosArray.length > 0) {
                for (var d = 0; d < deletedVideosArray.length; d++) {
                    if (deletedVideosArray[d].id == res[i].touristVisitImageId) {
                        $('#' + res[i].touristVisitImageId).remove();

                    }
                }
            }

        }
    }

    var drawTouristDetailImageForDeleted = function drawTouristDetailImageForDeleted(res) {

        var projectImages = document.getElementById('touristImages');
        projectImages.innerHTML = "";
        for (var i = 0; i < res.length; i++) {
            var divDrawed = document.createElement('div');
            var linkImageBrowsing = document.createElement('a');
            var imgProject = document.createElement('img');
            var iDeleteIcon = document.createElement('i');
            var divImageContent = document.createElement('div');
            var divOverlay = document.createElement('div');
            divDrawed.setAttribute('id', res[i].touristVisitImageId);
            divDrawed.className = "col-40";
            divOverlay.className = "delOverlay";
            iDeleteIcon.className = "ionicons ion-android-delete";
            imgProject.setAttribute('src', imgHost + res[i].mediaUrl);
            divImageContent.className = "imgContent";
            linkImageBrowsing.className = "pb-popup-dark2";
            linkImageBrowsing.setAttribute('id', 'linkDeletedImage_' + res[i].touristVisitImageId);
            divOverlay.appendChild(iDeleteIcon);
            divImageContent.appendChild(divOverlay);
            divImageContent.appendChild(imgProject);
            linkImageBrowsing.appendChild(divImageContent);
            divDrawed.appendChild(linkImageBrowsing);
            projectImages.appendChild(divDrawed);
            $('#linkDeletedImage_' + res[i].touristVisitImageId).on('click', function () {
                initDeleted = true;
                var imageDeletedId = $(this).attr('id').split('_')[1];
                localStorage.setItem('personal-tourist-details-image-deleted-id', imageDeletedId);

                // from here
                notificationService.confirm('هل انت متأكد من حذف الصورة ؟', function (buttonIndex) {
                    if (buttonIndex == 1) {
                        dataContextService.touristVisits.deleteUserTouristImages(localStorage.getItem('personal-tourist-details-image-deleted-id'),
                            function (imageDeleted) {
                                if (imageDeleted != null) {
                                    notificationService.alert('تنبيه', 'تم حذف الصورة بنجاح', function () {
                                        for (var i = 0; i < res.length; i++) {
                                            if (localStorage.getItem('personal-tourist-details-image-deleted-id') == res[i].touristVisitImageId) {
                                                deletedTouristImagesArray.push({ 'id': imageDeletedId });
                                                $('#' + res[i].touristVisitImageId).remove();
                                            }
                                        }

                                        if ($('#touristImages div.col-40') && $('#touristImages div.col-40').length == 0) {
                                            $("#btnDeleteUserTouristPicture").hide();
                                        }

                                        localStorage.removeItem('personal-tourist-details-image-deleted-id');
                                    });
                                }
                                else {
                                    notificationService.alert('تنبيه', 'لم يتم حذف الصورة', function () {
                                        localStorage.removeItem('personal-tourist-details-image-deleted-id');
                                    });

                                }
                            });
                    }
                });
                //to here
            });
            if (deletedTouristImagesArray.length > 0) {
                for (var d = 0; d < deletedTouristImagesArray.length; d++) {
                    if (deletedTouristImagesArray[d].id == res[i].touristVisitImageId) {
                        $('#' + res[i].touristVisitImageId).remove();

                    }
                }
            }

        }
    }
    var editUserPersonalTourist = function editUserPersonalTourist() {

        $('#addUserPersonalTouristPicture').unbind().on('click', function () {
            navigator.camera.getPicture(function (touristImageURI) {
                drawTouristImages(touristImageURI, userPersonlTouristImagesArray.length);

                userPersonlTouristImagesArray.push({ 'base64': touristImageURI, 'imageExtension': 'jpg' });

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
        $('#btnEditUserPersonalTourist').unbind().on('click', function () {
            if ($('#userTouristVisitEdit').parsley().validate()) {
                $('#btnEditUserPersonalTourist').attr('disabled', true);
                var personalUserLocationLatitude = localStorageService.getValue(localStorageService.getAllKeys().touristDetailsAddressLatitude);
                var personalUserLocationLongtitude = localStorageService.getValue(localStorageService.getAllKeys().touristDetailsAddressLongtitude);

                if (personalUserLocationLatitude == null || personalUserLocationLongtitude == null) {
                    personalUserLocationLatitude = $('#personalUserLocationLatitude').val();
                    personalUserLocationLongtitude = $('#personalUserLocationLongtitude').val();
                }

                var personalUserTouristId = $('#personalUserTouristId').val();
                var txtEditPersonalTouristName = $('#txtEditPersonalTouristName').val();
                var txtEditPersonalVisitDate = $('#txtEditPersonalVisitDate').val();
                var txtEditPersonalTouristCity = $('#txtEditPersonalTouristCity').val();
                var txtEditPersonalTouristDescription = $('#txtEditPersonalTouristDescription').val();
                var mainImageBase64 = null;
                if ($('#edit-tourist-visit-main-image').attr('src').indexOf('data:image/png;base64,') != -1) {
                    var mainImageBase64 = $('#edit-tourist-visit-main-image').attr('src').replace('data:image/png;base64,', '');
                }
                var touristVisitsVideos = [];
                $("#edit-tourist-details-multiple-videos-wrapper div img").each(
    function () {
        touristVisitsVideos.push($(this).attr('src')
            .replace('https://i.ytimg.com/vi/', '').replace('/hqdefault.jpg', ''));
    });
                dataContextService.touristVisits.editUserTourist(personalUserTouristId, txtEditPersonalTouristName,
                 txtEditPersonalTouristCity, txtEditPersonalVisitDate, txtEditPersonalTouristDescription, personalUserLocationLatitude, personalUserLocationLongtitude,
                  userPersonlTouristImagesArray, mainImageBase64, touristVisitsVideos, function (touristDetails) {
                      $('#btnEditUserPersonalTourist').attr('disabled', false);
                      if (touristDetails) {
                          notificationService.alert('تم', 'تم التعديل بنجاح', function () {
                              localStorageService.removeByKey(localStorageService.getAllKeys().touristDetailsAddressLatitude);
                              localStorageService.removeByKey(localStorageService.getAllKeys().touristDetailsAddressLongtitude);
                              mainView.router.loadPage({ pageName: 'advertismentsProfileVisits' });
                              userPersonlTouristImagesArray = [];
                              $('#touristDetails #txtTouristName').text($('#txtEditPersonalTouristName').val());
                              $('#touristDetails #txtTouristDate').text($('#txtEditPersonalVisitDate').val());
                              $('#touristDetails #txtTouristCityName').text($('#txtEditPersonalTouristCity').val());
                              $('#touristDetails #pTouristDetails').text($('#txtEditPersonalTouristDescription').val());
                              myApp.closeModal();
                          });
                      }
                  });
            }

        });
    }
    var goToPersonalPageTouristDetails = function (page) {
        if (typeof page != 'undefined') {

            $('#btnDeleteUserTouristPicture').text('حذف الصور');
            $('#btnDeleteUserTouristPicture').removeClass('green');
            $('#btnDeleteUserTouristPicture').addClass('red');

            $('#btnDeleteUserVideo').text('حذف الفيديو');
            $('#btnDeleteUserVideo').removeClass('green');
            $('#btnDeleteUserVideo').addClass('red');

            touristVideosArray = [];
            deletedVideosArray = [];
            var touristVideos = document.getElementById('touristVideos');
            touristVideos.innerHTML = "";
            touristVideosArrayAfterDeletedImages = [];
            touristVideosArrayWithId = [];


            touristImagesArray = [];
            deletedTouristImagesArray = [];
            var touristImages = document.getElementById('touristImages');
            touristImages.innerHTML = "";
            touristImagesArrayAfterDeletedImages = [];
            touristImagesArrayWithId = [];
            //loadSideMenuLinks();
            var userLoged = JSON.parse(localStorage.getItem('userLoggedIn'));
            dataContextService.touristVisits.getTouristDetails(localStorage.getItem('userTouristId'), function (touristDetails) {
                if (touristDetails != null) {
                    var touristDate = touristDetails.visitDate.split('T')[0].split('-')[0] + '-' + touristDetails.visitDate.split('T')[0].split('-')[1] + '-' + touristDetails.visitDate.split('T')[0].split('-')[2];
                    $('#touristDetails #txtTouristName').text(touristDetails.name);
                    $('#touristDetails #txtTouristDate').text(touristDate);
                    $('#touristDetails #txtTouristCityName').text(touristDetails.cityName);
                    $('#touristDetails #pTouristDetails').text(touristDetails.description);
                    if (touristDetails.isApproved) {
                        $('#labelTouristDetailsApproved').show();
                        $('#labelTouristDetailsRefuse').hide();
                    } else {
                        $('#labelTouristDetailsApproved').hide();
                        $('#labelTouristDetailsRefuse').show();
                    }
                    if (touristDetails.youTubeUrls != null && touristDetails.youTubeUrls.length != 0) {
                        drawPersonalPageTouristDetailsVideos(touristDetails.youTubeUrls, "touristVideos");
                        for (var i = 0; i < touristDetails.youTubeUrls.length; i++) {
                            touristVideosArrayWithId.push({ 'id': touristDetails.youTubeUrls[i].touristVisitImageId, 'image': imgHost + touristDetails.youTubeUrls[i].mediaUrl });
                            touristVideosArray.push(touristVideosArrayWithId[i].image);
                            $('#btnDeleteUserVideo').show();
                        }
                    }
                    else {
                        $('#btnDeleteUserVideo').hide();
                    }

                    $('#btnDeleteUserVideo').unbind().on('click', function () {
                        if (touristDetails.youTubeUrls != null && touristDetails.youTubeUrls != "") {
                            if (initDeleteVideo) {
                                initDeleteVideo = false;
                                $('#btnDeleteUserVideo').text('تراجع');
                                $('#btnDeleteUserVideo').removeClass('red');
                                $('#btnDeleteUserVideo').addClass('green');
                                drawTouristDetailVideoForDeleted(touristDetails.youTubeUrls);
                            }
                            else {
                                initDeleteVideo = true;
                                $('#btnDeleteUserVideo').text('حذف الفيديو');
                                $('#btnDeleteUserVideo').removeClass('green');
                                $('#btnDeleteUserVideo').addClass('red');
                                drawPersonalPageTouristDetailsVideos(touristDetails.youTubeUrls, "touristVideos");
                            }
                        }
                    });


                    if (touristDetails.imagesUrl != null && touristDetails.imagesUrl.length != 0) {
                        drawPersonalPageTouristDetailsImages(touristDetails.imagesUrl, "touristImages");
                        for (var i = 0; i < touristDetails.imagesUrl.length; i++) {
                            touristImagesArrayWithId.push({ 'id': touristDetails.imagesUrl[i].touristVisitImageId, 'image': imgHost + touristDetails.imagesUrl[i].mediaUrl });
                            touristImagesArray.push(touristImagesArrayWithId[i].image);
                            $('#btnDeleteUserTouristPicture').show();
                        }
                    }
                    else {
                        $('#btnDeleteUserTouristPicture').hide();
                    }

                    if (touristDetails.latitude && touristDetails.longitude) {
                        initMapWithCoordinates('personal-tourist-map', touristDetails.latitude, touristDetails.longitude, touristDetails.cityName, true);
                    } else {
                        $('#personal-tourist-map-wrapper').html('');
                    }
                    $('#btnDeleteUserTouristPicture').unbind().on('click', function () {
                        if (touristDetails.imagesUrl != null && touristDetails.imagesUrl != "") {
                            if (initDeleteTourist) {
                                initDeleteTourist = false;
                                $('#btnDeleteUserTouristPicture').text('تراجع');
                                $('#btnDeleteUserTouristPicture').removeClass('red');
                                $('#btnDeleteUserTouristPicture').addClass('green');
                                drawTouristDetailImageForDeleted(touristDetails.imagesUrl);
                            }
                            else {
                                initDeleteTourist = true;
                                $('#btnDeleteUserTouristPicture').text('حذف الصور');
                                $('#btnDeleteUserTouristPicture').removeClass('green');
                                $('#btnDeleteUserTouristPicture').addClass('red');
                                drawPersonalPageTouristDetailsImages(touristDetails.imagesUrl, "touristImages");
                            }
                        }
                    });

                    // my logic 
                    $('#personal-edit-tourist-details-multiple-videos').unbind().click(function () {


                        var videoLink = $('#txt-personal-edit-tourist-details-video-link').val();
                        var regExp = /^((?:https?:)?\/\/)?((?:www|m)\.)?((?:youtube\.com|youtu.be))(\/(?:[\w\-]+\?v=|embed\/|v\/)?)([\w\-]+)(\S+)?$/;
                        var matchingResult = videoLink.match(regExp);
                        if (matchingResult) {
                            var index = $('#edit-tourist-details-multiple-videos-wrapper div.col-25').length;

                            var subMainPicShow = document.getElementById('edit-tourist-details-multiple-videos-wrapper');
                            var divDrawed = document.createElement('div');
                            divDrawed.setAttribute('id', 'edit-tourist-visit-video-' + index);
                            var imageAdvertisementSubMain = document.createElement('img');
                            divDrawed.className = "col-25";
                            imageAdvertisementSubMain.setAttribute('src', 'https://i.ytimg.com/vi/' + matchingResult[5] + '/hqdefault.jpg');
                            imageAdvertisementSubMain.className = 'image-responsive';

                            var deleteImageButton = document.createElement('a');
                            deleteImageButton.className = 'button button-round red';
                            deleteImageButton.setAttribute('onclick', 'notificationService.confirm("هل انت متأكد من حذف الفيديو",function(buttonIndex){  if(buttonIndex==1){ $("#edit-tourist-visit-video-' + index + '").remove(); }})');

                            deleteImageButton.innerHTML = 'حذف';


                            divDrawed.appendChild(imageAdvertisementSubMain);
                            divDrawed.appendChild(deleteImageButton);
                            subMainPicShow.insertBefore(divDrawed, subMainPicShow.firstChild);
                        } else {
                            notificationService.alert('خطأ', 'ادخل رابط يوتيوب صحيح');
                        }

                        $('#txt-personal-edit-tourist-details-video-link').val('');

                    });
                    $('#btnEditPersonalTouristDetail').unbind().on('click', function () {
                        userPersonlTouristImagesArray = [];
                        $('#edit-tourist-details-multiple-videos-wrapper').html('');
                        $('#add-personal-tourist-images').html('');
                        $('#personalUserTouristId').val(touristDetails.id);
                        $('#personalUserLocationLatitude').val(touristDetails.latitude);
                        $('#personalUserLocationLongtitude').val(touristDetails.longitude);
                        $('#txtEditPersonalTouristName').val(touristDetails.name);
                        $('#txtEditPersonalVisitDate').attr('type', 'text');
                        $('#txtEditPersonalVisitDate').val(touristDate);
                        $('#txtEditPersonalTouristCity').val(touristDetails.cityName);
                        $('#txtEditPersonalTouristDescription').val(touristDetails.description);
                        $('#edit-tourist-visit-main-image').attr('src', imgHost + touristDetails.imageUrl)
                        $('#userTouristVisitEdit').parsley().reset();

                        editUserPersonalTourist();
                        $$('.popup-touristDetailsEdit').on('opened', function () {



                            if (touristDetails.latitude && touristDetails.longitude) {
                                //$('#edit-tourist-details-map-wrapper').html('<input id="pac-input" class="controls" type="text" placeholder="اسم الموقع السياحى"><div id="edit-tourist-details-map" style="width:100%;height:200px;"></div>');
                                //intializeSearchMapLocation(touristDetails.cityName, touristDetails.latitude, touristDetails.longitude, touristDetails.cityName, 'edit-tourist-details-map');

                                $('#edit-tourist-details-map-wrapper').html('<div id="edit-tourist-details-map" style="width:100%;height:200px;"></div>');
                                initMapWithCoordinates('edit-tourist-details-map', '16.889359', '42.570567', 'Jazan', false);

                            }
                            else {
                                $('#edit-tourist-details-map-wrapper').html('');
                            }
                        });
                        myApp.popup('.popup-touristDetailsEdit');
                    });
                }
                else {
                    //notificationService.alert('خطا في استرجاع تفاصيل هذا المشروع', 'تنبيه', function () { });
                }
            });
        }
    };

    var service = {
        goToPersonalPageTouristDetails: goToPersonalPageTouristDetails,
        drawPersonalPageTouristDetailsImages: drawPersonalPageTouristDetailsImages
    };

    return service;

})();