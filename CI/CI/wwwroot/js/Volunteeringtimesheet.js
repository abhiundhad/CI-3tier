function deletedata(timesheetid)
{
    $.ajax({
        url: "/Volunteering/deletedata",
        type: "POST",
        data: { 'timesheetid': timesheetid },
        success: function (res) {
            $('#detail1').html($(res).find('#detail1').html());
            $('#detail2').html($(res).find('#detail2').html());
        },
        error: function () {
            alert("recentvolunteering error");
        }

    });

}