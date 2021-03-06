$(document).ready(function () {

   
    LoadTable();
    btnDailog();
    onBtn();

    $('#AccountTable tbody').on('click', '#btnStop', function (e) {

        var data = $('#AccountTable').DataTable().row($(this).parents('tr')).data();
        var id = data.Id;
        if (data == null) {
            return false;
        };
        if (data.Status == "停用") {
            var title = "帳號啟用";
            var text = "確定要帳號啟用" + data.Account + "嗎?";

        }
        if (data.Status == "啟用") {
            var title = "帳號停用";
            var text = "確定要帳號停用" + data.Account + "嗎?";
        }

        swal.fire({
            title: title,
            text: text,
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "確定",
            cancelButtonText: "取消"
        }).then(function (result) {
            if (result.value) {
                ShowWait(function () {
                    $.ajax({
                        url: '/Account/AccountDisable/',
                        type: "POST",
                        datatype: 'json',
                        data: { id: id },
                        success: function (data) {
                            if (data.status) {
                                CloseWait();
                                LoadTable();
                            } else {
                                swal.fire(data.message);
                            }
                        },
                        error: function () {
                            swal.fire("失敗");
                        }

                    });
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
            text: "確定要重設密碼" + data.Account + "嗎?",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "確定",
            cancelButtonText: "取消"
        }).then(function (result) {
            if (result.value) {
                ShowWait(function () {
                    $.ajax({
                        url: '/Account/DeafultPassword/',
                        type: "POST",
                        datatype: 'json',
                        data: { Id: Id },
                        success: function (data) {
                            if (data.status) {
                                CloseWait();
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
                });
            }
        });
    })

    $('#AccountTable tbody').on('click', '#btnTrans', function (e) {

        var data = $('#AccountTable').DataTable().row($(this).parents('tr')).data();
        var id = data.Id;
        if (data == null) {
            return false;
        };

        swal.fire({
            title: "異動資料",
            text: "確定要異動資料" + data.Account + "嗎?",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "確定",
            cancelButtonText: "取消"
        }).then(function (result) {
            if (result.value) {
                window.location.href = "/Account/Edit/" + id;
            }
        });

    })

    $('#AccountTable tbody').on('click', '#btnDelete', function (e) {

        var data = $('#AccountTable').DataTable().row($(this).parents('tr')).data();
        var Id = data.Id;
        if (data == null) {
            return false;
        };

        Swal.fire({
            title: '刪除?',
            text: "確定要刪除" + data.Account+ "嗎?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: '確定',
            cancelButtonText: '取消',
        }).then(function (result) {
            if (result.value) {
                ShowWait(function () {
                    $.ajax({
                        url: '/Account/Delete/',
                        type: "POST",
                        datatype: 'json',
                        data: { id: Id },
                        success: function (data) {
                            if (data.status) {
                                CloseWait();
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
                });
            }
        });
    });

    $('#AccountTable tbody').on('click', '#btnInform', function (e) {
        var data = $('#AccountTable').DataTable().row($(this).parents('tr')).data();
        var Id = data.Id;
        if (data == null) {
            return false;
        };

        ShowWait(function () {
            $.ajax({
                url: '/Account/GetViewUserSub/',
                type: "POST",
                data: { id: Id },
                success: function (data) {
                    if (data.message != "") {
                        CloseWait();
                        var text = "";
                        var i = 0;
                        for (i = 0; i < data.message.length; i++) {
                            text += data.message[i] + "<br />";
                        }
                        Swal.fire({
                            title: "倉庫",
                            html: text,
                            confirmButtonColor: '#3085d6',
                            cancelButtonColor: '#d33',
                            confirmButtonText: '確定',
                        }).then(function (result) {
                            if (result.value) {
                                
                            }
                        });
                    } else {
                        CloseWait();
                    }

                },
                error: function () {
                    swal.fire("失敗")
                }
            });
        });

    });

});

//checkbox click 事件 取消提醒
function checkboxclick() {
    document.getElementById("checkboxs").style.display = "none";
}


function LoadTable() {
    
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
            "url": "/Account/LoadTable",
            "type": "POST",
            "datatype": "json",
            "data": {}
        },
        columnDefs: [{
            orderable: false, targets: [8], width: "60px"
        }],
        columns: [
            { data: "Account", "name": "帳號", "autoWidth": true, "className": "dt-body-center" },
            { data: "RoleId", "name": "角色ID", "autoWidth": true, "visible": false },
            { data: "RoleName", "name": "角色", "autoWidth": true, "className": "dt-body-center" },
            { data: "Name", "name": "姓名", "autoWidth": true, "className": "dt-body-center" },
            { data: "Email", "name": "信箱", "autoWidth": true, "className": "dt-body-center" },
            { data: "Status", "name": "狀態", "autoWidth": true, "className": "dt-body-center" },
            {
                data: "SubinventoryCount", "name": "倉庫", "autoWidth": true, render: function (data) {
                    if (data) {
                        return '<a style="cursor:pointer" id = "btnInform">' + data + ' <i class="fa fa-eye" aria-hidden="true"></i></a>'
                    }

                }, "className": "dt-body-center"
            },
            { data: "UserSubinventory", "name": "", "autoWidth": true, className: "dt-body-center", visible: false },
            {
                data: "Status", "autoWidth": true, "render": function (data) {
                    if (data == "啟用") {
                        return '<button class="btn btn-danger btn-sm" id = "btnStop">停用</button>'
                            + '&nbsp|&nbsp' + '<button class="btn btn-primary btn-sm" id="btnDefaultPassword">預設密碼</button>' +
                            '&nbsp|&nbsp' + '<button class = "btn btn btn-warning btn-sm" id ="btnTrans" >異動</button>' +
                            '&nbsp|&nbsp' + '<button class = "btn btn btn-danger btn-sm" id= "btnDelete" >刪除</button>';
                    } else {
                        return '<button class="btn btn-primary btn-sm" id = "btnStop">啟用</button>'
                            + '&nbsp|&nbsp' + '<button class="btn btn-primary btn-sm" id="btnDefaultPassword">預設密碼</button>' +
                            '&nbsp|&nbsp' + '<button class = "btn btn btn-warning btn-sm" id ="btnTrans" >異動</button>' +
                            '&nbsp|&nbsp' + '<button class = "btn btn btn-danger btn-sm" id= "btnDelete" >刪除</button>';
                    }

                }
            }
        ],
        buttons: [
            {
                text: '新增使用者',
                action: function () {
                    window.location.href = "/Account/Create";
                }
            },
        ],
    });
}


function onBtn() {


    $("#BtnAccountSave").click(function () {
        Insert();
    });


}

function Insert() {

    var valid = $('#EditForm').valid();
 
    obj = document.querySelectorAll('input[type=checkbox]');
    ORGANIZATIONID = [];
    ORGANIZATION_CODE = [];
    SUBINVENTORY_CODE = [];
    userSubinventory = [];
    for (i in obj) {
        if (obj[i].checked) {
            var checkValue = obj[i].id.indexOf("-");
            var organiztionCode = obj[i].id.substr(0, checkValue);
            ORGANIZATION_CODE.push(organiztionCode);
            var subCode = obj[i].id.substr(checkValue+1);
            SUBINVENTORY_CODE.push(subCode);
            var organizationId = obj[i].value.substr(0, checkValue);
            ORGANIZATIONID.push(organizationId);
            userSubinventory.push({
                ORGANIZATION_CODE: organiztionCode,
                SUBINVENTORY_CODE: subCode,
                ORGANIZATIONID: organizationId
            })
        }
    }

    if (valid == false) {
        if (userSubinventory.length == 0) {
            document.getElementById("checkboxs").style.display = "block";
            return;
        }
        return;
    } else {
        if (userSubinventory.length == 0) {
            document.getElementById("checkboxs").style.display = "block";
            return;
        }
    }
    var rolename = $('#ddlRoleName').val();

    var accountModel = {
        RoleName: rolename,
        Account: $('#Account').val().trim(),
        Name: $('#Name').val().trim(),
        Email: $('#Email').val().trim(),
        Password: $('#Password').val().trim(),
        Subinventory:userSubinventory
    };

    accountModel.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
    
    ShowWait(function () {
        $.ajax({
            url: "/account/createuser",
            type: "post",
            data: { accountmodel: accountModel, userSubinventory: userSubinventory },
            datatype: "json",
            success: function (data) {
                if (data.resultModel.Success) {
                    CloseWait();
                    swal.fire(data.resultModel.Msg);
                    clearItem();
                } else {
                    swal.fire(data.resultModel.Msg);
                }
            },
             error: function () {
                swal.fire("新增使用者失敗")
            }
        });
    });

}


function clearItem() {
    //清除欄位資料
    $('#Account').val("");
    $('#Name').val("");
    $('#Email').val("");
    $('#Password').val("");
    var obj = document.getElementById("ddlRoleName");
    obj.selectedIndex = 0;
    status = 0;
    id = null;

    obj = document.getElementsByName("checkboxs");
    for (var i = 0; i < obj.length; i++) {
        obj[i].checked = obj.checked;
    }
}

