//$(function () {

//    $('body').on('click', '.list-group .list-group-item', function () {
//        $(this).toggleClass('active');
//    });
//    $('.list-arrows button').click(function () {
//        var $button = $(this), actives = '';
//        if ($button.hasClass('move-left')) {
//            actives = $('.list-right ul li.active');
//            actives.clone().appendTo('.list-left ul');
//            actives.remove();
//        } else if ($button.hasClass('move-right')) {
//            actives = $('.list-left ul li.active');
//            actives.clone().appendTo('.list-right ul');
//            actives.remove();
//        }
//    });
//    $('.dual-list .selector').click(function () {
//        var $checkBox = $(this);
//        if (!$checkBox.hasClass('selected')) {
//            $checkBox.addClass('selected').closest('.well').find('ul li:not(.active)').addClass('active');
//            $checkBox.children('i').removeClass('bi bi-check').addClass('bi bi-check-all');
//            $checkBox.children('i').removeClass('bg-dark').addClass('bg-primary');
//        } else {
//            $checkBox.removeClass('selected').closest('.well').find('ul li.active').removeClass('active');
//            $checkBox.children('i').removeClass('bi bi-check-all').addClass('bi bi-check');
//            $checkBox.children('i').removeClass('bg-primary').addClass('bg-dark');
//        }
//    });
//    $('[name="SearchDualList"]').keyup(function (e) {
//        var code = e.keyCode || e.which;
//        if (code == '9') return;
//        if (code == '27') $(this).val(null);
//        var $rows = $(this).closest('.dual-list').find('.list-group li');
//        var val = $.trim($(this).val()).replace(/ +/g, ' ').toLowerCase();
//        $rows.show().filter(function () {
//            var text = $(this).text().replace(/\s+/g, ' ').toLowerCase();
//            return !~text.indexOf(val);
//        }).hide();
//    });

//});
document.getElementById('imgdiv').addEventListener("click", e => {
    document.getElementById('imginput').click();
});

document.getElementById('imginput').addEventListener("change", e => {
	const reader = new FileReader(); // Create a new FileReader object
	reader.onload = function () {
		document.getElementById('user-profile-img').src = reader.result; // Set the source of the image tag to the selected image
	}
	reader.readAsDataURL(e.target.files[0]); // Read the selected file as a data URL
});

function ved1() {
	var a = document.getElementById("s1");
	var c = document.getElementById("s2");
	var b = a.options[a.selectedIndex];
	for (var i = 0; i < a.length; i++) {

		if (a.options[i].selected == true) {
	
			a.options[i].selected = false
			c.add(a.options[i])

			ved1()
		}

	}
}
function ved2() {
	var a = document.getElementById("s1");
	var c = document.getElementById("s2");
	var b = c.options[c.selectedIndex];
	for (var i = 0; i < c.length; i++) {
		if (c.options[i].selected == true) {
			c.options[i].selected = false
			a.add(c.options[i])
			ved2()
		}
	}
}
function ved3() {
	var a = document.getElementById("s1");
	var c = document.getElementById("s2");
	for (var i = 0; i < a.length;) {
		c.add(a.options[c, i])
	}
}
function ved4() {
	var a = document.getElementById("s1");
	var c = document.getElementById("s2");
	for (var i = 0; i < c.length;) {
		a.add(c.options[a, i])
	}
}
document.getElementById('skillSave').addEventListener("click", e => {
	var selectedSkills = [];
	const skillsSelected = $('#s2 option');

	for (var i = 0; i < skillsSelected.length; i++) {
		selectedSkills.push(skillsSelected[i].value);
	}

	//skillsSelected.forEach(fillskill);

	//function fillskill(skill) {
	//	selectedSkills.push(skill.value);
	//}
	console.log(selectedSkills);
	$.ajax({
		url: '/Home/SaveUserSkills',
		type: 'POST',
		data: { selectedSkills: selectedSkills },

		success: function (response) {

			$('#userskilldiv').html($(response).find('#userskilldiv').html());
			document.getElementById('close').click();

		},
		error: function () {
			alert("could not comment");
		}
	});
});