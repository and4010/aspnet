var rowData
var ProcessDataTables
$(document).ready(function () {
    
    BtnEvent()
    search()
    //初始化日期
    $('#Demand_Date').datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true });
    $('#Cutting_Date_From').datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true });
    $('#Cutting_Date_To').datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true
    });


    firstLoad();


    ProcessDataTables.on('click', '#btnRecord', function (e) {

        var data = ProcessDataTables.row($(this).parents('tr')).data();
        var Process_Detail_Id = data.Process_Detail_Id;
        if (data == null) {
            return false;
        }
        if (Process_Detail_Id == 1) {
            window.location.href = '/Process/Schedule/' + Process_Detail_Id;
        }
        if (Process_Detail_Id == 2) {
            window.location.href = '/Process/Flat/' + Process_Detail_Id;
        }

        if (Process_Detail_Id == 3) {
            window.location.href = '/Process/PaperRoll/' + Process_Detail_Id;
        }

       
    })

    ProcessDataTables.on('click', '#btnEdit', function (e) {

        var data = ProcessDataTables.row($(this).parents('tr')).data();
        var Process_Detail_Id = data.Process_Detail_Id;
        if (data == null) {
            return false;
        }
        window.location.href = '/Process/Edit/' + Process_Detail_Id;
    })


    //重新整理表格寬度
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        $('#ProcessDataTables').DataTable().table().node().style.width = '';

        // Need to reset width on table, not sure why.
        $.fn.dataTable.tables({ visible: true, api: true }).table().node().style.width = '';

        // .draw() is necessary for "no results" message to be properly positioned
        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust().draw();


    });



})

function ProcessLoadTable(Process_Status, Process_Batch_no, Manchine_Num, Demand_Date, Cutting_Date_From, Cutting_Date_To, Subinventory) {

    ProcessDataTables = $('#ProcessDataTables').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        destroy: true,
        processing: true,
        serverSide: true,
        autoWidth: false,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Process/TableResult",
            "type": "POST",
            "datatype": "json",
            "data": {
                Process_Status: Process_Status, Process_Batch_no: Process_Batch_no,
                Manchine_Num: Manchine_Num, Demand_Date: Demand_Date, Cutting_Date_From: Cutting_Date_From,
                Cutting_Date_To: Cutting_Date_To, Subinventory: Subinventory
            }
        },
        select: {
            style: 'single',
            selector: 'td:first-child'
        },
        columnDefs: [{
            orderable: false, targets: [0, 24], width: "60px",
        }],
        columns: [
            {
                data: null,
                defaultContent: '',
                className: 'select-checkbox',
                orderable: false,
                targets: 0
            },
            {
                data: "Demand_Date", "name": "需求日", "autoWidth": true, "mRender": function (data, type, full) {
                    if (data != null) {
                        var dtStart = new Date(parseInt(data.substr(6)));
                        var dtStartWrapper = moment(dtStart);
                        return dtStartWrapper.format('YYYY-MM-DD');
                    } else {
                        return '';
                    }

                }, "className": "dt-body-center"
            },
            {
                data: "Cutting_Date_From", "name": "裁切日(起)", "autoWidth": true, "mRender": function (data, type, full) {
                    if (data != null) {
                        var dtStart = new Date(parseInt(data.substr(6)));
                        var dtStartWrapper = moment(dtStart);
                        return dtStartWrapper.format('YYYY-MM-DD');
                    } else {
                        return '';
                    }
                }, "className": "dt-body-center"
            },
            {
                data: "Cutting_Date_To", "name": "裁切日(迄)", "autoWidth": true, "mRender": function (data, type, full) {
                    if (data != null) {
                        var dtStart = new Date(parseInt(data.substr(6)));
                        var dtStartWrapper = moment(dtStart);
                        return dtStartWrapper.format('YYYY-MM-DD');
                    } else {
                        return '';
                    }
                }, "className": "dt-body-center"
            },
            { data: "Process_Batch_no", "name": "工單號", "autoWidth": true, "className": "dt-body-center"},
            { data: "Manchine_Num", "name": "機台", "autoWidth": true, "className": "dt-body-center"},
            {
                data: "Process_Status", "name": "狀態", "autoWidth": true, "render": function (data, type, row) {
                    if (data == "已排單") {
                        switch (row.Process_Batch_no.substr(0, 1)) {
                            case "P":
                                return '<a href=/Process/Schedule/' + row.Process_Detail_Id + '> ' + data + '</a>';
                                break;
                            case "F":
                                return '<a href=/Process/Flat/' + row.Process_Detail_Id + '> ' + data + '</a>';
                                break;
                            case "R":
                                return '<a href=/Process/PaperRoll/' + row.Process_Detail_Id + '> ' + data + '</a>';
                                break;
                        }
                    } else {
                        return data;
                    }

                    if (data == "已排單") {
                        switch (row.Process_Batch_no.substr(0, 1)) {
                            case "P":
                                return '<a href=/Process/Schedule/' + row.Process_Detail_Id + '> ' + data + '</a>';
                                break;
                            case "F":
                                return '<a href=/Process/Flat/' + row.Process_Detail_Id + '> ' + data + '</a>';
                                break;
                            case "R":
                                return '<a href=/Process/PaperRoll/' + row.Process_Detail_Id + '> ' + data + '</a>';
                                break;
                        }
                    }


                }, "className": "dt-body-center"
            },
            { data: "Cosutomer_Num", "name": "客戶名稱", "autoWidth": true, "className": "dt-body-center"},
            { data: "Paper_Type", "name": "紙別", "autoWidth": true, "className": "dt-body-center"},
            { data: "Basic_Weight", "name": "基重", "autoWidth": true, "className": "dt-body-right"},
            { data: "Specification", "name": "規格", "autoWidth": true, "className": "dt-body-center"},
            { data: "Ream_Weight", "name": "令重", "autoWidth": true, "className": "dt-body-right"},
            { data: "Grain_Direction", "name": "絲向", "autoWidth": true, "className": "dt-body-center"},
            { data: "Ream_Qty", "name": "令數", "autoWidth": true, "className": "dt-body-right" },
            { data: "TransactionUom", "name": "交易單位", "autoWidth": true, "className": "dt-body-center" },
            { data: "Weight", "name": "重量", "autoWidth": true, "className": "dt-body-right" },
            { data: "PrimaryUom", "name": "主要單位", "autoWidth": true, "className": "dt-body-center"},
            { data: "Packing_Type", "name": "包裝方式", "autoWidth": true, "className": "dt-body-center"},
            { data: "Outsourching_Remark", "name": "委外工單備註", "autoWidth": true, "className": "dt-body-center"},
            { data: "Produce_Remark", "name": "生產備註", "autoWidth": true, "className": "dt-body-center"},
            { data: "SelectedInventoryItemNumber", "name": "組成成份料號", "autoWidth": true, "className": "dt-body-center"},
            { data: "Product_Item", "name": "產品料號", "autoWidth": true, "className": "dt-body-center"},
            { data: "Order_Number", "name": "訂單編號", "autoWidth": true, "className": "dt-body-center"},
            { data: "Detail_Line", "name": "明細行", "autoWidth": true, "className": "dt-body-center"},
            {
                data: "", "autoWidth": true, "render": function (data, type, row) {
                    if (row.Process_Status == "已完工") {
                        return '<button class="btn btn-primary btn-sm" id = "btnEdit">編輯</button>' + '<button class="btn btn-primary btn-sm" id = "btnRecord">完工紀錄</button>';
                    }
                    if (row.Process_Status == "待核准") {
                        return '<button class="btn btn-primary btn-sm" id = "btnEdit">編輯</button>' + '<button class="btn btn-primary btn-sm" id = "btnRecord">完工紀錄</button>';
                    }
                    if (row.Process_Status == "已排單") {
                        return '<button class="btn btn-primary btn-sm" id = "btnEdit">編輯</button>';
                    } else {
                        return '<button class="btn btn-primary btn-sm" id = "btnEdit">編輯</button>';
                    }
       
                }
            }
        ],

    });

    ProcessDataTables.on('select', function (e, dt, type, indexes) {
        rowData = ProcessDataTables.rows(indexes).data();
    });

    ProcessDataTables.on('deselect', function (e, dt, type, indexes) {
        rowData = null;
    });

}

//不會用到了
//function ChooseBtn() {


//    var list = document.getElementById("listStatus");
//    for (i = 0; i <= list.childElementCount - 1; i++) {
//        list.children[i].addEventListener("click", function () {
//            $('#BtnStatus').text($(this).text());
//        })
//    }

//    var list = document.getElementById("listPrint");
//    for (i = 0; i <= list.childElementCount - 1; i++) {
//        list.children[i].addEventListener("click", function () {
//            $('#BtnPrint').text($(this).text());
//        })
//    }


//}

function BtnEvent() {
    $('#BtnStatus').click(function () {
        if (rowData == null) {
            swal.fire("請先選擇一項");
            return;
        }
        var Process_Status = rowData.pluck('Process_Status')[0]
        var Process_Detail_Id = rowData.pluck('Process_Detail_Id')[0]
        if (Process_Status == "待排單") {
            $.ajax({
                url: '/Process/_ProcessIndex',
                type: "POST",
                success: function (result) {
                    $('body').append(result);
                    Open($('#ProcessModal'), Process_Detail_Id);
                },
                error: function () {
                    swal.fire("失敗");
                }
            });

        } else {
            swal.fire("加工狀態不正確，重新選擇");
            return;
        }


    });

    $('#BtnCloss').click(function () {
        if (rowData == null) {
            swal.fire("請先選擇一項")
            return;
        }
        var BtnCloss = $('#BtnCloss').text();
        var Process_Status = rowData.pluck('Process_Status')[0]
        var Process_Detail_Id = rowData.pluck('Process_Detail_Id')[0]
        if (Process_Status == "已完工") {
            changeStatus(Process_Detail_Id, BtnCloss);
        } else {
            swal.fire("加工狀態不正確，重新選擇");
            return;
        }


    });

    $('#BtnEdit').click(function () {
        if (rowData == null) {
            swal.fire("請先選擇一項")
            return;
        }
        var Process_Status = rowData.pluck('Process_Status')[0]
        var Process_Detail_Id = rowData.pluck('Process_Detail_Id')[0]
        if (Process_Status == "已排單") {
            $.ajax({
                url: '/Process/_ProcessIndex',
                type: "POST",
                success: function (result) {
                    $('body').append(result);
                    Open($('#ProcessModal'), Process_Detail_Id);
                },
                error: function () {
                    swal.fire("失敗");
                }
            });
        } else {
            swal.fire("加工狀態不正確，重新選擇");
            return;
        }

    });
}

function search() {
    $('#btnSearch').click(function () {
        rowData = null;
        var Process_Status = $("#Process_Status").val();
        var Process_Batch_no = $("#Process_Batch_no").val();
        var Manchine_Num = $("#Manchine_Num").val();
        var Demand_Date = $("#Demand_Date").val();
        var Cutting_Date_From = $("#Cutting_Date_From").val();
        var Cutting_Date_To = $("#Cutting_Date_To").val();
        var Subinventory = $("#Subinventory").val();

        ProcessLoadTable(Process_Status, Process_Batch_no, Manchine_Num, Demand_Date, Cutting_Date_From, Cutting_Date_To, Subinventory);
        
        
    });

}

//彈出dialog
function Open(modal_dialog, Process_Detail_Id) {
    modal_dialog.modal({
        backdrop: "static",
        keyboard: true,
        show: true
    });

    //初始化
    $('#dialog_Cutting_Date_From').datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true });
    $('#dialog_Cutting_Date_To').datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true });

    modal_dialog.on('hidden.bs.modal', function (e) {
        $("div").remove(modal_dialog.selector);
    });

    modal_dialog.on('show.bs.modal', function (e) {
        $.validator.unobtrusive.parse('form');
    });

    //確認按鍵
    modal_dialog.on('click', '#btnDailogProcess', function (e) {
        var dialog_Cutting_Date_From = $('#dialog_Cutting_Date_From').val();
        var dialog_Cutting_Date_To = $('#dialog_Cutting_Date_To').val();
        var dialg_Manchine_Num = $('#dialg_Manchine_Num').val();
        var BtnStatus = "已排單";

        if (dialog_Cutting_Date_From.length == 0) {
            swal.fire("裁切日期(起)不得空白");
            return
        }

        if (dialog_Cutting_Date_To.length == 0) {
            swal.fire("裁切日期(迄)不得空白");
            return
        }

        if (dialg_Manchine_Num.length == 0) {
            swal.fire("機台不得空白");
            return
        }

        if (new Date(dialog_Cutting_Date_From) > new Date(dialog_Cutting_Date_To)) {
            swal.fire("時間起不得超過迄");
            return
        }

        $.ajax({
            url: '/Process/_BtnDailog',
            type: "POST",
            dataType: 'json',
            data: {
                Process_Detail_Id: Process_Detail_Id, dialog_Cutting_Date_From: dialog_Cutting_Date_From
                , dialog_Cutting_Date_To: dialog_Cutting_Date_To, dialg_Manchine_Num: dialg_Manchine_Num, BtnStatus: BtnStatus
            },
            success: function (result) {
                $(modal_dialog.selector).modal('hide');
                firstLoad();
                rowData = null;
            },
            error: function () {
                swal.fire("時間格式錯誤");
            }
        });



    });



    modal_dialog.modal('show');

}

function changeStatus(Process_Detail_Id, BtnStatus) {
    $.ajax({
        url: '/Process/_BtnDailog',
        type: "POST",
        dataType: 'json',
        data: { Process_Detail_Id: Process_Detail_Id, BtnStatus: BtnStatus },
        success: function (result) {
            firstLoad();
        },
        error: function () {
            swal.fire("失敗")
        }
    });


}

function firstLoad() {
    var Process_Status = $("#Process_Status").val();
    var Process_Batch_no = $("#Process_Batch_no").val();
    var Manchine_Num = $("#Manchine_Num").val();
    var Demand_Date = $("#Demand_Date").val();
    var Cutting_Date_From = $("#Cutting_Date_From").val();
    var Cutting_Date_To = $("#Cutting_Date_To").val();
    var Subinventory = $("#Subinventory").val();

    ProcessLoadTable(Process_Status, Process_Batch_no, Manchine_Num, Demand_Date, Cutting_Date_From, Cutting_Date_To, Subinventory);
}