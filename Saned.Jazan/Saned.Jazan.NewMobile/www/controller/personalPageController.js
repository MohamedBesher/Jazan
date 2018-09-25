var personalPageController = (function () {
    var userImageUri;
    var drawPersonalPage = function () { };
    var goToPersonalPage = function (page) {
        if (typeof page != 'undefined') {
            //loadSideMenuLinks();
            var userLoged = JSON.parse(localStorage.getItem('userLoggedIn'));
            if (userLoged) {
                $('#userImage').attr('src', imgHost + userLoged.photoUrl);
                var userImage = document.getElementById('userImage');
                userImage.setAttribute('onerror', 'replaceURL(this,"inner")');
                $('#userName').text(userLoged.fullName);

                if (userLoged.email) {
                    $('#user-email-wrapper').show();
                    $('#userEmail').text(userLoged.email);
                } else {
                    $('#user-email-wrapper').hide();
                }
              
                $('#userMobile').text(userLoged.phoneNumber);
            }
            else {
                notificationService.alert('تنبيه', 'قم بتسجيل الدخول اولا', function () { });
                mainView.router.loadPage({ pageName: 'login' });
            }

            $('#linkOpenUserProfilePopup').on('click', function () {
                myApp.popup('.popup-profileDetailsEdit');
                $$('.popup-profileDetailsEdit').on('opened', function () {
                    if (userLoged) {
                        $('#profileUserImage').attr('src', imgHost + userLoged.photoUrl);
                        $('#txtUserProfileName').val(userLoged.fullName);
                        $('#txtUserMobileNumber').val(userLoged.phoneNumber);
                        $('#txtUserProfileEmail').val(userLoged.email)
                    }
                    $('#btnAddProfilePic').unbind().on('click', function () {
                        navigator.camera.getPicture(function (userImageUri) {
                            localStorage.setItem('userImageUri', userImageUri)
                            $('#profileUserImage').attr('src', 'data:image/jpeg;base64,' + userImageUri);
                        }
                        ,
                            function (message) {
                                //notificationService.alert('خطا اثناء تحميل الصورة', 'تنبيه', function () { });
                            },
                        {
                            quality: 50,
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
                    $('#btnEditUserDetails').unbind().on('click', function () {
                        if ($('#userProfileEdit').parsley().validate()) {
                            var txtUserProfileName = $('#txtUserProfileName').val();
                            var txtUserMobileNumber = $('#txtUserMobileNumber').val();
                            //var txtUserProfileEmail = $('#txtUserProfileEmail').val();

                            dataContextService.user.editUserProfile(localStorage.getItem('userImageUri'), txtUserProfileName, txtUserMobileNumber, function (result) {
                                if (result != null) {
                                    
                                    notificationService.alert('تم', 'تم تعديل  بياناتك بنجاح', function () {
                                        var loggedInUser = JSON.parse(localStorage.getItem('userLoggedIn'));
                                        CallService('login', "POST", "api/User/FindByUserId/" + loggedInUser.userName, '', function (res) {
                                            if (res != null) {
                                                localStorage.setItem('userLoggedIn', JSON.stringify(res));
                                                if (typeof res.photoUrl != 'undefined' && res.photoUrl != null && res.photoUrl != '' && res.photoUrl != ' ') {
                                                    localStorage.setItem('usrPhoto', res.photoUrl);
                                                }
                                                else {
                                                    localStorage.removeItem('usrPhoto');
                                                }
                                                myApp.closeModal();
                                                mainView.router.loadPage({ pageName: 'intro' });
                                            }
                                            else {
                                            }
                                        });
                                    });





                                }
                                else {

                                }
                            });
                        }
                    });
                });
            });
        }
    }

    var service = {
        drawPersonalPage: drawPersonalPage,
        goToPersonalPage: goToPersonalPage
    };

    return service;

})();

