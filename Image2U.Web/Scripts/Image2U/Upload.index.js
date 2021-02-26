const _loader = document.getElementById("overlay");
const _bsProgress = document.getElementById("progress");
const API_ENDPOINT = "/upload/post";
const API_ENDPOINT_ASYNC = "/upload/postdataAsync";

const [fileSelect, fileElem, uploadFiles, customWidth, customHeight] = [
    document.getElementById("fileSelect")
    , document.getElementById("fileElem")
    , document.getElementById("uploadFiles")
    , document.getElementById("customWidth")
    , document.getElementById("customHeight")];

let _width = 0;
let _process = { idx: 0, total: 0 };

fileElem.addEventListener("change", function () {
    _bsProgress.classList.value = "progress-bar progress-bar-striped active progress-bar-warning";
});

fileSelect.addEventListener("click", function (e) {
    if (fileElem) {
        fileElem.click();
    };
    e.preventDefault();
}, false);

uploadFiles.addEventListener("click", function (e) {
    e.preventDefault();

    const fileSelectResult = document.getElementById("fileSelectResult");
    const tableBody = fileSelectResult.querySelector("tbody");
    const imgs = tableBody.getElementsByTagName("img");
    setProgress(true);
    setLoaderAsync(true).then(function () {
        uploadFilesBase64Async(imgs, customWidth.value, customHeight.value);
    });

}, false);

function validFileSize(bytes) {
    if (bytes === 0) return true;
    const _getSize = function (_bytes) {
        return _bytes / 1024 / 1024;
    }
    return _getSize(bytes) <= 2;
}

function getUploadFiles(imgs) {
    const rs = [];
    [].forEach.call(imgs, function (img) {
        const _isUpload = validFileSize(img.file.size);
        if (_isUpload) {
            rs.push(img);
        }
    });
    return rs;
}

function uploadFilesBase64Async(imgs, customWidth, customHeight) {
    const ajaxPost = function (image, _requestData, _url) {

        const _file = image.file;

        getFileBase64(_file).then(function (r) {
            _requestData.base64 = r;

            getImage(r).then(function (_r) {
                _requestData.width = _r.width;
                _requestData.height = _r.height;
            });

        }).then(function () {
            _requestData.fileName = image.name;
            _requestData.type = _file.type;
            _requestData.size = _file.size;
            _requestData.fileName = _file.name;
        }).then(function () {
            jUploadFile(_url, _requestData);
        });
    };

    const __imgs = getUploadFiles(imgs);

    _process.idx = __imgs.length;
    _process.total = __imgs.length;

    for (let image of __imgs) {

        const _requestData = {
            customWidth: customWidth,
            customHeight: customHeight
        };

        ajaxPost(image, _requestData, API_ENDPOINT_ASYNC);
    }
}

function jUploadFile(url, data) {
    const _token = $("[name*='__RequestVerificationToken']").val();
    const _request = {
        __RequestVerificationToken: _token,
        requestData: data
    };
    $.post({
        url: url,
        data: _request,
        async: true,
        beforeSend: function () {
            _process.idx--;
            setProgressBar(_process);
        },
        complete: function () {
            if (_process.idx === 0) {
                setLoader(false);
            }
        },
        success: function (r) {
            //https://www.itread01.com/content/1535390541.html
            console.log(r);
            getDownloadFile(r.FileName, r.Result);
            //if (r.IsOk) {
            //    const _url = `/upload/get?tempdataKey=${r.Data}`;
            //    window.open(_url, "_blank");
            //}
        }
    });
}

function setLoaderAsync(b, o) {
    return new Promise((resolve, reject) => {
        setLoader(b);
        setTimeout(() => { resolve(true); }, 10);
    });

}

function setLoader(b, o) {
    const _loader = o === undefined ? document.getElementById("overlay") : o;
    _loader.style.display = b ? "block" : "none";
}

function setProgress(b, o) {
    const _o = o === undefined ? document.getElementById("progress") : o;
    if (b) {
        _o.style.width = "";
        _o.classList.value = "progress-bar progress-bar-striped active progress-bar-success";
    } else {
        _o.classList.value = "progress-bar progress-bar-info";
    }
}

function pageReload() {
    location.reload();
}

function setProgressBar(__process) {

    //if (event.lengthComputable) {

    //    const percentComplete = (event.loaded / event.total) * 100;

    //    const pc = Math.round(percentComplete);
    const idx = __process.total - __process.idx;
    const percentAge = 100 / __process.total;
    const start = idx - 1;
    const end = idx;

    console.log("total:", __process.total, "idx:", idx, "start:", start, "end:", end, "%", percentAge * end + "%");

    for (let i = (start - 1) * 100; i < (end - 1) * 100; i++) {

        _bsProgress.style.width = i + 1 + "%";

        _bsProgress.innerHTML = `${i + 1}% (complete)`;
        console.log();
    }
}

function updateProgress(event) {
    if (event.lengthComputable) {

        const percentComplete = (event.loaded / event.total) * 100;

        const pc = Math.round(percentComplete);

        for (let i = _width; i < pc; i++) {
            _bsProgress.style.width = i + 1 + "%";
            _bsProgress.innerHTML = `${i + 1}% (complete)`;
        }
        _width = pc;
    }
}

function completeHandler() { }

function errorHandler() { }

function abortHandler() { }

function getElement(config) {
    const ele = document.createElement(config.type);
    ele.setAttribute("class", config.className);
    if (config.name) {
        console.log(config.name);
        input.setAttribute("name", config.name);
    }

    ele.textContent = config.text;
    return ele;
}

function setHTMLTRImage(file, i) {
    const _config = {
        type: "button",
        className: "btn btn-default btn-sm",
        text: "Remove"
    };

    const tr = document.createElement("tr");

    //const input = getElement(_config);
    //tr.appendChild(getTd(input));
    tr.appendChild(getTd(i + 1));
    tr.appendChild(getTd(file.name));
    tr.appendChild(getTd(""));
    tr.appendChild(getTd(bytesToSize(file.size)));
    //tr.appendChild(getTd(file.size.numberFormat(0, ".", ",")));
    //----
    const td = document.createElement("td");
    const img = document.createElement("img");

    img.src = file.src;
    img.file = file.file;

    img.onload = function () {
        window.URL.revokeObjectURL(this.src);
    };
    td.appendChild(img);
    tr.appendChild(td);
    return tr;
}

function getTh(v) {
    const th = document.createElement("th");
    th.innerHTML = v;
    return th;
}

function getTd(v) {
    const td = document.createElement("td");
    if (typeof v === "object") {
        td.appendChild(v);
        return td;
    } else if (typeof v === "string" || typeof v === "number") {
        const _span = document.createElement('span');
        _span.innerHTML = v;
        td.appendChild(_span);
    }
    return td;
}

function setFilesToTable(files) {
    if (!files.length) return;

    const setHTMLTableImage = function (_tableBody, _fileLists) {
        for (let i = 0; i < _fileLists.length; i++) {
            const file = _fileLists[i];
            const _tr = setHTMLTRImage(file, i);
            _tableBody.appendChild(_tr);
        }
    };

    const _uploadFiles = document.getElementById("uploadFiles");
    const _fileSelectResult = document.getElementById("fileSelectResult");
    const tableHead = _fileSelectResult.querySelector("thead");
    const tableBody = _fileSelectResult.querySelector("tbody");

    tableHead.innerHTML = "";
    tableBody.innerHTML = "";

    //const headTr = document.createElement("tr");

    //headTr.appendChild(getTh(""));
    //headTr.appendChild(getTh("#"));
    //headTr.appendChild(getTh("Name"));
    //headTr.appendChild(getTh("Size"));
    //headTr.appendChild(getTh("Preview"));

    //tableHead.appendChild(headTr);

    const filesCount = files.length;

    _uploadFiles.disabled = false;

    const fileLists = [];

    for (let i = 0; i < filesCount; i++) {

        const file = files[i];

        const fileObj = {
            idx: i,
            name: file.name,
            size: file.size,
            type: file.type.split("/").pop(),
            file: file,
            src: window.URL.createObjectURL(file)
        };

        fileLists.push(fileObj);
    }
    setHTMLTableImage(tableBody, fileLists);
}

function getFileBase64(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();

        reader.addEventListener("progress", updateProgress, false);

        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = error => reject(error);
    });
}

function getImage(src) {
    return new Promise((resolve, revoke) => {
        const img = new Image();
        img.onload = () => resolve(img);
        img.crossOrigin = "Anonymous";
        img.src = src;
    });
}

function getDownloadFile(fileName, blob) {
    return new window.Promise((resolve, reject) => {
        const binaryString = window.atob(blob);
        const binaryLen = binaryString.length;
        const bytes = new Uint8Array(binaryLen);
        for (let i = 0; i < binaryLen; i++) {
            const ascii = binaryString.charCodeAt(i);
            bytes[i] = ascii;
        }

        const url = window.URL.createObjectURL(new Blob([bytes], { type: "application/zip" }));
        const link = document.createElement("a");
        link.href = url;
        link.setAttribute("download", fileName);
        document.body.appendChild(link);
        link.click();
        resolve();
    });
}