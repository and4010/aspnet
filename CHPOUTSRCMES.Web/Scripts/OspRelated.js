﻿
$(document).ready(function () {


    $("#ddlOrganization").combobox({
        select: function (event, ui) {
            updateDdlInventoryItem(this.value);

        }
    });

    $("#ddlInventoryItem").combobox({
        select: function (event, ui) {
            var ORGANIZATION_ID = $("#ddlOrganization").val();
            updateDdlRelatedItem(ORGANIZATION_ID, this.value);

        }
    });


    //$('#ddlInventoryItem').combobox({
    //    onSelect: function (item){
    //        alert(item);
    //    }   
    //});

    //$("#ddlInventoryItem").onChange = function (newValue, oldValue) {
    //    alert(newValue);
    //}

    $("#ddlRelatedItem").combobox();


    var OspRelatedDataTablesBody = $('#OspRelatedDataTablesBody').DataTable({
        //"scrollX": true,
        language: {
            url: '/bower_components/datatables/language/zh-TW.json'
        },
        //autoWidth: false,
        //pageLength: 3,
        serverSide: true,
        processing: true,
        deferLoading: 0, //初始化DataTable時，不發出ajax
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/OspRelated/GetOspRelatedDT",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.ORGANIZATION_ID = $("#ddlOrganization").val();
                d.INVENTORY_ITEM_ID = $("#ddlInventoryItem").val();
                d.RELATED_ITEM_ID = $("#ddlRelatedItem").val();
            },
        },
        columns: [
            { data: "ITEM_NUMBER", name: "組成成分料號", "autoWidth": true },
            { data: "ITEM_DESCRIPTION", name: "組成成分料號描述", "autoWidth": true },
            { data: "RELATED_ITEM_NUMBER", name: "餘切料號", "autoWidth": true },
            { data: "RELATED_ITEM_DESCRIPTION", name: "餘切料號描述", "autoWidth": true },
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

            { data: "LAST_UPDATED_BY", name: "更新人員", "autoWidth": true },

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
            }
        ]
        //"order": [[1, 'asc']],
        //buttons: [],

    });

    $('.row-std').on('click', '#btnSearch', function (e) {

        OspRelatedDataTablesBody.ajax.reload();
        return false;

        //$.ajax({
        //    url: "/OspRelated/Search",
        //    type: "post",
        //    data: {
        //        INVENTORY_ITEM_ID: $("#ddlInventoryItem").val(),
        //        RELATED_ITEM_ID: $("#ddlRelatedItem").val(),

        //    },
        //    success: function (data) {
        //        if (data.status) {
        //            if (data.result == "搜尋成功") {
        //                OspRelatedDataTablesBody.ajax.reload();
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


    function updateDdlInventoryItem(ORGANIZATION_ID) {
        var ddl = $("#ddlInventoryItem");

        $.ajax({
            url: "/OspRelated/GetInventoryItemList",
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
                swal.fire('更新組成成份料號選單失敗');
            },
            complete: function () {
                var INVENTORY_ITEM_ID = $("#ddlInventoryItem").val();
                updateDdlRelatedItem(ORGANIZATION_ID, INVENTORY_ITEM_ID);
            }

        });
    }


    function updateDdlRelatedItem(ORGANIZATION_ID, INVENTORY_ITEM_ID) {
        var ddl = $("#ddlRelatedItem");

        $.ajax({
            url: "/OspRelated/GetRelatedItemList",
            type: "post",
            data: {
                ORGANIZATION_ID: ORGANIZATION_ID,
                INVENTORY_ITEM_ID: INVENTORY_ITEM_ID
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
                swal.fire('更新餘切料號選單失敗');
            },
            complete: function (data) {


            }

        });

    }
});