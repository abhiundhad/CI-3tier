
$(document).ready(function () {
    filter();
    
});
function filter( pg,sortValue ) {

    var Search = $("input[name='searchinput']").val();
    //if (Search == '')
    //    Search = '';
    console.log(Search)


    var country = [];

    //$('#countryDropdown').find("input:checked").each(function (i, ob) {
    //    country.push($(ob).val());
    //});
    $("input[type='checkbox'][name='country']:checked").each(function () {
        country.push($(this).val());
    });
    console.log(country)


    var city = [];
    $("input[type='checkbox'][name='city']:checked").each(function () {
        city.push($(this).val());
    });
    console.log(city)

    var theme = [];
    $("input[type='checkbox'][name='theme']:checked").each(function () {
        theme.push($(this).val());
    });
    console.log(theme)

    $.ajax({
        url: "/Landingpage/Filters",
        type: "POST",
        data: { 'search': Search, 'sortValue': sortValue, 'country': country, 'city': city, 'theme': theme, 'pg':pg },

        success: function (res) {
            $("#missions").html('');
            $("#missions").html(res);
           
        },
        error: function () {
            alert("some Error");
        }
    })
}

