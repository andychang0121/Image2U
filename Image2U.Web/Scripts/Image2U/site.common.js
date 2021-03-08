const resetBtn = document.getElementById("resetFiles");

(function (o) {
    if (!o) return;

    o.addEventListener("click", function (e) {
        preventDefaults(e);
        pageReload();
    });
})(resetBtn);

function preventDefaults(e) {
    e.preventDefault();
    e.stopPropagation();
}

function pageReload() {
    location.reload();
    window.scrollTo(0, 0);
}

