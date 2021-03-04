const dropZone = document.getElementById("dropbox");
const fileContainer = document.getElementById("fileContainer");

["dragenter", "dragover", "dragleave", "drop"].forEach(eventName => {
    dropZone.addEventListener(eventName, preventDefaults, false);
});

function preventDefaults(e) {
    e.preventDefault();
    e.stopPropagation();
}

["dragenter", "dragover"].forEach(eventName => {
    fileContainer.addEventListener(eventName, function (e) {
        preventDefaults(e);

        const dt = e.dataTransfer;
        dt.effectAllowed = "none";
        dt.dropEffect = "none";
    }, false);
});

["dragenter", "dragover"].forEach(eventName => {
    dropZone.addEventListener(eventName, highlight, false);
});

["dragleave", "drop"].forEach(eventName => {
    dropZone.addEventListener(eventName, unhighlight, false);
});

dropZone.addEventListener("drop", function (e) {
    const dt = e.dataTransfer;
    const files = dt.files;
    setDropFilesToTable(files);
});

function highlight(e) {
    dropZone.classList.add("highlight");
}

function unhighlight(e) {
    dropZone.classList.remove("highlight");
}

function setUploadFiles(b) {
    const btn = document.getElementById("uploadFiles");
    btn.removeAttribute("disabled");
}

function setDropFilesToTable(files) {
    removeFileResult();
    const setNode = function (o) {
        o.removeAttribute("data-fileResult-template");
        o.removeAttribute("style");
        o.setAttribute("data-fileResult", "");
        return o;
    }
    const setFileInfo = function (o, file) {
        const _name = o.querySelector("[data-file-name]");
        const _size = o.querySelector("[data-file-size]");
        _name.innerHTML = file.name;
        _size.innerHTML = bytesToSize(file.size);
        return o;
    }
    const setImage = function (o, src, file) {
        const _img = o.querySelector("[data-img]").getElementsByTagName("img");
        _img[0].src = src;
        _img[0].file = file;
        return o;
    }
    const _fileResult = document.getElementById("fileResult");
    const _template = document.querySelector("[data-fileResult-template]");

    [].forEach.call(files, (_file) => {
        getFileBase64(_file).then(function (r) {
            let _fileRow = setNode(_template.cloneNode(true));
            _fileRow.isUpload = validFileSize(_file.size);
            _fileRow = setFileInfo(_fileRow, _file);
            _fileRow = setImage(_fileRow, r, _file);

            setAlert(_fileRow, _fileRow.isUpload);
            _fileResult.appendChild(_fileRow);
        });
    });
    setUploadFiles();
}

function setAlert(o, isUpload) {
    const _alert = o.querySelector("[data-alert]");
    if (isUpload) {
        _alert.remove();
        return;
    }
    o.querySelector("[data-progress]").remove();
    _alert.removeAttribute("style");
    return;
}

function uploadFilesAction() {
    const customSize = getCustomSize();
    const _fileRs = document.querySelectorAll("[data-fileResult]");

    [].forEach.call(_fileRs, function (_rs) {
        const _progress = _rs.querySelector("[data-progress]");
        const _image = _rs.getElementsByTagName("img")[0];
        const _file = _image.file;

        if (validFileSize(_file.size)) {
            const _request = {
                fileName: _file.name,
                size: _file.size,
                type: _file.type,
                base64: _image.src,
                customWidth: customSize.customWidth,
                customHeight: customSize.customHeight
            };
            getImage(_image.src).then(function (_r) {
                _request.width = _r.width;
                _request.height = _r.height;
            }).then(() => {
                _progress.style.display = "";

                const _progressbar = _progress.querySelector("[data-progress-bar]");
                const _requestToken = document.getElementsByName("__RequestVerificationToken");
                const _requestData = {
                    __RequestVerificationToken: _requestToken[0].value,
                    requestData: _request
                };

                postUploadFile("/upload/postdataAsync", _requestData, _progressbar);
            });
        }
    });
}

function setAjaxLoader(b) {
    const _loader = document.getElementById("overlay");
    _loader.style.display = b ? "block" : "none";
}

function postUploadFile(url, data, progressbar) {
    $.post({
        xhr: function () {
            const xhr = new window.XMLHttpRequest();
            xhr.upload.addEventListener("progress", function (event) {
                if (event.lengthComputable) {
                    const percentComplete = (event.loaded / event.total) * 100;
                    const pc = (Math.round(percentComplete));
                    progressbar.style.width = `${pc}%`;
                }
            }, false);
            return xhr;
        },
        xhrFields: {
            onprogress: function (event) {
                if (event.lengthComputable) {
                    if (event.lengthComputable) {
                        const percentComplete = (event.loaded / event.total) * 100;
                        const pc = (Math.round(percentComplete));

                        progressbar.classList.remove("progress-bar-danger");
                        progressbar.style.width = progressbar.style.width === "100%"
                            ? "0%" : progressbar.style.width;

                        progressbar.style.width = pc + "%";
                    }
                }
            }
        },
        url: url,
        data: data,
        beforeSend: function () {
            setAjaxLoader(true);
        },
        success: function (r, textStatus, jqXHR) {
            getDownloadFile(r.FileName, r.Result).then(function (link) {
                link.remove();
            });
        },
        complete: function () {
            setAjaxLoader(false);
        }
    });
}

function removeFileResult() {
    const _f1 = document.querySelectorAll("[data-fileResult]");
    [].forEach.call(_f1, (fs) => {
        fs.remove();
    });
    return;
}

function removeSelected(o) {
    const _fileResult = getParentNodeBySelector(o.parentNode, "[data-fileResult]");
    if (_fileResult) _fileResult.remove();
}