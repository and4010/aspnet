

/* 左邊補0 */
function padLeft(str, len) {
    str = '' + str;
    return str.length >= len ? str : new Array(len - str.length + 1).join("0") + str;
}


$(document).ready(function () {
    //預設值
    $('#select-Status').val("*");
    var calendarEl = document.getElementById('calendar');
    var sy = document.getElementById("select-year");
    var sm = document.getElementById("select-month");
    var sw = document.getElementById("select-warehouse");


    var calendar = new FullCalendar.Calendar(calendarEl, {
        themeSystem: 'bootstrap',
        locale: 'zh-tw',
        timezone: "Asia/Taipei",
        plugins: ['dayGrid', 'moment', 'bootstrap'],
        //header: {
        //    left: 'prevYear,prev,next,nextYear today, myCustomButton',
        //    center: 'title',
        //    right: 'dayGridMonth,dayGridWeek,dayGridDay'
        //},
        header: false,
        buttonText: {
            today: '今天',
            month: '當月',
            week: '當週',
            day: '當日'
        },
        color: '#05ABBD',
        navLinks: false, // can click day/week names to navigate views
        displayEventTime: false,
        eventOrder: 'title'
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


        var status = $('#select-Status').val();
        calendar.addEventSource('/Purchase/GetEvents?id=' + sw.options[sw.selectedIndex].value + "&status=" + status);
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

        var status = $('#select-Status').val();
        calendar.addEventSource('/Purchase/GetEvents?id=' + sw.options[sw.selectedIndex].value + "&status=" + status);
        calendar.gotoDate(date);

        //var date = selectedYear + "-" + "12" + "-01";

        //if (calendar.getEventSources()[0] != null) {
        //    calendar.getEventSources()[0].remove();
        //}
        //$("#select-month").val("12");

        //calendar.addEventSource('/Purchase/GetEvents/' + date);
        //calendar.gotoDate(date);
    });

    $("#btn-refresh").on('click', function () {

        //showPdf(1);
        //winPrintDialogBox();


        var year = sy.options[sy.selectedIndex].value;

        var month = sm.options[sm.selectedIndex].value;

        var warehouse = sw.options[sw.selectedIndex].value;

        var date = year + "-" + month + "-01";

        if (calendar.getEventSources()[0] != null) {
            calendar.getEventSources()[0].remove();
        };
        var status = $('#select-Status').val();
        calendar.addEventSource('/Purchase/GetEvents?id=' + warehouse + "&status=" + status);
        //calendar.events = '/Purchase/GetEvents';
        //calendar.refetchEvents();
        calendar.gotoDate(date);
        //});
    });

    $("#select-warehouse").on('click', function () {

        //showPdf(1);
        //winPrintDialogBox();


        var year = sy.options[sy.selectedIndex].value;

        var month = sm.options[sm.selectedIndex].value;

        var warehouse = sw.options[sw.selectedIndex].value;

        var date = year + "-" + month + "-01";

        if (calendar.getEventSources()[0] != null) {
            calendar.getEventSources()[0].remove();
        };
        var status = $('#select-Status').val();
        calendar.addEventSource('/Purchase/GetEvents?id=' + warehouse + "&status=" + status);
        //calendar.events = '/Purchase/GetEvents';
        //calendar.refetchEvents();
        calendar.gotoDate(date);
        //});
    });

    $("#select-Status").on('change', function () {
        var year = sy.options[sy.selectedIndex].value;

        var month = sm.options[sm.selectedIndex].value;

        var warehouse = sw.options[sw.selectedIndex].value;

        var date = year + "-" + month + "-01";

        if (calendar.getEventSources()[0] != null) {
            calendar.getEventSources()[0].remove();
        };

        var status = $('#select-Status').val();
        calendar.addEventSource('/Purchase/GetEvents?id=' + warehouse + "&status=" + status);
        calendar.gotoDate(date);
    });

    calendarinit(calendar, sy, sm, sw);


    serach();

    $(window).on('pageshow', function () {
        var year = sy.options[sy.selectedIndex].value;

        var month = sm.options[sm.selectedIndex].value;

        var warehouse = sw.options[sw.selectedIndex].value;

        var date = year + "-" + month + "-01";

        if (calendar.getEventSources()[0] != null) {
            calendar.getEventSources()[0].remove();
        };
        var status = $('#select-Status').val();
        calendar.addEventSource('/Purchase/GetEvents?id=' + warehouse + "&status=" + status);
        calendar.gotoDate(date);
    });

})

function calendarinit(calendar, sy, sm, sw) {

    var year = sy.options[sy.selectedIndex].value;

    var month = sm.options[sm.selectedIndex].value;

    var warehouse = sw.options[sw.selectedIndex].value;

    var date = year + "-" + month + "-01";

    if (calendar.getEventSources()[0] != null) {
        calendar.getEventSources()[0].remove();
    };

    var status = $('#select-Status').val();
    calendar.addEventSource('/Purchase/GetEvents?id=' + warehouse + "&status=" + status);
    //calendar.gotoDate(year + "-" + month + "-01");

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

function serach() {
    $('#btnSearch').click(function () {

        Swal.fire({
            title: '請輸入櫃號',
            input: 'text',
            inputAttributes: {
                autocapitalize: 'off'
            },
            showCancelButton: true,
            cancelButtonText: '取消',
            confirmButtonText: '確認',
            showLoaderOnConfirm: true,
            preConfirm: function (text){
                if (text == "") {
                    Swal.showValidationMessage(
                        '櫃號不得空白'
                    )
                }
            },
            allowOutsideClick: false
        }).then(function (result) {
            if (result.value) {
                ShowWait(function () {
                    $.ajax({
                        url: '/Purchase/SearchCabinetNumber',
                        type: 'POST',
                        datatype: 'json',
                        data: { CabinetNumber: result.value },
                        success: function (data) {
                            if (data.Success) {
                                CloseWait();
                                window.location.href = '/Purchase/Detail/' + data.Msg;
                            } else {
                                swal.fire(data.Msg);
                            }
                        },
                        error: function () {
                            swal.fire("查詢櫃號失敗");
                        }
                    });
                });
                
            }
        })

    });

}