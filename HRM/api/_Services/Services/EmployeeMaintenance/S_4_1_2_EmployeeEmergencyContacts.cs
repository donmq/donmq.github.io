using AgileObjects.AgileMapper;
using API.Data;
using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using API.Helper.Constant;
using API.Models;
using Aspose.Cells;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Cell = SDCores.Cell;

namespace API._Services.Services.EmployeeMaintenance
{
    public class S_4_1_2_EmployeeEmergencyContacts : BaseServices, I_4_1_2_EmployeeEmergencyContacts
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public S_4_1_2_EmployeeEmergencyContacts(DBContext dbContext,IWebHostEnvironment webHostEnvironment): base(dbContext)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<OperationResult> Create(EmployeeEmergencyContactsDto model)
        {
            if(await _repositoryAccessor.HRMS_Emp_Emergency_Contact.AnyAsync(x => x.USER_GUID == model.USER_GUID.Trim()                                                                              
                                                                                && x.Seq == model.Seq)) 
            return new OperationResult(false, "Seq already exists.");

            var newItem = Mapper.Map(model).ToANew<HRMS_Emp_Emergency_Contact>(x => x.MapEntityKeys());
            _repositoryAccessor.HRMS_Emp_Emergency_Contact.Add(newItem);           
            return new OperationResult(await _repositoryAccessor.Save());         
        }

        public async Task<OperationResult> Delete(EmployeeEmergencyContactsDto model)
        {
            var item = await _repositoryAccessor.HRMS_Emp_Emergency_Contact.FirstOrDefaultAsync(x => x.USER_GUID == model.USER_GUID.Trim()  
                                                                                && x.Seq == model.Seq);
            if (item != null)
            {
                _repositoryAccessor.HRMS_Emp_Emergency_Contact.Remove(item);              
                return new OperationResult(await _repositoryAccessor.Save());
            }
            return new OperationResult(false);
        }

        public Task<OperationResult> DownloadExcelTemplate()
        {
            var path = Path.Combine(
                _webHostEnvironment.ContentRootPath, 
                "Resources\\Template\\EmployeeMaintenance\\4_1_2_EmployeeEmergencyContact\\Template.xlsx"
            );
            var workbook = new Workbook(path);
            var design = new WorkbookDesigner(workbook);
            MemoryStream stream = new();
            design.Workbook.Save(stream, SaveFormat.Xlsx);
            var result = stream.ToArray();
            return Task.FromResult(new OperationResult(true, null, result));
        }

        public async Task<DataMain> GetData(EmployeeEmergencyContactsParam param)
        {
            var rs = new DataMain();
            var pred_HRMS_Emp_Emergency_Contact = PredicateBuilder.New<HRMS_Emp_Emergency_Contact>(true);
            var pred_HRMS_Emp_Personal = PredicateBuilder.New<HRMS_Emp_Personal>(true);
            if(!string.IsNullOrWhiteSpace(param.USER_GUID)) {
                pred_HRMS_Emp_Personal.And(x => x.USER_GUID == param.USER_GUID.Trim());
                pred_HRMS_Emp_Emergency_Contact.And(x => x.USER_GUID == param.USER_GUID.Trim());
            }         
            var data = await _repositoryAccessor.HRMS_Emp_Emergency_Contact.FindAll(pred_HRMS_Emp_Emergency_Contact, true)
                        .GroupJoin(_repositoryAccessor.HRMS_Emp_Personal.FindAll(pred_HRMS_Emp_Personal, true),
                            x => x.USER_GUID,
                            y => y.USER_GUID,
                            (x, y) => new { HRMS_Emp_Emergency_Contact = x, HRMS_Emp_Personal = y } 
                        ).SelectMany(x => x.HRMS_Emp_Personal.DefaultIfEmpty(), 
                            (x, y) => new { x.HRMS_Emp_Emergency_Contact, HRMS_Emp_Personal = y }
                        ).GroupJoin(_repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "22", true),
                            x => x.HRMS_Emp_Emergency_Contact.Relationship, 
                            y => y.Code,
                            (x, y) => new { x.HRMS_Emp_Emergency_Contact, x.HRMS_Emp_Personal, HRMS_Basic_Code = y }
                        ).SelectMany(x => x.HRMS_Basic_Code.DefaultIfEmpty(),
                            (x, y) => new { x.HRMS_Emp_Emergency_Contact, x.HRMS_Emp_Personal, HRMS_Basic_Code = y }
                        ).GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == param.Language.ToLower(), true),
                            x => new { x.HRMS_Basic_Code.Type_Seq, x.HRMS_Basic_Code.Code },
                            y => new { y.Type_Seq, y.Code },
                            (x, y) => new { x.HRMS_Emp_Emergency_Contact, x.HRMS_Emp_Personal, x.HRMS_Basic_Code, HRMS_Basic_Code_Language = y }
                        ).SelectMany(x => x.HRMS_Basic_Code_Language.DefaultIfEmpty(),
                            (x, y) => new { x.HRMS_Emp_Emergency_Contact, x.HRMS_Emp_Personal, x.HRMS_Basic_Code, HRMS_Basic_Code_Language = y }
                        ).Select(x => new EmployeeEmergencyContactsDto(){ 
                            USER_GUID = x.HRMS_Emp_Emergency_Contact.USER_GUID,
                            Division = x.HRMS_Emp_Personal.Division,
                            Factory = x.HRMS_Emp_Personal.Factory,
                            Employee_ID = x.HRMS_Emp_Personal.Employee_ID,
                            Nationality = x.HRMS_Emp_Personal.Nationality,
                            Identification_Number = x.HRMS_Emp_Personal.Identification_Number, 
                            LocalFullName = x.HRMS_Emp_Personal.Local_Full_Name,
                            Seq = x.HRMS_Emp_Emergency_Contact.Seq,
                            Emergency_Contact = x.HRMS_Emp_Emergency_Contact.Emergency_Contact,
                            Relationship = x.HRMS_Emp_Emergency_Contact.Relationship + " - " + (x.HRMS_Basic_Code_Language != null ? x.HRMS_Basic_Code_Language.Code_Name : x.HRMS_Basic_Code.Code_Name),
                            Emergency_Contact_Phone = x.HRMS_Emp_Emergency_Contact.Emergency_Contact_Phone,
                            Temporary_Address = x.HRMS_Emp_Emergency_Contact.Temporary_Address,
                            Emergency_Contact_Address = x.HRMS_Emp_Emergency_Contact.Emergency_Contact_Address
                        }).ToListAsync();
            rs.Result = data;
            rs.TotalCount = data.Count;
            return rs;
        }

        public async Task<int> GetMaxSeq(string USER_GUID)
        {
            var dataExist = await _repositoryAccessor.HRMS_Emp_Emergency_Contact.FindAll(x => x.USER_GUID == USER_GUID).ToListAsync();
            if (!dataExist.Any())
                return 1;
            var listSeq = new List<int>(dataExist.Select(x => x.Seq));
            var max_seq = listSeq.Max();
            var result = Enumerable.Range(1, max_seq + 1)
                .Except(listSeq)
                .ToList();
            return result.FirstOrDefault();
        }

        public async Task<List<KeyValuePair<string, string>>> GetRelationships(string Language)
        {
            var data = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Relationship, true)
                        .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == Language.ToLower(), true),
                            HBC => new { HBC.Type_Seq, HBC.Code },
                            HBCL => new { HBCL.Type_Seq, HBCL.Code },
                            (HBC, HBCL) => new { HBC, HBCL })
                            .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                            (prev, HBCL) => new { prev.HBC, HBCL })
                        .Select(x => new KeyValuePair<string, string>(x.HBC.Code.Trim(), x.HBC.Code.Trim() + "-" + (x.HBCL != null ? x.HBCL.Code_Name.Trim() : x.HBC.Code_Name.Trim()))).Distinct().ToListAsync();
            return data;
        }

        public async Task<OperationResult> Update(EmployeeEmergencyContactsDto model)
        {
            var item = await _repositoryAccessor.HRMS_Emp_Emergency_Contact.FirstOrDefaultAsync(x => x.USER_GUID == model.USER_GUID.Trim()  
                                                                                && x.Seq == model.Seq);
            if (item != null)
            {
                item = Mapper.Map(model).Over(item);
                _repositoryAccessor.HRMS_Emp_Emergency_Contact.Update(item);                            
                return new OperationResult(await _repositoryAccessor.Save());
            }
            return new OperationResult(false);
        }

        #region Upload
        public async Task<OperationResult> UploadData(IFormFile file, List<string> role_List, string userName)
        {
            ExcelResult resp = ExcelUtility.CheckExcel(
                file, 
                "Resources\\Template\\EmployeeMaintenance\\4_1_2_EmployeeEmergencyContact\\Template.xlsx"
            );
            if (!resp.IsSuccess)
                return new OperationResult(false, resp.Error);

            List<HRMS_Emp_Emergency_Contact> emergencyAdd = new();
            List<EmployeeEmergencyContactsReport> excelReportList = new();

            List<string> basicCodeTypes = new()
            {
                BasicCodeTypeConstant.Division,
                BasicCodeTypeConstant.Factory,
                BasicCodeTypeConstant.Relationship,
            };

            var basicCodes = await GetAllBasicCodes(basicCodeTypes);

            var divisions = basicCodes[BasicCodeTypeConstant.Division];
            var factories = basicCodes[BasicCodeTypeConstant.Factory];
            var relations = basicCodes[BasicCodeTypeConstant.Relationship];

            var roles = await _repositoryAccessor.HRMS_Basic_Role
                .FindAll(x => role_List.Contains(x.Role))
                .Select(x => x.Factory).Distinct()
                .ToListAsync();

            var empPersonals = await _repositoryAccessor.HRMS_Emp_Personal
                .FindAll(x => roles.Contains(x.Factory))
                .Select(x => new {x.Division, x.Factory, x.Employee_ID, x.USER_GUID })
                .ToListAsync();

            try
            {
                for (int i = resp.WsTemp.Cells.Rows.Count; i < resp.Ws.Cells.Rows.Count; i++)
                {
                    List<string> errorMessage = new();
                    string division = resp.Ws.Cells[i, 0].StringValue?.Trim();
                    string factory = resp.Ws.Cells[i, 1].StringValue?.Trim();
                    string employeeID = resp.Ws.Cells[i, 2].StringValue?.Trim();
                    string emergencyContact = resp.Ws.Cells[i, 3].StringValue?.Trim();
                    string relationship = resp.Ws.Cells[i, 4].StringValue?.Trim();
                    string emergencyContactPhone = resp.Ws.Cells[i, 5].StringValue?.Trim();
                    string temporaryAddress = resp.Ws.Cells[i, 6].StringValue?.Trim();
                    string emergencyContactAddress = resp.Ws.Cells[i, 7].StringValue?.Trim();

                    // 0. Division
                    if (string.IsNullOrWhiteSpace(division) || !divisions.ContainsKey(division))
                        errorMessage.Add("Division is not valid");

                    // 1. Factory
                    var checkFactory = _repositoryAccessor.HRMS_Emp_Personal.Any(x => x.Division == division && x.Factory == factory);
                    if (string.IsNullOrWhiteSpace(factory) || checkFactory == false)
                        errorMessage.Add("Factory is invalid") ;

                    if (roles == null)
                        errorMessage.Add("uploaded [Factory] data does not match the role group") ;

                    // 2. Employee ID
                    if (string.IsNullOrWhiteSpace(employeeID) || employeeID.Length > 16)
                        errorMessage.Add("Employee ID is invalid");

                    // 3. Emergency Contact
                    if (string.IsNullOrWhiteSpace(emergencyContact) || emergencyContact.Length > 100)
                        errorMessage.Add("Emergency Contact is invalid");

                    // 4. Relationship
                    if (string.IsNullOrWhiteSpace(relationship) || !relations.ContainsKey(relationship))
                        errorMessage.Add("Relationship is invalid");

                    // 5. Emergency Contact Phone
                    if (string.IsNullOrWhiteSpace(emergencyContactPhone) || emergencyContactPhone.Length > 30)
                        errorMessage.Add("Emergency Contact Phone is invalid");

                    // 6. Temporary Address
                    if (temporaryAddress.Length > 225)
                        errorMessage.Add("Temporary Address is invalid");

                    // 7. Emergency Contact Address
                    if (string.IsNullOrWhiteSpace(emergencyContactAddress) || emergencyContactAddress.Length > 225)
                        errorMessage.Add("Emergency Contact Address is invalid");

                    string userGUID = null;
                    var empPersonal = empPersonals.FirstOrDefault(x => x.Division == division && x.Factory == factory && x.Employee_ID == employeeID);

                    if (empPersonal != null)
                        userGUID = empPersonal.USER_GUID;
                    else
                        errorMessage.Add("USER_GUID not found.");

                    if (!errorMessage.Any())
                    {
                        int maxSeq = 0;
                        if(emergencyAdd.Any(x => x.USER_GUID == userGUID))
                            maxSeq = emergencyAdd.Where(x => x.USER_GUID == userGUID).Max(x => x.Seq) + 1;
                        else maxSeq = await GetMaxSeq(userGUID);
                        
                        var newData = new HRMS_Emp_Emergency_Contact
                        {
                            USER_GUID = userGUID,
                            Seq = maxSeq,
                            Division = division,
                            Factory = factory,
                            Employee_ID = employeeID,
                            Emergency_Contact = emergencyContact,
                            Relationship = relationship,
                            Emergency_Contact_Phone = emergencyContactPhone,
                            Temporary_Address = temporaryAddress,
                            Emergency_Contact_Address = emergencyContactAddress,
                            Update_Time = DateTime.Now,
                            Update_By = userName,
                        };
                        emergencyAdd.Add(newData);
                    }

                    if (errorMessage.Any())
                    {
                        EmployeeEmergencyContactsReport report = new()
                        {
                            Division = division,
                            Factory = factory,
                            Employee_ID = employeeID,
                            Emergency_Contact = emergencyContact,
                            Relationship = relationship,
                            Emergency_Contact_Phone = emergencyContactPhone,
                            Temporary_Address = temporaryAddress,
                            Emergency_Contact_Address = emergencyContactAddress,
                            Error_Message = string.Join("\r\n",errorMessage)
                        };

                        excelReportList.Add(report);
                    }
                }

                EmployeeEmergencyContacts_UploadResult resultDto = new()
                {
                    Total = resp.Ws.Cells.Rows.Count - resp.WsTemp.Cells.Rows.Count,
                    Success = emergencyAdd.Count,
                    Error = excelReportList.Count
                };

                if (emergencyAdd.Any())
                {
                    _repositoryAccessor.HRMS_Emp_Emergency_Contact.AddMultiple(emergencyAdd);
                    await _repositoryAccessor.Save();
                    string folder = "uploaded\\excels\\EmployeeMaintenance\\4_1_2_EmployeeEmergencyContact\\Creates";
                    await FilesUtility.SaveFile(file, folder, $"EmployeeEmergencyContact_{DateTime.Now:yyyyMMddHHmmss}");
                }

                if (excelReportList.Any())
                {
                    List<Table> tables = new()
                    {
                        new Table("result", excelReportList)
                    };
                    List<Cell> cells = new()
                    {
                        new Cell("B1", userName),
                        new Cell("D1", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")),
                    };

                    ExcelResult excel = ExcelUtility.DownloadExcel(
                        tables, 
                        cells, 
                        "Resources\\Template\\EmployeeMaintenance\\4_1_2_EmployeeEmergencyContact\\Report.xlsx"
                    );
                    resultDto.ErrorReport = excel.Result;
                    return new OperationResult { IsSuccess = false, Data = resultDto, Error = "Please check Error Report" };
                }

                return new OperationResult { IsSuccess = true, Data = resultDto };
            }
            
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message);
            }
        }

        private async Task<Dictionary<string, Dictionary<string, HRMS_Basic_Code>>> GetAllBasicCodes(List<string> typeSeqs)
        {
            var codes = await _repositoryAccessor.HRMS_Basic_Code
                .FindAll(x => typeSeqs.Contains(x.Type_Seq))
                .ToListAsync();
            
            return codes.GroupBy(x => x.Type_Seq)
                .ToDictionary(x => x.Key, x => x.ToDictionary(c => c.Code));
        }
        #endregion
    }
}