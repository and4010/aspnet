
$(document).ready(function () {
    //setDefault("Asp");
    //function setDefault(defValue) {
    //    $('#ddlPstyp option').each(function () {
    //        if (($(this).attr('value')) === defValue) {
    //            $(this).attr('selected', 'selected');
    //        }
    //    });
    //}


    $("#ddlOrganization").combobox({
        select: function (event, ui) {
            updateDdlOspSubinventory(this.value);

        }
    });

    $("#ddlOspSubinventory").combobox({
        select: function (event, ui) {
            var ORGANIZATION_ID = $("#ddlOrganization").val();
            updateDdlPstyp(ORGANIZATION_ID, this.value);
        }
    });

    $("#ddlPstyp").combobox();

    
  

    var YszmpckqDataTablesBody = $('#YszmpckqDataTablesBody').DataTable({
        
        language: {
            url: '/bower_components/datatables/language/zh-TW.json'
        },
        //autoWidth: false,
        //"scrollY": "780px",
        //sScrollX: "100%",
        //sScrollXInner: "100%",
        //pageLength: 3,
        serverSide: true,
        processing: true,
        deferLoading: 0, //初始化DataTable時，不發出ajax
        
        //"scrollCollapse": true,
        
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Yszmpckq/GetYszmpckqDT",
            "type": "POST",
            "datatype": "json",
            "data": function (d)
            {
                d.ORGANIZATION_ID = $("#ddlOrganization").val();
                d.OSP_SUBINVENTORY = $("#ddlOspSubinventory").val();
                d.PSTYP = $("#ddlPstyp").val();
            },
        },
        columns: [
         { data: "ORGANIZATION_CODE", name: "庫存組織", "autoWidth": true },
         { data: "ORGANIZATION_NAME", name: "庫存組織名稱", "autoWidth": true },
         { data: "OSP_SUBINVENTORY", name: "加工廠", "autoWidth": true },
         { data: "OSP_SUBINVENTORY_NAME", name: "加工廠名稱", "autoWidth": true },
         { data: "PSTYP", name: "紙別", "autoWidth": true },
          { data: "PSTYP_CHT_NAME", name: "紙別中文名稱", "autoWidth": true },
          { data: "PSTYP_ENG_NAME", name: "紙別中文名稱", "autoWidth": true },
         { data: "BWETUP", name: "基重上限", "autoWidth": true },
         { data: "BWETDN", name: "基重下限", "autoWidth": true },
         { data: "RWTUP", name: "令重上限", "autoWidth": true },
         { data: "RWTDN", name: "令重下限", "autoWidth": true },
         { data: "PCKQ", name: "包數", "autoWidth": true },
         { data: "PAPER_QTY", name: "每包張數", "autoWidth": true },
         { data: "PIECES_QTY", name: "每件令數", "autoWidth": true },
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
        }
        ],

        //"order": [[1, 'asc']],
        //buttons: [],

    });

    
    $('.row-std').on('click', '#btnSearch', function (e) {

        //YszmpckqDataTablesBody.draw();
        YszmpckqDataTablesBody.ajax.reload();
        return false;
    });

    

    function updateDdlOspSubinventory(ORGANIZATION_ID) {
        var ddl = $("#ddlOspSubinventory");

        $.ajax({
            url: "/Yszmpckq/GetOspSubinventoryList",
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
                swal.fire('更新加工廠選單失敗');
            },
            complete: function () {
                var OSP_SUBINVENTORY_ID = $("#ddlOspSubinventory").val();
                updateDdlPstyp(ORGANIZATION_ID, OSP_SUBINVENTORY_ID);
            }

        });

    }

    function updateDdlPstyp(ORGANIZATION_ID, OSP_SUBINVENTORY_ID) {
        var ddl = $("#ddlPstyp");

        $.ajax({
            url: "/Yszmpckq/GetPstypList",
            type: "post",
            data: {
                ORGANIZATION_ID: ORGANIZATION_ID,
                OSP_SUBINVENTORY_ID: OSP_SUBINVENTORY_ID
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
                swal.fire('更新紙別選單失敗');
            },
            complete: function (data) {


            }

        });

    }


});