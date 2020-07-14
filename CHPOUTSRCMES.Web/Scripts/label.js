

function PrintLable(dataTables, url) {
    var data = dataTables.rows('.selected').data();
    if (data.length == 0) {
        return false;
    }
    var columns = dataTables.settings().init().columns;
    var keys = [];
    var values = [];
    var key = columns[1].data; //Id
    for (i = 0; i < data.length; i++) {
        keys.push(key);
        values.push(data[i][key]);
    }

    OpenWindowWithPost(url, '_blank', keys, values);
}

function PrintLable(dataTables, url, keyIndex) {
    var data = dataTables.rows('.selected').data();
    if (data.length == 0) {
        return false;
    }
    var columns = dataTables.settings().init().columns;
    var keys = [];
    var values = [];
    var key = columns[keyIndex].data;
    for (i = 0; i < data.length; i++) {
        keys.push(key);
        values.push(data[i][key]);
    }

    OpenWindowWithPost(url, '_blank', keys, values);
}

function PrintLable(dataTables, url, keyIndex, dataTables2, keyIndex2) {
    var keys = [];
    var values = [];
    if (dataTables != null) {
        var data = dataTables.rows('.selected').data();
        if (data.length != 0) {
           
            var columns = dataTables.settings().init().columns;

            var key = columns[keyIndex].data;
            for (i = 0; i < data.length; i++) {
                keys.push(key);
                values.push(data[i][key]);
            }
        }
       
    }


    if (dataTables2 != null) {
        var data2 = dataTables2.rows('.selected').data();
        if (data2.length != 0) {
            var columns2 = dataTables2.settings().init().columns;
            var key2 = columns2[keyIndex2].data;
            for (i = 0; i < data2.length; i++) {
                keys.push(key2)
                values.push(data2[i][key2]);
            }
        }


    } 

    if (keys.length == 0) {
        return;
    }


    OpenWindowWithPost(url, '_blank', keys, values);
}



function OpenWindowWithPost(url, name, keys, values) {
    var newWindow = window.open(url, name);
    if (!newWindow) {
        return false;
    }

    var html = "";
    html += "<html><head></head><body><form id='formid' method='post' action='" + url + "'>";
    if (keys && values && (keys.length == values.length)) {
        for (var i = 0; i < keys.length; i++) {
            html += "<input type='hidden' name='" + keys[i] + "' value='" + values[i] + "'/>";
        }
    }
    html += "</form><script type='text/javascript'>document.getElementById(\"formid\").submit()</script></body></html>";
    newWindow.document.write(html);
    return newWindow;
}