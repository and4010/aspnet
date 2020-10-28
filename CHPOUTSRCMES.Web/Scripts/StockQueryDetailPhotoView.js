var VimgSrc = []; //圖片路徑
var VimgFile = []; //文件
var VimgName = []; //圖片名字

$(document).ready(function () {
    getPhotoView();

})


function getPhotoView() {
    $.ajax({
        "url": "/Stock/GetPhoto",
        "type": "POST",
        "datatype": "json",
        "data": { STOCK_ID: $('#StockId').val() },
        success: function (data) {

            if (data.photo.length != 0) {
                for (var i = 0; i < data.photo.length; i++) {
                    VimgSrc.push("data:image/jpeg;base64," + data.photo[i]);
                }
            } else {
                swal.fire("無貨故照片");
            }
            addStockContent($('#saveBox'));
        },
        error: function (data) {
            addStockContent($('#saveBox'));
            swal.fire(data);
        },
    });
}


//點擊預覽&&圖片放大
function addStockContent(obj) {
    $(obj).html("");
    for (var a = 0; a < VimgSrc.length; a++) {
        var oldBox = $(obj).html();
        $(obj).html(oldBox + '<div class="col col-md-6 imgContainer"><img title=' + VimgName[a] + ' alt=' + VimgName[a] + ' src=' + VimgSrc[a] + ' onclick="StockimgDisplay(this)"></div>');
    };
}



//圖片放大
function StockimgDisplay(obj) {
    var src = $(obj).attr("src");
    var imgHtml = '<div style="width: 100%;height: 100vh;overflow: auto;background: rgba(0,0,0,0.5);text-align: center;position: fixed;top: 0;left: 0;z-index: 1000;"><img src=' + src + ' style="margin-top: 100px;width: 70%;margin-bottom: 100px;"/><p style="font-size: 50px;position: fixed;top: 30px;right: 30px;color: white;cursor: pointer;" onclick="closePicture(this)">×</p></div>'
    $('body').append(imgHtml);
}

//關閉圖片放大
function closePicture(obj) {
    $(obj).parent("div").remove();
}
