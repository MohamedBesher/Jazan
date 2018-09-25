var newsController = (function () {

    var drawNewsPage = function (data) {
        var mainWrapper = $('#news-wrapper');
        if (data && data.length > 0) {

            $.each(data, (function (i) {
                mainWrapper.append('<div class="col-100"><a data-news-id="' + data[i].id + '" href="#newsDetails"><div class="card"><p>' + data[i].title + '</p></div></a></div>');

                $('[data-news-id="' + data[i].id + '"]').unbind().click(function () {
                    var dataNewsId = $(this).attr('data-news-id');

                    mainView.router.loadPage({ pageName: 'newsDetails', query: { id: dataNewsId } });

                });

            }));
        } else {
            //mainWrapper.append('<div class="divAlert"><p class="pNotification"> لا يوجد اخبار </p></div>');
        }
    };

    var drawNewsDetailsPage = function (data) {
        var newsPublishingDate;

        if (data.publishingDate) {
            var month = new Date(data.publishingDate).getUTCMonth() + 1;
            var day = new Date(data.publishingDate).getUTCDate();
            var year = new Date(data.publishingDate).getUTCFullYear();

            newsPublishingDate = (day + '/' + month + '/' + year);
        }

        var mainWrapper = $('#news-warpper');
        var html = '<div class="row"><div class="col-100"><h3 class="">' + data.title + '</h3> </div><div class="col-100">'
                                   + '<h3><i class="ionicons ion-android-calendar"></i> ' + newsPublishingDate + '</h3> </div>'
                                   + '<div class="col-100">'
                                   + '<p>' + data.details + '</p>';

        if (data.imagePath) {
            html = html + '</div><div class="col-100"><a href="#" class="pb-popup-dark3"><img src="' + adminPanelURL + data.imagePath + '"    onerror="replaceURL(this,\'inner\')" ></a></div></div>';
        }

        mainWrapper.html(html);

    };

    var goToNewsPage = function (page) {
        if (typeof page != 'undefined') {
            $('#news-wrapper').html('');
            myApp.detachInfiniteScroll($$('#news-infinite'));

            localStorageService.setValue(localStorageService.getAllKeys().newsPageNumber, 1);

            if (initNewsInfinite == true) {
                initNewsInfinite = false;
                var newsInfiniteLoading = false;
                $$('#news-infinite').on('infinite', function () {
                    if (newsInfiniteLoading) return;
                    newsInfiniteLoading = true;

                    dataContextService.news.getPaged(localStorageService.getValue(localStorageService.getAllKeys().newsPageNumber),
                        5,
                        function (result) {
                            if (result) {
                                newsInfiniteLoading = false;

                                var newPageNumber = parseInt(localStorageService.getValue(localStorageService.getAllKeys().newsPageNumber)) + 1;
                                localStorageService.setValue(localStorageService.getAllKeys().newsPageNumber, newPageNumber);

                                drawNewsPage(result);

                                if (result && result.length < 5) {
                                    myApp.detachInfiniteScroll($$('#news-infinite'));
                                    return;
                                }
                            }
                        }
                    );
                });
            }

            dataContextService.news.getPaged(localStorageService.getValue(localStorageService.getAllKeys().newsPageNumber),
                    5,
                    function (result) {
                        if (result) {
                            var newPageNumber = parseInt(localStorageService.getValue(localStorageService.getAllKeys().newsPageNumber)) + 1;
                            localStorageService.setValue(localStorageService.getAllKeys().newsPageNumber, newPageNumber);
                            drawNewsPage(result);
                            if (result && result.length < 5) {
                                myApp.detachInfiniteScroll($$('#news-infinite'));
                                return;
                            } else {
                                myApp.attachInfiniteScroll($$('#news-infinite'));
                            }
                        }
                    }
                );

        }
    };

    var goToNewsDetailsPage = function (page) {
        if (typeof page != 'undefined') {
            var newsId = page.query.id;

            dataContextService.news.getById(newsId, function (data) {
                drawNewsDetailsPage(data);
            });

        }
    };

    var service = {
        goToNewsDetailsPage: goToNewsDetailsPage,
        goToNewsPage: goToNewsPage
    };

    return service;

})();

