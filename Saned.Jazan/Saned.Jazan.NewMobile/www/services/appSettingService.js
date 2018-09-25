var appSettingService = (function () {

    var mobileSettingEnum =
        {
            aboutUs: { id: 1, icon: '' },
            mobileNumber: { id: 2, icon: 'whatsapp.png' },
            line: { id: 3, icon: 'line.png' },
            twitter: { id: 4, icon: 'tw.png' },
            instgram: { id: 5, icon: 'instagram.png' },
            snapchat: { id: 6, icon: 'snapchat.png' },
            blackBerry: { id: 7, icon: 'bbm.png' },
            email: { id: 8, icon: 'telegram.png' }
        };



    var service = {
        mobileSettingEnum: mobileSettingEnum,
        serviceURL: 'http://192.168.0.99:1993/',
        imageHostURL: 'http://192.168.0.99:1993/uploads/'
    };

    return service;

})();

