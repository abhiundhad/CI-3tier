
function deletetimesheet(timesheetid) {

    $.ajax({
        url: '/Volunteering/deletetimesheet',
        type: 'POST',
        data: { timesheetid: timesheetid },

        success: function (response) {

            $('.timesheetdiv').html($(response).find('.timesheetdiv').html());


        },
        error: function () {
            alert("could not comment");
        }
    });

}
function deletegoalsheet(timesheetid) {

    $.ajax({
        url: '/Volunteering/deletetimesheet',
        type: 'POST',
        data: { timesheetid: timesheetid },

        success: function (response) {

            $('.goalsheetdiv').html($(response).find('.goalsheetdiv').html());


        },
        error: function () {
            alert("could not comment");
        }
    });

}
function editgoalsheet(timesheetid) {

    $.ajax({
        url: '/Volunteering/editsheet',
        type: 'POST',
        data: { timesheetid: timesheetid },

        success: function (response) {
            console.log(response.timesheet.action);
            var ele = document.getElementById('action1');
            ele.value = response.timesheet.action;
            var mission = document.getElementById('mission1');
            mission.value = response.timesheet.missionId;
            var date = document.getElementById('date2');
            date.value = response.timesheet.dateVolunteered;
            var notes = document.getElementById('notes1');
            notes.value = response.timesheet.notes;
            var timesheetid1 = document.getElementById('timesheetid2');
            timesheetid1.value = response.timesheet.timesheetId;
            /*ele.innerText = response.timesheet.action;*/




        },
        error: function () {
            alert("could not comment");
        }
    });

}
function edittimesheet(timesheetid) {

    $.ajax({
        url: '/Volunteering/editsheet',
        type: 'POST',
        data: { timesheetid: timesheetid },

        success: function (response) {
            console.log(response.timesheet.action);
            var hour = document.getElementById('hour1');
            hour.value = String(response.timesheet.timesheetTime).split(":")[0];
            var minute = document.getElementById('minute1');
            minute.value = String(response.timesheet.timesheetTime).split(":")[1];
            var mission = document.getElementById('mission');
            mission.value = response.timesheet.missionId;
            var date = document.getElementById('date1');
            date.value = response.timesheet.dateVolunteered;
            var notes = document.getElementById('notes');
            notes.value = response.timesheet.notes;
            var timesheetid1 = document.getElementById('timesheetid1');
            timesheetid1.value = response.timesheet.timesheetId;
            /*ele.innerText = response.timesheet.action;*/



        },
        error: function () {
            alert("could not comment");
        }
    });

}