var name_Variable = ["myForm", "getElementById", "abilitytxt", "samplePlanner", "languageSelect", "getItem", "btFullscreen", "saveSamplePlan", "selectOptions", "parse", "value", "text", "map", "options", "from", "some", "filter", "top11SamplePlanOptions", "stringify", "setItem", "remove", "length", "#training-planner tbody tr", "querySelectorAll", "mouseover", "backgroundColor", "style", "lightgreen", "addEventListener", "mouseout", "", "forEach", "training-planner", "keydown", "key", "Tab", "Enter", "preventDefault", "target", "parentElement", "indexOf", "rows", "input", "querySelector", "cellIndex", "cells", "focus", "select", "bangSoSanh", "td", "getElementsByTagName", "removeAttribute", "soSanhContainer", "display", "flex", "none", "#bangSoSanh .PA1", "#bangSoSanh .PA2", "#bangSoSanh .PA3", "#bangSoSanh .xoaSoSanh", "rowXoaButton", "hidden", "innerHTML", "graySkillThead", "contains", "classList", "gray", "https://top11experience.com/wp-content/themes/top11trainplanner/pro-getPlayerList.php", "savedPlayers-select", "param1=", "POST", "open", "Content-type", "application/x-www-form-urlencoded", "setRequestHeader", "onreadystatechange", "readyState", "status", "responseText", "send", "GET", "toLowerCase", "userAgent", "android", "iphone", "ipad", "ipod", "#training-planner .select-option option", "includes", "title", " - ", "cyan", "white", "index", "color", "black", "red", "split", "join", "slice", "resultTable", ".close-button", "block", "defTable", "graySkillTbody", "toFixed", "%", "attackTable", "physicalTable", "summary2", "summary3", "onchange", "selectedIndex", "button", "pro-langDic", "langCode", "langDic", ".langChange", "+", "#rolestxt", "GK", "language-select", "trim", ":", ".training-page .select-option option", "|", " | ", "https://top11experience.com/wp-content/themes/top11trainplanner/pro-langDic.php", "langCode=", "push", "top11experiencePlayerData", "label", "option", "createElement", "plan", "appendChild", "beforeunload", "B\u1ea1n c\xF3 ch\u1eafc mu\u1ed1n r\u1eddi kh\u1ecfi trang?", "returnValue", "DOMContentLoaded", "cloneNode", "nextSibling", "insertBefore", "parentNode", "removeChild", "Tr\xECnh duy\u1ec7t h\u1ed7 tr\u1ee3 thu\u1ed9c t\xEDnh hidden c\u1ee7a c\xE1c ph\u1ea7n t\u1eed option trong select.", "Tr\xECnh duy\u1ec7t kh\xF4ng h\u1ed7 tr\u1ee3 thu\u1ed9c t\xEDnh hidden c\u1ee7a c\xE1c ph\u1ea7n t\u1eed option trong select.", "add", "rowIndex", ",", ".select-option", "top11experiencePlayerSkillCol", "https://top11experience.com/wp-content/themes/top11trainplanner/pro-trainingPlanCalcu.php", "Content-Type", "<br>", "lightsalmon", "fontWeight", "normal", "blue", "bold", "disabled", "diemChuan=", "&diemTrungBinhBaiTap=", "&diemMucTieu=", "&soKyNang=", "&roles=", "#playerinfo-form input", "abilityCalbt", "rolestxt", "L", "L/R", "replace", "R", "isArray", "https://top11experience.com/wp-content/themes/top11trainplanner/pro-whitecol.php", "0", "whiteSkillThead", "whiteSkillTbody", "innerText", "substring", "roles=", "&skills=", "children", "tagName", "click", ".training-page", "fullscreen", "Exit Fullscreen", "requestFullscreen", "documentElement", "mozRequestFullScreen", "webkitRequestFullscreen", "msRequestFullscreen", "Fullscreen", "exitFullscreen", "mozCancelFullScreen", "webkitExitFullscreen", "msExitFullscreen", "inputValue", ": ", "x %", "nametxt", " : ", " ", "!", "param=", "Deleted ", "?", "splice", "https://top11experience.com/wp-content/themes/top11trainplanner/pro-savePlan.php", "[planLabelText]", ".overlay", "submit", "talentSelect", "ageSelect", "talentIndex=", "&ageIndex=", "DONE", "\u0110\xE3 x\u1ea3y ra l\u1ed7i khi g\u1eedi y\xEAu c\u1ea7u t\xEDnh to\xE1n \u0111\u1ebfn server.", "error", "https://top11experience.com/wp-content/themes/top11trainplanner/abilityCalcu_new.php", "toUpperCase", "\u0110\xE3 x\u1ea3y ra l\u1ed7i khi t\u1ea3i m\xE3 HTML t\u1eeb file PHP.", "https://top11experience.com/wp-content/themes/top11trainplanner/abilityForm.php", "?lang="];
var lang = [];
const langlengthPro = 108;
var form = document[name_Variable[1]](name_Variable[0]);
var abilityInput = document[name_Variable[1]](name_Variable[2]);
var selectElement = document[name_Variable[1]](name_Variable[3]);
var selectedLanguage = localStorage[name_Variable[5]](name_Variable[4]);
var fullscreenButton = document[name_Variable[1]](name_Variable[6]);
var btSaveSamplePlan = document[name_Variable[1]](name_Variable[7]);
var loadAbilityForm = false;

function saveOptionsToLocalStorage() {
    const selectElement = document.getElementById("samplePlanner"); // Thay `name_Variable[1]` bằng ID thực tế
    const savedOptions = JSON.parse(localStorage.getItem("selectOptions")) || [];
    const currentOptions = Array.from(selectElement.options).map((option) => {
        return {
            value: option.value,
            text: option.text
        };
    });
    const newOptions = currentOptions.filter((option) => {
        return !savedOptions.some((savedOption) => {
            return savedOption.value === option.value;
        });
    });
    const allOptions = [...savedOptions, ...newOptions];
    localStorage.setItem("selectOptions", JSON.stringify(allOptions));
    
    while (selectElement.options.length > 0) {
        selectElement.options.remove(0); // Xóa tất cả các option hiện có
    }
}
const rows = document.querySelectorAll("#training-planner tbody tr"); // Lấy tất cả các hàng từ bảng
rows.forEach((row) => {
    row.addEventListener("mouseover", () => {
        row.style.backgroundColor = "lightgreen"; // Đổi màu nền khi chuột di chuyển qua hàng
    });
    row.addEventListener("mouseout", () => {
        row.style.backgroundColor = ""; // Khôi phục màu nền khi chuột rời khỏi hàng
    });
});

const trainingTable = document.getElementById("training-planner"); // Lấy bảng huấn luyện
trainingTable.addEventListener("keydown", (event) => {
    if (event.key === "Tab" || event.key === "Enter") {
        event.preventDefault(); // Ngăn chặn hành vi mặc định
        const targetRow = event.target; // Lấy hàng mục tiêu
        const targetCell = targetRow.cellIndex; // Lấy chỉ số ô của hàng đó
        const targetCells = targetRow.cells; // Lấy tất cả các ô trong hàng

        // Tìm ô kế tiếp
        const nextCellIndex = Array.from(targetCells).indexOf(targetRow.cells[targetCell]) + 1;
        const nextCell = targetCells[nextCellIndex];

        if (nextCell) {
            nextCell.focus(); // Chuyển sự chú ý đến ô tiếp theo
            nextCell.select(); // Chọn ô tiếp theo
        }
    }
});

function removeSoSanhCellStyle() {
    const comparisonTable = document.getElementById("comparison-table"); // ID của bảng so sánh
    const rows = comparisonTable.getElementsByTagName("tr");
    for (let i = 0; i < 60; i++) {
        rows[i].classList.remove("highlight"); // Xóa lớp "highlight" khỏi từng hàng
    }
}

function xemSoSanh() {
    var comparisonSection = document.getElementById("comparison-section"); // ID của phần so sánh
    comparisonSection.style.display = "block"; // Hiển thị phần so sánh
}

function closeSoSanhTable() {
    var comparisonSection = document.getElementById("comparison-section"); // ID của phần so sánh
    comparisonSection.style.display = "none"; // Ẩn phần so sánh
}

function addSoSanh() {
    var plannerTable = document.getElementById("training-planner"); // ID của bảng huấn luyện
    var trainingDataRow = plannerTable.rows[2]; // Dữ liệu hàng thứ ba
    var option1 = document.querySelector("#option1"); // Chọn option1 (ID thực tế)
    var option2 = document.querySelector("#option2"); // Chọn option2 (ID thực tế)
    var option3 = document.querySelector("#option3"); // Chọn option3 (ID thực tế)
    var comparisonTable = document.getElementById("comparison-table"); // Bảng so sánh
    var checkboxList = document.querySelectorAll(".checkbox"); // Danh sách các checkbox
    var resultSection = document.getElementById("result-section"); // ID của kết quả
    var isPreview = false; // Biến kiểm tra chế độ xem trước
    var selectedIndex = -1;
    var selectedOption = option1; // Mặc định chọn option1

    for (var i = 0; i < checkboxList.length; i++) {
        if (checkboxList[i].checked === true) {
            selectedIndex = i;
            checkboxList[i].checked = false; // Bỏ chọn checkbox
            break;
        }
    }

    // Xác định tùy chọn được chọn dựa trên chỉ số đã chọn
    if (selectedIndex === 0) {
        selectedOption = option1;
    } else if (selectedIndex === 1) {
        selectedOption = option2;
    } else if (selectedIndex === 2) {
        selectedOption = option3;
    } else {
        alert("Không có tùy chọn nào được chọn."); // Thông báo lỗi.
    }

    for (var i = 0; i < selectedOption.length; i++) {
        var trainingCell = trainingDataRow.cells[i + 3]; // Lấy ô của hàng dữ liệu huấn luyện
        trainingCell.innerHTML = selectedOption[i].innerHTML; // Cập nhật dữ liệu
        if (selectedOption[i].classList.contains("highlight")) {
            for (var j = 0; j < 4; j++) {
                trainingCell.children[j].classList.add("highlight"); // Thêm lớp highlight cho ô
            }
        } else {
            for (var j = 0; j < 4; j++) {
                if (trainingCell.children[j].classList.contains("highlight")) {
                    trainingCell.children[j].classList.remove("highlight"); // Xóa lớp highlight
                }
            }
        }
    }
}

function xoaPA1() {
    var checkboxGroup1 = document.querySelectorAll("#group1 input[type='checkbox']"); // Lấy nhóm checkbox 1
    for (var i = 0; i < checkboxGroup1.length; i++) {
        checkboxGroup1[i].checked = false; // Bỏ chọn tất cả checkbox trong nhóm 1
    }
    var checkboxGroupStatus = document.querySelectorAll("#statusGroup input[type='checkbox']"); // Lấy nhóm trạng thái checkbox
    checkboxGroupStatus[0].checked = true; // Đánh dấu checkbox đầu tiên
    if (checkboxGroupStatus[0].checked && checkboxGroupStatus[1].checked && checkboxGroupStatus[2].checked) {
        var submitButton = document.getElementById("submit-button"); // Lấy nút gửi
        submitButton.disabled = true; // Vô hiệu hóa nút gửi
    }
}

function xoaPA2() {
    var checkboxGroup2 = document.querySelectorAll("#group2 input[type='checkbox']"); // Lấy nhóm checkbox 2
    for (var i = 0; i < checkboxGroup2.length; i++) {
        checkboxGroup2[i].checked = false; // Bỏ chọn tất cả checkbox trong nhóm 2
    }
    var checkboxGroupStatus = document.querySelectorAll("#statusGroup input[type='checkbox']"); // Lấy nhóm trạng thái checkbox
    checkboxGroupStatus[1].checked = true; // Đánh dấu checkbox thứ hai
    if (checkboxGroupStatus[0].checked && checkboxGroupStatus[1].checked && checkboxGroupStatus[2].checked) {
        var submitButton = document.getElementById("submit-button"); // Lấy nút gửi
        submitButton.disabled = true; // Vô hiệu hóa nút gửi
    }
}

function xoaPA3() {
    var checkboxGroup3 = document.querySelectorAll("#group3 input[type='checkbox']"); // Lấy nhóm checkbox 3
    for (var i = 0; i < checkboxGroup3.length; i++) {
        checkboxGroup3[i].checked = false; // Bỏ chọn tất cả checkbox trong nhóm 3
    }
    var checkboxGroupStatus = document.querySelectorAll("#statusGroup input[type='checkbox']"); // Lấy nhóm trạng thái checkbox
    checkboxGroupStatus[2].checked = true; // Đánh dấu checkbox thứ ba
    if (checkboxGroupStatus[0].checked && checkboxGroupStatus[1].checked && checkboxGroupStatus[2].checked) {
        var submitButton = document.getElementById("submit-button"); // Lấy nút gửi
        submitButton.disabled = true; // Vô hiệu hóa nút gửi
    }
}

function sendData() {
    var xhr = new XMLHttpRequest();
    var url = name_Variable[67]; // Địa chỉ URL sẽ gửi dữ liệu tới
    var selectElement = document.getElementById(name_Variable[68]); // Lấy phần tử select
    var selectedValue = selectElement.value; // Giá trị đã chọn từ select
    var requestData = name_Variable[69] + selectedValue; // Tạo dữ liệu gửi đi

    // Thiết lập yêu cầu
    xhr.open(name_Variable[70], url, true);
    xhr.setRequestHeader(name_Variable[72], name_Variable[73]); // Thiết lập header

    // Xử lý phản hồi từ server
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
            var responseData = xhr.responseText; // Lấy dữ liệu từ phản hồi
        }
    };

    xhr.send(requestData); // Gửi dữ liệu
}

function loadData() {
    var xhr = new XMLHttpRequest();
    xhr.open(name_Variable[80], name_Variable[67], true); // Thiết lập yêu cầu tải dữ liệu
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
            var responseData = JSON.parse(xhr.responseText); // Phân tích dữ liệu JSON
        }
    };
    xhr.send(); // Gửi yêu cầu
}

function isMobileDevice() {
    var userAgent = navigator.userAgent; // Lấy thông tin User-Agent
    // Kiểm tra nếu nó là thiết bị di động
    if (userAgent.includes(name_Variable[83]) ||
        userAgent.includes(name_Variable[84]) ||
        userAgent.includes(name_Variable[85]) ||
        userAgent.includes(name_Variable[86])) {
        return true; // Nếu có, trả về true
    } else {
        return false; // Nếu không, trả về false
    }
}

function findSkill(selectedSkill) {
    var skillList = document.getElementsByClassName(name_Variable[87]); // Lấy danh sách kỹ năng
    var resultSection = document.getElementById(name_Variable[32]); // Lấy phần kết quả
    var skillName = selectedSkill[name_Variable[62]]; // Tên kỹ năng được chọn

    for (var i = 0; i < skillList.length; i++) {
        if (skillList[i].textContent.includes(skillName)) { // Kiểm tra nếu tên kỹ năng có trong danh sách
            skillList[i][name_Variable[62]] = removeTextBeforeDash(skillList[i][name_Variable[62]]);
            skillList[i][name_Variable[62]] = skillName + name_Variable[90] + skillList[i][name_Variable[62]];
            skillList[i][name_Variable[26]].style.color = name_Variable[91]; // Thay đổi màu
        } else {
            skillList[i][name_Variable[62]] = removeTextBeforeDash(skillList[i][name_Variable[62]]);
            skillList[i][name_Variable[26]].style.color = name_Variable[92]; // Thay đổi màu khác
        }

        if (i < 18 && i != selectedSkill[name_Variable[93]]) {
            resultSection[name_Variable[41]][0][name_Variable[45]][i][name_Variable[26]].style.fontWeight = name_Variable[95];
        }
        selectedSkill[name_Variable[26]].style.fontWeight = name_Variable[96]; // Thay đổi trọng số của kỹ năng đã chọn
    }
}

function removeTextBeforeDash(inputString) {
    const parts = inputString.split(name_Variable[90]); // Tách chuỗi tại ký tự '-'
    if (parts.length > 1) {
        return parts.slice(1).join(name_Variable[90]); // Trả về chuỗi sau dấu '-'
    }
    return inputString; // Nếu không có dấu '-', trả về chuỗi ban đầu
}

function showTable() {
    var tableData = document.getElementById("data-table");
    tableData.style.display = "block";
    document.getElementById("additional-info").style.display = "block";

    var summaryData = document.getElementById("summary");
    var langData = document.getElementById("lang-data");

    langData.rows[0].cells[0].innerText = lang[10];
    langData.rows[1].cells[0].innerText = lang[101];
    langData.rows[1].cells[1].innerText = lang[99];
    langData.rows[1].cells[2].innerText = lang[100];

    var totalA = 0;
    var totalB = 0;

    // Process first section of data
    for (var i = 2; i < langData.rows.length; i++) {
        if (i < 7) {
            for (var j = 0; j < 3; j++) {
                langData.rows[i].cells[j].innerText = summaryData.rows[j].cells[i + 1].innerText;
                langData.rows[i].cells[j].className = summaryData.rows[3].cells[i + 1].classList.contains("highlight") ? "highlight" : "";
            }
            totalA += parseInt(summaryData.rows[1].cells[i + 1].innerText);
            totalB += parseInt(summaryData.rows[2].cells[i + 1].innerText);
        } else {
            langData.rows[i].cells[0].innerText = lang[98];
            langData.rows[i].cells[1].innerText = (totalA / 5).toFixed(1) + lang[106];
            langData.rows[i].cells[2].innerText = (totalB / 5).toFixed(1) + lang[106];
        }
    }

    // Process second section of data
    langData = document.getElementById("section-2");
    langData.rows[0].cells[0].innerText = lang[16];
    langData.rows[1].cells[0].innerText = lang[101];
    langData.rows[1].cells[1].innerText = lang[99];
    langData.rows[1].cells[2].innerText = lang[100];

    totalA = 0;
    totalB = 0;

    for (var i = 2; i < langData.rows.length; i++) {
        if (i < 7) {
            for (var j = 0; j < 3; j++) {
                langData.rows[i].cells[j].innerText = summaryData.rows[j].cells[i + 6].innerText;
                langData.rows[i].cells[j].className = summaryData.rows[3].cells[i + 6].classList.contains("highlight") ? "highlight" : "";
            }
            totalA += parseInt(summaryData.rows[1].cells[i + 6].innerText);
            totalB += parseInt(summaryData.rows[2].cells[i + 6].innerText);
        } else {
            langData.rows[i].cells[0].innerText = lang[98];
            langData.rows[i].cells[1].innerText = (totalA / 5).toFixed(1) + lang[106];
            langData.rows[i].cells[2].innerText = (totalB / 5).toFixed(1) + lang[106];
        }
    }

    // Process third section of data
    langData = document.getElementById("section-3");
    langData.rows[0].cells[0].innerText = lang[22];
    langData.rows[1].cells[0].innerText = lang[101];
    langData.rows[1].cells[1].innerText = lang[99];
    langData.rows[1].cells[2].innerText = lang[100];

    totalA = 0;
    totalB = 0;

    for (var i = 2; i < langData.rows.length; i++) {
        if (i < 7) {
            for (var j = 0; j < 3; j++) {
                langData.rows[i].cells[j].innerText = summaryData.rows[j].cells[i + 11].innerText;
                langData.rows[i].cells[j].className = summaryData.rows[3].cells[i + 11].classList.contains("highlight") ? "highlight" : "";
            }
            totalA += parseInt(summaryData.rows[1].cells[i + 11].innerText);
            totalB += parseInt(summaryData.rows[2].cells[i + 11].innerText);
        } else {
            langData.rows[i].cells[0].innerText = lang[98];
            langData.rows[i].cells[1].innerText = (totalA / 5).toFixed(1) + lang[106];
            langData.rows[i].cells[2].innerText = (totalB / 5).toFixed(1) + lang[106];
        }
    }

    // Process the final section of data
    var finalSection = document.getElementById("final-section");
    finalSection.rows[0].cells[0].innerText = summaryData.rows[0].cells[18].innerText;
    finalSection.rows[0].cells[1].innerText = summaryData.rows[0].cells[19].innerText;
    finalSection.rows[1].cells[0].innerText = lang[99];
    finalSection.rows[1].cells[1].innerText = lang[100];

    finalSection.rows[2].cells[0].innerText = summaryData.rows[1].cells[18].innerText;
    finalSection.rows[2].cells[1].innerText = summaryData.rows[2].cells[18].innerText;
    finalSection.rows[2].cells[2].innerText = summaryData.rows[1].cells[19].innerText;
    finalSection.rows[2].cells[3].innerText = summaryData.rows[2].cells[19].innerText;

    var anotherSection = document.getElementById("another-section");
    anotherSection.rows[0].cells[0].innerText = summaryData.rows[0].cells[20].innerText;
    anotherSection.rows[0].cells[1].innerText = summaryData.rows[0].cells[21].innerText;
    anotherSection.rows[1].cells[0].innerText = summaryData.rows[2].cells[20].innerText;
    anotherSection.rows[1].cells[1].innerText = summaryData.rows[2].cells[21].innerText;
}

function closeTable() {
    var tableElement = document.getElementById("table-id");  // Thay "table-id" bằng ID của bảng thực tế
    tableElement.style.display = "none";  // Ẩn bảng
    document.getElementById("additional-info").style.display = "none";  // Ẩn thông tin bổ sung
}

// Sự kiện khi người dùng chọn một mục từ selectElement
selectElement.onchange = function () {
    var planTable = document.getElementById("training-planner");  // Thay "training-planner" bằng ID thực tế của bảng
    var numRows = planTable.rows.length;  // Lấy số hàng hiện tại trong bảng
    addRow();  // Thêm một hàng mới vào bảng

    if (selectElement.value === "0") {  // Kiểm tra nếu giá trị được chọn là 0
        for (var i = 1; i < numRows - 2; i++) {  // Lặp qua các hàng trong bảng
            var rowToDelete = planTable.rows[numRows - i];  // Lấy hàng hiện tại cần xóa
            var identifier = rowToDelete.cells[22].textContent;  // Lấy giá trị xác định từ cột 22
            deleteRow(identifier);  // Gọi hàm xóa hàng với giá trị xác định
        }
        return;  // Dừng xử lý thêm nếu giá trị chọn là 0
    }

    var selectedValue = selectElement.value;  // Lấy giá trị đã chọn
    fillPlan(selectedValue);  // Gọi hàm để làm đầy kế hoạch với giá trị đã chọn
};

function langChange(selectedLang) {
    loadAbilityForm = false;  // Đặt biến trạng thái
    selectedLanguage = selectedLang;  // Cập nhật ngôn ngữ đã chọn

    // Lưu ngôn ngữ đã chọn vào localStorage
    localStorage.setItem("selectedLanguageKey", selectedLang);
    var storedLangData = localStorage.getItem("langDataKey");  // Lấy dữ liệu ngôn ngữ từ localStorage
    var langList = [];

    // Nếu có dữ liệu ngôn ngữ, chuyển đổi thành mảng
    if (storedLangData) {
        langList = JSON.parse(storedLangData);
    }

    var langIndex = -1;  // Biến để tìm chỉ số ngôn ngữ
    if (langList) {
        // Tìm ngôn ngữ đã chọn trong danh sách
        for (var i = 0; i < langList.length; i++) {
            if (langList[i].langCode === selectedLang) {
                langIndex = i;  // Lưu chỉ số nếu tìm thấy ngôn ngữ
                break;
            }
        }
        // Nếu tìm thấy ngôn ngữ, gọi hàm để điền thông tin ngôn ngữ
        if (langIndex !== -1) {
            var langData = langList[langIndex].langData;
            langFill(langData);
        } else {
            getLangDic(selectedLang);  // Nếu không tìm thấy, gọi hàm để lấy từ điển ngôn ngữ
        }
    }
}

function langFill(langData) {
    var elementsToFill = document.querySelectorAll("[data-lang-key]");  // Lấy các phần tử cần điền văn bản ngôn ngữ
    var shouldUseAlternate = document.getElementById("someElementId").classList.contains("someClass");  // Kiểm tra điều kiện nào đó
    var lang = JSON.parse(langData).contents;  // Giải mã dữ liệu ngôn ngữ

    if (lang.length < expectedLangLength) {
        var selectElement = document.getElementById("languageSelector");
        getLangDic(selectElement.value);  // Nếu số lượng ngôn ngữ nhỏ hơn kỳ vọng, lấy từ điển ngôn ngữ
    }

    // Điền văn bản cho các phần tử
    for (var i = 0; i < elementsToFill.length; i++) {
        if (elementsToFill[i].getAttribute("data-lang-key") !== "default") {
            elementsToFill[i].innerText = lang[i] ? lang[i] : lang;  // Điền ngôn ngữ tương ứng
        }
    }

    // Cập nhật các phần tử khác
    var additionalLangData = lang.slice(59, 90);
    var additionalElements = document.querySelectorAll("[data-additional-lang]");

    for (var i = 0; i < additionalElements.length; i++) {
        if (additionalElements[i].getAttribute("data-additional-lang")) {
            var elementKey = additionalElements[i].getAttribute("data-additional-lang");
            additionalElements[i].innerText = additionalLangData[elementKey];
        } else {
            additionalElements[i].innerText = additionalLangData[i];
        }
    }

    // Cập nhật màu nền
    setWhitecolV2(1);

    // Điền thông tin ngôn ngữ vào một bảng cụ thể
    var someTable = document.getElementById("tableId");  // ID của bảng thực tế
    someTable.rows[0].cells[0].innerText = lang[101];  // Cập nhật ô đầu tiên

    var dataSource = document.getElementById("dataSourceId");  // Lấy nguồn dữ liệu từ ID thực tế
    for (var j = 1; j < 20; j++) {
        someTable.rows[(j < 16 ? j : j + 1)].cells[0].innerText = dataSource.rows[0].cells[j + 2].innerText;  // Cập nhật thông tin cho bảng
    }
}

function getLangDic(languageCode) {
    var xhr = new XMLHttpRequest();  // Khởi tạo một đối tượng XMLHttpRequest
    var langEndpoint = "http://example.com/lang";  // Địa chỉ của API
    var requestUrl = langEndpoint + encodeURIComponent(languageCode);  // Tạo URL yêu cầu với mã ngôn ngữ

    xhr.open("GET", requestUrl, true);  // Mở yêu cầu
    xhr.setRequestHeader("Content-Type", "application/json");  // Đặt tiêu đề yêu cầu

    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {  // Kiểm tra trạng thái hoàn thành và mã phản hồi
            var responseData = xhr.responseText;  // Lấy dữ liệu phản hồi
            var langData = JSON.parse(responseData);  // Chuyển đổi dữ liệu JSON thành đối tượng
            saveLocalLangDic(responseData, languageCode);  // Lưu từ điển ngôn ngữ vào localStorage
            langFill(JSON.parse(langData));  // Gọi hàm để điền dữ liệu ngôn ngữ
        }
    };
    xhr.send();  // Gửi yêu cầu
}

function saveLocalLangDic(langData, languageCode) {
    var storedLangData = localStorage.getItem("langDataKey");  // Lấy dữ liệu ngôn ngữ đã lưu
    var langList = [];

    if (storedLangData) {
        langList = JSON.parse(storedLangData);  // Giải mã dữ liệu JSON
    }

    var langIndex = -1;  // Biến để theo dõi chỉ số ngôn ngữ
    for (var i = 0; i < langList.length; i++) {
        if (langList[i].langCode === languageCode) {
            langIndex = i;  // Tìm chỉ số của ngôn ngữ đã lưu
            break;
        }
    }

    // Cập nhật hoặc thêm mới từ điển ngôn ngữ
    if (langIndex !== -1) {
        langList[langIndex].langDic = langData;  // Cập nhật từ điển ngôn ngữ
    } else {
        var newLangEntry = {
            langCode: languageCode,
            langDic: langData
        };
        langList.push(newLangEntry);  // Thêm mới từ điển ngôn ngữ
    }

    var updatedLangData = JSON.stringify(langList);  // Chuyển đổi dữ liệu ngôn ngữ thành JSON
    localStorage.setItem("langDataKey", updatedLangData);  // Lưu lại dữ liệu trong localStorage
}

function loadSavedPlayerInfo() {
    var savedPlayerData = localStorage.getItem("playerDataKey");  // Lấy thông tin người chơi đã lưu
    var playerList = JSON.parse(savedPlayerData);  // Giải mã dữ liệu JSON

    if (playerList) {
        var selectElement = document.getElementById("playerSelect");  // Lấy phần tử chọn người chơi
        for (var i = 0; i < playerList.length; i++) {
            if (!selectElement.options.some(option => option.value === playerList[i].playerId)) {  // Kiểm tra xem người chơi đã có trong danh sách chưa
                var newOption = document.createElement("option");  // Tạo một tùy chọn mới
                newOption.value = playerList[i].playerId;  // Gán ID người chơi là giá trị
                newOption.text = playerList[i].playerName;  // Gán tên người chơi là văn bản hiển thị
                newOption.dataset.playerData = JSON.stringify({  // Gán dữ liệu người chơi vào tùy chọn
                    value: playerList[i].value,
                    plan: playerList[i].plan
                });
                selectElement.add(newOption);  // Thêm tùy chọn mới vào danh sách
            }
        }
    }
}
// Khi cửa sổ được tải, thực hiện hàm
window.addEventListener("load", function (event) {
    var selectElement = document.getElementById("playerSelect");  // Lấy phần tử chọn người chơi
    if (selectElement.options.length > 0) {  // Nếu trong danh sách có phần tử
        var firstOption = "firstOption";  // Giá trị của tùy chọn đầu tiên
        event.target.value = firstOption;  // Đặt giá trị của sự kiện là tùy chọn đầu tiên
        return firstOption;
        event.target.focus();  // Lấy lại tiêu điểm cho phần tử
    }
});

// Khi tài liệu sẵn sàng, thực hiện hàm
document.addEventListener("DOMContentLoaded", function () {
    if (localStorage.getItem("selectedLang")) {  // Kiểm tra nếu có ngôn ngữ đã chọn trong localStorage
        var selectElement = document.getElementById("languageSelector");  // Lấy phần tử chọn ngôn ngữ
        selectElement.value = selectedLanguage;  // Cập nhật giá trị cho phần tử chọn
        langChange(selectedLanguage);  // Thay đổi ngôn ngữ
    }
    loadSavedPlayerInfo();  // Tải thông tin người chơi đã lưu
    loadSamplePlan();  // Tải kế hoạch mẫu
});

// Hàm thêm hàng mới vào bảng
function addRow() {
    var tableElement = document.getElementById("dataTable");  // Lấy phần tử bảng
    var rowCount = tableElement.rows.length;  // Đếm số hàng trong bảng

    if (rowCount >= 33) {  // Nếu số hàng lớn hơn hoặc bằng 33
        alert(lang[91]);  // Thông báo cho người dùng
    } else {
        var lastRow = tableElement.rows[rowCount - 1];  // Lấy hàng cuối cùng
        var newRow = lastRow.cloneNode(true);  // Tạo một bản sao của hàng cuối cùng
        lastRow.parentNode.insertBefore(newRow, lastRow.nextSibling);  // Thêm hàng mới

        // Thiết lập giá trị cho hàng mới
        for (var i = 3; i < lastRow.cells.length - 1; i++) {  // Bắt đầu từ cột thứ 3 đến cột cuối
            newRow.cells[i].innerText = "";  // Đặt văn bản của ô thành rỗng
        }
    }
}

// Hàm để xóa một hàng trong bảng
function deleteRow(row) {
    var tableBody = row.parentNode.parentNode;  // Lấy phần thân bảng từ hàng đã cho
    var planTable = document.getElementById("dataTable");  // Thay "dataTable" bằng ID thực tế của bảng
    var rowCount = planTable.rows.length;  // Đếm số hàng hiện tại trong bảng

    if (rowCount === 4) {  // Nếu số hàng bằng 4
        alert(lang[92]);  // Gửi thông báo cho người dùng
    } else {
        tableBody.removeChild(row);  // Xóa hàng
    }

    var totalColumnValue1 = 0;
    var totalColumnValue2 = 0;

    // Tính tổng giá trị của hai cột trong bảng
    for (var i = 3; i < rowCount - 1; i++) {
        totalColumnValue1 += parseInt(planTable.rows[i].cells[20].innerText);  // Thay [20] bằng chỉ số cột thực tế
        totalColumnValue2 += parseInt(planTable.rows[i].cells[21].innerText);  // Thay [21] bằng chỉ số cột thực tế
    }

    // Cập nhật giá trị tổng vào hàng thứ ba
    planTable.rows[2].cells[20].innerText = totalColumnValue1;  // Cập nhật giá trị cột 20
    planTable.rows[2].cells[21].innerText = totalColumnValue2;  // Cập nhật giá trị cột 21
}

// Hàm kiểm tra tính năng ẩn của tùy chọn
function testHidden() {
    if (isOptionHiddenSupported()) {
        alert(name_Variable[145]);  // Gửi thông báo nếu hỗ trợ ẩn
    } else {
        alert(name_Variable[146]);  // Gửi thông báo nếu không hỗ trợ ẩn
    }
}

// Hàm kiểm tra hỗ trợ tính năng ẩn của tùy chọn
function isOptionHiddenSupported() {
    const hiddenOption = document.getElementById("hiddenOption");  // Lấy phần tử tùy chọn ẩn
    const supportedElement = document.getElementById("supportedElement");  // Lấy phần tử hỗ trợ
    return 'hidden' in supportedElement && 'option' in hiddenOption;  // Kiểm tra xem các thuộc tính có tồn tại trong các phần tử không
}

function callTrainingResult(selectElement) {
    event.preventDefault();  // Ngăn chặn hành động mặc định của sự kiện

    var selectedRow = selectElement.parentNode.parentNode;  // Lấy hàng đại diện cho tùy chọn đã chọn
    var selectedIndex = selectedRow.rowIndex;  // Lấy chỉ số hàng đã chọn
    var trainingResultsTable = document.getElementById("trainingResults"); // Lấy bảng kết quả đào tạo
    var abilityInput = document.getElementById("abilityInput").value; // Lấy giá trị nhập từ trường năng lực

    var trainingSession = trainingResultsTable.rows[selectedIndex < 4 ? 1 : selectedIndex - 1];
    var summary = "";
    var fullSummary = "";
    var isMobile = isMobileDevice();  // Kiểm tra xem thiết bị có phải di động không

    // Tạo chuỗi tóm tắt cho khoảng từ 3 đến 17
    for (var i = 3; i < 18; i++) {
        summary += trainingSession.cells[i].innerText + (i === 17 ? "" : ", ");  // Tạo chuỗi từ các ô
    }

    var totalRows = trainingResultsTable.rows.length;  // Đếm tổng số hàng trong bảng

    // Cập nhật nội dung kết quả đào tạo từ hàng đã chọn đến hàng cuối cùng
    for (var r = selectedIndex; r < totalRows; r++) {
        var currentRow = trainingResultsTable.rows[r];
        fullSummary += currentRow.cells[0].innerText + ": ";  // Lấy tên hoặc thông tin từ ô đầu tiên

        fullSummary += parseInt(currentRow.cells[1].innerText) + (r === totalRows - 1 ? "" : ", ");

        var nestedOptions = currentRow.cells[2].getElementsByTagName("select");
        if (nestedOptions.length > 0) {
            var selectedOption = nestedOptions[0].selectedOptions[0]; // Có thể là nhiều tùy chọn
            if (selectedOption) {
                fullSummary += selectedOption.innerText; // Thêm văn bản tùy chọn vào chuỗi
            }
        }
    }

    // Lấy dữ liệu từ localStorage
    var additionalData = localStorage.getItem("top11experiencePlayerSkillCol");

    // Tạo và cấu hình XMLHttpRequest
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "yourUrlEndpoint", true); // Đổi "yourUrlEndpoint" thành URL API
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
            var response = JSON.parse(xhr.responseText); // Phân tích cú pháp dữ liệu phản hồi
            for (var i = selectedIndex; i < totalRows; i++) {
                var resultRow = trainingResultsTable.rows[i];
                var resultData = response[i - selectedIndex]; // Lấy dữ liệu từ phản hồi

                // Cập nhật giá trị cho các ô trong hàng cụ thể
                for (var j = 0; j < resultData.length; j++) {
                    var cell = resultRow.cells[j + 3]; // Các ô bắt đầu từ cột thứ 4
                    cell.innerText = resultData[j]; // Cập nhật giá trị ô
                }
            }
            // Cập nhật lại tổng giá trị nếu cần
            var totalValue1 = 0, totalValue2 = 0;
            for (var i = 3; i < totalRows; i++) {
                totalValue1 += parseInt(trainingResultsTable.rows[i].cells[20].innerText);
                totalValue2 += parseInt(trainingResultsTable.rows[i].cells[21].innerText);
            }
            trainingResultsTable.rows[2].cells[20].innerText = totalValue1; // Cập nhật tổng
            trainingResultsTable.rows[2].cells[21].innerText = totalValue2; // Cập nhật tổng
        }
    };

    // Gửi dữ liệu đến máy chủ
    xhr.send("abilityInput=" + encodeURIComponent(abilityInput) +
        "&summary=" + encodeURIComponent(summary) +
        "&fullSummary=" + encodeURIComponent(fullSummary) +
        "&additionalData=" + encodeURIComponent(additionalData));
}

// Hàm cập nhật hoặc thêm dữ liệu vào một danh sách
function updateOrAddData(dataArray, newData) {
    const currentList = dataArray[name_Variable[97]](name_Variable[126]); // Lấy danh sách hiện tại
    if (currentList[name_Variable[21]] >= 4) {
        currentList[3] = newData;  // Cập nhật nếu danh sách có 4 phần tử trở lên
    } else {
        currentList.push(newData);  // Thêm dữ liệu mới vào danh sách
    }
    return currentList[name_Variable[98]](name_Variable[126]); // Trả về danh sách đã được cập nhật
}

// Hàm điền dữ liệu đã lưu vào giao diện
function fillSavedData(savedData) {
    var savedDataLength = savedData[name_Variable[112]];  // Lấy chiều dài dữ liệu đã lưu
    var dataArray = [];
    var displayText = name_Variable[30];  // Giá trị mặc định

    // Nếu có dữ liệu đã lưu
    if (savedDataLength > 0) {
        dataArray = JSON[name_Variable[9]](savedData[name_Variable[10]]); // Phân tích dữ liệu đã lưu
        displayText = dataArray[name_Variable[10]][name_Variable[97]](name_Variable[149]);  // Lấy giá trị cần hiển thị
    }

    var currentIndex = -1;  // Đánh dấu chỉ số hiện tại
    var options = document[name_Variable[23]](name_Variable[166]); // Lấy danh sách các tùy chọn
    options[name_Variable[31]](function (option) {
        currentIndex += 1;  // Tăng chỉ số
        if (displayText[name_Variable[21]] > 1) {
            option[name_Variable[10]] = displayText[currentIndex]; // Gán giá trị nếu có nhiều hơn 1
        } else {
            option[name_Variable[10]] = name_Variable[30]; // Gán giá trị mặc định
        }
    });

    // Cập nhật giao diện người dùng
    var formElement = document[name_Variable[1]](name_Variable[167]);
    formElement[name_Variable[160]] = true; // Thiết lập trạng thái hiển thị
    formElement[name_Variable[26]][name_Variable[94]] = name_Variable[66];

    // Nếu không có dữ liệu đã lưu
    if (savedData[name_Variable[112]] === 0) {
        var dataTable = document[name_Variable[1]](name_Variable[32]);
        var rowCount = dataTable[name_Variable[41]][name_Variable[21]];
        addRow();  // Thêm hàng mới
        for (var i = 1; i < rowCount - 2; i++) {
            var rowToRemove = dataTable[name_Variable[41]][rowCount - i];
            var deleteButton = rowToRemove[name_Variable[45]][22][name_Variable[43]](name_Variable[113]);
            deleteRow(deleteButton);  // Xóa hàng không cần thiết
        }
        return;  // Kết thúc hàm nếu không có dữ liệu
    }

    // Hiển thị dữ liệu và xóa các kiểu dáng không cần thiết
    displayTextV2();
    showSamplePlan();
    removeSoSanhCellStyle();
    // Xóa các phần tử PA theo yêu cầu
    xoaPA1();
    xoaPA2();
    xoaPA3();

    // Đặt lại chỉ số cho các phần tử trong biểu mẫu
    var formElements = form[name_Variable[23]](name_Variable[47]);
    formElements[name_Variable[31]](function (element) {
        element[name_Variable[112]] = 0; // Đặt lại giá trị cho các phần tử
    });
}

function showSamplePlan() {
    var samplePlanElement = document.getElementById(name_Variable[168]);  // Lấy phần tử kế hoạch mẫu
    var samplePlans = samplePlanElement[name_Variable[10]][name_Variable[97]](name_Variable[118]); // Lấy danh sách các kế hoạch mẫu
    var selectElement = document.getElementById(name_Variable[3]);  // Lấy phần tử chọn

    selectElement[name_Variable[112]] = 0;  // Đặt chỉ số phần tử đã chọn về 0

    // Đánh dấu tất cả các tùy chọn trong list
    for (var i = 1; i < selectElement[name_Variable[13]].length; i++) {
        selectElement[name_Variable[13]][i][name_Variable[61]] = true;  // Đánh dấu là chọn
    }

    // Xử lý từng kế hoạch trong danh sách kế hoạch mẫu
    for (var j = 0; j < samplePlans.length; j++) {
        var currentSamplePlan = samplePlans[j];

        // Thay thế ký tự 'L' hoặc 'R' bằng các giá trị khác nhau
        if (currentSamplePlan.includes(name_Variable[169])) {
            currentSamplePlan = currentSamplePlan.replace(/L/g, name_Variable[170]);
        } else if (currentSamplePlan.includes(name_Variable[172])) {
            currentSamplePlan = currentSamplePlan.replace(/R/g, name_Variable[170]);
        }

        // So sánh kế hoạch mẫu với các tùy chọn trong list
        for (var k = 1; k < selectElement[name_Variable[13]].length; k++) {
            var optionElement = selectElement[name_Variable[13]][k][name_Variable[11]][name_Variable[97]](name_Variable[123])[0];
            var optionPlans = optionElement[name_Variable[97]](name_Variable[118]);

            for (var l = 0; l < optionPlans.length; l++) {
                var optionPlan = optionPlans[l];

                // Thay thế ký tự 'L' hoặc 'R' trong tùy chọn
                if (optionPlan.includes(name_Variable[169])) {
                    optionPlan = optionPlan.replace(/L/g, name_Variable[170]);
                } else if (optionPlan.includes(name_Variable[172])) {
                    optionPlan = optionPlan.replace(/R/g, name_Variable[170]);
                }

                // Nếu kế hoạch mẫu và tùy chọn trùng khớp, bỏ chọn tùy chọn
                if (currentSamplePlan === optionPlan) {
                    selectElement[name_Variable[13]][k][name_Variable[61]] = false;  // Bỏ chọn
                }
            }
        }
    }
}

// Hàm lấy kế hoạch của người chơi từ JSON
function getPlayerPlan(jsonData) {
    var playerPlan = JSON.parse(jsonData); // Phân tích chuỗi JSON
    if (playerPlan) {
        return playerPlan[name_Variable[134]]; // Trả về kế hoạch của người chơi
    } else {
        return name_Variable[30]; // Trả về giá trị mặc định nếu không có kế hoạch
    }
}

// Hàm kiểm tra xem tùy chọn có tồn tại trong phần tử chọn hay không
function containsOption(selectElement, optionValue) {
    for (var index = 0; index < selectElement[name_Variable[13]].length; index++) {
        if (selectElement[name_Variable[13]][index][name_Variable[10]] === optionValue) {
            return true; // Nếu tùy chọn tồn tại, trả về true
        }
    }
    return false; // Nếu không có tùy chọn nào trùng khớp, trả về false
}

// Hàm tải kế hoạch mẫu từ localStorage
function loadSamplePlan() {
    const samplePlanData = JSON.parse(localStorage.getItem(name_Variable[5])); // Lấy kế hoạch mẫu từ localStorage
    var samplePlanElement = document.getElementById(name_Variable[168]); // Lấy phần tử kế hoạch mẫu
    var samplePlans = samplePlanElement[name_Variable[10]][name_Variable[97]](name_Variable[118]); // Lấy danh sách kế hoạch mẫu

    if (samplePlanData && Array.isArray(samplePlanData)) { // Kiểm tra xem dữ liệu có tồn tại và là một mảng
        const selectElement = document.getElementById(name_Variable[3]); // Lấy phần tử select từ DOM
        for (var i = 0; i < samplePlanData.length; i++) {
            const planName = samplePlanData[i][name_Variable[10]]; // Lấy tên kế hoạch
            const planDescription = samplePlanData[i][name_Variable[131]]; // Lấy mô tả kế hoạch

            // Nếu tùy chọn chưa có trong select thì thêm vào
            if (!containsOption(selectElement, planName)) {
                const newOption = document.createElement('option'); // Tạo phần tử option mới
                newOption.textContent = planName; // Thiết lập nội dung cho option
                newOption.value = planDescription; // Thiết lập giá trị cho option
                newOption.selected = true; // Đánh dấu là đã chọn
                selectElement.appendChild(newOption); // Thêm option vào select
            }
        }
    }
}

function setWhitecolV2(colourFlag) {
    var dataElement = document.getElementById(name_Variable[32]); // Lấy phần tử cần xử lý
    var resultElement = document.querySelector(name_Variable[87]); // Lấy phần tử dùng để hiển thị kết quả
    const inputElement = document.getElementById(name_Variable[168]).value; // Nhận giá trị từ một phần tử đầu vào
    var combinedResult = name_Variable[30]; // Khởi tạo kết quả kết hợp ban đầu

    // Duyệt qua các phần tử từ chỉ số 3 đến 17
    for (var i = 3; i < 18; i++) {
        combinedResult += dataElement[name_Variable[41]][0][name_Variable[45]][i][name_Variable[62]]
            + (i < 17 ? name_Variable[149] : name_Variable[30]); // Kết hợp giá trị
    };

    const xhr = new XMLHttpRequest(); // Tạo một đối tượng XMLHttpRequest
    xhr.open(name_Variable[70], name_Variable[174], true); // Mở yêu cầu
    xhr.setRequestHeader(name_Variable[153], name_Variable[73]); // Thiết lập tiêu đề yêu cầu

    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
            var responseData = xhr.responseText; // Nhận dữ liệu phản hồi
            var parsedData = responseData[name_Variable[97]](name_Variable[154])[0];
            var totalValue1 = 0; // Khai báo tổng cho lần 1
            var totalValue2 = 0; // Khai báo tổng cho lần 2
            var summary1 = name_Variable[30]; // Tổng kết tạm thời 1
            var summary2 = name_Variable[30]; // Tổng kết tạm thời 2

            for (var i = 0; i < dataElement[name_Variable[41]].length; i++) { // Duyệt qua dữ liệu
                var currentItem = dataElement[name_Variable[41]][i];
                for (var j = 3; j < 18; j++) { // Duyệt qua các chỉ số trong mục hiện tại
                    var currentCell = currentItem[name_Variable[45]][j];
                    if (!parsedData[name_Variable[88]](j < 10 ? name_Variable[175] + j : name_Variable[30] + j)) {
                        currentCell[name_Variable[65]][name_Variable[20]](i < 3 ? name_Variable[176] : name_Variable[177]);
                        currentCell[name_Variable[65]][name_Variable[147]](i < 3 ? name_Variable[63] : name_Variable[104]);
                        if (i === 1) {
                            totalValue1 += parseFloat(currentItem[name_Variable[45]][j][name_Variable[62]]);
                            summary1 += j + name_Variable[149];
                        }
                    } else {
                        currentCell[name_Variable[65]][name_Variable[20]](i < 3 ? name_Variable[63] : name_Variable[104]);
                        currentCell[name_Variable[65]][name_Variable[147]](i < 3 ? name_Variable[176] : name_Variable[177]);
                        if (i === 1 && parsedData[name_Variable[88]](j < 10 ? name_Variable[175] + j : name_Variable[30] + j)) {
                            totalValue2 += parseFloat(currentItem[name_Variable[45]][j][name_Variable[62]]);
                            summary2 += j + name_Variable[149];
                        }
                    }

                    // Cập nhật thông tin sau khi duyệt qua tất cả các chỉ số
                    if (j === 17) {
                        dataElement[name_Variable[41]][1][name_Variable[45]][19][name_Variable[62]] = (totalValue2 * 100.0 / (totalValue1 + totalValue2)).toFixed(1);
                        dataElement[name_Variable[41]][2][name_Variable[45]][19][name_Variable[62]] = dataElement[name_Variable[41]][1][name_Variable[45]][19][name_Variable[178]];
                    }
                }
            }

            // Lưu kết quả vào localStorage
            localStorage.setItem(name_Variable[151], summary2.slice(0, summary2.length - 1) + name_Variable[123] + summary1.slice(0, summary1.length - 1));

            // Xử lý với các phần tử trong response
            for (var i = 1; i < 31; i++) {
                var itemData = parsedData[name_Variable[97]](name_Variable[154])[i]; // Lấy dữ liệu từng mục
                if (itemData) {
                    if (resultElement[i][name_Variable[62]].includes(name_Variable[125])) {
                        var existingData = resultElement[i][name_Variable[62]].split(name_Variable[125]);
                        resultElement[i][name_Variable[62]] = existingData[0] + name_Variable[126] + itemData[name_Variable[97]](name_Variable[123])[0];
                    } else {
                        resultElement[i][name_Variable[62]] += name_Variable[126] + itemData[name_Variable[97]](name_Variable[123])[0];
                    }
                    resultElement[i][name_Variable[89]] = itemData[name_Variable[97]](name_Variable[123])[1];

                    // Kiểm tra và cập nhật trạng thái
                    if (parseInt(itemData[name_Variable[97]](name_Variable[125])[1]) === 0) {
                        resultElement[i][name_Variable[61]] = true;
                    } else {
                        resultElement[i][name_Variable[61]] = false;
                    }
                }
            }

            // Kiểm tra phần tử chọn và gọi hàm fillPlan nếu cần thiết
            var selectElement = document.getElementById(name_Variable[68]);
            if (selectElement.selectedIndex !== 0 && colourFlag === 0) {
                fillPlan(getPlayerPlan(selectElement.value)); // Gọi hàm fillPlan với giá trị của phần tử chọn
            }

            // Gọi hàm để thiết lập tiêu đề cho tùy chọn khoan
            setDrillOptionsTitle();
        }
    };

    // Gửi yêu cầu đến server
    xhr.send(name_Variable[180] + encodeURIComponent(inputElement) + name_Variable[181] + encodeURIComponent(combinedResult));
}

function setDrillOptionsTitle() {
    var drillOptionsElement = document.getElementById(name_Variable[32]); // Lấy phần tử chứa các tùy chọn khoan
    if (drillOptionsElement) {
        var targetArray = drillOptionsElement[name_Variable[41]][3]; // Lấy phần tử thứ 4 trong mảng

        // Duyệt qua các tùy chọn từ chỉ số 4 đến hết
        for (var i = 4; i < drillOptionsElement[name_Variable[41]].length; i++) {
            var currentItem = drillOptionsElement[name_Variable[41]][i]; // Mục hiện tại
            if (currentItem[name_Variable[45]].length > 1) { // Kiểm tra xem có nhiều hơn một tùy chọn không
                var firstOption = currentItem[name_Variable[45]][1]; // Lấy tùy chọn đầu tiên

                // Kiểm tra các điều kiện trên tùy chọn
                if (firstOption[name_Variable[182]].length > 0 &&
                    firstOption[name_Variable[182]][0][name_Variable[183]].toString() === name_Variable[47]) {

                    var firstElement = firstOption[name_Variable[182]][0]; // Lấy phần tử đầu tiên
                    var subOptions = firstElement[name_Variable[50]](name_Variable[132]); // Lấy các tùy chọn con

                    // Cập nhật tiêu đề cho các tùy chọn trong mảng
                    for (var j = 0; j < subOptions.length; j++) {
                        var targetSubOption = targetArray[name_Variable[45]][1][name_Variable[50]](name_Variable[47])[0][name_Variable[50]](name_Variable[132])[j];
                        subOptions[j][name_Variable[89]] = targetSubOption[name_Variable[89]]; // Cập nhật tiêu đề
                    }
                }
            }
        }
    }
}

// Thiết lập sự kiện cho nút chuyển sang chế độ toàn màn hình
fullscreenButton.addEventListener(name_Variable[184], function (event) {
    event.preventDefault(); // Ngăn chặn hành động mặc định của sự kiện
    var targetElement = document.querySelector(name_Variable[185]); // Lấy phần tử mục tiêu
    var currentFullscreenState = fullscreenButton[name_Variable[62]]; // Lấy trạng thái toàn màn hình hiện tại

    // Kiểm tra trạng thái toàn màn hình
    if (currentFullscreenState[name_Variable[81]]() === name_Variable[186]) {
        fullscreenButton[name_Variable[62]] = name_Variable[187]; // Cập nhật trạng thái
        targetElement[name_Variable[65]].addEventListener(name_Variable[147], name_Variable[186]); // Thêm sự kiện cho nút

        // Bắt đầu chế độ toàn màn hình
        if (document.documentElement.requestFullscreen) {
            document.documentElement.requestFullscreen();
        } else if (document.documentElement.webkitRequestFullscreen) { // Safari
            document.documentElement.webkitRequestFullscreen();
        } else if (document.documentElement.mozRequestFullScreen) { // Firefox
            document.documentElement.mozRequestFullScreen();
        } else if (document.documentElement.msRequestFullscreen) { // IE/Edge
            document.documentElement.msRequestFullscreen();
        }
    } else {
        fullscreenButton[name_Variable[62]] = name_Variable[193]; // Cập nhật trạng thái khi thoát chế độ toàn màn hình
        targetElement[name_Variable[65]].removeEventListener(name_Variable[20], name_Variable[186]); // Xóa sự kiện cho nút

        // Thoát chế độ toàn màn hình
        if (document.exitFullscreen) {
            document.exitFullscreen();
        } else if (document.webkitExitFullscreen) { // Safari
            document.webkitExitFullscreen();
        } else if (document.mozCancelFullScreen) { // Firefox
            document.mozCancelFullScreen();
        } else if (document.msExitFullscreen) { // IE/Edge
            document.msExitFullscreen();
        }
    }
});

function displayTextV2() {
    event.preventDefault(); // Ngăn chặn hành động mặc định của sự kiện
    langChange(selectedLanguage); // Thay đổi ngôn ngữ

    var textArray = []; // Mảng để lưu trữ văn bản
    var tableElement = document.querySelector(name_Variable[166]); // Lấy phần tử bảng
    tableElement.forEach(function (row) {
        textArray.push(row[name_Variable[10]]); // Thêm văn bản của từng hàng vào mảng
    });

    localStorage.setItem(name_Variable[198], textArray); // Lưu mảng văn bản vào localStorage

    var mainTable = document.getElementById(name_Variable[32]); // Lấy bảng chính
    var specificValue = document.querySelector(name_Variable[119]).value.split(name_Variable[118]); // Lấy và chia giá trị cụ thể
    var resultElement = document.querySelector(name_Variable[87]); // Lấy phần tử kết quả
    var totalSum = 0; // Tổng giá trị
    var count = 0; // Đếm số biến

    // Duyệt qua hàng trong bảng
    for (var rowIndex = 1; rowIndex < 3; rowIndex++) {
        var currentRow = mainTable[name_Variable[41]][rowIndex];
        for (var colIndex = 1; colIndex < 18; colIndex++) {
            var cell = currentRow[name_Variable[45]][colIndex];
            if (rowIndex === 2 && colIndex < 3) continue; // Bỏ qua một số ô

            // Cập nhật giá trị ô
            cell[name_Variable[62]] = textArray[(colIndex < 3) ? (colIndex - 1) : colIndex];

            if (rowIndex === 1 && colIndex > 2) {
                totalSum += parseFloat(textArray[colIndex]); // Cộng dồn tổng
            }

            if (rowIndex === 2) {
                cell[name_Variable[26]][name_Variable[94]] = name_Variable[96]; // Cập nhật thông tin cell
                cell[name_Variable[26]][name_Variable[156]] = name_Variable[159];
            }
        }
    }

    // Cập nhật các ô tổng trong bảng
    mainTable[name_Variable[41]][1][name_Variable[45]][18][name_Variable[62]] = (totalSum / 15.0).toFixed(1);
    mainTable[name_Variable[41]][2][name_Variable[45]][18][name_Variable[62]] = (totalSum / 15.0).toFixed(1);

    setWhitecolV2(0); // Gọi hàm để xử lý màu trắng
    var additionalValue = mainTable[name_Variable[41]][0][name_Variable[45]][19][name_Variable[62]]; // Giá trị bổ sung
    var anotherElement = document.getElementById(name_Variable[167]); // Lấy phần tử khác
    anotherElement[name_Variable[160]] = false; // Cập nhật thuộc tính
    anotherElement[name_Variable[26]][name_Variable[94]] = name_Variable[158]; // Cập nhật thông tin
}

function fillPlan(planData) {
    var table = document.getElementById(name_Variable[32]); // Lấy bảng từ DOM
    var totalRows = table[name_Variable[41]][name_Variable[21]]; // Tổng số hàng trong bảng

    // Xóa các hàng dư thừa
    for (var rowIndex = 1; rowIndex < totalRows - 3; rowIndex++) {
        var currentRow = table[name_Variable[41]][totalRows - rowIndex];
        var rowIdentifier = currentRow[name_Variable[45]][22][name_Variable[43]](name_Variable[113]);
        deleteRow(rowIdentifier); // Xóa hàng hiện tại
    }

    totalRows = table[name_Variable[41]][name_Variable[21]] - 3; // Cập nhật tổng số hàng sau khi xóa

    if (planData[name_Variable[21]] > 0) {  // Nếu có dữ liệu từ kế hoạch
        var planItems = planData[name_Variable[97]](name_Variable[149]); // Lấy danh sách các mục từ kế hoạch
        for (var i = 0; i < planItems[name_Variable[21]] - totalRows; i++) {
            addRow(); // Thêm hàng mới vào bảng
        }

        var rowCounter = 3;
        planItems.forEach(function (item) {
            var itemDetails = item[name_Variable[97]](name_Variable[123]); // Lấy chi tiết của mục
            var value1 = parseInt(itemDetails[0]);
            var value2 = itemDetails[1];
            var currentRow = table[name_Variable[41]][rowCounter]; // Lấy hàng hiện tại

            // Cập nhật giá trị vào ô tương ứng
            var valueCell = currentRow[name_Variable[45]][1][name_Variable[43]](name_Variable[47]);
            valueCell[name_Variable[112]] = value1; // Cập nhật giá trị vào ô

            var textCell = currentRow[name_Variable[45]][2][name_Variable[43]](name_Variable[42]);
            textCell[name_Variable[10]] = value2; // Cập nhật text vào ô

            rowCounter++; // Tăng biến đếm hàng
        });

        // Xóa các hàng không còn cần thiết
        totalRows = table[name_Variable[41]][name_Variable[21]];
        if (rowCounter < totalRows) {
            for (var i = 1; i < totalRows - rowCounter + 1; i++) {
                var currentRow = table[name_Variable[41]][totalRows - i];
                var rowIdentifier = currentRow[name_Variable[45]][22][name_Variable[43]](name_Variable[113]);
                deleteRow(rowIdentifier); // Xóa hàng không cần thiết
            }
        }

        // Gọi hàm kết quả huấn luyện với dữ liệu cụ thể
        var targetRow = table[name_Variable[41]][3];
        var trainingResultCell = targetRow[name_Variable[45]][2][name_Variable[43]](name_Variable[42]);
        callTrainingResult(trainingResultCell); // Gọi hàm xử lý kết quả
    }
}

function getPlanValue() {
    var table = document.getElementById(name_Variable[32]); // Lấy bảng từ DOM
    // Tạo chuỗi chứa giá trị từ hàng thứ 1 và chia giá trị tổng từ hàng thứ 2
    var planValue = table[name_Variable[41]][1][name_Variable[45]][2][name_Variable[62]]
        + name_Variable[199]
        + parseInt(table[name_Variable[41]][2][name_Variable[45]][18][name_Variable[62]] / 10)
        + name_Variable[200];

    var valuesArray = []; // Khởi tạo mảng chứa các giá trị

    // Duyệt qua các hàng trong bảng
    for (var rowIndex = 3; rowIndex < table[name_Variable[41]][name_Variable[21]]; rowIndex++) {
        var currentRow = table[name_Variable[41]][rowIndex];
        var quantity = currentRow[name_Variable[43]](name_Variable[47])[name_Variable[112]]; // Lấy số lượng
        var itemName = currentRow[name_Variable[43]](name_Variable[42])[name_Variable[10]]; // Lấy tên mặt hàng

        if (quantity > 0) {
            valuesArray.push(quantity + name_Variable[123] + itemName); // Thêm vào mảng nếu số lượng lớn hơn 0
        }
    }

    var resultString = valuesArray.join(name_Variable[149]); // Kết hợp các giá trị trong mảng thành chuỗi
    return resultString; // Trả về chuỗi kết quả
}

function savePlayerInfo() {
    event.preventDefault(); // Ngăn hành động mặc định của sự kiện
    var playerDataFromStorage = localStorage.getItem(name_Variable[130]); // Lấy dữ liệu người chơi từ localStorage
    var playersArray = []; // Khởi tạo mảng lưu trữ người chơi

    if (playerDataFromStorage) {
        playersArray = JSON.parse(playerDataFromStorage); // Chuyển đổi dữ liệu JSON từ localStorage
    }

    var allValues = []; // Mảng lưu trữ tất cả các giá trị
    var playerLabel = document.getElementById(name_Variable[201]).value
        + name_Variable[202]
        + document.getElementById(name_Variable[168]).value; // Lấy nhãn người chơi từ các phần tử đầu vào

    var tableElement = document.querySelector(name_Variable[166]); // Lấy bảng
    tableElement.forEach(function (row) {
        allValues.push(row.value); // Thêm giá trị từ từng hàng vào mảng
    });

    var combinedValues = allValues.join(name_Variable[149]); // Kết hợp các giá trị thành chuỗi
    var existingPlayerIndex = -1; // Biến lưu chỉ số người chơi đã tồn tại

    // Kiểm tra xem người chơi đã tồn tại trong mảng chưa
    for (var i = 0; i < playersArray.length; i++) {
        if (playersArray[i][name_Variable[131]] === playerLabel) {
            existingPlayerIndex = i;
            break; // Thoát khỏi vòng lặp khi tìm thấy
        }
    }

    var planValue = getPlanValue(); // Lấy giá trị kế hoạch
    if (existingPlayerIndex !== -1) { // Nếu người chơi đã tồn tại
        playersArray[existingPlayerIndex][name_Variable[10]] = combinedValues; // Cập nhật giá trị
        playersArray[existingPlayerIndex][name_Variable[134]] = planValue; // Cập nhật kế hoạch
        updateSelectedPlayer(JSON.stringify({
            value: combinedValues,
            plan: planValue
        })); // Cập nhật người chơi đã chọn
    } else {
        // Nếu người chơi chưa tồn tại, thêm mới
        var newPlayer = {
            label: playerLabel,
            value: combinedValues,
            plan: planValue
        };
        playersArray.push(newPlayer); // Thêm người chơi mới vào mảng

        var selectElement = document.getElementById(name_Variable[68]); // Lấy phần tử lựa chọn
        var newOption = document.createElement("option"); // Tạo mục chọn mới
        newOption.value = playerLabel; // Gán giá trị
        newOption.innerHTML = JSON.stringify({
            value: combinedValues,
            plan: planValue
        });
        selectElement.appendChild(newOption); // Thêm tùy chọn mới vào phần tử lựa chọn
        selectElement.selectedIndex = playersArray.length - 1; // Đặt mục vừa thêm làm mục được chọn
    }

    // Tạo đối tượng chứa thông tin người chơi
    var playerRecord = {
        label: playerLabel,
        value: combinedValues,
        plan: planValue
    };

    savePlayerData(JSON.stringify(playerRecord)); // Lưu thông tin người chơi
    var playersDataString = JSON.stringify(playersArray); // Chuyển đổi mảng người chơi thành chuỗi JSON
    localStorage.setItem(name_Variable[130], playersDataString); // Lưu vào localStorage

    alert(lang[89] + name_Variable[203] + playerLabel + name_Variable[204]); // Hiển thị thông báo thành công
}

function updateSelectedPlayer(playerData) {
    const selectElement = document.getElementById(name_Variable[68]); // Lấy phần tử lựa chọn
    const selectedOption = selectElement.options[selectElement.selectedIndex]; // Lấy tùy chọn được chọn
    selectedOption.innerHTML = playerData; // Cập nhật nội dung của tùy chọn
}

function savePlayerData(playerData) {
    var xhr = new XMLHttpRequest(); // Tạo đối tượng XMLHttpRequest
    var requestUrl = name_Variable[67]; // URL để gửi dữ liệu
    var encodedData = name_Variable[205] + playerData.replace(/\+/g, name_Variable[125]); // Mã hóa dữ liệu

    xhr.open(name_Variable[70], requestUrl, true); // Thiết lập phương thức và URL
    xhr.setRequestHeader(name_Variable[153], name_Variable[73]); // Đặt header cho yêu cầu

    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
            var responseData = xhr.responseText; // Lấy phản hồi từ server
            // Có thể xử lý phản hồi nếu cần
        }
    };

    xhr.send(encodedData); // Gửi yêu cầu với dữ liệu đã mã hóa
}

function deletePlayerInfo() {
    event.preventDefault(); // Ngăn chặn hành động mặc định của sự kiện
    var playerValues = [];
    var playerLabel = name_Variable[206] + document.getElementById(name_Variable[201]).value + name_Variable[202] + document.getElementById(name_Variable[168]).value; // Lấy nhãn người chơi

    var tableElement = document.querySelector(name_Variable[166]); // Lấy bảng người chơi
    tableElement.forEach(function (row) {
        playerValues.push(row.value); // Thêm giá trị từng hàng vào mảng
    });

    var combinedValues = playerValues.join(name_Variable[149]); // Kết hợp tất cả giá trị thành chuỗi
    var planValue = getPlanValue(); // Lấy giá trị kế hoạch từ hàm getPlanValue

    // Đối tượng chứa thông tin người chơi
    var playerInfo = {
        label: playerLabel,
        value: combinedValues,
        plan: planValue
    };

    savePlayerData(JSON.stringify(playerInfo)); // Lưu thông tin người chơi
    var storedPlayers = localStorage.getItem(name_Variable[130]); // Lấy dữ liệu người chơi từ localStorage
    var playersArray = [];

    if (storedPlayers) {
        playersArray = JSON.parse(storedPlayers); // Chuyển đổi dữ liệu JSON thành mảng
    }

    var selectElement = document.getElementById(name_Variable[68]); // Lấy phần tử lựa chọn
    var selectedIndex = selectElement.selectedIndex; // Lấy chỉ số tùy chọn đã chọn

    // Kiểm tra nếu không có tùy chọn nào được chọn
    if (selectedIndex === 0) {
        alert(lang[107]); // Thông báo không có người chơi
        return;
    }

    var confirmation = confirm(lang[93] + name_Variable[203] + playerLabel + name_Variable[207]); // Xác nhận xóa
    if (!confirmation) {
        return; // Nếu người dùng không xác nhận thì thoát
    }

    // Đặt giá trị ô thành mặc định (hoặc giá trị khác) trước khi xóa
    tableElement.forEach(function (row) {
        row.value = name_Variable[30]; // Xóa giá trị
    });

    // Xóa người chơi khỏi mảng
    playersArray.splice(selectedIndex - 1, 1); // Xóa 1 mục tại chỉ số đã chọn
    localStorage.setItem(name_Variable[130], JSON.stringify(playersArray)); // Lưu lại mảng đã cập nhật vào localStorage

    // Cập nhật lại lựa chọn trong UI
    selectElement.remove(selectedIndex); // Xóa tùy chọn đã chọn khỏi phần tử lựa chọn
    alert(lang[90] + name_Variable[203] + playerLabel + name_Variable[204]); // Thông báo đã xóa thành công
}

function duplicateRow(rowElement) {
    // Lấy phần tử hàng từ đối số truyền vào
    var table = rowElement.parentNode; // Cha của hàng (bảng)
    var tableElement = document.getElementById(name_Variable[32]); // Lấy bảng chính
    var totalRows = tableElement.rows.length; // Tổng số hàng trong bảng

    // Kiểm tra xem đã đạt tối đa số hàng hay chưa
    if (totalRows >= 33) {
        alert(lang[91]); // Thông báo rằng không thể thêm hàng mới
        return;
    }

    // Nhân bản hàng hiện tại
    var newRow = rowElement.cloneNode(true); // Nhân bản hàng với tất cả các nút con
    var selectElement = newRow.querySelector(name_Variable[150]); // Tìm phần tử chọn trong hàng mới

    // Cập nhật giá trị của selectElement từ hàng gốc
    selectElement.selectedIndex = rowElement.querySelector(name_Variable[150]).selectedIndex; // Giữ nguyên chỉ số đã chọn
    table.appendChild(newRow); // Thêm hàng mới vào phần tử bảng
}

function sendPlanToServer(planData) {
    // Tạo đối tượng XMLHttpRequest mới
    var xhr = new XMLHttpRequest();
    var requestUrl = name_Variable[209]; // URL để gửi tin nhắn
    var requestData = name_Variable[69] + planData; // Dữ liệu để gửi

    // Thiết lập yêu cầu
    xhr.open(name_Variable[70], requestUrl, true); // Phương thức và URL
    xhr.setRequestHeader(name_Variable[72], name_Variable[73]); // Đặt tiêu đề yêu cầu

    // Xử lý phản hồi từ server
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
            var responseData = xhr.responseText; // Xử lý phản hồi, nếu cần
            // Có thể thêm mã để xử lý responseData
        }
    };

    xhr.send(requestData); // Gửi yêu cầu với dữ liệu
}
// Khi nút lưu kế hoạch mẫu được nhấn
btSaveSamplePlan.addEventListener('click', function (event) {
    event.preventDefault(); // Ngăn chặn hành động mặc định

    // Lấy dữ liệu từ localStorage
    var storedData = localStorage.getItem(name_Variable[3]);
    var plansList = [];

    // Nếu có dữ liệu đã lưu, chuyển đổi nó thành mảng
    if (storedData) {
        plansList = JSON.parse(storedData);
    }

    // Lấy bảng và dữ liệu kế hoạch từ giao diện
    var tableElement = document.getElementById(name_Variable[32]);

    // Tạo nhãn cho kế hoạch
    var planLabel = tableElement.rows[1].cells[2].innerHTML + name_Variable[199] +
        parseInt(tableElement.rows[2].cells[18].innerHTML / 10) +
        name_Variable[200];

    var planValues = [];

    // Lấy thông tin từ bảng bắt đầu từ hàng thứ 3
    for (var i = 3; i < tableElement.rows.length; i++) {
        var row = tableElement.rows[i];
        var selectValue = row.cells[name_Variable[47]].children[0].value;
        var textValue = row.cells[name_Variable[42]].innerHTML;
        planValues.push(selectValue + name_Variable[123] + textValue);
    }

    var combinedValues = planValues.join(name_Variable[149]);

    // Kiểm tra xem kế hoạch đã tồn tại trong danh sách đã lưu không
    var existingIndex = -1;
    for (var i = 0; i < plansList.length; i++) {
        if (plansList[i][name_Variable[131]] === planLabel) {
            existingIndex = i;
            break;
        }
    }

    var newPlanLabel = planLabel;
    var isNewPlan = false;

    // Nếu kế hoạch đã tồn tại, yêu cầu người dùng nhập nhãn mới
    if (existingIndex !== -1) {
        newPlanLabel = lang[96]; // Khai báo kế hoạch đã tồn tại
        const userInput = prompt(lang[103].replace('{0}', planLabel), planLabel);

        if (userInput === null) {
            return; // Nếu người dùng hủy
        } else {
            if (userInput.trim() !== planLabel) { // Nếu nhãn đã thay đổi
                planLabel = userInput.trim();
                existingIndex = -1; // Đánh dấu như một kế hoạch mới
            }
        }
        isNewPlan = true;
    } else {
        // Nếu không có kế hoạch, yêu cầu nhãn mới
        const userInput = prompt(lang[97] + name_Variable[203] + planLabel + name_Variable[204], planLabel);

        if (userInput === null) {
            return; // Nếu người dùng hủy
        } else {
            planLabel = userInput.trim();
            isNewPlan = true;
        }
    }

    // Lưu kế hoạch mới hoặc cập nhật kế hoạch đã tồn tại
    if (isNewPlan) {
        if (existingIndex !== -1) {
            plansList[existingIndex][name_Variable[10]] = combinedValues; // Cập nhật giá trị
            var updatedPlan = {
                label: planLabel,
                value: combinedValues
            };
            sendPlanToServer(JSON.stringify(updatedPlan)); // Gửi tới server
            alert(lang[95] + name_Variable[203] + planLabel + name_Variable[204]);
        } else {
            var newPlan = {
                label: planLabel,
                value: combinedValues
            };
            sendPlanToServer(JSON.stringify(newPlan)); // Gửi tới server
            plansList.push(newPlan); // Thêm kế hoạch mới vào danh sách
            alert(lang[94] + name_Variable[203] + planLabel + name_Variable[204]);
        }
        localStorage.setItem(name_Variable[3], JSON.stringify(plansList)); // Lưu vào localStorage
    }

    // Cập nhật giao diện với kế hoạch mới
    var selectElement = document.getElementById(name_Variable[3]);
    const newOption = document.createElement('option');
    newOption.text = combinedValues;
    newOption.value = planLabel;

    selectElement.add(newOption); // Thêm tùy chọn mới vào select
});

// Hiện thị form khả năng
function abilityForm() {
    event.preventDefault(); // Ngăn chặn hành động mặc định của sự kiện
    var formElement = document.getElementById(name_Variable[211]); // Lấy phần tử form
    formElement.style.display = name_Variable[54]; // Thiết lập hiển thị cho form
}

// Đóng form khả năng
function closeForm() {
    var formElement = document.getElementById(name_Variable[211]); // Lấy phần tử form
    formElement.style.display = name_Variable[55]; // Thiết lập hiển thị để đóng form

    // Đặt lại giá trị cho tất cả các input trong form
    var inputs = form.elements[name_Variable[42]]; // Lấy tất cả input trong form
    for (var i = 0; i < inputs.length - 1; i++) {
        inputs[i].value = name_Variable[30]; // Đặt lại giá trị cho từng input
    }
}

// Lắng nghe sự kiện gửi form
form.addEventListener("submit", function (event) {
    event.preventDefault(); // Ngăn chặn hành động mặc định của sự kiện
    var selectedIndex = getSelectedIndex(document.getElementById("talentSelect")); // lấy chỉ số đã chọn
    var inputValue = document.getElementById(name_Variable[214]).value; // Lấy giá trị input
    var requestData = name_Variable[215] + selectedIndex + name_Variable[216] + inputValue; // Dữ liệu cần gửi

    var xhr = new XMLHttpRequest(); // Tạo đối tượng XMLHttpRequest
    xhr.onreadystatechange = function () {
        if (xhr.readyState === XMLHttpRequest.DONE) { // Kiểm tra trạng thái hoàn thành
            if (xhr.status === 200) { // Nếu thành công
                abilityInput.value = xhr.responseText; // Cập nhật giá trị cho input khả năng
            } else {
                console.error(name_Variable[218]); // Ghi lại lỗi vào console
            }
        }
    };

    // Thiết lập yêu cầu
    xhr.open(name_Variable[70], name_Variable[220], true);
    xhr.setRequestHeader(name_Variable[72], name_Variable[73]);
    xhr.send(requestData); // Gửi yêu cầu
    closeForm(); // Đóng form sau khi gửi
});

// Xử lý sự kiện cho input
var inputElement = document.getElementById(name_Variable[168]);
inputElement.addEventListener(name_Variable[42], function () {
    inputElement.value = inputElement.value.trim(); // Xóa khoảng trắng trong giá trị
});

// Xử lý sự kiện cho button tải form
document.getElementById(name_Variable[167]).addEventListener(name_Variable[184], function (event) {
    event.preventDefault(); // Ngăn chặn hành động mặc định

    if (loadAbilityForm === false) { // Nếu form chưa được tải
        var xhr = new XMLHttpRequest(); // Tạo đối tượng XMLHttpRequest
        xhr.onreadystatechange = function () {
            if (xhr.readyState === XMLHttpRequest.DONE) { // Kiểm tra trạng thái hoàn thành
                if (xhr.status === 200) { // Nếu thành công
                    document.getElementById(name_Variable[0]).innerHTML = xhr.responseText; // Cập nhật nội dung
                    loadAbilityForm = true; // Đánh dấu là đã tải form
                    abilityForm(); // Mở form khả năng
                } else {
                    console.error(name_Variable[222]); // Ghi lại lỗi vào console
                }
            }
        };

        var url = name_Variable[223] + name_Variable[224] + selectedLanguage; // Thiết lập URL
        xhr.open(name_Variable[80], url, true); // Mở kết nối
        xhr.send(); // Gửi yêu cầu
    } else {
        abilityForm(); // Nếu đã tải, mở form
    }
});

// Hàm lấy chỉ số đã chọn
function getSelectedIndex(selectElement) {
    return selectElement.selectedIndex; // Trả về chỉ số đã chọn
}