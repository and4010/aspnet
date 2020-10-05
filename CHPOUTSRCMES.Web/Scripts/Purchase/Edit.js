$(document).ready(function () {
    $(":file").filestyle({ buttonText: "選擇照片" });
    click();
    photoView();
    getPhotoList();
    SetSpinnerValue();
});



function click() {
    $("#BtnReturn").click(function () {
        window.history.go(-1);
    });

    $("#BtnRollSave").click(function () {
        RollSave();
    });

    $("#BtnFlatSave").click(function () {
        FlatSave();
    });
}

//已存檔入庫預設照片
function photoView() {
    $('#fileinput').on("change", function () {
        //imgName = [];
        //imgSrc = [];
        //imgFile = [];
        //files = [];
        var file = $('#fileinput')[0];
        var fileList = file.files;
        for (var i = 0; i < fileList.length; i++) {
            var imgSrcI = getObjectURL(fileList[i]);
            if (!imgNameDuplicateCheck(fileList[i].name)) {
                imgName.push(fileList[i].name);
                imgSrc.push(imgSrcI);
                imgFile.push(fileList[i]);
                files.push(fileList[i]);
            }
        }
        //photo.js使用
        addNewContent($('#imgBox'));
    });

}

function RollSave() {
    var id = $("#Id").val();
    var status = $("#Status").val();
    var reason = $("#select-reason").val();
    var remak = $("#textarea").val();
    var Locator = $("#select-Locator").val();
    var formData = new FormData();
    for (var i = 0; i < files.length; i++) {
        formData.append(files[i].name, files[i]);
    }
    formData.append("id", id);
    formData.append("Reason", reason);
    formData.append("Locator", Locator);
    formData.append("Remark", remak);

    $.ajax({
        url: '/Purchase/RollEditSave',
        type: 'POST',
        datatype: 'json',
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.resultModel.Success) {
                window.history.go(-1);
            } else {
                swal.fire(data.resultModel.Msg)
            }
        },
        error: function (data) {

        }
    });

}

function FlatSave() {
    var id = $("#Id").val();
    var status = $("#Status").val();
    var reason = $("#select-reason").val();
    var remak = $("#textarea").val();
    var Locator = $("#select-Locator").val();
    var formData = new FormData();
    for (var i = 0; i < files.length; i++) {
        formData.append(files[i].name, files[i]);
    }
    formData.append("id", id);
    formData.append("Reason", reason);
    formData.append("Locator", Locator);
    formData.append("Remark", remak);



    $.ajax({
        "url": "/Purchase/FlatEditSave",
        "type": "POST",
        "datatype": "json",
        "data": formData,
        success: function (data) {
            if (data.resultModel.Success) {
                window.history.go(-1);
            } else {
                swal.fire(data.resultModel.Msg)
            }
        },
        error: function (data) {
            swal.fire(data);
        },
        cache: false,
        contentType: false,
        processData: false
    });
}




function getServicePhoto() {
    var id = $("#Id").val();
    $.ajax({
        "url": "/Purchase/GetPhotos",
        "type": "POST",
        "datatype": "json",
        "data": { id: id },
        success: function (data) {
            if (data.ListBytePhoto != null) {
                if (data.ListBytePhoto.length != 0) {
                    for (var i = 0; i < data.ListBytePhoto.length; i++) {
                        uploadedImgSrc.push("data:image/jpeg;base64," + data.ListBytePhoto[i]);
                    }
                    AddNewContent($('#saveBox'));
                }
            }
          
        },
        error: function (data) {
            swal.fire(data);
        },
    });
}

function getPhotoList() {
    var id = $("#Id").val();
    $.ajax({
        "url": "/Purchase/GetPhotoList",
        "type": "POST",
        "datatype": "json",
        "data": { id: id },
        success: function (data) {
            if (data.Code == 0) {
                for (var i = 0; i < data.Result.length; i++) {
                    getPhotoById(data.Result[i], i == data.Result.length - 1);
                }
            } else {
                swal.fire(data.Msg);
            }
        },
        error: function (data) {
            swal.fire(data);
        },
    });
}

function getPhotoById(id, final) {

    $.ajax({
        "url": "/Purchase/GetPhoto",
        "type": "POST",
        "datatype": "json",
        "async": false,
        "data": { id: id },
        success: function (data) {
            if (data.Code == 0) {
                uploadedImgSrc.push("data:image/jpeg;base64," + data.Result);
            }
            else {
                swal.fire(data.Msg);
            }

            if (final) AddNewContent($('#saveBox'));
        },
        error: function (data) {
            if (final) AddNewContent($('#saveBox'));
            swal.fire(data);
        },
    });
}


function SetSpinnerValue() {
    var PickId = $("#Id").val();
    $.ajax({
        url: '/Purchase/SetSpinnerValue/',
        type: "POST",
        dataType: 'json',
        data: { PickId: PickId },
        success: function (data) {
            if (data.resultDataModel.Success) {
                if (data.resultDataModel.Data.ReasonCode != "") {
                    $("#select-reason").val(data.resultDataModel.Data.ReasonCode);
                }
                if (data.resultDataModel.Data.LocatorId != null) {
                    $("#select-Locator").val(data.resultDataModel.Data.LocatorId);
                }
            
            }
        },
        error: function () {
            $.swal.fire("失敗")
        }
    });
}