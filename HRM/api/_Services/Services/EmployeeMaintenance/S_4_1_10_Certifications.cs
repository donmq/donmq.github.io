using API.Data;
using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.EmployeeMaintenance
{
    public class S_4_1_10_Certifications : BaseServices, I_4_1_10_Certifications
    {
        private readonly string folder = "uploaded\\EmployeeMaintenance\\4_1_10_Certifications";
        public S_4_1_10_Certifications(DBContext dbContext) : base(dbContext)
        {
        }
        public async Task<List<KeyValuePair<string, string>>> GetDropDownList(Certifications_MainParam param)
        {
            var HBC = await _repositoryAccessor.HRMS_Basic_Code.FindAll().ToListAsync();
            var HBCL = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == param.Lang.ToLower()).ToList();
            var result = new List<KeyValuePair<string, string>>();
            var data = HBC.GroupJoin(HBCL,
                    x => new { x.Type_Seq, x.Code },
                    y => new { y.Type_Seq, y.Code },
                    (x, y) => new { hbc = x, hbcl = y })
                    .SelectMany(x => x.hbcl.DefaultIfEmpty(),
                    (x, y) => new { x.hbc, hbcl = y });
            result.AddRange(data.Where(x => x.hbc.Type_Seq == "1").Select(x => new KeyValuePair<string, string>("DI", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList());
            result.AddRange(data.Where(x => x.hbc.Type_Seq == "30").Select(x => new KeyValuePair<string, string>("CA", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList());
            if (!string.IsNullOrWhiteSpace(param.Division))
            {
                var HBFC = _repositoryAccessor.HRMS_Basic_Factory_Comparison.FindAll(x => x.Division == param.Division && x.Kind == "1").ToList();
                if (HBFC.Any())
                {
                    var dataFilter = data.Join(HBFC,
                        x => new { Factory = x.hbc.Code, x.hbc.Type_Seq },
                        y => new { y.Factory, Type_Seq = "2" },
                        (x, y) => new { x.hbc, x.hbcl, hbfc = y });
                    result.AddRange(dataFilter.Where(x => x.hbc.Type_Seq == "2").Select(x => new KeyValuePair<string, string>("FA", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList());
                }
            }
            return result;
        }
        public async Task<List<Certifications_TypeheadKeyValue>> GetEmployeeList(Certifications_SubParam param)
        {
            var HEP = await _repositoryAccessor.HRMS_Emp_Personal.FindAll(x => x.Factory == param.Factory && x.Employee_ID.Contains(param.Employee_Id)).ToListAsync();
            var HEC = _repositoryAccessor.HRMS_Emp_Certification.FindAll(x => x.Division == param.Division && x.Factory == param.Factory).ToList();
            var data = HEP
                .GroupJoin(HEC,
                    x => x.Employee_ID,
                    y => y.Employee_ID,
                    (x, y) => new { HEP = x, HEC = y })
                .SelectMany(x => x.HEC.DefaultIfEmpty(),
                    (x, y) => new { x.HEP, HEC = y })
                .GroupBy(x => x.HEP);
            var result = data
                .Select(x => new Certifications_TypeheadKeyValue
                {
                    UseR_GUID = x.Key.USER_GUID,
                    Key = x.Key.Employee_ID,
                    Local_Full_Name = x.Key.Local_Full_Name,
                    Max_Seq = x.Any(y => y.HEC != null) ? x.Max(y => y.HEC.Seq) : 0
                })
                .ToList();
            return result;
        }
        public async Task<PaginationUtility<Certifications_MainData>> GetSearchDetail(PaginationParam paginationParams, Certifications_MainParam searchParam, List<string> roleList)
        {
            return PaginationUtility<Certifications_MainData>.Create(await GetData(searchParam, roleList), paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<OperationResult> GetSubDetail(Certifications_SubParam param)
        {
            var predicateCertification = PredicateBuilder.New<HRMS_Emp_Certification>(true);
            var predicateFile = PredicateBuilder.New<HRMS_Emp_File>(x => x.Program_Code == "4.1.10");
            if (!string.IsNullOrWhiteSpace(param.Division))
            {
                predicateCertification.And(x => x.Division == param.Division);
                predicateFile.And(x => x.Division == param.Division);
            }
            if (!string.IsNullOrWhiteSpace(param.Factory))
            {
                predicateCertification.And(x => x.Factory == param.Factory);
                predicateFile.And(x => x.Factory == param.Factory);
            }
            if (!string.IsNullOrWhiteSpace(param.Employee_Id))
                predicateCertification.And(x => x.Employee_ID == param.Employee_Id);
            var HEC = _repositoryAccessor.HRMS_Emp_Certification.FindAll(predicateCertification);
            if (!HEC.Any())
                return new OperationResult(false, "NotExitedData");
            var HEF = _repositoryAccessor.HRMS_Emp_File.FindAll(predicateFile);
            var data = HEC
                .GroupJoin(HEF,
                    x => new { x.Division, x.Factory, x.Program_Code, x.SerNum },
                    y => new { y.Division, y.Factory, y.Program_Code, y.SerNum },
                    (x, y) => new { HED = x, HEF = y })
                .SelectMany(x => x.HEF.DefaultIfEmpty(),
                    (x, y) => new { x.HED, HEF = y })
                .GroupBy(x => x.HED);
            var result = await data.Select(x => new Certifications_SubData
            {
                Seq = x.Key.Seq,
                Category_Of_Certification = x.Key.Certification,
                Name_Of_Certification = x.Key.Name_Of_Certification,
                Score = x.Key.Score,
                Level = x.Key.Level,
                Result = x.Key.Result,
                Passing_Date = x.Key.Passing_Date,
                Certification_Valid_Period = x.Key.Certification_Valid_Period,
                Remark = x.Key.Remark,
                Update_By = x.Key.Update_By,
                Update_Time = x.Key.Update_Time,
                Ser_Num = x.Key.SerNum,
                File_List = x.Where(y => y.HEF != null).OrderBy(y => y.HEF.FileID).Select(y => new Certifications_FileModel
                {
                    Id = y.HEF.FileID,
                    Name = y.HEF.FileName,
                    Size = Convert.ToInt32(y.HEF.FileSize)
                }).GroupBy(x => new { x.Id, x.Name, x.Size }).Select(x => x.First()).ToList()
            })
            .OrderBy(x => x.Seq)
            .ToListAsync();
            return new OperationResult(true, result);
        }

        public async Task<OperationResult> DownloadFile(Certifications_DownloadFileModel param)
        {
            var file = await _repositoryAccessor.HRMS_Emp_File.FirstOrDefaultAsync(x =>
               x.Division == param.Division &&
               x.Factory == param.Factory &&
               x.SerNum == param.Ser_Num &&
               x.FileName == param.File_Name &&
               x.Program_Code == "4.1.10"
           );
            if (file == null)
                return new OperationResult(false, "NotExitedData");
            string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string path = $"{webRootPath}\\{folder}\\{param.Factory}\\{param.Division}\\{param.Employee_Id}\\{param.Ser_Num}\\{file.FileName}";
            if (!File.Exists(path))
                return new OperationResult(false, "NotExitedFile");
            byte[] bytes = File.ReadAllBytes(path);
            var fileNameArray = file.FileName.Split(".");
            DocumentManagement_FileModel fileData = new()
            {
                Name = file.FileName,
                Content = $"data:{fileNameArray[^1]};base64,{Convert.ToBase64String(bytes)}"
            };
            return new OperationResult(true, fileData);
        }

        public async Task<OperationResult> CheckExistedData(Certifications_SubModel param)
        {
            return new OperationResult(await _repositoryAccessor.HRMS_Emp_Certification.AnyAsync(x => x.Division == param.Division && x.Factory == param.Factory && x.Employee_ID == param.Employee_Id));
        }

        public async Task<OperationResult> PutData(Certifications_SubMemory input)
        {
            var predicateCertification = PredicateBuilder.New<HRMS_Emp_Certification>(true);
            var predicateFile = PredicateBuilder.New<HRMS_Emp_File>(x => x.Program_Code == "4.1.10");
            if (!string.IsNullOrWhiteSpace(input.Param.Division))
            {
                predicateCertification.And(x => x.Division == input.Param.Division);
                predicateFile.And(x => x.Division == input.Param.Division);
            }
            if (!string.IsNullOrWhiteSpace(input.Param.Factory))
            {
                predicateCertification.And(x => x.Factory == input.Param.Factory);
                predicateFile.And(x => x.Factory == input.Param.Factory);
            }
            if (!string.IsNullOrWhiteSpace(input.Param.Employee_Id))
                predicateCertification.And(x => x.Employee_ID == input.Param.Employee_Id);
            List<HRMS_Emp_Certification> certRemoveList = await _repositoryAccessor.HRMS_Emp_Certification.FindAll(predicateCertification).ToListAsync();
            if (!certRemoveList.Any())
                return new OperationResult(false, "NotExitedData");
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                List<HRMS_Emp_File> HEFs = _repositoryAccessor.HRMS_Emp_File.FindAll(predicateFile).ToList();
                List<string> serNumList = certRemoveList.Select(x => x.SerNum).ToList();
                List<HRMS_Emp_File> fileRemoveList = HEFs.Where(x => serNumList.Contains(x.SerNum)).ToList();
                List<HRMS_Emp_Certification> certAddList = new();
                List<HRMS_Emp_File> fileAddList = new();
                string serNumStart = DateTime.Now.ToString("yyyyMMdd");
                string serNum = "";
                var recentFiles = certRemoveList.Where(x => x.SerNum.StartsWith(serNumStart)).OrderByDescending(x => x.SerNum);
                if (recentFiles.Any())
                    serNum = recentFiles.FirstOrDefault().SerNum;
                foreach (var certification in input.Data)
                {
                    string selectedSerNum = "";
                    if (!string.IsNullOrWhiteSpace(certification.Ser_Num))
                        selectedSerNum = certification.Ser_Num;
                    else
                    {
                        serNum = string.IsNullOrWhiteSpace(serNum) ? serNumStart + "0000001" : serNum[0..8] + (int.Parse(serNum[9..]) + 1).ToString("0000000");
                        selectedSerNum = serNum;
                    }
                    string location = $"{folder}\\{input.Param.Factory}\\{input.Param.Division}\\{input.Param.Employee_Id}\\{selectedSerNum}";
                    HRMS_Emp_Certification certAdd = new()
                    {
                        Division = input.Param.Division,
                        Factory = input.Param.Factory,
                        Employee_ID = input.Param.Employee_Id,
                        Seq = certification.Seq,
                        Certification = certification.Category_Of_Certification,
                        Name_Of_Certification = certification.Name_Of_Certification,
                        Score = certification.Score,
                        Level = certification.Level,
                        Result = certification.Result,
                        Passing_Date = Convert.ToDateTime(certification.Passing_Date_Str),
                        Certification_Valid_Period = !string.IsNullOrWhiteSpace(certification.Certification_Valid_Period_Str) ? Convert.ToDateTime(certification.Certification_Valid_Period_Str) : null,
                        Remark = certification.Remark,
                        Program_Code = "4.1.10",
                        SerNum = selectedSerNum,
                        Update_By = certification.Update_By,
                        Update_Time = Convert.ToDateTime(certification.Update_Time_Str)
                    };
                    certAddList.Add(certAdd);
                    if (!certification.File_List.Any())
                    {
                        string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                        string path = $"{webRootPath}\\{location}";
                        if (Directory.Exists(path))
                            Directory.Delete(path, true);
                    }
                    foreach (var file in certification.File_List)
                    {
                        HRMS_Emp_File fileAdd = new()
                        {
                            Division = certAdd.Division,
                            Factory = certAdd.Factory,
                            Program_Code = "4.1.10",
                            SerNum = selectedSerNum,
                            FileID = file.Id,
                            FileName = file.Name,
                            FileSize = file.Size,
                            Update_By = certAdd.Update_By,
                            Update_Time = certAdd.Update_Time
                        };
                        if (!string.IsNullOrWhiteSpace(file.Content))
                        {
                            var fileNameArray = file.Name.Split(".");
                            string savedFileName = await FilesUtility.UploadAsync(file.Content, location, fileNameArray[0], fileNameArray[^1]);
                            if (string.IsNullOrWhiteSpace(savedFileName))
                                return new OperationResult(false, "SaveFileError");
                            fileAdd.FileName = savedFileName;
                        }
                        fileAddList.Add(fileAdd);
                    }
                }
                _repositoryAccessor.HRMS_Emp_Certification.RemoveMultiple(certRemoveList);
                if (fileRemoveList.Any())
                    _repositoryAccessor.HRMS_Emp_File.RemoveMultiple(fileRemoveList);
                await _repositoryAccessor.Save();
                if (certAddList.Any())
                    _repositoryAccessor.HRMS_Emp_Certification.AddMultiple(certAddList);
                if (fileAddList.Any())
                    _repositoryAccessor.HRMS_Emp_File.AddMultiple(fileAddList);
                await _repositoryAccessor.Save();
                await _repositoryAccessor.CommitAsync();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult(false);
            }
        }
        public async Task<OperationResult> PostData(Certifications_SubMemory input)
        {
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                var HEC = _repositoryAccessor.HRMS_Emp_Certification.FindAll(x => x.Program_Code == "4.1.10");
                string serNumStart = DateTime.Now.ToString("yyyyMMdd");
                string serNum = "";
                var recentFiles = HEC.Where(x => x.SerNum.StartsWith(serNumStart)).OrderByDescending(x => x.SerNum);
                if (recentFiles.Any())
                    serNum = recentFiles.FirstOrDefault().SerNum;
                List<HRMS_Emp_Certification> addCertifications = new();
                List<HRMS_Emp_File> addFiles = new();
                var _HEC = HEC.Where(x =>
                   x.Factory == input.Param.Factory &&
                   x.Division == input.Param.Division &&
                   x.Employee_ID == input.Param.Employee_Id);
                foreach (var certification in input.Data)
                {
                    if (_HEC.Any(x => x.Seq == certification.Seq))
                        return new OperationResult(false, "AlreadyCertification");
                    serNum = string.IsNullOrWhiteSpace(serNum) ? serNumStart + "0000001" : serNum[0..8] + (int.Parse(serNum[9..]) + 1).ToString("0000000");
                    HRMS_Emp_Certification addCertification = new()
                    {
                        Division = input.Param.Division,
                        Factory = input.Param.Factory,
                        Employee_ID = input.Param.Employee_Id,
                        Seq = certification.Seq,
                        Certification = certification.Category_Of_Certification,
                        Name_Of_Certification = certification.Name_Of_Certification,
                        Score = certification.Score,
                        Level = certification.Level,
                        Result = certification.Result,
                        Passing_Date = Convert.ToDateTime(certification.Passing_Date_Str),
                        Certification_Valid_Period = !string.IsNullOrWhiteSpace(certification.Certification_Valid_Period_Str) ? Convert.ToDateTime(certification.Certification_Valid_Period_Str) : null,
                        Remark = certification.Remark,
                        Program_Code = "4.1.10",
                        SerNum = serNum,
                        Update_By = certification.Update_By,
                        Update_Time = Convert.ToDateTime(certification.Update_Time_Str)
                    };
                    addCertifications.Add(addCertification);
                    string path = $"{folder}\\{addCertification.Factory}\\{addCertification.Division}\\{addCertification.Employee_ID}\\{addCertification.SerNum}";
                    foreach (Certifications_FileModel file in certification.File_List)
                    {
                        var fileNameArray = file.Name.Split(".");
                        string savedFileName = await FilesUtility.UploadAsync(file.Content, path, fileNameArray[0], fileNameArray[^1]);
                        if (string.IsNullOrWhiteSpace(savedFileName))
                            return new OperationResult(false, "SaveFileError");
                        HRMS_Emp_File addFile = new()
                        {
                            Division = addCertification.Division,
                            Factory = addCertification.Factory,
                            Program_Code = addCertification.Program_Code,
                            SerNum = addCertification.SerNum,
                            FileID = file.Id,
                            FileName = savedFileName,
                            FileSize = file.Size,
                            Update_By = addCertification.Update_By,
                            Update_Time = addCertification.Update_Time
                        };
                        addFiles.Add(addFile);
                    }
                }
                if (addCertifications.Any())
                    _repositoryAccessor.HRMS_Emp_Certification.AddMultiple(addCertifications);
                if (addFiles.Any())
                    _repositoryAccessor.HRMS_Emp_File.AddMultiple(addFiles);
                await _repositoryAccessor.Save();
                await _repositoryAccessor.CommitAsync();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult(false);
            }
        }

        public async Task<OperationResult> DeleteData(Certifications_SubModel data, string userName)
        {
            var predicateCertification = PredicateBuilder.New<HRMS_Emp_Certification>(true);
            var predicateCertificationSeq = PredicateBuilder.New<HRMS_Emp_Certification>(x => x.Seq == data.Seq);
            var predicateFile = PredicateBuilder.New<HRMS_Emp_File>(x => x.Program_Code == "4.1.10");
            if (!string.IsNullOrWhiteSpace(data.Division))
            {
                predicateCertification.And(x => x.Division == data.Division);
                predicateCertificationSeq.And(x => x.Division == data.Division);
                predicateFile.And(x => x.Division == data.Division);
            }
            if (!string.IsNullOrWhiteSpace(data.Factory))
            {
                predicateCertification.And(x => x.Factory == data.Factory);
                predicateCertificationSeq.And(x => x.Factory == data.Factory);
                predicateFile.And(x => x.Factory == data.Factory);
            }
            if (!string.IsNullOrWhiteSpace(data.Employee_Id))
            {
                predicateCertification.And(x => x.Employee_ID == data.Employee_Id);
                predicateCertificationSeq.And(x => x.Employee_ID == data.Employee_Id);
            }
            HRMS_Emp_Certification selected_HEC = await _repositoryAccessor.HRMS_Emp_Certification.FirstOrDefaultAsync(predicateCertificationSeq);
            if (selected_HEC == null)
                return new OperationResult(false, "NotExitedData");
            predicateFile.And(x => x.SerNum == selected_HEC.SerNum);
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                _repositoryAccessor.HRMS_Emp_Certification.Remove(selected_HEC);
                List<HRMS_Emp_File> HEFs = await _repositoryAccessor.HRMS_Emp_File.FindAll(predicateFile).ToListAsync();
                if (HEFs.Any())
                {
                    _repositoryAccessor.HRMS_Emp_File.RemoveMultiple(HEFs);
                    string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    string path = $"{webRootPath}\\{folder}\\{selected_HEC.Factory}\\{selected_HEC.Division}\\{selected_HEC.Employee_ID}\\{selected_HEC.SerNum}";
                    if (Directory.Exists(path))
                        Directory.Delete(path, true);
                }
                await _repositoryAccessor.Save();
                List<HRMS_Emp_Certification> HECs = await _repositoryAccessor.HRMS_Emp_Certification.FindAll(predicateCertification).ToListAsync();
                if (HECs.Any())
                {
                    List<HRMS_Emp_Certification> readd_List = new();
                    List<HRMS_Emp_Certification> remove_List = new();
                    for (int i = 0; i < HECs.Count; i++)
                    {
                        if (HECs[i].Seq != i + 1)
                        {
                            HRMS_Emp_Certification add_HEC = new()
                            {
                                Division = HECs[i].Division,
                                Factory = HECs[i].Factory,
                                Employee_ID = HECs[i].Employee_ID,
                                Certification = HECs[i].Certification,
                                Name_Of_Certification = HECs[i].Name_Of_Certification,
                                Score = HECs[i].Score,
                                Level = HECs[i].Level,
                                Result = HECs[i].Result,
                                Passing_Date = HECs[i].Passing_Date,
                                Certification_Valid_Period = HECs[i].Certification_Valid_Period,
                                Remark = HECs[i].Remark,
                                Program_Code = HECs[i].Program_Code,
                                SerNum = HECs[i].SerNum,
                                Seq = i + 1,
                                Update_By = userName,
                                Update_Time = DateTime.Now,
                            };
                            remove_List.Add(HECs[i]);
                            readd_List.Add(add_HEC);
                        }
                    }
                    if (remove_List.Any())
                    {
                        _repositoryAccessor.HRMS_Emp_Certification.RemoveMultiple(remove_List);
                        await _repositoryAccessor.Save();
                        _repositoryAccessor.HRMS_Emp_Certification.AddMultiple(readd_List);
                        await _repositoryAccessor.Save();
                    }
                }
                await _repositoryAccessor.CommitAsync();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                await _repositoryAccessor.RollbackAsync();
                return new OperationResult(false);
            }
        }

        public async Task<OperationResult> DownloadExcel(Certifications_MainParam param, List<string> roleList)
        {
            ExcelResult excelResult = ExcelUtility.DownloadExcel(
                await GetData(param, roleList, true), 
                "Resources\\Template\\EmployeeMaintenance\\4_1_10_Certifications\\Download.xlsx"
            );
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }
        private async Task<List<Certifications_MainData>> GetData(Certifications_MainParam searchParam, List<string> roleList, bool isDownload = false)
        {
            var predicateCertification = PredicateBuilder.New<HRMS_Emp_Certification>(true);
            var predicatePersonal = PredicateBuilder.New<HRMS_Emp_Personal>(true);
            var predicateDepartment = PredicateBuilder.New<HRMS_Org_Department>(true);
            var predicateDepartmentLang = PredicateBuilder.New<HRMS_Org_Department_Language>(x => x.Language_Code.ToLower() == searchParam.Lang.ToLower());
            var predicateFile = PredicateBuilder.New<HRMS_Emp_File>(x => x.Program_Code == "4.1.10");

            if (!string.IsNullOrWhiteSpace(searchParam.Division))
            {
                predicateCertification.And(x => x.Division == searchParam.Division);
                predicateFile.And(x => x.Division == searchParam.Division);
                predicateDepartment.And(x => x.Division == searchParam.Division);
                predicateDepartmentLang.And(x => x.Division == searchParam.Division);
            }
            if (!string.IsNullOrWhiteSpace(searchParam.Factory))
            {
                predicateCertification.And(x => x.Factory == searchParam.Factory);
                predicateFile.And(x => x.Factory == searchParam.Factory);
                predicateDepartment.And(x => x.Factory == searchParam.Factory);
                predicateDepartmentLang.And(x => x.Factory == searchParam.Factory);
            }
            if (!string.IsNullOrWhiteSpace(searchParam.Employee_Id))
            {
                predicateCertification.And(x => x.Employee_ID.ToLower().Contains(searchParam.Employee_Id.ToLower().Trim()));
            }
            if (!string.IsNullOrWhiteSpace(searchParam.Local_Full_Name))
                predicatePersonal.And(x => x.Local_Full_Name.ToLower().Replace(" ", "").Contains(searchParam.Local_Full_Name.ToLower().Replace(" ", "")));
            if (!string.IsNullOrWhiteSpace(searchParam.Category_Of_Certification))
                predicateCertification.And(x => x.Certification == searchParam.Category_Of_Certification);
            if (!string.IsNullOrWhiteSpace(searchParam.Passing_Date_From_Str))
                predicateCertification.And(x => x.Passing_Date >= Convert.ToDateTime(searchParam.Passing_Date_From_Str));
            if (!string.IsNullOrWhiteSpace(searchParam.Passing_Date_To_Str))
                predicateCertification.And(x => x.Passing_Date <= Convert.ToDateTime(searchParam.Passing_Date_To_Str));
            if (!string.IsNullOrWhiteSpace(searchParam.Certification_Valid_Period_From_Str))
                predicateCertification.And(x => x.Certification_Valid_Period >= Convert.ToDateTime(searchParam.Certification_Valid_Period_From_Str));
            if (!string.IsNullOrWhiteSpace(searchParam.Certification_Valid_Period_To_Str))
                predicateCertification.And(x => x.Certification_Valid_Period <= Convert.ToDateTime(searchParam.Certification_Valid_Period_To_Str));
            var HEC = _repositoryAccessor.HRMS_Emp_Certification.FindAll(predicateCertification).ToList();
            var HEP = await Query_Permission_Data_Filter(roleList, predicatePersonal);
            var HBC = _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "30");
            var HBCL = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == searchParam.Lang.ToLower());
            var HBC_Lang = HBC.GroupJoin(HBCL,
                x => new { x.Type_Seq, x.Code },
                y => new { y.Type_Seq, y.Code },
                    (x, y) => new { HBC = x, HBCL = y })
                .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                    (x, y) => new { x.HBC, HBCL = y })
                .Select(x => new
                {
                    x.HBC.Code,
                    Code_Name = x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name
                })
                .Distinct();
            var data = HEC
                .Join(HEP,
                    x => new { x.Division, x.Factory, x.Employee_ID },
                    y => new { y.Division, y.Factory, y.Employee_ID },
                    (x, y) => new { HEC = x, HEP = y });
            List<Certifications_MainData> result = new();
            if (isDownload)
            {
                var HOD = _repositoryAccessor.HRMS_Org_Department.FindAll(predicateDepartment);
                var HODL = _repositoryAccessor.HRMS_Org_Department_Language.FindAll(predicateDepartmentLang);
                var HOD_Lang = HOD
                    .GroupJoin(HODL,
                        x => x.Department_Code,
                        y => y.Department_Code,
                        (x, y) => new { HOD = x, HODL = y })
                    .SelectMany(x => x.HODL.DefaultIfEmpty(),
                        (x, y) => new { x.HOD, HODL = y })
                    .Select(x => new
                    {
                        x.HOD.Department_Code,
                        Department_Name = x.HODL != null ? x.HODL.Name : x.HOD.Department_Name
                    });
                result = data
                    .GroupJoin(HBC_Lang,
                        x => x.HEC.Certification,
                        y => y.Code,
                        (x, y) => new { x.HEC, x.HEP, HBC_Lang = y })
                    .SelectMany(x => x.HBC_Lang.DefaultIfEmpty(),
                        (x, y) => new { x.HEC, x.HEP, HBC_Lang = y })
                    .GroupJoin(HOD_Lang,
                        x => x.HEP.Department,
                        y => y.Department_Code,
                        (x, y) => new { x.HEC, x.HEP, x.HBC_Lang, HOD_Lang = y })
                    .SelectMany(x => x.HOD_Lang.DefaultIfEmpty(),
                        (x, y) => new { x.HEC, x.HEP, x.HBC_Lang, HOD_Lang = y })
                    .Select(x => new Certifications_MainData
                    {
                        Division = x.HEC.Division,
                        Factory = x.HEC.Factory,
                        Employee_Id = x.HEC.Employee_ID,
                        Local_Full_Name = x.HEP.Local_Full_Name,
                        Department = x.HEP.Department,
                        Department_Name = x.HOD_Lang.Department_Name ?? "",
                        Category_Of_Certification = $"{x.HEC.Certification}-{x.HBC_Lang.Code_Name ?? ""}",
                        Name_Of_Certification = x.HEC.Name_Of_Certification,
                        Score = x.HEC.Score,
                        Level = x.HEC.Level,
                        Result_Str = x.HEC.Result ? "Y" : "N",
                        Passing_Date_Str = x.HEC.Passing_Date.ToString("yyyy/MM/dd"),
                        Certification_Valid_Period_Str = x.HEC.Certification_Valid_Period.Value.ToString("yyyy/MM/dd"),
                        Remark = x.HEC.Remark
                    })
                    .OrderBy(x => x.Employee_Id)
                    .ToList();
                return result;
            }
            var HEF = _repositoryAccessor.HRMS_Emp_File.FindAll(predicateFile);
            result = data
                .GroupJoin(HEF,
                    x => new { x.HEC.Division, x.HEC.Factory, x.HEC.Program_Code, x.HEC.SerNum },
                    y => new { y.Division, y.Factory, y.Program_Code, y.SerNum },
                    (x, y) => new { x.HEC, x.HEP, HEF = y })
                .SelectMany(x => x.HEF.DefaultIfEmpty(),
                    (x, y) => new { x.HEC, x.HEP, HEF = y })
                .GroupBy(x => x.HEC)
                .Select(x => new Certifications_MainData
                {
                    Division = x.Key.Division,
                    Factory = x.Key.Factory,
                    Employee_Id = x.Key.Employee_ID,
                    Local_Full_Name = x.FirstOrDefault(y => y.HEP != null).HEP.Local_Full_Name,
                    Seq = x.Key.Seq,
                    Category_Of_Certification = $"{x.Key.Certification}-{HBC_Lang.FirstOrDefault(y => y.Code == x.Key.Certification).Code_Name}",
                    Name_Of_Certification = x.Key.Name_Of_Certification,
                    Score = x.Key.Score,
                    Level = x.Key.Level,
                    Result = x.Key.Result,
                    Passing_Date = x.Key.Passing_Date,
                    Certification_Valid_Period = x.Key.Certification_Valid_Period,
                    Remark = x.Key.Remark,
                    Update_By = x.Key.Update_By,
                    Update_Time = x.Key.Update_Time,
                    Ser_Num = x.Key.SerNum,
                    File_List = x.Where(y => y.HEF != null).OrderBy(y => y.HEF.FileID).Select(y => new Certifications_FileModel
                    {
                        Id = y.HEF.FileID,
                        Name = y.HEF.FileName,
                        Size = Convert.ToInt32(y.HEF.FileSize)
                    }).GroupBy(x => new { x.Id, x.Name, x.Size }).Select(x => x.First()).ToList()
                })
                .OrderBy(x => x.Employee_Id)
                .ThenBy(x => x.Seq)
                .ToList();
            return result;
        }
    }
}