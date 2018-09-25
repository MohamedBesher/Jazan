var personalPageTouristVisitController = (function () {
    var drawPersonalPageTouristVisit = function (userTouristVisits) {
        var divpersonalProjects = document.getElementById('usersVisits');
        for (var i = 0; i < userTouristVisits.touristVisits.length; i++) {
            var divMainElement = document.createElement('div');
            var linkProjectDetails = document.createElement('a');
            var divProjectPicureName = document.createElement('div');
            var imgProject = document.createElement('img');
            var divProjectName = document.createElement('div');
            var hProjectName = document.createElement('h3');
            var linkDeleteproject = document.createElement('a');
            var labelLinkDelete = document.createElement('label');
            divProjectName.className = "dalelOverlay";
            divMainElement.setAttribute('id', userTouristVisits.touristVisits[i].id)
            divMainElement.setAttribute('class', 'col-50 projectsLength');
            linkProjectDetails.setAttribute('id', 'linkProjectDetails_' + userTouristVisits.touristVisits[i].id);
            divProjectPicureName.className = "card";
            imgProject.setAttribute('src', imgHost + userTouristVisits.touristVisits[i].imageUrl)
            hProjectName.innerHTML = userTouristVisits.touristVisits[i].name;
            linkDeleteproject.setAttribute('id', 'linkProjectDelete_' + userTouristVisits.touristVisits[i].id);
            linkDeleteproject.setAttribute('class', 'button button-round red');
            labelLinkDelete.innerHTML = "حذف";
            linkDeleteproject.appendChild(labelLinkDelete)
            divProjectName.appendChild(hProjectName);
            divMainElement.appendChild(linkProjectDetails);
            divMainElement.appendChild(linkDeleteproject);
            divProjectPicureName.appendChild(imgProject);
            divProjectPicureName.appendChild(divProjectName);
            linkProjectDetails.appendChild(divProjectPicureName);
            divMainElement.appendChild(linkProjectDetails);
            divMainElement.appendChild(linkDeleteproject);
            divpersonalProjects.appendChild(divMainElement);
            $('#linkProjectDelete_' + userTouristVisits.touristVisits[i].id).on('click', function () {
                var elementDeleted = $(this).attr('id').split('_')[1];
                localStorage.setItem('deleted-tourist-visit-id', elementDeleted);
                notificationService.confirm('هل انت متأكد من حذف الزيارة السياحية ؟', function (buttonIndex) {
                    if (buttonIndex == 1) {
                        dataContextService.touristVisits.deleteTourist(localStorage.getItem('deleted-tourist-visit-id'), function (success) {
                            if (success) {

                                notificationService.alert('تم', 'تم حذف الزيارة السياحية بنجاح', function () {
                                  
                                    for (var i = 0; i < userTouristVisits.touristVisits.length; i++) {
                                        if (localStorage.getItem('deleted-tourist-visit-id') == userTouristVisits.touristVisits[i].id) {
                                            $('#' + userTouristVisits.touristVisits[i].id).remove();
                                            return;
                                        }
                                    }
                                    localStorage.removeItem('deleted-tourist-visit-id');
                                });
                            }
                            else {
                                notificationService.alert('تم', 'لم يتم الحذف الزيارة السياحية', function () { localStorage.removeItem('deleted-tourist-visit-id'); });
                            }
                        });
                    }


                });
            });
            $('#linkProjectDetails_' + userTouristVisits.touristVisits[i].id).on('click', function () {
                var elementTouristId = $(this).attr('id').split('_')[1];
                localStorage.setItem('userTouristId', elementTouristId);
                mainView.router.loadPage({ pageName: 'touristVisitProfileDetails', query: { elementTouristId: elementTouristId } });
            });
        }
    };
    var goToPersonalPageTouristVisit = function (page) {
        if (typeof page != 'undefined') {
            $('#usersVisits').html('');
            myApp.detachInfiniteScroll($$('#visitsInfinte'));
            localStorage.setItem('userProfile-touristVisits-page-number', 1);
            //loadSideMenuLinks();
            var loading = false;
            var divpersonalProjects = document.getElementById('personalProjects');
            divpersonalProjects.innerHTML = "";
            var userLoggedIn = JSON.parse(localStorage.getItem('userLoggedIn'));
            //var JSON.parse(userLoggedIn)
            dataContextService.touristVisits.getPaged(4, 1, userLoggedIn.userId,null, function (userTouristVisits) {
                drawPersonalPageTouristVisit(userTouristVisits);
                if (userTouristVisits && userTouristVisits.touristVisits && userTouristVisits.touristVisits.length == 4) {
                    myApp.attachInfiniteScroll($$('#visitsInfinte'));
                }
            });
            if (initUserVisitsInfinite == true) {
                initUserVisitsInfinite = false;
                $$('#visitsInfinte').on('infinite', function () {
                    if (loading) return;
                    loading = true;
                    var pageNumber = parseInt(localStorage.getItem('userProfile-touristVisits-page-number')) + 1;
                    dataContextService.touristVisits.getPaged(4, pageNumber, userLoggedIn.userId,null, function (userTouristVisits) {
                        localStorage.setItem('userProfile-touristVisits-page-number', parseInt(localStorage.getItem('userProfile-touristVisits-page-number')) + 1);
                        drawPersonalPageTouristVisit(userTouristVisits);
                        if (userTouristVisits != null && userTouristVisits.touristVisits && userTouristVisits.touristVisits.length < 4) {
                            myApp.detachInfiniteScroll($$('#visitsInfinte'));
                        }
                        loading = false;
                    });
                });
            }
        }
    };
    var service = {
        drawPersonalPageTouristVisit: drawPersonalPageTouristVisit,
        goToPersonalPageTouristVisit: goToPersonalPageTouristVisit
    };

    return service;

})();

