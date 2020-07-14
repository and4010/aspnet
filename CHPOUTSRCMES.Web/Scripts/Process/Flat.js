var FlatInvestTable
var FlatProductionDataTables
var EditorFlatProduction
var EditorFlatInvest

$(document).ready(function () {
    BtnRecord();
    init();
    onBtnClick();
    LoadFlatInvestTable();
    LoadFlatProductionDataTable();

    var Process_Status = $('#Process_Status').val();
    if (Process_Status == "已完工" || Process_Status == "待核准") {
         //完工紀錄使用
        DsiplayFlatHide();
        DisplayInvestFlatEnable(true);
        DisplayProductionFlatEnable(true);
    } else {
        DisplayInvestFlatEnable(true);
        DisplayProductionFlatEnable(true);
        DsiplayFlatShow();
    }

    //setTimeout(function () {
    //    var table = $('#FlatInvestDataTables').DataTable();
    //    if (table.data().length == 0) {
    //        DisplayFlatEnable(true);
    //    } else {
    //        //EnableBarcode(false);
    //        DisplayFlatEnable(true);
    //    }
    //}, 100);


    //重新整理表格寬度
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        $('#FlatInvestDataTables').DataTable().table().node().style.width = '';
        $('#FlatProductionDataTables').DataTable().table().node().style.width = '';

        // Need to reset width on table, not sure why.
        $.fn.dataTable.tables({ visible: true, api: true }).table().node().style.width = '';

        // .draw() is necessary for "no results" message to be properly positioned
        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust().draw();


    });

    FlatInvestTable.on('click', '#btnDelete', function (e) {
        e.preventDefault();
        $('#Production_Loss').html("");
        EditorFlatInvest.remove($(this).closest('tr'), {
            title: '刪除',
            message: '你確定要刪除?',
            buttons: '確定'
        });
    });

    FlatProductionDataTables.on('click', '#btnEdit', function (e) {
        e.preventDefault();
        $('#Production_Loss').html("");
        EditorFlatProduction.edit($(this).closest('tr'), {
            title: '編輯',
            buttons: '確定'
        });
        EditorFlatProduction.on('preSubmit', function (e, d) {
            var Roll_Ream_Wt = this.field('Roll_Ream_Wt');
            var Weight = this.field('Weight');

            if (Weight.val() === '') {
                this.field('Weight').error('請勿空白');
                return false;
            }

            if (Roll_Ream_Wt.val() === '') {
                this.field('Roll_Ream_Wt').error('請勿空白');
                return false;
            }

            return true;
        });

    });

    FlatProductionDataTables.on('click', '#btnDeleteFlatProductionTable', function (e) {
        $('#Production_Loss').html("");
        e.preventDefault();
        EditorFlatProduction.remove($(this).closest('tr'), {
            title: '刪除',
            message: '你確定要刪除?',
            buttons: '確定'
        });
    });


})





function init() {
    //交換事件
    $('#Flat_Invest_Barcode').change(function (e) {
        var Process_Detail_Id = $("#Process_Detail_Id").val()
        var Barcode = $('#Flat_Invest_Barcode').val()
        $.ajax({
            url: '/Process/Barcode',
            type: 'post',
            datatype: 'json',
            data: { Barcode: Barcode, Process_Detail_Id: Process_Detail_Id },
            success: function (data) {
                if (data.result == false) {
                    swal.fire("條碼無資料");
                    FlatClearText();
                } else {
                    FlatDispalyText(data);
                }
            },
            error: function () {
                swal.fire("失敗");
            }
        });

    });

    $("#Flat_Invest_Barcode").keyup(function (event) {
        if (event.keyCode === 13) {
            $("#BtnFlat_ProcessSave").click();
        }
    });

    //在關鍵字input按下Enter，執行n送出 
    $("#Flat_Production_Barcode").keyup(function (event) {
        if (event.keyCode === 13) {
            $('#BtnFlatBarcodeSave').click();
        }
    });


    $("#Process_Batch_no").keyup(function (event) {
        if (event.keyCode === 13) {
            $("#BtnOrderNumber").click();
        }
    });


}

function onBtnClick() {

    $('#BtnProcess_Batch_no').click(function (e) {
        var ProcessBatchNo = $('#ProcessBatchNo').val();
        $.ajax({
            url: '/Process/CheckOrderNumber',
            datatype: 'json',
            type: "POST",
            data: { ProcessBatchNo: ProcessBatchNo },
            success: function (data) {
                if (data.boolean) {
                    DisplayInvestFlatEnable(false);
                } else {
                    swal.fire("訂單輸入不對請重新輸入");
                }
            },
            error: function () {

            }
        });

    });


    //投出驗證單號
    $('#BtnProcess_Production_Batch_no').click(function (e) {
        var ProcessBatchNo = $('#ProcessProductionBatchNo').val();
        $.ajax({
            url: '/Process/CheckOrderNumber',
            datatype: 'json',
            type: "POST",
            data: { ProcessBatchNo: ProcessBatchNo },
            success: function (data) {
                if (data.boolean) {
                    DisplayProductionFlatEnable(false);
                } else {
                    swal.fire("訂單輸入不對請重新輸入");
                }
            },
            error: function () {

            }
        });

    });



    $('#BtnFlat_ProcessSave').click(function () {
        var Barcode = $('#Flat_Invest_Barcode').val().trim();
        var Process_Detail_Id = $("#Process_Detail_Id").val();
        if (Barcode.length == 0) {
            swal.fire("投入條碼不得空白");
            return;
        }

        FlatInvestSaveBarcode(Barcode, Process_Detail_Id);
    });


    //產出產生明細
    $('#BtnFlat_Product_detail').click(function () {
        var Flat_Production_Roll_Ream_Qty = $('#Flat_Production_Roll_Ream_Qty').val().trim();
        var Flat_Production_Roll_Ream_Wt = $('#Flat_Production_Roll_Ream_Wt').val().trim();
        var FlatInvestDataTables = $('#FlatInvestDataTables').DataTable().data();
        var Flat_Product_Item = $('#Flat_Product_Item').text();
        var Process_Detail_Id = $("#Process_Detail_Id").val();


        if (FlatInvestDataTables.length == 0) {
            swal.fire("請先新增投入條碼。");
            return;
        }

        if (Flat_Production_Roll_Ream_Qty.length == 0) {
            swal.fire("棧板數不得空白");
            return;
        }
        if (Flat_Production_Roll_Ream_Wt.length == 0) {
            swal.fire("令數不得空白");
            return;
        }

        FlatProductionDetail(Flat_Production_Roll_Ream_Qty, Flat_Production_Roll_Ream_Wt, Flat_Product_Item, Process_Detail_Id);

    })

    $('#BtnFlatBarcodeSave').click(function () {
        var Production_Barcode = $('#Flat_Production_Barcode').val();
        var table = $('#FlatProductionDataTables').DataTable();
        var Process_Detail_Id = $("#Process_Detail_Id").val();
        if (Production_Barcode.length == 0) {
            swal.fire("條碼請勿空白。");
            return
        }
        if (table.data().length == 0) {
            swal.fire("表格無資料，請先輸入資料。");
            return
        }
        ChangeFlatProductionStauts(Production_Barcode, Process_Detail_Id);
        $('#Flat_Production_Barcode').select();
    })

    //計算損耗
    $('#BtnFlatCalculate').click(function () {
        var Process_Detail_Id = $("#Process_Detail_Id").val();
        var table = $('#FlatProductionDataTables').DataTable();
        if (table.data().length == 0) {
            swal.fire("表格無資料，請先輸入資料。");
            return
        }
        var FlatProduction = $('#FlatProductionDataTables').DataTable().column(7).data();
        var FlatInvestDataTables = $('#FlatInvestDataTables').DataTable().column(3).data();

        for (i = 0; i < FlatProduction.length; i++) {
            if (FlatProduction[i] == "待入庫") {
                Swal.fire({
                    title: '產出資料錯誤',
                    text: "尚有資料未入庫",
                    icon: 'warning',
                    showCancelButton: false,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: '確定',
                    cancelButtonText: '取消'
                });
                return;
            }
        }
     
        var OriginalWeight = 0;
        //投入重量
        for (i = 0; i < FlatInvestDataTables.length; i++) {
            OriginalWeight += parseFloat(FlatInvestDataTables[i]);
        }
        var InvestWeight = OriginalWeight;




        //訂單總重
        var OrderWeight = $('#CHP_PROCESS_T_Weight').val();
        var ProductWeight = parseFloat(OrderWeight*100);
        var TotalWeight = (ProductWeight / InvestWeight).toFixed(2);
        var Percentage = ((InvestWeight / ProductWeight) * 100).toFixed(2);
        $.ajax({
            "url": "/Process/Loss",
            "type": "POST",
            "datatype": "json",
            "data": { TotalWeight: TotalWeight, Percentage: Percentage, Process_Detail_Id: Process_Detail_Id },
            success: function () {
                $('#Production_Loss').html("重量(KG)&ensp;" + TotalWeight + "&emsp;得率" + Percentage + "%");
            }
        });


    });

    //返回
    $('#BtnReturn').click(function () {
        window.location.href = '/Process/Index';

    });

    $('#BtnSave').click(function () {
        var loss = $('#Production_Loss').text();
        var FlatProduction = $('#FlatProductionDataTables').DataTable().column(7).data();
        var FlatInvestDataTables = $('#FlatInvestDataTables').DataTable().column(3).data();
        var Process_Batch_no = $('#Flat_Process_Batch_No').text()

        for (i = 0; i < FlatProduction.length; i++) {
            if (FlatProduction[i] == "待入庫") {
                Swal.fire({
                    title: '產出資料錯誤',
                    text: "尚有資料未入庫",
                    icon: 'warning',
                    showCancelButton: false,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: '確定',
                    cancelButtonText: '取消'
                });
                return;
            }
        }

        if (loss.length == 0) {
            swal.fire("損耗量未計算");
            return;
        }


        $.ajax({
            url: '/Process/_Subinventory/',
            type: 'POST',
            success: function (result) {
                $('body').append(result);
                Open($('#SubinventoryModal'), Process_Batch_no);
            }
        });

    });

    //列印標籤紙捲
    $('#BtnRePrint').click(function () {
        PrintLable(FlatInvestTable, "/Home/GetLabel", "1");
    });


    //列印標籤紙捲
    $('#BtnLabel').click(function () {
        PrintLable(FlatProductionDataTables, "/Home/GetLabel", "1");
        PrintLable(CotangentDataTable, "/Home/GetLabel", "1");
    });


}

//投入初始化table
function LoadFlatInvestTable() {

    EditorFlatInvest = new $.fn.dataTable.Editor({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        ajax: {
            url: '/Process/InvestEdit',
            type: "POST",
            datatype: "json",
            contentType: 'application/json',
            data: function (d) {
                var InvestList = [];
                $.each(d.data, function (key, value) {

                    var Invest = {
                        'Invest_Id': d.data[key]['Invest_Id'],
                        'Remnant': d.data[key]['Remnant'],
                        'Remaining_Weight': d.data[key]['Remaining_Weight']
                    }
                    InvestList.push(Invest);
                });

                var data = {
                    'Action': d.action,
                    'InvestList': InvestList
                }

                return JSON.stringify(data);
            },
            success: function () {
                //LoadFlatProductionDataTable();
            }
        },
        table: "#FlatInvestDataTables",
        idSrc: 'Invest_Id',
        fields: [
            {
                name: "Invest_Id"
            },
        ],
    });

    EditorFlatInvest.hide('Invest_Id');


    var Process_Detail_Id = $("#Process_Detail_Id").val()
    FlatInvestTable = $('#FlatInvestDataTables').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        destroy: true,
        //processing: true,
        //serverSide: true,
        //scrollX: true,
        autoWidth: false,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Process/InvestLoadTable",
            "type": "POST",
            "datatype": "json",
            "data": { Process_Detail_Id: Process_Detail_Id }
        },
        select: {
            style: 'multi',
            selector: 'td:first-child'
        },
        buttons: [
            'selectAll',
            'selectNone'
        ],
        columnDefs: [{
            orderable: false, targets: [0, 4], width: "60px",
        }],
        columns: [
            {
                data: null,
                defaultContent: '',
                className: 'select-checkbox',
                orderable: false,
                targets: 0
            },
            { data: "Barcode", "name": "條碼號", "autoWidth": true, "className": "dt-body-center"},
            { data: "Item_No", "name": "料號", "autoWidth": true, "className": "dt-body-left"},
            { data: "Ream_Qty", "name": "令數(RE)", "autoWidth": true, "className": "dt-body-right"},
            {
                data: "", "autoWidth": true, "render": function (data) {
                    return '<button class = "btn btn-danger btn-sm" id = "btnDelete">刪除</button>';
                }
            }
        ],

    });
}

//投入條碼儲存
function FlatInvestSaveBarcode(Barcode, Process_Detail_Id) {
    $.ajax({
        "url": "/Process/InvestTable",
        "type": "POST",
        "datatype": "json",
        "data": { Barcode: Barcode, Process_Detail_Id: Process_Detail_Id},
        success: function (data) {
            if (data.check == 0) {
                LoadFlatInvestTable();
            } else if (data.check == 1) {
                swal.fire("條碼資料已存在");
            } else if (data.check == 2) {
                swal.fire("資料不存在");
            }
        }


    });

}

//產出初始化table
function LoadFlatProductionDataTable() {

    EditorFlatProduction = new $.fn.dataTable.Editor({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        ajax: {
            url: '/Process/ProductionEdit',
            type: "POST",
            datatype: "json",
            contentType: 'application/json',
            data: function (d) {
                var ProductionList = [];
                $.each(d.data, function (key, value) {

                    var Production = {
                        'Production_Id': d.data[key]['Production_Id'],
                        'Roll_Ream_Wt': d.data[key]['Roll_Ream_Wt'],
                        'Weight': d.data[key]['Weight']

                    }
                    ProductionList.push(Production);
                });

                var data = {
                    'Action': d.action,
                    'ProductionList': ProductionList
                }

                return JSON.stringify(data);
            },
            success: function () {
                //LoadFlatProductionDataTable();
            }
        },
        table: "#FlatProductionDataTables",
        idSrc: 'Production_Id',
        fields: [
            {
                label: "重量",
                name: "Weight"
            },
            {
                name: "Production_Id"
            },
            {
                label: "令數",
                name: "Roll_Ream_Wt"
            }
        ],
    });

    EditorFlatProduction.field('Production_Id').hide();


    var Process_Detail_Id = $("#Process_Detail_Id").val()
    FlatProductionDataTables = $('#FlatProductionDataTables').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        destroy: true,
        //processing: true,
        //serverSide: true,
        //scrollX: true,
        autoWidth: false,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Process/ProductionLoadDataTables",
            "type": "POST",
            "datatype": "json",
            "data": { Process_Detail_Id: Process_Detail_Id },
        },
        select: {
            style: 'multi',
            selector: 'td:first-child'
        },
        buttons: [
            'selectAll',
            'selectNone'
        ],
        columnDefs: [{
            orderable: false, targets: [0, 8], width: "60px",
        }],
        columns: [
            {
                data: null,
                defaultContent: '',
                className: 'select-checkbox',
                orderable: false,
                targets: 0
            },
            { data: "Barcode", "name": "條碼號", "autoWidth": true, "className": "dt-body-center"},
            { data: "Item_No", "name": "料號", "autoWidth": true, "className": "dt-body-left" },
            { data: "Weight", "name": "重量", "autoWidth": true, "className": "dt-body-right" },
            {
                data: "", "name": "單位", "autoWidth": true, "className": "dt-body-center", render: function () {
                    return 'KG';
                }
            },
            { data: "Roll_Ream_Wt", "name": "令數", "autoWidth": true, "className": "dt-body-right"},
            {
                data: "", "name": "單位", "autoWidth": true, "className": "dt-body-center", render: function () {
                    return 'RE';
                }
            },
            {
                data: "Status", "name": "狀態", "autoWidth": true, "className": "dt-body-center", "render": function (data) {
                    if (data == "已入庫") {
                        return data;
                    } else {
                        return '<span style="color:red">' + data + '</span>';
                    }
                }
            },
            {
                data: "", "autoWidth": true, "render": function (data) {
                    return '<button class = "btn btn-primary btn-sm" id = "btnEdit">編輯</button>' +
                        '&nbsp|&nbsp'+ '<button class = "btn btn-danger btn-sm" id = "btnDeleteFlatProductionTable">刪除</button>';
                }
            }
        ],

    });

}

//產生明細
function FlatProductionDetail(Production_Roll_Ream_Qty, Production_Roll_Ream_Wt, Product_Item, Process_Detail_Id) {

    $.ajax({
        "url": "/Process/ProductionDetail",
        "type": "POST",
        "datatype": "json",
        "data": {
            Production_Roll_Ream_Qty: Production_Roll_Ream_Qty,
            Production_Roll_Ream_Wt: Production_Roll_Ream_Wt,
            Product_Item: Product_Item,
            Process_Detail_Id: Process_Detail_Id
        },
        success: function (data) {
            if (data.boolean) {
                LoadFlatProductionDataTable();
            } else {
                swal.fire("")
            }
        }
    })

}

function ChangeFlatProductionStauts(Production_Barcode, Process_Detail_Id) {
    $.ajax({
        "url": "/Process/ProductionChangeStatus",
        "type": "POST",
        "datatype": "json",
        "data": { Production_Barcode: Production_Barcode, Process_Detail_Id: Process_Detail_Id },
        success: function (data) {
            if (data.check == 1) {
                swal.fire("此條碼已入庫");
            } else if (data.check == 2) {
                LoadFlatProductionDataTable();
            } else if (data.check == 3) {
                swal.fire("無條碼資料");
            }
        }
    })
}


function FlatDispalyText(data) {
    $('#Flat_Invest_Item_No').html(data.cpd.Item_No);
}

function FlatClearText() {
    $('#Flat_Invest_Item_No').html("");
    $('#Flat_Invest_Barcode').val('');
}

function EnableBarcode(boolean) {
    $('#Flat_Invest_Barcode').attr('disabled', boolean);
    $('#BtnFlat_Product_detail').attr('disabled', boolean);

}


//彈出dialog
function Open(modal_dialog, Process_Batch_no) {
    modal_dialog.modal({
        backdrop: "static",
        keyboard: true,
        show: true
    });

    modal_dialog.on('hidden.bs.modal', function (e) {
        $("div").remove(modal_dialog.selector);
    });

    modal_dialog.on('show.bs.modal', function (e) {
        $.validator.unobtrusive.parse('form');
    });

    //確認按鍵
    modal_dialog.on('click', '#btnSaveSubinventory', function (e) {
        var Subinventory = $('#dialg_Subinventory').val();



        $.ajax({
            url: '/Process/ChagneIndexStatus/',
            dataType: 'json',
            type: 'post',
            data: { Process_Batch_no: Process_Batch_no, Subinventory: Subinventory },
            success: function (e) {
                window.location.href = '/Process/Index';
            }
        })



    });



    modal_dialog.modal('show');

}
//完工紀錄使用
function BtnRecord() {
    $('#BtnEdit').click(function () {
        var ProcessBatchNo = $('#ProcessBatchNo').val();
        $.ajax({
            url: '/Process/CheckOrderNumber',
            datatype: 'json',
            type: "POST",
            data: { ProcessBatchNo: ProcessBatchNo },
            success: function (data) {
                if (data.boolean) {
                    DisplayInvestFlatEnable(false);
                    changeStaus();
                } else {
                    swal.fire("訂單輸入不對請重新輸入");
                }
            },
            error: function () {

            }
        });
    });

    $('#BtnApprove').click(function () {
        var Process_Batch_no = $('#Flat_Process_Batch_No').text()
        var Status = "核准";
        $.ajax({
            url: '/Process/ChagneIndexStatus/',
            dataType: 'json',
            type: 'post',
            data: { Process_Batch_no: Process_Batch_no, Status: Status },
            success: function (e) {
                window.location.href = '/Process/Index';
            }
        });


    });




}



function DisplayInvestFlatEnable(boolean) {
    $('#Flat_Invest_Barcode').attr('disabled', boolean);
    $('#BtnFlat_ProcessSave').attr('disabled', boolean);


    
}

function DisplayProductionFlatEnable(boolean) {
    $('#Flat_Production_Roll_Ream_Qty').attr('disabled', boolean);
    $('#Flat_Production_Roll_Ream_Wt').attr('disabled', boolean);
    $('#BtnFlat_Product_detail').attr('disabled', boolean);
    $('#Flat_Production_Barcode').attr('disabled', boolean);
    $('#BtnFlatBarcodeSave').attr('disabled', boolean);
    $('#BtnFlatCalculate').attr('disabled', boolean);


}

function DsiplayFlatHide() {
    $('#BtnProcess_Batch_no').hide()
    $("#BtnSave").hide();
    $('#BtnEdit').show()
    $('#BtnApprove').show()

}

function DsiplayFlatShow() {
    $('#BtnProcess_Batch_no').show()
    $('#BtnEdit').hide()
    $('#BtnApprove').hide()
}


//完工紀錄使用
function changeStaus() {
    var Process_Batch_no = $('#ProcessBatchNo').val();
    var Status = "完工紀錄";
    $.ajax({
        url: '/Process/ChagneIndexStatus/',
        dataType: 'json',
        type: 'post',
        data: { Process_Batch_no: Process_Batch_no, Status: Status },
        success: function (e) {
            //window.location.href = '/Process/Index';
        }
    });
}