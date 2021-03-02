const dropZone = document.getElementById("dropbox");

["dragenter", "dragover", "dragleave", "drop"].forEach(eventName => {
    dropZone.addEventListener(eventName, preventDefaults, false);
});

function preventDefaults(e) {
    e.preventDefault();
    e.stopPropagation();
}

["dragenter", "dragover"].forEach(eventName => {
    dropZone.addEventListener(eventName, highlight, false);
});

["dragleave", "drop"].forEach(eventName => {
    dropZone.addEventListener(eventName, unhighlight, false);
});

dropZone.addEventListener("drop", function (e) {
    const dt = e.dataTransfer;
    const files = dt.files;
    console.log(files);
});

function highlight(e) {
    dropZone.classList.add("highlight");
}

function unhighlight(e) {
    dropZone.classList.remove("highlight");
}

function setDropFilesToTable(files) {
    console.log(files);
}