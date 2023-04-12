var dashboadtime = document.getElementById("dashboadtime");
function currentTime() {
    var time = new Date();
    var dateString = time.toDateString();
    var timeString = time.toLocaleTimeString();
    dashboadtime.innerHTML = dateString + ' ' + timeString;
    let t = setTimeout(function () { currentTime() }, 1000);
}
currentTime();
$(document).ready(function () {
    $('#example').DataTable();
});