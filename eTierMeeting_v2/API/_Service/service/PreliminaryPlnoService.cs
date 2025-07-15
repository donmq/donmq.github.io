using AutoMapper;
using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Machine_API.Helpers.Params;
using Machine_API.Helpers.Utilities;
using Machine_API.Models.MachineCheckList;
using Microsoft.EntityFrameworkCore;
using Aspose.Cells;
using System.Drawing;
using Machine_API._Accessor;
using Machine_API.Resources;

namespace Machine_API._Service.service
{
    public class PreliminaryPlnoService : IPreliminaryPlnoService
    {
        private readonly IMachineRepositoryAccessor _repository;
        private readonly LocalizationService _languageService;

        public PreliminaryPlnoService(
            IMachineRepositoryAccessor repository,
            LocalizationService languageService)
        {
            _repository = repository;
            _languageService = languageService;
        }

        public async Task<List<PreliminaryPlnoExportDTO>> ExportExcel(string search)
        {
            var userList = _repository.User.FindAll();
            var preliminaryPlno = _repository.PreliminaryPlno.FindAll(x => x.Active == true);
            var allBuilding = _repository.Building.FindAll(x => x.Visible == true);
            var allCell = _repository.Cells.FindAll(x => x.Visible == true);
            var allPlno = _repository.Cell_Plno.FindAll();
            var allHp_a15 = _repository.hp_a15.FindAll();

            var userPreliminaryPlno = await preliminaryPlno.Join(userList, x => x.EmpNumber, y => y.UserName, (x, y) => new
            {
                EmpNumber = y.UserName,
                EmpName = y.EmpName,
                UpdateBy = y.UpdateBy,
                UpdateTime = y.UpdateDate,
            }).Distinct().ToListAsync();

            List<PreliminaryPlnoDTO> preliminaryPlnoList = new List<PreliminaryPlnoDTO>();
            foreach (var item in userPreliminaryPlno)
            {
                PreliminaryPlnoDTO preNew = new PreliminaryPlnoDTO();
                preNew.EmpName = item.EmpName;
                preNew.EmpNumber = item.EmpNumber;
                preNew.UpdateBy = item.UpdateBy;
                preNew.UpdateTime = item.UpdateTime;

                var preliminaryPlno_Buiding = _repository.PreliminaryPlno.FindAll(x => x.EmpNumber == item.EmpNumber && x.Active == true)
                .Join(allBuilding, x => x.BuildingID, y => y.BuildingID, (x, y) => new BuildingDto
                {
                    BuildingID = x.BuildingID,
                    BuildingCode = y.BuildingCode,
                    BuildingName = y.BuildingName.Trim() + " (" + y.BuildingCode + ")",
                    Visible = y.Visible,
                }).Distinct().OrderBy(x => x.BuildingCode).ToList();
                preNew.ListBuilding = preliminaryPlno_Buiding;

                var preliminaryPlno_Cell = _repository.PreliminaryPlno.FindAll(x => x.EmpNumber == item.EmpNumber && x.Active == true)
                .Join(allCell, x => x.CellID, y => y.CellID, (x, y) => new CellDto
                {
                    //CellID = x.CellID.ToInt(),
                    CellName = y.CellCode + "-" + y.CellName,
                    CellCode = y.CellCode,
                    Visible = y.Visible,
                }).Distinct().ToList();
                preNew.ListCell = preliminaryPlno_Cell;

                var preliminaryPlno_CellPlno = _repository.PreliminaryPlno.FindAll(x => x.EmpNumber == item.EmpNumber && x.Active == true)
                .Join(allBuilding, x => x.BuildingID, y => y.BuildingID, (x, y) => new
                {
                    Plno = x.Plno,
                    BuildingID = x.BuildingID,
                    BuildingCode = y.BuildingCode,
                    CellID = x.CellID,
                    CellCode = x.CellCode
                })
                .Join(allPlno, x => x.Plno, y => y.Plno, (x, y) => new
                {
                    x.Plno,
                    x.BuildingID,
                    x.BuildingCode,
                    x.CellID,
                    x.CellCode

                })
                .Join(allHp_a15, x => x.Plno, y => y.Plno, (x, y) => new
                {
                    Plno = y.Plno,
                    PlnoCode = y.Plno + "_" + x.BuildingID.ToString() + (x.CellID.ToString() == "0" ? "" : "_" + x.CellID.ToString()) + (x.CellCode == null ? "" : "_" + x.CellCode.ToString()),
                    Place = y.Plno + "-" + y.Place.Trim() + " (" + x.BuildingCode + ")",
                    BuildingCode = x.BuildingCode,

                }).OrderBy(x => x.BuildingCode).Select(x => new Hp_a15Dto
                {
                    Plno = x.Plno,
                    PlnoCode = x.PlnoCode,
                    BuildingCode = x.BuildingCode,
                    Place = x.Place,
                }).Distinct().ToList();
                preNew.ListHpA15 = preliminaryPlno_CellPlno;

                preliminaryPlnoList.Add(preNew);
            }
            if (!string.IsNullOrEmpty(search))
                preliminaryPlnoList = preliminaryPlnoList.Where(x =>
                   x.EmpNumber.ToLower().Contains(search.ToLower())
                || x.EmpName.ToLower().NonUnicode().Contains(search.ToLower())
                || x.ListBuilding.Any(w => w.BuildingCode.ToLower().Contains(search.ToLower()) || w.BuildingName.ToLower().Contains(search.ToLower()))
                || x.ListCell.Any(w => w.CellName.ToLower().Contains(search.ToLower()) || w.CellCode.ToLower().Contains(search.ToLower()))
                || x.ListHpA15.Any(w => w.Plno.ToLower().Contains(search.ToLower()) || w.Place.ToLower().Contains(search.ToLower()))
                || x.EmpName.ToLower().Contains(search.ToLower())).OrderBy(x => x.EmpNumber).ToList();


            List<PreliminaryPlnoExportDTO> preliminaryPlnoExportList = new List<PreliminaryPlnoExportDTO>();
            foreach (var item in preliminaryPlnoList)
            {
                PreliminaryPlnoExportDTO newItem = new PreliminaryPlnoExportDTO();
                newItem.EmpName = item.EmpName;
                newItem.EmpNumber = item.EmpNumber;
                newItem.UpdateBy = item.UpdateBy;
                newItem.UpdateTime = item.UpdateTime;

                newItem.ListBuilding = string.Join(" || ", item.ListBuilding.Select(y => y.BuildingName.Trim()));
                newItem.ListCell = string.Join(" || ", item.ListCell.Select(y => y.CellName.Trim()));
                newItem.ListHpA15 = string.Join(" || ", item.ListHpA15.Select(y => y.Place.Trim()));
                preliminaryPlnoExportList.Add(newItem);

            }
            return await Task.FromResult(preliminaryPlnoExportList.ToList());
        }

        public async Task<List<PreliminaryPlnoExportAllDTO>> ExportPreminaryInLocationBuildingExcel()
        {
            var userList = _repository.User.FindAll();
            var preliminaryPlno = _repository.PreliminaryPlno.FindAll(x => x.Active == true);
            var allBuilding = _repository.Building.FindAll(x => x.Visible == true);
            var allCell = _repository.Cells.FindAll(x => x.Visible == true);
            var allPlno = _repository.Cell_Plno.FindAll();
            var allHp_a15 = _repository.hp_a15.FindAll();
            var pDC = _repository.PDC.FindAll();


            //Query data
            var queryListPreliminary = await preliminaryPlno.GroupJoin(userList,
                    x => x.EmpNumber,
                    y => y.UserName,
                (x, y) => new { T1 = x, T2 = y })
                .SelectMany(x => x.T2.DefaultIfEmpty(), (x, y) => new { x.T1, T2 = y })
                .GroupJoin(allCell,
                    x => x.T1.CellID,
                    y => y.CellID,
                (x, y) => new { x.T1, x.T2, T3 = y })
                .SelectMany(x => x.T3.DefaultIfEmpty(), (x, y) => new { x.T1, x.T2, T3 = y })
                .GroupJoin(allPlno,
                    x => x.T1.CellID,
                    y => y.CellID,
                (x, y) => new { x.T1, x.T2, x.T3, T4 = y })
                .SelectMany(x => x.T4.DefaultIfEmpty(), (x, y) => new { x.T1, x.T2, x.T3, T4 = y })
                .GroupJoin(allHp_a15,
                    x => x.T1.Plno,
                    y => y.Plno,
                (x, y) => new { x.T1, x.T2, x.T3, x.T4, T5 = y })
                .SelectMany(x => x.T5.DefaultIfEmpty(), (x, y) => new { x.T1, x.T2, x.T3, x.T4, T5 = y })
                .GroupJoin(allBuilding,
                    x => x.T1.BuildingID,
                    y => y.BuildingID,
                (x, y) => new { x.T1, x.T2, x.T3, x.T4, x.T5, T6 = y })
                .SelectMany(x => x.T6.DefaultIfEmpty(), (x, y) => new { x.T1, x.T2, x.T3, x.T4, x.T5, T6 = y })
                .GroupJoin(pDC,
                    x => x.T3.PDCID,
                    y => y.PDCID,
                    (x, y) => new { x.T1, x.T2, x.T3, x.T4, x.T5, x.T6, T7 = y }
                )
                .SelectMany(x => x.T7.DefaultIfEmpty(), (x, y) => new PreliminaryPlnoExportAllDTO
                {
                    EmpNumber = x.T1.EmpNumber,
                    EmpName = x.T2.EmpName,
                    Place = x.T5.Place,
                    BuildingName = x.T6.BuildingName.Substring(0, 2),
                    BuildingID = x.T6.BuildingID,
                    BuildingCode = x.T6.BuildingCode,
                    PDCName = y.PDCName,
                    Plno = x.T1.Plno
                }).Where(x => x.BuildingCode != "BKH").OrderBy(x => x.BuildingName).ThenBy(x => x.Place).ToListAsync();

            //GroupBy BuildingCode
            var queryListBuildingCode = queryListPreliminary
                .GroupBy(x => new { x.BuildingCode, x.BuildingName })
                .Select(x => x.Key)
                .ToList();

            var results = new List<PreliminaryPlnoExportAllDTO>();
            foreach (var itemBuild in queryListBuildingCode)
            {
                var preliminaries = queryListPreliminary.Where(x => x.BuildingCode == itemBuild.BuildingCode).ToList();
                var plnoes = preliminaries
                    .GroupBy(x => new { x.Plno, x.Place })
                    .Select(x => x.Key)
                    .ToList();

                var preliminaryPlnoItem = new PreliminaryPlnoExportAllDTO
                {
                    BuildingName = itemBuild.BuildingName,
                    ListHpA15 = new List<Hp_a15Dto>()
                };

                foreach (var plno in plnoes)
                {
                    var emps = preliminaries.Where(x => x.Plno == plno.Plno).ToList();

                    var hpA15 = new Hp_a15Dto
                    {
                        Plno = plno.Plno,
                        Place = plno.Place,
                        ListUser = new List<UserDto>()
                    };

                    foreach (var item in emps)
                    {
                        var user = new UserDto();
                        user.UserName = item.EmpNumber;
                        user.EmpName = item.EmpName;

                        hpA15.ListUser.Add(user);
                    }

                    preliminaryPlnoItem.ListHpA15.Add(hpA15);
                }
                results.Add(preliminaryPlnoItem);
            }

            return results;
        }

        public async Task<List<PreliminaryPlnoExportAllDTO>> ExportPreminaryOtherLocationBuildingExcel()
        {

            var userList = _repository.User.FindAll();
            var preliminaryPlno = _repository.PreliminaryPlno.FindAll(x => x.Active == true);
            var allBuilding = _repository.Building.FindAll(x => x.Visible == true);
            var allCell = _repository.Cells.FindAll(x => x.Visible == true);
            var allPlno = _repository.Cell_Plno.FindAll();
            var allHp_a15 = _repository.hp_a15.FindAll();
            var pDC = _repository.PDC.FindAll();

            var buildingName = await preliminaryPlno.GroupJoin(allCell,
                x => new { CellID = x.CellID.Value, x.CellCode },
                y => new { y.CellID, y.CellCode },
                (x, y) => new { T1 = x, T2 = y })
                .SelectMany(x => x.T2.DefaultIfEmpty(), (x, y) => new { x.T1, T2 = y })
            .GroupJoin(allBuilding,
                x => x.T2.BuildingID,
                y => y.BuildingID,
                (x, y) => new { x.T1, x.T2, T3 = y }
            ).SelectMany(x => x.T3.DefaultIfEmpty(), (x, y) => new { x.T1, x.T2, T3 = y }).Select(x => new
            {
                x.T1.CellID,
                x.T1.CellCode,
                x.T2.CellName,
                x.T1.BuildingID,
                x.T3.BuildingName
            }).Distinct().ToListAsync();

            var queryListPreliminary = await preliminaryPlno.GroupJoin(allBuilding,
                x => x.BuildingID,
                y => y.BuildingID,
                (x, y) => new { T1 = x, T2 = y }
            ).SelectMany(x => x.T2.DefaultIfEmpty(), (x, y) => new { x.T1, T2 = y })
            .GroupJoin(allCell,
                x => x.T1.CellID,
                y => y.CellID,
                (x, y) => new { x.T1, x.T2, T3 = y }
            ).SelectMany(x => x.T3.DefaultIfEmpty(), (x, y) => new { x.T1, x.T2, T3 = y })
            .GroupJoin(allPlno,
                x => x.T1.Plno,
                y => y.Plno,
                (x, y) => new { x.T1, x.T2, x.T3, T4 = y }
            ).SelectMany(x => x.T4.DefaultIfEmpty(), (x, y) => new { x.T1, x.T2, x.T3, T4 = y })
            .GroupJoin(allHp_a15,
                x => x.T1.Plno,
                y => y.Plno,
                (x, y) => new { x.T1, x.T2, x.T3, x.T4, T5 = y }
            ).SelectMany(x => x.T5.DefaultIfEmpty(), (x, y) => new { x.T1, x.T2, x.T3, x.T4, T5 = y })
            .GroupJoin(userList,
                x => x.T1.EmpNumber,
                y => y.UserName,
                (x, y) => new { x.T1, x.T2, x.T3, x.T4, x.T5, T6 = y }
            ).SelectMany(x => x.T6.DefaultIfEmpty(), (x, y) => new { x.T1, x.T2, x.T3, x.T4, x.T5, T6 = y })
            .Select(x => new PreliminaryPlnoExportAllDTO
            {
                CellCode = x.T1.CellCode,
                CellID = x.T1.CellID,
                CellName = x.T3.CellName,
                BuildingName = "",
                BuildingID = x.T2.BuildingID,
                Place = x.T5.Place,
                Plno = x.T4.Plno,
                EmpNumber = x.T1.EmpNumber,
                EmpName = x.T6.EmpName,
                //Add
                Is_Manager = x.T1.Is_Manager,
                Is_Preliminary = x.T1.Is_Preliminary

            }).Where(x => x.BuildingID == 100 && x.CellCode != null && x.Is_Manager != null)
            .OrderBy(x => x.CellName)
            .ThenBy(x => x.Plno)
            .ThenBy(x => x.Place)
            .ToListAsync();

            foreach (var item in queryListPreliminary)
            {
                var building = buildingName.Where(x => x.CellCode == item.CellCode).FirstOrDefault();
                if (building != null)
                {
                    item.BuildingName = building.BuildingName;
                }
            }

            //GroupBy BuildingCode
            var queryListBuildingCode = queryListPreliminary
                .GroupBy(x => new { x.CellCode, x.CellName, x.BuildingName })
                .Select(x => x.Key)
                .ToList();

            var queryListManager = queryListPreliminary.Where(x => x.Is_Manager == true);
            var queryListNotIsManager = queryListPreliminary.Where(x => x.Is_Preliminary == true);
            var results = new List<PreliminaryPlnoExportAllDTO>();
            foreach (var itemCell in queryListBuildingCode)
            {
                var preliminaries = queryListPreliminary.Where(x => x.CellCode == itemCell.CellCode).ToList();
                var plnoes = preliminaries
                    .GroupBy(x => new { x.Plno, x.Place, x.CellCode })
                    .Select(x => x.Key)
                    .ToList();

                var preliminaryPlnoItem = new PreliminaryPlnoExportAllDTO
                {
                    BuildingName = itemCell.BuildingName,
                    CellName = itemCell.CellName,
                    ListHpA15 = new List<Hp_a15Dto>(),
                };

                foreach (var plno in plnoes)
                {
                    var preliminaryListManager = queryListManager
                    .Where(x => x.Plno == plno.Plno && x.Is_Manager != null)
                    .ToList(); //List manager 
                    var emps = preliminaries.Where(x => x.Plno == plno.Plno).ToList();
                    var preliminaryListUser = queryListNotIsManager
                    .Where(x => x.Plno == plno.Plno && x.Is_Preliminary != null)
                    .ToList(); // List user 
                    var hpA15 = new Hp_a15Dto
                    {
                        Plno = plno.Plno,
                        Place = plno.Place,
                        CellCode = plno.CellCode,
                        ListUserManager = new List<UserDto>(),
                        ListUser = new List<UserDto>()
                    };
                    foreach (var item in preliminaryListManager)
                    {
                        var userListManager = new UserDto();
                        userListManager.UserName = item.EmpNumber;
                        userListManager.EmpName = item.EmpName;
                        hpA15.ListUserManager.Add(userListManager);
                    }

                    //foreach (var item in emps)
                    foreach (var item in preliminaryListUser)
                    {
                        var user = new UserDto();
                        user.UserName = item.EmpNumber;
                        user.EmpName = item.EmpName;

                        hpA15.ListUser.Add(user);
                    }

                    preliminaryPlnoItem.ListHpA15.Add(hpA15);
                }
                results.Add(preliminaryPlnoItem);
            }
            return results;

        }


        public void PutStaticAllInBuildingValue(WorkbookDesigner designer, ref Worksheet wsheet, List<PreliminaryPlnoExportAllDTO> resultAll)
        {
            Aspose.Cells.Cells cells = wsheet.Cells;
            int indexBuilding = 4;
            int indexHp = 4;
            int indexUser = 4;

            wsheet.Cells["A3"].PutValue(_languageService.GetLocalizedHtmlString("factory-area"));
            wsheet.Cells["B3"].PutValue(_languageService.GetLocalizedHtmlString("custody_department"));
            wsheet.Cells["C3"].PutValue(_languageService.GetLocalizedHtmlString("inventory_unit"));
            wsheet.Cells["D3"].PutValue(_languageService.GetLocalizedHtmlString("employee_code"));
            wsheet.Cells["E3"].PutValue(_languageService.GetLocalizedHtmlString("curator_inventory"));

            foreach (var item in resultAll)
            {
                wsheet.Cells["A" + indexBuilding].PutValue(item.BuildingName);
                wsheet.Cells["B" + indexBuilding].PutValue(item.Place);
                var totalUsersByBuilding = 0;

                foreach (var hp in item.ListHpA15)
                {
                    wsheet.Cells["C" + indexHp].PutValue(hp.Place);
                    var hpRange = wsheet.Cells.CreateRange($"C{indexHp}:C{indexHp + hp.ListUser.Count() - 1}");
                    hpRange.Merge();
                    // Add list user
                    foreach (var user in hp.ListUser)
                    {
                        // list user = 8
                        wsheet.Cells["D" + indexUser].PutValue(user.UserName);
                        wsheet.Cells["E" + indexUser].PutValue(user.EmpName);

                        indexUser++;
                    }

                    totalUsersByBuilding += hp.ListUser.Count();
                    indexHp += hp.ListUser.Count();
                }

                var buildingRange = wsheet.Cells.CreateRange($"A{indexBuilding}:A{indexBuilding + totalUsersByBuilding - 1}");
                buildingRange.Merge();
                var pDCRage = wsheet.Cells.CreateRange($"B{indexBuilding}:B{indexBuilding + totalUsersByBuilding - 1}");
                pDCRage.Merge();
                indexBuilding = indexHp;
            }
            indexBuilding--;
            Style borderStyle = designer.Workbook.CreateStyle();
            Style styleColADE = designer.Workbook.CreateStyle();
            //Set Style
            borderStyle.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            borderStyle.Borders[BorderType.TopBorder].Color = Color.Black;
            borderStyle.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            borderStyle.Borders[BorderType.BottomBorder].Color = Color.Black;
            borderStyle.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            borderStyle.Borders[BorderType.LeftBorder].Color = Color.Black;
            borderStyle.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            borderStyle.Borders[BorderType.RightBorder].Color = Color.Black;
            borderStyle.VerticalAlignment = TextAlignmentType.Center;
            borderStyle.Font.Name = "Calibri";
            borderStyle.Font.Size = 12;
            //Set Style Colum ADE
            styleColADE.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            styleColADE.Borders[BorderType.TopBorder].Color = Color.Black;
            styleColADE.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            styleColADE.Borders[BorderType.BottomBorder].Color = Color.Black;
            styleColADE.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            styleColADE.Borders[BorderType.LeftBorder].Color = Color.Black;
            styleColADE.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            styleColADE.Borders[BorderType.RightBorder].Color = Color.Black;
            styleColADE.Font.Name = "Calibri";
            styleColADE.Font.Size = 12;
            styleColADE.HorizontalAlignment = TextAlignmentType.Center;
            styleColADE.VerticalAlignment = TextAlignmentType.Center;

            //Creat range
            var borderRange = wsheet.Cells.CreateRange($"A4:E{indexBuilding}");
            var colA = wsheet.Cells.CreateRange($"A4:A{indexBuilding}");
            var colDE = wsheet.Cells.CreateRange($"D4:E{indexBuilding}");

            //Apply Style
            borderRange.ApplyStyle(borderStyle, new Aspose.Cells.StyleFlag() { All = true, Borders = true });
            colA.ApplyStyle(styleColADE, new Aspose.Cells.StyleFlag() { All = true, Borders = true });
            colDE.ApplyStyle(styleColADE, new Aspose.Cells.StyleFlag() { All = true, Borders = true });

            wsheet.AutoFitColumns();
        }


        public void PutStaticAllOtherBuildingValue(WorkbookDesigner designer, ref Worksheet wsheet, List<PreliminaryPlnoExportAllDTO> resultAll)
        {
            int indexBuilding = 4;
            int indexHp = 4;
            int indexUser = 4;
            int indexUserManager = 4;

            int totalManager = 0;
            int totalPreliminary = 0;

            Style borderStyle = designer.Workbook.CreateStyle();
            Style styleA = designer.Workbook.CreateStyle();
            Style styleColBCDFG = designer.Workbook.CreateStyle();
            //var styleColor = new Style();

            wsheet.Cells["A3"].PutValue(_languageService.GetLocalizedHtmlString("factory-area"));
            wsheet.Cells["B3"].PutValue(_languageService.GetLocalizedHtmlString("custody_department"));
            wsheet.Cells["C3"].PutValue(_languageService.GetLocalizedHtmlString("manager_code"));
            wsheet.Cells["D3"].PutValue(_languageService.GetLocalizedHtmlString("supervisor_name"));
            wsheet.Cells["E3"].PutValue(_languageService.GetLocalizedHtmlString("inventory_unit"));
            wsheet.Cells["F3"].PutValue(_languageService.GetLocalizedHtmlString("employee_code"));
            wsheet.Cells["G3"].PutValue(_languageService.GetLocalizedHtmlString("curator_inventory"));
            wsheet.Cells["H3"].PutValue(_languageService.GetLocalizedHtmlString("number_of_manager"));
            wsheet.Cells["I3"].PutValue(_languageService.GetLocalizedHtmlString("restorer_name"));

            foreach (var item in resultAll)
            {
                int mergeCount = 0;
                int mergeCustodyDepartment = 0;
                wsheet.Cells["A" + indexBuilding].PutValue(item.CellName);

                foreach (var hp in item.ListHpA15)
                {
                    totalPreliminary = hp.ListUser.Count();
                    totalManager = hp.ListUserManager.Count();

                    if (Math.Max(totalPreliminary, totalManager) == 0)
                    {
                        mergeCount++;
                    }
                    else
                    {
                        mergeCount = mergeCount + Math.Max(totalPreliminary, totalManager);
                        mergeCustodyDepartment = mergeCustodyDepartment + Math.Max(totalPreliminary, totalManager);
                    }

                    var colIndex = 0;
                    if (Math.Max(totalPreliminary, totalManager) >= 2)
                        colIndex = Math.Max(totalPreliminary, totalManager) - 1;

                    wsheet.Cells["B" + indexHp].PutValue(item.BuildingName);
                    wsheet.Cells["E" + indexHp].PutValue(hp.Plno?.Trim() + '-' + hp.Place?.Trim() + '(' + hp.CellCode?.Trim() + ')');

                    var hpRange = wsheet.Cells.CreateRange($"E{indexHp}:E{indexHp + colIndex}");
                    hpRange.Merge();

                    //Add List User Manager
                    foreach (var user in hp.ListUserManager)
                    {
                        wsheet.Cells["C" + indexUserManager].PutValue(user.UserName);
                        wsheet.Cells["D" + indexUserManager].PutValue(user.EmpName);
                        indexUserManager++;
                    }

                    //Add List User 
                    foreach (var user in hp.ListUser)
                    {
                        wsheet.Cells["F" + indexUser].PutValue(user.UserName);
                        wsheet.Cells["G" + indexUser].PutValue(user.EmpName);

                        indexUser++;
                    }

                    // If list user and manager have amount = 0 increase 1 row 
                    if (hp.ListUserManager.Count() == 0)
                        indexUserManager++;
                    if (hp.ListUser.Count() == 0)
                        indexUser++;


                    indexHp = Math.Max(totalManager, totalPreliminary) == 0 ? indexHp + 1 : indexHp + Math.Max(totalManager, totalPreliminary);
                    indexUserManager = indexUser = Math.Max(indexUserManager, indexUser);
                }

                mergeCount--;
                if (mergeCustodyDepartment > 0)
                    mergeCustodyDepartment--;
                //col B
                var pDCRage = wsheet.Cells.CreateRange($"B{indexBuilding}:B{indexBuilding + (mergeCustodyDepartment)}");
                pDCRage.Merge();

                mergeCustodyDepartment = 0;

                //col A
                var buildingRange = wsheet.Cells.CreateRange($"A{indexBuilding}:A{indexBuilding + (mergeCount)}");
                buildingRange.Merge();

                mergeCount = 0;
                indexBuilding = indexHp;

            }
            indexBuilding--;

            //Set Style
            borderStyle.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            borderStyle.Borders[BorderType.TopBorder].Color = Color.Black;
            borderStyle.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            borderStyle.Borders[BorderType.BottomBorder].Color = Color.Black;
            borderStyle.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            borderStyle.Borders[BorderType.LeftBorder].Color = Color.Black;
            borderStyle.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            borderStyle.Borders[BorderType.RightBorder].Color = Color.Black;
            borderStyle.VerticalAlignment = TextAlignmentType.Center;
            borderStyle.Font.Name = "Calibri";
            borderStyle.Font.Size = 12;

            styleColBCDFG.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            styleColBCDFG.Borders[BorderType.TopBorder].Color = Color.Black;
            styleColBCDFG.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            styleColBCDFG.Borders[BorderType.BottomBorder].Color = Color.Black;
            styleColBCDFG.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            styleColBCDFG.Borders[BorderType.LeftBorder].Color = Color.Black;
            styleColBCDFG.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            styleColBCDFG.Borders[BorderType.RightBorder].Color = Color.Black;
            styleColBCDFG.Font.Name = "Calibri";
            styleColBCDFG.Font.Size = 12;
            styleColBCDFG.HorizontalAlignment = TextAlignmentType.Center;
            styleColBCDFG.VerticalAlignment = TextAlignmentType.Center;

            styleA.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            styleA.Borders[BorderType.TopBorder].Color = Color.Black;
            styleA.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            styleA.Borders[BorderType.BottomBorder].Color = Color.Black;
            styleA.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            styleA.Borders[BorderType.LeftBorder].Color = Color.Black;
            styleA.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            styleA.Borders[BorderType.RightBorder].Color = Color.Black;
            styleA.Font.Name = "Calibri";
            styleA.Font.Size = 12;
            styleA.VerticalAlignment = TextAlignmentType.Top;

            // //Creat range
            var borderRange = wsheet.Cells.CreateRange($"A4:I{indexBuilding}");
            var colA = wsheet.Cells.CreateRange($"A4:A{indexBuilding}");
            var colBD = wsheet.Cells.CreateRange($"B4:D{indexBuilding}");
            var colFG = wsheet.Cells.CreateRange($"F4:G{indexBuilding}");

            // //Apply Style
            borderRange.ApplyStyle(borderStyle, new Aspose.Cells.StyleFlag() { All = true, Borders = true });
            colA.ApplyStyle(styleA, new Aspose.Cells.StyleFlag() { All = true, Borders = true });
            colBD.ApplyStyle(styleColBCDFG, new Aspose.Cells.StyleFlag() { All = true, Borders = true });
            colFG.ApplyStyle(styleColBCDFG, new Aspose.Cells.StyleFlag() { All = true, Borders = true });

            wsheet.AutoFitColumns();
        }
        public async Task<PageListUtility<PreliminaryPlnoDTO>> GetAllPreliminaryPlno(PaginationParams pagination, string search)
        {
            var userList = _repository.User.FindAll();
            var preliminaryPlno = _repository.PreliminaryPlno.FindAll(x => x.Active == true).Distinct();
            var allBuilding = _repository.Building.FindAll(x => x.Visible == true);
            var allCell = _repository.Cells.FindAll(x => x.Visible == true);
            var allPlno = _repository.Cell_Plno.FindAll();
            var allHp_a15 = _repository.hp_a15.FindAll();

            var userPreliminaryPlno = await preliminaryPlno.Join(userList, x => x.EmpNumber, y => y.UserName, (x, y) => new
            {
                EmpNumber = y.UserName,
                EmpName = y.EmpName,
                UpdateBy = y.UpdateBy,
                UpdateTime = y.UpdateDate,
                Is_Manager = x.Is_Manager,
                Is_Preliminary = x.Is_Preliminary,
            }).Distinct().ToListAsync();

            List<PreliminaryPlnoDTO> preliminaryPlnoList = new List<PreliminaryPlnoDTO>();
            foreach (var item in userPreliminaryPlno)
            {
                PreliminaryPlnoDTO preNew = new PreliminaryPlnoDTO();
                preNew.EmpName = item.EmpName;
                preNew.EmpNumber = item.EmpNumber;
                preNew.UpdateBy = item.UpdateBy;
                preNew.UpdateTime = item.UpdateTime;
                preNew.Is_Manager = item.Is_Manager;
                preNew.Is_Preliminary = item.Is_Preliminary;


                var preliminaryPlno_Buiding = _repository.PreliminaryPlno.FindAll(x => x.EmpNumber == item.EmpNumber && x.Active == true)
                .Join(allBuilding, x => x.BuildingID, y => y.BuildingID, (x, y) => new BuildingDto
                {
                    BuildingID = x.BuildingID,
                    BuildingCode = y.BuildingCode,
                    BuildingName = y.BuildingName.Trim() + " (" + y.BuildingCode + ")",
                    Visible = y.Visible,
                }).Distinct().OrderBy(x => x.BuildingCode).ToList();
                preNew.ListBuilding = preliminaryPlno_Buiding;

                var preliminaryPlno_Cell = _repository.PreliminaryPlno.FindAll(x => x.EmpNumber == item.EmpNumber && x.Active == true)
                .Join(allCell, x => x.CellID, y => y.CellID, (x, y) => new CellDto
                {
                    //CellID = x.CellID.ToInt(),
                    CellName = y.CellCode + "-" + y.CellName,
                    CellCode = y.CellCode,
                    Visible = y.Visible,
                }).Distinct().ToList();
                preNew.ListCell = preliminaryPlno_Cell;

                var preliminaryPlno_CellPlno = _repository.PreliminaryPlno.FindAll(x => x.EmpNumber == item.EmpNumber && x.Active == true)
                .Join(allBuilding, x => x.BuildingID, y => y.BuildingID, (x, y) => new
                {
                    Plno = x.Plno,
                    BuildingID = x.BuildingID,
                    BuildingCode = y.BuildingCode,
                    CellID = x.CellID,
                    CellCode = x.CellCode,

                })
                .Join(allPlno, x => x.Plno, y => y.Plno, (x, y) => new
                {
                    x.Plno,
                    x.BuildingID,
                    x.BuildingCode,
                    x.CellID,
                    x.CellCode

                })
                .Join(allHp_a15, x => x.Plno, y => y.Plno, (x, y) => new
                {
                    Plno = y.Plno,
                    PlnoCode = y.Plno + "_" + x.BuildingID.ToString() + (x.CellID.ToString() == "0" ? "" : "_" + x.CellID.ToString()) + (x.CellCode == null ? "" : "_" + x.CellCode.ToString()),
                    Place = y.Plno + "-" + y.Place.Trim() + " (" + x.BuildingCode + ")",
                    BuildingCode = x.BuildingCode,

                }).OrderBy(x => x.BuildingCode).Select(x => new Hp_a15Dto
                {
                    Plno = x.Plno,
                    PlnoCode = x.PlnoCode,
                    BuildingCode = x.BuildingCode,
                    Place = x.Place,
                }).Distinct().ToList();
                preNew.ListHpA15 = preliminaryPlno_CellPlno;


                preliminaryPlnoList.Add(preNew);
            }
            if (!string.IsNullOrEmpty(search))
                preliminaryPlnoList = preliminaryPlnoList.Where(x =>
                   x.EmpNumber.ToLower().Contains(search.ToLower())
                || x.EmpName.ToLower().NonUnicode().Contains(search.ToLower())
                || x.ListBuilding.Any(w => w.BuildingCode.ToLower().Contains(search.ToLower()) || w.BuildingName.ToLower().Contains(search.ToLower()))
                || x.ListCell.Any(w => w.CellName.ToLower().Contains(search.ToLower()) || w.CellCode.ToLower().Contains(search.ToLower()))
                || x.ListHpA15.Any(w => w.Plno.ToLower().Contains(search.ToLower()) || w.Place.ToLower().Contains(search.ToLower()))
                || x.EmpName.ToLower().Contains(search.ToLower())).OrderBy(x => x.EmpNumber).ToList();
            return PageListUtility<PreliminaryPlnoDTO>.PageList(preliminaryPlnoList, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<OperationResult> AddPreliminaryPlno(PreliminaryPlnoAddDTO preliminaryPlno, string lang)
        {
            List<PreliminaryPlno> preliminaryPlnoList = new List<PreliminaryPlno>();
            foreach (var item in preliminaryPlno.RoleList)
            {
                PreliminaryPlno preliminaryPlnoNew = new PreliminaryPlno();
                preliminaryPlnoNew.EmpNumber = preliminaryPlno.EmpNumber;
                preliminaryPlnoNew.CreateBy = preliminaryPlno.UpdateBy;
                preliminaryPlnoNew.CreateTime = DateTime.Now;
                preliminaryPlnoNew.UpdateTime = DateTime.Now;
                preliminaryPlnoNew.BuildingID = item.BuildingID.ToInt();
                if (!string.IsNullOrEmpty(item.Cell))
                {
                    preliminaryPlnoNew.CellID = item.Cell.ToInt();
                    preliminaryPlnoNew.CellCode = _repository.Cells.FindAll(x => x.CellID == item.Cell.ToInt()).FirstOrDefault() != null
                    ? _repository.Cells.FindAll(x => x.CellID == item.Cell.ToInt()).FirstOrDefault().CellCode
                    : null;
                }
                else
                {
                    preliminaryPlnoNew.CellID = 0;
                }
                preliminaryPlnoNew.Plno = item.Plno;
                preliminaryPlnoNew.Active = true;


                preliminaryPlnoNew.Is_Manager = item.Is_Manager;
                preliminaryPlnoNew.Is_Preliminary = item.Is_Preliminary;

                preliminaryPlnoList.Add(preliminaryPlnoNew);
            }
            _repository.PreliminaryPlno.AddMultiple(preliminaryPlnoList);
            try
            {
                await _repository.SaveChangesAsync();
                return new OperationResult
                {
                    Success = true,
                    Message = lang == "vi-VN" ? "Bạn đã thêm phân quyền thành công" :
                    (lang == "en-US" ? "You have successfully added" :
                    (lang == "zh-TW" ? "您已成功添加" : "Anda telah berhasil menambahkan izin"))
                };
            }
            catch (System.Exception)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = lang == "vi-VN" ? "Lỗi hệ thống! Vui lòng thử lại" :
                    (lang == "en-US" ? "System error! Please try again" :
                    (lang == "zh-TW" ? "系統發生錯誤!請再試" : "Sistem bermasalah! Silakan coba lagi"))
                };
            }
        }
        public async Task<OperationResult> UpdatePreliminaryPlno(PreliminaryPlnoAddDTO preliminaryPlno, string lang)
        {

            List<PreliminaryPlno> preliminaryPlnoList = new List<PreliminaryPlno>();
            var listAll = _repository.PreliminaryPlno.FindAll(x => x.EmpNumber == preliminaryPlno.EmpNumber);
            foreach (var item in listAll)
            {
                item.Active = false;

            }
            foreach (var item in preliminaryPlno.RoleList)
            {
                PreliminaryPlno preliminaryPlnoExist = _repository.PreliminaryPlno.FindAll(x =>
                x.EmpNumber == preliminaryPlno.EmpNumber &&
                x.Plno == item.Plno && x.BuildingID == item.BuildingID.ToInt()
                && (!string.IsNullOrEmpty(item.Cell) ? (x.CellID == item.Cell.ToInt()) : x.CellID == 0)
                ).FirstOrDefault();
                if (preliminaryPlnoExist != null)
                {
                    preliminaryPlnoExist.Active = true;
                    preliminaryPlnoExist.UpdateTime = DateTime.Now;
                    preliminaryPlnoExist.UpdateBy = preliminaryPlno.UpdateBy;
                    preliminaryPlnoExist.Is_Manager = item.Is_Manager;
                    preliminaryPlnoExist.Is_Preliminary = item.Is_Preliminary;
                    _repository.PreliminaryPlno.Update(preliminaryPlnoExist);
                }
                else
                {
                    PreliminaryPlno preliminaryPlnoNew = new PreliminaryPlno();
                    preliminaryPlnoNew.EmpNumber = preliminaryPlno.EmpNumber;
                    preliminaryPlnoNew.CreateBy = preliminaryPlno.UpdateBy;
                    preliminaryPlnoNew.CreateTime = DateTime.Now;
                    preliminaryPlnoNew.BuildingID = item.BuildingID.ToInt();

                    if (!string.IsNullOrEmpty(item.Cell))
                    {
                        preliminaryPlnoNew.CellID = item.Cell.ToInt();
                        preliminaryPlnoNew.CellCode = _repository.Cells.FindAll(x => x.CellID == item.Cell.ToInt()).FirstOrDefault() != null
                        ? _repository.Cells.FindAll(x => x.CellID == item.Cell.ToInt()).FirstOrDefault().CellCode
                        : null;
                    }
                    else
                    {
                        preliminaryPlnoNew.CellID = 0;
                    }
                    preliminaryPlnoNew.Plno = item.Plno;
                    preliminaryPlnoNew.Active = true;
                    preliminaryPlnoNew.Is_Manager = item.Is_Manager;
                    preliminaryPlnoNew.Is_Preliminary = item.Is_Preliminary;
                    preliminaryPlnoList.Add(preliminaryPlnoNew);
                }
            }
            _repository.PreliminaryPlno.AddMultiple(preliminaryPlnoList);
            try
            {
                await _repository.SaveChangesAsync();
                return new OperationResult
                {
                    Success = true,
                    Message = lang == "vi-VN" ? "Bạn đã cập nhật phân quyền thành công" :
                    (lang == "en-US" ? "You have successfully updated" :
                    (lang == "zh-TW" ? "您已成功更新" : "Anda telah berhasil memperbarui izin"))
                };
            }
            catch (System.Exception)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = lang == "vi-VN" ? "Lỗi hệ thống! Vui lòng thử lại" :
                    (lang == "en-US" ? "System error! Please try again" :
                    (lang == "zh-TW" ? "系統發生錯誤!請再試" : "Sistem bermasalah! Silakan coba lagi"))
                };
            }
        }
        public async Task<OperationResult> RemovePreliminaryPlno(string empNumber, string lang)
        {
            var preliminaryList = _repository.PreliminaryPlno.FindAll(x => x.EmpNumber == empNumber).Distinct().ToList();
            _repository.PreliminaryPlno.RemoveMultiple(preliminaryList);
            try
            {
                await _repository.SaveChangesAsync();
                return new OperationResult
                {
                    Success = true,
                    Message = lang == "vi-VN" ? "Bạn đã xóa quyền thành công" :
                    (lang == "en-US" ? "You have successfully delete" :
                    (lang == "zh-TW" ? "您已成功删除" : "Anda telah berhasil menghapus izin"))
                };
            }
            catch (System.Exception)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = lang == "vi-VN" ? "Lỗi hệ thống! Vui lòng thử lại" :
                    (lang == "en-US" ? "System error! Please try again" :
                    (lang == "zh-TW" ? "系統發生錯誤!請再試" : "Sistem bermasalah! Silakan coba lagi"))
                };
            }
        }
        public async Task<PreliminaryPlnoDTO> GetPreliminaryPlnos(string empNumber)
        {
            var preliminaryList = await _repository.PreliminaryPlno.FindAll(x => x.EmpNumber == empNumber.Trim() && x.Active == true).ToListAsync();
            var listCellPlno = _repository.Cell_Plno.FindAll();
            var listCell = _repository.Cells.FindAll();
            var listBuilding = _repository.Building.FindAll();
            var listHp15 = _repository.hp_a15.FindAll();
            var dataJoin = new PreliminaryPlnoDTO();
            foreach (var item in preliminaryList)
            {

                dataJoin = new PreliminaryPlnoDTO
                {

                    EmpNumber = empNumber,
                    Is_Manager = item.Is_Manager,
                    Is_Preliminary = item.Is_Preliminary,
                    ListBuilding = preliminaryList.Join(listBuilding, lb => lb.BuildingID, b => b.BuildingID, (lb, b) => new BuildingDto
                    {
                        BuildingID = lb.BuildingID,
                        BuildingCode = b.BuildingCode,
                        BuildingName = b.BuildingName
                    }).GroupBy(x => new
                    {
                        BuildingID = x.BuildingID,
                    }, y => y).Select(x => x.FirstOrDefault()).OrderBy(x => x.BuildingCode).ToList(),
                    ListCell = preliminaryList.Join(listCell, lc => lc.CellID, c => c.CellID, (lc, c) => new CellDto
                    {
                        CellCode = c.CellCode,
                        CellName = c.CellName
                    }).GroupBy(x => new
                    {
                        CellCode = x.CellCode
                    }, y => y).Select(x => x.FirstOrDefault()).OrderBy(x => x.CellCode).ToList(),
                    ListHpA15 = preliminaryList.Join(listHp15, lh => lh.Plno, p => p.Plno, (lh, p) => new Hp_a15Dto
                    {
                        Plno = p.Plno,
                        BuildingID = lh.BuildingID,
                        CellID = lh.CellID
                    }).Distinct().ToList(),
                };
            }

            return dataJoin;
        }

        public void PutStaticValue(ref Worksheet ws)
        {
            ws.Cells["A1"].PutValue(_languageService.GetLocalizedHtmlString("admin_user_code"));
            ws.Cells["B1"].PutValue(_languageService.GetLocalizedHtmlString("admin_name"));
            ws.Cells["C1"].PutValue(_languageService.GetLocalizedHtmlString("building"));
            ws.Cells["D1"].PutValue(_languageService.GetLocalizedHtmlString("cellplno_cell_name2"));
            ws.Cells["E1"].PutValue(_languageService.GetLocalizedHtmlString("cellplno_report"));
        }
        public void CustomStyle(ref Cell cellCustom)
        {
            string value = Convert.ToString(cellCustom.Value);

            if (value == "1")
            {
                cellCustom.PutValue(_languageService.GetLocalizedHtmlString("match"));
                Style styleCustom = cellCustom.GetStyle();
                styleCustom.Font.Color = Color.Green;
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
            else if (value == "0")
            {
                cellCustom.PutValue(_languageService.GetLocalizedHtmlString("not_scan"));
                Style styleCustom = cellCustom.GetStyle();
                styleCustom.Font.Color = Color.Red;
                styleCustom.Font.IsBold = true;
                cellCustom.SetStyle(styleCustom);
            }
        }
    }
}