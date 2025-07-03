using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aspose.Cells;
using eTierV2_API._Repositories;
using eTierV2_API._Repositories.Interfaces;
using eTierV2_API._Services.Interfaces.Production.T1.C2B;
using eTierV2_API.DTO.HSEUpload;
using eTierV2_API.Helpers.Enums;
using eTierV2_API.Helpers.Params;
using eTierV2_API.Helpers.Utilities;
using eTierV2_API.Models;
using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace eTierV2_API._Services.Services.Production.T1.C2B
{
    public class HSEUResultUploadService : IHSEUResultUploadService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRepositoryAccessor _repoAccessor;

        public HSEUResultUploadService( IWebHostEnvironment webHostEnvironment,IRepositoryAccessor repoAccessor)
        {
            _webHostEnvironment = webHostEnvironment;
            _repoAccessor = repoAccessor;
        }

        public async Task<List<string>> GetDeptInBuilding(string building)
        {
            var eTMTeamUnits = _repoAccessor.eTM_Team_Unit.FindAll();
            var vWDeptFromMESs = _repoAccessor.VW_DeptFromMES.FindAll(x => x.Building.Trim() == building);
            var depts = await eTMTeamUnits.Join(
                vWDeptFromMESs,
                x => x.TU_Code,
                y => y.Dept_ID,
                (x, y) => y.Dept_ID
            ).Distinct().ToListAsync();
            return depts;
        }

        public async Task<List<string>> GetBuildings()
        {
            var eTMTeamUnits = _repoAccessor.eTM_Team_Unit.FindAll(x => x.Class1_Level.Trim() == "CTB");
            var vWDeptFromMESs = _repoAccessor.VW_DeptFromMES.FindAll();

            var buildings = await eTMTeamUnits.Join(
                vWDeptFromMESs,
                x => x.TU_Code,
                y => y.Dept_ID,
                (x, y) => y.Building
            ).Distinct().ToListAsync();
            return buildings;
        }

        public async Task<List<string>> GetEvalutionCategory()
        {
            var evalutionCategory = await _repoAccessor.eTM_Page_Item_Settings.FindAll(x => x.Center_Level.Trim() == "Production" &&
                                                                    x.Tier_Level.Trim() == "T2" &&
                                                                    x.Class_Level.Trim() == "CTB" &&
                                                                    x.Page_Name.Trim() == "Safety" && x.Is_Active == true).Select(x => x.Item_Name).ToListAsync();
            return evalutionCategory;
        }

        public async Task<List<DataDownloadTemplate>> GetETMUnits()
        {
            var eTMTeamUnits = _repoAccessor.eTM_Team_Unit.FindAll();
            var viewDeptFromMes = _repoAccessor.VW_DeptFromMES.FindAll(m => m.PS_ID == "ASY");
            var data = await eTMTeamUnits.Join(
                viewDeptFromMes,
                x => x.TU_Code,
                y => y.Dept_ID,
                (x, y) => new DataDownloadTemplate()
                {
                    Building = y.Building,
                    Line_Sname = y.Line_Sname,
                    TU_Code = x.TU_Code
                }
            ).ToListAsync();
            return data;
        }

        public async Task<bool> UploadExcel(IFormFile file, string updatedBy)
        {
            var timeNow = DateTime.Now;
            if (file == null)
            {
                return false;
            }

            var extension = Path.GetExtension(file.FileName).ToLower();
            var uploadFile = $"HSE_Result_Upload{extension}";
            var uploadPath = @"Excels";
            var folder = Path.Combine(_webHostEnvironment.WebRootPath, uploadPath);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string filePath = Path.Combine(folder, uploadFile);

            if (File.Exists(filePath))
                File.Delete(filePath);

            try
            {
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }
            catch (Exception)
            {
                throw;
            }

            // Đọc file
            var designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(filePath);
            var ws = designer.Workbook.Worksheets[0];
            int rows = ws.Cells.MaxDataRow;

            var data = new List<eTM_HSE_Score_Data>();
            for (int i = 2; i <= rows; i++)
            {
                var year = ws.Cells[i, 0].IntValue;
                var month = ws.Cells[i, 1].IntValue;
                var center_Level = ws.Cells[i, 2].StringValue;
                var tier_Level = ws.Cells[i, 3].StringValue;
                var section = ws.Cells[i, 4].StringValue;
                var building = ws.Cells[i, 5].StringValue;
                var line_Sname = ws.Cells[i, 6].StringValue;
                var deptID = ws.Cells[i, 7].StringValue;

                // Tính số Evalucation Category trong file excel
                for (int t = 8; t < 30; t++)
                {
                    var itemEvalucation = ws.Cells[1, t].StringValue;
                    if (!string.IsNullOrEmpty(itemEvalucation))
                    {
                        var valueScore = ws.Cells[i, t].StringValue;
                        if (!string.IsNullOrEmpty(valueScore))
                        {

                            var item_ID = await _repoAccessor.eTM_Page_Item_Settings.FindAll(x => x.Center_Level.Trim() == center_Level.Trim() &&
                                                                        x.Tier_Level.Trim() == tier_Level.Trim() &&
                                                                        x.Class_Level.Trim() == section.Trim() &&
                                                                        x.Item_Name.Trim() == itemEvalucation.Trim() && x.Is_Active == true).Select(x => x.Item_ID).FirstOrDefaultAsync();
                            var dataCheck = await _repoAccessor.eTM_HSE_Score_Data.FindAll(x => x.Year == year && x.Month == month &&
                                                            x.Center_Level.Trim() == center_Level.Trim() &&
                                                            x.Tier_Level.Trim() == tier_Level.Trim() && x.Class_Level.Trim() == section.Trim() &&
                                                            item_ID == x.Item_ID && x.TU_Code.Trim() == deptID.Trim()).FirstOrDefaultAsync();

                            if (dataCheck != null)
                            {
                                // Check nếu record có các cột tương đương có trong DB rồi thì update lại score,update time,update by
                                dataCheck.Score = decimal.Parse(valueScore);
                                dataCheck.Update_By = updatedBy;
                                dataCheck.Update_Time = timeNow;
                            }
                            else
                            {
                                // Ngược lại thì thêm vào DB
                                var hSEScoreDataItem = new eTM_HSE_Score_Data();
                                hSEScoreDataItem.Year = year;
                                hSEScoreDataItem.Month = month;
                                hSEScoreDataItem.Center_Level = center_Level;
                                hSEScoreDataItem.Tier_Level = tier_Level;
                                hSEScoreDataItem.Class_Level = section;
                                hSEScoreDataItem.TU_Code = deptID;
                                hSEScoreDataItem.Item_ID = item_ID;
                                hSEScoreDataItem.Score = decimal.Parse(valueScore);
                                hSEScoreDataItem.Update_By = updatedBy;
                                hSEScoreDataItem.Update_Time = timeNow;

                                data.Add(hSEScoreDataItem);
                            }
                        }
                    }
                }
            }
            _repoAccessor.eTM_HSE_Score_Data.AddMultiple(data);
            var save = await _repoAccessor.eTM_HSE_Score_Data.SaveAll();
            return save;
        }

        public async Task<PagedList<HSEDataSearchDto>> Search(eTierV2_API.Helpers.Params.PaginationParam param, HSESearchParam paramSearch)
        {
            var predHSEDataUpload = PredicateBuilder.New<eTM_HSE_Score_Data>(true);
            var predViewFromMes = PredicateBuilder.New<VW_DeptFromMES>(true);
            predHSEDataUpload.And(x => x.Year == paramSearch.Year && x.Month == paramSearch.Month);
            if (!string.IsNullOrEmpty(paramSearch.Building))
            {
                predViewFromMes.And(x => x.Building.Trim() == paramSearch.Building);
            }
            if (!string.IsNullOrEmpty(paramSearch.DeptID))
            {
                predViewFromMes.And(x => x.Dept_ID.Trim() == paramSearch.DeptID.Trim());
            }

            var hseScoreData = _repoAccessor.eTM_HSE_Score_Data.FindAll(predHSEDataUpload);
            var viewFromMesData = _repoAccessor.VW_DeptFromMES.FindAll(predViewFromMes);
            var pageSettingItemData = _repoAccessor.eTM_Page_Item_Settings.FindAll();
            var data = await hseScoreData.Join(
                viewFromMesData,
                x => x.TU_Code,
                y => y.Dept_ID,
                (x, y) => new { hseScoreData = x, viewFromMesData = y }
            ).Join(
                pageSettingItemData,
                x => x.hseScoreData.Item_ID,
                y => y.Item_ID,
                (x, y) => new HSEDataSearchDto()
                {
                    HSE_Score_ID = x.hseScoreData.HSE_Score_ID,
                    Center_Level = x.hseScoreData.Center_Level,
                    Tier_Level = x.hseScoreData.Tier_Level,
                    Class_Level = x.hseScoreData.Center_Level,
                    Building = x.viewFromMesData.Building,
                    Dept_ID = x.viewFromMesData.Dept_ID,
                    Line_Sname = x.viewFromMesData.Line_Sname,
                    Evaluation = y.Item_Name,
                    Score = x.hseScoreData.Score,
                    Target = y.Target,
                    Action = x.hseScoreData.Score < y.Target ? ActionHseConstants.NEED : ActionHseConstants.NO_NEED,
                    Update_By = x.hseScoreData.Update_By,
                    Update_Time = x.hseScoreData.Update_Time,
                }
            ).ToListAsync();
            foreach (var item in data)
            {
                if (item.Action == ActionHseConstants.NEED)
                {
                    var checkUpload = await _repoAccessor.eTM_HSE_Score_Image.FindAll(x => x.HSE_Score_ID == item.HSE_Score_ID).ToListAsync();
                    if (checkUpload.Any())
                    {
                        // Record cần upload hình và đã up rồi
                        item.Action = ActionHseConstants.NEED_YES;
                    }
                    else
                    {
                        // Record cần upload hình và đã chưa up
                        item.Action = ActionHseConstants.NEED_NO;
                    }
                }
            }

            var checkImageAlert = data.Any(x => x.Action == ActionHseConstants.NEED_NO);
            data.ForEach(item => item.CheckImageAlert = checkImageAlert);

            if (paramSearch.ClickImageAlert)
            {
                data = data.Where(x => x.Action == ActionHseConstants.NEED_NO).ToList();
            }
            return PagedList<HSEDataSearchDto>.Create(data, param.PageNumber, param.PageSize, true);
        }

        public async Task<bool> Remove(int score_ID)
        {
            var hseScoreData = await _repoAccessor.eTM_HSE_Score_Data.FindAll(x => x.HSE_Score_ID == score_ID).FirstOrDefaultAsync();
            var hseImages = await _repoAccessor.eTM_HSE_Score_Image.FindAll(x => x.HSE_Score_ID == score_ID).ToListAsync();
            if (hseImages.Any())
            {
                _repoAccessor.eTM_HSE_Score_Image.RemoveMultiple(hseImages);
            }
            _repoAccessor.eTM_HSE_Score_Data.Remove(hseScoreData);

            var save = await _repoAccessor.eTM_HSE_Score_Data.SaveAll();
            return save;
        }

        public async Task<bool> UpdateScoreData(HSEDataSearchDto model, string updatedBy)
        {
            var modelFind = await _repoAccessor.eTM_HSE_Score_Data.FindAll(x => x.HSE_Score_ID == model.HSE_Score_ID).FirstOrDefaultAsync();
            if (modelFind != null)
            {
                modelFind.Score = model.Score;
                modelFind.Update_By = updatedBy;
                modelFind.Update_Time = DateTime.Now;
                return await _repoAccessor.eTM_HSE_Score_Data.SaveAll();
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UploadImages(ImageDataUpload data, string updatedBy)
        {
            var fn = new FunctionUtility();
            var images = data.Images;
            var remarks = data.Remarks;
            var pathContainsImage = "Production/T2/HSEUpload";
            for (int i = 0; i < images.Count; i++)
            {
                var imageFileName = await fn.UploadAsync(images[i], pathContainsImage, Guid.NewGuid().ToString().Substring(0, 20));
                var image_Path = pathContainsImage + "/" + imageFileName;

                var scoreImageModel = new eTM_HSE_Score_Image();
                scoreImageModel.HSE_Image_ID = Guid.NewGuid();
                scoreImageModel.HSE_Score_ID = data.HseID;
                scoreImageModel.Image_Path = image_Path;
                scoreImageModel.Remark = remarks[i];
                _repoAccessor.eTM_HSE_Score_Image.Add(scoreImageModel);
            }
            return await _repoAccessor.eTM_HSE_Score_Image.SaveAll();
        }

        public async Task<List<ImageRemark>> GetListImageToHseID(int hseID)
        {
            var images = await _repoAccessor.eTM_HSE_Score_Image.FindAll(x => x.HSE_Score_ID == hseID).Select(x => new ImageRemark()
            {
                Name = x.Image_Path.Replace("Production/T2/HSEUpload/", ""),
                Remark = x.Remark
            }).ToListAsync();
            return images;
        }

        public async Task<bool> EditImages(ImageDataUpload data, string updatedBy)
        {
            var fn = new FunctionUtility();
            var images = data.Images;
            var remarks = data.Remarks;
            var pathContainsImage = "Production/T2/HSEUpload";
            var dataOld = await _repoAccessor.eTM_HSE_Score_Image.FindAll(x => x.HSE_Score_ID == data.HseID).ToListAsync();
            if(dataOld.Any()) {
                _repoAccessor.eTM_HSE_Score_Image.RemoveMultiple(dataOld);
            }
            for (int i = 0; images is not null && i < images.Count; i++)
            {
                var imageFileName = await fn.UploadAsync(images[i], pathContainsImage, Guid.NewGuid().ToString().Substring(0, 20));
                var image_Path = pathContainsImage + "/" + imageFileName;

                var scoreImageModel = new eTM_HSE_Score_Image();
                scoreImageModel.HSE_Image_ID = Guid.NewGuid();
                scoreImageModel.HSE_Score_ID = data.HseID;
                scoreImageModel.Image_Path = image_Path;
                scoreImageModel.Remark = remarks[i];
                _repoAccessor.eTM_HSE_Score_Image.Add(scoreImageModel);
            }
            var save = await _repoAccessor.eTM_HSE_Score_Image.SaveAll();
            if(save) {
                if (dataOld.Any()) {
                    dataOld.ForEach(item => {
                        var extension = item.Image_Path.Split(".")[1].ToLower();  // đuôi của file
                        var uploadFile = $"{item.Image_Path.Replace("Production/T2/HSEUpload/", "").Split(".")[0]}.{extension}";
                        var uploadPath = @"Production/T2/HSEUpload/";
                        var folder = Path.Combine(_webHostEnvironment.WebRootPath, uploadPath);
                        string filePath = Path.Combine(folder, uploadFile);

                        if (File.Exists(filePath))
                            File.Delete(filePath);
                    });
                }
                return true;
            } else {
                return false;
            }
        }
    }
}