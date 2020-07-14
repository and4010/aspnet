
/* 左邊補0 */
function padLeft(str, len) {
    str = '' + str;
    return str.length >= len ? str : new Array(len - str.length + 1).join("0") + str;
}



$(document).ready(function () {

    var calendarEl = document.getElementById('calendar');
    var sy = document.getElementById("select-year");
    var sm = document.getElementById("select-month");
    var sw = document.getElementById("select-warehouse");


    var calendar = new FullCalendar.Calendar(calendarEl, {
        locale: 'zh-tw',
        timezone: "Asia/Taipei",
        plugins: ['interaction', 'dayGrid'],
        header: {
            left: 'prevYear,prev,next,nextYear today, myCustomButton',
            center: 'title',
            right: 'dayGridMonth,dayGridWeek,dayGridDay'
        },
        header: false,
        buttonText: {
            today: '今天',
            month: '當月',
            week: '當週',
            day: '當日'
        },
        color: '#05ABBD',
        //defaultDate: '2019-08-12',
        navLinks: false, // can click day/week names to navigate views
      //  editable: true,
        //eventLimit: true, // allow "more" link when too many events
        //events: '/Purchase/GetEvents',
        //events: {
        //    url: '/Purchase/GetEvents',
        //    method: 'POST',
        //    extraParams: {
        //        custom_param1: 'something',
        //        custom_param2: 'somethingelse'
        //    },
        //    failure: function() {
        //        alert('there was an error while fetching events!');
        //    },
        //    color: 'yellow',   // a non-ajax option
        //    textColor: 'black' // a non-ajax option
        //},
        customButtons: {
            myCustomButton: {
                text: 'custom!',
                click: function () {
                    calendar.gotoDate("0099-08-08");
                    //alert('clicked the custom button!');
                }
            }
        },
    });

    calendar.render();
    calendar.setOption('height', 750);
    $(".fc-right").append('<select class="select_month"><option value="">選擇月份</option><option value="01">01</option><option value="02">02</option><option value="03">03</option><option value="04">04</option><option value="05">05</option><option value="06">06</option><option value="07">07</option><option value="08">08</option><option value="09">09</option><option value="10">10</option><option value="11">11</option><option value="12">12</option></select>');

    $("#select-month").on("change", function (event) {
        var year = sy.options[sy.selectedIndex].value;
        var month = sm.options[sm.selectedIndex].value;


        var date = year + "-" + month + "-01";

        if (calendar.getEventSources()[0] != null) {
            calendar.getEventSources()[0].remove();
        };


        calendar.addEventSource('/Purchase/GetEvents/' + date);
        calendar.gotoDate(date);


    });


    $("#select-year").on("change", function (event) {
        var selectedYear = sy.options[sy.selectedIndex].value;
        //var month = sm.options[sm.selectedIndex].value;
        var date = null;

        var nowDate = new Date();
        var nowYear = nowDate.getFullYear().toString();
        var nowMonth = padLeft((nowDate.getMonth() + 1).toString(), 2);

        if (selectedYear == nowYear) {
            $("#select-month").val(nowMonth);
            date = selectedYear + "-" + nowMonth + "-01";
        } else {
            $("#select-month").val("12");
            date = selectedYear + "-" + "12" + "-01";
        }

        if (calendar.getEventSources()[0] != null) {
            calendar.getEventSources()[0].remove();
        }

        calendar.addEventSource('/Purchase/GetEvents/' + date);
        calendar.gotoDate(date);

        //var date = selectedYear + "-" + "12" + "-01";

        //if (calendar.getEventSources()[0] != null) {
        //    calendar.getEventSources()[0].remove();
        //}
        //$("#select-month").val("12");

        //calendar.addEventSource('/Purchase/GetEvents/' + date);
        //calendar.gotoDate(date);
    });

    $(".btn-group").on('click', '#btn-refresh', function () {

        //showPdf(1);
        //winPrintDialogBox();


        var year = sy.options[sy.selectedIndex].value;

        var month = sm.options[sm.selectedIndex].value;

        var warehouse = sw.options[sw.selectedIndex].value;

        var date = year + "-" + month + "-01";

        if (calendar.getEventSources()[0] != null) {
            calendar.getEventSources()[0].remove();
        };
        calendar.addEventSource('/Purchase/GetEvents/' + date);
        //calendar.events = '/Purchase/GetEvents';
        //calendar.refetchEvents();
        calendar.gotoDate(date);
        //});
    });



    calendarinit(calendar, sy, sm, sw);


})

function calendarinit(calendar, sy, sm, sw) {

    var year = sy.options[sy.selectedIndex].value;

    var month = sm.options[sm.selectedIndex].value;

    var warehouse = sw.options[sw.selectedIndex].value;

    var date = year + "-" + month + "-01";

    if (calendar.getEventSources()[0] != null) {
        calendar.getEventSources()[0].remove();
    };

    calendar.addEventSource('/Purchase/GetEvents/' + date);
    calendar.gotoDate(year + "-" + month + "-01");

    //var date = calendar.getDate()
    //var year = calendar.formatDate(date, { year: 'numeric' }).substring(0,4);
    //var month = calendar.formatDate(date, { month: 'numeric' }).substring(0,2);
    //var day = calendar.formatDate(date, { day: 'numeric' }).substring(0,2);

    //setSelectedValue(sy, year);
    //setSelectedValue(sm, month);



}


function setSelectedValue(selectObj, valueToSet) {
    for (var i = 0; i < selectObj.options.length; i++) {
        if (selectObj.options[i].text == valueToSet) {
            selectObj.options[i].selected = true;
            return;
        };
    };
}

