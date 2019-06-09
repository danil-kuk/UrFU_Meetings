
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

//Возвращение на ту же вкладку после обновления страницы в разделе Мои Мероприятия

$(document).ready(function () {
    $('a[data-toggle="pill"]').on('show.bs.tab', function (e) {
        localStorage.setItem('activeTab', $(e.target).attr('href'));
    });
    var activeTab = localStorage.getItem('activeTab');
    if (activeTab) {
        $('#myPills a[href="' + activeTab + '"]').tab('show');
    }
});


//Ниже для каждой темы идут скрипты, которые позволяют фильтровать события по темам на доске мероприятий
$(function () {
    $("#showTheme0").click(function () {
        var eventsToShow = document.getElementsByClassName('theme0');
        for (var i = 0, length = eventsToShow.length; i < length; i++) {
            
            if ($(this).is(":checked")) {
                if (eventsToShow[i].className.split(" ")[0] == "skip" && !$("#showOldEvents").is(":checked")) {
                    eventsToShow[i].style.display = 'none';
                }
                else {
                    eventsToShow[i].style.display = '';
                }
            }
            else {
                eventsToShow[i].style.display = 'none';
            }
        }
    });
});

$(function () {
    $("#showTheme1").click(function () {
        var eventsToShow = document.getElementsByClassName('theme1');
        for (var i = 0, length = eventsToShow.length; i < length; i++) {

            if ($(this).is(":checked")) {
                if (eventsToShow[i].className.split(" ")[0] == "skip" && !$("#showOldEvents").is(":checked")) {
                    eventsToShow[i].style.display = 'none';
                }
                else {
                    eventsToShow[i].style.display = '';
                }
            }
            else {
                eventsToShow[i].style.display = 'none';
            }
        }
    });
});

$(function () {
    $("#showTheme2").click(function () {
        var eventsToShow = document.getElementsByClassName('theme2');
        for (var i = 0, length = eventsToShow.length; i < length; i++) {

            if ($(this).is(":checked")) {
                if (eventsToShow[i].className.split(" ")[0] == "skip" && !$("#showOldEvents").is(":checked")) {
                    eventsToShow[i].style.display = 'none';
                }
                else {
                    eventsToShow[i].style.display = '';
                }
            }
            else {
                eventsToShow[i].style.display = 'none';
            }
        }
    });
});

$(function () {
    $("#showTheme3").click(function () {
        var eventsToShow = document.getElementsByClassName('theme3');
        for (var i = 0, length = eventsToShow.length; i < length; i++) {

            if ($(this).is(":checked")) {
                if (eventsToShow[i].className.split(" ")[0] == "skip" && !$("#showOldEvents").is(":checked")) {
                    eventsToShow[i].style.display = 'none';
                }
                else {
                    eventsToShow[i].style.display = '';
                }
            }
            else {
                eventsToShow[i].style.display = 'none';
            }
        }
    });
});

$(function () {
    $("#showTheme4").click(function () {
        var eventsToShow = document.getElementsByClassName('theme4');
        for (var i = 0, length = eventsToShow.length; i < length; i++) {

            if ($(this).is(":checked")) {
                if (eventsToShow[i].className.split(" ")[0] == "skip" && !$("#showOldEvents").is(":checked")) {
                    eventsToShow[i].style.display = 'none';
                }
                else {
                    eventsToShow[i].style.display = '';
                }
            }
            else {
                eventsToShow[i].style.display = 'none';
            }
        }
    });
});

$(function () {
    $("#showTheme5").click(function () {
        var eventsToShow = document.getElementsByClassName('theme5');
        for (var i = 0, length = eventsToShow.length; i < length; i++) {

            if ($(this).is(":checked")) {
                if (eventsToShow[i].className.split(" ")[0] == "skip" && !$("#showOldEvents").is(":checked")) {
                    eventsToShow[i].style.display = 'none';
                }
                else {
                    eventsToShow[i].style.display = '';
                }
            }
            else {
                eventsToShow[i].style.display = 'none';
            }
        }
    });
});

$(function () {
    $("#showTheme6").click(function () {
        var eventsToShow = document.getElementsByClassName('theme6');
        for (var i = 0, length = eventsToShow.length; i < length; i++) {

            if ($(this).is(":checked")) {
                if (eventsToShow[i].className.split(" ")[0] == "skip" && !$("#showOldEvents").is(":checked")) {
                    eventsToShow[i].style.display = 'none';
                }
                else {
                    eventsToShow[i].style.display = '';
                }
            }
            else {
                eventsToShow[i].style.display = 'none';
            }
        }
    });
});

$(function () {
    $("#showTheme7").click(function () {
        var eventsToShow = document.getElementsByClassName('theme7');
        for (var i = 0, length = eventsToShow.length; i < length; i++) {

            if ($(this).is(":checked")) {
                if (eventsToShow[i].className.split(" ")[0] == "skip" && !$("#showOldEvents").is(":checked")) {
                    eventsToShow[i].style.display = 'none';
                }
                else {
                    eventsToShow[i].style.display = '';
                }
            }
            else {
                eventsToShow[i].style.display = 'none';
            }
        }
    });
});

$(function () {
    $("#showTheme8").click(function () {
        var eventsToShow = document.getElementsByClassName('theme8');
        for (var i = 0, length = eventsToShow.length; i < length; i++) {

            if ($(this).is(":checked")) {
                if (eventsToShow[i].className.split(" ")[0] == "skip" && !$("#showOldEvents").is(":checked")) {
                    eventsToShow[i].style.display = 'none';
                }
                else {
                    eventsToShow[i].style.display = '';
                }
            }
            else {
                eventsToShow[i].style.display = 'none';
            }
        }
    });
});

$(function () {
    $("#showTheme9").click(function () {
        var eventsToShow = document.getElementsByClassName('theme9');
        for (var i = 0, length = eventsToShow.length; i < length; i++) {

            if ($(this).is(":checked")) {
                if (eventsToShow[i].className.split(" ")[0] == "skip" && !$("#showOldEvents").is(":checked")) {
                    eventsToShow[i].style.display = 'none';
                }
                else {
                    eventsToShow[i].style.display = '';
                }
            }
            else {
                eventsToShow[i].style.display = 'none';
            }
        }
    });
});