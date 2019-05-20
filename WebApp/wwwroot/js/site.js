// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//Автоматическое переключение обложки мероприятия при изменении его темы
$(function () {
    $("#DropdownList").change(function () {
        var theme = $(this);
        var iconId = theme.val();
        $("#eventIcon").attr("src", "/img/eventIcons/eventIcon" + iconId + ".svg");
    })
});

//Скрывать старые мероприятия на доске
$(function () {
    $("#showOldEvents").click(function () {
        var eventsToSkip = document.getElementsByClassName('skip');
        for (var i = 0, length = eventsToSkip.length; i < length; i++) {
        if ($(this).is(":checked")) {
            eventsToSkip[i].style.display = '';
        }
        else {
            eventsToSkip[i].style.display = 'none';
            }
        }
    });
});
