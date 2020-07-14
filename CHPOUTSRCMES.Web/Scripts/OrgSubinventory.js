
$(document).ready(function () {

    $("#ddlOrganization").combobox({
        select: function (event, ui) {
            updateDdlSubinventory(this.value);
        }
    });

    $("#ddlSubinventory").combobox({
        select: function (event, ui) {
            var ORGANIZATION_ID = $("#ddlOrganization").val();
            updateDdlLocator(ORGANIZATION_ID, this.value);
        }
    });

    $("#ddlLocator").combobox();

    var OrgSubinventoryDataTablesBody = $('#OrgSubinventoryDataTablesBody').DataTable({
        //"scrollX": true,
        language: {
            url: '/bower_components/datatables/language/zh-TW.json'
        },
        autoWidth: false,
        //pageLength: 3,
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
                d.SUBINVENTORY_CODE = $("#ddlSubinventory").val();
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
         { data: "CREATED_BY_NAME", name: "建立人員", "autoWidth": true },
         {
             data: "CREATION_DATE", name: "建立日期", "autoWidth": true, "mRender": function (data, type, full) {
                 if (data != null) {
                     var dtStart = new Date(parseInt(data.substr(6)));
                     var dtStartWrapper = moment(dtStart);
                     return dtStartWrapper.format('YYYY-MM-DD');
                 } else {
                     return '';
                 }
             }
         },
         
         { data: "LAST_UPDATED_BY_NAME", name: "更新人員", "autoWidth": true },
         
        {
        data: "LAST_UPDATE_DATE", name: "更新日期", "autoWidth": true, "mRender": function (data, type, full) {
            if (data != null) {
                var dtStart = new Date(parseInt(data.substr(6)));
                var dtStartWrapper = moment(dtStart);
                return dtStartWrapper.format('YYYY-MM-DD');
            } else {
                return '';
            }
        }
    },



        ],

        //"order": [[1, 'asc']],
        //buttons: [],

    });

    $('.row-std').on('click', '#btnSearch', function (e) {

        OrgSubinventoryDataTablesBody.ajax.reload();
        return false;

        //$.ajax({
        //    url: "/OrgSubinventory/Search",
        //    type: "post",
        //    data: {
        //        ORGANIZATION_ID: $("#ddlOrganization").val(),
        //        SUBINVENTORY_CODE: $("#ddlSubinventory").val(),
        //        LOCATOR_ID: $("#ddlLocator").val()
        //    },
        //    success: function (data) {
        //        if (data.status) {
        //            if (data.result == "搜尋成功") {
        //                OrgSubinventoryDataTablesBody.ajax.reload();
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


    function updateDdlSubinventory(ORGANIZATION_ID) {
        var ddl = $("#ddlSubinventory");

        $.ajax({
            url: "/OrgSubinventory/GetSubinventoryList",
            type: "post",
            data: {
                ORGANIZATION_ID: ORGANIZATION_ID
            },
            success: function (data) {
                ddl.html("");

                for (var i = 0; i < data.length; i++) {
                    ddl.append($('<option></option>').val(data[i].Value).html(data[i].Text));
                }

                var optionCount = ddl[0].length;
                if (optionCount == 2) {
                    //選單數量為2時，選擇第2個
                    ddl.combobox('autocomplete', ddl[0][1].value, ddl[0][1].text);
                } else {
                    ddl.combobox('autocomplete', ddl[0][0].value, ddl[0][0].text);
                }

            },
            error: function () {
                swal.fire('更新倉庫選單失敗');
            },
            complete: function () {
                var SUBINVENTORY_CODE = $("#ddlSubinventory").val();
                updateDdlLocator(ORGANIZATION_ID, SUBINVENTORY_CODE);
            }

        });

    }

    function updateDdlLocator(ORGANIZATION_ID, SUBINVENTORY_CODE) {
        var ddl = $("#ddlLocator");

        $.ajax({
            url: "/OrgSubinventory/GetLocatorList",
            type: "post",
            data: {
                ORGANIZATION_ID: ORGANIZATION_ID,
                SUBINVENTORY_CODE: SUBINVENTORY_CODE
            },
            success: function (data) {
                ddl.html("");

                for (var i = 0; i < data.length; i++) {
                    ddl.append($('<option></option>').val(data[i].Value).html(data[i].Text));
                }

                var optionCount = ddl[0].length;
                if (optionCount == 2) {
                    //選單數量為2時，選擇第2個
                    ddl.combobox('autocomplete', ddl[0][1].value, ddl[0][1].text);
                } else {
                    ddl.combobox('autocomplete', ddl[0][0].value, ddl[0][0].text);
                }

            },
            error: function () {
                swal.fire('更新儲位選單失敗');
            },
            complete: function () {
                
            }

        });

    }

});