var dataContextService = (function () {

    var selectMaximumImagesCountByAdvertisementId = function (advertisementId, callback) {
        return callService("GET", 'api/Advertisements/SelectMaximumImagesCount/' + advertisementId,
         null, "application/json", callback);
    }

    var getPagedNews = function (pageNumber, pageSize, callback) {
        return callService("POST", 'api/News/GetNews',
           {
               "pageNumber": pageNumber,
               "pageSize": pageSize
           }, "application/json", callback);
    }

    var getNewsById = function (newsId, callback) {
        return callService("GET", 'api/News/GetSingleNews/' + newsId,
           null, "application/json", callback);
    }


    var getPagedNotifications = function (pageNumber, pageSize, userId, callback) {
        return callService("GET", 'api/Notification/SelectNotificationByUserId/' + userId + '/' + pageNumber + '/' + pageSize,
            null, "application/json", callback);
    }

    var addDevice = function (deviceId, callback) {
        return callService("POST", 'api/Notification/AddDevice/' + deviceId,
           null, "application/json", callback);
    }

    var getAboutUs = function (callback) {
        return callService("GET", "api/MobileSettings/GetMobileSettingBySettingType/" + appSettingService.mobileSettingEnum.aboutUs.id,
            null, "application/json", callback);
    }

    var getContactUs = function (callback) {
        return callService("GET", "api/MobileSettings/GetAllMobileSetting/",
            null, "application/json", callback);
    }

    var getPagedComments = function (pageNumber, relatedId, relatedTypeId, callback) {
        return callService("GET", 'api/Comments/GetPagedComments/' + pageNumber + '/5/' + relatedId + '/' + relatedTypeId,
            null, "application/json", callback);
    }

    var addTouristVisits = function (name, cityName, visitDate, latitude, longitude, description, mainImageBase64, youTubeUrls, images, callback) {
        return callService("POST", "api/TouristVisit/Add",
           {
               "name": name,
               "cityName": cityName,
               "visitDate": visitDate,
               "latitude": latitude,
               "longitude": longitude,
               "description": description,
               "mainImageBase64": mainImageBase64,
               "mainImageExtension": "jpg",
               "youTubeUrls": youTubeUrls,
               "images": images,
           }, "application/json", callback);
    }

    var calculateDynamicPageSize = function (callback) {
        return callService("POST", "api/TouristVisit/CalculateDynamicPageSize", null, "application/x-www-form-urlencoded", callback);
    }

    var saveView = function (relatedId, relatedTypeId, callback) {
        return callService("POST", "api/Views/SaveView",
            {
                "RelatedTypeId": relatedTypeId,
                "RelatedId": relatedId,
                "DeviceId": localStorage.getItem('deviceId')
            }, "application/json", callback);
    }

    var getById = function (touristDetailsId, callback) {
        return callService("GET", 'api/TouristVisit/GetById/' + touristDetailsId, null, "application/json", callback);
    }

    var saveRating = function (rateValue, relatedId, relatedType, callback) {
        return callService("POST", "api/Rating/SaveRating",
            {
                "RateValues": '1' + ':' + rateValue,
                "RelatedId": relatedId,
                "RelatedType": relatedType,
            }, "application/json", callback);
    }

    var saveComment = function (commentText, relatedId, commentTypeId, callback) {
        return callService("POST", "api/Comments/SaveComment/",
            {
                "CommentText": commentText,
                "RelatedId": relatedId,
                "commentTypeId": commentTypeId
            }, "application/json", callback);
    }

    var getPagedTouristVisits = function (pageSize, pageNumber, userId, isApproved, callback) {
        return callService("POST", "api/TouristVisit/GetPaged",
            {
                "pageSize": pageSize,
                "pageNumber": pageNumber,
                "userId": userId,
                "isApproved": isApproved,
            }, "application/json", callback);
    }
    var getPagedUserProfileProjects = function (pageSize, pageNumber, UserId, callback) {
        return callService('POST', 'api/Advertisements/GetPagedAdvertisement', {
            "PageSize": pageSize,
            "PageNumber": pageNumber,
            "UserId": UserId
        }, "application/json", callback)
    }
    //var getPagedTouristVisits = function (pageSize, pageNumber, UserId, callback) {
    //    return callService('POST', 'api/TouristVisit/GetPaged', {
    //        "PageSize": pageSize,

    //        "PageNumber": pageNumber,

    //        "UserId": UserId
    //    }, "application/json", callback)
    //}
    var deleteUserTouristVisit = function (touristId, callback) {
        return callService('DELETE', 'api/TouristVisit/Delete/' + touristId, "", "application/json", callback)
    }
    var deleteUserProjects = function (projectId, callback) {
        return callService('DELETE', 'api/Advertisements/DeleteAdvertisement/' + projectId, "", "application/x-www-form-urlencoded", callback)
    }
    var deleteUserProjectsImages = function (imageId, callback) {
        return callService('DELETE', 'api/Advertisements/DeleteImageAdvertisement/' + imageId, "", "application/json", callback)
    }
    var deleteUserTouristImages = function (imageId, callback) {
        return callService('DELETE', 'api/TouristVisit/DeleteTouristVisitImage/' + imageId, "", "application/json", callback)
    }

    var editUserAdvertisement = function (advertisementId, advertisementName, cityName,
    categoryId, packageId, description, Latitude, longitude, workingHours, mobile, email, webSite, advertisementImages,
           mobile, twitter, faceBook, instagram, snapchat, mainImageBase64, callback) {
        return callService('POST', 'api/Advertisements/Edit/',
            {
                "id": advertisementId,
                "name": advertisementName,
                "cityName": cityName,
                "categoryId": categoryId,
                "packageId": packageId,
                "description": description,
                "Latitude": Latitude,
                "longitude": longitude,
                "workingHours": workingHours,
                "Email": email,
                "advertisementImages": advertisementImages,
                "WebSite": webSite,
                "mobile": mobile,
                "twitter": twitter,
                "faceBook": faceBook,
                "instagram": instagram,
                "snapchat": snapchat,
                "mainImageBase64": mainImageBase64,
                "mainImageExtension": "jpg",
            }, "application/json", callback);
    }
    var editUserTourist = function (advertisementId, advertisementName, cityName, visitDate,
          description, Latitude, longitude, images, mainImageBase64, youTubeUrls, callback) {
        return callService('POST', 'api/TouristVisit/Edit/',
            {
                "id": advertisementId,
                "name": advertisementName,
                "cityName": cityName,
                "visitDate": visitDate,
                "description": description,
                "latitude": Latitude,
                "longitude": longitude,
                "images": images,
                "mainImageBase64": mainImageBase64,
                "mainImageExtension": "jpg",
                "youTubeUrls": youTubeUrls

            }, "application/json", callback);
    }
    var editUserProfile = function (photoUrl, fullName, phoneNumber, callback) {
        return callService('POST', 'api/User/EditUser', {
            photoUrl: photoUrl,
            fullName: fullName,
            phoneNumber: phoneNumber,
        }, "application/json", callback)
    }
    var getUserProfileProjectDetails = function (advertisementID, UserId, callback) {
        return callService('GET', 'api/Advertisements/' + advertisementID + "/" + UserId, '', "application/json", callback)
    }
    var getUserProfileTouristDetails = function (touristID, callback) {
        return callService('GET', 'api/TouristVisit/GetById/' + touristID, '', "application/json", callback)
    }
    var getCultureCompetitonQuestion = function (callback) {
        return callService('GET', 'api/CulturalCompetition/SelectActiveQuestion', '', "application/json", callback)
    }

    var sendCultureCompetitionAnswer = function (questionId, answer, callback) {
        return callService('POST', 'api/CulturalCompetition/SaveAnswer', {
            "questionId": questionId,
            "value": answer
        }, "application/json", callback)
    }
    var getActiveWinners = function (callback) {
        return callService('GET', 'api/CulturalCompetition/SelectWinnerUsers', '', "application/json", callback)
    }
    var getActiveSponser = function (callback) {
        return callService('GET', 'api/CulturalCompetition/SelectActiveSponsors', '', "application/json", callback)
    }
    var getMainBannerAdvertisement = function (pageName, callback) {

        return callService('POST', 'api/Advertisements/GetMainBannerAdvertisements', {
            'PageNumber': pageName,
            'PageSize': 5000

        }, "application/json", callback)
    }
    function callService(callType, methodName, dataVariables, contentType, callback) {
        if (methodName === 'api/Advertisements/GetMainBannerAdvertisements') {

        } else {
            ShowLoader('');
        }
        if (!contentType) {
            contentType = "application/json";
        }

        var token = localStorageService.getValue(localStorageService.getAllKeys().appToken);

        if (dataVariables) {
            dataVariables = JSON.stringify(dataVariables);
        }


        $.ajax({
            type: callType,
            url: serviceURL + methodName,
            headers: {
                "content-type": contentType,
                "cache-control": "no-cache",
                "authorization": "bearer " + token
            },
            data: dataVariables,
            async: true,
            crossDomain: true
        }).done(function (result) {
            if (methodName === 'api/Advertisements/GetMainBannerAdvertisements') {
                //$('[data-page].page-on-center .page-content .advHeader').block({
                //    css: {
                //        backgroundColor: 'transparent',
                //        border: 'none'
                //    },
                //    message: '<div class="preloader-indicator-modal"><span class="preloader preloader-white"><span class="preloader-inner"><span class="preloader-inner-gap"></span><span class="preloader-inner-left"><span class="preloader-inner-half-circle"></span></span><span class="preloader-inner-right"><span class="preloader-inner-half-circle"></span></span></span></span></div><div class="preloader-indicator-modal"><span class="preloader preloader-white"><span class="preloader-inner"><span class="preloader-inner-gap"></span><span class="preloader-inner-left"><span class="preloader-inner-half-circle"></span></span><span class="preloader-inner-right"><span class="preloader-inner-half-circle"></span></span></span></span></div>',
                //    overlayCSS: { backgroundColor: 'transparent' }
                //});
            } else {
                HideLoader('');
            }

            return callback(result);
        }).fail(function (error, textStatus) {
            if (methodName === 'api/Advertisements/GetMainBannerAdvertisements') {
                //$('[data-page].page-on-center .page-content .advHeader').unblock();
            } else {
                HideLoader('');
            }

            if (error.status === 401) {
                if (localStorage.getItem('refreshToken')) {
                    notificationService.alert('خطأ', 'مدة صلاحية رمز التحقق الخاص بك قد انتهت , جاري تنشيط رمز التحقق  .', function () {

                        RefreshToken('', callType, 'Token', function (result) {

                            localStorage.setItem('appToken', result.access_token);
                            localStorage.setItem('refreshToken', result.refresh_token);
                            notificationService.alert('نجاح', 'تم تنشيط رمز التحقق الخاص بك ', function () {
                                //localStorage.removeItem('USName');
                                //localStorage.removeItem('userLoggedIn');
                                //localStorage.removeItem('Visitor');
                                //localStorage.removeItem('loginUsingSocial');
                                //mainView.router.loadPage({ pageName: 'login' });
                                mainView.router.loadPage({ pageName: 'intro' });
                                return callback(null);
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
                        return callback(null);
                    });
                }
            } else {
                return callback(null);
            }

        });
    }

    function refreshAccessToken(pageName, CallType, MethodName, callback) {
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


    var service = {
        news:
            {
                getPaged: getPagedNews,
                getById: getNewsById
            },
        mobileSetting:
            {
                getAboutUs: getAboutUs,
                getContactUs: getContactUs
            },
        notifications:
            {
                getPaged: getPagedNotifications,
                addDevice: addDevice
            },
        user: {
            editUserProfile: editUserProfile

        },
        comments:
            {
                getPaged: getPagedComments,
                add: saveComment
            },
        touristVisits:
            {
                add: addTouristVisits,
                calculateDynamicPageSize: calculateDynamicPageSize,
                getById: getById,
                getPaged: getPagedTouristVisits,
                deleteTourist: deleteUserTouristVisit,
                getTouristDetails: getUserProfileTouristDetails,
                deleteUserTouristImages: deleteUserTouristImages,
                editUserTourist: editUserTourist
            },
        view:
            {
                add: saveView
            },
        rating:
            {
                add: saveRating
            },
        advertisement: {
            getPaged: getPagedUserProfileProjects,
            deleteAdvertisement: deleteUserProjects,
            getAdvertisementDetails: getUserProfileProjectDetails,
            deleteUserProjectsImage: deleteUserProjectsImages,
            editUserAdvertisement: editUserAdvertisement,
            getMainBannerAdvertisement: getMainBannerAdvertisement,
            selectMaximumImagesCountByAdvertisementId: selectMaximumImagesCountByAdvertisementId

        },
        cultureCompetiton: {
            getQuestion: getCultureCompetitonQuestion,
            sendAnswer: sendCultureCompetitionAnswer,
            getWinners: getActiveWinners,
            getActiveSponser: getActiveSponser

        }
    };

    return service;

})();
