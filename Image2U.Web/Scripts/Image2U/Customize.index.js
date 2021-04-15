((t, o) => {
    const _target = document.getElementById(t);
    const _result = document.getElementById(o);

    if (!_target || !_result) return;

    _target.addEventListener("click", function (e) {
        if (_result) {
            _result.click();
        };
        preventDefaults(e);
    }, false);

})("selectcustomfile", "fileElem");

((s, i) => {
    const _o = document.getElementById(s);

    _o.addEventListener("click", function (e) {
        preventDefaults(e);
        const _i = document.getElementById(i);
        if (!_i || !_i.files) return;

        const _files = _i.files[0];
        //1: toBase64
        //2: post
        //3: download

    }, false);
})("executeCustomize", "fileElem");

function setCustomizeFile(o) {
    const _f = o ? o[0] : undefined;
    if (!_f) return;
    const _name = _f.name;
    const _input = document.getElementById("uploadcustomizefile");
    const _execute = document.getElementById("executeCustomize");
    _input.value = _name;
    _execute.disabled = false;
}