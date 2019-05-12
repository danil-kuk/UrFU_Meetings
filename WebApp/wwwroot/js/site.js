// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//Автоматическое переключение обложки мероприятия при изменении его темы
$(function () {
    $("#DropdownList").change(function () {
        var theme = $(this);
        var coverId = theme.val();
        $("#eventCover").attr("src", "/img/cover" + coverId + ".svg");
    })
});