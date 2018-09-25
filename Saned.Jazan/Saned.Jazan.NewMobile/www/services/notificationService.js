var notificationService = (function () {

    var alert = function (title, message, callback) {
        //notificationService.alert(message, title, callback);

        navigator.notification.alert(message, callback, 'دليل جازان', 'تم');
    }

    var confirm = function (message, callback) {
        navigator.notification.confirm(message, callback, 'دليل جازان', ['حذف', 'الغاء']);
    }

    var service = {
        alert: alert,
        confirm: confirm
    };
    return service;

})();

