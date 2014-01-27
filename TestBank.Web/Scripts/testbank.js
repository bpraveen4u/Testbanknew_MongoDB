

$(document).ready(function () {
    /*if ($('#body').height() < $(document).height())
        $('#body').height($(document).height() - 211);*/
    //alert($('#header').height() + ":" + $('#footer').height());
    $('#listQuestion tr').not(':first').hover(function () {
        $(this).addClass('tr-hover');
    }, function () {
        $(this).removeClass('tr-hover');
    });

    $('#listAssessment tr').not(':first').hover(function () {
        $(this).addClass('tr-hover');
    }, function () {
        $(this).removeClass('tr-hover');
    });

    $('#listResults tr').not(':first').hover(function () {
        $(this).addClass('tr-hover');
    }, function () {
        $(this).removeClass('tr-hover');
    });

    

});


