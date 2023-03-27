var card = document.getElementsByClassName("missioncard");
var cardimg = document.getElementsByClassName("missionimg");
var carddisplay = document.getElementsByClassName("card");
var theme = document.getElementsByClassName("theme-btn");

if (localStorage.getItem("view") === "list") {
    list();
}
function list() {
    localStorage.setItem("view", "list");
    for (i = 0; i < card.length; i++) {
        card[i].style.width = "100%";
        card[i].style.marginTop = "1%";
        carddisplay[i].style.display = "flex";
        carddisplay[i].style.flexDirection = "row";
        // cardimg[i].style.width = "40%";
        cardimg[i].style.height = "100%";
        theme[i].style.top = "97%";
        theme[i].style.left = "20%";



    }

}
function grid() {
    localStorage.setItem("view", "grid");
    for (i = 0; i < card.length; i++) {


        if (screen.width > 1023) {
            card[i].style.width = "33%";
            carddisplay[i].style.flexDirection = "column";
            cardimg[i].style.width = "100%";
            theme[i].style.top = "95%";
            theme[i].style.left = "35%";
        }
        else {
            card[i].style.width = "50%";
            carddisplay[i].style.flexDirection = "column";
            cardimg[i].style.width = "100%";
            theme[i].style.top = "95%";
            theme[i].style.left = "35%";

        }
        // else if (screen.width<768){

        // }



    }

}


var missionid1 = 0;
function addID(missionid) {
    missionid1 = missionid
}
function addtofav(missionId, Id,callId) {
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

function sendRecom(Id) {

    var Email = Array.from(document.querySelectorAll('input[name="email"]:checked')).map(e => e.id);
    var sendbtn = document.getElementById("sendbutton");
    sendbtn.innerHTML = "Sending...";
    $.ajax
        ({
          
            
       
            url: '/Volunteering/sendRecom',
            type: 'POST',
            data: { missionid: missionid1, Id: Id, Email: Email },
        
            
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
function alertmsg() {
    alert("please login/register First");
}
function pagination(page) {

}