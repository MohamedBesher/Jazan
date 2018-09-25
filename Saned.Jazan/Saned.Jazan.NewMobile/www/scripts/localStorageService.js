var localStorageService = (function () {



    var getAllKeys = function () { return appStorageKeys };

    var setValue = function (key, value) {
        localStorage.setItem(key, value);
    }

    var getValue = function (key) {
        return localStorage.getItem(key);
    }

    var removeByKey = function (key) {
        localStorage.removeItem(key);
    }

    var appStorageKeys = {
        notificationPageNumber: "notificationPageNumber",
        advertismentsPageNumber: "advertisments-page-number",
        newsPageNumber: "newsPageNumber",
        ACST: "ACST",
        userName: "USName",
        appToken: "appToken",
        loginUsingSocial: "loginUsingSocial",
        userLoggedIn: "userLoggedIn",
        userPhoto: "usrPhoto",
        userEntersCode: "UserEntersCode",
        userId: "UserID",
        refreshToken: "refreshToken",
        userID: "UserID",
        touristDetailsAddressLatitude: "tourist-details-address-latitude",
        touristDetailsAddressLongtitude: "tourist-details-address-address-longtitude",
        touristDetailsAddress: "tourist-details-address",
        categotyId: 'categoryId',
        userProjectId: 'userProjectId',
        placeDetails: 'placeDetails',
        bannerDetailsId: 'bannerDetailsId',
        subCategoryId: 'subCategoryId',
        packageId: 'packageId',
        mainBanner: 'mainBanner',
        categoryBetweenBanner: 'categoryBetweenBanner',
        map: 'map',
        advertisementEdit: 'advertisementEdit',
        uploadImageNoLimit: 'uploadImageNoLimit',
        upCategoryBanner: 'upCategoryBanner',
        advertisementNotification: 'advertisementNotification',
        snapChat: 'snapChat',
        instgram: 'instgram',
        facebook: 'facebook',
        twitter: 'twitter',
        website: 'website',
        email: 'email',
        mobile: 'mobile',
        uploadImageLimited: 'uploadImageLimited',
        maxProfileAdvertisement: 'maxProfileAdvertisement',
        maxProfileTouristVisit: 'maxProfileTouristVisit',
        pageSizeInfinite: 'pageSizeInfinite',
        maxItemsPlaces: 'maxItemsPlaces',
        maxItemComments: 'maxItemComments',
        subMainCategoryId: 'subMainCategoryId',
        instgram: 'instgram',
        map: 'map',
        facebook: 'facebook',
        twitter: 'twitter',
        mobile: 'mobile',
        email: 'email',
        website: 'website',
        snapChat: 'snapChat',
        profileImageURI: 'profileImageURI',
        mainCategoryId: 'mainCategoryId',
        subMainCategoryId: 'subMainCategoryId',
        bannerDetailsId: 'bannerDetailsId',
        touristPageNumber: 'tourist-page-number',
        touristCommentPageNumber: 'tourist-comment-page-number',
        touristPageSize: 'tourist-page-size',
        touristVisitId: 'tourist-visit-id',
        touristCommentPageNumber: 'tourist-comment-page-number',
        advertisementID: 'advertisementID',
        personalBannerDetailsId: 'personalBannerDetailsId',
        Visitor: 'Visitor',
        appToken: 'appToken',
        usrPhoto: 'usrPhoto',
        USName: 'USName',
        refreshToken: 'refreshToken',
        Visitor: 'Visitor',
        loginUsingSocial: 'loginUsingSocial',
        userLoggedIn: 'userLoggedIn',
        usrPhoto: 'usrPhoto',
        USName: 'USName',
        appToken: 'appToken',
        Visitor: 'Visitor',
        loginUsingSocial: 'loginUsingSocial',
        userLoggedIn: 'userLoggedIn',
        usrPhoto: 'usrPhoto',
        USName: 'USName',
        appToken: 'appToken',
        loginUsingSocial: 'loginUsingSocial',
        Visitor: 'Visitor',
        userLoggedIn: 'userLoggedIn',
        usrPhoto: 'usrPhoto',
        userType: 'userType',
        USName: 'USName',
        UserID: 'UserID',
        UserEntersCode: 'UserEntersCode',
        UserEntersCode: 'UserEntersCode',
        confirmationMail: 'confirmationMail'
    }

    var service = {
        getAllKeys: getAllKeys,
        getValue: getValue,
        setValue: setValue,
        removeByKey: removeByKey
    };

    return service;

})();