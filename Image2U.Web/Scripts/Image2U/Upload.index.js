const _loader = document.getElementById("overlay");
const _bsProgress = document.getElementById("progress");
const API_ENDPOINT = "/upload/post";

const [fileSelect, fileElem, uploadFiles] = [
    document.getElementById("fileSelect")
    , document.getElementById("fileElem")
    , document.getElementById("uploadFiles")
];

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

    const isPortaitList = [];

    const form = new FormData();

    for (let image of imgs) {
        const isPortait = image.clientHeight > image.clientWidth;
        form.append("files", image.file);
        isPortaitList.push(isPortait);
    }

    form.append("isPortaits", isPortaitList);

    FileUpload(form);

}, false);

function pageReload() {
    location.reload();
}

function updateProgress(event) {
    if (event.lengthComputable) {

        const percentComplete = (event.loaded / event.total) * 100;

        const pc = Math.round(percentComplete);

        var width = pc;

        const timeId = setInterval(frame, 100);

        function frame() {
            if (width > 100) {
                clearInterval(timeId);
            } else {
                width++;
                _bsProgress.style.width = width + "%";
                _bsProgress.innerHTML = (width - 1) + "%";
            }
        }
    }
}

function completeHandler() { }

function errorHandler() { }

function abortHandler() { }

function FileUpload(form) {
    $.post({
        url: API_ENDPOINT,
        headers: { 'X-Requested-With': "XMLHttpRequest" },
        contentType: false,
        processData: false,
        mimeType: "multipart/form-data",
        data: form,
        xhr: function () {
            const xhr = new window.XMLHttpRequest();
            xhr.upload.addEventListener("progress", updateProgress, false);
            return xhr;
        },
        beforeSend: function () {
            _loader.style.display = "block";
            _bsProgress.classList.value = "progress-bar progress-bar-striped active progress-bar-warning";
        },
        success: function (r) {
            const _response = JSON.parse(r);
            _bsProgress.classList.value = "progress-bar progress-bar-info";
            _loader.style.display = "none";
            if (_response.IsOk) {
                const _url = `/upload/get?tempdataKey=${_response.Data}`;
                window.open(_url);
            }
        }
    });
    return;


    //const request = new XMLHttpRequest();

    //request.upload.addEventListener("progress", updateProgress, false);
    //request.addEventListener("load", completeHandler, false);
    //request.addEventListener("error", errorHandler, false);
    //request.addEventListener("abort", abortHandler, false);

    //console.log(request, request.upload);

    //request.open("POST", API_ENDPOINT, true);
    //request.setRequestHeader("Cache-Control", "no-cache");
    //request.setRequestHeader('Accept', 'multipart/form-data');
    //request.setRequestHeader("X-Requested-With", "XMLHttpRequest");
    ////request.setRequestHeader("Content-Type", "multipart/form-data");
    ////request.setRequestHeader("X-File-Name", file.fileName);
    ////request.setRequestHeader("X-File-Size", file.fileSize);
    ////request.setRequestHeader("X-File-Type", file.type);

    //request.onreadystatechange = function (e) {
    //    if (request.readyState === 4 && request.status === 200) {

    //        _loader.style.display = "none";

    //        const _response = JSON.parse(request.response);

    //        if (_response.IsOk) {
    //            const _url = `/upload/get?tempdataKey=${_response.Data}`;
    //            window.open(_url);
    //        }
    //    }
    //};
    //_loader.style.display = "block";
    //request.send(form);
}

function handleFiles(files) {
    const getTh = function (v) {
        const th = document.createElement("th");
        th.innerHTML = v;
        return th;
    }

    const getTd = function (v) {
        const td = document.createElement("td");
        td.innerHTML = v;
        return td;
    }

    if (!files.length) {
        return;
    }
    const _uploadFiles = document.getElementById("uploadFiles");
    const _fileSelectResult = document.getElementById("fileSelectResult");
    const tableHead = _fileSelectResult.querySelector("thead");
    const tableBody = _fileSelectResult.querySelector("tbody");
    tableHead.innerHTML = "";
    tableBody.innerHTML = "";

    const headTr = document.createElement("tr");

    headTr.appendChild(getTh("#"));
    headTr.appendChild(getTh("Name"));
    headTr.appendChild(getTh("Size"));
    headTr.appendChild(getTh("Preview"));

    const filesCount = files.length;

    _uploadFiles.disabled = false;

    const fileLists = [];
    for (let i = 0; i < filesCount; i++) {
        const file = files[i];
        const fileObj = {
            idx: i,
            name: file.name,
            size: file.size,
            type: file.type.split('/').pop(),
            src: window.URL.createObjectURL(file),
            file: file
        }
        fileLists.push(fileObj);
    }

    for (let i = 0; i < fileLists.length; i++) {
        const file = fileLists[i];

        //const input = document.createElement("input");
        //input.setAttribute("type", "checkbox");
        //console.log(input);

        const tr = document.createElement("tr");

        tr.appendChild(getTd(i + 1));
        tr.appendChild(getTd(file.name));
        tr.appendChild(getTd(file.size.numberFormat(0, '.', ',')));

        const td = document.createElement("td");

        const img = document.createElement("img");
        img.src = file.src;
        img.file = file.file;
        img.onload = function () {
            window.URL.revokeObjectURL(this.src);
        }
        td.appendChild(img);
        tr.appendChild(td);

        tableBody.appendChild(tr);
    }
}

function getOrientation(file, callback) {
    const reader = new FileReader();
    reader.onload = function (e) {

        const view = new DataView(e.target.result);

        if (view.getUint16(0, false) !== 0xFFD8) {
            return callback(-2);
        }
        const length = view.byteLength;

        let offset = 2;

        while (offset < length) {
            if (view.getUint16(offset + 2, false) <= 8) return callback(-1);

            const marker = view.getUint16(offset, false);

            offset += 2;
            if (marker === 0xFFE1) {
                if (view.getUint32(offset += 2, false) !== 0x45786966) {
                    return callback(-1);
                }

                const little = view.getUint16(offset += 6, false) === 0x4949;
                offset += view.getUint32(offset + 4, little);
                const tags = view.getUint16(offset, little);
                offset += 2;
                for (let i = 0; i < tags; i++) {
                    if (view.getUint16(offset + (i * 12), little) === 0x0112) {
                        return callback(view.getUint16(offset + (i * 12) + 8, little));
                    }
                }
            }
            else if ((marker & 0xFF00) !== 0xFF00) {
                break;
            }
            else {
                offset += view.getUint16(offset, false);
            }
        }
        return callback(-1);
    };
    reader.readAsArrayBuffer(file);
}