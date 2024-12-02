var valiableData = ["myForm", "getElementById", "abilitytxt", "samplePlanner", "languageSelect", "getItem",
    "btFullscreen", "saveSamplePlan", "selectOptions", "parse", "value", "text", "map", "options", "from", "some", "filter",
    "top11SamplePlanOptions", "stringify", "setItem", "remove", "length", "#training-planner tbody tr", "querySelectorAll",
    "mouseover", "backgroundColor", "style", "lightgreen", "addEventListener", "mouseout", "", "forEach", "training-planner", "keydown", "key", "Tab", "Enter", "preventDefault", "target", "parentElement", "indexOf", "rows", "input", "querySelector", "cellIndex", "cells", "focus", "select", "bangSoSanh", "td", "getElementsByTagName", "removeAttribute", "soSanhContainer", "display", "flex", "none", "#bangSoSanh .PA1", "#bangSoSanh .PA2", "#bangSoSanh .PA3", "#bangSoSanh .xoaSoSanh", "rowXoaButton", "hidden", "innerHTML", "graySkillThead", "contains", "classList", "gray", "https://top11experience.com/wp-content/themes/top11trainplanner/pro-getPlayerList.php", "savedPlayers-select", "param1=", "POST", "open", "Content-type", "application/x-www-form-urlencoded", "setRequestHeader", "onreadystatechange", "readyState", "status", "responseText", "send", "GET", "toLowerCase", "userAgent", "android", "iphone", "ipad", "ipod", "#training-planner .select-option option", "includes", "title", " - ", "cyan", "white", "index", "color", "black", "red", "split", "join", "slice", "resultTable", ".close-button", "block", "defTable", "graySkillTbody", "toFixed", "%", "attackTable", "physicalTable", "summary2", "summary3", "onchange", "selectedIndex", "button", "pro-langDic", "langCode", "langDic", ".langChange", "+", "#rolestxt", "GK", "language-select", "trim", ":", ".training-page .select-option option", "|", " | ", "https://top11experience.com/wp-content/themes/top11trainplanner/pro-langDic.php", "langCode=", "push", "top11experiencePlayerData", "label", "option", "createElement", "plan", "appendChild", "beforeunload", "B\u1ea1n c\xF3 ch\u1eafc mu\u1ed1n r\u1eddi kh\u1ecfi trang?", "returnValue", "DOMContentLoaded", "cloneNode", "nextSibling", "insertBefore", "parentNode", "removeChild", "Tr\xECnh duy\u1ec7t h\u1ed7 tr\u1ee3 thu\u1ed9c t\xEDnh hidden c\u1ee7a c\xE1c ph\u1ea7n t\u1eed option trong select.", "Tr\xECnh duy\u1ec7t kh\xF4ng h\u1ed7 tr\u1ee3 thu\u1ed9c t\xEDnh hidden c\u1ee7a c\xE1c ph\u1ea7n t\u1eed option trong select.", "add", "rowIndex", ",", ".select-option", "top11experiencePlayerSkillCol", "https://top11experience.com/wp-content/themes/top11trainplanner/pro-trainingPlanCalcu.php", "Content-Type", "<br>", "lightsalmon", "fontWeight", "normal", "blue", "bold", "disabled", "diemChuan=", "&diemTrungBinhBaiTap=", "&diemMucTieu=", "&soKyNang=", "&roles=", "#playerinfo-form input", "abilityCalbt", "rolestxt", "L", "L/R", "replace", "R", "isArray", "https://top11experience.com/wp-content/themes/top11trainplanner/pro-whitecol.php", "0", "whiteSkillThead", "whiteSkillTbody", "innerText", "substring", "roles=", "&skills=", "children", "tagName", "click", ".training-page", "fullscreen", "Exit Fullscreen", "requestFullscreen", "documentElement", "mozRequestFullScreen", "webkitRequestFullscreen", "msRequestFullscreen", "Fullscreen", "exitFullscreen", "mozCancelFullScreen", "webkitExitFullscreen", "msExitFullscreen", "inputValue", ": ", "x %", "nametxt", " : ", " ", "!", "param=", "Deleted ", "?", "splice", "https://top11experience.com/wp-content/themes/top11trainplanner/pro-savePlan.php", "[planLabelText]", ".overlay", "submit", "talentSelect", "ageSelect", "talentIndex=", "&ageIndex=", "DONE", "\u0110\xE3 x\u1ea3y ra l\u1ed7i khi g\u1eedi y\xEAu c\u1ea7u t\xEDnh to\xE1n \u0111\u1ebfn server.", "error", "https://top11experience.com/wp-content/themes/top11trainplanner/abilityCalcu_new.php", "toUpperCase", "\u0110\xE3 x\u1ea3y ra l\u1ed7i khi t\u1ea3i m\xE3 HTML t\u1eeb file PHP.", "https://top11experience.com/wp-content/themes/top11trainplanner/abilityForm.php", "?lang="];
var lang = [];
const langlengthPro = 108;
var form = document.getElementById("myForm");
var abilityInput = document.getElementById("abilitytxt");
var selectElement = document.getElementById("samplePlanner");
var selectedLanguage = localStorage.getItem("languageSelect");
var fullscreenButton = document.getElementById("btFullscreen");
var btSaveSamplePlan = document.getElementById("saveSamplePlan");
var loadAbilityForm = false;

function saveOptionsToLocalStorage() {
    const selectElement = document.getElementById("samplePlanner");
    const _0x202A4 = JSON.parse(localStorage.getItem("selectOptions")) || [];
    const _0x1F052 = Array.from(selectElement.options).map((_0x20155) => {
        return {
            value: _0x20155.value,
            text: _0x20155.text
        }
    });
    const _0x202E7 = _0x1F052.filter((_0x20155) => {
        return !_0x202A4.some((_0x2036D) => {
            return _0x2036D.value === _0x20155.value
        })
    });
    const _0x2032A = [..._0x202A4, ..._0x202E7];
    localStorage.setItem("top11SamplePlanOptions", JSON.stringify(_0x2032A));
    while (selectElement.options.length > 0) {
        selectElement.options.remove(0)
    }
}
const rows = document.querySelectorAll("#training-planner tbody tr");
rows[valiableData[31]]((_0x1E018) => {
    _0x1E018.addEventListener("mouseover", () => {
        _0x1E018.style.backgroundColor = "lightgreen"
    });
    _0x1E018.addEventListener("mouseout", () => {
        _0x1E018.style.backgroundColor = ""
    })
});
const trainingTable = document.getElementById(valiableData[32]);
trainingTable.addEventListener(valiableData[33], (_0x1E167) => {
    if (_0x1E167[valiableData[34]] === valiableData[35] || _0x1E167[valiableData[34]] === valiableData[36]) {
        _0x1E167[valiableData[37]]();
        const _0x1E09E = _0x1E167[valiableData[38]];
        const _0x1E05B = _0x1E09E[valiableData[39]];
        const _0x1E0E1 = _0x1E05B[valiableData[39]];
        const _0x1E124 = Array.from(table[valiableData[41]])[valiableData[40]](_0x1E0E1);
        const _0x1E1ED = table[valiableData[41]][_0x1E124 + 1];
        if (_0x1E1ED) {
            const _0x1E1AA = _0x1E1ED[valiableData[45]][_0x1E05B[valiableData[44]]][valiableData[43]](valiableData[42]);
            if (_0x1E1AA) {
                _0x1E1AA[valiableData[46]]();
                _0x1E1AA[valiableData[47]]()
            }
        }
    }
});

function removeSoSanhCellStyle() {
    const _0x1E2F9 = document.getElementById(valiableData[48]);
    const _0x20198 = _0x1E2F9[valiableData[50]](valiableData[49]);
    for (let _0x1E597 = 0; _0x1E597 < 60; _0x1E597++) {
        _0x20198[_0x1E597][valiableData[51]](valiableData[26])
    }
}

function xemSoSanh() {
    var _0x1EBDF = document.getElementById(valiableData[52]);
    _0x1EBDF.style[valiableData[53]] = valiableData[54]
}

function closeSoSanhTable() {
    var _0x1EBDF = document.getElementById(valiableData[52]);
    _0x1EBDF.style[valiableData[53]] = valiableData[55]
}

function addSoSanh() {
    var _0x1EAD3 = document.getElementById(valiableData[32]);
    var _0x1E018 = _0x1EAD3[valiableData[41]][2];
    var _0x1EC22 = document.querySelectorAll(valiableData[56]);
    var _0x1EC65 = document.querySelectorAll(valiableData[57]);
    var _0x1ECA8 = document.querySelectorAll(valiableData[58]);
    var _0x1EBDF = document.getElementById(valiableData[48]);
    var _0x1ED71 = document.querySelectorAll(valiableData[59]);
    var _0x1EB9C = document.getElementById(valiableData[60]);
    _0x1EB9C[valiableData[61]] = false;
    var _0x1EB59 = -1;
    var _0x1ED2E = _0x1EC22;
    for (var _0x1E597 = 0; _0x1E597 < _0x1ED71.length; _0x1E597++) {
        if (_0x1ED71[_0x1E597][valiableData[61]] === true) {
            _0x1EB59 = _0x1E597;
            _0x1ED71[_0x1E597][valiableData[61]] = false;
            break
        }
    };
    if (_0x1EB59 === 0) {
        _0x1ED2E = _0x1EC22
    } else {
        if (_0x1EB59 === 1) {
            _0x1ED2E = _0x1EC65
        } else {
            if (_0x1EB59 === 2) {
                _0x1ED2E = _0x1ECA8
            } else {
                alert(lang[106])
            }
        }
    };
    for (var _0x1E597 = 0; _0x1E597 < _0x1ED2E.length; _0x1E597++) {
        var _0x1ECEB = _0x1E018[valiableData[45]][_0x1E597 + 3];
        _0x1ED2E[_0x1E597][valiableData[62]] = _0x1ECEB[valiableData[62]];
        var _0x1E0E1 = _0x1EBDF[valiableData[41]][_0x1E597 + 1];
        if (_0x1ECEB[valiableData[65]][valiableData[64]](valiableData[63])) {
            for (var _0x1EB16 = 0; _0x1EB16 < 4; _0x1EB16++) {
                _0x1E0E1[valiableData[45]][_0x1EB16].style.backgroundColor = valiableData[66]
            }
        } else {
            for (var _0x1EB16 = 0; _0x1EB16 < 4; _0x1EB16++) {
                if (_0x1E0E1[valiableData[45]][_0x1EB16].style.backgroundColor === valiableData[66]) {
                    _0x1E0E1[valiableData[45]][_0x1EB16][valiableData[51]](valiableData[26])
                }
            }
        }
    }
}

function xoaPA1() {
    var _0x209B5 = document.querySelectorAll(valiableData[56]);
    for (var _0x1E597 = 0; _0x1E597 < _0x209B5.length; _0x1E597++) {
        _0x209B5[_0x1E597][valiableData[62]] = ""
    };
    var _0x1ED71 = document.querySelectorAll(valiableData[59]);
    _0x1ED71[0][valiableData[61]] = true;
    if (_0x1ED71[0][valiableData[61]] === true && _0x1ED71[1][valiableData[61]] === true && _0x1ED71[2][valiableData[61]] === true) {
        var _0x1EB9C = document.getElementById(valiableData[60]);
        _0x1EB9C[valiableData[61]] = true
    }
}

function xoaPA2() {
    var _0x209B5 = document.querySelectorAll(valiableData[57]);
    for (var _0x1E597 = 0; _0x1E597 < _0x209B5.length; _0x1E597++) {
        _0x209B5[_0x1E597][valiableData[62]] = ""
    };
    var _0x1ED71 = document.querySelectorAll(valiableData[59]);
    _0x1ED71[1][valiableData[61]] = true;
    if (_0x1ED71[0][valiableData[61]] === true && _0x1ED71[1][valiableData[61]] === true && _0x1ED71[2][valiableData[61]] === true) {
        var _0x1EB9C = document.getElementById(valiableData[60]);
        _0x1EB9C[valiableData[61]] = true
    }
}

function xoaPA3() {
    var _0x209B5 = document.querySelectorAll(valiableData[58]);
    for (var _0x1E597 = 0; _0x1E597 < _0x209B5.length; _0x1E597++) {
        _0x209B5[_0x1E597][valiableData[62]] = ""
    };
    var _0x1ED71 = document.querySelectorAll(valiableData[59]);
    _0x1ED71[2][valiableData[61]] = true;
    if (_0x1ED71[0][valiableData[61]] === true && _0x1ED71[1][valiableData[61]] === true && _0x1ED71[2][valiableData[61]] === true) {
        var _0x1EB9C = document.getElementById(valiableData[60]);
        _0x1EB9C[valiableData[61]] = true
    }
}

function sendData() {
    var _0x1E984 = new XMLHttpRequest();
    var _0x1E9C7 = valiableData[67];
    var selectElement = document.getElementById(valiableData[68]);
    var _0x1F3B9 = selectElement.value;
    var _0x1E511 = valiableData[69] + _0x1F3B9;
    _0x1E984[valiableData[71]](valiableData[70], _0x1E9C7, true);
    _0x1E984[valiableData[74]](valiableData[72], valiableData[73]);
    _0x1E984[valiableData[75]] = function () {
        if (_0x1E984[valiableData[76]] === 4 && _0x1E984[valiableData[77]] === 200) {
            var _0x203B0 = _0x1E984[valiableData[78]]
        }
    };
    _0x1E984[valiableData[79]](_0x1E511)
}

function loadData() {
    var _0x1E984 = new XMLHttpRequest();
    _0x1E984[valiableData[71]](valiableData[80], valiableData[67], true);
    _0x1E984[valiableData[75]] = function () {
        if (_0x1E984[valiableData[76]] === 4 && _0x1E984[valiableData[77]] === 200) {
            var _0x1E511 = JSON.parse(_0x1E984[valiableData[78]])
        }
    };
    _0x1E984[valiableData[79]]()
}

function isMobileDevice() {
    var _0x1FD68 = navigator[valiableData[82]][valiableData[81]]();
    if (_0x1FD68[valiableData[40]](valiableData[83]) > -1 || _0x1FD68[valiableData[40]](valiableData[84]) > -1 || _0x1FD68[valiableData[40]](valiableData[85]) > -1 || _0x1FD68[valiableData[40]](valiableData[86]) > -1) {
        return true
    } else {
        return false
    }
}

function findSkill(_0x1FB50) {
    var _0x1F052 = document.querySelectorAll(valiableData[87]);
    var _0x1FC19 = document.getElementById(valiableData[32]);
    var _0x1FB93 = _0x1FB50[valiableData[62]];
    for (var _0x1FBD6 = 0; _0x1FBD6 < _0x1F052.length; _0x1FBD6++) {
        if (_0x1F052[_0x1FBD6][valiableData[89]][valiableData[88]](_0x1FB93)) {
            _0x1F052[_0x1FBD6][valiableData[62]] = removeTextBeforeDash(_0x1F052[_0x1FBD6][valiableData[62]]);
            _0x1F052[_0x1FBD6][valiableData[62]] = _0x1FB93 + valiableData[90] + _0x1F052[_0x1FBD6][valiableData[62]];
            _0x1F052[_0x1FBD6].style.backgroundColor = valiableData[91]
        } else {
            _0x1F052[_0x1FBD6][valiableData[62]] = removeTextBeforeDash(_0x1F052[_0x1FBD6][valiableData[62]]);
            _0x1F052[_0x1FBD6].style.backgroundColor = valiableData[92]
        };
        if (_0x1FBD6 < 18 && _0x1FBD6 != _0x1FB50[valiableData[93]]) {
            _0x1FC19[valiableData[41]][0][valiableData[45]][_0x1FBD6].style[valiableData[94]] = valiableData[95]
        };
        _0x1FB50.style[valiableData[94]] = valiableData[96]
    }
}

function removeTextBeforeDash(_0x201DB) {
    const _0x1F938 = _0x201DB[valiableData[97]](valiableData[90]);
    if (_0x1F938.length > 1) {
        return _0x1F938[valiableData[99]](1)[valiableData[98]](valiableData[90])
    };
    return _0x201DB
}

function showTable() {
    var _0x1E2F9 = document.getElementById(valiableData[100]);
    _0x1E2F9.style[valiableData[53]] = valiableData[54];
    document[valiableData[43]](valiableData[101]).style[valiableData[53]] = valiableData[102];
    var _0x20049 = document.getElementById(valiableData[32]);
    var _0x20823 = document.getElementById(valiableData[103]);
    _0x20823[valiableData[41]][0][valiableData[45]][0][valiableData[62]] = lang[10];
    _0x20823[valiableData[41]][1][valiableData[45]][0][valiableData[62]] = lang[101];
    _0x20823[valiableData[41]][1][valiableData[45]][1][valiableData[62]] = lang[99];
    _0x20823[valiableData[41]][1][valiableData[45]][2][valiableData[62]] = lang[100];
    var _0x20866 = 0;
    var _0x208A9 = 0;
    for (var _0x1E597 = 2; _0x1E597 < _0x20823[valiableData[41]].length; _0x1E597++) {
        if (_0x1E597 < 7) {
            for (var _0x1EB16 = 0; _0x1EB16 < 3; _0x1EB16++) {
                _0x20823[valiableData[41]][_0x1E597][valiableData[45]][_0x1EB16][valiableData[62]] = _0x20049[valiableData[41]][_0x1EB16][valiableData[45]][_0x1E597 + 1][valiableData[62]];
                _0x20823[valiableData[41]][_0x1E597][valiableData[45]][_0x1EB16].style.backgroundColor = _0x20049[valiableData[41]][3][valiableData[45]][_0x1E597 + 1][valiableData[65]][valiableData[64]](valiableData[104]) ? valiableData[66] : ""
            };
            _0x20866 += parseInt(_0x20049[valiableData[41]][1][valiableData[45]][_0x1E597 + 1][valiableData[62]]);
            _0x208A9 += parseInt(_0x20049[valiableData[41]][2][valiableData[45]][_0x1E597 + 1][valiableData[62]])
        } else {
            _0x20823[valiableData[41]][_0x1E597][valiableData[45]][0][valiableData[62]] = lang[98];
            _0x20823[valiableData[41]][_0x1E597][valiableData[45]][1][valiableData[62]] = parseFloat(_0x20866 / 5)[valiableData[105]](1) + valiableData[106];
            _0x20823[valiableData[41]][_0x1E597][valiableData[45]][2][valiableData[62]] = parseFloat(_0x208A9 / 5)[valiableData[105]](1) + valiableData[106]
        }
    };
    _0x20823 = document.getElementById(valiableData[107]);
    _0x20823[valiableData[41]][0][valiableData[45]][0][valiableData[62]] = lang[16];
    _0x20823[valiableData[41]][1][valiableData[45]][0][valiableData[62]] = lang[101];
    _0x20823[valiableData[41]][1][valiableData[45]][1][valiableData[62]] = lang[99];
    _0x20823[valiableData[41]][1][valiableData[45]][2][valiableData[62]] = lang[100];
    _0x20866 = 0;
    _0x208A9 = 0;
    for (var _0x1E597 = 2; _0x1E597 < _0x20823[valiableData[41]].length; _0x1E597++) {
        if (_0x1E597 < 7) {
            for (var _0x1EB16 = 0; _0x1EB16 < 3; _0x1EB16++) {
                _0x20823[valiableData[41]][_0x1E597][valiableData[45]][_0x1EB16][valiableData[62]] = _0x20049[valiableData[41]][_0x1EB16][valiableData[45]][_0x1E597 + 6][valiableData[62]];
                _0x20823[valiableData[41]][_0x1E597][valiableData[45]][_0x1EB16].style.backgroundColor = _0x20049[valiableData[41]][3][valiableData[45]][_0x1E597 + 6][valiableData[65]][valiableData[64]](valiableData[104]) ? valiableData[66] : ""
            };
            _0x20866 += parseInt(_0x20049[valiableData[41]][1][valiableData[45]][_0x1E597 + 6][valiableData[62]]);
            _0x208A9 += parseInt(_0x20049[valiableData[41]][2][valiableData[45]][_0x1E597 + 6][valiableData[62]])
        } else {
            _0x20823[valiableData[41]][_0x1E597][valiableData[45]][0][valiableData[62]] = lang[98];
            _0x20823[valiableData[41]][_0x1E597][valiableData[45]][1][valiableData[62]] = parseFloat(_0x20866 / 5)[valiableData[105]](1) + valiableData[106];
            _0x20823[valiableData[41]][_0x1E597][valiableData[45]][2][valiableData[62]] = parseFloat(_0x208A9 / 5)[valiableData[105]](1) + valiableData[106]
        }
    };
    _0x20823 = document.getElementById(valiableData[108]);
    _0x20823[valiableData[41]][0][valiableData[45]][0][valiableData[62]] = lang[22];
    _0x20823[valiableData[41]][1][valiableData[45]][0][valiableData[62]] = lang[101];
    _0x20823[valiableData[41]][1][valiableData[45]][1][valiableData[62]] = lang[99];
    _0x20823[valiableData[41]][1][valiableData[45]][2][valiableData[62]] = lang[100];
    _0x20866 = 0;
    _0x208A9 = 0;
    for (var _0x1E597 = 2; _0x1E597 < _0x20823[valiableData[41]].length; _0x1E597++) {
        if (_0x1E597 < 7) {
            for (var _0x1EB16 = 0; _0x1EB16 < 3; _0x1EB16++) {
                _0x20823[valiableData[41]][_0x1E597][valiableData[45]][_0x1EB16][valiableData[62]] = _0x20049[valiableData[41]][_0x1EB16][valiableData[45]][_0x1E597 + 11][valiableData[62]];
                _0x20823[valiableData[41]][_0x1E597][valiableData[45]][_0x1EB16].style.backgroundColor = _0x20049[valiableData[41]][3][valiableData[45]][_0x1E597 + 11][valiableData[65]][valiableData[64]](valiableData[104]) ? valiableData[66] : ""
            };
            _0x20866 += parseInt(_0x20049[valiableData[41]][1][valiableData[45]][_0x1E597 + 11][valiableData[62]]);
            _0x208A9 += parseInt(_0x20049[valiableData[41]][2][valiableData[45]][_0x1E597 + 11][valiableData[62]])
        } else {
            _0x20823[valiableData[41]][_0x1E597][valiableData[45]][0][valiableData[62]] = lang[98];
            _0x20823[valiableData[41]][_0x1E597][valiableData[45]][1][valiableData[62]] = parseFloat(_0x20866 / 5)[valiableData[105]](1) + valiableData[106];
            _0x20823[valiableData[41]][_0x1E597][valiableData[45]][2][valiableData[62]] = parseFloat(_0x208A9 / 5)[valiableData[105]](1) + valiableData[106]
        }
    };
    _0x20823 = document.getElementById(valiableData[109]);
    _0x20823[valiableData[41]][0][valiableData[45]][0][valiableData[62]] = _0x20049[valiableData[41]][0][valiableData[45]][18][valiableData[62]];
    _0x20823[valiableData[41]][0][valiableData[45]][1][valiableData[62]] = _0x20049[valiableData[41]][0][valiableData[45]][19][valiableData[62]];
    _0x20823[valiableData[41]][1][valiableData[45]][0][valiableData[62]] = lang[99];
    _0x20823[valiableData[41]][1][valiableData[45]][2][valiableData[62]] = lang[99];
    _0x20823[valiableData[41]][1][valiableData[45]][1][valiableData[62]] = lang[100];
    _0x20823[valiableData[41]][1][valiableData[45]][3][valiableData[62]] = lang[100];
    _0x20823[valiableData[41]][2][valiableData[45]][0][valiableData[62]] = _0x20049[valiableData[41]][1][valiableData[45]][18][valiableData[62]];
    _0x20823[valiableData[41]][2][valiableData[45]][1][valiableData[62]] = _0x20049[valiableData[41]][2][valiableData[45]][18][valiableData[62]];
    _0x20823[valiableData[41]][2][valiableData[45]][2][valiableData[62]] = _0x20049[valiableData[41]][1][valiableData[45]][19][valiableData[62]];
    _0x20823[valiableData[41]][2][valiableData[45]][3][valiableData[62]] = _0x20049[valiableData[41]][2][valiableData[45]][19][valiableData[62]];
    _0x20823 = document.getElementById(valiableData[110]);
    _0x20823[valiableData[41]][0][valiableData[45]][0][valiableData[62]] = _0x20049[valiableData[41]][0][valiableData[45]][20][valiableData[62]];
    _0x20823[valiableData[41]][0][valiableData[45]][1][valiableData[62]] = _0x20049[valiableData[41]][0][valiableData[45]][21][valiableData[62]];
    _0x20823[valiableData[41]][1][valiableData[45]][0][valiableData[62]] = _0x20049[valiableData[41]][2][valiableData[45]][20][valiableData[62]];
    _0x20823[valiableData[41]][1][valiableData[45]][1][valiableData[62]] = _0x20049[valiableData[41]][2][valiableData[45]][21][valiableData[62]]
}

function closeTable() {
    var _0x1E2F9 = document.getElementById(valiableData[100]);
    _0x1E2F9.style[valiableData[53]] = valiableData[55];
    document[valiableData[43]](valiableData[101]).style[valiableData[53]] = valiableData[55]
}
selectElement[valiableData[111]] = function () {
    var _0x1E2F9 = document.getElementById(valiableData[32]);
    var _0x1E273 = _0x1E2F9[valiableData[41]].length;
    addRow();
    if (selectElement[valiableData[112]] === 0) {
        for (var _0x1E33C = 1; _0x1E33C < _0x1E273 - 2; _0x1E33C++) {
            var _0x1E018 = _0x1E2F9[valiableData[41]][_0x1E273 - _0x1E33C];
            var _0x1E230 = _0x1E018[valiableData[45]][22][valiableData[43]](valiableData[113]);
            deleteRow(_0x1E230)
        };
        return
    };
    var _0x1E2B6 = selectElement.value;
    fillPlan(_0x1E2B6)
};

function langChange(_0x1FC5C) {
    loadAbilityForm = false;
    selectedLanguage = _0x1FC5C;
    localStorage.setItem("languageSelect", _0x1FC5C);
    var _0x1FE74 = localStorage.getItem(valiableData[114]);
    var _0x1FC9F = [];
    if (_0x1FE74) {
        _0x1FC9F = JSON.parse(_0x1FE74)
    };
    var _0x1E554 = -1;
    if (_0x1FC9F) {
        for (var _0x1E597 = 0; _0x1E597 < _0x1FC9F.length; _0x1E597++) {
            if (_0x1FC9F[_0x1E597][valiableData[115]] === _0x1FC5C) {
                _0x1E554 = _0x1E597;
                break
            }
        };
        if (_0x1E554 !== -1) {
            var _0x1FE31 = _0x1FC9F[_0x1E554][valiableData[116]];
            langFill(_0x1FE31)
        } else {
            getLangDic(_0x1FC5C)
        }
    }
}

function langFill(_0x1FEB7) {
    var _0x1F333 = document.querySelectorAll(valiableData[117]);
    var _0x1EEC0 = document[valiableData[43]](valiableData[119]).value[valiableData[97]](valiableData[118]);
    var _0x1FF80 = (_0x1EEC0[valiableData[88]](valiableData[120]) ? 1 : 0);
    var _0x1EB16 = 0;
    lang = JSON.parse(_0x1FEB7)[valiableData[116]];
    if (lang.length < langlengthPro) {
        var selectElement = document.getElementById(valiableData[121]);
        getLangDic(selectElement.value)
    };
    for (var _0x1E597 = 0; _0x1E597 < _0x1F333.length; _0x1E597++) {
        if (_0x1F333[_0x1E597][valiableData[62]][valiableData[122]]() !== "") {
            _0x1F333[_0x1E597][valiableData[62]] = (lang[_0x1EB16][valiableData[88]](valiableData[123])) ? lang[_0x1EB16][valiableData[97]](valiableData[123])[_0x1FF80] : lang[_0x1EB16];
            _0x1EB16++
        }
    };
    var _0x1FF3D = lang[valiableData[99]](59, 90);
    var _0x1F052 = document.querySelectorAll(valiableData[124]);
    for (var _0x1E597 = 0; _0x1E597 < _0x1F052.length; _0x1E597++) {
        if (_0x1F052[_0x1E597][valiableData[62]][valiableData[88]](valiableData[125])) {
            var _0x1FFC3 = _0x1F052[_0x1E597][valiableData[62]];
            var _0x1F938 = _0x1FFC3[valiableData[97]](valiableData[126]);
            _0x1F938[0] = _0x1FF3D[_0x1F052[_0x1E597][valiableData[93]]];
            var _0x1FEFA = _0x1F938[valiableData[98]](valiableData[126]);
            _0x1F052[_0x1E597][valiableData[62]] = _0x1FEFA
        } else {
            _0x1F052[_0x1E597][valiableData[62]] = _0x1FF3D[_0x1F052[_0x1E597][valiableData[93]]]
        }
    };
    setWhitecolV2(1);
    var _0x20006 = document.getElementById(valiableData[48]);
    _0x20006[valiableData[41]][0][valiableData[45]][0][valiableData[62]] = lang[101];
    var _0x20049 = document.getElementById(valiableData[32]);
    for (var _0x1EB16 = 1; _0x1EB16 < 20; _0x1EB16++) {
        _0x20006[valiableData[41]][(_0x1EB16 < 16 ? _0x1EB16 : _0x1EB16 + 1)][valiableData[45]][0][valiableData[62]] = _0x20049[valiableData[41]][0][valiableData[45]][_0x1EB16 + 2][valiableData[62]]
    }
}

function getLangDic(_0x1FC5C) {
    var _0x1E984 = new XMLHttpRequest();
    var _0x1E9C7 = valiableData[127];
    var _0x1E511 = valiableData[128] + encodeURIComponent(_0x1FC5C);
    _0x1E984[valiableData[71]](valiableData[70], _0x1E9C7, true);
    _0x1E984[valiableData[74]](valiableData[72], valiableData[73]);
    _0x1E984[valiableData[75]] = function () {
        if (_0x1E984[valiableData[76]] === 4 && _0x1E984[valiableData[77]] === 200) {
            var _0x1FCE2 = _0x1E984[valiableData[78]];
            var _0x1FC9F = JSON.parse(_0x1FCE2);
            saveLocalLangDic(_0x1FCE2, _0x1FC5C);
            langFill(JSON.stringify(_0x1FC9F))
        }
    };
    _0x1E984[valiableData[79]](_0x1E511)
}

function saveLocalLangDic(_0x1FC9F, _0x1FC5C) {
    var _0x1FE74 = localStorage.getItem(valiableData[114]);
    var _0x2021E = [];
    if (_0x1FE74) {
        _0x2021E = JSON.parse(_0x1FE74)
    };
    var _0x1E554 = -1;
    for (var _0x1E597 = 0; _0x1E597 < _0x2021E.length; _0x1E597++) {
        if (_0x2021E[_0x1E597][valiableData[115]] === _0x1FC5C) {
            _0x1E554 = _0x1E597;
            break
        }
    };
    if (_0x1E554 !== -1) {
        _0x2021E[_0x1E554][valiableData[116]] = _0x1FC9F
    } else {
        var _0x20261 = {
            langCode: _0x1FC5C,
            langDic: _0x1FC9F
        };
        _0x2021E[valiableData[129]](_0x20261)
    };
    var _0x1E835 = JSON.stringify(_0x2021E);
    localStorage.setItem(valiableData[114], _0x1E835)
}

function loadSavedPlayerInfo() {
    var _0x1E76C = localStorage.getItem(valiableData[130]);
    var _0x1E729 = JSON.parse(_0x1E76C);
    if (_0x1E729) {
        var selectElement = document.getElementById(valiableData[68]);
        for (var _0x1E597 = 0; _0x1E597 < _0x1E729.length; _0x1E597++) {
            if (!selectElement[valiableData[62]][valiableData[88]](_0x1E729[_0x1E597][valiableData[131]])) {
                var _0x20155 = document[valiableData[133]](valiableData[132]);
                _0x20155.text = _0x1E729[_0x1E597][valiableData[131]];
                var _0x200CF = {
                    value: _0x1E729[_0x1E597].value,
                    plan: _0x1E729[_0x1E597][valiableData[134]]
                };
                _0x20155.value = JSON.stringify(_0x200CF);
                selectElement[valiableData[135]](_0x20155)
            }
        }
    }
}
window.addEventListener(valiableData[136], function (_0x1E167) {
    var _0x1E3C2 = document.getElementById(valiableData[68]);
    if (_0x1E3C2[valiableData[112]] > 0) {
        var _0x1E37F = valiableData[137];
        _0x1E167[valiableData[138]] = _0x1E37F;
        return _0x1E37F;
        _0x1E167[valiableData[37]]()
    }
});
document.addEventListener(valiableData[139], function () {
    if (localStorage.getItem("languageSelect")) {
        var selectElement = document.getElementById(valiableData[121]);
        selectElement.value = selectedLanguage;
        langChange(selectedLanguage)
    };
    loadSavedPlayerInfo();
    loadSamplePlan()
});

function addRow() {
    var _0x1E2F9 = document.getElementById(valiableData[32]);
    var _0x1E273 = _0x1E2F9[valiableData[41]].length;
    if (_0x1E273 >= 33) {
        alert(lang[91])
    } else {
        var _0x1EA4D = _0x1E2F9[valiableData[41]][_0x1E2F9[valiableData[41]].length - 1];
        var _0x1EA90 = _0x1EA4D[valiableData[140]](true);
        _0x1EA4D[valiableData[143]][valiableData[142]](_0x1EA90, _0x1EA4D[valiableData[141]]);
        _0x1EA4D = _0x1E2F9[valiableData[41]][_0x1E2F9[valiableData[41]].length - 1];
        for (var _0x1E597 = 3; _0x1E597 < _0x1EA4D[valiableData[45]].length - 1; _0x1E597++) {
            _0x1EA4D[valiableData[45]][_0x1E597][valiableData[62]] = ""
        }
    }
}

function deleteRow(_0x1F5D1) {
    var _0x1E018 = _0x1F5D1[valiableData[143]][valiableData[143]];
    var _0x1E2F9 = document.getElementById(valiableData[32]);
    var _0x1E273 = _0x1E2F9[valiableData[41]].length;
    if (_0x1E273 === 4) {
        alert(lang[92])
    } else {
        _0x1E018[valiableData[143]][valiableData[144]](_0x1E018)
    };
    var _0x1F11B = 0;
    var _0x1F227 = 0;
    for (var _0x1F0D8 = 3; _0x1F0D8 < _0x1E273 - 1; _0x1F0D8++) {
        _0x1F11B += parseInt(_0x1E2F9[valiableData[41]][_0x1F0D8][valiableData[45]][20][valiableData[62]]);
        _0x1F227 += parseInt(_0x1E2F9[valiableData[41]][_0x1F0D8][valiableData[45]][21][valiableData[62]])
    };
    _0x1E2F9[valiableData[41]][2][valiableData[45]][20][valiableData[62]] = _0x1F11B;
    _0x1E2F9[valiableData[41]][2][valiableData[45]][21][valiableData[62]] = _0x1F227
}

function testHidden() {
    if (isOptionHiddenSupported()) {
        alert(valiableData[145])
    } else {
        alert(valiableData[146])
    }
}

function isOptionHiddenSupported() {
    const _0x1FDEE = document[valiableData[133]](valiableData[47]);
    const _0x1FDAB = document[valiableData[133]](valiableData[132]);
    return valiableData[61] in _0x1FDAB && valiableData[147] in _0x1FDEE
}

function callTrainingResult(selectElement) {
    event[valiableData[37]]();
    var _0x1E0E1 = selectElement[valiableData[143]][valiableData[143]];
    var _0x1EF03 = _0x1E0E1[valiableData[148]];
    var _0x1E2F9 = document.getElementById(valiableData[32]);
    var _0x1EEC0 = _0x1E2F9[valiableData[41]][1][valiableData[45]][2][valiableData[62]];
    var abilityInput = document.getElementById("abilitytxt").value;
    var _0x1E018 = _0x1E2F9[valiableData[41]][(_0x1EF03 < 4 ? 1 : _0x1EF03 - 1)];
    var _0x1EF46 = "";
    var _0x1EE3A = "";
    var _0x1EE7D = isMobileDevice();
    for (var _0x1E597 = 3; _0x1E597 < 18; _0x1E597++) {
        _0x1EF46 += _0x1E018[valiableData[45]][_0x1E597][valiableData[62]] + (_0x1E597 === 17 ? "" : valiableData[149])
    };
    var _0x1E273 = _0x1E2F9[valiableData[41]].length;
    for (r = _0x1EF03; r < _0x1E273; r++) {
        _0x1E018 = _0x1E2F9[valiableData[41]][r];
        _0x1EE3A += _0x1E018[valiableData[43]](valiableData[150])[valiableData[112]] + valiableData[123];
        _0x1EE3A += parseInt(_0x1E018[valiableData[43]](valiableData[42]).value) + (r === _0x1E273 - 1 ? "" : valiableData[149]);
        var _0x1EDF7 = _0x1E018[valiableData[43]](valiableData[150]);
        var _0x1EDB4 = _0x1EDF7.options[_0x1EDF7[valiableData[112]]];
        if (_0x1EDB4) {
            _0x1EDF7.style[valiableData[94]] = _0x1EDB4.style[valiableData[94]]
        }
    };
    var _0x1EF89 = localStorage.getItem(valiableData[151]);
    var _0x1E984 = new XMLHttpRequest();
    _0x1E984[valiableData[71]](valiableData[70], valiableData[152], true);
    _0x1E984[valiableData[74]](valiableData[153], valiableData[73]);
    _0x1E984[valiableData[75]] = function () {
        if (_0x1E984[valiableData[76]] === 4 && _0x1E984[valiableData[77]] === 200) {
            var _0x1F15E = _0x1E984[valiableData[78]];
            var _0x1F1E4 = _0x1F15E[valiableData[97]](valiableData[154]);
            for (var _0x1F26A = _0x1EF03; _0x1F26A < _0x1E273; _0x1F26A++) {
                _0x1E018 = _0x1E2F9[valiableData[41]][_0x1F26A];
                var _0x1F1A1 = _0x1F1E4[_0x1F26A - _0x1EF03][valiableData[97]](valiableData[125]);
                var _0x1F2F0 = _0x1E018[valiableData[43]](valiableData[42]);
                _0x1F2F0.style.backgroundColor = (_0x1F2F0.value !== _0x1F1A1[21] ? valiableData[155] : "");
                _0x1F2F0.value = _0x1F1A1[21];
                for (var _0x1F2AD = 0; _0x1F2AD < 19; _0x1F2AD++) {
                    var _0x1EFCC = _0x1E018[valiableData[45]][3 + _0x1F2AD];
                    _0x1EFCC[valiableData[62]] = _0x1F1A1[_0x1F2AD];
                    if ((_0x1F2AD < 15 && parseInt(_0x1EFCC[valiableData[62]]) >= 340) || (_0x1F2AD === 15 && parseInt(_0x1EFCC[valiableData[62]]) >= 180)) {
                        _0x1EFCC.style.backgroundColor = valiableData[96];
                        _0x1EFCC[valiableData[89]] = ((_0x1F2AD < 15 && parseInt(_0x1EFCC[valiableData[62]]) >= 340) ? lang[104] : lang[105])
                    } else {
                        _0x1EFCC.style.backgroundColor = _0x1E2F9[valiableData[41]][1][valiableData[45]][3 + _0x1F2AD].style.backgroundColor;
                        _0x1EFCC[valiableData[51]](valiableData[89])
                    };
                    if (_0x1F2AD < 15) {
                        if (parseInt(_0x1E018[valiableData[45]][3 + _0x1F2AD][valiableData[62]]) === parseInt(_0x1E2F9[valiableData[41]][(_0x1F26A === 3 ? 1 : _0x1F26A - 1)][valiableData[45]][3 + _0x1F2AD][valiableData[62]])) {
                            _0x1E018[valiableData[45]][3 + _0x1F2AD].style[valiableData[94]] = ((_0x1E018[valiableData[45]][3 + _0x1F2AD].style.backgroundColor === valiableData[66]) ? valiableData[66] : "");
                            _0x1E018[valiableData[45]][3 + _0x1F2AD].style[valiableData[156]] = valiableData[157]
                        } else {
                            _0x1E018[valiableData[45]][3 + _0x1F2AD].style[valiableData[94]] = valiableData[158];
                            _0x1E018[valiableData[45]][3 + _0x1F2AD].style[valiableData[156]] = valiableData[159]
                        }
                    };
                    if (_0x1F26A === _0x1E273 - 1 && _0x1F2AD < 17) {
                        _0x1E2F9[valiableData[41]][2][valiableData[45]][3 + _0x1F2AD][valiableData[62]] = _0x1F1A1[_0x1F2AD]
                    }
                };
                if (_0x1F26A + 1 < _0x1E273) {
                    _0x1E018 = _0x1E2F9[valiableData[41]][_0x1F26A + 1];
                    var _0x1F095 = _0x1F1A1[19][valiableData[97]](valiableData[149]);
                    var _0x1F052 = _0x1E018[valiableData[43]](valiableData[150]);
                    var selectElement = _0x1E018[valiableData[43]](valiableData[47]);
                    for (var _0x1F00F = 1; _0x1F00F < 30; _0x1F00F++) {
                        _0x1F052[_0x1F00F][valiableData[62]] = updateOrAddData(_0x1F052[_0x1F00F][valiableData[62]], parseInt(_0x1F095[_0x1F00F - 1]));
                        _0x1F052[_0x1F00F][valiableData[160]] = (parseInt(_0x1F095[_0x1F00F - 1]) === 0 ? true : false);
                        _0x1F052[_0x1F00F][valiableData[61]] = (parseInt(_0x1F095[_0x1F00F - 1]) === 0 ? true : false)
                    }
                }
            };
            var _0x1F11B = 0;
            var _0x1F227 = 0;
            for (var _0x1F0D8 = 3; _0x1F0D8 < _0x1E273; _0x1F0D8++) {
                _0x1F11B += parseInt(_0x1E2F9[valiableData[41]][_0x1F0D8][valiableData[45]][20][valiableData[62]]);
                _0x1F227 += parseInt(_0x1E2F9[valiableData[41]][_0x1F0D8][valiableData[45]][21][valiableData[62]])
            };
            _0x1E2F9[valiableData[41]][2][valiableData[45]][20][valiableData[62]] = _0x1F11B;
            _0x1E2F9[valiableData[41]][2][valiableData[45]][21][valiableData[62]] = _0x1F227
        }
    };
    _0x1E984[valiableData[79]](valiableData[161] + encodeURIComponent(abilityInput > 0 ? abilityInput : 0) + valiableData[162] + encodeURIComponent(_0x1EF46) + valiableData[163] + encodeURIComponent(_0x1EE3A) + valiableData[164] + encodeURIComponent(_0x1EF89) + valiableData[165] + encodeURIComponent(_0x1EEC0))
}

function updateOrAddData(_0x201DB, _0x208EC) {
    const _0x1F938 = _0x201DB[valiableData[97]](valiableData[126]);
    if (_0x1F938.length >= 4) {
        _0x1F938[3] = _0x208EC
    } else {
        _0x1F938[valiableData[129]](_0x208EC)
    };
    return _0x1F938[valiableData[98]](valiableData[126])
}

function fillSavedData(_0x1FA44) {
    var _0x1FA01 = _0x1FA44[valiableData[112]];
    var _0x1F9BE = [];
    var _0x1FA87 = "";
    if (_0x1FA01 > 0) {
        _0x1F9BE = JSON.parse(_0x1FA44.value);
        _0x1FA87 = _0x1F9BE.value[valiableData[97]](valiableData[149])
    };
    var _0x1E597 = -1;
    var _0x1F508 = document.querySelectorAll(valiableData[166]);
    _0x1F508[valiableData[31]](function (_0x1F58E) {
        _0x1E597 += 1;
        if (_0x1FA87.length > 1) {
            _0x1F58E.value = _0x1FA87[_0x1E597]
        } else {
            _0x1F58E.value = ""
        }
    });
    var _0x1F614 = document.getElementById(valiableData[167]);
    _0x1F614[valiableData[160]] = true;
    _0x1F614.style[valiableData[94]] = valiableData[66];
    if (_0x1FA44[valiableData[112]] === 0) {
        var _0x1E2F9 = document.getElementById(valiableData[32]);
        var _0x1E273 = _0x1E2F9[valiableData[41]].length;
        addRow();
        for (var _0x1E33C = 1; _0x1E33C < _0x1E273 - 2; _0x1E33C++) {
            var _0x1E018 = _0x1E2F9[valiableData[41]][_0x1E273 - _0x1E33C];
            var _0x1E230 = _0x1E018[valiableData[45]][22][valiableData[43]](valiableData[113]);
            deleteRow(_0x1E230)
        };
        return
    };
    displayTextV2();
    showSamplePlan();
    removeSoSanhCellStyle();
    xoaPA1();
    xoaPA2();
    xoaPA3();
    var _0x1FACA = form.querySelectorAll(valiableData[47]);
    _0x1FACA[valiableData[31]](function (_0x1FB0D) {
        _0x1FB0D[valiableData[112]] = 0
    })
}

function showSamplePlan() {
    var _0x1EEC0 = document.getElementById(valiableData[168]);
    var _0x207E0 = _0x1EEC0.value[valiableData[97]](valiableData[118]);
    var selectElement = document.getElementById("samplePlanner");
    selectElement[valiableData[112]] = 0;
    for (var _0x1E597 = 1; _0x1E597 < selectElement.options.length; _0x1E597++) {
        selectElement.options[_0x1E597][valiableData[61]] = true
    };
    for (var _0x2075A = 0; _0x2075A < _0x207E0.length; _0x2075A++) {
        var _0x2079D = _0x207E0[_0x2075A];
        if (_0x2079D[valiableData[88]](valiableData[169])) {
            _0x2079D = _0x2079D[valiableData[171]](/L/g, valiableData[170])
        } else {
            if (_0x2079D[valiableData[88]](valiableData[172])) {
                _0x2079D = _0x2079D[valiableData[171]](/R/g, valiableData[170])
            }
        };
        for (var _0x1E597 = 1; _0x1E597 < selectElement.options.length; _0x1E597++) {
            var _0x20717 = selectElement.options[_0x1E597].text[valiableData[97]](valiableData[123])[0];
            var _0x206D4 = _0x20717[valiableData[97]](valiableData[118]);
            for (var _0x1EB16 = 0; _0x1EB16 < _0x206D4.length; _0x1EB16++) {
                var _0x20691 = _0x206D4[_0x1EB16];
                if (_0x20691[valiableData[88]](valiableData[169])) {
                    _0x20691 = _0x20691[valiableData[171]](/L/g, valiableData[170])
                } else {
                    if (_0x20691[valiableData[88]](valiableData[172])) {
                        _0x20691 = _0x20691[valiableData[171]](/R/g, valiableData[170])
                    }
                };
                if (_0x2079D === _0x20691) {
                    selectElement.options[_0x1E597][valiableData[61]] = false
                }
            }
        }
    }
}

function getPlayerPlan(_0x1FD25) {
    var _0x1F9BE = [];
    _0x1F9BE = JSON.parse(_0x1FD25);
    if (_0x1F9BE) {
        return _0x1F9BE[valiableData[134]]
    } else {
        return ""
    }
}

function containsOption(selectElement, _0x1F3B9) {
    for (var _0x1E597 = 0; _0x1E597 < selectElement.options.length; _0x1E597++) {
        if (selectElement.options[_0x1E597].value === _0x1F3B9) {
            return true
        }
    };
    return false
}

function loadSamplePlan() {
    const _0x20112 = JSON.parse(localStorage.getItem("samplePlanner"));
    var _0x1EEC0 = document.getElementById(valiableData[168]);
    var _0x1F938 = _0x1EEC0.value[valiableData[97]](valiableData[118]);
    if (_0x20112 && Array[valiableData[173]](_0x20112)) {
        const selectElement = document.getElementById("samplePlanner");
        for (var _0x1E597 = 0; _0x1E597 < _0x20112.length; _0x1E597++) {
            const _0x200CF = _0x20112[_0x1E597].value;
            const _0x2008C = _0x20112[_0x1E597][valiableData[131]];
            if (!containsOption(selectElement, _0x200CF)) {
                const _0x1E6E6 = document[valiableData[133]](valiableData[132]);
                _0x1E6E6.value = _0x200CF;
                _0x1E6E6.text = _0x2008C;
                _0x1E6E6[valiableData[61]] = true;
                selectElement[valiableData[135]](_0x1E6E6)
            }
        }
    }
}

function setWhitecolV2(_0x204BC) {
    var _0x1E2F9 = document.getElementById(valiableData[32]);
    var _0x1F052 = document.querySelectorAll(valiableData[87]);
    const _0x1EEC0 = document.getElementById(valiableData[168]).value;
    var _0x204FF = "";
    for (var _0x20479 = 3; _0x20479 < 18; _0x20479++) {
        _0x204FF += _0x1E2F9[valiableData[41]][0][valiableData[45]][_0x20479][valiableData[62]] + (_0x20479 < 17 ? valiableData[149] : "")
    };
    const _0x1E984 = new XMLHttpRequest();
    _0x1E984[valiableData[71]](valiableData[70], valiableData[174], true);
    _0x1E984[valiableData[74]](valiableData[153], valiableData[73]);
    _0x1E984[valiableData[75]] = function () {
        if (_0x1E984[valiableData[76]] === 4 && _0x1E984[valiableData[77]] === 200) {
            var _0x1FCE2 = _0x1E984[valiableData[78]];
            var _0x2060B = _0x1FCE2[valiableData[97]](valiableData[154])[0];
            var _0x1F657 = 0;
            var _0x1F720 = 0;
            var _0x20585 = "";
            var _0x2064E = "";
            for (var _0x1E597 = 0; _0x1E597 < _0x1E2F9[valiableData[41]].length; _0x1E597++) {
                var _0x1E018 = _0x1E2F9[valiableData[41]][_0x1E597];
                for (var _0x1EB16 = 3; _0x1EB16 < 18; _0x1EB16++) {
                    var _0x1EFCC = _0x1E018[valiableData[45]][_0x1EB16];
                    if (!_0x2060B[valiableData[88]](_0x1EB16 < 10 ? valiableData[175] + _0x1EB16 : "" + _0x1EB16)) {
                        _0x1EFCC[valiableData[65]].remove(_0x1E597 < 3 ? valiableData[176] : valiableData[177]);
                        _0x1EFCC[valiableData[65]][valiableData[147]](_0x1E597 < 3 ? valiableData[63] : valiableData[104]);
                        if (_0x1E597 === 1) {
                            _0x1F657 += parseFloat(_0x1E018[valiableData[45]][_0x1EB16][valiableData[62]]);
                            _0x20585 += _0x1EB16 + valiableData[149]
                        }
                    } else {
                        _0x1EFCC[valiableData[65]].remove(_0x1E597 < 3 ? valiableData[63] : valiableData[104]);
                        _0x1EFCC[valiableData[65]][valiableData[147]](_0x1E597 < 3 ? valiableData[176] : valiableData[177]);
                        if (_0x1E597 === 1 && _0x2060B[valiableData[88]](_0x1EB16 < 10 ? valiableData[175] + _0x1EB16 : "" + _0x1EB16)) {
                            _0x1F720 += parseFloat(_0x1E018[valiableData[45]][_0x1EB16][valiableData[62]]);
                            _0x2064E += _0x1EB16 + valiableData[149]
                        }
                    };
                    if (_0x1EB16 === 17) {
                        _0x1E2F9[valiableData[41]][1][valiableData[45]][19][valiableData[62]] = (_0x1F720 * 100.0 / (_0x1F657 + _0x1F720))[valiableData[105]](1);
                        _0x1E2F9[valiableData[41]][2][valiableData[45]][19][valiableData[62]] = _0x1E2F9[valiableData[41]][1][valiableData[45]][19][valiableData[178]]
                    }
                }
            };
            localStorage.setItem(valiableData[151], _0x2064E[valiableData[179]](0, _0x2064E.length - 1) + valiableData[123] + _0x20585[valiableData[179]](0, _0x20585.length - 1));
            for (var _0x1FBD6 = 1; _0x1FBD6 < 31; _0x1FBD6++) {
                var _0x205C8 = _0x1FCE2[valiableData[97]](valiableData[154])[_0x1FBD6];
                if (_0x205C8) {
                    if (_0x1F052[_0x1FBD6][valiableData[62]][valiableData[88]](valiableData[125])) {
                        var _0x20542 = _0x1F052[_0x1FBD6][valiableData[62]][valiableData[97]](valiableData[125]);
                        _0x1F052[_0x1FBD6][valiableData[62]] = _0x20542[0][valiableData[122]]() + valiableData[126] + _0x205C8[valiableData[97]](valiableData[123])[0]
                    } else {
                        _0x1F052[_0x1FBD6][valiableData[62]] += valiableData[126] + _0x205C8[valiableData[97]](valiableData[123])[0]
                    };
                    _0x1F052[_0x1FBD6][valiableData[89]] = _0x205C8[valiableData[97]](valiableData[123])[1];
                    if (parseInt(_0x205C8[valiableData[97]](valiableData[125])[1]) === 0) {
                        _0x1F052[_0x1FBD6][valiableData[61]] = true
                    } else {
                        _0x1F052[_0x1FBD6][valiableData[61]] = false
                    }
                }
            };
            var selectElement = document.getElementById(valiableData[68]);
            if (selectElement[valiableData[112]] !== 0 && _0x204BC === 0) {
                fillPlan(getPlayerPlan(selectElement.value))
            };
            setDrillOptionsTitle()
        }
    };
    _0x1E984[valiableData[79]](valiableData[180] + encodeURIComponent(_0x1EEC0) + valiableData[181] + encodeURIComponent(_0x204FF))
}

function setDrillOptionsTitle() {
    var _0x1E2F9 = document.getElementById(valiableData[32]);
    if (_0x1E2F9) {
        var _0x20436 = _0x1E2F9[valiableData[41]][3];
        for (var _0x1E597 = 4; _0x1E597 < _0x1E2F9[valiableData[41]].length; _0x1E597++) {
            var _0x1E018 = _0x1E2F9[valiableData[41]][_0x1E597];
            if (_0x1E018[valiableData[45]].length > 1) {
                var _0x1EFCC = _0x1E018[valiableData[45]][1];
                if (_0x1EFCC[valiableData[182]].length > 0 && _0x1EFCC[valiableData[182]][0][valiableData[183]][valiableData[81]]() === valiableData[47]) {
                    var _0x1FB0D = _0x1EFCC[valiableData[182]][0];
                    var _0x1F052 = _0x1FB0D[valiableData[50]](valiableData[132]);
                    for (var _0x1EB16 = 0; _0x1EB16 < _0x1F052.length; _0x1EB16++) {
                        var _0x203F3 = _0x20436[valiableData[45]][1][valiableData[50]](valiableData[47])[0][valiableData[50]](valiableData[132])[_0x1EB16];
                        _0x1F052[_0x1EB16][valiableData[89]] = _0x203F3[valiableData[89]]
                    }
                }
            }
        }
    }
}
fullscreenButton.addEventListener(valiableData[184], function () {
    event[valiableData[37]]();
    var _0x1E448 = document[valiableData[43]](valiableData[185]);
    var _0x1E405 = fullscreenButton[valiableData[62]];
    if (_0x1E405[valiableData[81]]() === valiableData[186]) {
        fullscreenButton[valiableData[62]] = valiableData[187];
        _0x1E448[valiableData[65]][valiableData[147]](valiableData[186]);
        if (document[valiableData[189]][valiableData[188]]) {
            document[valiableData[189]][valiableData[188]]()
        } else {
            if (document[valiableData[189]][valiableData[190]]) {
                document[valiableData[189]][valiableData[190]]()
            } else {
                if (document[valiableData[189]][valiableData[191]]) {
                    document[valiableData[189]][valiableData[191]]()
                } else {
                    if (document[valiableData[189]][valiableData[192]]) {
                        document[valiableData[189]][valiableData[192]]()
                    }
                }
            }
        }
    } else {
        fullscreenButton[valiableData[62]] = valiableData[193];
        _0x1E448[valiableData[65]].remove(valiableData[186]);
        if (document[valiableData[194]]) {
            document[valiableData[194]]()
        } else {
            if (document[valiableData[195]]) {
                document[valiableData[195]]()
            } else {
                if (document[valiableData[196]]) {
                    document[valiableData[196]]()
                } else {
                    if (document[valiableData[197]]) {
                        document[valiableData[197]]()
                    }
                }
            }
        }
    }
});

function displayTextV2() {
    event[valiableData[37]]();
    langChange(selectedLanguage);
    var _0x1F54B = [];
    var _0x1F508 = document.querySelectorAll(valiableData[166]);
    _0x1F508[valiableData[31]](function (_0x1F58E) {
        _0x1F54B[valiableData[129]](_0x1F58E.value)
    });
    localStorage.setItem(valiableData[198], _0x1F54B);
    var _0x1E2F9 = document.getElementById(valiableData[32]);
    var _0x1EEC0 = document[valiableData[43]](valiableData[119]).value[valiableData[97]](valiableData[118]);
    var _0x1F052 = document.querySelectorAll(valiableData[87]);
    var _0x1F69A = 0;
    var _0x1F657 = 0;
    var _0x1F720 = 0;
    for (var _0x1E597 = 1; _0x1E597 < 3; _0x1E597++) {
        var _0x1E018 = _0x1E2F9[valiableData[41]][_0x1E597];
        for (var _0x1EB16 = 1; _0x1EB16 < 18; _0x1EB16++) {
            var _0x1EFCC = _0x1E018[valiableData[45]][_0x1EB16];
            if (_0x1E597 === 2 && _0x1EB16 < 3) {
                continue
            };
            _0x1EFCC[valiableData[62]] = _0x1F54B[(_0x1EB16 < 3) ? (_0x1EB16 - 1) : _0x1EB16];
            if (_0x1E597 === 1 && _0x1EB16 > 2) {
                _0x1F69A += parseFloat(_0x1F54B[_0x1EB16])
            };
            if (_0x1E597 === 2) {
                _0x1EFCC.style[valiableData[94]] = valiableData[96];
                _0x1EFCC.style[valiableData[156]] = valiableData[159]
            }
        }
    };
    _0x1E2F9[valiableData[41]][1][valiableData[45]][18][valiableData[62]] = (_0x1F69A / 15.0)[valiableData[105]](1);
    _0x1E2F9[valiableData[41]][2][valiableData[45]][18][valiableData[62]] = (_0x1F69A / 15.0)[valiableData[105]](1);
    setWhitecolV2(0);
    var _0x1F6DD = _0x1E2F9[valiableData[41]][0][valiableData[45]][19][valiableData[62]];
    var _0x1F614 = document.getElementById(valiableData[167]);
    _0x1F614[valiableData[160]] = false;
    _0x1F614.style[valiableData[94]] = valiableData[158]
}

function fillPlan(_0x1F482) {
    table = document.getElementById(valiableData[32]);
    var _0x1E273 = table[valiableData[41]].length;
    for (var _0x1E33C = 1; _0x1E33C < _0x1E273 - 3; _0x1E33C++) {
        var _0x1E018 = table[valiableData[41]][_0x1E273 - _0x1E33C];
        var _0x1E230 = _0x1E018[valiableData[45]][22][valiableData[43]](valiableData[113]);
        deleteRow(_0x1E230)
    };
    _0x1E273 = table[valiableData[41]].length - 3;
    if (_0x1F482.length > 0) {
        var _0x1F333 = _0x1F482[valiableData[97]](valiableData[149]);
        for (var _0x1E597 = 0; _0x1E597 < _0x1F333.length - _0x1E273; _0x1E597++) {
            addRow()
        };
        var _0x1EB16 = 3;
        _0x1F333[valiableData[31]](function (_0x1F86F) {
            var _0x1F938 = _0x1F86F[valiableData[97]](valiableData[123]);
            var _0x1F8B2 = parseInt(_0x1F938[0]);
            var _0x1F3B9 = _0x1F938[1];
            var _0x1E018 = table[valiableData[41]][_0x1EB16];
            var _0x1F97B = _0x1E018[valiableData[45]][1][valiableData[43]](valiableData[47]);
            _0x1F97B[valiableData[112]] = _0x1F8B2;
            var _0x1F8F5 = _0x1E018[valiableData[45]][2][valiableData[43]](valiableData[42]);
            _0x1F8F5.value = _0x1F3B9;
            _0x1EB16++
        });
        _0x1E273 = table[valiableData[41]].length;
        if (_0x1EB16 < _0x1E273) {
            for (var _0x1E33C = 1; _0x1E33C < _0x1E273 - _0x1EB16 + 1; _0x1E33C++) {
                var _0x1E018 = table[valiableData[41]][_0x1E273 - _0x1E33C];
                var _0x1E230 = _0x1E018[valiableData[45]][22][valiableData[43]](valiableData[113]);
                deleteRow(_0x1E230)
            }
        };
        var _0x1E018 = table[valiableData[41]][3];
        var _0x1F82C = _0x1E018[valiableData[45]][2][valiableData[43]](valiableData[42]);
        callTrainingResult(_0x1F82C)
    }
}

function getPlanValue() {
    var _0x1E2F9 = document.getElementById(valiableData[32]);
    var _0x1E61D = _0x1E2F9[valiableData[41]][1][valiableData[45]][2][valiableData[62]] + valiableData[199] + parseInt(_0x1E2F9[valiableData[41]][2][valiableData[45]][18][valiableData[62]] / 10) + valiableData[200];
    var _0x1E511 = [];
    for (var _0x1E597 = 3; _0x1E597 < _0x1E2F9[valiableData[41]].length; _0x1E597++) {
        var _0x1E018 = _0x1E2F9[valiableData[41]][_0x1E597];
        var _0x1E7AF = _0x1E018[valiableData[43]](valiableData[47])[valiableData[112]];
        var _0x1E5DA = _0x1E018[valiableData[43]](valiableData[42]).value;
        if (_0x1E7AF > 0) {
            _0x1E511[valiableData[129]](_0x1E7AF + valiableData[123] + _0x1E5DA)
        }
    };
    var _0x1E8BB = _0x1E511[valiableData[98]](valiableData[149]);
    return _0x1E8BB
}

function savePlayerInfo() {
    event[valiableData[37]]();
    var _0x1E76C = localStorage.getItem(valiableData[130]);
    var _0x1E729 = [];
    if (_0x1E76C) {
        _0x1E729 = JSON.parse(_0x1E76C)
    };
    var _0x1F54B = [];
    var _0x1E61D = document.getElementById(valiableData[201]).value + valiableData[202] + document.getElementById(valiableData[168]).value;
    var _0x1F508 = document.querySelectorAll(valiableData[166]);
    _0x1F508[valiableData[31]](function (_0x1F58E) {
        _0x1F54B[valiableData[129]](_0x1F58E.value)
    });
    var _0x1E8BB = _0x1F54B[valiableData[98]](valiableData[149]);
    var _0x1E554 = -1;
    for (var _0x1E597 = 0; _0x1E597 < _0x1E729.length; _0x1E597++) {
        if (_0x1E729[_0x1E597][valiableData[131]] === _0x1E61D) {
            _0x1E554 = _0x1E597;
            break
        }
    };
    var _0x1F482 = getPlanValue();
    if (_0x1E554 !== -1) {
        _0x1E729[_0x1E554].value = _0x1E8BB;
        _0x1E729[_0x1E554][valiableData[134]] = _0x1F482;
        updateSelectedPlayer(JSON.stringify({
            value: _0x1E8BB,
            plan: _0x1F482
        }))
    } else {
        var _0x1E660 = {
            label: _0x1E61D,
            value: _0x1E8BB,
            plan: _0x1F482
        };
        _0x1E729[valiableData[129]](_0x1E660);
        var selectElement = document.getElementById(valiableData[68]);
        var _0x20155 = document[valiableData[133]](valiableData[132]);
        _0x20155.text = _0x1E61D;
        var _0x200CF = {
            value: _0x1E8BB,
            plan: _0x1F482
        };
        _0x20155.value = JSON.stringify(_0x200CF);
        selectElement[valiableData[135]](_0x20155);
        selectElement[valiableData[112]] = _0x1E729.length
    };
    var _0x1F43F = {
        label: _0x1E61D,
        value: _0x1E8BB,
        plan: _0x1F482
    };
    savePlayerData(JSON.stringify(_0x1F43F));
    var _0x1E835 = JSON.stringify(_0x1E729);
    localStorage.setItem(valiableData[130], _0x1E835);
    alert(lang[89] + valiableData[203] + _0x1E61D + valiableData[204])
}

function updateSelectedPlayer(_0x2092F) {
    const selectElement = document.getElementById(valiableData[68]);
    const _0x20972 = selectElement.options[selectElement[valiableData[112]]];
    _0x20972.value = _0x2092F
}

function savePlayerData(_0x1F3B9) {
    var _0x1E984 = new XMLHttpRequest();
    var _0x1E9C7 = valiableData[67];
    var _0x1E511 = valiableData[205] + _0x1F3B9[valiableData[171]](/\+/g, valiableData[125]);
    _0x1E984[valiableData[71]](valiableData[70], _0x1E9C7, true);
    _0x1E984[valiableData[74]](valiableData[153], valiableData[73]);
    _0x1E984[valiableData[75]] = function () {
        if (_0x1E984[valiableData[76]] === 4 && _0x1E984[valiableData[77]] === 200) {
            var _0x203B0 = _0x1E984[valiableData[78]]
        }
    };
    _0x1E984[valiableData[79]](_0x1E511)
}

function deletePlayerInfo() {
    event[valiableData[37]]();
    var _0x1F54B = [];
    var _0x1E61D = valiableData[206] + document.getElementById(valiableData[201]).value + valiableData[202] + document.getElementById(valiableData[168]).value;
    var _0x1F508 = document.querySelectorAll(valiableData[166]);
    _0x1F508[valiableData[31]](function (_0x1F58E) {
        _0x1F54B[valiableData[129]](_0x1F58E.value)
    });
    var _0x1E8BB = _0x1F54B[valiableData[98]](valiableData[149]);
    var _0x1F482 = getPlanValue();
    var _0x1F43F = {
        label: _0x1E61D,
        value: _0x1E8BB,
        plan: _0x1F482
    };
    savePlayerData(JSON.stringify(_0x1F43F));
    var _0x1E76C = localStorage.getItem(valiableData[130]);
    var _0x1E729 = [];
    if (_0x1E76C) {
        _0x1E729 = JSON.parse(_0x1E76C)
    };
    var selectElement = document.getElementById(valiableData[68]);
    var _0x1F4C5 = selectElement[valiableData[112]];
    var _0x1E61D = document.getElementById(valiableData[201]).value;
    if (_0x1F4C5 === 0) {
        alert(lang[107]);
        return
    };
    var _0x1F3FC = confirm(lang[93] + valiableData[203] + _0x1E61D + valiableData[207]);
    if (!_0x1F3FC) {
        return
    };
    var _0x1F508 = document.querySelectorAll(valiableData[166]);
    _0x1F508[valiableData[31]](function (_0x1F58E) {
        _0x1F58E.value = ""
    });
    _0x1E729[valiableData[208]](_0x1F4C5 - 1, 1);
    var _0x1E835 = JSON.stringify(_0x1E729);
    localStorage.setItem(valiableData[130], _0x1E835);
    selectElement.remove(_0x1F4C5);
    alert(lang[90] + valiableData[203] + _0x1E61D + valiableData[204])
}

function duplicateRow(_0x1F763) {
    var _0x1E018 = _0x1F763[valiableData[143]][valiableData[143]];
    var _0x1E2F9 = document.getElementById(valiableData[32]);
    var _0x1E273 = _0x1E2F9[valiableData[41]].length;
    if (_0x1E273 >= 33) {
        alert(lang[91])
    } else {
        var _0x1F7A6 = _0x1E018[valiableData[140]](true);
        var _0x1F7E9 = _0x1E018[valiableData[43]](valiableData[150]);
        var _0x1F4C5 = _0x1F7E9[valiableData[112]];
        var selectElement = _0x1F7A6[valiableData[43]](valiableData[150]);
        selectElement[valiableData[112]] = _0x1F4C5;
        _0x1E018[valiableData[143]][valiableData[142]](_0x1F7A6, _0x1E018[valiableData[141]])
    }
}

function sendPlanToServer(_0x1F482) {
    var _0x1E984 = new XMLHttpRequest();
    var _0x1E9C7 = valiableData[209];
    var _0x1E511 = valiableData[69] + _0x1F482;
    _0x1E984[valiableData[71]](valiableData[70], _0x1E9C7, true);
    _0x1E984[valiableData[74]](valiableData[72], valiableData[73]);
    _0x1E984[valiableData[75]] = function () {
        if (_0x1E984[valiableData[76]] === 4 && _0x1E984[valiableData[77]] === 200) {
            var _0x203B0 = _0x1E984[valiableData[78]]
        }
    };
    _0x1E984[valiableData[79]](_0x1E511)
}
btSaveSamplePlan.addEventListener(valiableData[184], function () {
    event[valiableData[37]]();
    var _0x1E76C = localStorage.getItem("samplePlanner");
    var _0x1E729 = [];
    if (_0x1E76C) {
        _0x1E729 = JSON.parse(_0x1E76C)
    };
    var _0x1E2F9 = document.getElementById(valiableData[32]);
    var _0x1E61D = _0x1E2F9[valiableData[41]][1][valiableData[45]][2][valiableData[62]] + valiableData[199] + parseInt(_0x1E2F9[valiableData[41]][2][valiableData[45]][18][valiableData[62]] / 10) + valiableData[200];
    var _0x1E511 = [];
    for (var _0x1E597 = 3; _0x1E597 < _0x1E2F9[valiableData[41]].length; _0x1E597++) {
        var _0x1E018 = _0x1E2F9[valiableData[41]][_0x1E597];
        var _0x1E7AF = _0x1E018[valiableData[43]](valiableData[47])[valiableData[112]];
        var _0x1E5DA = _0x1E018[valiableData[43]](valiableData[42]).value;
        _0x1E511[valiableData[129]](_0x1E7AF + valiableData[123] + _0x1E5DA)
    };
    var _0x1E8BB = _0x1E511[valiableData[98]](valiableData[149]);
    var _0x1E554 = -1;
    for (var _0x1E597 = 0; _0x1E597 < _0x1E729.length; _0x1E597++) {
        if (_0x1E729[_0x1E597][valiableData[131]] === _0x1E61D) {
            _0x1E554 = _0x1E597;
            break
        }
    };
    var _0x1E4CE = "";
    var _0x1E6A3 = "";
    var _0x1E48B = false;
    if (_0x1E554 !== -1) {
        _0x1E4CE = lang[96];
        const _0x1E878 = prompt(lang[103][valiableData[171]](valiableData[210], _0x1E61D), _0x1E61D);
        if (_0x1E878 === null) {
            return
        } else {
            if (_0x1E878[valiableData[122]]() !== _0x1E61D) {
                _0x1E61D = _0x1E878[valiableData[122]]();
                _0x1E554 = -1
            }
        };
        _0x1E48B = true
    } else {
        const _0x1E878 = prompt(lang[97] + valiableData[203] + _0x1E61D + valiableData[204], _0x1E61D);
        if (_0x1E878 === null) {
            return
        } else {
            _0x1E61D = _0x1E878[valiableData[122]]();
            _0x1E48B = true
        }
    };
    if (_0x1E48B) {
        if (_0x1E554 !== -1) {
            _0x1E729[_0x1E554].value = _0x1E8BB;
            var _0x1E7F2 = {
                label: _0x1E61D,
                value: _0x1E8BB
            };
            sendPlanToServer(JSON.stringify(_0x1E7F2));
            alert(lang[95] + valiableData[203] + _0x1E61D + valiableData[204])
        } else {
            var _0x1E660 = {
                label: _0x1E61D,
                value: _0x1E8BB
            };
            sendPlanToServer(JSON.stringify(_0x1E660));
            _0x1E729[valiableData[129]](_0x1E660);
            alert(lang[94] + valiableData[203] + _0x1E61D + valiableData[204])
        };
        var _0x1E835 = JSON.stringify(_0x1E729);
        localStorage.setItem("samplePlanner", _0x1E835)
    };
    var selectElement = document.getElementById("samplePlanner");
    const _0x1E6E6 = document[valiableData[133]](valiableData[132]);
    _0x1E6E6.value = _0x1E8BB;
    _0x1E6E6.text = _0x1E61D;
    selectElement[valiableData[135]](_0x1E6E6)
});

function abilityForm() {
    event[valiableData[37]]();
    var _0x1EA0A = document[valiableData[43]](valiableData[211]);
    _0x1EA0A.style[valiableData[53]] = valiableData[54]
}

function closeForm() {
    var _0x1EA0A = document[valiableData[43]](valiableData[211]);
    _0x1EA0A.style[valiableData[53]] = valiableData[55];
    var _0x1F333 = form.querySelectorAll(valiableData[42]);
    for (var _0x1F376 = 0; _0x1F376 < _0x1F333.length - 1; _0x1F376++) {
        _0x1F333[_0x1F376].value = ""
    }
}
form.addEventListener(valiableData[212], function (_0x1E167) {
    _0x1E167[valiableData[37]]();
    var _0x1E941 = getSelectedIndex(document.getElementById(valiableData[213]));
    var _0x1E8FE = document.getElementById(valiableData[214]).value;
    var _0x1E511 = valiableData[215] + _0x1E941 + valiableData[216] + _0x1E8FE;
    var _0x1E984 = new XMLHttpRequest();
    _0x1E984[valiableData[75]] = function () {
        if (_0x1E984[valiableData[76]] === XMLHttpRequest[valiableData[217]]) {
            if (_0x1E984[valiableData[77]] === 200) {
                abilityInput.value = _0x1E984[valiableData[78]]
            } else {
                console[valiableData[219]](valiableData[218])
            }
        }
    };
    _0x1E984[valiableData[71]](valiableData[70], valiableData[220], true);
    _0x1E984[valiableData[74]](valiableData[72], valiableData[73]);
    _0x1E984[valiableData[79]](_0x1E511);
    closeForm()
});
var inputElement = document.getElementById(valiableData[168]);
inputElement.addEventListener(valiableData[42], function () {
    inputElement.value = inputElement.value[valiableData[221]]()
});
document.getElementById(valiableData[167]).addEventListener(valiableData[184], function () {
    event[valiableData[37]]();
    if (loadAbilityForm === false) {
        var _0x1E984 = new XMLHttpRequest();
        _0x1E984[valiableData[75]] = function () {
            if (_0x1E984[valiableData[76]] === XMLHttpRequest[valiableData[217]]) {
                if (_0x1E984[valiableData[77]] === 200) {
                    document.getElementById("myForm")[valiableData[62]] = _0x1E984[valiableData[78]];
                    loadAbilityForm = true;
                    abilityForm()
                } else {
                    console[valiableData[219]](valiableData[222])
                }
            }
        };
        var _0x1E9C7 = valiableData[223];
        _0x1E9C7 += valiableData[224] + selectedLanguage;
        _0x1E984[valiableData[71]](valiableData[80], _0x1E9C7, true);
        _0x1E984[valiableData[79]]()
    } else {
        abilityForm()
    }
});

function getSelectedIndex(selectElement) {
    return selectElement[valiableData[112]]
}