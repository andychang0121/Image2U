const _loader = document.getElementById("overlay");
const _bsProgress = document.getElementById("progress");

const [API_ENDPOINT, API_ENDPOINT_ASYNC] = ["/upload/post", "/upload/postdataAsync"];

const [fileSelect, fileElem, customWidth, customHeight] = [
    document.getElementById("fileSelect")
    , document.getElementById("fileElem")
    , document.getElementById("customWidth")
    , document.getElementById("customHeight")];