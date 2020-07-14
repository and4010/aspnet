$(document).ready(function () {

    LoadTable();

    onclcik();
});


function LoadTable(Organization_code) {
    $('#MachinePaperTypeTable').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        processing: true,
        serverSide: true,
        //scrollX: true,
        autoWidth: false,
        destroy:true,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        "lengthMenu": [[25, 50, 100, 150], [25, 50, 100, 150]],
        ajax: {
            "url": "/MachinePaperType/MachinePaperType",
            "type": "POST",
            "datatype": "json",
            "data": { Organization_code: Organization_code }
        },
        columns: [
            { data: "Machine_code", "name": "機台紙別代碼", "autoWidth": true, "className": "dt-body-center"},
            { data: "Machine_meaning", "name": "機台紙別意義", "autoWidth": true, "className": "dt-body-center"},
            { data: "Description", "name": "機台紙別摘要", "autoWidth": true, "className": "dt-body-center"},
            { data: "Paper_type", "name": "紙別", "autoWidth": true, "className": "dt-body-center"},
            { data: "Machine_num", "name": "機台", "autoWidth": true, "className": "dt-body-center"},
            { data: "Supplier_num", "name": "供應商編號", "autoWidth": true, "className": "dt-body-center"},
            { data: "Supplier_name", "name": "供應商名稱", "autoWidth": true, "className": "dt-body-center"},
            { data: "Created_by", "name": "建立人員ID", "autoWidth": true, "className": "dt-body-center"},
            {
                data: "Creation_date", "name": "建立日期", "autoWidth": true, "mRender": function (data, type, full) {
                    if (data != null) {
                        var dtStart = new Date(parseInt(data.substr(6)));
                        var dtStartWrapper = moment(dtStart);
                        return dtStartWrapper.format('YYYY-MM-DD');
                    } else {
                        return '';
                    }

                }
            },
            { data: "Last_updated_by", "name": "最後更新人員ID", "autoWidth": true },
            {
                data: "Last_update_date", "name": "最後更新日期", "autoWidth": true, "mRender": function (data, type, full) {
                    if (data != null) {
                        var dtStart = new Date(parseInt(data.substr(6)));
                        var dtStartWrapper = moment(dtStart);
                        return dtStartWrapper.format('YYYY-MM-DD');
                    } else {
                        return '';
                    }
                },
            }
        ],

    });
}

function onclcik() {
    $('#btnSearch').click(function () {
        var Organization_code = $("#Organization_code").val();
        LoadTable(Organization_code);
    });
}