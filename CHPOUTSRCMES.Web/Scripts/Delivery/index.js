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


    //$.fn.dataTable.moment('YYYY-MM-DD');

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
                //var ids = [];
                //var dates = [];
                var TripDetailDTList = [];
                var size = Object.keys(tripDetailDTData).length;
                for (var i = 0; i < size; i++) {
                    var TRIP_ID = Object.keys(tripDetailDTData)[i];
                    var AUTHORIZE_DATE = Object.values(tripDetailDTData[TRIP_ID])[0];
                    //ids.push(tripDetailDT_ID);
                    //dates.push(TRANSACTION_AUTHORIZE_DATE);
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
                    //'TripDetailDT_IDs': ids,
                    //'TRANSACTION_AUTHORIZE_DATEs': dates
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
        //formOptions: {
        //    main: {
        //        buttons:  true

        //    }
        //},

        table: "#TripDataTablesBody",
        idSrc: 'TRIP_ID',
        //template: '#customForm',
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
                //var ids = [];
                //var dates = [];
                var TripDetailDTList = [];
                var size = Object.keys(tripDetailDTData).length;
                for (var i = 0; i < size; i++) {
                    var TRIP_ID = Object.keys(tripDetailDTData)[i];
                    var AUTHORIZE_DATE = Object.values(tripDetailDTData[TRIP_ID])[0];
                    //ids.push(tripDetailDT_ID);
                    //dates.push(TRANSACTION_AUTHORIZE_DATE);
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
                    //'TripDetailDT_IDs': ids,
                    //'TRANSACTION_AUTHORIZE_DATEs': dates
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
                swal.fire('出貨核准失敗');
            },
            complete: function (data) {


            }
        },
        //formOptions: {
        //    main: {
        //        buttons:  true

        //    }
        //},

        table: "#TripDataTablesBody",
        idSrc: 'TRIP_ID',
        //template: '#customForm',
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
        //"scrollX": true,
        language: {
            url: '/bower_components/datatables/language/zh-TW.json'
        },
        autoWidth: false,
        //pageLength: 3,
        serverSide: true,
        processing: true,
        deferLoading: 0, //初始化DataTable時，不發出ajax
        //sScrollX: "100%",
        //sScrollXInner: "110%",
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'B><'col-sm-3'f>>" +
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
            { data: "SUB_ID", name: "項次", "autoWidth": true },
            { data: "FREIGHT_TERMS_NAME", name: "內銷地區別", "autoWidth": true },
            { data: "TRIP_NAME", name: "航程號", "autoWidth": true },
            { data: "DELIVERY_NAME", name: "交運單", "autoWidth": true },
            { data: "DetailType", name: "作業別", "autoWidth": true },
            { data: "DELIVERY_STATUS", name: "狀態", "autoWidth": true },
            { data: "CUSTOMER_NAME", name: "客戶", "autoWidth": true },
            { data: "CUSTOMER_LOCATION_CODE", name: "送貨地點", "autoWidth": true },
            //{ data: "ORDER_NUMBER", name: "訂單", "autoWidth": true },
            //{ data: "ORDER_SHIP_NUMBER", name: "訂單行號", "autoWidth": true },
            //{ data: "ITEM_DESCRIPTION", name: "料號名稱", "autoWidth": true },
            //{ data: "PAPER_TYPE", name: "紙別", "autoWidth": true },
            //{ data: "BASIC_WEIGHT", name: "基重", "autoWidth": true },

            //{ data: "SPECIFICATION", name: "規格", "autoWidth": true },
            //{ data: "GRAIN_DIRECTION", name: "絲向", "autoWidth": true },
            //{ data: "PACKING_TYPE", name: "包裝方式", "autoWidth": true },
            //{ data: "SRC_REQUESTED_QUANTITY", name: "訂單原始數量", "autoWidth": true },
            //{ data: "SRC_REQUESTED_QUANTITY_UOM", name: "訂單主單位", "autoWidth": true },
            //{ data: "REQUESTED_QUANTITY2", name: "預計出庫輔數量", "autoWidth": true },
            //{ data: "SRC_REQUESTED_QUANTITY_UOM2", name: "輔單位(RE)", "autoWidth": true },
            //{ data: "REQUESTED_QUANTITY", name: "預計出庫量", "autoWidth": true },
            //{ data: "REQUESTED_QUANTITY_UOM", name: "庫存單位(KG)", "autoWidth": true },
            { data: "SUBINVENTORY_CODE", name: "出貨倉庫", "autoWidth": true },
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
            { data: "NOTE", name: "備註", "autoWidth": true, className: "dt-body-left" },

            {
                data: null, "autoWidth": true, orderable: false,
                render: function (data, type, row, meta) {
                    if (data.DELIVERY_STATUS == null || data.DELIVERY_STATUS == "已取消" || data.DELIVERY_STATUS == "未印") {
                        return null
                    }
                    if (data.DELIVERY_STATUS == "待出" || data.DELIVERY_STATUS == "已揀") {
                        //return '<a href="' + data.DELIVERY_NAME + '">出貨</a>';
                        return '<button class="btn btn-danger btn-sm btn-edit"><i class="fa fa-pencil"></i>出貨</button>'
                    }
                    if (data.DELIVERY_STATUS == "待核准" || data.DELIVERY_STATUS == "已出貨") {
                        //return '<a href="' + data.DELIVERY_NAME + '">紀錄</a>';
                        return '<button class="btn btn-primary btn-sm btn-view"><i class="glyphicon glyphicon-eye-open"></i>紀錄</button>'
                    }
                }
            }

        ],

        "order": [[1, 'asc']],
        select: {
            style: 'multi',
            //blurable: true,
            //selector: 'td:first-child'
        },
        buttons: {
            dom: {
                container: {
                    className: 'dt-buttons'
                },
                //button: {
                //    tag: 'button',
                //    className: 'btn externalBtn'
                //}
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
                    }
                },
                {
                    text: '列印備貨單',
                    className: 'btn-primary',
                    action: function () {
                        PrintPickList();
                    },
                    init: function (api, node, config) {
                        $(node).removeClass('btn-default')
                    }
                },
                //{
                //    extend: 'edit',
                //    editor: editor,
                //    className: "btn-danger"
                //},
                //{
                //    extend: 'edit',
                //    editor: editor,
                //    formButtons: {
                //        text: 'Save',
                //        action: function () { this.submit(); },
                //        className: 'btn btn-danger'
                //    }
                //},

                //{
                //    text: '編輯核准日',
                //    className: 'btn-danger',
                //    init: function (api, node, config) {
                //        $(node).removeClass('btn-default')
                //    },
                //    action: function (e, dt, node, config) {
                //        var count = dt.rows({ selected: true }).count();

                //        if (count == 0) {
                //            return;
                //        }
                //        //var data = dt.rows({ selected: true }).data().pluck('Id')[0];

                //        //var Id = dt.rows(indexes).data().pluck('Id')[0];

                //        for (i = 0; i < count; i++) {

                //            if (dt.rows({ selected: true }).data().pluck('DELIVERY_STATUS')[i] == '已出貨') {
                //                swal.fire('已出貨，無法再修改核准日');
                //                return;
                //            }
                //            if (dt.rows({ selected: true }).data().pluck('DELIVERY_STATUS')[i] == '已取消') {
                //                swal.fire('已取消，無法再修改核准日');
                //                return;
                //            }
                //            //if (data[i].DELIVERY_STATUS == '已出貨') {
                //            //    swal.fire('已出貨，無法再修改核准日');
                //            //    return;
                //            //}
                //        }

                //        editor.edit(TripDataTablesBody.rows({ selected: true }).indexes())
                //            .title('編輯出貨核准日')
                //            .buttons({
                //                text: '確定',
                //                action: function () {
                //                    this.submit();
                //                },
                //                className: 'btn-danger'
                //            });
                //    }
                //}

                //{
                //    text: '出貨確認',
                //    className: 'btn-primary',
                //    action: function () {
                //        DeliveryConfirm();
                //    }
                //},
                //{
                //    text: '取消確認',
                //    className: 'btn-warning',
                //    action: function () {
                //        CancelConfirm();
                //    }
                //},
                //{
                //    text: '出貨核准',
                //    className: 'btn-danger',
                //    action: function () {
                //        DeliveryAuthorize();
                //    }
                //},
                //{
                //    text: '取消核准',
                //    className: 'btn-warning',
                //    action: function () {
                //        CancelAuthorize();
                //    }
                //}
            ],
        },

        "rowCallback": function (row, data, displayNum, displayIndex, dataIndex) {

            if ($.inArray(data.Id, selected) !== -1) {
                var selectRow = ':eq(' + dataIndex + ')';
                TripDataTablesBody.row(selectRow, { page: 'current' }).select();
            }
        }
    });


    //var data = editor.field('TRANSACTION_AUTHORIZE_DATE').def();

    //editor.field('TRANSACTION_AUTHORIZE_DATE').def(function () {

    //    if (data != null) {
    //        var dtStart = new Date(parseInt(data.substr(6)));
    //        var dtStartWrapper = moment(dtStart);
    //        return dtStartWrapper.format('YYYY-MM-DD');
    //    } else {
    //        return '';
    //    }
    //});

    //$('#TripDataTablesBody').on('click', 'tbody td:not(:first-child)', function (e) {
    //    editor.inline(this);
    //});

    //TripDataTablesBody.on('order.dt search.dt', function () {
    //    TripDataTablesBody.column(1, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
    //        cell.innerHTML = i + 1;
    //    });
    //}).draw();

    TripDataTablesBody.on('select', function (e, dt, type, indexes) {
        if (type === 'row') {

            //var rowsData = TripDataTablesBody.rows({ page: 'current' }).data();
            //for (i = 0 ; i < rowsData.length; i++) {
            //    for (j = 0; j < selected.length; j++) {
            //        if (selected[j] == rowsData[i].Id) {
            //            selected.splice(j, 1);
            //        }
            //    }
            //}

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
            //window.open("/Delivery/RollEdit/" + data.Id, "_blank");
            return false;
        } else if (data.DetailType == "平版") {
            window.location.href = "/Delivery/FlatEdit/" + data.Id
            //window.open("/Delivery/FlatEdit/" + data.Id, "_blank");
            return false;
        } else if (data.DetailType == "代紙") {
            window.location.href = "/Delivery/InsteadEdit/" + data.Id
            //window.open("/Delivery/InsteadEdit/" + data.Id, "_blank");
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
            //window.open("/Delivery/RollView/" + data.Id, "_blank");
            return false;
        } else if (data.DetailType == "平版") {
            window.location.href = "/Delivery/FlatView/" + data.Id
            //window.open("/Delivery/FlatView/" + data.Id, "_blank");
            return false;
        } else if (data.DetailType == "代紙") {
            window.location.href = "/Delivery/InsteadView/" + data.Id
            //window.open("/Delivery/InsteadView/" + data.Id, "_blank");
            return false;
        } else {
            return false;
        }
    })


    $('.box-footer').on('click', '#btnSearch', function (e) {
        TripDataTablesBody.ajax.reload();
        return false;

        //$.ajax({
        //    url: "/Delivery/Search",
        //    type: "post",
        //    data: {
        //        'TripActualShipBeginDate': $("#txtTripActualShipBeginDate").val(),
        //        'TripActualShipEndDate': $("#txtTripActualShipEndDate").val(),
        //        'DeliveryName': $("#txtDeliveryName").val(),
        //        'SelectedSubinventory': $("#ddlWarehouse").val(),
        //        SelectedTrip: $("#ddlTrip").val(),
        //        TransactionDate: $("#txtTransactionDate").val(),
        //        SelectedDeliveryStatus: $("#ddlDeliveryStatus").val()

        //    },
        //    success: function (data) {
        //        if (data.status) {
        //            if (data.result == "搜尋成功") {
        //                TripDataTablesBody.ajax.reload();
        //            }
        //        }
        //        else {
        //            swal.fire(data.result);
        //        }
        //    },
        //    error: function () {
        //        swal.fire('搜尋失敗');
        //    },
        //    complete: function (data) {


        //    }

        //});
        //return false;
    });

    //$(".buttons-excel").detach();

  

    //$("#btnPrintPickList").click(function () {
    //    PrintPickList();

    //});
    TripDataTablesBody.buttons(2, null).containers().appendTo('#btnExportExcel');

    $("#btnUpdateTransactionAuthorizeDates").click(function () {
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
                swal.fire('已出貨，無法再修改核准日');
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


    });

    $("#btnDeliveryConfirm").click(function () {
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

        //DeliveryConfirm();
    });

    $("#btnCancelConfirm").click(function () {
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

        //CancelConfirm();
    });

    //$("#btnPrintPick").click(function () {
    //    PrintPickList();
    //});

    $("#btnDeliveryAuthorize").click(function () {
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
                //{
                //    text: '駁回',
                //    className: 'btn-danger',
                //    action: function () {
                //        swal.fire({
                //            title: "駁回",
                //            text: "確定駁回出貨嗎?",
                //            type: "warning",
                //            showCancelButton: true,
                //            confirmButtonColor: "#DD6B55",
                //            confirmButtonText: "確定",
                //            cancelButtonText: "取消"
                //        }).then(function (result) {
                //            if (result.value) {
                //                //editor.submit();
                //                CancelAuthorize(data);
                //            }
                //        });
                //    },
                //},
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
                                //DeliveryAuthorize(data);
                            }
                        });
                    }
                }
            ]);
    });


    $("#btnDeliveryReject").click(function () {
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

        //CancelAuthorize();
    });


    $("#btnTripChancel").click(function () {
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

    });

    function GetTransactionAuthorizeMinDate() {
        var now = new Date();
        now.setDate(1);
        now.setMonth(now.getMonth() - 1);
        var miniDate = moment(now).format('YYYY-MM-DD');
        return miniDate;
    }

    function GetTransactionAuthorizeMaxDate() {
        var now = new Date();
        now.setDate(3);
        var miniDate = moment(now).format('YYYY-MM-DD');
        return miniDate;
    }


    function DeliveryConfirm(data) {


        var list = [];
        for (i = 0; i < data.length; i++) {
            list.push(data[i].Id);
        }

        $.ajax({
            url: "/Delivery/DeliveryConfirm",
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
                swal.fire('出貨申請失敗');
            },
            complete: function (data) {


            }

        });
    }

    function CancelConfirm(data) {


        var list = [];
        for (i = 0; i < data.length; i++) {
            list.push(data[i].Id);
        }

        $.ajax({
            url: "/Delivery/CancelConfirm",
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
                swal.fire('取消出貨申請失敗');
            },
            complete: function (data) {


            }

        });
    }

    function PrintPickList() {
        var data = TripDataTablesBody.rows('.selected').data();
        if (data.length == 0) {
            return false;
        }

        var list = [];
        for (i = 0; i < data.length; i++) {
            list.push(data[i].Id);
        }

        $.ajax({
            url: "/Delivery/PrintPickList",
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
                swal.fire('列印備貨單失敗');
            },
            complete: function (data) {


            }

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

        $.ajax({
            url: "/Delivery/CancelAuthorize",
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
                swal.fire('取消出貨核准失敗');
            },
            complete: function (data) {


            }

        });
    }


    function CancelTrip(data) {


        var list = [];
        for (i = 0; i < data.length; i++) {
            list.push(data[i].Id);
        }

        $.ajax({
            url: "/Delivery/CancelTrip",
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
                swal.fire('取消航程號失敗');
            },
            complete: function (data) {


            }

        });
    }


    function refresh() {

        TripDataTablesBody.ajax.reload();
        if ($('#Advanced').text() == false) {
            //TripDataTablesBody.buttons('.abc').nodes().addClass('hidden');
            //TripDataTablesBody.buttons('.abc').nodes().css("display", "none");
            //TripDataTablesBody.buttons().destroy();


        }
    }

    refresh();
});
