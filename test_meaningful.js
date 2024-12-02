var _$_ed92 = ["myForm", "getElementById", "abilitytxt", "samplePlanner", "languageSelect", "getItem", "btFullscreen", "saveSamplePlan", "selectOptions", "parse", "value", "text", "map", "options", "from", "some", "filter", "top11SamplePlanOptions", "stringify", "setItem", "remove", "length", "#training-planner tbody tr", "querySelectorAll", "mouseover", "backgroundColor", "style", "lightgreen", "addEventListener", "mouseout", "", "forEach", "training-planner", "keydown", "key", "Tab", "Enter", "preventDefault", "target", "parentElement", "indexOf", "rows", "input", "querySelector", "cellIndex", "cells", "focus", "select", "bangSoSanh", "td", "getElementsByTagName", "removeAttribute", "soSanhContainer", "display", "flex", "none", "#bangSoSanh .PA1", "#bangSoSanh .PA2", "#bangSoSanh .PA3", "#bangSoSanh .xoaSoSanh", "rowXoaButton", "hidden", "innerHTML", "graySkillThead", "contains", "classList", "gray", "https://top11experience.com/wp-content/themes/top11trainplanner/pro-getPlayerList.php", "savedPlayers-select", "param1=", "POST", "open", "Content-type", "application/x-www-form-urlencoded", "setRequestHeader", "onreadystatechange", "readyState", "status", "responseText", "send", "GET", "toLowerCase", "userAgent", "android", "iphone", "ipad", "ipod", "#training-planner .select-option option", "includes", "title", " - ", "cyan", "white", "index", "color", "black", "red", "split", "join", "slice", "resultTable", ".close-button", "block", "defTable", "graySkillTbody", "toFixed", "%", "attackTable", "physicalTable", "summary2", "summary3", "onchange", "selectedIndex", "button", "pro-langDic", "langCode", "langDic", ".langChange", "+", "#rolestxt", "GK", "language-select", "trim", ":", ".training-page .select-option option", "|", " | ", "https://top11experience.com/wp-content/themes/top11trainplanner/pro-langDic.php", "langCode=", "push", "top11experiencePlayerData", "label", "option", "createElement", "plan", "appendChild", "beforeunload", "B\u1ea1n c\xF3 ch\u1eafc mu\u1ed1n r\u1eddi kh\u1ecfi trang?", "returnValue", "DOMContentLoaded", "cloneNode", "nextSibling", "insertBefore", "parentNode", "removeChild", "Tr\xECnh duy\u1ec7t h\u1ed7 tr\u1ee3 thu\u1ed9c t\xEDnh hidden c\u1ee7a c\xE1c ph\u1ea7n t\u1eed option trong select.", "Tr\xECnh duy\u1ec7t kh\xF4ng h\u1ed7 tr\u1ee3 thu\u1ed9c t\xEDnh hidden c\u1ee7a c\xE1c ph\u1ea7n t\u1eed option trong select.", "add", "rowIndex", ",", ".select-option", "top11experiencePlayerSkillCol", "https://top11experience.com/wp-content/themes/top11trainplanner/pro-trainingPlanCalcu.php", "Content-Type", "<br>", "lightsalmon", "fontWeight", "normal", "blue", "bold", "disabled", "diemChuan=", "&diemTrungBinhBaiTap=", "&diemMucTieu=", "&soKyNang=", "&roles=", "#playerinfo-form input", "abilityCalbt", "rolestxt", "L", "L/R", "replace", "R", "isArray", "https://top11experience.com/wp-content/themes/top11trainplanner/pro-whitecol.php", "0", "whiteSkillThead", "whiteSkillTbody", "innerText", "substring", "roles=", "&skills=", "children", "tagName", "click", ".training-page", "fullscreen", "Exit Fullscreen", "requestFullscreen", "documentElement", "mozRequestFullScreen", "webkitRequestFullscreen", "msRequestFullscreen", "Fullscreen", "exitFullscreen", "mozCancelFullScreen", "webkitExitFullscreen", "msExitFullscreen", "inputValue", ": ", "x %", "nametxt", " : ", " ", "!", "param=", "Deleted ", "?", "splice", "https://top11experience.com/wp-content/themes/top11trainplanner/pro-savePlan.php", "[planLabelText]", ".overlay", "submit", "talentSelect", "ageSelect", "talentIndex=", "&ageIndex=", "DONE", "\u0110\xE3 x\u1ea3y ra l\u1ed7i khi g\u1eedi y\xEAu c\u1ea7u t\xEDnh to\xE1n \u0111\u1ebfn server.", "error", "https://top11experience.com/wp-content/themes/top11trainplanner/abilityCalcu_new.php", "toUpperCase", "\u0110\xE3 x\u1ea3y ra l\u1ed7i khi t\u1ea3i m\xE3 HTML t\u1eeb file PHP.", "https://top11experience.com/wp-content/themes/top11trainplanner/abilityForm.php", "?lang="];
var lang = [];
const langlengthPro = 108;
var form = document["getElementById"]("myForm");
var abilityInput = document["getElementById"]("abilitytxt");
var selectElement = document["getElementById"]("samplePlanner");
var selectedLanguage = localStorage["getItem"]("languageSelect");
var fullscreenButton = document["getElementById"]("btFullscreen");
var btSaveSamplePlan = document["getElementById"]("saveSamplePlan");
var loadAbilityForm = false;

function saveOptionsToLocalStorage() {
    const selectElement = document["getElementById"]("samplePlanner");
    const storedOptionsArray = JSON["parse"](localStorage["getItem"]("selectOptions")) || [];
    const filteredOptions = Array["from"](selectElement["options"])["map"]((newOptionsArray) => {
        return {
            value: newOptionsArray["value"],
            text: newOptionsArray["text"]
        }
    });
    const mergedOptions = filteredOptions["filter"]((newOptionsArray) => {
        return !storedOptionsArray["some"]((tableRows) => {
            return tableRows["value"] === newOptionsArray["value"]
        })
    });
    const hoverEvent = [...storedOptionsArray, ...mergedOptions];
    localStorage["setItem"]("top11SamplePlanOptions", JSON["stringify"](hoverEvent));
    while (selectElement["options"]["length"] > 0) {
        selectElement["options"]["remove"](0)
    }
}
const rows = document["querySelectorAll"]("#training-planner tbody tr");
rows["forEach"]((trainingTable) => {
    trainingTable["addEventListener"]("mouseover", () => {
        trainingTable["style"]["backgroundColor"] = "lightgreen"
    });
    trainingTable["addEventListener"]("mouseout", () => {
        trainingTable["style"]["backgroundColor"] = ""
    })
});
const trainingTable = document["getElementById"]("training-planner");
trainingTable["addEventListener"]("keydown", (keyboardEvent) => {
    if (keyboardEvent["key"] === "Tab" || keyboardEvent["key"] === "Enter") {
        keyboardEvent["preventDefault"]();
        const rowCells = keyboardEvent["target"];
        const tableColumns = rowCells["parentElement"];
        const currentRowIndex = tableColumns["parentElement"];
        const comparisonTable = Array["from"](table["rows"])["indexOf"](currentRowIndex);
        const isMobileDevice = table["rows"][comparisonTable + 1];
        if (isMobileDevice) {
            const selectedOption = isMobileDevice["cells"][tableColumns["cellIndex"]]["querySelector"]("input");
            if (selectedOption) {
                selectedOption["focus"]();
                selectedOption["select"]()
            }
        }
    }
});

function removeSoSanhCellStyle() {
    const localStorageKey = document["getElementById"]("bangSoSanh");
    const var_15 = localStorageKey["getElementsByTagName"]("td");
    for (let var_16 = 0; var_16 < 60; var_16++) {
        var_15[var_16]["removeAttribute"]("style")
    }
}

function xemSoSanh() {
    var var_17 = document["getElementById"]("soSanhContainer");
    var_17["style"]["display"] = "flex"
}

function closeSoSanhTable() {
    var var_17 = document["getElementById"]("soSanhContainer");
    var_17["style"]["display"] = "none"
}

function addSoSanh() {
    var var_18 = document["getElementById"]("training-planner");
    var trainingTable = var_18["rows"][2];
    var var_19 = document["querySelectorAll"]("#bangSoSanh .PA1");
    var var_20 = document["querySelectorAll"]("#bangSoSanh .PA2");
    var var_21 = document["querySelectorAll"]("#bangSoSanh .PA3");
    var var_17 = document["getElementById"]("bangSoSanh");
    var var_22 = document["querySelectorAll"]("#bangSoSanh .xoaSoSanh");
    var var_23 = document["getElementById"]("rowXoaButton");
    var_23["hidden"] = false;
    var var_24 = -1;
    var var_25 = var_19;
    for (var var_16 = 0; var_16 < var_22["length"]; var_16++) {
        if (var_22[var_16]["hidden"] === true) {
            var_24 = var_16;
            var_22[var_16]["hidden"] = false;
            break
        }
    };
    if (var_24 === 0) {
        var_25 = var_19
    } else {
        if (var_24 === 1) {
            var_25 = var_20
        } else {
            if (var_24 === 2) {
                var_25 = var_21
            } else {
                alert(lang[106])
            }
        }
    };
    for (var var_16 = 0; var_16 < var_25["length"]; var_16++) {
        var var_26 = trainingTable["cells"][var_16 + 3];
        var_25[var_16]["innerHTML"] = var_26["innerHTML"];
        var currentRowIndex = var_17["rows"][var_16 + 1];
        if (var_26["classList"]["contains"]("graySkillThead")) {
            for (var var_27 = 0; var_27 < 4; var_27++) {
                currentRowIndex["cells"][var_27]["style"]["backgroundColor"] = "gray"
            }
        } else {
            for (var var_27 = 0; var_27 < 4; var_27++) {
                if (currentRowIndex["cells"][var_27]["style"]["backgroundColor"] === "gray") {
                    currentRowIndex["cells"][var_27]["removeAttribute"]("style")
                }
            }
        }
    }
}

function xoaPA1() {
    var var_28 = document["querySelectorAll"]("#bangSoSanh .PA1");
    for (var var_16 = 0; var_16 < var_28["length"]; var_16++) {
        var_28[var_16]["innerHTML"] = ""
    };
    var var_22 = document["querySelectorAll"]("#bangSoSanh .xoaSoSanh");
    var_22[0]["hidden"] = true;
    if (var_22[0]["hidden"] === true && var_22[1]["hidden"] === true && var_22[2]["hidden"] === true) {
        var var_23 = document["getElementById"]("rowXoaButton");
        var_23["hidden"] = true
    }
}

function xoaPA2() {
    var var_28 = document["querySelectorAll"]("#bangSoSanh .PA2");
    for (var var_16 = 0; var_16 < var_28["length"]; var_16++) {
        var_28[var_16]["innerHTML"] = ""
    };
    var var_22 = document["querySelectorAll"]("#bangSoSanh .xoaSoSanh");
    var_22[1]["hidden"] = true;
    if (var_22[0]["hidden"] === true && var_22[1]["hidden"] === true && var_22[2]["hidden"] === true) {
        var var_23 = document["getElementById"]("rowXoaButton");
        var_23["hidden"] = true
    }
}

function xoaPA3() {
    var var_28 = document["querySelectorAll"]("#bangSoSanh .PA3");
    for (var var_16 = 0; var_16 < var_28["length"]; var_16++) {
        var_28[var_16]["innerHTML"] = ""
    };
    var var_22 = document["querySelectorAll"]("#bangSoSanh .xoaSoSanh");
    var_22[2]["hidden"] = true;
    if (var_22[0]["hidden"] === true && var_22[1]["hidden"] === true && var_22[2]["hidden"] === true) {
        var var_23 = document["getElementById"]("rowXoaButton");
        var_23["hidden"] = true
    }
}

function sendData() {
    var var_29 = new XMLHttpRequest();
    var var_30 = _$_ed92[67];
    var selectElement = document["getElementById"](_$_ed92[68]);
    var var_31 = selectElement["value"];
    var var_32 = _$_ed92[69] + var_31;
    var_29[_$_ed92[71]](_$_ed92[70], var_30, true);
    var_29[_$_ed92[74]](_$_ed92[72], _$_ed92[73]);
    var_29[_$_ed92[75]] = function () {
        if (var_29[_$_ed92[76]] === 4 && var_29[_$_ed92[77]] === 200) {
            var var_33 = var_29[_$_ed92[78]]
        }
    };
    var_29[_$_ed92[79]](var_32)
}

function loadData() {
    var var_29 = new XMLHttpRequest();
    var_29[_$_ed92[71]](_$_ed92[80], _$_ed92[67], true);
    var_29[_$_ed92[75]] = function () {
        if (var_29[_$_ed92[76]] === 4 && var_29[_$_ed92[77]] === 200) {
            var var_32 = JSON["parse"](var_29[_$_ed92[78]])
        }
    };
    var_29[_$_ed92[79]]()
}

function isMobileDevice() {
    var var_34 = navigator[_$_ed92[82]][_$_ed92[81]]();
    if (var_34["indexOf"](_$_ed92[83]) > -1 || var_34["indexOf"](_$_ed92[84]) > -1 || var_34["indexOf"](_$_ed92[85]) > -1 || var_34["indexOf"](_$_ed92[86]) > -1) {
        return true
    } else {
        return false
    }
}

function findSkill(var_35) {
    var filteredOptions = document["querySelectorAll"](_$_ed92[87]);
    var var_36 = document["getElementById"]("training-planner");
    var var_37 = var_35["innerHTML"];
    for (var var_38 = 0; var_38 < filteredOptions["length"]; var_38++) {
        if (filteredOptions[var_38][_$_ed92[89]][_$_ed92[88]](var_37)) {
            filteredOptions[var_38]["innerHTML"] = removeTextBeforeDash(filteredOptions[var_38]["innerHTML"]);
            filteredOptions[var_38]["innerHTML"] = var_37 + _$_ed92[90] + filteredOptions[var_38]["innerHTML"];
            filteredOptions[var_38]["style"]["backgroundColor"] = _$_ed92[91]
        } else {
            filteredOptions[var_38]["innerHTML"] = removeTextBeforeDash(filteredOptions[var_38]["innerHTML"]);
            filteredOptions[var_38]["style"]["backgroundColor"] = _$_ed92[92]
        };
        if (var_38 < 18 && var_38 != var_35[_$_ed92[93]]) {
            var_36["rows"][0]["cells"][var_38]["style"][_$_ed92[94]] = _$_ed92[95]
        };
        var_35["style"][_$_ed92[94]] = _$_ed92[96]
    }
}

function removeTextBeforeDash(var_39) {
    const var_40 = var_39[_$_ed92[97]](_$_ed92[90]);
    if (var_40["length"] > 1) {
        return var_40[_$_ed92[99]](1)[_$_ed92[98]](_$_ed92[90])
    };
    return var_39
}

function showTable() {
    var localStorageKey = document["getElementById"](_$_ed92[100]);
    localStorageKey["style"]["display"] = "flex";
    document["querySelector"](_$_ed92[101])["style"]["display"] = _$_ed92[102];
    var var_41 = document["getElementById"]("training-planner");
    var var_42 = document["getElementById"](_$_ed92[103]);
    var_42["rows"][0]["cells"][0]["innerHTML"] = lang[10];
    var_42["rows"][1]["cells"][0]["innerHTML"] = lang[101];
    var_42["rows"][1]["cells"][1]["innerHTML"] = lang[99];
    var_42["rows"][1]["cells"][2]["innerHTML"] = lang[100];
    var var_43 = 0;
    var var_44 = 0;
    for (var var_16 = 2; var_16 < var_42["rows"]["length"]; var_16++) {
        if (var_16 < 7) {
            for (var var_27 = 0; var_27 < 3; var_27++) {
                var_42["rows"][var_16]["cells"][var_27]["innerHTML"] = var_41["rows"][var_27]["cells"][var_16 + 1]["innerHTML"];
                var_42["rows"][var_16]["cells"][var_27]["style"]["backgroundColor"] = var_41["rows"][3]["cells"][var_16 + 1]["classList"]["contains"](_$_ed92[104]) ? "gray" : ""
            };
            var_43 += parseInt(var_41["rows"][1]["cells"][var_16 + 1]["innerHTML"]);
            var_44 += parseInt(var_41["rows"][2]["cells"][var_16 + 1]["innerHTML"])
        } else {
            var_42["rows"][var_16]["cells"][0]["innerHTML"] = lang[98];
            var_42["rows"][var_16]["cells"][1]["innerHTML"] = parseFloat(var_43 / 5)[_$_ed92[105]](1) + _$_ed92[106];
            var_42["rows"][var_16]["cells"][2]["innerHTML"] = parseFloat(var_44 / 5)[_$_ed92[105]](1) + _$_ed92[106]
        }
    };
    var_42 = document["getElementById"](_$_ed92[107]);
    var_42["rows"][0]["cells"][0]["innerHTML"] = lang[16];
    var_42["rows"][1]["cells"][0]["innerHTML"] = lang[101];
    var_42["rows"][1]["cells"][1]["innerHTML"] = lang[99];
    var_42["rows"][1]["cells"][2]["innerHTML"] = lang[100];
    var_43 = 0;
    var_44 = 0;
    for (var var_16 = 2; var_16 < var_42["rows"]["length"]; var_16++) {
        if (var_16 < 7) {
            for (var var_27 = 0; var_27 < 3; var_27++) {
                var_42["rows"][var_16]["cells"][var_27]["innerHTML"] = var_41["rows"][var_27]["cells"][var_16 + 6]["innerHTML"];
                var_42["rows"][var_16]["cells"][var_27]["style"]["backgroundColor"] = var_41["rows"][3]["cells"][var_16 + 6]["classList"]["contains"](_$_ed92[104]) ? "gray" : ""
            };
            var_43 += parseInt(var_41["rows"][1]["cells"][var_16 + 6]["innerHTML"]);
            var_44 += parseInt(var_41["rows"][2]["cells"][var_16 + 6]["innerHTML"])
        } else {
            var_42["rows"][var_16]["cells"][0]["innerHTML"] = lang[98];
            var_42["rows"][var_16]["cells"][1]["innerHTML"] = parseFloat(var_43 / 5)[_$_ed92[105]](1) + _$_ed92[106];
            var_42["rows"][var_16]["cells"][2]["innerHTML"] = parseFloat(var_44 / 5)[_$_ed92[105]](1) + _$_ed92[106]
        }
    };
    var_42 = document["getElementById"](_$_ed92[108]);
    var_42["rows"][0]["cells"][0]["innerHTML"] = lang[22];
    var_42["rows"][1]["cells"][0]["innerHTML"] = lang[101];
    var_42["rows"][1]["cells"][1]["innerHTML"] = lang[99];
    var_42["rows"][1]["cells"][2]["innerHTML"] = lang[100];
    var_43 = 0;
    var_44 = 0;
    for (var var_16 = 2; var_16 < var_42["rows"]["length"]; var_16++) {
        if (var_16 < 7) {
            for (var var_27 = 0; var_27 < 3; var_27++) {
                var_42["rows"][var_16]["cells"][var_27]["innerHTML"] = var_41["rows"][var_27]["cells"][var_16 + 11]["innerHTML"];
                var_42["rows"][var_16]["cells"][var_27]["style"]["backgroundColor"] = var_41["rows"][3]["cells"][var_16 + 11]["classList"]["contains"](_$_ed92[104]) ? "gray" : ""
            };
            var_43 += parseInt(var_41["rows"][1]["cells"][var_16 + 11]["innerHTML"]);
            var_44 += parseInt(var_41["rows"][2]["cells"][var_16 + 11]["innerHTML"])
        } else {
            var_42["rows"][var_16]["cells"][0]["innerHTML"] = lang[98];
            var_42["rows"][var_16]["cells"][1]["innerHTML"] = parseFloat(var_43 / 5)[_$_ed92[105]](1) + _$_ed92[106];
            var_42["rows"][var_16]["cells"][2]["innerHTML"] = parseFloat(var_44 / 5)[_$_ed92[105]](1) + _$_ed92[106]
        }
    };
    var_42 = document["getElementById"](_$_ed92[109]);
    var_42["rows"][0]["cells"][0]["innerHTML"] = var_41["rows"][0]["cells"][18]["innerHTML"];
    var_42["rows"][0]["cells"][1]["innerHTML"] = var_41["rows"][0]["cells"][19]["innerHTML"];
    var_42["rows"][1]["cells"][0]["innerHTML"] = lang[99];
    var_42["rows"][1]["cells"][2]["innerHTML"] = lang[99];
    var_42["rows"][1]["cells"][1]["innerHTML"] = lang[100];
    var_42["rows"][1]["cells"][3]["innerHTML"] = lang[100];
    var_42["rows"][2]["cells"][0]["innerHTML"] = var_41["rows"][1]["cells"][18]["innerHTML"];
    var_42["rows"][2]["cells"][1]["innerHTML"] = var_41["rows"][2]["cells"][18]["innerHTML"];
    var_42["rows"][2]["cells"][2]["innerHTML"] = var_41["rows"][1]["cells"][19]["innerHTML"];
    var_42["rows"][2]["cells"][3]["innerHTML"] = var_41["rows"][2]["cells"][19]["innerHTML"];
    var_42 = document["getElementById"](_$_ed92[110]);
    var_42["rows"][0]["cells"][0]["innerHTML"] = var_41["rows"][0]["cells"][20]["innerHTML"];
    var_42["rows"][0]["cells"][1]["innerHTML"] = var_41["rows"][0]["cells"][21]["innerHTML"];
    var_42["rows"][1]["cells"][0]["innerHTML"] = var_41["rows"][2]["cells"][20]["innerHTML"];
    var_42["rows"][1]["cells"][1]["innerHTML"] = var_41["rows"][2]["cells"][21]["innerHTML"]
}

function closeTable() {
    var localStorageKey = document["getElementById"](_$_ed92[100]);
    localStorageKey["style"]["display"] = "none";
    document["querySelector"](_$_ed92[101])["style"]["display"] = "none"
}
selectElement[_$_ed92[111]] = function () {
    var localStorageKey = document["getElementById"]("training-planner");
    var var_45 = localStorageKey["rows"]["length"];
    addRow();
    if (selectElement[_$_ed92[112]] === 0) {
        for (var var_46 = 1; var_46 < var_45 - 2; var_46++) {
            var trainingTable = localStorageKey["rows"][var_45 - var_46];
            var var_47 = trainingTable["cells"][22]["querySelector"](_$_ed92[113]);
            deleteRow(var_47)
        };
        return
    };
    var var_48 = selectElement["value"];
    fillPlan(var_48)
};

function langChange(var_49) {
    loadAbilityForm = false;
    selectedLanguage = var_49;
    localStorage["setItem"]("languageSelect", var_49);
    var var_50 = localStorage["getItem"](_$_ed92[114]);
    var var_51 = [];
    if (var_50) {
        var_51 = JSON["parse"](var_50)
    };
    var var_52 = -1;
    if (var_51) {
        for (var var_16 = 0; var_16 < var_51["length"]; var_16++) {
            if (var_51[var_16][_$_ed92[115]] === var_49) {
                var_52 = var_16;
                break
            }
        };
        if (var_52 !== -1) {
            var var_53 = var_51[var_52][_$_ed92[116]];
            langFill(var_53)
        } else {
            getLangDic(var_49)
        }
    }
}

function langFill(var_54) {
    var var_55 = document["querySelectorAll"](_$_ed92[117]);
    var var_56 = document["querySelector"](_$_ed92[119])["value"][_$_ed92[97]](_$_ed92[118]);
    var var_57 = (var_56[_$_ed92[88]](_$_ed92[120]) ? 1 : 0);
    var var_27 = 0;
    lang = JSON["parse"](var_54)[_$_ed92[116]];
    if (lang["length"] < langlengthPro) {
        var selectElement = document["getElementById"](_$_ed92[121]);
        getLangDic(selectElement["value"])
    };
    for (var var_16 = 0; var_16 < var_55["length"]; var_16++) {
        if (var_55[var_16]["innerHTML"][_$_ed92[122]]() !== "") {
            var_55[var_16]["innerHTML"] = (lang[var_27][_$_ed92[88]](_$_ed92[123])) ? lang[var_27][_$_ed92[97]](_$_ed92[123])[var_57] : lang[var_27];
            var_27++
        }
    };
    var var_58 = lang[_$_ed92[99]](59, 90);
    var filteredOptions = document["querySelectorAll"](_$_ed92[124]);
    for (var var_16 = 0; var_16 < filteredOptions["length"]; var_16++) {
        if (filteredOptions[var_16]["innerHTML"][_$_ed92[88]](_$_ed92[125])) {
            var var_59 = filteredOptions[var_16]["innerHTML"];
            var var_40 = var_59[_$_ed92[97]](_$_ed92[126]);
            var_40[0] = var_58[filteredOptions[var_16][_$_ed92[93]]];
            var var_60 = var_40[_$_ed92[98]](_$_ed92[126]);
            filteredOptions[var_16]["innerHTML"] = var_60
        } else {
            filteredOptions[var_16]["innerHTML"] = var_58[filteredOptions[var_16][_$_ed92[93]]]
        }
    };
    setWhitecolV2(1);
    var var_61 = document["getElementById"]("bangSoSanh");
    var_61["rows"][0]["cells"][0]["innerHTML"] = lang[101];
    var var_41 = document["getElementById"]("training-planner");
    for (var var_27 = 1; var_27 < 20; var_27++) {
        var_61["rows"][(var_27 < 16 ? var_27 : var_27 + 1)]["cells"][0]["innerHTML"] = var_41["rows"][0]["cells"][var_27 + 2]["innerHTML"]
    }
}

function getLangDic(var_49) {
    var var_29 = new XMLHttpRequest();
    var var_30 = _$_ed92[127];
    var var_32 = _$_ed92[128] + encodeURIComponent(var_49);
    var_29[_$_ed92[71]](_$_ed92[70], var_30, true);
    var_29[_$_ed92[74]](_$_ed92[72], _$_ed92[73]);
    var_29[_$_ed92[75]] = function () {
        if (var_29[_$_ed92[76]] === 4 && var_29[_$_ed92[77]] === 200) {
            var var_62 = var_29[_$_ed92[78]];
            var var_51 = JSON["parse"](var_62);
            saveLocalLangDic(var_62, var_49);
            langFill(JSON["stringify"](var_51))
        }
    };
    var_29[_$_ed92[79]](var_32)
}

function saveLocalLangDic(var_51, var_49) {
    var var_50 = localStorage["getItem"](_$_ed92[114]);
    var var_63 = [];
    if (var_50) {
        var_63 = JSON["parse"](var_50)
    };
    var var_52 = -1;
    for (var var_16 = 0; var_16 < var_63["length"]; var_16++) {
        if (var_63[var_16][_$_ed92[115]] === var_49) {
            var_52 = var_16;
            break
        }
    };
    if (var_52 !== -1) {
        var_63[var_52][_$_ed92[116]] = var_51
    } else {
        var var_64 = {
            langCode: var_49,
            langDic: var_51
        };
        var_63[_$_ed92[129]](var_64)
    };
    var var_65 = JSON["stringify"](var_63);
    localStorage["setItem"](_$_ed92[114], var_65)
}

function loadSavedPlayerInfo() {
    var var_66 = localStorage["getItem"](_$_ed92[130]);
    var var_67 = JSON["parse"](var_66);
    if (var_67) {
        var selectElement = document["getElementById"](_$_ed92[68]);
        for (var var_16 = 0; var_16 < var_67["length"]; var_16++) {
            if (!selectElement["innerHTML"][_$_ed92[88]](var_67[var_16][_$_ed92[131]])) {
                var newOptionsArray = document[_$_ed92[133]](_$_ed92[132]);
                newOptionsArray["text"] = var_67[var_16][_$_ed92[131]];
                var var_68 = {
                    value: var_67[var_16]["value"],
                    plan: var_67[var_16][_$_ed92[134]]
                };
                newOptionsArray["value"] = JSON["stringify"](var_68);
                selectElement[_$_ed92[135]](newOptionsArray)
            }
        }
    }
}
window["addEventListener"](_$_ed92[136], function (keyboardEvent) {
    var var_69 = document["getElementById"](_$_ed92[68]);
    if (var_69[_$_ed92[112]] > 0) {
        var var_70 = _$_ed92[137];
        keyboardEvent[_$_ed92[138]] = var_70;
        return var_70;
        keyboardEvent["preventDefault"]()
    }
});
document["addEventListener"](_$_ed92[139], function () {
    if (localStorage["getItem"]("languageSelect")) {
        var selectElement = document["getElementById"](_$_ed92[121]);
        selectElement["value"] = selectedLanguage;
        langChange(selectedLanguage)
    };
    loadSavedPlayerInfo();
    loadSamplePlan()
});

function addRow() {
    var localStorageKey = document["getElementById"]("training-planner");
    var var_45 = localStorageKey["rows"]["length"];
    if (var_45 >= 33) {
        alert(lang[91])
    } else {
        var var_71 = localStorageKey["rows"][localStorageKey["rows"]["length"] - 1];
        var var_72 = var_71[_$_ed92[140]](true);
        var_71[_$_ed92[143]][_$_ed92[142]](var_72, var_71[_$_ed92[141]]);
        var_71 = localStorageKey["rows"][localStorageKey["rows"]["length"] - 1];
        for (var var_16 = 3; var_16 < var_71["cells"]["length"] - 1; var_16++) {
            var_71["cells"][var_16]["innerHTML"] = ""
        }
    }
}

function deleteRow(var_73) {
    var trainingTable = var_73[_$_ed92[143]][_$_ed92[143]];
    var localStorageKey = document["getElementById"]("training-planner");
    var var_45 = localStorageKey["rows"]["length"];
    if (var_45 === 4) {
        alert(lang[92])
    } else {
        trainingTable[_$_ed92[143]][_$_ed92[144]](trainingTable)
    };
    var var_74 = 0;
    var var_75 = 0;
    for (var var_76 = 3; var_76 < var_45 - 1; var_76++) {
        var_74 += parseInt(localStorageKey["rows"][var_76]["cells"][20]["innerHTML"]);
        var_75 += parseInt(localStorageKey["rows"][var_76]["cells"][21]["innerHTML"])
    };
    localStorageKey["rows"][2]["cells"][20]["innerHTML"] = var_74;
    localStorageKey["rows"][2]["cells"][21]["innerHTML"] = var_75
}

function testHidden() {
    if (isOptionHiddenSupported()) {
        alert(_$_ed92[145])
    } else {
        alert(_$_ed92[146])
    }
}

function isOptionHiddenSupported() {
    const var_77 = document[_$_ed92[133]]("select");
    const var_78 = document[_$_ed92[133]](_$_ed92[132]);
    return "hidden" in var_78 && _$_ed92[147] in var_77
}

function callTrainingResult(selectElement) {
    event["preventDefault"]();
    var currentRowIndex = selectElement[_$_ed92[143]][_$_ed92[143]];
    var var_79 = currentRowIndex[_$_ed92[148]];
    var localStorageKey = document["getElementById"]("training-planner");
    var var_56 = localStorageKey["rows"][1]["cells"][2]["innerHTML"];
    var abilityInput = document["getElementById"]("abilitytxt")["value"];
    var trainingTable = localStorageKey["rows"][(var_79 < 4 ? 1 : var_79 - 1)];
    var var_80 = "";
    var var_81 = "";
    var var_82 = isMobileDevice();
    for (var var_16 = 3; var_16 < 18; var_16++) {
        var_80 += trainingTable["cells"][var_16]["innerHTML"] + (var_16 === 17 ? "" : _$_ed92[149])
    };
    var var_45 = localStorageKey["rows"]["length"];
    for (r = var_79; r < var_45; r++) {
        trainingTable = localStorageKey["rows"][r];
        var_81 += trainingTable["querySelector"](_$_ed92[150])[_$_ed92[112]] + _$_ed92[123];
        var_81 += parseInt(trainingTable["querySelector"]("input")["value"]) + (r === var_45 - 1 ? "" : _$_ed92[149]);
        var var_83 = trainingTable["querySelector"](_$_ed92[150]);
        var var_84 = var_83["options"][var_83[_$_ed92[112]]];
        if (var_84) {
            var_83["style"][_$_ed92[94]] = var_84["style"][_$_ed92[94]]
        }
    };
    var var_85 = localStorage["getItem"](_$_ed92[151]);
    var var_29 = new XMLHttpRequest();
    var_29[_$_ed92[71]](_$_ed92[70], _$_ed92[152], true);
    var_29[_$_ed92[74]](_$_ed92[153], _$_ed92[73]);
    var_29[_$_ed92[75]] = function () {
        if (var_29[_$_ed92[76]] === 4 && var_29[_$_ed92[77]] === 200) {
            var var_86 = var_29[_$_ed92[78]];
            var var_87 = var_86[_$_ed92[97]](_$_ed92[154]);
            for (var var_88 = var_79; var_88 < var_45; var_88++) {
                trainingTable = localStorageKey["rows"][var_88];
                var var_89 = var_87[var_88 - var_79][_$_ed92[97]](_$_ed92[125]);
                var var_90 = trainingTable["querySelector"]("input");
                var_90["style"]["backgroundColor"] = (var_90["value"] !== var_89[21] ? _$_ed92[155] : "");
                var_90["value"] = var_89[21];
                for (var var_91 = 0; var_91 < 19; var_91++) {
                    var var_92 = trainingTable["cells"][3 + var_91];
                    var_92["innerHTML"] = var_89[var_91];
                    if ((var_91 < 15 && parseInt(var_92["innerHTML"]) >= 340) || (var_91 === 15 && parseInt(var_92["innerHTML"]) >= 180)) {
                        var_92["style"]["backgroundColor"] = _$_ed92[96];
                        var_92[_$_ed92[89]] = ((var_91 < 15 && parseInt(var_92["innerHTML"]) >= 340) ? lang[104] : lang[105])
                    } else {
                        var_92["style"]["backgroundColor"] = localStorageKey["rows"][1]["cells"][3 + var_91]["style"]["backgroundColor"];
                        var_92["removeAttribute"](_$_ed92[89])
                    };
                    if (var_91 < 15) {
                        if (parseInt(trainingTable["cells"][3 + var_91]["innerHTML"]) === parseInt(localStorageKey["rows"][(var_88 === 3 ? 1 : var_88 - 1)]["cells"][3 + var_91]["innerHTML"])) {
                            trainingTable["cells"][3 + var_91]["style"][_$_ed92[94]] = ((trainingTable["cells"][3 + var_91]["style"]["backgroundColor"] === "gray") ? "gray" : "");
                            trainingTable["cells"][3 + var_91]["style"][_$_ed92[156]] = _$_ed92[157]
                        } else {
                            trainingTable["cells"][3 + var_91]["style"][_$_ed92[94]] = _$_ed92[158];
                            trainingTable["cells"][3 + var_91]["style"][_$_ed92[156]] = _$_ed92[159]
                        }
                    };
                    if (var_88 === var_45 - 1 && var_91 < 17) {
                        localStorageKey["rows"][2]["cells"][3 + var_91]["innerHTML"] = var_89[var_91]
                    }
                };
                if (var_88 + 1 < var_45) {
                    trainingTable = localStorageKey["rows"][var_88 + 1];
                    var var_93 = var_89[19][_$_ed92[97]](_$_ed92[149]);
                    var filteredOptions = trainingTable["querySelector"](_$_ed92[150]);
                    var selectElement = trainingTable["querySelector"]("select");
                    for (var var_94 = 1; var_94 < 30; var_94++) {
                        filteredOptions[var_94]["innerHTML"] = updateOrAddData(filteredOptions[var_94]["innerHTML"], parseInt(var_93[var_94 - 1]));
                        filteredOptions[var_94][_$_ed92[160]] = (parseInt(var_93[var_94 - 1]) === 0 ? true : false);
                        filteredOptions[var_94]["hidden"] = (parseInt(var_93[var_94 - 1]) === 0 ? true : false)
                    }
                }
            };
            var var_74 = 0;
            var var_75 = 0;
            for (var var_76 = 3; var_76 < var_45; var_76++) {
                var_74 += parseInt(localStorageKey["rows"][var_76]["cells"][20]["innerHTML"]);
                var_75 += parseInt(localStorageKey["rows"][var_76]["cells"][21]["innerHTML"])
            };
            localStorageKey["rows"][2]["cells"][20]["innerHTML"] = var_74;
            localStorageKey["rows"][2]["cells"][21]["innerHTML"] = var_75
        }
    };
    var_29[_$_ed92[79]](_$_ed92[161] + encodeURIComponent(abilityInput > 0 ? abilityInput : 0) + _$_ed92[162] + encodeURIComponent(var_80) + _$_ed92[163] + encodeURIComponent(var_81) + _$_ed92[164] + encodeURIComponent(var_85) + _$_ed92[165] + encodeURIComponent(var_56))
}

function updateOrAddData(var_39, var_95) {
    const var_40 = var_39[_$_ed92[97]](_$_ed92[126]);
    if (var_40["length"] >= 4) {
        var_40[3] = var_95
    } else {
        var_40[_$_ed92[129]](var_95)
    };
    return var_40[_$_ed92[98]](_$_ed92[126])
}

function fillSavedData(var_96) {
    var var_97 = var_96[_$_ed92[112]];
    var var_98 = [];
    var var_99 = "";
    if (var_97 > 0) {
        var_98 = JSON["parse"](var_96["value"]);
        var_99 = var_98["value"][_$_ed92[97]](_$_ed92[149])
    };
    var var_16 = -1;
    var var_100 = document["querySelectorAll"](_$_ed92[166]);
    var_100["forEach"](function (var_101) {
        var_16 += 1;
        if (var_99["length"] > 1) {
            var_101["value"] = var_99[var_16]
        } else {
            var_101["value"] = ""
        }
    });
    var var_102 = document["getElementById"](_$_ed92[167]);
    var_102[_$_ed92[160]] = true;
    var_102["style"][_$_ed92[94]] = "gray";
    if (var_96[_$_ed92[112]] === 0) {
        var localStorageKey = document["getElementById"]("training-planner");
        var var_45 = localStorageKey["rows"]["length"];
        addRow();
        for (var var_46 = 1; var_46 < var_45 - 2; var_46++) {
            var trainingTable = localStorageKey["rows"][var_45 - var_46];
            var var_47 = trainingTable["cells"][22]["querySelector"](_$_ed92[113]);
            deleteRow(var_47)
        };
        return
    };
    displayTextV2();
    showSamplePlan();
    removeSoSanhCellStyle();
    xoaPA1();
    xoaPA2();
    xoaPA3();
    var var_103 = form["querySelectorAll"]("select");
    var_103["forEach"](function (var_104) {
        var_104[_$_ed92[112]] = 0
    })
}

function showSamplePlan() {
    var var_56 = document["getElementById"](_$_ed92[168]);
    var var_105 = var_56["value"][_$_ed92[97]](_$_ed92[118]);
    var selectElement = document["getElementById"]("samplePlanner");
    selectElement[_$_ed92[112]] = 0;
    for (var var_16 = 1; var_16 < selectElement["options"]["length"]; var_16++) {
        selectElement["options"][var_16]["hidden"] = true
    };
    for (var var_106 = 0; var_106 < var_105["length"]; var_106++) {
        var var_107 = var_105[var_106];
        if (var_107[_$_ed92[88]](_$_ed92[169])) {
            var_107 = var_107[_$_ed92[171]](/L/g, _$_ed92[170])
        } else {
            if (var_107[_$_ed92[88]](_$_ed92[172])) {
                var_107 = var_107[_$_ed92[171]](/R/g, _$_ed92[170])
            }
        };
        for (var var_16 = 1; var_16 < selectElement["options"]["length"]; var_16++) {
            var var_108 = selectElement["options"][var_16]["text"][_$_ed92[97]](_$_ed92[123])[0];
            var var_109 = var_108[_$_ed92[97]](_$_ed92[118]);
            for (var var_27 = 0; var_27 < var_109["length"]; var_27++) {
                var var_110 = var_109[var_27];
                if (var_110[_$_ed92[88]](_$_ed92[169])) {
                    var_110 = var_110[_$_ed92[171]](/L/g, _$_ed92[170])
                } else {
                    if (var_110[_$_ed92[88]](_$_ed92[172])) {
                        var_110 = var_110[_$_ed92[171]](/R/g, _$_ed92[170])
                    }
                };
                if (var_107 === var_110) {
                    selectElement["options"][var_16]["hidden"] = false
                }
            }
        }
    }
}

function getPlayerPlan(var_111) {
    var var_98 = [];
    var_98 = JSON["parse"](var_111);
    if (var_98) {
        return var_98[_$_ed92[134]]
    } else {
        return ""
    }
}

function containsOption(selectElement, var_31) {
    for (var var_16 = 0; var_16 < selectElement["options"]["length"]; var_16++) {
        if (selectElement["options"][var_16]["value"] === var_31) {
            return true
        }
    };
    return false
}

function loadSamplePlan() {
    const var_112 = JSON["parse"](localStorage["getItem"]("samplePlanner"));
    var var_56 = document["getElementById"](_$_ed92[168]);
    var var_40 = var_56["value"][_$_ed92[97]](_$_ed92[118]);
    if (var_112 && Array[_$_ed92[173]](var_112)) {
        const selectElement = document["getElementById"]("samplePlanner");
        for (var var_16 = 0; var_16 < var_112["length"]; var_16++) {
            const var_68 = var_112[var_16]["value"];
            const var_113 = var_112[var_16][_$_ed92[131]];
            if (!containsOption(selectElement, var_68)) {
                const var_114 = document[_$_ed92[133]](_$_ed92[132]);
                var_114["value"] = var_68;
                var_114["text"] = var_113;
                var_114["hidden"] = true;
                selectElement[_$_ed92[135]](var_114)
            }
        }
    }
}

function setWhitecolV2(var_115) {
    var localStorageKey = document["getElementById"]("training-planner");
    var filteredOptions = document["querySelectorAll"](_$_ed92[87]);
    const var_56 = document["getElementById"](_$_ed92[168])["value"];
    var var_116 = "";
    for (var var_117 = 3; var_117 < 18; var_117++) {
        var_116 += localStorageKey["rows"][0]["cells"][var_117]["innerHTML"] + (var_117 < 17 ? _$_ed92[149] : "")
    };
    const var_29 = new XMLHttpRequest();
    var_29[_$_ed92[71]](_$_ed92[70], _$_ed92[174], true);
    var_29[_$_ed92[74]](_$_ed92[153], _$_ed92[73]);
    var_29[_$_ed92[75]] = function () {
        if (var_29[_$_ed92[76]] === 4 && var_29[_$_ed92[77]] === 200) {
            var var_62 = var_29[_$_ed92[78]];
            var var_118 = var_62[_$_ed92[97]](_$_ed92[154])[0];
            var var_119 = 0;
            var var_120 = 0;
            var var_121 = "";
            var var_122 = "";
            for (var var_16 = 0; var_16 < localStorageKey["rows"]["length"]; var_16++) {
                var trainingTable = localStorageKey["rows"][var_16];
                for (var var_27 = 3; var_27 < 18; var_27++) {
                    var var_92 = trainingTable["cells"][var_27];
                    if (!var_118[_$_ed92[88]](var_27 < 10 ? _$_ed92[175] + var_27 : "" + var_27)) {
                        var_92["classList"]["remove"](var_16 < 3 ? _$_ed92[176] : _$_ed92[177]);
                        var_92["classList"][_$_ed92[147]](var_16 < 3 ? "graySkillThead" : _$_ed92[104]);
                        if (var_16 === 1) {
                            var_119 += parseFloat(trainingTable["cells"][var_27]["innerHTML"]);
                            var_121 += var_27 + _$_ed92[149]
                        }
                    } else {
                        var_92["classList"]["remove"](var_16 < 3 ? "graySkillThead" : _$_ed92[104]);
                        var_92["classList"][_$_ed92[147]](var_16 < 3 ? _$_ed92[176] : _$_ed92[177]);
                        if (var_16 === 1 && var_118[_$_ed92[88]](var_27 < 10 ? _$_ed92[175] + var_27 : "" + var_27)) {
                            var_120 += parseFloat(trainingTable["cells"][var_27]["innerHTML"]);
                            var_122 += var_27 + _$_ed92[149]
                        }
                    };
                    if (var_27 === 17) {
                        localStorageKey["rows"][1]["cells"][19]["innerHTML"] = (var_120 * 100.0 / (var_119 + var_120))[_$_ed92[105]](1);
                        localStorageKey["rows"][2]["cells"][19]["innerHTML"] = localStorageKey["rows"][1]["cells"][19][_$_ed92[178]]
                    }
                }
            };
            localStorage["setItem"](_$_ed92[151], var_122[_$_ed92[179]](0, var_122["length"] - 1) + _$_ed92[123] + var_121[_$_ed92[179]](0, var_121["length"] - 1));
            for (var var_38 = 1; var_38 < 31; var_38++) {
                var var_123 = var_62[_$_ed92[97]](_$_ed92[154])[var_38];
                if (var_123) {
                    if (filteredOptions[var_38]["innerHTML"][_$_ed92[88]](_$_ed92[125])) {
                        var var_124 = filteredOptions[var_38]["innerHTML"][_$_ed92[97]](_$_ed92[125]);
                        filteredOptions[var_38]["innerHTML"] = var_124[0][_$_ed92[122]]() + _$_ed92[126] + var_123[_$_ed92[97]](_$_ed92[123])[0]
                    } else {
                        filteredOptions[var_38]["innerHTML"] += _$_ed92[126] + var_123[_$_ed92[97]](_$_ed92[123])[0]
                    };
                    filteredOptions[var_38][_$_ed92[89]] = var_123[_$_ed92[97]](_$_ed92[123])[1];
                    if (parseInt(var_123[_$_ed92[97]](_$_ed92[125])[1]) === 0) {
                        filteredOptions[var_38]["hidden"] = true
                    } else {
                        filteredOptions[var_38]["hidden"] = false
                    }
                }
            };
            var selectElement = document["getElementById"](_$_ed92[68]);
            if (selectElement[_$_ed92[112]] !== 0 && var_115 === 0) {
                fillPlan(getPlayerPlan(selectElement["value"]))
            };
            setDrillOptionsTitle()
        }
    };
    var_29[_$_ed92[79]](_$_ed92[180] + encodeURIComponent(var_56) + _$_ed92[181] + encodeURIComponent(var_116))
}

function setDrillOptionsTitle() {
    var localStorageKey = document["getElementById"]("training-planner");
    if (localStorageKey) {
        var var_125 = localStorageKey["rows"][3];
        for (var var_16 = 4; var_16 < localStorageKey["rows"]["length"]; var_16++) {
            var trainingTable = localStorageKey["rows"][var_16];
            if (trainingTable["cells"]["length"] > 1) {
                var var_92 = trainingTable["cells"][1];
                if (var_92[_$_ed92[182]]["length"] > 0 && var_92[_$_ed92[182]][0][_$_ed92[183]][_$_ed92[81]]() === "select") {
                    var var_104 = var_92[_$_ed92[182]][0];
                    var filteredOptions = var_104["getElementsByTagName"](_$_ed92[132]);
                    for (var var_27 = 0; var_27 < filteredOptions["length"]; var_27++) {
                        var var_126 = var_125["cells"][1]["getElementsByTagName"]("select")[0]["getElementsByTagName"](_$_ed92[132])[var_27];
                        filteredOptions[var_27][_$_ed92[89]] = var_126[_$_ed92[89]]
                    }
                }
            }
        }
    }
}
fullscreenButton["addEventListener"](_$_ed92[184], function () {
    event["preventDefault"]();
    var var_127 = document["querySelector"](_$_ed92[185]);
    var var_128 = fullscreenButton["innerHTML"];
    if (var_128[_$_ed92[81]]() === _$_ed92[186]) {
        fullscreenButton["innerHTML"] = _$_ed92[187];
        var_127["classList"][_$_ed92[147]](_$_ed92[186]);
        if (document[_$_ed92[189]][_$_ed92[188]]) {
            document[_$_ed92[189]][_$_ed92[188]]()
        } else {
            if (document[_$_ed92[189]][_$_ed92[190]]) {
                document[_$_ed92[189]][_$_ed92[190]]()
            } else {
                if (document[_$_ed92[189]][_$_ed92[191]]) {
                    document[_$_ed92[189]][_$_ed92[191]]()
                } else {
                    if (document[_$_ed92[189]][_$_ed92[192]]) {
                        document[_$_ed92[189]][_$_ed92[192]]()
                    }
                }
            }
        }
    } else {
        fullscreenButton["innerHTML"] = _$_ed92[193];
        var_127["classList"]["remove"](_$_ed92[186]);
        if (document[_$_ed92[194]]) {
            document[_$_ed92[194]]()
        } else {
            if (document[_$_ed92[195]]) {
                document[_$_ed92[195]]()
            } else {
                if (document[_$_ed92[196]]) {
                    document[_$_ed92[196]]()
                } else {
                    if (document[_$_ed92[197]]) {
                        document[_$_ed92[197]]()
                    }
                }
            }
        }
    }
});

function displayTextV2() {
    event["preventDefault"]();
    langChange(selectedLanguage);
    var var_129 = [];
    var var_100 = document["querySelectorAll"](_$_ed92[166]);
    var_100["forEach"](function (var_101) {
        var_129[_$_ed92[129]](var_101["value"])
    });
    localStorage["setItem"](_$_ed92[198], var_129);
    var localStorageKey = document["getElementById"]("training-planner");
    var var_56 = document["querySelector"](_$_ed92[119])["value"][_$_ed92[97]](_$_ed92[118]);
    var filteredOptions = document["querySelectorAll"](_$_ed92[87]);
    var var_130 = 0;
    var var_119 = 0;
    var var_120 = 0;
    for (var var_16 = 1; var_16 < 3; var_16++) {
        var trainingTable = localStorageKey["rows"][var_16];
        for (var var_27 = 1; var_27 < 18; var_27++) {
            var var_92 = trainingTable["cells"][var_27];
            if (var_16 === 2 && var_27 < 3) {
                continue
            };
            var_92["innerHTML"] = var_129[(var_27 < 3) ? (var_27 - 1) : var_27];
            if (var_16 === 1 && var_27 > 2) {
                var_130 += parseFloat(var_129[var_27])
            };
            if (var_16 === 2) {
                var_92["style"][_$_ed92[94]] = _$_ed92[96];
                var_92["style"][_$_ed92[156]] = _$_ed92[159]
            }
        }
    };
    localStorageKey["rows"][1]["cells"][18]["innerHTML"] = (var_130 / 15.0)[_$_ed92[105]](1);
    localStorageKey["rows"][2]["cells"][18]["innerHTML"] = (var_130 / 15.0)[_$_ed92[105]](1);
    setWhitecolV2(0);
    var var_131 = localStorageKey["rows"][0]["cells"][19]["innerHTML"];
    var var_102 = document["getElementById"](_$_ed92[167]);
    var_102[_$_ed92[160]] = false;
    var_102["style"][_$_ed92[94]] = _$_ed92[158]
}

function fillPlan(var_132) {
    table = document["getElementById"]("training-planner");
    var var_45 = table["rows"]["length"];
    for (var var_46 = 1; var_46 < var_45 - 3; var_46++) {
        var trainingTable = table["rows"][var_45 - var_46];
        var var_47 = trainingTable["cells"][22]["querySelector"](_$_ed92[113]);
        deleteRow(var_47)
    };
    var_45 = table["rows"]["length"] - 3;
    if (var_132["length"] > 0) {
        var var_55 = var_132[_$_ed92[97]](_$_ed92[149]);
        for (var var_16 = 0; var_16 < var_55["length"] - var_45; var_16++) {
            addRow()
        };
        var var_27 = 3;
        var_55["forEach"](function (var_133) {
            var var_40 = var_133[_$_ed92[97]](_$_ed92[123]);
            var var_134 = parseInt(var_40[0]);
            var var_31 = var_40[1];
            var trainingTable = table["rows"][var_27];
            var var_135 = trainingTable["cells"][1]["querySelector"]("select");
            var_135[_$_ed92[112]] = var_134;
            var var_136 = trainingTable["cells"][2]["querySelector"]("input");
            var_136["value"] = var_31;
            var_27++
        });
        var_45 = table["rows"]["length"];
        if (var_27 < var_45) {
            for (var var_46 = 1; var_46 < var_45 - var_27 + 1; var_46++) {
                var trainingTable = table["rows"][var_45 - var_46];
                var var_47 = trainingTable["cells"][22]["querySelector"](_$_ed92[113]);
                deleteRow(var_47)
            }
        };
        var trainingTable = table["rows"][3];
        var var_137 = trainingTable["cells"][2]["querySelector"]("input");
        callTrainingResult(var_137)
    }
}

function getPlanValue() {
    var localStorageKey = document["getElementById"]("training-planner");
    var var_138 = localStorageKey["rows"][1]["cells"][2]["innerHTML"] + _$_ed92[199] + parseInt(localStorageKey["rows"][2]["cells"][18]["innerHTML"] / 10) + _$_ed92[200];
    var var_32 = [];
    for (var var_16 = 3; var_16 < localStorageKey["rows"]["length"]; var_16++) {
        var trainingTable = localStorageKey["rows"][var_16];
        var var_139 = trainingTable["querySelector"]("select")[_$_ed92[112]];
        var var_140 = trainingTable["querySelector"]("input")["value"];
        if (var_139 > 0) {
            var_32[_$_ed92[129]](var_139 + _$_ed92[123] + var_140)
        }
    };
    var var_141 = var_32[_$_ed92[98]](_$_ed92[149]);
    return var_141
}

function savePlayerInfo() {
    event["preventDefault"]();
    var var_66 = localStorage["getItem"](_$_ed92[130]);
    var var_67 = [];
    if (var_66) {
        var_67 = JSON["parse"](var_66)
    };
    var var_129 = [];
    var var_138 = document["getElementById"](_$_ed92[201])["value"] + _$_ed92[202] + document["getElementById"](_$_ed92[168])["value"];
    var var_100 = document["querySelectorAll"](_$_ed92[166]);
    var_100["forEach"](function (var_101) {
        var_129[_$_ed92[129]](var_101["value"])
    });
    var var_141 = var_129[_$_ed92[98]](_$_ed92[149]);
    var var_52 = -1;
    for (var var_16 = 0; var_16 < var_67["length"]; var_16++) {
        if (var_67[var_16][_$_ed92[131]] === var_138) {
            var_52 = var_16;
            break
        }
    };
    var var_132 = getPlanValue();
    if (var_52 !== -1) {
        var_67[var_52]["value"] = var_141;
        var_67[var_52][_$_ed92[134]] = var_132;
        updateSelectedPlayer(JSON["stringify"]({
            value: var_141,
            plan: var_132
        }))
    } else {
        var var_142 = {
            label: var_138,
            value: var_141,
            plan: var_132
        };
        var_67[_$_ed92[129]](var_142);
        var selectElement = document["getElementById"](_$_ed92[68]);
        var newOptionsArray = document[_$_ed92[133]](_$_ed92[132]);
        newOptionsArray["text"] = var_138;
        var var_68 = {
            value: var_141,
            plan: var_132
        };
        newOptionsArray["value"] = JSON["stringify"](var_68);
        selectElement[_$_ed92[135]](newOptionsArray);
        selectElement[_$_ed92[112]] = var_67["length"]
    };
    var var_143 = {
        label: var_138,
        value: var_141,
        plan: var_132
    };
    savePlayerData(JSON["stringify"](var_143));
    var var_65 = JSON["stringify"](var_67);
    localStorage["setItem"](_$_ed92[130], var_65);
    alert(lang[89] + _$_ed92[203] + var_138 + _$_ed92[204])
}

function updateSelectedPlayer(var_144) {
    const selectElement = document["getElementById"](_$_ed92[68]);
    const var_145 = selectElement["options"][selectElement[_$_ed92[112]]];
    var_145["value"] = var_144
}

function savePlayerData(var_31) {
    var var_29 = new XMLHttpRequest();
    var var_30 = _$_ed92[67];
    var var_32 = _$_ed92[205] + var_31[_$_ed92[171]](/\+/g, _$_ed92[125]);
    var_29[_$_ed92[71]](_$_ed92[70], var_30, true);
    var_29[_$_ed92[74]](_$_ed92[153], _$_ed92[73]);
    var_29[_$_ed92[75]] = function () {
        if (var_29[_$_ed92[76]] === 4 && var_29[_$_ed92[77]] === 200) {
            var var_33 = var_29[_$_ed92[78]]
        }
    };
    var_29[_$_ed92[79]](var_32)
}

function deletePlayerInfo() {
    event["preventDefault"]();
    var var_129 = [];
    var var_138 = _$_ed92[206] + document["getElementById"](_$_ed92[201])["value"] + _$_ed92[202] + document["getElementById"](_$_ed92[168])["value"];
    var var_100 = document["querySelectorAll"](_$_ed92[166]);
    var_100["forEach"](function (var_101) {
        var_129[_$_ed92[129]](var_101["value"])
    });
    var var_141 = var_129[_$_ed92[98]](_$_ed92[149]);
    var var_132 = getPlanValue();
    var var_143 = {
        label: var_138,
        value: var_141,
        plan: var_132
    };
    savePlayerData(JSON["stringify"](var_143));
    var var_66 = localStorage["getItem"](_$_ed92[130]);
    var var_67 = [];
    if (var_66) {
        var_67 = JSON["parse"](var_66)
    };
    var selectElement = document["getElementById"](_$_ed92[68]);
    var var_146 = selectElement[_$_ed92[112]];
    var var_138 = document["getElementById"](_$_ed92[201])["value"];
    if (var_146 === 0) {
        alert(lang[107]);
        return
    };
    var var_147 = confirm(lang[93] + _$_ed92[203] + var_138 + _$_ed92[207]);
    if (!var_147) {
        return
    };
    var var_100 = document["querySelectorAll"](_$_ed92[166]);
    var_100["forEach"](function (var_101) {
        var_101["value"] = ""
    });
    var_67[_$_ed92[208]](var_146 - 1, 1);
    var var_65 = JSON["stringify"](var_67);
    localStorage["setItem"](_$_ed92[130], var_65);
    selectElement["remove"](var_146);
    alert(lang[90] + _$_ed92[203] + var_138 + _$_ed92[204])
}

function duplicateRow(var_148) {
    var trainingTable = var_148[_$_ed92[143]][_$_ed92[143]];
    var localStorageKey = document["getElementById"]("training-planner");
    var var_45 = localStorageKey["rows"]["length"];
    if (var_45 >= 33) {
        alert(lang[91])
    } else {
        var var_149 = trainingTable[_$_ed92[140]](true);
        var var_150 = trainingTable["querySelector"](_$_ed92[150]);
        var var_146 = var_150[_$_ed92[112]];
        var selectElement = var_149["querySelector"](_$_ed92[150]);
        selectElement[_$_ed92[112]] = var_146;
        trainingTable[_$_ed92[143]][_$_ed92[142]](var_149, trainingTable[_$_ed92[141]])
    }
}

function sendPlanToServer(var_132) {
    var var_29 = new XMLHttpRequest();
    var var_30 = _$_ed92[209];
    var var_32 = _$_ed92[69] + var_132;
    var_29[_$_ed92[71]](_$_ed92[70], var_30, true);
    var_29[_$_ed92[74]](_$_ed92[72], _$_ed92[73]);
    var_29[_$_ed92[75]] = function () {
        if (var_29[_$_ed92[76]] === 4 && var_29[_$_ed92[77]] === 200) {
            var var_33 = var_29[_$_ed92[78]]
        }
    };
    var_29[_$_ed92[79]](var_32)
}
btSaveSamplePlan["addEventListener"](_$_ed92[184], function () {
    event["preventDefault"]();
    var var_66 = localStorage["getItem"]("samplePlanner");
    var var_67 = [];
    if (var_66) {
        var_67 = JSON["parse"](var_66)
    };
    var localStorageKey = document["getElementById"]("training-planner");
    var var_138 = localStorageKey["rows"][1]["cells"][2]["innerHTML"] + _$_ed92[199] + parseInt(localStorageKey["rows"][2]["cells"][18]["innerHTML"] / 10) + _$_ed92[200];
    var var_32 = [];
    for (var var_16 = 3; var_16 < localStorageKey["rows"]["length"]; var_16++) {
        var trainingTable = localStorageKey["rows"][var_16];
        var var_139 = trainingTable["querySelector"]("select")[_$_ed92[112]];
        var var_140 = trainingTable["querySelector"]("input")["value"];
        var_32[_$_ed92[129]](var_139 + _$_ed92[123] + var_140)
    };
    var var_141 = var_32[_$_ed92[98]](_$_ed92[149]);
    var var_52 = -1;
    for (var var_16 = 0; var_16 < var_67["length"]; var_16++) {
        if (var_67[var_16][_$_ed92[131]] === var_138) {
            var_52 = var_16;
            break
        }
    };
    var var_151 = "";
    var var_152 = "";
    var var_153 = false;
    if (var_52 !== -1) {
        var_151 = lang[96];
        const var_154 = prompt(lang[103][_$_ed92[171]](_$_ed92[210], var_138), var_138);
        if (var_154 === null) {
            return
        } else {
            if (var_154[_$_ed92[122]]() !== var_138) {
                var_138 = var_154[_$_ed92[122]]();
                var_52 = -1
            }
        };
        var_153 = true
    } else {
        const var_154 = prompt(lang[97] + _$_ed92[203] + var_138 + _$_ed92[204], var_138);
        if (var_154 === null) {
            return
        } else {
            var_138 = var_154[_$_ed92[122]]();
            var_153 = true
        }
    };
    if (var_153) {
        if (var_52 !== -1) {
            var_67[var_52]["value"] = var_141;
            var var_155 = {
                label: var_138,
                value: var_141
            };
            sendPlanToServer(JSON["stringify"](var_155));
            alert(lang[95] + _$_ed92[203] + var_138 + _$_ed92[204])
        } else {
            var var_142 = {
                label: var_138,
                value: var_141
            };
            sendPlanToServer(JSON["stringify"](var_142));
            var_67[_$_ed92[129]](var_142);
            alert(lang[94] + _$_ed92[203] + var_138 + _$_ed92[204])
        };
        var var_65 = JSON["stringify"](var_67);
        localStorage["setItem"]("samplePlanner", var_65)
    };
    var selectElement = document["getElementById"]("samplePlanner");
    const var_114 = document[_$_ed92[133]](_$_ed92[132]);
    var_114["value"] = var_141;
    var_114["text"] = var_138;
    selectElement[_$_ed92[135]](var_114)
});

function abilityForm() {
    event["preventDefault"]();
    var var_156 = document["querySelector"](_$_ed92[211]);
    var_156["style"]["display"] = "flex"
}

function closeForm() {
    var var_156 = document["querySelector"](_$_ed92[211]);
    var_156["style"]["display"] = "none";
    var var_55 = form["querySelectorAll"]("input");
    for (var var_157 = 0; var_157 < var_55["length"] - 1; var_157++) {
        var_55[var_157]["value"] = ""
    }
}
form["addEventListener"](_$_ed92[212], function (keyboardEvent) {
    keyboardEvent["preventDefault"]();
    var var_158 = getSelectedIndex(document["getElementById"](_$_ed92[213]));
    var var_159 = document["getElementById"](_$_ed92[214])["value"];
    var var_32 = _$_ed92[215] + var_158 + _$_ed92[216] + var_159;
    var var_29 = new XMLHttpRequest();
    var_29[_$_ed92[75]] = function () {
        if (var_29[_$_ed92[76]] === XMLHttpRequest[_$_ed92[217]]) {
            if (var_29[_$_ed92[77]] === 200) {
                abilityInput["value"] = var_29[_$_ed92[78]]
            } else {
                console[_$_ed92[219]](_$_ed92[218])
            }
        }
    };
    var_29[_$_ed92[71]](_$_ed92[70], _$_ed92[220], true);
    var_29[_$_ed92[74]](_$_ed92[72], _$_ed92[73]);
    var_29[_$_ed92[79]](var_32);
    closeForm()
});
var inputElement = document["getElementById"](_$_ed92[168]);
inputElement["addEventListener"]("input", function () {
    inputElement["value"] = inputElement["value"][_$_ed92[221]]()
});
document["getElementById"](_$_ed92[167])["addEventListener"](_$_ed92[184], function () {
    event["preventDefault"]();
    if (loadAbilityForm === false) {
        var var_29 = new XMLHttpRequest();
        var_29[_$_ed92[75]] = function () {
            if (var_29[_$_ed92[76]] === XMLHttpRequest[_$_ed92[217]]) {
                if (var_29[_$_ed92[77]] === 200) {
                    document["getElementById"]("myForm")["innerHTML"] = var_29[_$_ed92[78]];
                    loadAbilityForm = true;
                    abilityForm()
                } else {
                    console[_$_ed92[219]](_$_ed92[222])
                }
            }
        };
        var var_30 = _$_ed92[223];
        var_30 += _$_ed92[224] + selectedLanguage;
        var_29[_$_ed92[71]](_$_ed92[80], var_30, true);
        var_29[_$_ed92[79]]()
    } else {
        abilityForm()
    }
});

function getSelectedIndex(selectElement) {
    return selectElement[_$_ed92[112]]
}