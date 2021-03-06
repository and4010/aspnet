var selected = [];
var editor;
var deliveryAuthorizeEditor;

$(document).ready(function () {

    //航程號下拉選單自動完成
    $("#ddlTrip").combobox();

    //jQuery ui datepicker初始化
    $.datepicker.setDefaults($.datepicker.regional["zh-TW"]);
    $('#txtTripActualShipBeginDate').datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true
    });
    $('#txtTripActualShipEndDate').datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true
    });
    $('#txtTransactionDate').datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true
    });

    editor = new $.fn.dataTable.Editor({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        ajax: {
            url: '/Delivery/UpdateTransactionAuthorizeDates',
            "type": "POST",
            "dataType": "json",
            contentType: 'application/json',
            "data": function (d) {
                var tripDetailDTData = d.data;
                var TripDetailDTList = [];
                var size = Object.keys(tripDetailDTData).length;
                for (var i = 0; i < size; i++) {
                    var TRIP_ID = Object.keys(tripDetailDTData)[i];
                    var AUTHORIZE_DATE = Object.keys(tripDetailDTData[TRIP_ID]).map(function (e) {
                        return tripDetailDTData[TRIP_ID][e]
                    })[0];

                    var TRIP_ID_REPEAT = false;

                    for (var j = 0; j < TripDetailDTList.length; j++) {
                        if (TripDetailDTList[j] == TRIP_ID) {
                            TRIP_ID_REPEAT = true;
                            break;
                        }
                    }

                    if (TRIP_ID_REPEAT == false) {
                        var TripDetailDT = {
                            'TRIP_ID': TRIP_ID,
                            'AUTHORIZE_DATE': AUTHORIZE_DATE
                        }
                        TripDetailDTList.push(TripDetailDT);
                    }
                }
                var data = {
                    'action': d.action,
                    'TripDetailDTList': TripDetailDTList
                }
                return JSON.stringify(data);
            },
            success: function (data) {
                if (data.status) {
                    TripDataTablesBody.ajax.reload(null, false);
                }
                else {
                    swal.fire(data.result);
                }
            },
            error: function () {
                swal.fire('更新出貨核准日失敗');
            },
            complete: function (data) {


            }
        },
       
        table: "#TripDataTablesBody",
        idSrc: 'TRIP_ID',
        fields: [
            {
                label: "出貨核准日:",
                name: "AUTHORIZE_DATE",
                type: 'date',
                dateFormat: 'yy-mm-dd',
                opts: {
                    minDate: GetTransactionAuthorizeMinDate(),
                    maxDate: GetTransactionAuthorizeMaxDate(),
                    changeMonth: true,
                    changeYear: true
                }


            }
        ],
        i18n: {
            create: {
                button: "新增",
                title: "新增出貨核准日",
                submit: "確定"
            },
            edit: {
                button: "編輯核准日",
                title: "編輯出貨核准日",
                submit: "確定",
                'className': 'btn-danger'
            },
            remove: {
                button: "刪除",
                title: "確定要刪除??",
                submit: "確定",
                confirm: {
                    _: "確定要刪除 %d ?",
                }
            },
            multi: {
                "title": "多欄位異動",
                "info": "請注意，您一次選擇多個不同的出貨核准日，此次異動將會變成同樣的出貨核准日！",
                "restore": "取消更改",
                "noMulti": "This input can be edited individually, but not part of a group."
            },
        }


    });


    deliveryAuthorizeEditor = new $.fn.dataTable.Editor({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        ajax: {
            url: '/Delivery/DeliveryAuthorize',
            "type": "POST",
            "dataType": "json",
            contentType: 'application/json',
            "data": function (d) {
                var tripDetailDTData = d.data;
                var TripDetailDTList = [];
                var size = Object.keys(tripDetailDTData).length;
                for (var i = 0; i < size; i++) {
                    var TRIP_ID = Object.keys(tripDetailDTData)[i];
                    var AUTHORIZE_DATE = Object.keys(tripDetailDTData[TRIP_ID]).map(function (e) {
                        return tripDetailDTData[TRIP_ID][e]
                    })[0];

                    var TRIP_ID_REPEAT = false;

                    for (var j = 0; j < TripDetailDTList.length; j++) {
                        if (TripDetailDTList[j] == TRIP_ID) {
                            TRIP_ID_REPEAT = true;
                            break;
                        }
                    }

                    if (TRIP_ID_REPEAT == false) {
                        var TripDetailDT = {
                            'TRIP_ID': TRIP_ID,
                            'AUTHORIZE_DATE': AUTHORIZE_DATE
                        }
                        TripDetailDTList.push(TripDetailDT);
                    }
                }
                var data = {
                    'action': d.action,
                    'TripDetailDTList': TripDetailDTList
                }
                return JSON.stringify(data);
            },
            success: function (data) {
                if (data.status) {
                    TripDataTablesBody.ajax.reload(null, false);
                    swal.fire(data.result);
                }
                else {
                    swal.fire(data.result);
                }
            },
            error: function () {
                swal.fire('出貨核准失敗');
            },
            complete: function (data) {


            }
        },

        table: "#TripDataTablesBody",
        idSrc: 'TRIP_ID',
        fields: [
            {
                label: "出貨核准日:",
                name: "AUTHORIZE_DATE",
                type: 'date',
                dateFormat: 'yy-mm-dd',
                opts: {
                    minDate: GetTransactionAuthorizeMinDate(),
                    maxDate: GetTransactionAuthorizeMaxDate(),
                    changeMonth: true,
                    changeYear: true
                }


            }
        ],
        i18n: {
            create: {
                button: "新增",
                title: "新增出貨核准日",
                submit: "確定"
            },
            edit: {
                button: "編輯核准日",
                title: "編輯出貨核准日",
                submit: "確定",
                'className': 'btn-danger'
            },
            remove: {
                button: "刪除",
                title: "確定要刪除??",
                submit: "確定",
                confirm: {
                    _: "確定要刪除 %d ?",
                }
            },
            multi: {
                "title": "多欄位異動",
                "info": "請注意，您一次選擇多個不同的出貨核准日，此次異動將會變成同樣的出貨核准日！",
                "restore": "取消更改",
                "noMulti": "This input can be edited individually, but not part of a group."
            },
        }


    });

    var TripDataTablesBody = $('#TripDataTablesBody').DataTable({
        language: {
            url: '/bower_components/datatables/language/zh-TW.json'
        },
        autoWidth: false,
        serverSide: true,
        processing: true,
        deferLoading: 0, //初始化DataTable時，不發出ajax
        lengthMenu: [[25, 50, 100, 200], [25, 50, 100, 200]],
        dom:
            "<'row'<'col-sm-2 width-s'l><'col-sm-7'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Delivery/DeliverySearch",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.TripActualShipBeginDate = $("#txtTripActualShipBeginDate").val();
                d.TripActualShipEndDate = $("#txtTripActualShipEndDate").val();
                d.DeliveryName = $("#txtDeliveryName").val();
                d.SelectedSubinventory = $("#ddlWarehouse option:selected").text();
                d.SelectedTrip = $("#ddlTrip").val();
                d.TransactionDate = $("#txtTransactionDate").val();
                d.SelectedDeliveryStatus = $("#ddlDeliveryStatus").val();
            },
        },
        columns: [
            { data: null, defaultContent: '', className: 'select-checkbox', orderable: false, width: "40px" },
            {
                data: null, "autoWidth": true, orderable: false,
                render: function (data, type, row, meta) {
                    if (data.DELIVERY_STATUS == null || data.DELIVERY_STATUS == "已取消") {
                        return null
                    }
                    if (data.DELIVERY_STATUS == "未印") {
                        return '<button class="btn btn-primary btn-sm btn-print"><i class="glyphicon glyphicon-print"></i>備貨單</button>'
                    }
                    if (data.DELIVERY_STATUS == "未印" || data.DELIVERY_STATUS == "待出") {
                        return '<button class="btn btn-danger btn-sm btn-edit"><i class="fa fa-pencil"></i>出貨</button>' + '&nbsp|&nbsp' + '<button class="btn btn-primary btn-sm btn-print"><i class="glyphicon glyphicon-print"></i>備貨單</button>'
                    }
                    if (data.DELIVERY_STATUS == "已揀") {
                        return '<button class="btn btn-danger btn-sm btn-edit"><i class="fa fa-pencil"></i>出貨</button>'
                    }
                    if (data.DELIVERY_STATUS == "待核准" || data.DELIVERY_STATUS == "已出貨") {
                        return '<button class="btn btn-primary btn-sm btn-view"><i class="glyphicon glyphicon-eye-open"></i>紀錄</button>'
                    }
                }
            },
            { data: "SUB_ID", name: "項次", "autoWidth": true },
            { data: "FREIGHT_TERMS_NAME", name: "內銷地區別", "autoWidth": true },
            { data: "TRIP_CAR", name: "車次", "autoWidth": true },
            { data: "TRIP_NAME", name: "航程號", "autoWidth": true },
            { data: "DELIVERY_NAME", name: "交運單", "autoWidth": true },
            { data: "DetailType", name: "作業別", "autoWidth": true },
            { data: "DELIVERY_STATUS", name: "狀態", "autoWidth": true },
            { data: "CUSTOMER_NAME", name: "客戶", "autoWidth": true },
            { data: "SUBINVENTORY_CODE", name: "出貨倉庫", "autoWidth": true },
            { data: "RP_SUM", name: "主單位需求量總和", "autoWidth": true },
            { data: "REQUESTED_PRIMARY_UOM", name: "主單位", "autoWidth": true },
            {
                data: "RS_SUM", name: "次單位需求量總和", "autoWidth": true, "mRender": function (data, type, full) {
                    if (data != null) {
                        if (data == 0) {
                            return '';
                        }
                        return data;
                    } else {
                        return '';
                    }
                }},
            { data: "REQUESTED_SECONDARY_UOM", name: "次單位", "autoWidth": true },
            { data: "NOTE", name: "備註", "autoWidth": true, className: "dt-body-left" },
            { data: "SHIP_CUSTOMER_NAME", name: "送貨客戶名稱", "autoWidth": true },
            {
                data: "TRIP_ACTUAL_SHIP_DATE", name: "組車日", "autoWidth": true, "mRender": function (data, type, full) {
                    if (data != null) {
                        var dtStart = new Date(parseInt(data.substr(6)));
                        var dtStartWrapper = moment(dtStart);
                        return dtStartWrapper.format('YYYY-MM-DD');
                    } else {
                        return '';
                    }
                }
            },

            {
                data: "TRANSACTION_DATE", name: "出貨申請日", "autoWidth": true, "mRender": function (data, type, full) {
                    if (data != null) {
                        var dtStart = new Date(parseInt(data.substr(6)));
                        var dtStartWrapper = moment(dtStart);
                        return dtStartWrapper.format('YYYY-MM-DD');
                    } else {
                        return '';
                    }
                }
            },
            {
                data: "AUTHORIZE_DATE", name: "出貨核准日", "autoWidth": true,
            },

        ],

        "order": [[2, 'asc']],
        select: {
            style: 'multi',
        },
        buttons: {
            dom: {
                container: {
                    className: 'dt-buttons'
                },
            },

            buttons: [
                'selectAll',
                'selectNone',
                {
                    extend: 'excel',
                    text: '匯出Excel',
                    className: 'btn-primary',
                    init: function (api, node, config) {
                        $(node).removeClass('btn-default')
                    },
                    filename: function () {
                        return moment().format("YYYYMMDDHHmmss");
                    }
                },
                {
                    text: '取消申請',
                    className: 'btn-danger',
                    action: function () {
                        var data = TripDataTablesBody.rows('.selected').data();
                        if (data.length == 0) {
                            return false;
                        }

                        for (var i = 0; i < data.length; i++) {
                            if (data[i].DELIVERY_STATUS != '待核准') {
                                swal.fire('交運單' + data[i].DELIVERY_NAME + '狀態須為待核准');
                                return false;
                            }
                        }

                        swal.fire({
                            title: "取消申請",
                            text: "確定取消申請出貨嗎?",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonColor: "#DD6B55",
                            confirmButtonText: "確定",
                            cancelButtonText: "取消"
                        }).then(function (result) {
                            if (result.value) {
                                CancelConfirm(data);
                            }
                        });
                    },
                    init: function (api, node, config) {
                        $(node).removeClass('btn-default')
                    }
                },
                {
                    text: '出貨申請',
                    className: 'btn-danger',
                    action: function () {
                        var data = TripDataTablesBody.rows('.selected').data();
                        if (data.length == 0) {
                            return false;
                        }

                        for (var i = 0; i < data.length; i++) {
                            if (data[i].DELIVERY_STATUS != '已揀') {
                                swal.fire('交運單' + data[i].DELIVERY_NAME + '狀態須為已揀');
                                return false;
                            }
                        }

                        swal.fire({
                            title: "出貨申請",
                            text: "確定申請出貨嗎?",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonColor: "#DD6B55",
                            confirmButtonText: "確定",
                            cancelButtonText: "取消"
                        }).then(function (result) {
                            if (result.value) {
                                DeliveryConfirm(data);
                            }
                        });
                    },
                    init: function (api, node, config) {
                        $(node).removeClass('btn-default')
                    }
                },
                {
                    text: '編輯核准日',
                    className: 'buttons-advanced margin-left-ms btn-danger',
                    action: function () {
                        var data = TripDataTablesBody.rows('.selected').data();
                        if (data.length == 0) {
                            return false;
                        }

                        for (var i = 0; i < data.length; i++) {
                            if (data[i].DELIVERY_STATUS == '已出貨') {
                                swal.fire('已出貨，無法再修改核准日');
                                return;
                            }
                            if (data[i].DELIVERY_STATUS == '已取消') {
                                swal.fire('已取消，無法再修改核准日');
                                return;
                            }
                        }

                        editor.edit(TripDataTablesBody.rows({ selected: true }).indexes())
                            .title('編輯出貨核准日')
                            .buttons([
                                {
                                    text: '確定',
                                    className: 'btn-danger',
                                    action: function () {
                                        this.submit();
                                    }
                                }
                            ]);
                    },
                    init: function (api, node, config) {
                        $(node).removeClass('btn-default')
                    }
                },
                {
                    text: '航程號取消',
                    className: 'buttons-advanced btn-danger',
                    action: function () {
                        var data = TripDataTablesBody.rows('.selected').data();
                        if (data.length == 0) {
                            return false;
                        }

                        for (var i = 0; i < data.length; i++) {
                            if (data[i].DELIVERY_STATUS == '已取消') {
                                swal.fire('交運單' + data[i].DELIVERY_NAME + '已取消');
                                return false;
                            }
                            if (data[i].DELIVERY_STATUS == '已出貨') {
                                swal.fire('交運單' + data[i].DELIVERY_NAME + '已出貨不可取消');
                                return false;
                            }
                        }

                        swal.fire({
                            title: "航程號取消",
                            text: "確定取消航程號嗎?",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonColor: "#DD6B55",
                            confirmButtonText: "確定",
                            cancelButtonText: "取消"
                        }).then(function (result) {
                            if (result.value) {
                                CancelTrip(data);
                            }
                        });
                    },
                    init: function (api, node, config) {
                        $(node).removeClass('btn-default')
                    }
                },
                {
                    text: '出貨駁回',
                    className: 'buttons-advanced btn-danger',
                    action: function () {
                        var data = TripDataTablesBody.rows('.selected').data();
                        if (data.length == 0) {
                            return false;
                        }

                        for (var i = 0; i < data.length; i++) {
                            if (data[i].DELIVERY_STATUS != '待核准') {
                                swal.fire('交運單' + data[i].DELIVERY_NAME + '狀態須為待核准');
                                return false;
                            }
                        }

                        swal.fire({
                            title: "駁回出貨",
                            text: "確定駁回出貨嗎?",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonColor: "#DD6B55",
                            confirmButtonText: "確定",
                            cancelButtonText: "取消"
                        }).then(function (result) {
                            if (result.value) {
                                CancelAuthorize(data);
                            }
                        });
                    },
                    init: function (api, node, config) {
                        $(node).removeClass('btn-default')
                    }
                },
                {
                    text: '出貨核准',
                    className: 'buttons-advanced btn-danger',
                    action: function () {
                        var data = TripDataTablesBody.rows('.selected').data();
                        if (data.length == 0) {
                            return false;
                        }

                        for (var i = 0; i < data.length; i++) {
                            if (data[i].DELIVERY_STATUS != '待核准') {
                                swal.fire('交運單' + data[i].DELIVERY_NAME + '狀態須為待核准');
                                return false;
                            }
                        }

                        deliveryAuthorizeEditor.edit(TripDataTablesBody.rows({ selected: true }).indexes())
                            .title('出貨核准')
                            .buttons([
                                {
                                    text: '確定',
                                    className: 'btn-danger',
                                    action: function () {
                                        swal.fire({
                                            title: "出貨核准",
                                            text: "確定核准出貨嗎?",
                                            type: "warning",
                                            showCancelButton: true,
                                            confirmButtonColor: "#DD6B55",
                                            confirmButtonText: "確定",
                                            cancelButtonText: "取消"
                                        }).then(function (result) {
                                            if (result.value) {
                                                deliveryAuthorizeEditor.submit();
                                            }
                                        });
                                    }
                                }
                            ]);
                    },
                    init: function (api, node, config) {
                        $(node).removeClass('btn-default')
                    }
                },
            ],
        },

        "rowCallback": function (row, data, displayNum, displayIndex, dataIndex) {

            if ($.inArray(data.Id, selected) !== -1) {
                var selectRow = ':eq(' + dataIndex + ')';
                TripDataTablesBody.row(selectRow, { page: 'current' }).select();
            }

            //判斷是否顯示進階功能
            $.ajax({
                url: "/Delivery/GetAdvancedStatus",
                type: "post",
                data: {},
                success: function (data) {
                    $('.buttons-advanced')[0].style.visibility = data.status ? 'visible' : 'hidden'
                    $('.buttons-advanced')[1].style.visibility = data.status ? 'visible' : 'hidden'
                    $('.buttons-advanced')[2].style.visibility = data.status ? 'visible' : 'hidden'
                    $('.buttons-advanced')[3].style.visibility = data.status ? 'visible' : 'hidden'
                },
                error: function () {
                    swal.fire('取得使用者進階權限失敗');
                },
                complete: function (data) {

                }
            });       
        }
    });

    TripDataTablesBody.on('select', function (e, dt, type, indexes) {
        if (type === 'row') {

            var Id = dt.rows(indexes).data().pluck('Id')[0];
            var index = $.inArray(Id, selected);
            if (index === -1) {
                selected.push(Id);
            }
        }
    });

    TripDataTablesBody.on('deselect', function (e, dt, type, indexes) {
        if (type === 'row') {


            var Id = dt.rows(indexes).data().pluck('Id')[0];
            var index = $.inArray(Id, selected);
            selected.splice(index, 1);
        }
    });


    $('#TripDataTablesBody tbody').on('click', '.btn-edit', function (e) {

        var data = TripDataTablesBody.row($(this).parents('tr')).data();
        if (data == null) {
            return false;
        }
        if (data.DetailType == "捲筒") {
            window.location.href = "/Delivery/RollEdit/" + data.Id
            return false;
        } else if (data.DetailType == "平版") {
            window.location.href = "/Delivery/FlatEdit/" + data.Id
            return false;
        } else if (data.DetailType == "代紙") {
            window.location.href = "/Delivery/InsteadEdit/" + data.Id
            return false;
        } else {
            return false;
        }
    })

    $('#TripDataTablesBody tbody').on('click', '.btn-view', function (e) {

        var data = TripDataTablesBody.row($(this).parents('tr')).data();
        if (data == null) {
            return false;
        }
        if (data.DetailType == "捲筒") {
            window.location.href = "/Delivery/RollView/" + data.Id
            return false;
        } else if (data.DetailType == "平版") {
            window.location.href = "/Delivery/FlatView/" + data.Id
            return false;
        } else {
            return false;
        }
    })

    $('#TripDataTablesBody tbody').on('click', '.btn-print', function (e) {

        var data = TripDataTablesBody.row($(this).parents('tr')).data();
        if (data == null) {
            return false;
        }

        PrintPickList(data);
        
    })

    $('#btnSearch').click(function () {
        TripDataTablesBody.ajax.reload();
        return false;
    });

    
    TripDataTablesBody.buttons(2, null).containers().appendTo('#btnExportExcel');

    //取得判斷核准日可選範圍的基準日
    function GetStandardDate() {
        var sandardDate = new Date();
        sandardDate.setDate(5);
        return sandardDate;
    }

    function GetTransactionAuthorizeMinDate() {
        var sandardDate = GetStandardDate();
        var now = new Date();
        if (now > sandardDate) {
            now.setDate(1);
            return moment(now).format('YYYY-MM-DD');
        } else {
            now.setMonth(now.getMonth() - 1);
            now.setDate(1);
            return moment(now).format('YYYY-MM-DD');
        }
    }

    function GetTransactionAuthorizeMaxDate() {
        return new Date();
    }


    function DeliveryConfirm(data) {


        var list = [];
        for (i = 0; i < data.length; i++) {
            list.push(data[i].Id);
        }

        ShowWait(function () {
            $.ajax({
                url: "/Delivery/DeliveryConfirm",
                type: "post",
                data: {
                    'id': list
                },
                success: function (data) {
                    if (data.status) {
                        CloseWait();
                        TripDataTablesBody.ajax.reload(null, false);
                    }
                    else {
                        swal.fire(data.result);
                    }
                },
                error: function () {
                    swal.fire('出貨申請失敗');
                },
                complete: function (data) {


                }

            });
        });

        
    }

    function CancelConfirm(data) {


        var list = [];
        for (i = 0; i < data.length; i++) {
            list.push(data[i].Id);
        }
        ShowWait(function () {
            $.ajax({
                url: "/Delivery/CancelConfirm",
                type: "post",
                data: {
                    'id': list
                },
                success: function (data) {
                    if (data.status) {
                        CloseWait();
                        TripDataTablesBody.ajax.reload(null, false);

                    }
                    else {
                        swal.fire(data.result);
                    }
                },
                error: function () {
                    swal.fire('取消出貨申請失敗');
                },
                complete: function (data) {


                }

            });
        });
        
    }

    function PrintPickList(selectData) {
        
        ShowWait(function () {
            $.ajax({
                url: "/Delivery/PrintPickList",
                type: "post",
                data: {
                    'id': selectData.Id
                },
                success: function (data) {
                    if (data.status) {
                        CloseWait();
                        TripDataTablesBody.ajax.reload(null, false);
                        window.open("/Delivery/PickingReport/?tripName=" + selectData.TRIP_NAME);
                    }
                    else {
                        swal.fire(data.result);
                    }
                },
                error: function () {
                    swal.fire('列印備貨單失敗');
                },
                complete: function (data) {


                }

            });
        });


        
    }

    function DeliveryAuthorize(data) {


        var list = [];
        for (i = 0; i < data.length; i++) {
            list.push(data[i].Id);
        }

        $.ajax({
            url: "/Delivery/DeliveryAuthorize",
            type: "post",
            data: {
                'id': list
            },
            success: function (data) {
                if (data.status) {

                    TripDataTablesBody.ajax.reload(null, false);

                }
                else {
                    swal.fire(data.result);
                }
            },
            error: function () {
                swal.fire('出貨核准失敗');
            },
            complete: function (data) {


            }

        });
    }

    function CancelAuthorize(data) {


        var list = [];
        for (i = 0; i < data.length; i++) {
            list.push(data[i].Id);
        }

        ShowWait(function () {
            $.ajax({
                url: "/Delivery/CancelAuthorize",
                type: "post",
                data: {
                    'id': list
                },
                success: function (data) {
                    if (data.status) {
                        CloseWait();
                        TripDataTablesBody.ajax.reload(null, false);

                    }
                    else {
                        swal.fire(data.result);
                    }
                },
                error: function () {
                    swal.fire('取消出貨核准失敗');
                },
                complete: function (data) {


                }

            });
        });


        
    }


    function CancelTrip(data) {


        var list = [];
        for (i = 0; i < data.length; i++) {
            list.push(data[i].Id);
        }


        ShowWait(function () {
            $.ajax({
                url: "/Delivery/CancelTrip",
                type: "post",
                data: {
                    'id': list
                },
                success: function (data) {
                    if (data.status) {
                        CloseWait();
                        TripDataTablesBody.ajax.reload(null, false);

                    }
                    else {
                        swal.fire(data.result);
                    }
                },
                error: function () {
                    swal.fire('取消航程號失敗');
                },
                complete: function (data) {


                }

            });
        });

        
    }


    function refresh() {

        TripDataTablesBody.ajax.reload();
        if ($('#Advanced').text() == false) {
        }
    }

    refresh();
});
