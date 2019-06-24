// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
(function () {
    function customizeMessagePage() {
        const enteredOnElement = document.querySelector('#EnteredOn');
        if (enteredOnElement) {
            enteredOnElement.addEventListener('focus', event => {
                if (!event.target.value) {
                    const currentDate = new Date();
                    const year = currentDate.getFullYear();
                    let month = currentDate.getMonth() + 1;
                    if (month.toString().length == 1) {
                        month = '0' + month;
                    }
                    let day = currentDate.getDate() + 1;
                    if (day.toString().length == 1) {
                        day = '0' + day;
                    }
                    let hour = currentDate.getHours();
                    if (hour.toString().length == 1) {
                        hour = '0' + hour;
                    }
                    let minute = currentDate.getMinutes();
                    if (minute.toString().length == 1) {
                        minute = '0' + minute;
                    }
                    event.target.value = year + '-' +
                        month + '-' + day + ' ' +
                        hour + ':' + minute;
                }
            });
        }
    }

    if (location.href.toLowerCase().indexOf("message") >= 0) {
        customizeMessagePage();
    }
}());
