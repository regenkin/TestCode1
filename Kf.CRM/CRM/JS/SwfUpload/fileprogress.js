/*
UI处理
参数:
    file:           SWFUpload文件对象
    targetid:    父容器标识
*/
function FileProgress(file, targetid) {
    //定义文件处理标识
    this.ProgressId = file.id;

    //获取当前容器对象
    this.fileProgressElement = document.getElementById(file.id);

    if (!this.fileProgressElement) {
        //container
        this.fileProgressElement = document.createElement("div");
        this.fileProgressElement.id = file.id;
        this.fileProgressElement.className = swfu.settings.custom_settings.container_css;

        //state button
        this.stateButton = document.createElement("input");
        this.stateButton.type = "button";
        this.stateButton.className = swfu.settings.custom_settings.icoWaiting_css;
        this.fileProgressElement.appendChild(this.stateButton);

        //filename
        this.filenameSpan = document.createElement("span");
        this.filenameSpan.className = swfu.settings.custom_settings.fname_css;
        this.filenameSpan.appendChild(document.createTextNode(file.name));
        this.fileProgressElement.appendChild(this.filenameSpan);

        //filesize
        this.filesizeSpan = document.createElement("span");
        this.filesizeSpan.className = "filesize";
        this.filesizeSpan.appendChild(document.createTextNode(formatUnits(file.size)));
        this.fileProgressElement.appendChild(this.filesizeSpan);

        //statebar div
        this.stateDiv = document.createElement("div");
        this.stateDiv.className = swfu.settings.custom_settings.state_div_css;
        this.stateBar = document.createElement("div");
        this.stateBar.className = swfu.settings.custom_settings.state_bar_css;
        this.stateBar.innerHTML = "&nbsp;";
        this.stateBar.style.width = "0%";
        this.stateDiv.appendChild(this.stateBar);
        this.fileProgressElement.appendChild(this.stateDiv);

        //span percent
        this.percentSpan = document.createElement("span");
        this.percentSpan.className = swfu.settings.custom_settings.percent_css;
        this.percentSpan.style.marginTop = "10px";
        this.percentSpan.innerHTML = "(等待上传中...)";
        this.fileProgressElement.appendChild(this.percentSpan);

        //delete href
        this.hrefSpan = document.createElement("span");
        this.hrefSpan.className = swfu.settings.custom_settings.href_delete_css;
        this.hrefControl = document.createElement("a");
        this.hrefControl.innerHTML = "删除";
        this.hrefControl.onclick = function () {
            swfu.cancelUpload(file.id);
            //alert( document.getElementById(targetid).children.length);
            document.getElementById(targetid).removeChild(document.getElementById(file.id));
            fg_fileSizes -= file.size;
            fg_uploads -= file.size;
            fg_object = new FileGroupProgress();
            fg_object.setFileCountSize(fg_fileSizes);
            fg_object.setUploadProgress(fg_uploads, fg_fileSizes);
            try {
                (typeof (eval('del_attachment')) == "function") && del_attachment(file.id);
            } catch (e) { }
            //alert(file.id);
        }
        this.hrefSpan.appendChild(this.hrefControl);
        this.fileProgressElement.appendChild(this.hrefSpan);

        //view href
        this.hrefSpan = document.createElement("span");
        this.hrefSpan.className = swfu.settings.custom_settings.href_delete_css;
        this.hrefControl = document.createElement("a");
        this.hrefControl.innerHTML = "查看";
        this.hrefControl.onclick = function () {
            //alert(JSON.stringify(file));
            try {
                (typeof (eval('view_attachment')) == "function") && view_attachment(file.name);
            } catch (e) { }
            //alert(file.id);
        }
        this.hrefSpan.appendChild(this.hrefControl);
        this.fileProgressElement.appendChild(this.hrefSpan);

        //insert container
        document.getElementById(targetid).appendChild(this.fileProgressElement);
    }
    else {
        this.reset();
    }
}

//恢复默认设置
FileProgress.prototype.reset = function () {
    this.stateButton = this.fileProgressElement.childNodes[0];
    this.fileSpan = this.fileProgressElement.childNodes[1];
    this.fileSize = this.fileProgressElement.childNodes[2];
    this.stateDiv = this.fileProgressElement.childNodes[3];
    this.stateBar = this.stateDiv.childNodes[0];
    this.percentSpan = this.fileProgressElement.childNodes[4];
    this.hrefSpan = this.fileProgressElement.childNodes[5];
    this.hrefControl = this.hrefSpan.childNodes[0];

    /*this.stateButton.className = swfu.settings.custom_settings.icoNormal_css;*/

    /*this.stateBar.className = swfu.settings.custom_settings.state_bar_css;
    this.stateBar.innerHTML = "&nbsp;";
    this.stateBar.style.width = "0%";*/

    /*this.percentSpan.className = swfu.settings.custom_settings.percent_css;
    this.percentSpan.innerHTML = "";*/
}

/*
设置状态按钮状态
state:        当前状态,1:初始化完成,2:正在等待,3:正在上传
settings:    swfupload.settings对象
*/
FileProgress.prototype.setUploadState = function (state, settings) {
    switch (state) {
        case 1:
            //初始化完成
            this.stateButton.className = settings.custom_settings.icoNormal_css;
            break;
        case 2:
            //正在等待
            this.stateButton.className = settings.custom_settings.icoWaiting_css;
            break;
        case 3:
            //正在上传
            this.stateButton.className = settings.custom_settings.icoUpload_css;
    }
}

/*
设置上传进度
percent:     已上传百分比
*/
FileProgress.prototype.setProgress = function (percent) {
    this.stateBar.style.width = percent + "%";
    this.percentSpan.innerHTML = percent + "%";
    //this.stateButton.className = swfu.settings.custom_settings.icoUpload_css;
    if (percent == 100) {
        //this.hrefSpan.style.display = "none";
        this.stateButton.className = swfu.settings.custom_settings.icoNormal_css;
    }
}

/*
上传完成
*/
FileProgress.prototype.setComplete = function (settings) {
    this.stateButton.className = settings.custom_settings.icoNormal_css;
    //this.hrefSpan.style.display = "none";
}

/*
控制上传进度对象是否显示
*/
FileProgress.prototype.setShow = function (show) {
    this.fileProgressElement.style.display = show ? "" : "none";
}
