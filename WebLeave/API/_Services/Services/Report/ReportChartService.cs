using API._Repositories;
using API._Services.Interfaces.Report;
using API.Dtos.Report.ReportChart;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.Report
{
    public class ReportChartService : IReportChartService
    {
        private readonly IRepositoryAccessor _repoAccessor;
        public ReportChartService(IRepositoryAccessor repoAccessor)
        {
            _repoAccessor = repoAccessor;
        }

        public async Task<ReportChartDto> GetDataChart()
        {
            // ----------------------------company in chart----------------------------//
            Company company = await _repoAccessor.Company.FirstOrDefaultAsync(x => x.Visible == true);
            NameLang nameCompanys = new()
            {
                name = company.CompanyName
            };

            ReportChartDto data = new()
            {
                Id = company.CompanyID,
                names = new List<NameLang> { nameCompanys }
            };

            // ------------------------------area in chart-----------------------------//
            List<ReportChartDto> listArea = new();

            List<int> areaIDsDB = await _repoAccessor.Area
                .FindAll(x => x.CompanyID == company.CompanyID && x.Visible == true)
                .Select(x => x.AreaID).ToListAsync();

            List<AreaLang> areasLangDB = await _repoAccessor.AreaLang
                .FindAll(x => areaIDsDB.Contains(x.AreaID.Value)).ToListAsync();

            foreach (var itemAreaId in areaIDsDB)
            {
                ReportChartDto areaItem = new()
                {
                    // OrderBy theo langId nên thứ tự en => vi => zh-TW;
                    names = areasLangDB.Where(x => x.AreaID == itemAreaId).OrderBy(x => x.LanguageID).Select(y => new NameLang()
                    {
                        name = y.AreaName,
                        lang = y.LanguageID
                    }).ToList(),
                    Id = itemAreaId
                };
                listArea.Add(areaItem);
            }
            data.children = listArea;

            return data;
        }

        public async Task<ReportChartDto> GetDataChartInArea(int areaID)
        {
            ReportChartDto data = new()
            {
                Id = areaID,
                names = await _repoAccessor.AreaLang.FindAll(x => x.AreaID == areaID).OrderBy(x => x.LanguageID).Select(x => new NameLang()
                {
                    name = x.AreaName,
                    lang = x.LanguageID
                }).ToListAsync()
            };

            // -----------------------------------------get building là children của Area--------------------------//
            List<ReportChartDto> buildings = new();
            List<int> buildingIDsDB = await _repoAccessor.Building.FindAll(x => x.AreaID == areaID && x.Visible == true).Select(x => x.BuildingID).ToListAsync();
            List<BuildLang> buildingsLangDB = await _repoAccessor.BuildLang.FindAll(x => buildingIDsDB.Contains(x.BuildingID.Value)).ToListAsync();
            if (buildingIDsDB.Any())
            {
                foreach (var itemBuildingId in buildingIDsDB)
                {
                    ReportChartDto buildingItem = new()
                    {
                        Id = itemBuildingId,
                        names = buildingsLangDB.Where(x => x.BuildingID == itemBuildingId).OrderBy(x => x.LanguageID).Select(y => new NameLang()
                        {
                            name = y.BuildingName,
                            lang = y.LanguageID
                        }).ToList()
                    };

                    // Trong từng buildingItem lại có children là department
                    List<ReportChartDto> departments = new();
                    List<int> departmentIDsDB = await _repoAccessor.Department.FindAll(x => x.BuildingID == itemBuildingId && x.Visible == true).Select(x => x.DeptID).ToListAsync();
                    List<DetpLang> departmentsLangDB = await _repoAccessor.DetpLang.FindAll(x => departmentIDsDB.Contains(x.DeptID.Value)).ToListAsync();
                    foreach (int itemDepId in departmentIDsDB)
                    {
                        ReportChartDto departmentItem = new()
                        {
                            Id = itemDepId,
                            names = departmentsLangDB.Where(x => x.DeptID == itemDepId).OrderBy(x => x.LanguageID).Select(y => new NameLang()
                            {
                                name = y.DeptName,
                                lang = y.LanguageID
                            }).ToList()
                        };
                        departments.Add(departmentItem);
                    }
                    buildingItem.children = departments;
                    buildings.Add(buildingItem);
                }

                data.children = buildings;

            }
            else
            {
                //Nếu Listbuilding trống => tìm trực tiếp đến department vì những dept thuộc văn phòng không cần tham chiếu đến Building
                List<ReportChartDto> departments = new();
                List<int> departmentIDsDB = await _repoAccessor.Department.FindAll(x => x.AreaID == areaID && x.Visible == true).Select(x => x.DeptID).ToListAsync();
                List<DetpLang> departmentsLangDB = await _repoAccessor.DetpLang.FindAll(x => departmentIDsDB.Contains(x.DeptID.Value)).ToListAsync();
                foreach (int itemDepId in departmentIDsDB)
                {
                    ReportChartDto departmentItem = new()
                    {
                        Id = itemDepId,
                        names = departmentsLangDB.Where(x => x.DeptID == itemDepId).OrderBy(x => x.LanguageID).Select(y => new NameLang()
                        {
                            name = y.DeptName,
                            lang = y.LanguageID
                        }).ToList()
                    };

                    // Trong từng department lại có list children là Part
                    List<ReportChartDto> parts = new();
                    List<int> partIDsDB = await _repoAccessor.Part.FindAll(x => x.DeptID == itemDepId).Select(x => x.PartID).ToListAsync();
                    List<PartLang> partLangsDB = await _repoAccessor.PartLang.FindAll(x => partIDsDB.Contains(x.PartID.Value)).ToListAsync();
                    foreach (int itemPartId in partIDsDB)
                    {
                        ReportChartDto partItem = new()
                        {
                            Id = itemPartId,
                            names = partLangsDB.Where(x => x.PartID == itemPartId).OrderBy(x => x.LanguageID).Select(y => new NameLang()
                            {
                                name = y.PartName,
                                lang = y.LanguageID
                            }).ToList()
                        };
                        parts.Add(partItem);
                    }
                    departmentItem.children = parts;
                    departments.Add(departmentItem);
                }
                data.children = departments;
            }

            return data;
        }
    }
}