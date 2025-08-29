using API.Data;
using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.EmployeeMaintenance
{
    public class S_4_1_9_DocumentManagement : BaseServices, I_4_1_9_DocumentManagement
    {
        private readonly string folder = "uploaded\\EmployeeMaintenance\\4_1_9_DocumentManagement";
        public S_4_1_9_DocumentManagement(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<KeyValuePair<string, string>>> GetDropDownList(DocumentManagement_MainParam param)
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
            result.AddRange(data.Where(x => x.hbc.Type_Seq == "29").Select(x => new KeyValuePair<string, string>("DT", $"{x.hbc.Code}-{(x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name)}")).Distinct().ToList());
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
        public async Task<List<DocumentManagement_TypeheadKeyValue>> GetEmployeeList(DocumentManagement_SubParam param)
        {
            var HEP = await _repositoryAccessor.HRMS_Emp_Personal.FindAll(x => x.Factory == param.Factory && x.Employee_ID.Contains(param.Employee_Id)).ToListAsync();
            var HED = _repositoryAccessor.HRMS_Emp_Document.FindAll(x => x.Division == param.Division && x.Factory == param.Factory).ToList();
            var data = HEP
                .GroupJoin(HED,
                    x => x.Employee_ID,
                    y => y.Employee_ID,
                    (x, y) => new { HEP = x, HED = y })
                .SelectMany(x => x.HED.DefaultIfEmpty(),
                    (x, y) => new { x.HEP, HED = y })
                .GroupBy(x => x.HEP);
            var result = data
                .Select(x => new DocumentManagement_TypeheadKeyValue
                {
                    UseR_GUID = x.Key.USER_GUID,
                    Key = x.Key.Employee_ID,
                    Local_Full_Name = x.Key.Local_Full_Name,
                    Max_Seq = x.Any(y => y.HED != null) ? x.Max(y => y.HED.Seq) : 0
                })
                .ToList();
            return result;
        }
        public async Task<PaginationUtility<DocumentManagement_MainData>> GetSearchDetail(PaginationParam paginationParams, DocumentManagement_MainParam searchParam, List<string> roleList)
        {
            return PaginationUtility<DocumentManagement_MainData>.Create(await GetData(searchParam, roleList), paginationParams.PageNumber, paginationParams.PageSize);
        }
        public async Task<OperationResult> GetSubDetail(DocumentManagement_SubParam param)
        {
            var predicateDocument = PredicateBuilder.New<HRMS_Emp_Document>(true);
            var predicateFile = PredicateBuilder.New<HRMS_Emp_File>(x => x.Program_Code == "4.1.9");
            if (!string.IsNullOrWhiteSpace(param.Division))
            {
                predicateDocument.And(x => x.Division == param.Division);
                predicateFile.And(x => x.Division == param.Division);
            }
            if (!string.IsNullOrWhiteSpace(param.Factory))
            {
                predicateDocument.And(x => x.Factory == param.Factory);
                predicateFile.And(x => x.Factory == param.Factory);
            }
            if (!string.IsNullOrWhiteSpace(param.Employee_Id))
                predicateDocument.And(x => x.Employee_ID == param.Employee_Id);
            var HED = _repositoryAccessor.HRMS_Emp_Document.FindAll(predicateDocument);
            if (!HED.Any())
                return new OperationResult(false, "NotExitedData");
            var HEF = _repositoryAccessor.HRMS_Emp_File.FindAll(predicateFile);
            var data = HED
                .GroupJoin(HEF,
                    x => new { x.Division, x.Factory, x.Program_Code, x.SerNum },
                    y => new { y.Division, y.Factory, y.Program_Code, y.SerNum },
                    (x, y) => new { HED = x, HEF = y })
                .SelectMany(x => x.HEF.DefaultIfEmpty(),
                    (x, y) => new { x.HED, HEF = y })
                .GroupBy(x => x.HED);
            var result = await data.Select(x => new DocumentManagement_SubData
            {
                Document_Type = x.Key.Document_Type,
                Passport_Full_Name = x.Key.Passport_Name,
                Seq = x.Key.Seq,
                Document_Number = x.Key.Document_Number,
                Validity_Date_From = x.Key.Validity_Start,
                Validity_Date_To = x.Key.Validity_End,
                Update_By = x.Key.Update_By,
                Update_Time = x.Key.Update_Time,
                Ser_Num = x.Key.SerNum,
                File_List = x.Where(y => y.HEF != null).OrderBy(y => y.HEF.FileID).Select(y => new DocumentManagement_FileModel
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
        public async Task<OperationResult> PostData(DocumentManagement_SubMemory input)
        {
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                var HED = _repositoryAccessor.HRMS_Emp_Document.FindAll(x => x.Program_Code == "4.1.9");
                string serNumStart = DateTime.Now.ToString("yyyyMMdd");
                string serNum = "";
                var recentFiles = HED.Where(x => x.SerNum.StartsWith(serNumStart)).OrderByDescending(x => x.SerNum);
                if (recentFiles.Any())
                    serNum = recentFiles.FirstOrDefault().SerNum;
                List<HRMS_Emp_Document> addDocuments = new();
                List<HRMS_Emp_File> addFiles = new();
                var _HED = HED.Where(x =>
                    x.Factory == input.Param.Factory &&
                    x.Division == input.Param.Division &&
                    x.Employee_ID == input.Param.Employee_Id);
                foreach (var document in input.Data)
                {
                    if (_HED.Any(x => x.Document_Type == document.Document_Type && x.Seq == document.Seq))
                        return new OperationResult(false, "AlreadyExitedDocument");
                    serNum = string.IsNullOrWhiteSpace(serNum) ? serNumStart + "0000001" : serNum[0..8] + (int.Parse(serNum[9..]) + 1).ToString("0000000");
                    HRMS_Emp_Document addDocument = new()
                    {
                        Division = input.Param.Division,
                        Factory = input.Param.Factory,
                        Employee_ID = input.Param.Employee_Id,
                        Document_Type = document.Document_Type,
                        Seq = document.Seq,
                        Passport_Name = document.Passport_Full_Name,
                        Document_Number = document.Document_Number,
                        Validity_Start = Convert.ToDateTime(document.Validity_Date_From_Str),
                        Validity_End = Convert.ToDateTime(document.Validity_Date_To_Str),
                        Program_Code = "4.1.9",
                        SerNum = serNum,
                        Update_By = document.Update_By,
                        Update_Time = Convert.ToDateTime(document.Update_Time_Str)
                    };
                    addDocuments.Add(addDocument);
                    string path = $"{folder}\\{addDocument.Factory}\\{addDocument.Division}\\{addDocument.Employee_ID}\\{addDocument.SerNum}";
                    foreach (DocumentManagement_FileModel file in document.File_List)
                    {
                        var fileNameArray = file.Name.Split(".");
                        string savedFileName = await FilesUtility.UploadAsync(file.Content, path, fileNameArray[0], fileNameArray[^1]);
                        if (string.IsNullOrWhiteSpace(savedFileName))
                            return new OperationResult(false, "SaveFileError");
                        HRMS_Emp_File addFile = new()
                        {
                            Division = addDocument.Division,
                            Factory = addDocument.Factory,
                            Program_Code = addDocument.Program_Code,
                            SerNum = addDocument.SerNum,
                            FileID = file.Id,
                            FileName = savedFileName,
                            FileSize = file.Size,
                            Update_By = addDocument.Update_By,
                            Update_Time = addDocument.Update_Time
                        };
                        addFiles.Add(addFile);
                    }
                }
                if (addDocuments.Any())
                    _repositoryAccessor.HRMS_Emp_Document.AddMultiple(addDocuments);
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

        public async Task<OperationResult> DownloadFile(DocumentManagement_DownloadFileModel param)
        {
            var file = await _repositoryAccessor.HRMS_Emp_File.FirstOrDefaultAsync(x =>
                x.Division == param.Division &&
                x.Factory == param.Factory &&
                x.SerNum == param.Ser_Num &&
                x.FileName == param.File_Name &&
                x.Program_Code == "4.1.9"
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

        public async Task<OperationResult> DeleteData(DocumentManagement_SubModel data, string userName)
        {
            var predicateDocument = PredicateBuilder.New<HRMS_Emp_Document>(true);
            var predicateDocumentSeq = PredicateBuilder.New<HRMS_Emp_Document>(x => x.Seq == data.Seq);
            var predicateFile = PredicateBuilder.New<HRMS_Emp_File>(x => x.Program_Code == "4.1.9");
            if (!string.IsNullOrWhiteSpace(data.Division))
            {
                predicateDocument.And(x => x.Division == data.Division);
                predicateDocumentSeq.And(x => x.Division == data.Division);
                predicateFile.And(x => x.Division == data.Division);
            }
            if (!string.IsNullOrWhiteSpace(data.Factory))
            {
                predicateDocument.And(x => x.Factory == data.Factory);
                predicateDocumentSeq.And(x => x.Factory == data.Factory);
                predicateFile.And(x => x.Factory == data.Factory);
            }
            if (!string.IsNullOrWhiteSpace(data.Employee_Id))
            {
                predicateDocument.And(x => x.Employee_ID == data.Employee_Id);
                predicateDocumentSeq.And(x => x.Employee_ID == data.Employee_Id);
            }
            if (!string.IsNullOrWhiteSpace(data.Document_Type))
                predicateDocumentSeq.And(x => x.Document_Type == data.Document_Type);
            HRMS_Emp_Document selected_HED = await _repositoryAccessor.HRMS_Emp_Document.FirstOrDefaultAsync(predicateDocumentSeq);
            if (selected_HED == null)
                return new OperationResult(false, "NotExitedData");
            predicateFile.And(x => x.SerNum == selected_HED.SerNum);
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                _repositoryAccessor.HRMS_Emp_Document.Remove(selected_HED);
                List<HRMS_Emp_File> HEFs = await _repositoryAccessor.HRMS_Emp_File.FindAll(predicateFile).ToListAsync();
                if (HEFs.Any())
                {
                    _repositoryAccessor.HRMS_Emp_File.RemoveMultiple(HEFs);
                    string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    string path = $"{webRootPath}\\{folder}\\{selected_HED.Factory}\\{selected_HED.Division}\\{selected_HED.Employee_ID}\\{selected_HED.SerNum}";
                    if (Directory.Exists(path))
                        Directory.Delete(path, true);
                }
                await _repositoryAccessor.Save();
                List<HRMS_Emp_Document> HEDs = await _repositoryAccessor.HRMS_Emp_Document.FindAll(predicateDocument).ToListAsync();
                if (HEDs.Any())
                {
                    List<HRMS_Emp_Document> readd_List = new();
                    List<HRMS_Emp_Document> remove_List = new();
                    for (int i = 0; i < HEDs.Count; i++)
                    {
                        if (HEDs[i].Seq != i + 1)
                        {
                            HRMS_Emp_Document add_HED = new()
                            {
                                Division = HEDs[i].Division,
                                Factory = HEDs[i].Factory,
                                Employee_ID = HEDs[i].Employee_ID,
                                Document_Type = HEDs[i].Document_Type,
                                Passport_Name = HEDs[i].Passport_Name,
                                Document_Number = HEDs[i].Document_Number,
                                Validity_Start = HEDs[i].Validity_Start,
                                Validity_End = HEDs[i].Validity_End,
                                Program_Code = HEDs[i].Program_Code,
                                SerNum = HEDs[i].SerNum,
                                Seq = i + 1,
                                Update_By = userName,
                                Update_Time = DateTime.Now,
                            };
                            remove_List.Add(HEDs[i]);
                            readd_List.Add(add_HED);
                        }
                    }
                    if (remove_List.Any())
                    {
                        _repositoryAccessor.HRMS_Emp_Document.RemoveMultiple(remove_List);
                        await _repositoryAccessor.Save();
                        _repositoryAccessor.HRMS_Emp_Document.AddMultiple(readd_List);
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

        public async Task<OperationResult> PutData(DocumentManagement_SubMemory input)
        {
            var predicateDocument = PredicateBuilder.New<HRMS_Emp_Document>(true);
            var predicateFile = PredicateBuilder.New<HRMS_Emp_File>(x => x.Program_Code == "4.1.9");
            if (!string.IsNullOrWhiteSpace(input.Param.Division))
            {
                predicateDocument.And(x => x.Division == input.Param.Division);
                predicateFile.And(x => x.Division == input.Param.Division);
            }
            if (!string.IsNullOrWhiteSpace(input.Param.Factory))
            {
                predicateDocument.And(x => x.Factory == input.Param.Factory);
                predicateFile.And(x => x.Factory == input.Param.Factory);
            }
            if (!string.IsNullOrWhiteSpace(input.Param.Employee_Id))
                predicateDocument.And(x => x.Employee_ID == input.Param.Employee_Id);
            List<HRMS_Emp_Document> docRemoveList = await _repositoryAccessor.HRMS_Emp_Document.FindAll(predicateDocument).ToListAsync();
            if (!docRemoveList.Any())
                return new OperationResult(false, "NotExitedData");
            await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                List<HRMS_Emp_File> HEFs = _repositoryAccessor.HRMS_Emp_File.FindAll(predicateFile).ToList();
                List<string> serNumList = docRemoveList.Select(x => x.SerNum).ToList();
                List<HRMS_Emp_File> fileRemoveList = HEFs.Where(x => serNumList.Contains(x.SerNum)).ToList();
                List<HRMS_Emp_Document> docAddList = new();
                List<HRMS_Emp_File> fileAddList = new();
                string serNumStart = DateTime.Now.ToString("yyyyMMdd");
                string serNum = "";
                var recentFiles = docRemoveList.Where(x => x.SerNum.StartsWith(serNumStart)).OrderByDescending(x => x.SerNum);
                if (recentFiles.Any())
                    serNum = recentFiles.FirstOrDefault().SerNum;
                foreach (var document in input.Data)
                {
                    string selectedSerNum = "";
                    if (!string.IsNullOrWhiteSpace(document.Ser_Num))
                        selectedSerNum = document.Ser_Num;
                    else
                    {
                        serNum = string.IsNullOrWhiteSpace(serNum) ? serNumStart + "0000001" : serNum[0..8] + (int.Parse(serNum[9..]) + 1).ToString("0000000");
                        selectedSerNum = serNum;
                    }
                    string location = $"{folder}\\{input.Param.Factory}\\{input.Param.Division}\\{input.Param.Employee_Id}\\{selectedSerNum}";
                    HRMS_Emp_Document docAdd = new()
                    {
                        Division = input.Param.Division,
                        Factory = input.Param.Factory,
                        Employee_ID = input.Param.Employee_Id,
                        Document_Type = document.Document_Type,
                        Seq = document.Seq,
                        Passport_Name = document.Passport_Full_Name,
                        Document_Number = document.Document_Number,
                        Validity_Start = Convert.ToDateTime(document.Validity_Date_From_Str),
                        Validity_End = Convert.ToDateTime(document.Validity_Date_To_Str),
                        Program_Code = "4.1.9",
                        SerNum = selectedSerNum,
                        Update_By = document.Update_By,
                        Update_Time = Convert.ToDateTime(document.Update_Time_Str)
                    };
                    docAddList.Add(docAdd);
                    if (!document.File_List.Any())
                    {
                        string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                        string path = $"{webRootPath}\\{location}";
                        if (Directory.Exists(path))
                            Directory.Delete(path, true);
                    }
                    foreach (var file in document.File_List)
                    {
                        HRMS_Emp_File fileAdd = new()
                        {
                            Division = docAdd.Division,
                            Factory = docAdd.Factory,
                            Program_Code = "4.1.9",
                            SerNum = selectedSerNum,
                            FileID = file.Id,
                            FileName = file.Name,
                            FileSize = file.Size,
                            Update_By = docAdd.Update_By,
                            Update_Time = docAdd.Update_Time
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
                _repositoryAccessor.HRMS_Emp_Document.RemoveMultiple(docRemoveList);
                if (fileRemoveList.Any())
                    _repositoryAccessor.HRMS_Emp_File.RemoveMultiple(fileRemoveList);
                await _repositoryAccessor.Save();
                if (docAddList.Any())
                    _repositoryAccessor.HRMS_Emp_Document.AddMultiple(docAddList);
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
        public async Task<OperationResult> CheckExistedData(DocumentManagement_SubModel param)
        {
            return new OperationResult(await _repositoryAccessor.HRMS_Emp_Document.AnyAsync(x => x.Division == param.Division && x.Factory == param.Factory && x.Employee_ID == param.Employee_Id));
        }
        public async Task<OperationResult> DownloadExcel(DocumentManagement_MainParam param, List<string> roleList)
        {
            ExcelResult excelResult = ExcelUtility.DownloadExcel(
                await GetData(param, roleList, true),
                "Resources\\Template\\EmployeeMaintenance\\4_1_9_DocumentManagement\\Download.xlsx"
            );
            return new OperationResult(excelResult.IsSuccess, excelResult.Error, excelResult.Result);
        }
        private async Task<List<DocumentManagement_MainData>> GetData(DocumentManagement_MainParam searchParam, List<string> roleList, bool isDownload = false)
        {
            var predicateDocument = PredicateBuilder.New<HRMS_Emp_Document>(true);
            var predicatePersonal = PredicateBuilder.New<HRMS_Emp_Personal>(true);
            var predicateFile = PredicateBuilder.New<HRMS_Emp_File>(x => x.Program_Code == "4.1.9");

            if (!string.IsNullOrWhiteSpace(searchParam.Division))
            {
                predicateDocument.And(x => x.Division == searchParam.Division);
                predicateFile.And(x => x.Division == searchParam.Division);

            }
            if (!string.IsNullOrWhiteSpace(searchParam.Factory))
            {
                predicateDocument.And(x => x.Factory == searchParam.Factory);
                predicateFile.And(x => x.Factory == searchParam.Factory);
            }
            if (!string.IsNullOrWhiteSpace(searchParam.Employee_Id))
            {
                predicateDocument.And(x => x.Employee_ID.ToLower().Contains(searchParam.Employee_Id.ToLower().Trim()));
            }
            if (!string.IsNullOrWhiteSpace(searchParam.Local_Full_Name))
                predicatePersonal.And(x => x.Local_Full_Name.ToLower().Replace(" ", "").Contains(searchParam.Local_Full_Name.ToLower().Replace(" ", "")));
            if (!string.IsNullOrWhiteSpace(searchParam.Document_Type))
                predicateDocument.And(x => x.Document_Type == searchParam.Document_Type);
            if (!string.IsNullOrWhiteSpace(searchParam.Validity_Date_Start_From_Str))
                predicateDocument.And(x => x.Validity_Start.Date >= Convert.ToDateTime(searchParam.Validity_Date_Start_From_Str).Date);
            if (!string.IsNullOrWhiteSpace(searchParam.Validity_Date_Start_To_Str))
                predicateDocument.And(x => x.Validity_Start.Date <= Convert.ToDateTime(searchParam.Validity_Date_Start_To_Str).Date);
            if (!string.IsNullOrWhiteSpace(searchParam.Validity_Date_End_From_Str))
                predicateDocument.And(x => x.Validity_End.Date >= Convert.ToDateTime(searchParam.Validity_Date_End_From_Str).Date);
            if (!string.IsNullOrWhiteSpace(searchParam.Validity_Date_End_To_Str))
                predicateDocument.And(x => x.Validity_End.Date <= Convert.ToDateTime(searchParam.Validity_Date_End_To_Str).Date);
            var HED = _repositoryAccessor.HRMS_Emp_Document.FindAll(predicateDocument).ToList();
            var HEP = await Query_Permission_Data_Filter(roleList, predicatePersonal);
            var HBC = _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "29");
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
            var data = HED
                .Join(HEP,
                    x => new { x.Division, x.Factory, x.Employee_ID },
                    y => new { y.Division, y.Factory, y.Employee_ID },
                    (x, y) => new { HED = x, HEP = y });
            List<DocumentManagement_MainData> result = new();
            if (isDownload)
            {
                result = data
                .GroupJoin(HBC_Lang,
                    x => x.HED.Document_Type,
                    y => y.Code,
                    (x, y) => new { x.HED, x.HEP, HBC_Lang = y })
                .SelectMany(x => x.HBC_Lang.DefaultIfEmpty(),
                    (x, y) => new { x.HED, x.HEP, HBC_Lang = y })
                .Select(x => new DocumentManagement_MainData
                {
                    Division = x.HED.Division,
                    Factory = x.HED.Factory,
                    Employee_Id = x.HED.Employee_ID,
                    Local_Full_Name = x.HEP.Local_Full_Name,
                    Document_Type_Name = $"{x.HED.Document_Type}-{x.HBC_Lang.Code_Name ?? ""}",
                    Passport_Full_Name = x.HED.Passport_Name,
                    Seq = x.HED.Seq,
                    Document_Number = x.HED.Document_Number,
                    Validity_Date_From_Str = x.HED.Validity_Start.ToString("yyyy/MM/dd"),
                    Validity_Date_To_Str = x.HED.Validity_End.ToString("yyyy/MM/dd")
                })
                .OrderBy(x => x.Employee_Id)
                .ThenBy(x => x.Seq)
                .ToList();
                return result;
            }
            var HEF = _repositoryAccessor.HRMS_Emp_File.FindAll(predicateFile);
            result = data
                .GroupJoin(HEF,
                    x => new { x.HED.Division, x.HED.Factory, x.HED.Program_Code, x.HED.SerNum },
                    y => new { y.Division, y.Factory, y.Program_Code, y.SerNum },
                    (x, y) => new { x.HED, x.HEP, HEF = y })
                .SelectMany(x => x.HEF.DefaultIfEmpty(),
                    (x, y) => new { x.HED, x.HEP, HEF = y })
                .GroupBy(x => x.HED)
                .Select(x => new DocumentManagement_MainData
                {
                    Division = x.Key.Division,
                    Factory = x.Key.Factory,
                    Employee_Id = x.Key.Employee_ID,
                    Local_Full_Name = x.FirstOrDefault(y => y.HEP != null).HEP.Local_Full_Name,
                    Document_Type = x.Key.Document_Type,
                    Document_Type_Name = $"{x.Key.Document_Type}-{HBC_Lang.FirstOrDefault(y => y.Code == x.Key.Document_Type).Code_Name}",
                    Passport_Full_Name = x.Key.Passport_Name,
                    Seq = x.Key.Seq,
                    Document_Number = x.Key.Document_Number,
                    Validity_Date_From = x.Key.Validity_Start,
                    Validity_Date_To = x.Key.Validity_End,
                    Update_By = x.Key.Update_By,
                    Update_Time = x.Key.Update_Time,
                    Ser_Num = x.Key.SerNum,
                    File_List = x.Where(y => y.HEF != null).OrderBy(y => y.HEF.FileID).Select(y => new DocumentManagement_FileModel
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