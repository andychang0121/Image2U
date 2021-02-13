const _loader = document.getElementById("overlay");
const _bsProgress = document.getElementById("progress");
const API_ENDPOINT = "/upload/post";

const [fileSelect, fileElem, uploadFiles, customWidth, customHeight] = [
    document.getElementById("fileSelect")
    , document.getElementById("fileElem")
    , document.getElementById("uploadFiles")
    , document.getElementById("customWidth")
    , document.getElementById("customHeight")];

let _width = 0;

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

    multiUploadFiles(imgs);

}, false);

function singleUploadFiles(imgs) {
    for (let image of imgs) {
        const isPortaitList = [];
        const form = new FormData();
        const isPortait = image.clientHeight > image.clientWidth;
        form.append("files", image.file);
        isPortaitList.push(isPortait);
        form.append("isPortaits", isPortaitList);
        FileUpload(form);
    }
}

function multiUploadFiles(imgs) {
    const isPortaitList = [];

    const form = new FormData();
    form.append("customWidth", customWidth.value);
    form.append("customHeight", customHeight.value);

    for (let image of imgs) {
        const isPortait = image.clientHeight > image.clientWidth;
        form.append("files", image.file);
        isPortaitList.push(isPortait);
    }

    form.append("isPortaits", isPortaitList);

    FileUpload(form);
}

function pageReload() {
    location.reload();
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

function FileUpload(form) {
    const request = new XMLHttpRequest();
    request.upload.addEventListener("progress", updateProgress, false);
    request.addEventListener("load", completeHandler, false);
    request.addEventListener("error", errorHandler, false);
    request.addEventListener("abort", abortHandler, false);
    request.open("POST", API_ENDPOINT, true);
    request.setRequestHeader("Cache-Control", "no-cache");
    request.setRequestHeader('Accept', 'multipart/form-data');
    request.setRequestHeader("X-Requested-With", "XMLHttpRequest");

    request.onreadystatechange = function (e) {
        if (request.readyState === 4 && request.status === 200) {
            _bsProgress.classList.value = "progress-bar progress-bar-info";
            _loader.style.display = "none";

            try {
                const _response = JSON.parse(request.response);

                if (_response.IsOk) {
                    const _url = `/upload/get?tempdataKey=${_response.Data}`;
                    window.open(_url);
                }
            } catch (e) {

            }

        }
    };
    _loader.style.display = "block";
    _bsProgress.style.width = "";
    _bsProgress.classList.value = "progress-bar progress-bar-striped active progress-bar-success";
    request.send(form);
}

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
    tr.appendChild(getTd(file.size.numberFormat(0, '.', ',')));
    //----
    const td = document.createElement("td");
    const img = document.createElement("img");

    img.src = file.src;
    img.file = file.file;
    img.onload = function () {
        window.URL.revokeObjectURL(this.src);
    }

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
    }
    td.innerHTML = v;
    return td;
}

function handleFiles(files) {
    if (!files.length) return;

    const setHTMLTableImage = function (_tableBody, _fileLists) {
        for (let i = 0; i < _fileLists.length; i++) {
            const file = _fileLists[i];
            const tr = setHTMLTRImage(file, i);
            _tableBody.appendChild(tr);
        }
    }

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
            width: 0,
            height: 0,
            type: file.type.split("/").pop(),
            file: file,
            src: window.URL.createObjectURL(file)
        }

        getSize(file).then((e) => {
            fileObj.width = e.width;
            fileObj.height = e.height;
        });
        fileLists.push(fileObj);
    }
    setHTMLTableImage(tableBody, fileLists);
}

function getSize(file) {
    return new Promise((resolve, reject) => {
        var _URL = window.URL || window.webkitURL;
        var img = new Image();
        img.onload = () => resolve({ height: img.height, width: img.width });
        img.onerror = reject;
        img.src = _URL.createObjectURL(file);
    });
}
