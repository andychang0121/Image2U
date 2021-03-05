const resetBtn = document.getElementById("resetFiles");

(function (o) {
    if (!o) return;
    o.addEventListener("click", function (e) {
        preventDefaults(e);
        pageReload();
    });
})(resetBtn);

