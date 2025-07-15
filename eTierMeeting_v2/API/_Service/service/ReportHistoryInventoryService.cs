using System.Drawing;
using Aspose.Cells;
using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Machine_API.Resources;

namespace Machine_API._Service.service
{
    public class ReportHistoryInventoryService : IReportHistoryInventoryService
    {
        private readonly LocalizationService _languageService;
        public ReportHistoryInventoryService(LocalizationService languageService)
        {
            _languageService = languageService;

        }

        public void CustomStyle(ref Cell cellCustom)
        {
            if (cellCustom.Value != null)
            {
                string value = cellCustom.Value.ToString();
                if (value == "1")
                {
                    cellCustom.PutValue(_languageService.GetLocalizedHtmlString("match"));
                    Style styleCustom = cellCustom.GetStyle();
                    styleCustom.Font.Color = Color.Blue;
                    styleCustom.Font.IsBold = true;
                    cellCustom.SetStyle(styleCustom);
                }
                else if (value == "-1")
                {
                    cellCustom.PutValue(_languageService.GetLocalizedHtmlString("wrong_position"));
                    Style styleCustom = cellCustom.GetStyle();
                    styleCustom.Font.Color = Color.Orange;
                    styleCustom.Font.IsBold = true;
                    cellCustom.SetStyle(styleCustom);
                }
                else
                {
                    cellCustom.PutValue(_languageService.GetLocalizedHtmlString("not_scan"));
                    Style styleCustom = cellCustom.GetStyle();
                    styleCustom.Font.Color = Color.Red;
                    styleCustom.Font.IsBold = true;
                    cellCustom.SetStyle(styleCustom);
                }
            }
        }

        public void PutStaticValue(ref Worksheet ws, ResultAllInventoryDto resultAll)
        {

            var dataStaticSokiem = resultAll.ListDetail.FirstOrDefault(x => x.TypeInventory == 1);
            var dataStaticPhuckiem = resultAll.ListDetail.FirstOrDefault(x => x.TypeInventory == 2);
            var dataStaticRutkiem = resultAll.ListDetail.FirstOrDefault(x => x.TypeInventory == 3);

            ws.Cells["A6"].PutValue(_languageService.GetLocalizedHtmlString("machine_code"));
            ws.Cells["B6"].PutValue(_languageService.GetLocalizedHtmlString("machine_name"));
            ws.Cells["C6"].PutValue(_languageService.GetLocalizedHtmlString("suppplier"));
            ws.Cells["D6"].PutValue(_languageService.GetLocalizedHtmlString("location"));
            // ws.Cells["E6"].PutValue(_languageService.GetLocalizedHtmlString("scan_location"));
            ws.Cells["E6"].PutValue(_languageService.GetLocalizedHtmlString("state"));
            ws.Cells["F6"].PutValue(_languageService.GetLocalizedHtmlString("sokiem"));
            ws.Cells["G6"].PutValue(_languageService.GetLocalizedHtmlString("phuc_kiem"));
            ws.Cells["H6"].PutValue(_languageService.GetLocalizedHtmlString("rut_kiem"));

            ws.Cells["E9"].PutValue(_languageService.GetLocalizedHtmlString("employee_code"));
            ws.Cells["E10"].PutValue(_languageService.GetLocalizedHtmlString("employee_name"));
            ws.Cells["E12"].PutValue(_languageService.GetLocalizedHtmlString("totalMachine"));
            ws.Cells["E13"].PutValue(_languageService.GetLocalizedHtmlString("match"));
            ws.Cells["E14"].PutValue(_languageService.GetLocalizedHtmlString("wrong_position"));
            ws.Cells["E15"].PutValue(_languageService.GetLocalizedHtmlString("not_scan"));
            ws.Cells["E17"].PutValue(_languageService.GetLocalizedHtmlString("percent_match"));
            ws.Cells["E19"].PutValue(_languageService.GetLocalizedHtmlString("recent_times"));
            ws.Cells["E20"].PutValue(_languageService.GetLocalizedHtmlString("startTimeInventory"));
            ws.Cells["E21"].PutValue(_languageService.GetLocalizedHtmlString("endTimeInventory"));

            if (dataStaticSokiem != null)
            {
                ws.Cells["F9"].PutValue(dataStaticSokiem.EmpNumber);
                ws.Cells["F10"].PutValue(dataStaticSokiem.EmpName);
                ws.Cells["F12"].PutValue(dataStaticSokiem.CountMachine);
                ws.Cells["F13"].PutValue(dataStaticSokiem.CountMatch);
                ws.Cells["F14"].PutValue(dataStaticSokiem.CountWrongPosition);
                ws.Cells["F15"].PutValue(dataStaticSokiem.CountNotScan);
                ws.Cells["F17"].PutValue(dataStaticSokiem.PercenMatch + "%");
                ws.Cells["F19"].PutValue(dataStaticSokiem.CreateTime == null ? "" : dataStaticSokiem.CreateTime.Value.ToString("yyyy/MM/dd"));
                ws.Cells["F20"].PutValue(dataStaticSokiem.DateStartInventory == null ? "" : dataStaticSokiem.DateStartInventory.Value.ToString("HH:mm"));
                ws.Cells["F21"].PutValue(dataStaticSokiem.DateEndInventory == null ? "" : dataStaticSokiem.DateEndInventory.Value.ToString("HH:mm"));
            }
            if (dataStaticPhuckiem != null)
            {
                ws.Cells["G9"].PutValue(dataStaticPhuckiem.EmpNumber);
                ws.Cells["G10"].PutValue(dataStaticPhuckiem.EmpName);
                ws.Cells["G12"].PutValue(dataStaticPhuckiem.CountMachine);
                ws.Cells["G13"].PutValue(dataStaticPhuckiem.CountMatch);
                ws.Cells["G14"].PutValue(dataStaticPhuckiem.CountWrongPosition);
                ws.Cells["G15"].PutValue(dataStaticPhuckiem.CountNotScan);
                ws.Cells["G17"].PutValue(dataStaticPhuckiem.PercenMatch + "%");
                ws.Cells["G19"].PutValue(dataStaticPhuckiem.CreateTime == null ? "" : dataStaticPhuckiem.CreateTime.Value.ToString("yyyy/MM/dd"));
                ws.Cells["G20"].PutValue(dataStaticPhuckiem.DateStartInventory == null ? "" : dataStaticPhuckiem.DateStartInventory.Value.ToString("HH:mm"));
                ws.Cells["G21"].PutValue(dataStaticPhuckiem.DateEndInventory == null ? "" : dataStaticPhuckiem.DateEndInventory.Value.ToString("HH:mm"));
            }
            if (dataStaticRutkiem != null)
            {
                ws.Cells["H9"].PutValue(dataStaticRutkiem.EmpNumber);
                ws.Cells["H10"].PutValue(dataStaticRutkiem.EmpName);
                ws.Cells["H12"].PutValue(dataStaticRutkiem.CountMachine);
                ws.Cells["H13"].PutValue(dataStaticRutkiem.CountMatch);
                ws.Cells["H14"].PutValue(dataStaticRutkiem.CountWrongPosition);
                ws.Cells["H15"].PutValue(dataStaticRutkiem.CountNotScan);
                ws.Cells["H17"].PutValue(dataStaticRutkiem.PercenMatch + "%");
                ws.Cells["H19"].PutValue(dataStaticRutkiem.CreateTime == null ? "" : dataStaticRutkiem.CreateTime.Value.ToString("yyyy/MM/dd"));
                ws.Cells["H20"].PutValue(dataStaticRutkiem.DateStartInventory == null ? "" : dataStaticRutkiem.DateStartInventory.Value.ToString("HH:mm"));
                ws.Cells["H21"].PutValue(dataStaticRutkiem.DateEndInventory == null ? "" : dataStaticRutkiem.DateEndInventory.Value.ToString("HH:mm"));
            }
        }

        public void PutStaticAllInventoryValue(ref Worksheet ws, ResultAllInventoryDto resultAll)
        {

            var dataStaticSokiem = resultAll.ListDetail.FirstOrDefault(x => x.TypeInventory == 1);
            var dataStaticPhuckiem = resultAll.ListDetail.FirstOrDefault(x => x.TypeInventory == 2);
            var dataStaticRutkiem = resultAll.ListDetail.FirstOrDefault(x => x.TypeInventory == 3);


            ws.Cells["A3"].PutValue(_languageService.GetLocalizedHtmlString("machine_code"));
            ws.Cells["B3"].PutValue(_languageService.GetLocalizedHtmlString("machine_name"));
            ws.Cells["C3"].PutValue(_languageService.GetLocalizedHtmlString("suppplier"));
            ws.Cells["D3"].PutValue(_languageService.GetLocalizedHtmlString("location"));
            // ws.Cells["E3"].PutValue(_languageService.GetLocalizedHtmlString("scan_location"));
            ws.Cells["E3"].PutValue(_languageService.GetLocalizedHtmlString("state"));
            ws.Cells["F3"].PutValue(_languageService.GetLocalizedHtmlString("sokiem"));
            ws.Cells["G3"].PutValue(_languageService.GetLocalizedHtmlString("phuc_kiem"));
            ws.Cells["H3"].PutValue(_languageService.GetLocalizedHtmlString("rut_kiem"));
            ws.Cells["I3"].PutValue(_languageService.GetLocalizedHtmlString("remark"));

            ws.Cells["A8"].PutValue(_languageService.GetLocalizedHtmlString("totalMachine"));
            ws.Cells["A9"].PutValue(_languageService.GetLocalizedHtmlString("match"));
            ws.Cells["A10"].PutValue(_languageService.GetLocalizedHtmlString("wrong_position"));
            ws.Cells["A11"].PutValue(_languageService.GetLocalizedHtmlString("not_found_in_system"));
            ws.Cells["A12"].PutValue(_languageService.GetLocalizedHtmlString("not_scan"));
            ws.Cells["A13"].PutValue(_languageService.GetLocalizedHtmlString("percent_match"));
            ws.Cells["B7"].PutValue(_languageService.GetLocalizedHtmlString("sokiem"));
            ws.Cells["C7"].PutValue(_languageService.GetLocalizedHtmlString("phuc_kiem"));

            if (dataStaticSokiem != null)
            {
                var percentAccurateSokiem = ((decimal)dataStaticSokiem.CountMachine != 0 ? ((decimal)dataStaticSokiem.CountMatch / (decimal)dataStaticSokiem.CountMachine) * 100 : 0).ToString("0.##");

                ws.Cells["B8"].PutValue(dataStaticSokiem.CountMachine);
                ws.Cells["B9"].PutValue(dataStaticSokiem.CountMatch);
                ws.Cells["B10"].PutValue(dataStaticSokiem.CountWrongPosition);
                ws.Cells["B11"].PutValue("");
                ws.Cells["B12"].PutValue(dataStaticSokiem.CountNotScan);
                ws.Cells["B13"].PutValue(percentAccurateSokiem + "%");
            }
            if (dataStaticPhuckiem != null)
            {
                var percentAccuratePhuckiem = ((decimal)dataStaticPhuckiem.CountMachine != 0 ? ((decimal)dataStaticPhuckiem.CountMatch / (decimal)dataStaticPhuckiem.CountMachine) * 100 : 0).ToString("0.##");

                ws.Cells["C8"].PutValue(dataStaticPhuckiem.CountMachine);
                ws.Cells["C9"].PutValue(dataStaticPhuckiem.CountMatch);
                ws.Cells["C10"].PutValue(dataStaticPhuckiem.CountWrongPosition);
                ws.Cells["C11"].PutValue("");
                ws.Cells["C12"].PutValue(dataStaticPhuckiem.CountNotScan);
                ws.Cells["C13"].PutValue(percentAccuratePhuckiem + "%");
            }
        }
    }
}