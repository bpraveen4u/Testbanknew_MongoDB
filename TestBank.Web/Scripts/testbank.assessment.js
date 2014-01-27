
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

    $('#btnAdd').unbind("click").bind("click", function () {
        $("#allQuestions option:selected").appendTo("#selectedQuestions");
    });

    $('#btnAddAll').unbind("click").bind("click", function () {
        $("#allQuestions option").appendTo("#selectedQuestions");
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
        //$("#selectedQuestions option").appendTo("#allQuestions");
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

    //var t = @Html.Raw(Json.Encode(Model));
    //var myModel = @{@Html.Raw(Json.Encode(Model));}

});

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

