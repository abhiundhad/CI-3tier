
//var missionid1 = 0;
//function addID(missionid) {
//    missionid1 = missionid
//}
function addtofav(missionId, Id, callId) {
    console.log(callId)
    $.ajax({
        url: '/Volunteering/Addfav',
        type: 'POST',
        data: { missionId: missionId, Id: Id },
        success: function (result) {
            if (result.favmission == "0") {

                var favbtn = document.getElementById("favmissiondiv");
                var heartbtn = document.getElementById("heart-" + missionId.toString());
                heartbtn.style.color = "#F88634";
                favbtn.style.color = "orange"

            }
            else {

                var favbtn = document.getElementById("favmissiondiv");
                var heartbtn = document.getElementById("heart-" + missionId.toString());
                heartbtn.style.color = "white";
                favbtn.style.color = "white"

            }

        }
    });
}



function sendRecomrel(missionid) {

    var Email = Array.from(document.querySelectorAll('input[name="email"]:checked')).map(e => e.id);

    var sendbtn = document.getElementById("sendbtn");
    sendbtn.innerHTML = "Sending...";
    $.ajax
        ({
            url: '/Volunteering/sendRecom',
            type: 'POST',
            data: { missionid: missionid,  Email: Email },
            success: function (result) {
              
                const checkboxes = document.querySelectorAll('input[name="email"]:checked');
                checkboxes.forEach((checkbox) => {
                    checkbox.checked = false;
                });
                sendbtn.innerHTML = "Send successfully";
                setTimeout(() => {


                    sendbtn.innerHTML = "Send Recommandation";

                }, 2000);
            },
            error: function () {
                // Handle error response from the server, e.g. show an error message to the user
                alert('Error: Could not recommend mission.');
            }
        });

}