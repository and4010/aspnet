$(document).ready(function () {
    click();
    getServicePhoto();
});

var VimgSrc = []; //圖片路徑
var VimgFile = []; //文件
var VimgName = []; //圖片名字

function click() {
    $("#BtnReturn").click(function () {
        window.history.go(-1);
    });
}



//點擊預覽&&圖片放大
function addContent(obj) {
    $(imgBox).html("");
    for (var a = 0; a < VimgSrc.length; a++) {
        var oldBox = $(obj).html();
        $(obj).html(oldBox + '<div class="imgContainer"><img title=' + VimgName[a] + ' alt=' + VimgName[a] + ' src=' + VimgSrc[a] + ' onclick="imgDisplay(this)"></div>');
    };
}


//圖片放大
function imgDisplay(obj) {
    var src = $(obj).attr("src");
    var imgHtml = '<div style="width: 100%;height: 100vh;overflow: auto;background: rgba(0,0,0,0.5);text-align: center;position: fixed;top: 0;left: 0;z-index: 1000;"><img src=' + src + ' style="margin-top: 100px;width: 30%;margin-bottom: 100px;"/><p style="font-size: 50px;position: fixed;top: 30px;right: 30px;color: white;cursor: pointer;" onclick="closePicture(this)">×</p></div>'
    $('body').append(imgHtml);
}

//關閉圖片放大
function closePicture(obj) {
    $(obj).parent("div").remove();
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
                        VimgSrc.push("data:image/jpeg;base64," + data.ListBytePhoto[i]);
                    }
                    addContent($('#imgBox'));
                }
            }

        },
        error: function (data) {
            swal.fire(data);
        },
    });
}