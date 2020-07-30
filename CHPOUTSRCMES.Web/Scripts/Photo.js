var imgSrc = []; //圖片路徑
var imgFile = []; //文件
var imgName = []; //圖片名字
var files = []; //存放照片

//點擊預覽&&圖片放大
function addNewContent(obj) {
    $(imgBox).html("");
    for (var a = 0; a < imgSrc.length; a++) {
        var oldBox = $(obj).html();
        //$(obj).html(oldBox + '<div class="imgContainer"><img title=' + imgName[a] + ' alt=' + imgName[a] + ' src=' + imgSrc[a] + ' onclick="imgDisplay(this)"></div>');
        $(obj).html(oldBox + '<div class="imgContainer"><img title=' + imgName[a] + ' alt=' + imgName[a] + ' src=' + imgSrc[a] + ' onclick="imgDisplay(this)"><p onclick="removeImg(this,' + a + ')" class="imgDelete">删除</p></div>');
    }
}

//點擊預覽&&圖片放大不含刪除
function AddNewContent(obj) {
    $(imgBox).html("");
    for (var a = 0; a < imgSrc.length; a++) {
        var oldBox = $(obj).html();
        $(obj).html(oldBox + '<div class="imgContainer"><img title=' + imgName[a] + ' alt=' + imgName[a] + ' src=' + imgSrc[a] + ' onclick="imgDisplay(this)"></div>');
    };
}


//圖片放大
function imgDisplay(obj) {
    var src = $(obj).attr("src");
    var imgHtml = '<div style="width: 100%;height: 100vh;overflow: auto;background: rgba(0,0,0,0.5);text-align: center;position: fixed;top: 0;left: 0;z-index: 1000;"><img src=' + src + ' style="margin-top: 100px;width: 70%;margin-bottom: 100px;"/><p style="font-size: 50px;position: fixed;top: 30px;right: 30px;color: white;cursor: pointer;" onclick="closePicture(this)">×</p></div>';
    $('body').append(imgHtml);
}

//關閉圖片放大
function closePicture(obj) {
    $(obj).parent("div").remove();
}

//删除
function removeImg(obj, index) {
    imgSrc.splice(index, 1);
    imgFile.splice(index, 1);
    imgName.splice(index, 1);
    files.splice(index, 1);
    var boxId = "#" + $(obj).parent('.imgContainer').parent().attr("id");
    addNewContent(boxId);
}

//圖片預覽路徑
function getObjectURL(file) {
    var url = null;
    if (window.createObjectURL != undefined) { // basic
        url = window.createObjectURL(file);
    } else if (window.URL != undefined) { // mozilla(firefox)
        url = window.URL.createObjectURL(file);
    } else if (window.webkitURL != undefined) { // webkit or chrome
        url = window.webkitURL.createObjectURL(file);
    }
    return url;
}

