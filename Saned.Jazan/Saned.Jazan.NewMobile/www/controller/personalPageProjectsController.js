var personalPageProjectsController = (function () {


    var drawPersonalPageProject = function (userProjects) {
        var divpersonalProjects = document.getElementById('personalProjects');
        for (var i = 0; i < userProjects.advertisements.length; i++) {
            var divMainElement = document.createElement('div');
            var linkProjectDetails = document.createElement('a');
            var divProjectPicureName = document.createElement('div');
            var imgProject = document.createElement('img');
            var divProjectName = document.createElement('div');
            var hProjectName = document.createElement('h3');
            var linkDeleteproject = document.createElement('a');
            var labelLinkDelete = document.createElement('label');
            divProjectName.className = "dalelOverlay";
            divMainElement.setAttribute('id', userProjects.advertisements[i].advertisementId)
            divMainElement.setAttribute('class', 'col-50 projectsLength');
            linkProjectDetails.setAttribute('id', 'linkProjectDetails_' + userProjects.advertisements[i].advertisementId);
            divProjectPicureName.className = "card";
            imgProject.setAttribute('src', imgHost + userProjects.advertisements[i].advertisementImageUrl)
            hProjectName.innerHTML = userProjects.advertisements[i].advertisementName;
            linkDeleteproject.setAttribute('id', 'linkProjectDelete_' + userProjects.advertisements[i].advertisementId);
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
            $('#linkProjectDelete_' + userProjects.advertisements[i].advertisementId).on('click', function () {
                var elementDeleted = $(this).attr('id').split('_')[1];
                localStorage.setItem('deleted-project-id', elementDeleted);
                notificationService.confirm('هل انت متأكد من حذف المشروع ؟', function (buttonIndex) {
                    if (buttonIndex == 1) {
                        dataContextService.advertisement.deleteAdvertisement(localStorage.getItem('deleted-project-id'), function (success) {
                            if (success == "1") {
                               
                                notificationService.alert('تم', 'تم الحذف المشروع بنجاح', function () 
                                {
                                    
                                    for (var i = 0; i < userProjects.advertisements.length; i++) {
                                        if (localStorage.getItem('deleted-project-id') == userProjects.advertisements[i].advertisementId) {
                                            $('#' + userProjects.advertisements[i].advertisementId).remove();
                                            return;
                                        }
                                    }
                                    localStorage.removeItem('deleted-project-id');
                                });
                            }
                            else {
                                notificationService.alert('تم', 'لم يتم حذف المشروع ', function () {
                                    localStorage.removeItem('deleted-project-id');
                                });
                            }
                        });
                    }
                });
            }); 
            $('#linkProjectDetails_' + userProjects.advertisements[i].advertisementId).on('click', function () {

                var elementProjectId = $(this).attr('id').split('_')[1];
                localStorage.setItem('userProjectId', elementProjectId);
                mainView.router.loadPage({ pageName: 'advertismentsProfileDetails', query: { elementProjectId: elementProjectId } });
            });
        }
    };
    var goToPersonalPageProject = function (page) {
        if (typeof page != 'undefined') {

            $('#personalProjects').innerHTML = "";
            myApp.detachInfiniteScroll($$('#projectsInfinite'));
            $$('.infinite-scroll-preloader').remove();
            localStorage.setItem('userProfile-projects-page-number', 1);
            //loadSideMenuLinks();
            var loading = false;
            var divpersonalProjects = document.getElementById('personalProjects');
            divpersonalProjects.innerHTML = "";
            var userLoggedIn = JSON.parse(localStorage.getItem('userLoggedIn'));

            dataContextService.advertisement.getPaged(4, 1, userLoggedIn.userId, function (userProjects) {
                drawPersonalPageProject(userProjects);
                if (userProjects && userProjects.advertisements && userProjects.advertisements.length == 4) {
                    myApp.attachInfiniteScroll($$('#projectsInfinite'));
                }

                //if (userProjects != null) {
                //    myApp.attachInfiniteScroll($$('#projectsInfinite'));
                //    drawPersonalPageProject(userProjects);
                //}
                //else {
                //    notificationService.alert('خطا في استرجاع الاعلانات الشخصية', 'تنبيه', function () { });
                //}
            });
            if (initUserProjectsInfinite == true) {
                initUserProjectsInfinite = false;
                $$('#projectsInfinite').on('infinite', function () {
                    if (loading) return;
                    loading = true;
                    var pageNumber = parseInt(localStorage.getItem('userProfile-projects-page-number')) + 1;
                    dataContextService.advertisement.getPaged(4, pageNumber, userLoggedIn.userId, function (userProjects) {
                        localStorage.setItem('userProfile-projects-page-number', parseInt(localStorage.getItem('userProfile-projects-page-number')) + 1);
                        drawPersonalPageProject(userProjects);
                        if (userProjects != null && userProjects.advertisements && userProjects.advertisements.length < 4) {
                            myApp.detachInfiniteScroll($$('#projectsInfinite'));
                        }
                        loading = false;
                    });

                    //if (loading) return;
                    //loading = true;
                    //var pageNumber = parseInt(localStorage.getItem('userProfile-projects-page-number')) + 1;
                    //dataContextService.advertisement.getPaged(8, pageNumber, userLoggedIn.userId, function (userProjects) {
                    //    localStorage.setItem('userProfile-projects-page-number', parseInt(localStorage.getItem('userProfile-projects-page-number')) + 1);
                    //    if (userProjects != null && userProjects <= 8) {
                    //        drawPersonalPageProject(userProjects);
                    //        myApp.detachInfiniteScroll($$('#initUserProjectsInfinite'));
                    //        $$('.infinite-scroll-preloader').remove();
                    //        return;
                    //    }
                    //});
                });
            }
        }
    };
    var service = {
        drawPersonalPage: drawPersonalPageProject,
        goToPersonalPageProject: goToPersonalPageProject
    };

    return service;

})();

