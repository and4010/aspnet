var editor
$(document).ready(function () {
    $("#ContainerNo").combobox();
    table();

    //click();

})


function click() {
    $('#BtnPartNoSearch').click(function (e) {
        var ContainerNo = $('#ContainerNo').val()
        var Subinventory = $('#Subinventory').val()
        var Locator = $('#Locator').val()

        if (ContainerNo == null) {
            swal.fire("櫃號不得空白")
            return
        }
        table(ContainerNo, Subinventory, Locator);
    });



}



function table() {

    editor = new $.fn.dataTable.Editor({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        ajax: {
            url: '/Reason/Editor',
            "type": "POST",
            "datatype": "json",
            "contentType": 'application/json',
            "data": function (d) {
                var reasonModel;
                $.each(d.data, function (key, value) {
                    var ReasonModel = {
                        'Reason_code': d.data[key]['Reason_code'],
                        'Reason_desc': d.data[key]['Reason_desc'],
                    };
                    reasonModel = ReasonModel;
                });

              
                var data = {
                    'Action': d.action,
                    'ReasonModel': reasonModel
                };
                return JSON.stringify(data);
            },
            success: function (data) {
                if (!data.resultModel.Success) {
                    swal.fire(data.resultModel.Msg);
                } else {
                    table();
                }
            }
        },
        table: "#ReasonTable",
        idSrc: 'Reason_code',
        formOptions: {
            main: {
                onBackground: 'none'
            }
        },
        fields: [
            {
                label: "原因代碼:",
                name: "Reason_code",
            },
            {
                label: "原因",
                name: "Reason_desc",
            }
        ],
        i18n: {
            create: {
                button: "新增",
                title: "新增原因",
                submit: "確定"
            },
            edit: {
                button: "編輯",
                title: "更改原因",
                submit:"確定"
            },
            remove: {
                button: "刪除",
                title: "確定要刪除??",
                submit: "確定",
                confirm: {
                    "_": "你確定要刪除這筆資料?",
                    "1": "你確定要刪除這筆資料?"
                }
            }
        }


    });

    editor.on('close', function () {
        editor.field('Reason_code').show();
        table();
    });

    $('input', editor.field('Reason_code').node()).on('keypress', function () {
        editor.field('Reason_code').error(
            this.length < 8 ? 'Password must be at least 8 characters.' :''
        );
    });

    editor.on('preSubmit', function (e, d, action) {
        if (action== 'remove') {
            return true
        }
        var Reason_code = this.field('Reason_code');
        var Reason_desc = this.field('Reason_desc');


        if (Reason_code.val() === '') {
            Reason_code.error('請勿空白');
            return false;
        }

        if (Reason_desc.val() === '') {
            Reason_desc.error('請勿空白');
            return false;
        }

        return true;
    });



    var Reaontable =  $('#ReasonTable').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        destroy: true,
        processing: true,
        serverSide: true,
        //scrollX: true,
        autoWidth: false,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        ajax: {
            "url": "/Reason/ReasonJson",
            "type": "POST",
            "datatype": "json",
            "data": {}
        },
        columns: [
            {
                data: null,
                defaultContent: '',
                className: 'select-checkbox',
                orderable: false,
                targets: 0,
            },
            { data: "Reason_code", "name": "代碼", "autoWidth": true, "className": "dt-body-center"},
            { data: "Reason_desc", "name": "原因", "autoWidth": true, "className": "dt-body-center"},
            { data: "Create_by", "name": "建立人員ID", "autoWidth": true, "className": "dt-body-center"},
            {
                data: "Create_date", "name": "建立日期", "autoWidth": true, "mRender": function (data, type, full) {
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
        select: {
            style: 'single',
        },
        buttons: [
            {
                extend: 'create',
                editor: editor
            },
            {
                extend: "selected",
                text: "編輯",
                action: function (e, dt, node, config) {
                    editor.field('Reason_code').hide();
                    editor.edit(Reaontable.rows({ selected: true }).indexes(), {
                        title: '編輯',
                        buttons: '確定',
                    }).mode('edit');
                }
            },
            {
                extend: "remove",
                editor: editor
            }

        ]

    });


}