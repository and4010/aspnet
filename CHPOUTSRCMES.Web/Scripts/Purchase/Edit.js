$(document).ready(function () {
    $(":file").filestyle({ buttonText: "選擇照片" });
    click();
    photoView();
    getServicePhoto();
    //getSpinnerValue();
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
        imgName = [];
        imgSrc = [];
        imgFile = [];
        files = [];
        var file = $('#fileinput')[0];
        var fileList = file.files;
        for (var i = 0; i < fileList.length; i++) {
            var imgSrcI = getObjectURL(fileList[i]);
            imgName.push(fileList[i].name);
            imgSrc.push(imgSrcI);
            imgFile.push(fileList[i]);
            files.push(fileList[i]);
        }
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
            if (data.boolean) {
                window.history.go(-1);
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
    formData.append("reason", reason);
    formData.append("Locator", Locator);
    formData.append("remak", remak);



    $.ajax({
        "url": "/Purchase/FlatEditSave",
        "type": "POST",
        "datatype": "json",
        "data": formData,
        success: function (data) {
            if (data.boolean) {
                window.history.go(-1);
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
        "url": "/Purchase/GetPhoto",
        "type": "POST",
        "datatype": "json",
        "data": { id: id },
        success: function (data) {
            if (data.ListBytePhoto != null) {
                if (data.ListBytePhoto.length != 0) {
                    for (var i = 0; i < data.ListBytePhoto.length; i++) {
                        imgSrc.push("data:image/jpeg;base64," + data.ListBytePhoto[i]);
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

//function getSpinnerValue() {
//   $("#select-reason").change(function (e) {
//        var reason = $("#select-reason").val();
//        $.ajax({
//            url: '/Purchase/Reason/',
//            type: "POST",
//            dataType: 'json', // 轉json
//            data: { reason },
//            success: function (data) {
//                $("#textarea").val(data);
//            },
//            error: function () {
//                $.swal.fire("失敗")
//            }
//        });

//    });

//}