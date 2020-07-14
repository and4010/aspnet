$(document).ready(function () {
    onclick();
    LoadTable();
});

function LoadTable(Catalog_elem_val_050, Catalog_elem_val_020, Catalog_elem_val_070, Organization_code) {

    $('#PartNoTable').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        processing: true,
        serverSide: true,
        //scrollX: true,
        autoWidth: false,
        destroy:true,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        "lengthMenu": [[200, 250, 300, 350], [200, 250, 300, 350]],
        ajax: {
            "url": "/PartNo/PartNoJson",
            "type": "POST",
            "datatype": "json",
            "data": { "Catalog_elem_val_050": Catalog_elem_val_050, "Catalog_elem_val_020" : Catalog_elem_val_020, "Catalog_elem_val_070" : Catalog_elem_val_070, "Organization_code" : Organization_code }
        },
        buttons: [
            {
                extend: 'excel',
                text: '匯出Excel',
                className: 'btn-primary'
            },
        ],
        columns: [
            { data: "Category_code_inv", "name": "存貨分類", "autoWidth": true, "className": "dt-body-center"},
            { data: "Category_code_cost", "name": "成本分類", "autoWidth": true, "className": "dt-body-center"},
            { data: "Item_number", "name": "料號", "autoWidth": true, "className": "dt-body-left"},
            { data: "Item_desc_eng", "name": "英文摘要", "autoWidth": true, "className": "dt-body-center"},
            { data: "Item_desc_tch", "name": "中文摘要", "autoWidth": true, "className": "dt-body-center"},
            { data: "Primary_uom_code", "name": "主要單位", "autoWidth": true, "className": "dt-body-center"},
            { data: "Secondary_uom_code", "name": "次要單位", "autoWidth": true, "className": "dt-body-center"},
            { data: "Item_type", "name": "料號狀態", "autoWidth": true, "className": "dt-body-center"},
            { data: "Catalog_elem_val_010", "name": "大紙別", "autoWidth": true, "className": "dt-body-center"},
            { data: "Catalog_elem_val_020", "name": "紙別簡稱", "autoWidth": true, "className": "dt-body-center"},
            { data: "Catalog_elem_val_030", "name": "料件等級", "autoWidth": true, "className": "dt-body-center"},
            { data: "Catalog_elem_val_040", "name": "基重", "autoWidth": true, "className": "dt-body-right"},
            { data: "Catalog_elem_val_050", "name": "規格", "autoWidth": true, "className": "dt-body-center"},
            { data: "Catalog_elem_val_060", "name": "令重", "autoWidth": true, "className": "dt-body-right"},
            { data: "Catalog_elem_val_070", "name": "捲筒/平版", "autoWidth": true, "className": "dt-body-center"},
            { data: "Catalog_elem_val_090", "name": "市場常規", "autoWidth": true, "className": "dt-body-center"},
            { data: "Catalog_elem_val_110", "name": "令包/無令打件", "autoWidth": true, "className": "dt-body-center"},
            { data: "Catalog_elem_val_130", "name": "紙別中文名稱", "autoWidth": true, "className": "dt-body-center"},
            { data: "Created_by", "name": "建立人員ID", "autoWidth": true, "className": "dt-body-center"},
            {
                data: "Creation_Date", "name": "建立日期", "autoWidth": true, "mRender": function (data, type, full) {
                    if (data != null) {
                        var dtStart = new Date(parseInt(data.substr(6)));
                        var dtStartWrapper = moment(dtStart);
                        return dtStartWrapper.format('YYYY-MM-DD');
                    } else {
                        return '';
                    }
                }, "className": "dt-body-center"
            },
            { data: "Last_Updated_by", "name": "異動人員", "autoWidth": true, "className": "dt-body-center"},
            {
                data: "Last_Update_Date", "name": "異動日期", "autoWidth": true, "mRender": function (data, type, full) {
                    if (data != null) {
                        var dtStart = new Date(parseInt(data.substr(6)));
                        var dtStartWrapper = moment(dtStart);
                        return dtStartWrapper.format('YYYY-MM-DD');
                    } else {
                        return '';
                    }


                }, "className": "dt-body-center"
            },
        ],

    });
}


function onclick() {
    $('#btnSearch').click(function () {

        var Catalog_elem_val_050 = $("#Catalog_elem_val_050").val();
        var Catalog_elem_val_020 = $("#Catalog_elem_val_020").val();
        var Catalog_elem_val_070 = $("#Catalog_elem_val_070").val();
        var Organization_code = $("#Organization_code").val();


        LoadTable(Catalog_elem_val_050, Catalog_elem_val_020, Catalog_elem_val_070, Organization_code);


    });
}