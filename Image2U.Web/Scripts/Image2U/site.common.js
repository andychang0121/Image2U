function goTop(x, y) {
    x = x === undefined || x === null ? 0 : x;
    y = y === undefined || y === null ? 0 : x;
    window.scrollTo(x, y);
}

function preventDefaults(e) {
    e.preventDefault();
    e.stopPropagation();
}

function pageReload() {
    location.reload();
    goTop();
}

