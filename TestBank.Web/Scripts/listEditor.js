$("#addItem").click(function () {
    $.ajax({
        url: this.href,
        cache: false,
        success: function (html) { $("#editorRows").append(html); }
    });
    $('#body').height($('#body').height() + 44);
    return false;
});

$("a.deleteRow").live("click", function () {
    $(this).parents("tr.editorRow:first").remove();
    $('#body').height($('#body').height() - 44);
    return false;
});