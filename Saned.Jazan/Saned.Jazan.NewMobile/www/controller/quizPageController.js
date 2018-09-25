var quizPageController = (function () {
    var questionId;
    var drawMainSliderAdvertisements = function drawMainSliderAdvertisements(advertiseMentResult) {


        if (advertiseMentResult && advertiseMentResult.length > 0) {
            $('#cultural-competition-swiper').html(' <div class="owl-carousel" id="cultural-competition-carousel"></div>');
            for (var i = 0; i < advertiseMentResult.length; i++) {
                var onClickMainBannerAd = '  mainView.router.loadPage({ pageName: \'advertismentsDetails\', query: { id: ' + advertiseMentResult[i].id + ' } });'
                $('#cultural-competition-carousel').append(' <div class="item"><a onclick="' + onClickMainBannerAd + '"><img src="' + imgHost + advertiseMentResult[i].imageUrl + '"></a></div>');
            }


            $('#cultural-competition-carousel').unbind().owlCarousel({
                rtl: true,
                loop: true,
                autoplay: true,
                autoplayTimeout: 1500,
                items: 1,
                margin: 0,
                nav: false,
            });


        } else {
            document.getElementById('cultural-competition-swiper').innerHTML = "";
        }


    }


    var drawWinners = function drawWinners(winners) {
        var divWinners = document.getElementById('winners');
        divWinners.innerHTML = "";
        for (var i = 0; i < winners.length; i++) {

            var divMinnerImageName = document.createElement('div');
            var imgWinner = document.createElement('img');
            var pName = document.createElement('p');
            divMinnerImageName.setAttribute('class', 'col-33');
            imgWinner.setAttribute('src', imgHost + winners[i].photoUrl);
            imgWinner.setAttribute('onerror', 'replaceURL(this,"inner")');
            pName.innerHTML = winners[i].name;
            divMinnerImageName.appendChild(imgWinner);
            divMinnerImageName.appendChild(pName);
            divWinners.appendChild(divMinnerImageName);
        }
    }
    var goToCultureCompetitionPage = function (page) {

        if (typeof page != 'undefined') {


            $('#add-cultural-competition-answer').parsley().reset();
            $('#textAreaUserAnswer').val("");

            dataContextService.cultureCompetiton.getQuestion(function (question) {
                if (question) {
                    questionId = question.id;
                    $('#txtQuestionTitle').text(question.title);
                    $('#txtQuestionBody').text(question.question);
                    //$('#btnSendUserAnswer').show();
                    $('#add-cultural-competition-answer').show();
                }
                else {
                    $('#txtQuestionTitle').text('');
                    $('#txtQuestionBody').text('');
                    //$('#btnSendUserAnswer').hide();
                    $('#add-cultural-competition-answer').hide();
                    //notificationService.alert('تم', 'خطا في استرجاع سؤال المسابقة الثقافية', null);
                }
            });
            dataContextService.cultureCompetiton.getWinners(function (winners) {
                if (winners && winners.length > 0) {
                    drawWinners(winners);
                    //$('#divQuizNotification').hide();

                }
                else {
                    //$('#divQuizNotification').show();
                    var divWinners = document.getElementById('winners');
                    divWinners.innerHTML = "";
                }
            });
            dataContextService.cultureCompetiton.getActiveSponser(function (sponser) {

                if (sponser && sponser.length > 0) {
                    drawMainSliderAdvertisements(sponser);


                }
                else {

                }
            });
            $('#btnSendUserAnswer').unbind().on('click', function () {
                if (isUserLoggedIn()) {
                    notificationService.alert("دليل جازان", "يجب عليك تسجيل الدخول حتى تتمكن من اضافة الإجابة", function () { })
                } else {
                    var textAreaUserAnswer = $('#textAreaUserAnswer').val();
                    if ($('#add-cultural-competition-answer').parsley().validate()) {

                        dataContextService.cultureCompetiton.sendAnswer(questionId, textAreaUserAnswer, function (answer) {
                            if (answer) {
                                notificationService.alert('دليل جزان', 'تم إضافة الإجابة بنجاح', function () {
                                    $('#textAreaUserAnswer').val("");
                                });
                            }
                        });
                    }
                }

            });
        }

    }

    var service = {
        goToCultureCompetitionPage: goToCultureCompetitionPage
    };

    return service;

})();


