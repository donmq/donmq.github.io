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
    const storedOptionsArray = JSON.parse(localStorage.getItem("selectOptions")) || [];
    const filteredOptions = Array.from(selectElement.options).map((x) => {
        return {
            value: x.value,
            text: x.text
        }
    });
    const mergedOptions = filteredOptions.filter((x) => {
        return !storedOptionsArray.some((y) => {
            return y.value === x.value
        })
    });
    const hoverEvent = [...storedOptionsArray, ...mergedOptions];
    localStorage.setItem("top11SamplePlanOptions", JSON.stringify(hoverEvent));
    while (selectElement.options.length > 0) {
        selectElement.options.remove(0)
    }
}
const rows = document.querySelectorAll("#training-planner tbody tr");
rows.forEach((trainingTable) => {
    trainingTable.addEventListener("mouseover", () => {
        trainingTable.style.backgroundColor = "lightgreen"
    });
    trainingTable.addEventListener("mouseout", () => {
        trainingTable.style.backgroundColor = ""
    })
});
const trainingTable = document.getElementById("training-planner");
trainingTable.addEventListener("keydown", (keyboardEvent) => {
    if (keyboardEvent.key === "Tab" || keyboardEvent.key === "Enter") {
        keyboardEvent.preventDefault();
        const rowCells = keyboardEvent.target;
        const tableColumns = rowCells.parentElement;
        const currentRowIndex = tableColumns.parentElement;
        const comparisonTable = Array.from(table.rows).indexOf(currentRowIndex);
        const isMobileDevice = table.rows[comparisonTable + 1];
        if (isMobileDevice) {
            const selectedOption = isMobileDevice.cells[tableColumns.cellIndex].querySelector("input");
            if (selectedOption) {
                selectedOption.focus();
                selectedOption.select()
            }
        }
    }
});

function removeSoSanhCellStyle() {
    const localStorageKey = document.getElementById("bangSoSanh");
    const elementTD = localStorageKey.getElementsByTagName("td");
    for (let i = 0; i < 60; i++) {
        elementTD[i].removeAttribute("style")
    }
}

function xemSoSanh() {
    var elementSoSanh = document.getElementById("soSanhContainer");
    elementSoSanh.style.display = "flex"
}

function closeSoSanhTable() {
    var elementSoSanh = document.getElementById("soSanhContainer");
    elementSoSanh.style.display = "none"
}

function addSoSanh() {
    var elementTraining = document.getElementById("training-planner");
    var trainingTable = elementTraining.rows[2];
    var var_19 = document.querySelectorAll("#bangSoSanh .PA1");
    var var_20 = document.querySelectorAll("#bangSoSanh .PA2");
    var var_21 = document.querySelectorAll("#bangSoSanh .PA3");
    var elementSoSanh = document.getElementById("bangSoSanh");
    var var_22 = document.querySelectorAll("#bangSoSanh .xoaSoSanh");
    var var_23 = document.getElementById("rowXoaButton");
    var_23.hidden = false;
    var var_24 = -1;
    var var_25 = var_19;
    for (var i = 0; i < var_22.length; i++) {
        if (var_22[i].hidden === true) {
            var_24 = i;
            var_22[i].hidden = false;
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
    for (var i = 0; i < var_25.length; i++) {
        var var_26 = trainingTable.cells[i + 3];
        var_25[i].innerHTML = var_26.innerHTML;
        var currentRowIndex = elementSoSanh.rows[i + 1];
        if (var_26.classList.contains("graySkillThead")) {
            for (var i = 0; i < 4; i++) {
                currentRowIndex.cells[i].style.backgroundColor = "gray"
            }
        } else {
            for (var i = 0; i < 4; i++) {
                if (currentRowIndex.cells[i].style.backgroundColor === "gray") {
                    currentRowIndex.cells[i].removeAttribute("style")
                }
            }
        }
    }
}

function xoaPA1() {
    var var_28 = document.querySelectorAll("#bangSoSanh .PA1");
    for (var i = 0; i < var_28.length; i++) {
        var_28[i].innerHTML = ""
    };
    var var_22 = document.querySelectorAll("#bangSoSanh .xoaSoSanh");
    var_22[0].hidden = true;
    if (var_22[0].hidden === true && var_22[1].hidden === true && var_22[2].hidden === true) {
        var var_23 = document.getElementById("rowXoaButton");
        var_23.hidden = true
    }
}

function xoaPA2() {
    var var_28 = document.querySelectorAll("#bangSoSanh .PA2");
    for (var i = 0; i < var_28.length; i++) {
        var_28[i].innerHTML = ""
    };
    var var_22 = document.querySelectorAll("#bangSoSanh .xoaSoSanh");
    var_22[1].hidden = true;
    if (var_22[0].hidden === true && var_22[1].hidden === true && var_22[2].hidden === true) {
        var var_23 = document.getElementById("rowXoaButton");
        var_23.hidden = true
    }
}

function xoaPA3() {
    var var_28 = document.querySelectorAll("#bangSoSanh .PA3");
    for (var i = 0; i < var_28.length; i++) {
        var_28[i].innerHTML = ""
    };
    var var_22 = document.querySelectorAll("#bangSoSanh .xoaSoSanh");
    var_22[2].hidden = true;
    if (var_22[0].hidden === true && var_22[1].hidden === true && var_22[2].hidden === true) {
        var var_23 = document.getElementById("rowXoaButton");
        var_23.hidden = true
    }
}

function sendData() {
    var var_29 = new XMLHttpRequest();
    var var_30 = "https://top11experience.com/wp-content/themes/top11trainplanner/pro-getPlayerList.php";
    var selectElement = document.getElementById("savedPlayers-select");
    var var_31 = selectElement.value;
    var var_32 = "param1=" + var_31;
    var_29.open("POST", var_30, true);
    var_29.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    var_29.onreadystatechange = function () {
        if (var_29.readyState === 4 && var_29.status === 200) {
            var var_33 = var_29.responseText
        }
    };
    var_29.send(var_32)
}

function loadData() {
    var var_29 = new XMLHttpRequest();
    var_29.open("GET", "https://top11experience.com/wp-content/themes/top11trainplanner/pro-getPlayerList.php", true);
    var_29.onreadystatechange = function () {
        if (var_29.readyState === 4 && var_29.status === 200) {
            var var_32 = JSON.parse(var_29.responseText)
        }
    };
    var_29.send()
}

function isMobileDevice() {
    var var_34 = navigator.userAgent.toLowerCase();
    if (var_34.indexOf("android") > -1 || var_34.indexOf("iphone") > -1 || var_34.indexOf("ipad") > -1 || var_34.indexOf("ipod") > -1) {
        return true
    } else {
        return false
    }
}

function findSkill(var_35) {
    var filteredOptions = document.querySelectorAll("#training-planner .select-option option");
    var var_36 = document.getElementById("training-planner");
    var var_37 = var_35.innerHTML;
    for (var i = 0; i < filteredOptions.length; i++) {
        if (filteredOptions[i].title.includes(var_37)) {
            filteredOptions[i].innerHTML = removeTextBeforeDash(filteredOptions[i].innerHTML);
            filteredOptions[i].innerHTML = var_37 + " - " + filteredOptions[i].innerHTML;
            filteredOptions[i].style.backgroundColor = "cyan"
        } else {
            filteredOptions[i].innerHTML = removeTextBeforeDash(filteredOptions[i].innerHTML);
            filteredOptions[i].style.backgroundColor = "white"
        };
        if (i < 18 && i != var_35.index) {
            var_36.rows[0].cells[i].style.color = "black"
        };
        var_35.style.color = "red"
    }
}

function removeTextBeforeDash(var_39) {
    const var_40 = var_39.split(" - ");
    if (var_40.length > 1) {
        return var_40.slice(1).join(" - ")
    };
    return var_39
}

function showTable() {
    var localStorageKey = document.getElementById("resultTable");
    localStorageKey.style.display = "flex";
    document.querySelector(".close-button").style.display = "block";
    var var_41 = document.getElementById("training-planner");
    var var_42 = document.getElementById("defTable");
    var_42.rows[0].cells[0].innerHTML = lang[10];
    var_42.rows[1].cells[0].innerHTML = lang[101];
    var_42.rows[1].cells[1].innerHTML = lang[99];
    var_42.rows[1].cells[2].innerHTML = lang[100];
    var var_43 = 0;
    var var_44 = 0;
    for (var i = 2; i < var_42.rows.length; i++) {
        if (i < 7) {
            for (var i = 0; i < 3; i++) {
                var_42.rows[i].cells[i].innerHTML = var_41.rows[i].cells[i + 1].innerHTML;
                var_42.rows[i].cells[i].style.backgroundColor = var_41.rows[3].cells[i + 1].classList.contains("graySkillTbody") ? "gray" : ""
            };
            var_43 += parseInt(var_41.rows[1].cells[i + 1].innerHTML);
            var_44 += parseInt(var_41.rows[2].cells[i + 1].innerHTML)
        } else {
            var_42.rows[i].cells[0].innerHTML = lang[98];
            var_42.rows[i].cells[1].innerHTML = parseFloat(var_43 / 5).toFixed(1) + "%";
            var_42.rows[i].cells[2].innerHTML = parseFloat(var_44 / 5).toFixed(1) + "%"
        }
    };
    var_42 = document.getElementById("attackTable");
    var_42.rows[0].cells[0].innerHTML = lang[16];
    var_42.rows[1].cells[0].innerHTML = lang[101];
    var_42.rows[1].cells[1].innerHTML = lang[99];
    var_42.rows[1].cells[2].innerHTML = lang[100];
    var_43 = 0;
    var_44 = 0;
    for (var i = 2; i < var_42.rows.length; i++) {
        if (i < 7) {
            for (var i = 0; i < 3; i++) {
                var_42.rows[i].cells[i].innerHTML = var_41.rows[i].cells[i + 6].innerHTML;
                var_42.rows[i].cells[i].style.backgroundColor = var_41.rows[3].cells[i + 6].classList.contains("graySkillTbody") ? "gray" : ""
            };
            var_43 += parseInt(var_41.rows[1].cells[i + 6].innerHTML);
            var_44 += parseInt(var_41.rows[2].cells[i + 6].innerHTML)
        } else {
            var_42.rows[i].cells[0].innerHTML = lang[98];
            var_42.rows[i].cells[1].innerHTML = parseFloat(var_43 / 5).toFixed(1) + "%";
            var_42.rows[i].cells[2].innerHTML = parseFloat(var_44 / 5).toFixed(1) + "%"
        }
    };
    var_42 = document.getElementById("physicalTable");
    var_42.rows[0].cells[0].innerHTML = lang[22];
    var_42.rows[1].cells[0].innerHTML = lang[101];
    var_42.rows[1].cells[1].innerHTML = lang[99];
    var_42.rows[1].cells[2].innerHTML = lang[100];
    var_43 = 0;
    var_44 = 0;
    for (var i = 2; i < var_42.rows.length; i++) {
        if (i < 7) {
            for (var i = 0; i < 3; i++) {
                var_42.rows[i].cells[i].innerHTML = var_41.rows[i].cells[i + 11].innerHTML;
                var_42.rows[i].cells[i].style.backgroundColor = var_41.rows[3].cells[i + 11].classList.contains("graySkillTbody") ? "gray" : ""
            };
            var_43 += parseInt(var_41.rows[1].cells[i + 11].innerHTML);
            var_44 += parseInt(var_41.rows[2].cells[i + 11].innerHTML)
        } else {
            var_42.rows[i].cells[0].innerHTML = lang[98];
            var_42.rows[i].cells[1].innerHTML = parseFloat(var_43 / 5).toFixed(1) + "%";
            var_42.rows[i].cells[2].innerHTML = parseFloat(var_44 / 5).toFixed(1) + "%"
        }
    };
    var_42 = document.getElementById("summary2");
    var_42.rows[0].cells[0].innerHTML = var_41.rows[0].cells[18].innerHTML;
    var_42.rows[0].cells[1].innerHTML = var_41.rows[0].cells[19].innerHTML;
    var_42.rows[1].cells[0].innerHTML = lang[99];
    var_42.rows[1].cells[2].innerHTML = lang[99];
    var_42.rows[1].cells[1].innerHTML = lang[100];
    var_42.rows[1].cells[3].innerHTML = lang[100];
    var_42.rows[2].cells[0].innerHTML = var_41.rows[1].cells[18].innerHTML;
    var_42.rows[2].cells[1].innerHTML = var_41.rows[2].cells[18].innerHTML;
    var_42.rows[2].cells[2].innerHTML = var_41.rows[1].cells[19].innerHTML;
    var_42.rows[2].cells[3].innerHTML = var_41.rows[2].cells[19].innerHTML;
    var_42 = document.getElementById("summary3");
    var_42.rows[0].cells[0].innerHTML = var_41.rows[0].cells[20].innerHTML;
    var_42.rows[0].cells[1].innerHTML = var_41.rows[0].cells[21].innerHTML;
    var_42.rows[1].cells[0].innerHTML = var_41.rows[2].cells[20].innerHTML;
    var_42.rows[1].cells[1].innerHTML = var_41.rows[2].cells[21].innerHTML
}

function closeTable() {
    var localStorageKey = document.getElementById("resultTable");
    localStorageKey.style.display = "none";
    document.querySelector(".close-button").style.display = "none"
}
selectElement.onchange = function () {
    var localStorageKey = document.getElementById("training-planner");
    var var_45 = localStorageKey.rows.length;
    addRow();
    if (selectElement.selectedIndex === 0) {
        for (var i = 1; i < var_45 - 2; i++) {
            var trainingTable = localStorageKey.rows[var_45 - i];
            var var_47 = trainingTable.cells[22].querySelector("button");
            deleteRow(var_47)
        };
        return
    };
    var var_48 = selectElement.value;
    fillPlan(var_48)
};

function langChange(var_49) {
    loadAbilityForm = false;
    selectedLanguage = var_49;
    localStorage.setItem("languageSelect", var_49);
    var var_50 = localStorage.getItem("pro-langDic");
    var var_51 = [];
    if (var_50) {
        var_51 = JSON.parse(var_50)
    };
    var var_52 = -1;
    if (var_51) {
        for (var i = 0; i < var_51.length; i++) {
            if (var_51[i].langCode === var_49) {
                var_52 = i;
                break
            }
        };
        if (var_52 !== -1) {
            var var_53 = var_51[var_52].langDic;
            langFill(var_53)
        } else {
            getLangDic(var_49)
        }
    }
}

function langFill(var_54) {
    var var_55 = document.querySelectorAll(".langChange");
    var var_56 = document.querySelector("#rolestxt").value.split("+");
    var var_57 = (var_56.includes("GK") ? 1 : 0);
    var i = 0;
    lang = JSON.parse(var_54).langDic;
    if (lang.length < langlengthPro) {
        var selectElement = document.getElementById("language-select");
        getLangDic(selectElement.value)
    };
    for (var i = 0; i < var_55.length; i++) {
        if (var_55[i].innerHTML.trim() !== "") {
            var_55[i].innerHTML = (lang[i].includes(":")) ? lang[i].split(":")[var_57] : lang[i];
            i++
        }
    };
    var var_58 = lang.slice(59, 90);
    var filteredOptions = document.querySelectorAll(".training-page .select-option option");
    for (var i = 0; i < filteredOptions.length; i++) {
        if (filteredOptions[i].innerHTML.includes("|")) {
            var var_59 = filteredOptions[i].innerHTML;
            var var_40 = var_59.split(" | ");
            var_40[0] = var_58[filteredOptions[i].index];
            var var_60 = var_40.join(" | ");
            filteredOptions[i].innerHTML = var_60
        } else {
            filteredOptions[i].innerHTML = var_58[filteredOptions[i].index]
        }
    };
    setWhitecolV2(1);
    var var_61 = document.getElementById("bangSoSanh");
    var_61.rows[0].cells[0].innerHTML = lang[101];
    var var_41 = document.getElementById("training-planner");
    for (var i = 1; i < 20; i++) {
        var_61.rows[(i < 16 ? i : i + 1)].cells[0].innerHTML = var_41.rows[0].cells[i + 2].innerHTML
    }
}

function getLangDic(var_49) {
    var var_29 = new XMLHttpRequest();
    var var_30 = "https://top11experience.com/wp-content/themes/top11trainplanner/pro-langDic.php";
    var var_32 = "langCode=" + encodeURIComponent(var_49);
    var_29.open("POST", var_30, true);
    var_29.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    var_29.onreadystatechange = function () {
        if (var_29.readyState === 4 && var_29.status === 200) {
            var var_62 = var_29.responseText;
            var var_51 = JSON.parse(var_62);
            saveLocalLangDic(var_62, var_49);
            langFill(JSON.stringify(var_51))
        }
    };
    var_29.send(var_32)
}

function saveLocalLangDic(var_51, var_49) {
    var var_50 = localStorage.getItem("pro-langDic");
    var var_63 = [];
    if (var_50) {
        var_63 = JSON.parse(var_50)
    };
    var var_52 = -1;
    for (var i = 0; i < var_63.length; i++) {
        if (var_63[i].langCode === var_49) {
            var_52 = i;
            break
        }
    };
    if (var_52 !== -1) {
        var_63[var_52].langDic = var_51
    } else {
        var var_64 = {
            langCode: var_49,
            langDic: var_51
        };
        var_63.push(var_64)
    };
    var var_65 = JSON.stringify(var_63);
    localStorage.setItem("pro-langDic", var_65)
}

function loadSavedPlayerInfo() {
    var var_66 = localStorage.getItem("top11experiencePlayerData");
    var var_67 = JSON.parse(var_66);
    if (var_67) {
        var selectElement = document.getElementById("savedPlayers-select");
        for (var i = 0; i < var_67.length; i++) {
            if (!selectElement.innerHTML.includes(var_67[i].label)) {
                var x = document.createElement("option");
                x.text = var_67[i].label;
                var var_68 = {
                    value: var_67[i].value,
                    plan: var_67[i].plan
                };
                x.value = JSON.stringify(var_68);
                selectElement.appendChild(x)
            }
        }
    }
}
window.addEventListener("beforeunload", function (keyboardEvent) {
    var var_69 = document.getElementById("savedPlayers-select");
    if (var_69.selectedIndex > 0) {
        var var_70 = "B\u1ea1n c\xF3 ch\u1eafc mu\u1ed1n r\u1eddi kh\u1ecfi trang?";
        keyboardEvent.returnValue = var_70;
        return var_70;
        keyboardEvent.preventDefault()
    }
});
document.addEventListener("DOMContentLoaded", function () {
    if (localStorage.getItem("languageSelect")) {
        var selectElement = document.getElementById("language-select");
        selectElement.value = selectedLanguage;
        langChange(selectedLanguage)
    };
    loadSavedPlayerInfo();
    loadSamplePlan()
});

function addRow() {
    var localStorageKey = document.getElementById("training-planner");
    var var_45 = localStorageKey.rows.length;
    if (var_45 >= 33) {
        alert(lang[91])
    } else {
        var var_71 = localStorageKey.rows[localStorageKey.rows.length - 1];
        var var_72 = var_71.cloneNode(true);
        var_71.parentNode.insertBefore(var_72, var_71.nextSibling);
        var_71 = localStorageKey.rows[localStorageKey.rows.length - 1];
        for (var i = 3; i < var_71.cells.length - 1; i++) {
            var_71.cells[i].innerHTML = ""
        }
    }
}

function deleteRow(var_73) {
    var trainingTable = var_73.parentNode.parentNode;
    var localStorageKey = document.getElementById("training-planner");
    var var_45 = localStorageKey.rows.length;
    if (var_45 === 4) {
        alert(lang[92])
    } else {
        trainingTable.parentNode.removeChild(trainingTable)
    };
    var var_74 = 0;
    var var_75 = 0;
    for (var i = 3; i < var_45 - 1; i++) {
        var_74 += parseInt(localStorageKey.rows[i].cells[20].innerHTML);
        var_75 += parseInt(localStorageKey.rows[i].cells[21].innerHTML)
    };
    localStorageKey.rows[2].cells[20].innerHTML = var_74;
    localStorageKey.rows[2].cells[21].innerHTML = var_75
}

function testHidden() {
    if (isOptionHiddenSupported()) {
        alert("Tr\xECnh duy\u1ec7t h\u1ed7 tr\u1ee3 thu\u1ed9c t\xEDnh hidden c\u1ee7a c\xE1c ph\u1ea7n t\u1eed option trong select.")
    } else {
        alert("Tr\xECnh duy\u1ec7t kh\xF4ng h\u1ed7 tr\u1ee3 thu\u1ed9c t\xEDnh hidden c\u1ee7a c\xE1c ph\u1ea7n t\u1eed option trong select.")
    }
}

function isOptionHiddenSupported() {
    const var_77 = document.createElement("select");
    const var_78 = document.createElement("option");
    return "hidden" in var_78 && "add" in var_77
}

function callTrainingResult(selectElement) {
    event.preventDefault();
    var currentRowIndex = selectElement.parentNode.parentNode;
    var var_79 = currentRowIndex.rowIndex;
    var localStorageKey = document.getElementById("training-planner");
    var var_56 = localStorageKey.rows[1].cells[2].innerHTML;
    var abilityInput = document.getElementById("abilitytxt").value;
    var trainingTable = localStorageKey.rows[(var_79 < 4 ? 1 : var_79 - 1)];
    var var_80 = "";
    var var_81 = "";
    var var_82 = isMobileDevice();
    for (var i = 3; i < 18; i++) {
        var_80 += trainingTable.cells[i].innerHTML + (i === 17 ? "" : ",")
    };
    var var_45 = localStorageKey.rows.length;
    for (r = var_79; r < var_45; r++) {
        trainingTable = localStorageKey.rows[r];
        var_81 += trainingTable.querySelector(".select-option").selectedIndex + ":";
        var_81 += parseInt(trainingTable.querySelector("input").value) + (r === var_45 - 1 ? "" : ",");
        var var_83 = trainingTable.querySelector(".select-option");
        var var_84 = var_83.options[var_83.selectedIndex];
        if (var_84) {
            var_83.style.color = var_84.style.color
        }
    };
    var var_85 = localStorage.getItem("top11experiencePlayerSkillCol");
    var var_29 = new XMLHttpRequest();
    var_29.open("POST", "https://top11experience.com/wp-content/themes/top11trainplanner/pro-trainingPlanCalcu.php", true);
    var_29.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    var_29.onreadystatechange = function () {
        if (var_29.readyState === 4 && var_29.status === 200) {
            var var_86 = var_29.responseText;
            var var_87 = var_86.split("<br>");
            for (var i = var_79; i < var_45; i++) {
                trainingTable = localStorageKey.rows[i];
                var var_89 = var_87[i - var_79].split("|");
                var var_90 = trainingTable.querySelector("input");
                var_90.style.backgroundColor = (var_90.value !== var_89[21] ? "lightsalmon" : "");
                var_90.value = var_89[21];
                for (var z = 0; z < 19; z++) {
                    var var_92 = trainingTable.cells[3 + z];
                    var_92.innerHTML = var_89[z];
                    if ((z < 15 && parseInt(var_92.innerHTML) >= 340) || (z === 15 && parseInt(var_92.innerHTML) >= 180)) {
                        var_92.style.backgroundColor = "red";
                        var_92.title = ((z < 15 && parseInt(var_92.innerHTML) >= 340) ? lang[104] : lang[105])
                    } else {
                        var_92.style.backgroundColor = localStorageKey.rows[1].cells[3 + z].style.backgroundColor;
                        var_92.removeAttribute("title")
                    };
                    if (z < 15) {
                        if (parseInt(trainingTable.cells[3 + z].innerHTML) === parseInt(localStorageKey.rows[(i === 3 ? 1 : i - 1)].cells[3 + z].innerHTML)) {
                            trainingTable.cells[3 + z].style.color = ((trainingTable.cells[3 + z].style.backgroundColor === "gray") ? "gray" : "");
                            trainingTable.cells[3 + z].style.fontWeight = "normal"
                        } else {
                            trainingTable.cells[3 + z].style.color = "blue";
                            trainingTable.cells[3 + z].style.fontWeight = "bold"
                        }
                    };
                    if (i === var_45 - 1 && z < 17) {
                        localStorageKey.rows[2].cells[3 + z].innerHTML = var_89[z]
                    }
                };
                if (i + 1 < var_45) {
                    trainingTable = localStorageKey.rows[i + 1];
                    var var_93 = var_89[19].split(",");
                    var filteredOptions = trainingTable.querySelector(".select-option");
                    var selectElement = trainingTable.querySelector("select");
                    for (var z = 1; z < 30; z++) {
                        filteredOptions[z].innerHTML = updateOrAddData(filteredOptions[z].innerHTML, parseInt(var_93[z - 1]));
                        filteredOptions[z].disabled = (parseInt(var_93[z - 1]) === 0 ? true : false);
                        filteredOptions[z].hidden = (parseInt(var_93[z - 1]) === 0 ? true : false)
                    }
                }
            };
            var var_74 = 0;
            var var_75 = 0;
            for (var i = 3; i < var_45; i++) {
                var_74 += parseInt(localStorageKey.rows[i].cells[20].innerHTML);
                var_75 += parseInt(localStorageKey.rows[i].cells[21].innerHTML)
            };
            localStorageKey.rows[2].cells[20].innerHTML = var_74;
            localStorageKey.rows[2].cells[21].innerHTML = var_75
        }
    };
    var_29.send("diemChuan=" + encodeURIComponent(abilityInput > 0 ? abilityInput : 0) + "&diemTrungBinhBaiTap=" + encodeURIComponent(var_80) + "&diemMucTieu=" + encodeURIComponent(var_81) + "&soKyNang=" + encodeURIComponent(var_85) + "&roles=" + encodeURIComponent(var_56))
}

function updateOrAddData(var_39, var_95) {
    const var_40 = var_39.split(" | ");
    if (var_40.length >= 4) {
        var_40[3] = var_95
    } else {
        var_40.push(var_95)
    };
    return var_40.join(" | ")
}

function fillSavedData(var_96) {
    var var_97 = var_96.selectedIndex;
    var var_98 = [];
    var var_99 = "";
    if (var_97 > 0) {
        var_98 = JSON.parse(var_96.value);
        var_99 = var_98.value.split(",")
    };
    var i = -1;
    var var_100 = document.querySelectorAll("#playerinfo-form input");
    var_100.forEach(function (x) {
        i += 1;
        if (var_99.length > 1) {
            x.value = var_99[i]
        } else {
            x.value = ""
        }
    });
    var var_102 = document.getElementById("abilityCalbt");
    var_102.disabled = true;
    var_102.style.color = "gray";
    if (var_96.selectedIndex === 0) {
        var localStorageKey = document.getElementById("training-planner");
        var var_45 = localStorageKey.rows.length;
        addRow();
        for (var i = 1; i < var_45 - 2; i++) {
            var trainingTable = localStorageKey.rows[var_45 - i];
            var var_47 = trainingTable.cells[22].querySelector("button");
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
    var var_103 = form.querySelectorAll("select");
    var_103.forEach(function (x) {
        x.selectedIndex = 0
    })
}

function showSamplePlan() {
    var var_56 = document.getElementById("rolestxt");
    var var_105 = var_56.value.split("+");
    var selectElement = document.getElementById("samplePlanner");
    selectElement.selectedIndex = 0;
    for (var i = 1; i < selectElement.options.length; i++) {
        selectElement.options[i].hidden = true
    };
    for (var i = 0; i < var_105.length; i++) {
        var var_106 = var_105[i];
        if (var_106.includes("L")) {
            var_106 = var_106.replace(/L/g, "L/R")
        } else {
            if (var_106.includes("R")) {
                var_106 = var_106.replace(/R/g, "L/R")
            }
        };
        for (var i = 1; i < selectElement.options.length; i++) {
            var var_108 = selectElement.options[i].text.split(":")[0];
            var var_109 = var_108.split("+");
            for (var i = 0; i < var_109.length; i++) {
                var var_110 = var_109[i];
                if (var_110.includes("L")) {
                    var_110 = var_110.replace(/L/g, "L/R")
                } else {
                    if (var_110.includes("R")) {
                        var_110 = var_110.replace(/R/g, "L/R")
                    }
                };
                if (var_106 === var_110) {
                    selectElement.options[i].hidden = false
                }
            }
        }
    }
}

function getPlayerPlan(var_111) {
    var var_98 = [];
    var_98 = JSON.parse(var_111);
    if (var_98) {
        return var_98.plan
    } else {
        return ""
    }
}

function containsOption(selectElement, var_31) {
    for (var i = 0; i < selectElement.options.length; i++) {
        if (selectElement.options[i].value === var_31) {
            return true
        }
    };
    return false
}

function loadSamplePlan() {
    const var_112 = JSON.parse(localStorage.getItem("samplePlanner"));
    var var_56 = document.getElementById("rolestxt");
    var var_40 = var_56.value.split("+");
    if (var_112 && Array.isArray(var_112)) {
        const selectElement = document.getElementById("samplePlanner");
        for (var i = 0; i < var_112.length; i++) {
            const var_68 = var_112[i].value;
            const var_113 = var_112[i].label;
            if (!containsOption(selectElement, var_68)) {
                const var_114 = document.createElement("option");
                var_114.value = var_68;
                var_114.text = var_113;
                var_114.hidden = true;
                selectElement.appendChild(var_114)
            }
        }
    }
}

function setWhitecolV2(var_115) {
    var localStorageKey = document.getElementById("training-planner");
    var filteredOptions = document.querySelectorAll("#training-planner .select-option option");
    const var_56 = document.getElementById("rolestxt").value;
    var var_116 = "";
    for (var i = 3; i < 18; i++) {
        var_116 += localStorageKey.rows[0].cells[i].innerHTML + (i < 17 ? "," : "")
    };
    const var_29 = new XMLHttpRequest();
    var_29.open("POST", "https://top11experience.com/wp-content/themes/top11trainplanner/pro-whitecol.php", true);
    var_29.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    var_29.onreadystatechange = function () {
        if (var_29.readyState === 4 && var_29.status === 200) {
            var var_62 = var_29.responseText;
            var var_118 = var_62.split("<br>")[0];
            var var_119 = 0;
            var var_120 = 0;
            var var_121 = "";
            var var_122 = "";
            for (var i = 0; i < localStorageKey.rows.length; i++) {
                var trainingTable = localStorageKey.rows[i];
                for (var i = 3; i < 18; i++) {
                    var var_92 = trainingTable.cells[i];
                    if (!var_118.includes(i < 10 ? "0" + i : "" + i)) {
                        var_92.classList.remove(i < 3 ? "whiteSkillThead" : "whiteSkillTbody");
                        var_92.classList.add(i < 3 ? "graySkillThead" : "graySkillTbody");
                        if (i === 1) {
                            var_119 += parseFloat(trainingTable.cells[i].innerHTML);
                            var_121 += i + ","
                        }
                    } else {
                        var_92.classList.remove(i < 3 ? "graySkillThead" : "graySkillTbody");
                        var_92.classList.add(i < 3 ? "whiteSkillThead" : "whiteSkillTbody");
                        if (i === 1 && var_118.includes(i < 10 ? "0" + i : "" + i)) {
                            var_120 += parseFloat(trainingTable.cells[i].innerHTML);
                            var_122 += i + ","
                        }
                    };
                    if (i === 17) {
                        localStorageKey.rows[1].cells[19].innerHTML = (var_120 * 100.0 / (var_119 + var_120)).toFixed(1);
                        localStorageKey.rows[2].cells[19].innerHTML = localStorageKey.rows[1].cells[19].innerText
                    }
                }
            };
            localStorage.setItem("top11experiencePlayerSkillCol", var_122.substring(0, var_122.length - 1) + ":" + var_121.substring(0, var_121.length - 1));
            for (var i = 1; i < 31; i++) {
                var var_123 = var_62.split("<br>")[i];
                if (var_123) {
                    if (filteredOptions[i].innerHTML.includes("|")) {
                        var var_124 = filteredOptions[i].innerHTML.split("|");
                        filteredOptions[i].innerHTML = var_124[0].trim() + " | " + var_123.split(":")[0]
                    } else {
                        filteredOptions[i].innerHTML += " | " + var_123.split(":")[0]
                    };
                    filteredOptions[i].title = var_123.split(":")[1];
                    if (parseInt(var_123.split("|")[1]) === 0) {
                        filteredOptions[i].hidden = true
                    } else {
                        filteredOptions[i].hidden = false
                    }
                }
            };
            var selectElement = document.getElementById("savedPlayers-select");
            if (selectElement.selectedIndex !== 0 && var_115 === 0) {
                fillPlan(getPlayerPlan(selectElement.value))
            };
            setDrillOptionsTitle()
        }
    };
    var_29.send("roles=" + encodeURIComponent(var_56) + "&skills=" + encodeURIComponent(var_116))
}

function setDrillOptionsTitle() {
    var localStorageKey = document.getElementById("training-planner");
    if (localStorageKey) {
        var var_125 = localStorageKey.rows[3];
        for (var i = 4; i < localStorageKey.rows.length; i++) {
            var trainingTable = localStorageKey.rows[i];
            if (trainingTable.cells.length > 1) {
                var var_92 = trainingTable.cells[1];
                if (var_92.children.length > 0 && var_92.children[0].tagName.toLowerCase() === "select") {
                    var var_126 = var_92.children[0];
                    var filteredOptions = var_126.getElementsByTagName("option");
                    for (var z = 0; z < filteredOptions.length;z++) {
                        var var_127 = var_125.cells[1].getElementsByTagName("select")[0].getElementsByTagName("option")[z];
                        filteredOptions[z].title = var_127.title
                    }
                }
            }
        }
    }
}
fullscreenButton.addEventListener("click", function () {
    event.preventDefault();
    var var_128 = document.querySelector(".training-page");
    var var_129 = fullscreenButton.innerHTML;
    if (var_129.toLowerCase() === "fullscreen") {
        fullscreenButton.innerHTML = "Exit Fullscreen";
        var_128.classList.add("fullscreen");
        if (document.documentElement.requestFullscreen) {
            document.documentElement.requestFullscreen()
        } else {
            if (document.documentElement.mozRequestFullScreen) {
                document.documentElement.mozRequestFullScreen()
            } else {
                if (document.documentElement.webkitRequestFullscreen) {
                    document.documentElement.webkitRequestFullscreen()
                } else {
                    if (document.documentElement.msRequestFullscreen) {
                        document.documentElement.msRequestFullscreen()
                    }
                }
            }
        }
    } else {
        fullscreenButton.innerHTML = "Fullscreen";
        var_128.classList.remove("fullscreen");
        if (document.exitFullscreen) {
            document.exitFullscreen()
        } else {
            if (document.mozCancelFullScreen) {
                document.mozCancelFullScreen()
            } else {
                if (document.webkitExitFullscreen) {
                    document.webkitExitFullscreen()
                } else {
                    if (document.msExitFullscreen) {
                        document.msExitFullscreen()
                    }
                }
            }
        }
    }
});

function displayTextV2() {
    event.preventDefault();
    langChange(selectedLanguage);
    var var_130 = [];
    var var_100 = document.querySelectorAll("#playerinfo-form input");
    var_100.forEach(function (x) {
        var_130.push(x.value)
    });
    localStorage.setItem("inputValue", var_130);
    var localStorageKey = document.getElementById("training-planner");
    var var_56 = document.querySelector("#rolestxt").value.split("+");
    var filteredOptions = document.querySelectorAll("#training-planner .select-option option");
    var var_131 = 0;
    var var_119 = 0;
    var var_120 = 0;
    for (var i = 1; i < 3; i++) {
        var trainingTable = localStorageKey.rows[i];
        for (var i = 1; i < 18; i++) {
            var var_92 = trainingTable.cells[i];
            if (i === 2 && i < 3) {
                continue
            };
            var_92.innerHTML = var_130[(i < 3) ? (i - 1) : i];
            if (i === 1 && i > 2) {
                var_131 += parseFloat(var_130[i])
            };
            if (i === 2) {
                var_92.style.color = "red";
                var_92.style.fontWeight = "bold"
            }
        }
    };
    localStorageKey.rows[1].cells[18].innerHTML = (var_131 / 15.0).toFixed(1);
    localStorageKey.rows[2].cells[18].innerHTML = (var_131 / 15.0).toFixed(1);
    setWhitecolV2(0);
    var var_132 = localStorageKey.rows[0].cells[19].innerHTML;
    var var_102 = document.getElementById("abilityCalbt");
    var_102.disabled = false;
    var_102.style.color = "blue"
}

function fillPlan(var_133) {
    table = document.getElementById("training-planner");
    var var_45 = table.rows.length;
    for (var i = 1; i < var_45 - 3; i++) {
        var trainingTable = table.rows[var_45 - i];
        var var_47 = trainingTable.cells[22].querySelector("button");
        deleteRow(var_47)
    };
    var_45 = table.rows.length - 3;
    if (var_133.length > 0) {
        var var_55 = var_133.split(",");
        for (var i = 0; i < var_55.length - var_45; i++) {
            addRow()
        };
        var i = 3;
        var_55.forEach(function (var_134) {
            var var_40 = var_134.split(":");
            var var_135 = parseInt(var_40[0]);
            var var_31 = var_40[1];
            var trainingTable = table.rows[i];
            var var_136 = trainingTable.cells[1].querySelector("select");
            var_136.selectedIndex = var_135;
            var var_137 = trainingTable.cells[2].querySelector("input");
            var_137.value = var_31;
            i++
        });
        var_45 = table.rows.length;
        if (i < var_45) {
            for (var i = 1; i < var_45 - i + 1; i++) {
                var trainingTable = table.rows[var_45 - i];
                var var_47 = trainingTable.cells[22].querySelector("button");
                deleteRow(var_47)
            }
        };
        var trainingTable = table.rows[3];
        var var_138 = trainingTable.cells[2].querySelector("input");
        callTrainingResult(var_138)
    }
}

function getPlanValue() {
    var localStorageKey = document.getElementById("training-planner");
    var var_139 = localStorageKey.rows[1].cells[2].innerHTML + ": " + parseInt(localStorageKey.rows[2].cells[18].innerHTML / 10) + "x %";
    var var_32 = [];
    for (var i = 3; i < localStorageKey.rows.length; i++) {
        var trainingTable = localStorageKey.rows[i];
        var var_140 = trainingTable.querySelector("select").selectedIndex;
        var var_141 = trainingTable.querySelector("input").value;
        if (var_140 > 0) {
            var_32.push(var_140 + ":" + var_141)
        }
    };
    var var_142 = var_32.join(",");
    return var_142
}

function savePlayerInfo() {
    event.preventDefault();
    var var_66 = localStorage.getItem("top11experiencePlayerData");
    var var_67 = [];
    if (var_66) {
        var_67 = JSON.parse(var_66)
    };
    var var_130 = [];
    var var_139 = document.getElementById("nametxt").value + " : " + document.getElementById("rolestxt").value;
    var var_100 = document.querySelectorAll("#playerinfo-form input");
    var_100.forEach(function (x) {
        var_130.push(x.value)
    });
    var var_142 = var_130.join(",");
    var var_52 = -1;
    for (var i = 0; i < var_67.length; i++) {
        if (var_67[i].label === var_139) {
            var_52 = i;
            break
        }
    };
    var var_133 = getPlanValue();
    if (var_52 !== -1) {
        var_67[var_52].value = var_142;
        var_67[var_52].plan = var_133;
        updateSelectedPlayer(JSON.stringify({
            value: var_142,
            plan: var_133
        }))
    } else {
        var var_156 = {
            label: var_139,
            value: var_142,
            plan: var_133
        };
        var_67.push(var_156);
        var selectElement = document.getElementById("savedPlayers-select");
        var x = document.createElement("option");
        x.text = var_139;
        var var_68 = {
            value: var_142,
            plan: var_133
        };
        x.value = JSON.stringify(var_68);
        selectElement.appendChild(x);
        selectElement.selectedIndex = var_67.length
    };
    var var_143 = {
        label: var_139,
        value: var_142,
        plan: var_133
    };
    savePlayerData(JSON.stringify(var_143));
    var var_65 = JSON.stringify(var_67);
    localStorage.setItem("top11experiencePlayerData", var_65);
    alert(lang[89] + " " + var_139 + "!")
}

function updateSelectedPlayer(var_144) {
    const selectElement = document.getElementById("savedPlayers-select");
    const var_145 = selectElement.options[selectElement.selectedIndex];
    var_145.value = var_144
}

function savePlayerData(var_31) {
    var var_29 = new XMLHttpRequest();
    var var_30 = "https://top11experience.com/wp-content/themes/top11trainplanner/pro-getPlayerList.php";
    var var_32 = "param=" + var_31.replace(/\+/g, "|");
    var_29.open("POST", var_30, true);
    var_29.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    var_29.onreadystatechange = function () {
        if (var_29.readyState === 4 && var_29.status === 200) {
            var var_33 = var_29.responseText
        }
    };
    var_29.send(var_32)
}

function deletePlayerInfo() {
    event.preventDefault();
    var var_130 = [];
    var var_139 = "Deleted " + document.getElementById("nametxt").value + " : " + document.getElementById("rolestxt").value;
    var var_100 = document.querySelectorAll("#playerinfo-form input");
    var_100.forEach(function (x) {
        var_130.push(x.value)
    });
    var var_142 = var_130.join(",");
    var var_133 = getPlanValue();
    var var_143 = {
        label: var_139,
        value: var_142,
        plan: var_133
    };
    savePlayerData(JSON.stringify(var_143));
    var var_66 = localStorage.getItem("top11experiencePlayerData");
    var var_67 = [];
    if (var_66) {
        var_67 = JSON.parse(var_66)
    };
    var selectElement = document.getElementById("savedPlayers-select");
    var var_146 = selectElement.selectedIndex;
    var var_139 = document.getElementById("nametxt").value;
    if (var_146 === 0) {
        alert(lang[107]);
        return
    };
    var var_147 = confirm(lang[93] + " " + var_139 + "?");
    if (!var_147) {
        return
    };
    var var_100 = document.querySelectorAll("#playerinfo-form input");
    var_100.forEach(function (x) {
        x.value = ""
    });
    var_67.splice(var_146 - 1, 1);
    var var_65 = JSON.stringify(var_67);
    localStorage.setItem("top11experiencePlayerData", var_65);
    selectElement.remove(var_146);
    alert(lang[90] + " " + var_139 + "!")
}

function duplicateRow(var_148) {
    var trainingTable = var_148.parentNode.parentNode;
    var localStorageKey = document.getElementById("training-planner");
    var var_45 = localStorageKey.rows.length;
    if (var_45 >= 33) {
        alert(lang[91])
    } else {
        var var_149 = trainingTable.cloneNode(true);
        var var_150 = trainingTable.querySelector(".select-option");
        var var_146 = var_150.selectedIndex;
        var selectElement = var_149.querySelector(".select-option");
        selectElement.selectedIndex = var_146;
        trainingTable.parentNode.insertBefore(var_149, trainingTable.nextSibling)
    }
}

function sendPlanToServer(var_133) {
    var var_29 = new XMLHttpRequest();
    var var_30 = "https://top11experience.com/wp-content/themes/top11trainplanner/pro-savePlan.php";
    var var_32 = "param1=" + var_133;
    var_29.open("POST", var_30, true);
    var_29.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    var_29.onreadystatechange = function () {
        if (var_29.readyState === 4 && var_29.status === 200) {
            var var_33 = var_29.responseText
        }
    };
    var_29.send(var_32)
}
btSaveSamplePlan.addEventListener("click", function () {
    event.preventDefault();
    var var_66 = localStorage.getItem("samplePlanner");
    var var_67 = [];
    if (var_66) {
        var_67 = JSON.parse(var_66)
    };
    var localStorageKey = document.getElementById("training-planner");
    var var_139 = localStorageKey.rows[1].cells[2].innerHTML + ": " + parseInt(localStorageKey.rows[2].cells[18].innerHTML / 10) + "x %";
    var var_32 = [];
    for (var i = 3; i < localStorageKey.rows.length; i++) {
        var trainingTable = localStorageKey.rows[i];
        var var_140 = trainingTable.querySelector("select").selectedIndex;
        var var_141 = trainingTable.querySelector("input").value;
        var_32.push(var_140 + ":" + var_141)
    };
    var var_142 = var_32.join(",");
    var var_52 = -1;
    for (var i = 0; i < var_67.length; i++) {
        if (var_67[i].label === var_139) {
            var_52 = i;
            break
        }
    };
    var var_151 = "";
    var var_152 = "";
    var var_153 = false;
    if (var_52 !== -1) {
        var_151 = lang[96];
        const var_154 = prompt(lang[103].replace("[planLabelText]", var_139), var_139);
        if (var_154 === null) {
            return
        } else {
            if (var_154.trim() !== var_139) {
                var_139 = var_154.trim();
                var_52 = -1
            }
        };
        var_153 = true
    } else {
        const var_154 = prompt(lang[97] + " " + var_139 + "!", var_139);
        if (var_154 === null) {
            return
        } else {
            var_139 = var_154.trim();
            var_153 = true
        }
    };
    if (var_153) {
        if (var_52 !== -1) {
            var_67[var_52].value = var_142;
            var var_155 = {
                label: var_139,
                value: var_142
            };
            sendPlanToServer(JSON.stringify(var_155));
            alert(lang[95] + " " + var_139 + "!")
        } else {
            var var_156 = {
                label: var_139,
                value: var_142
            };
            sendPlanToServer(JSON.stringify(var_156));
            var_67.push(var_156);
            alert(lang[94] + " " + var_139 + "!")
        };
        var var_65 = JSON.stringify(var_67);
        localStorage.setItem("samplePlanner", var_65)
    };
    var selectElement = document.getElementById("samplePlanner");
    const var_114 = document.createElement("option");
    var_114.value = var_142;
    var_114.text = var_139;
    selectElement.appendChild(var_114)
});

function abilityForm() {
    event.preventDefault();
    var var_157 = document.querySelector(".overlay");
    var_157.style.display = "flex"
}

function closeForm() {
    var var_157 = document.querySelector(".overlay");
    var_157.style.display = "none";
    var var_55 = form.querySelectorAll("input");
    for (var i = 0; i < var_55.length - 1; i++) {
        var_55[i].value = ""
    }
}
form.addEventListener("submit", function (keyboardEvent) {
    keyboardEvent.preventDefault();
    var var_158 = getSelectedIndex(document.getElementById("talentSelect"));
    var var_159 = document.getElementById("ageSelect").value;
    var var_32 = "talentIndex=" + var_158 + "&ageIndex=" + var_159;
    var var_29 = new XMLHttpRequest();
    var_29.onreadystatechange = function () {
        if (var_29.readyState === XMLHttpRequest.DONE) {
            if (var_29.status === 200) {
                abilityInput.value = var_29.responseText
            } else {
                console.error("\u0110\xE3 x\u1ea3y ra l\u1ed7i khi g\u1eedi y\xEAu c\u1ea7u t\xEDnh to\xE1n \u0111\u1ebfn server.")
            }
        }
    };
    var_29.open("POST", "https://top11experience.com/wp-content/themes/top11trainplanner/abilityCalcu_new.php", true);
    var_29.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    var_29.send(var_32);
    closeForm()
});
var inputElement = document.getElementById("rolestxt");
inputElement.addEventListener("input", function () {
    inputElement.value = inputElement.value.toUpperCase()
});
document.getElementById("abilityCalbt").addEventListener("click", function () {
    event.preventDefault();
    if (loadAbilityForm === false) {
        var var_29 = new XMLHttpRequest();
        var_29.onreadystatechange = function () {
            if (var_29.readyState === XMLHttpRequest.DONE) {
                if (var_29.status === 200) {
                    document.getElementById("myForm").innerHTML = var_29.responseText;
                    loadAbilityForm = true;
                    abilityForm()
                } else {
                    console.error("\u0110\xE3 x\u1ea3y ra l\u1ed7i khi t\u1ea3i m\xE3 HTML t\u1eeb file PHP.")
                }
            }
        };
        var var_30 = "https://top11experience.com/wp-content/themes/top11trainplanner/abilityForm.php";
        var_30 += "?lang=" + selectedLanguage;
        var_29.open("GET", var_30, true);
        var_29.send()
    } else {
        abilityForm()
    }
});

function getSelectedIndex(selectElement) {
    return selectElement.selectedIndex
}