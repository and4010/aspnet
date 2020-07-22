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
        var file = $('#fileinput')[0];
        var fileList = file.files;
        for (var i = 0; i<fileList.length; i++) {
            var imgSrcI = getObjectURL(fileList[i]);
            imgName.push(fileList[i].name);
            imgSrc.push(imgSrcI);
            imgFile.push(fileList[i]);
        }
        addNewContent($('#imgBox'));
    });

}

function RollSave() {  
    var id = $("#Id").val();
    var status = $("#Status").val();
    var reason = $("#select-reason").val();
    var remak = $("#textarea").val();

    $.ajax({
        url: '/Purchase/RollEditSave',
        type: 'POST',
        datatype: 'json',
        data: { remak: remak, id: id, status: status, reason: reason, imgFile: imgFile},
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
    var formData = new FormData();
    var d = imgFile;
    if (d.length > 0) {
        formData.append("imgFile", d);
    }

    $.ajax({
        url: "/Purchase/FlatEditSave",
        type: "POST",
        data:  formData  ,
        dataType: "JSON",
        success: function (data) {
            if (data.boolean) {
                window.history.go(-1);
            }

        },
        error: function (data) {

        }
    });
}




function getServicePhoto() {
   
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