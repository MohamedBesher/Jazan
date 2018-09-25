var notificationController = (function () {


    var drawNotificationPage = function (notificationList) {
        var mainWrapper = $('#user-notification-wrapper');
        $.each(notificationList, (function (index) {
            mainWrapper.append('<div class="card" data-related-type="'
                               + notificationList[index].relatedType +
                               '" data-related-id="' + notificationList[index].relatedId + '"><p>'
                               + notificationList[index].arabicMessage + '</p></div>');

            $('[data-related-id="' + notificationList[index].relatedId + '"]').unbind().click(function () {
                var dataRelatedId = $(this).attr('data-related-id');
                var dataRelatedType = $(this).attr('data-related-type');

                if (dataRelatedType == 1) {
                    localStorage.setItem('placeDetails', dataRelatedId);
                    mainView.router.loadPage({ pageName: 'placeDetails', query: { placeDetails: dataRelatedId } });
                } else if (dataRelatedType == 2) {
                    mainView.router.loadPage({ pageName: 'touristDetails', query: { id: dataRelatedId } });
                }
            });

        }));

    };

    var goToNotificationPage = function (page) {

            if (typeof page != 'undefined') {

            var userLoged = JSON.parse(localStorage.getItem('userLoggedIn'));
            if (!userLoged) {
                notificationService.alert('تنبيه', 'قم بتسجيل الدخول اولا',function () { });
                mainView.router.loadPage({ pageName: 'login' });
            }

            $('#user-notification-wrapper').html('');
            myApp.detachInfiniteScroll($$('#user-notification-infinite'));

            localStorageService.setValue(localStorageService.getAllKeys().notificationPageNumber, 0);

            if (initNotification) {
                initNotification = false;
                var notificationInfiniteLoading = false;
                $$('#user-notification-infinite').on('infinite', function () {
                    if (notificationInfiniteLoading) return;
                    notificationInfiniteLoading = true;

                    dataContextService.notifications.getPaged(localStorageService.getValue(localStorageService.getAllKeys().notificationPageNumber),
                        5, userLoged.userId,
                        function (result) {
                            if (result) {
                                notificationInfiniteLoading = false;

                                var newPageNumber = parseInt(localStorageService.getValue(localStorageService.getAllKeys().notificationPageNumber)) + 1;
                                localStorageService.setValue(localStorageService.getAllKeys().notificationPageNumber, newPageNumber);

                                drawNotificationPage(result);

                                if (result && result.length < 5) {
                                    myApp.detachInfiniteScroll($$('#user-notification-infinite'));
                                    return;
                                }
                            }
                        }
                    );
                });
            }

            dataContextService.notifications.getPaged(localStorageService.getValue(localStorageService.getAllKeys().notificationPageNumber),
                       5, userLoged.userId,
                       function (result) {
                           if (result) {


                               var newPageNumber = parseInt(localStorageService.getValue(localStorageService.getAllKeys().notificationPageNumber)) + 1;
                               localStorageService.setValue(localStorageService.getAllKeys().notificationPageNumber, newPageNumber);

                               drawNotificationPage(result);

                               if (result && result.length < 5) {
                                   myApp.detachInfiniteScroll($$('#user-notification-infinite'));
                                   return;
                               } else {
                                   myApp.attachInfiniteScroll($$('#user-notification-infinite'));
                               }
                           }
                       }
                   );


        }

    };

   

    var service = {
        drawNotificationPage: drawNotificationPage,
        goToNotificationPage: goToNotificationPage
    };

    return service;

})();

