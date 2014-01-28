
var questionsList = null;
$(document).ready(function () {
    
    //Assessment javascript
    $('#selCategory').empty();
    //alert($('#selCategory'));
    var category = ["c#", "c"];
    $.each(category, function (val, text) {
        //alert(val);
        $('#selCategory').append($('<option/>').val(text).html(text))
    });

    $('#selCategory').change(function () {
        getQuestionsByCat(this.value);
        $('#allQuestions').empty();
        $.each(questionsList, function (val, question) {
            $('#allQuestions').append($('<option/>').val(question.id).html(question.description))
        });
    });
    $("#selCategory").trigger("change");

    $('#btnSubmit').unbind("click").bind("click", function () {
        submit();
    });

    $('#btnAdd').unbind("click").bind("click", function () {
        $.each($("#allQuestions option:selected"), function (val, opt) {
            var result = $.grep($("#selectedQuestions option"), function (e) { return e.value == opt.value; });
            if (result.length == 1) {
                $(this).remove();
            }
            else {
                $(opt).appendTo("#selectedQuestions");
            }
        });
    });

    $('#btnAddAll').unbind("click").bind("click", function () {
        $.each($("#allQuestions option"), function (val, opt) {
            var result = $.grep($("#selectedQuestions option"), function (e) { return e.value == opt.value; });
            if (result.length == 1) {
                $(this).remove();
            }
            else {
                $(opt).appendTo("#selectedQuestions");
            }
        });
    });

    $('#btnRemove').unbind("click").bind("click", function () {
        $.each($("#selectedQuestions option:selected"), function (val, opt) {
            var result = $.grep(questionsList, function (e) { return e.id == opt.value; });
            if (result.length == 1) {
                $(this).remove();
                $("#allQuestions option[value='" + opt.value + "']").remove();
                $(opt).appendTo("#allQuestions");
            }
            else {
                $(this).remove();
            }
        });
    });

    $('#btnRemoveAll').unbind("click").bind("click", function () {
        $.each($("#selectedQuestions option"), function (val, opt) {
            var result = $.grep(questionsList, function (e) { return e.id == opt.value; });
            if (result.length == 1) {
                $(this).remove();
                $("#allQuestions option[value='" + opt.value + "']").remove();
                $(opt).appendTo("#allQuestions");
            }
            else {
                $(this).remove();
            }
        });
    });

});

function submit() {
    $.each($("#selectedQuestions option"), function (val, opt) {
        opt.selected = true;
    });
}

function getQuestionsByCat(cat) {
    $.ajax({
        async: false,
        type: "GET",
        url: AppPath + "/Questions/GetByCategory",
        dataType: "json",
        data: {category: cat},
        cache: false,
        success: function (objOutput) {
            questionsList = objOutput;
            return true;
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //alert(errorThrown);
        }
    });
}

