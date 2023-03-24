/**mission couresel js*/

let slideIndex = 1;
showSlides(slideIndex);

// Next/previous controls
function plusSlides(n) {
    showSlides(slideIndex += n);
}

// Thumbnail image controls
function currentSlide(n) {
    showSlides(slideIndex = n);
}

function showSlides(n) {
    let i;
    let slides = document.getElementsByClassName("mySlides");
    let dots = document.getElementsByClassName("demo");
    if (n > slides.length) { slideIndex = 1 }
    if (n < 1) { slideIndex = slides.length }
    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }
    for (i = 0; i < dots.length; i++) {
        dots[i].className = dots[i].className.replace(" active", "");
    }
    slides[slideIndex - 1].style.display = "block";
    dots[slideIndex - 1].className += " active";
}

function  addRating(starId, missionId, Id) {
    $.ajax({
        url: '/Volunteering/Addrating',
        type: 'POST',
        data: { missionId: missionId, Id: Id, rating: starId },
        success: function (result) {
            if (parseInt(result.ratingExists.rating, 10)) {
                for (i = 1; i <= parseInt(result.ratingExists.rating, 10); i++) {
                    var starbtn = document.getElementById(String(i));

                    starbtn.style.color = "#F88634";
                }
                for (i = parseInt(result.ratingExists.rating, 10) + 1; i <= 5; i++) {
                    var starbtn = document.getElementById(String(i));


                    starbtn.style.color = "black";
                }
            }
            else {
                for (i = 1; i <= parseInt(result.newRating.rating, 10); i++) {
                    var starbtn = document.getElementById(String(i));

                    starbtn.style.backgroundColor = "#F88634";
                }

            }
        },
        error: function () {
            alert("could not like mission");
        }
    });
}
function addtofav1(missionId, Id) {
    $.ajax({
        url: '/Volunteering/Addfav',
        type: 'POST',
        data: { missionId: missionId, Id: Id },
        success: function (result) {

            if (result.favmission == "0") {

                var favbtn = document.getElementById("favmissiondiv");
                var heartbtn = document.getElementById("heart");
                heartbtn.style.color = "#F88634";
                favbtn.style.color = "orange"

            }
            else {

                var favbtn = document.getElementById("favmissiondiv");
                var heartbtn = document.getElementById("heart");
                heartbtn.style.color = "black";
                favbtn.style.color = "black"

            }
        }
    });
}
function sendRecom(missionid, Id) {

    var Email = Array.from(document.querySelectorAll('input[name="email"]:checked')).map(e => e.id);
    $.ajax
        ({
            url: '/Volunteering/sendRecom',
            type: 'POST',
            data: { missionid: missionid, Id: Id, Email: Email },
         
            success: function (result)
            {
                alert("Recomendations sent successfully!");
                const checkboxes = document.querySelectorAll('input[name="email"]:checked');
                checkboxes.forEach((checkbox) => {
                    checkbox.checked = false;
                });
            },
            error: function ()
            {

                // Handle error response from the server, e.g. show an error message to the user
                alert('Error: Could not recommend mission.');
            }
        });

}


function missionapplied(missionid, id)
{
    $.ajax
        ({
            url: '/Volunteering/AppliedMission',
            type: 'POST',
            data: { missionid: missionid, id: id },
            success: function (result)
            {

                alert("Applied successfully");
            },
            error: function () {
                alert('Error: Could not recommend mission.');
            }
        });

}

function AddPost(missionid, id) {
    var comttext = document.getElementById("floatingTextarea2").value;
    $.ajax
        ({
            url: '/Volunteering/addComment',
            type: 'POST',
            data: { missionid: missionid, id: id, comttext: comttext },
            success: function (result) {
                $('.commentdiv').html($(result).find('.commentdiv').html());

             

            },
            error: function () {
                // Handle error response from the server, e.g. show an error message to the user
                alert('Error: Could not recommend mission.');
            }
        });
}