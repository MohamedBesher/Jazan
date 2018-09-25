var accessToken;
var UserData = null;


var googleapi = {
    authorize: function (options) {
        var deferred = $.Deferred();
        var authUrl = 'https://accounts.google.com/o/oauth2/auth?' + $.param({
            client_id: options.client_id,
            redirect_uri: options.redirect_uri,
            response_type: 'code',
            scope: options.scope
        });

        var authWindow = window.open(authUrl, '_blank', 'location=no,toolbar=no,zoom=no');

        $(authWindow).on('loadstart', function (e) {
            var url = e.originalEvent.url;
            var code = /\?code=(.+)$/.exec(url);
            var error = /\?error=(.+)$/.exec(url);

            if (code || error) {
                authWindow.close();
            }

            if (code) {
                var x = code[1];

                $.post('https://accounts.google.com/o/oauth2/token', {
                    code: code[1],
                    client_id: options.client_id,
                    client_secret: options.client_secret,
                    redirect_uri: options.redirect_uri,
                    grant_type: 'authorization_code'
                }).done(function (data) {
                    deferred.resolve(data);
                }).fail(function (response) {
                    console.log(response.responseJSON);
                });
            } else if (error) {
                //The user denied access to the app
                deferred.reject({
                    error: error[1]
                });
            }
        });

        return deferred.promise();
    }
};

function callGoogle() {
    
    googleapi.authorize({
        client_id: '1080550871240-t2969vh9ojt5vgehsngop71pr10i07vk.apps.googleusercontent.com',
        client_secret: 'tk7Ik8u9EeKwfRc7g71kzrHF',
        redirect_uri: 'http://localhost',
        scope: 'https://www.googleapis.com/auth/plus.login https://www.googleapis.com/auth/userinfo.email'
    }).done(function (data) {
        accessToken = data.access_token;
        localStorage.setItem('ACST', accessToken);
        var res = getDataProfile();
        return res;
    });
}

function getDataProfile() {
    var term = null;
    var token = localStorage.getItem('ACST');

    var accessToken = token;
    var grantUrl = 'https://www.googleapis.com/oauth2/v1/tokeninfo?access_token=' + accessToken;

    $.ajax({
        type: 'GET',
        url: grantUrl,
        async: false,
        contentType: "application/json",
        dataType: 'jsonp',
        success: function (nullResponse) {
            var response = nullResponse;
            CallService('login', "POST", "api/Account/RegisterExternal", { "Provider": "Google", "ExternalAccessToken": accessToken, "userId": response.user_id }, function (res) {
                if (res != null) {
                    localStorage.setItem('USName', res.userName);
                    localStorage.setItem('appToken', res.access_token);
                    localStorage.setItem('loginUsingSocial', true);
                    CallService('login', "POST", "api/User/FindByUserId/" + res.userName, '', function (res) {
                        if (res != null) {
                            localStorage.setItem('userLoggedIn', JSON.stringify(res));
                            if (typeof res.photoUrl != 'undefined' && res.photoUrl != null && res.photoUrl != '' && res.photoUrl != ' ') {
                                localStorage.setItem('usrPhoto', res.photoUrl);
                            }
                            else {
                                localStorage.removeItem('usrPhoto');
                            }

                            mainView.router.loadPage({ pageName: 'intro' });
                        }
                        else {
                            notificationService.alert('خطأ','لا يمكن التحقق من البيانات .', function () { });
                        }
                    });
                }
            });
        },
        error: function (e) {
            //Handle the error
            console.log(e);
        }
    });



    disconnectUser();
}

function disconnectUser() {
    var token = localStorage.getItem('ACST');
    var revokeUrl = 'https://accounts.google.com/o/oauth2/revoke?token=' + token;

    $.ajax({
        type: 'GET',
        url: revokeUrl,
        async: false,
        contentType: "application/json",
        dataType: 'jsonp',
        success: function (nullResponse) {
            accessToken = null;
            //console.log(JSON.stringify(nullResponse));
            //console.log("-----signed out..!!----" + accessToken);
        },
        error: function (e) {
            //Handle the error
            console.log(e);
        }
    });
}