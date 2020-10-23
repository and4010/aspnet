$(document).ready(function () {

    SetCheckBoxValue();
    setUsrValue();
    btn();
});

function btn() {
    $("#BtnAccountEdit").click(function () {
        Edit();
    });
}

function Edit() {

    var valid = $('#EditForm').valid();

    obj = document.getElementsByName("checkboxs");
    ORGANIZATIONID = [];
    ORGANIZATION_CODE = [];
    SUBINVENTORY_CODE = [];
    userSubinventory = [];
    for (i in obj) {
        if (obj[i].checked) {
            var checkValue = obj[i].id.indexOf("-");
            var organiztionCode = obj[i].id.substr(0, checkValue);
            ORGANIZATION_CODE.push(organiztionCode);
            var subCode = obj[i].id.substr(checkValue + 1);
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
        if (SUBINVENTORY_CODE.length == 0) {
            document.getElementById("checkboxs").style.display = "block";
            return;
        }
        return;
    } else {
        if (SUBINVENTORY_CODE.length == 0) {
            document.getElementById("checkboxs").style.display = "block";
            return;
        }
    }
    var rolename = $('#ddlRoleName').val();

    var accountModel = {
        Id: $('#UserId').val(),
        RoleName: rolename,
        Name: $('#Name').val().trim(),
        Email: $('#Email').val().trim(),
        Subinventory: userSubinventory
    };

    accountModel.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();


    $.ajax({
        url: '/Account/Edit',
        type: "POST",
        data: { accountModel: accountModel, userSubinventory: userSubinventory },
        success: function (data) {
            if (data.status) {
                swal.fire("更改成功");
                window.history.go(-1);
            } else {
                swal.fire(data.message);
            }
        },
        error: function () {
            swal.fire("失敗");
        }

    });
}

//預設checkbox勾選
function SetCheckBoxValue() {
    var id = $('#UserId').val();
    $.ajax({
        url: "/account/GetCheckboxValue",
        type: "post",
        data: { id: id },
        datatype: "json",
        success: function (data) {
            if (data.message != "") {
                CheckBoxValue(data.message);
            }
        }
    });
}

//預設checkbox 勾選
function CheckBoxValue(Subinventory) {
    var value = Subinventory;
    if (value != null) {
        sarray = [];
        sarray = value.split(",");
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


//預設user
function setUsrValue() {
    var id = $('#UserId').val();
    $.ajax({
        url: "/account/GetUserValue",
        type: "post",
        data: { id: id },
        datatype: "json",
        success: function (data) {
            if (data.message != "") {
                $('#Name').val(data.message.Name);
                $('#Account').val(data.message.Account);
                $('#Email').val(data.message.Email);
                $("#ddlRoleName").val(data.message.RoleName);
            }
        }
    });
}