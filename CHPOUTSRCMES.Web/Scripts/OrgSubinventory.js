
$(document).ready(function () {

    $("#ddlOrganization").change(function (event) {
        updateDdlSubinventory($(this).val());
    });

    $("#ddlSubinventory").change(function (event) {
        var ORGANIZATION_ID = $("#ddlOrganization").val();
        updateDdlLocator(ORGANIZATION_ID, $("#ddlSubinventory option:selected").text());
    });

    var OrgSubinventoryDataTablesBody = $('#OrgSubinventoryDataTablesBody').DataTable({
        //"scrollX": true,
        language: {
            url: '/bower_components/datatables/language/zh-TW.json'
        },
        autoWidth: false,
        serverSide: true,
        processing: true,
        deferLoading: 0, //初始化DataTable時，不發出ajax
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/OrgSubinventory/GetOrgSubinventoryDT",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.ORGANIZATION_ID = $("#ddlOrganization").val();
                d.SUBINVENTORY_CODE = $("#ddlSubinventory option:selected").text();
                d.LOCATOR_ID = $("#ddlLocator").val();
            },
        },
        columns: [
         { data: "ORGANIZATION_CODE", name: "庫存組織", "autoWidth": true },
         { data: "ORGANIZATION_NAME", name: "庫存組織名稱", "autoWidth": true },
         { data: "SUBINVENTORY_CODE", name: "倉庫", "autoWidth": true },
         { data: "SUBINVENTORY_NAME", name: "倉庫名稱", "autoWidth": true },
         { data: "OSP_FLAG", name: "加工廠", "autoWidth": true },
          { data: "LOCATOR_SEGMENTS", name: "儲位節段", "autoWidth": true },
         { data: "LOCATOR_DESC", name: "儲位描述", "autoWidth": true },

        ],

    });

    $('#btnSearch').click(function () {
        search();
        return false;
    });


    function updateDdlSubinventory(ORGANIZATION_ID) {
        var ddl = $("#ddlSubinventory");
        ShowWait(function () {
            $.ajax({
                url: "/OrgSubinventory/GetSubinventoryList",
                type: "post",
                data: {
                    ORGANIZATION_ID: ORGANIZATION_ID
                },
                success: function (data) {
                    CloseWait();
                    ddl.html("");

                    for (var i = 0; i < data.length; i++) {
                        ddl.append($('<option></option>').val(data[i].Value).html(data[i].Text));
                    }

                    var optionCount = ddl[0].length;
                    if (optionCount == 2) {
                        //選單數量為2時，選擇第2個
                        ddl.val(ddl[0][1].value);
                    } else {
                        ddl.val(ddl[0][0].value);
                    }

                },
                error: function () {
                    swal.fire('更新倉庫選單失敗');
                },
                complete: function () {
                    var SUBINVENTORY_CODE = $("#ddlSubinventory option:selected").text();
                    updateDdlLocator(ORGANIZATION_ID, SUBINVENTORY_CODE);
                }

            });
        });

    }

    function updateDdlLocator(ORGANIZATION_ID, SUBINVENTORY_CODE) {
        var ddl = $("#ddlLocator");

        ShowWait(function () {
            $.ajax({
                url: "/OrgSubinventory/GetLocatorList",
                type: "post",
                data: {
                    ORGANIZATION_ID: ORGANIZATION_ID,
                    SUBINVENTORY_CODE: SUBINVENTORY_CODE
                },
                success: function (data) {
                    CloseWait();
                    ddl.html("");

                    for (var i = 0; i < data.length; i++) {
                        ddl.append($('<option></option>').val(data[i].Value).html(data[i].Text));
                    }

                    var optionCount = ddl[0].length;
                    if (optionCount == 2) {
                        //選單數量為2時，選擇第2個
                        ddl.val(ddl[0][1].value);
                    } else {
                        ddl.val(ddl[0][0].value);
                    }

                },
                error: function () {
                    swal.fire('更新儲位選單失敗');
                },
                complete: function () {

                }

            });
        });
       

    }

    function search() {
        OrgSubinventoryDataTablesBody.ajax.reload();
    }

    search();

});