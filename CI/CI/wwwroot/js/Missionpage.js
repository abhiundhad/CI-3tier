





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

function addRating(starId, missionId, Id) {
    $.ajax({
        url: '/Volunteering/Addrating',
        type: 'POST',
        data: { missionId: missionId, Id: Id, rating: starId },
        success: function (result) {

            $('#Ratting').html($(result).find('#Ratting').html());
            $('#avgrat').html($(result).find('#avgrat').html());

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
                favbtn.innerHTML = "Remove From Favourite"

            }
            else {

                var favbtn = document.getElementById("favmissiondiv");
                var heartbtn = document.getElementById("heart");
                heartbtn.style.color = "black";
                favbtn.style.color = "black"
                favbtn.innerHTML = "Add to Favourite"

            }
        }
    });
}
function sendRecom(missionid) {

    var Email = Array.from(document.querySelectorAll('input[name="email"]:checked')).map(e => e.id);
    var sendbtn = document.getElementById("sendbutton");
    sendbtn.innerHTML = "Sending...";
    $.ajax
        ({
            url: '/Volunteering/sendRecom',
            type: 'POST',
            data: { missionid: missionid, Email: Email },

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


function missionapplied(missionid, id) {
    $.ajax
        ({
            url: '/Volunteering/AppliedMission',
            type: 'POST',
            data: { missionid: missionid, id: id },
            success: function (result) {

                $('.Appleddiv').html($(result).find('.Appleddiv').html());
                $('#seatleft').html($(result).find('#seatleft').html());
                $('#recentvolunteer').html($(result).find('#recentvolunteer').html());

                Swal.fire({
                    position: 'top-end',
                    icon: 'success',
                    title: 'Successfully Applied',
                    showConfirmButton: false,
                    timer: 2500
                })
            },
            error: function () {
                alert('Error: Could not recommend mission.');
            }
        });

}

function AddPost(missionid, id) {
    var comttext = document.getElementById("floatingTextarea2").value;
    if (comttext == "")
    {
        alert("Please Write Comment");
    } else
    {


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
}



function recentvol(pg, missionid, id) {


    //var missionid = $("input[name='theme']").val();


    $.ajax({
        url: "/Volunteering/Volunteering",
        type: "POST",
        data: { 'pg': pg, 'missionid': missionid, 'id': id },

        success: function (res) {
            //$("#recentvolunteering").html('');
            //$("#recentvolunteering").html(res);
            $('#recent').html($(res).find('#recent').html());

        },
        error: function () {
            alert("recentvolunteering error");
        }
    })
}
function cmtdelete(cmtid, missionid, id) {
    $.ajax({
        url: "/Volunteering/cmtdelete",
        type: "POST",
        data: { 'cmtid': cmtid, 'missionid': missionid, 'id': id },

        success: function (res) {


            $('.commentdiv').html($(res).find('.commentdiv').html());

        },
        error: function () {
            alert("recentvolunteering error");
        }
    })
}