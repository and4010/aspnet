var RecordInvestDataTables
var RecordProductionDataTables
var CotangentDataTable

var EditorInvenst
var EditorProduction
var EditorCotangent

$(document).ready(function () {

   
    LoadRecordInvestTable();
    LoadRecordProductionDataTable();
    if ($("#Process_Detail_Id").val() == 1) {
        LoadRecordCotangentDataTables();
    }

    RecordInvestDataTables.on('select', function (e, dt, items, indexes) {
        var rowData = RecordInvestDataTables.rows(indexes).data();
        displaytext(rowData)
    });

    RecordInvestDataTables.on('deselect', function (e, dt, items, indexes) {
        clear()
    });

    btnClick();
})



function LoadRecordInvestTable() {
    var Process_Detail_Id = $("#Process_Detail_Id").val()
    RecordInvestDataTables = $('#RecordInvestDataTables').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        lengthMenu: [[25, 50, 100, 200], [25, 50, 100, 200]],
        select:true,
        destroy: true,
        autoWidth: false,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Process/InvestLoadTable",
            "type": "POST",
            "datatype": "json",
            "data": { Process_Detail_Id }
        },
        select: {
            style: 'multi',
            selector: 'td:first-child'
        },
        columnDefs: [{
            orderable: false, targets: [0], width: "60px",
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
            { data: "Remnant", "name": "殘捲", "autoWidth": true, "className": "dt-body-center"},
            { data: "Basic_Weight", "name": "基重", "autoWidth": true, "className": "dt-body-right"},
            { data: "Specification", "name": "寬幅", "autoWidth": true, "className": "dt-body-right"},
            { data: "Lot_Number", "name": "捲號", "autoWidth": true, "className": "dt-body-center"},
            { data: "Paper_Type", "name": "紙別", "autoWidth": true, "className": "dt-body-center"},
            { data: "Original_Weight", "name": "原重", "autoWidth": true, "className": "dt-body-right"},
            { data: "Remaining_Weight", "name": "餘重", "autoWidth": true, "className": "dt-body-right"},
            {
                data: "", "autoWidth": true, "render": function (data) {
                    return '<button class="btn btn-primary btn-sm" id = "btnEdit">編輯</button>';
                }
            }
        ],

    });

}


function LoadRecordProductionDataTable() {
    var Process_Detail_Id = $("#Process_Detail_Id").val()
    RecordProductionDataTables = $('#RecordProductionDataTables').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        lengthMenu: [[25, 50, 100, 200], [25, 50, 100, 200]],
        destroy: true,
        autoWidth: false,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Process/ProductionLoadDataTables",
            "type": "POST",
            "datatype": "json",
            "data": { Process_Detail_Id },
        },
        select: {
            style: 'multi',
            selector: 'td:first-child'
        },
        columnDefs: [{
            orderable: false, targets: [0], width: "60px",
        }],
        columns: [
            {
                data: null,
                defaultContent: '',
                className: 'select-checkbox',
                orderable: false,
                targets: 0,
                autoWidth : true,
            },
            { data: "Barcode", "name": "條碼號", "autoWidth": true, "className": "dt-body-center"},
            { data: "Item_No", "name": "料號", "autoWidth": true, "className": "dt-body-left"},
            { data: "Roll_Ream_Wt", "name": "令數", "autoWidth": true, "className": "dt-body-right"},
            { data: "Weight", "name": "重量", "autoWidth": true, "className": "dt-body-center"},
            { data: "Status", "name": "狀態", "autoWidth": true },
            {
                data: "", "autoWidth": true, "render": function (data) {
                    return '<button class = "btn btn-danger btn-sm" id = "btnEdit">編輯</button>';
                }
            }
        ],

    });
}

function LoadRecordCotangentDataTables() {
    CotangentDataTable = $('#RecordCotangentDataTables').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        destroy: true,
        autoWidth: false,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Process/RecordCotangentDataTables",
            "type": "POST",
            "datatype": "json",
            "data": { },
        },
        select: {
            style: 'multi',
            selector: 'td:first-child'
        },
        columnDefs: [{
            orderable: false, targets: [0], width: "60px",
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
            { data: "Related_item", "name": "料號", "autoWidth": true, "className": "dt-body-left"},
            { data: "Cotangent_Ttl_Roll_Ream", "name": "令數", "autoWidth": true, "className": 'editable', "className": "dt-body-right"},
            { data: "Kg", "name": "公斤", "autoWidth": true, "className": "dt-body-center"},
        ],
    });
}


function btnClick() {
    $('#BtnEdit').click(function () {
       

    });
}









function displaytext(rowData) {
    var Barcode = rowData.pluck('Barcode')[0]
    var Invest_Remnant = rowData.pluck('Remnant')[0]
    var Invest_Original_Weight = rowData.pluck('Original_Weight')[0]
    var Invest_Remaining_Weight = rowData.pluck('Remaining_Weight')[0]
    var Invest_Basic_Weight = rowData.pluck('Basic_Weight')[0]
    var Invest_Specification = rowData.pluck('Specification')[0]
    var Invest_Lot_Number = rowData.pluck('Lot_Number')[0]



    $('#Record_Invest_Barcode').text(Barcode);
    $('#Record_Invest_Remnant').text(Invest_Remnant);
    $('#Record_Invest_Original_Weight').text(Invest_Original_Weight);
    $('#Record_Invest_Remaining_Weight').text(Invest_Remaining_Weight);
    $('#Record_Invest_Basic_Weight').text(Invest_Basic_Weight);
    $('#Record_Invest_Specification').text(Invest_Specification);
    $('#Record_Invest_Lot_Number').text(Invest_Lot_Number);

}

function clear() {
    $('#Record_Invest_Barcode').text("");
    $('#Record_Invest_Remnant').text("");
    $('#Record_Invest_Original_Weight').text("");
    $('#Record_Invest_Remaining_Weight').text("");
    $('#Record_Invest_Basic_Weight').text("");
    $('#Record_Invest_Specification').text("");
    $('#Record_Invest_Lot_Number').text("");
}