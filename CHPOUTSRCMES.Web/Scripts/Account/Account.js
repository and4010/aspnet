// 0 一般儲存
// 1 忘記密碼
// 2 異動

var status;
var id;
var EditorAccount;
$(document).ready(function () {

    status = 0;
    LoadTable();
    btnDailog();

    $('#AccountTable tbody').on('click', '#btnStop', function (e) {

        var data = $('#AccountTable').DataTable().row($(this).parents('tr')).data();
        var id = data.Id;
        if (data == null) {
            return false;
        };
        if (data.Status == "停用") {
            var title = "帳號啟用";
            var text = "確定要帳號啟用嗎?";

        }
        if (data.Status == "啟用") {
            var title = "帳號停用";
            var text = "確定要帳號停用嗎?";
        }

        swal.fire({
            title: title,
            text: text,
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "確定",
            cancelButtonText: "取消"
        }).then(function (result) {
            if (result.value) {
                $.ajax({
                    url: '/Account/AccountDisable/',
                    type: "POST",
                    data: { id: id },
                    success: function () {
                        LoadTable();
                    },
                    error: function () {
                        swal.fire("失敗");
                    }

                });
            }
        });




    });

    $('#AccountTable tbody').on('click', '#btnDefaultPassword', function (e) {

        var data = $('#AccountTable').DataTable().row($(this).parents('tr')).data();
        var Id = data.Id;
        if (data == null) {
            return false;
        };

        swal.fire({
            title: "密碼重設",
            text: "確定要重設密碼嗎?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "確定",
            cancelButtonText: "取消"
        }).then(function (result) {
            if (result.value) {
                $.ajax({
                    url: '/Account/DeafultPassword/',
                    type: "POST",
                    data: { Id: Id },
                    success: function (data) {
                        if (data.status) {
                            LoadTable();
                            swal.fire("重設成功");
                        } else {
                            swal.fire(data.message);
                        }

                    },
                    error: function () {
                        swal.fire("失敗");
                    }

                });
            }
        });




        //clearItem()
        //EnableAll(false)
        //EnableInputPassword(true)
        //$("#scrollbox").collapse('show');
        //data.Subinventory

        //selectToValue(data.RoleName)
        //$('#Account').val(data.Account)
        //$('#Name').val(data.Name)
        //$('#Email').val(data.Email)

        //status = 1
        //id = Id


    })

    $('#AccountTable tbody').on('click', '#btnTrans', function (e) {

        var data = $('#AccountTable').DataTable().row($(this).parents('tr')).data();
        var Id = data.Id;
        if (data == null) {
            return false;
        };

        e.preventDefault();
        EditorAccount.field('RoleId').hide();
        EditorAccount.field('Account').hide();
        EditorAccount.field('Email').hide();
        EditorAccount.edit($(this).closest('tr'), {
            title: '編輯',
            buttons: '確定'
        });

        //clearItem()
        //EnableAll(false)
        //EnableEdit(true)
        //$("#scrollbox").collapse('show');


        //selectToValue(data.RoleName)
        //$('#Account').val(data.Account)
        //$('#Email').val(data.Email)
        //$('#Password').val(data.Password)
        //SetCheckBoxValue(data.Subinventory)


        //status = 2
        //id = Id
    })

    $('#AccountTable tbody').on('click', '#btnDelete', function (e) {

        var data = $('#AccountTable').DataTable().row($(this).parents('tr')).data();
        var Id = data.Id;
        if (data == null) {
            return false;
        };

        Swal.fire({
            title: '刪除?',
            text: "確定要刪除?" + data.Account,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: '確定',
            cancelButtonText: '取消',
        }).then(function (result) {
            if (result.value) {
                $.ajax({
                    url: '/Account/Delete/',
                    type: "POST",
                    data: { id: Id },
                    success: function (data) {
                        if (data.status) {
                            LoadTable();
                            swal.fire(data.message)
                        } else {
                            swal.fire(data.message);
                        }

                    },
                    error: function () {
                        swal.fire("失敗")
                    }
                });
            }
        });
    });


});

//checkbox click 事件 取消提醒
function checkboxclick() {
    document.getElementById("checkboxs").style.display = "none";
}


function LoadCheckbox() {
    var optionsA = [];
    $.ajax({
        url: '/Account/Subinventory',
        "type": "POST",
        "datatype": "json",
        success: function (data) {
            var option = {};
            for (i = 0; i < data.model.length; i++) {
                option.id = data.model[i].SUBINVENTORY_CODE + data.model[i].SUBINVENTORY_NAME;
                option.name = data.model[i].SUBINVENTORY_CODE + data.model[i].SUBINVENTORY_NAME;
                option.label = data.model[i].SUBINVENTORY_CODE + data.model[i].SUBINVENTORY_NAME;
                option.value = data.model[i].SUBINVENTORY_CODE + data.model[i].SUBINVENTORY_NAME;
                optionsA.push(option);
                option = {};
            }
            EditorAccount.field('Subinventory[].SubinventoryName').update(optionsA);

        }
    });
}

function LoadTable() {
    //仔入checkbox
    LoadCheckbox();

    EditorAccount = new $.fn.dataTable.Editor({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        ajax: {
            url: '/Account/EditorAccount',
            type: "POST",
            dataType: "json",
            contentType: 'application/json',
            data: function (d) {
                var AccountModel;
                $.each(d.data, function (key, value) {
                    var SubinventoryDetail = [];
                    var test = [];
                    for (i = 0; i < value.Subinventory.length; i++) {
                        test = value.Subinventory[i];
                        SubinventoryDetail.push(test);
                        test = {};
                    };

                    var accountModel = {
                        'Id': d.data[key]['Id'],
                        'RoleId': d.data[key]['RoleId'],
                        'Account': d.data[key]['Account'],
                        'Name': d.data[key]['Name'],
                        'Email': d.data[key]['Email'],
                        'Status': d.data[key]['Status'],
                        'Subinventory': SubinventoryDetail
                    };

                    AccountModel = accountModel;
                });
                var data = {
                    'Action': d.action,
                    'AccountModel': AccountModel
                };
                return JSON.stringify(data);
            },
            success: function () {
                LoadTable();
                LoadCheckbox();
            }
        },
        table: "#AccountTable",
        idSrc: 'Id',
        formOptions: {
            main: {
                onBackground: 'none'
            }
        },
        fields: [
            {
                label: "角色:",
                name: "RoleId",
                type: "select",
                options: [
                    { label: "使用者", value: 1 },
                    { label: "華紙使用者", value: 2 },
                    { label: "系統管理員", value: 3 }
                ]
            },
            {
                label: "帳號",
                name: "Account",
            },
            {
                label: "使用者",
                name: "Name",
            },
            {
                label: "信箱",
                name: "Email",
                attr: {
                    type: 'email',
                    required: true,
                    placeholder: 'email@mydomain.com'
                }
            },
            {
                label: "Id",
                name: "Id",
            },
            {
                name: "Subinventory[].SubinventoryName",
                //separator: "|",
                type: "checkbox"
            }
        ],
        i18n: {
            edit: {
                button: "異動",
                title: "更改異動",
                submit: "確定"
            },
            create: {
                button: "新增",
                title: "新增",
                submit: "確定"
            }
        }

    });
    EditorAccount.field('Id').hide();
    EditorAccount.on('preSubmit', function (e, d) {
        var RoleId
        var Account
        var Name
        var Email
        var Subinventory
        $.each(d.data, function (key, values) {
            RoleId = d.data[key]['RoleId']
            Account = d.data[key]['Account']
            Name = d.data[key]['Name']
            Email = d.data[key]['Email']
            Subinventory = d.data[key]['Subinventory']

        });
        if (RoleId.length == 0) {
            this.field('RoleId').error('請勿空白');
            return false;
        }
        if (Account.length == 0) {
            this.field('Account').error('請勿空白');
            return false;
        }
        if (Name.length == 0) {
            this.field('Name').error('請勿空白');
            return false;
        }
        if (Email.length == 0) {
            this.field('Email').error('請勿空白');
            return false;
        }
        if (Subinventory.length == 0) {
            this.field('Subinventory[].SubinventoryName').error('至少選一個');
            return false;
        }
        return true;
    });

    EditorAccount.on('close', function () {
        LoadTable();
    });
    $('#AccountTable').DataTable({
        "language": {
            "url": "/bower_components/datatables/language/zh-TW.json"
        },
        destroy: true,
        processing: true,
        serverSide: true,
        autoWidth: false,
        dom:
            "<'row'<'col-sm-2'l><'col-sm-7'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        "lengthMenu": [[50, 100, 150, 200], [50, 100, 150, 200]],
        ajax: {
            "url": "/Account/AccountJson",
            "type": "POST",
            "datatype": "json",
            "data": {}
        },
        columnDefs: [{
            orderable: false, targets: [7], width: "60px"
        }],
        columns: [
            { data: "Account", "name": "帳號", "autoWidth": true, "className": "dt-body-center" },
            { data: "RoleId", "name": "角色ID", "autoWidth": true, "visible": false },
            { data: "RoleName", "name": "角色", "autoWidth": true, "className": "dt-body-center" },
            { data: "Name", "name": "姓名", "autoWidth": true, "className": "dt-body-center" },
            { data: "Email", "name": "信箱", "autoWidth": true, "className": "dt-body-center" },
            { data: "Status", "name": "狀態", "autoWidth": true, "className": "dt-body-center" },
            { data: "Subinventory", "name": "倉庫", "autoWidth": true, render: "[,].SubinventoryName", "className": "dt-body-center" },
            {
                data: "Status", "width": "350px", "render": function (data) {
                    if (data == "啟用") {
                        return '<button class="btn btn-danger btn-sm" id = "btnStop">停用</button>'
                            + '&nbsp|&nbsp' + '<button class="btn btn-primary btn-sm" id="btnDefaultPassword">預設密碼</button>' +
                            '&nbsp|&nbsp' + '<button class = "btn btn btn-warning btn-sm" id ="btnTrans" >異動</button>' +
                            '&nbsp|&nbsp' + '<button class = "btn btn btn-danger btn-sm" id= "btnDelete" >刪除</button>';
                    } else {
                        return '<button class="btn btn-danger btn-sm" id = "btnStop">啟用</button>'
                            + '&nbsp|&nbsp' + '<button class="btn btn-primary btn-sm" id="btnDefaultPassword">預設密碼</button>' +
                            '&nbsp|&nbsp' + '<button class = "btn btn btn-warning btn-sm" id ="btnTrans" >異動</button>' +
                            '&nbsp|&nbsp' + '<button class = "btn btn btn-danger btn-sm" id= "btnDelete" >刪除</button>';
                    }

                }
            }
        ],
        buttons: [
            {
                extend: "create",
                text: '新增',
                editor: EditorAccount
            },
        ]
    });
}

function btnDailog() {
    $('#BtnchagnePassword').click(function () {
        $.ajax({
            url: '/Account/_ChangePassword',
            type: "POST",
            datatype: 'json',
            success: function (result) {
                $('body').append(result);
                Open($('#changePasswordModal'));
            }
        });
    });
}

//彈出dialog
function Open(modal_dialog) {
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
    modal_dialog.on('click', '#btnConfirm', function (e) {
        var Id = $('#UserId').val()
        var OldPassword = $('#OldPassword').val();
        var NewPassword = $('#NewPassword').val();
        var ConfirmPassword = $('#ConfirmPassword').val();
        $.ajax({
            url: '/Account/ChagnePassword',
            type: "POST",
            datatype: 'json',
            data: {
                Id: Id,
                OldPassword: OldPassword,
                NewPassword: NewPassword,
                ConfirmPassword: ConfirmPassword
            },
            success: function (data) {
                //if (data.status) {
                //    swal.fire("更改成功");
                //} else {
                //    swal.fire("更改失敗");
                //}
            },
            error: function () {
                swal.fire("失敗");
            }

        });
    });

    modal_dialog.modal('show');

}



function ChangePasswordDialog() {






    //var Id = $('#UserId').val();
    //swal.fire({
    //    title: '變更密碼',
    //    text: '請輸入更改密碼',
    //    input: 'text',
    //    inputAttributes: {
    //        autocapitalize: 'off'
    //    },
    //    showCancelButton: true,
    //    confirmButtonText: '確定',
    //    showLoaderOnConfirm: true,
    //    allowOutsideClick: false,
    //    confirmButtonColor: '#3085d6',
    //    cancelButtonColor: '#d33',
    //    preConfirm: function (result) {
    //        if (result.length == 0) {
    //            return
    //        }
    //    }
    //}).then((Password) => {
    //    if (Password.value) {
    //        $.ajax({
    //            url: '/Account/ChangePasswrod',
    //            type: "POST",
    //            data: {
    //                Id: Id, Password: Password.value
    //            },
    //            success: function (data) {
    //                if (data.status) {
    //                    swal.fire("更改成功");
    //                } else {
    //                    swal.fire("更改失敗");
    //                }
    //            },
    //            error: function () {
    //                swal.fire("失敗");
    //            }

    //        });
    //    } else {
    //        swal.frie("不得空白");
    //    }
    //});

}


/*以下用不到*/
function onBtn() {

    $("#BtnAccountSave").click(function () {

        if (status == 0) {
            Insert();
        }
        if (status == 1) {
            password();

        }

        if (status == 2) {
            Edit();
        }

    });


    $("#BtnCancel").click(function () {
        clearItem();
        EnableAll(false);
    })

}

function Insert() {

    var valid = $('#EditForm').valid();

    obj = document.getElementsByName("checkboxs");
    Subinventory = [];
    for (i in obj) {
        if (obj[i].checked)
            Subinventory.push(obj[i].value);
    }


    if (valid == false) {
        if (Subinventory.length == 0) {
            document.getElementById("checkboxs").style.display = "block";
            return;
        }
        return;
    } else {
        if (Subinventory.length == 0) {
            document.getElementById("checkboxs").style.display = "block";
            return;
        }
    }

    var accountModel = {
        RoleName: User.value,
        Account: $('#Account').val().trim(),
        Name: $('#Name').val().trim(),
        Email: $('#Email').val().trim(),
        Password: $('#Password').val().trim()
    };

    accountModel.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();

    $.ajax({
        url: "/Account/Create",
        type: "POST",
        data: { accountModel: accountModel, Subinventory: Subinventory },
        dataType: "JSON",
        success: function (data) {
            if (data.status) {
                swal.fire("新增成功");
                LoadTable();
                clearItem();
            } else {
                swal.fire("新增失敗");
            }
        }
    });

}


function Edit() {

    var valid = $('#EditForm').valid();

    obj = document.getElementsByName("checkboxs");
    Subinventory = [];
    for (i in obj) {
        if (obj[i].checked)
            Subinventory.push(obj[i].value);
    }

    if (valid == false) {
        if (Subinventory.length == 0) {
            document.getElementById("checkboxs").style.display = "block";
            return;
        }
        return;
    } else {
        if (Subinventory.length == 0) {
            document.getElementById("checkboxs").style.display = "block";
            return;
        }
    }

    var strUser = User.value;

    var Name = $('#Name').val().trim();


    $.ajax({
        url: '/Account/Edit',
        type: "POST",
        data: { id: id, strUser: strUser, Name: Name, Subinventory: Subinventory },
        success: function (data) {
            if (data.status) {
                swal.fire("更改成功");
                LoadTable();
                clearItem();
                EnableEdit(false);
            } else {
                swal.fire("更改失敗");
            }
        },
        error: function () {
            swal.fire("失敗");
        }

    });
}



function clearItem() {
    //清除欄位資料
    $('#Account').val("");
    $('#Name').val("");
    $('#Email').val("");
    $('#Password').val("");
    User.selectedIndex = 0;
    status = 0;
    id = null;

    obj = document.getElementsByName("checkboxs");
    for (var i = 0; i < obj.length; i++) {
        obj[i].checked = obj.checked;
    }
}


function EnableInputPassword(Boolean) {

    $('#Account').attr("disabled", Boolean);
    $('#Name').attr("disabled", Boolean);
    $('#Email').attr("disabled", Boolean);

    $('input:checkbox').attr({ disabled: Boolean });
    $('#User').prop('disabled', Boolean);
}


function EnableEdit(Boolean) {

    $('#Account').attr("disabled", Boolean);
    $('#Email').attr("disabled", Boolean);
    $('#Password').attr("disabled", Boolean);

}

function EnableAll(Boolean) {
    $('#User').prop('disabled', Boolean);
    $('#Account').attr("disabled", Boolean);
    $('#Name').attr("disabled", Boolean);
    $('#Email').attr("disabled", Boolean);
    $('#Password').attr("disabled", Boolean);
    $('input:checkbox').attr({ disabled: Boolean });
}


function selectToValue(RoleName) {
    switch (RoleName) {
        case "使用者":
            document.getElementById("User").value = 1;
            break;
        case "華紙使用者":
            document.getElementById("User").value = 2;
            break;
        case "系統管理員":
            document.getElementById("User").value = 3;
            break;
    }


}


//預設checkbox 勾選
function SetCheckBoxValue(Subinventory) {
    var value = Subinventory;
    if (value != null) {
        sarray = [];
        sarray = value.split(" ");
        obj = document.getElementsByName("checkboxs");
        for (var i = 0; i < sarray.length; i++) {
            for (var j = 0; j < obj.length; j++) {
                if (obj[j].value == sarray[i]) {
                    obj[j].checked = true;
                }
            }
        }
    }

}

