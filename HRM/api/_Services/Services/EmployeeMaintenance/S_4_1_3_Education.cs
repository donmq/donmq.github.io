using AgileObjects.AgileMapper;
using API.Data;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using API.Helper.Constant;
using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;

namespace API._Services.Services.EmployeeMaintenance
{
    public class S_4_1_3_Education : BaseServices, I_4_1_3_Education
    {
        private readonly string folder = "uploaded\\EmployeeMaintenance\\4_1_3_Educations";
        public S_4_1_3_Education(DBContext dbContext) : base(dbContext) { }

        public async Task<OperationResult> Create(HRMS_Emp_EducationalDto model)
        {
            var education = await FindEducation(model, true);
            if (education != null) return new OperationResult(false, "This education is existed");


            var data = Mapper.Map(model).ToANew<HRMS_Emp_Educational>(x => x.MapEntityKeys());
            _repositoryAccessor.HRMS_Emp_Educational.Add(data);

            await _repositoryAccessor.Save();
            return new OperationResult(true);
        }


        /// <summary>
        /// Danh sách bằng cấp
        /// </summary>
        /// <returns></returns>
        public async Task<List<KeyValuePair<string, string>>> GetDegrees(string language) => await GetDataFromBasicCode(BasicCodeTypeConstant.Degrees, language);

        public async Task<List<KeyValuePair<string, string>>> GetAcademicSystems(string language) => await GetDataFromBasicCode(BasicCodeTypeConstant.AcademicSystems, language);

        /// <summary>
        /// Chuyên môn
        /// </summary>
        /// <returns></returns>
        public async Task<List<KeyValuePair<string, string>>> GetMajors(string language) => await GetDataFromBasicCode(BasicCodeTypeConstant.Majors, language);



        public async Task<List<HRMS_Emp_EducationalDto>> GetDataPagination(HRMS_Emp_EducationalParam filter)
        {
            var predicateEducation = PredicateBuilder.New<HRMS_Emp_Educational>(true);
            var predicatePersonal = PredicateBuilder.New<HRMS_Emp_Personal>(true);


            if (!string.IsNullOrWhiteSpace(filter.USER_GUID))
                predicateEducation.And(x => x.USER_GUID == filter.USER_GUID);

            var degreeQuery = GetQueryBasicCode(BasicCodeTypeConstant.Degrees, filter.Language);
            var academicSystemQuery = GetQueryBasicCode(BasicCodeTypeConstant.AcademicSystems, filter.Language);
            var majorQuery = GetQueryBasicCode(BasicCodeTypeConstant.Majors, filter.Language);

            var query = await _repositoryAccessor.HRMS_Emp_Educational.FindAll(predicateEducation, true)
                                // Personal 
                                .GroupJoin(_repositoryAccessor.HRMS_Emp_Personal.FindAll(predicatePersonal, true),
                                        x => new { x.USER_GUID },
                                        y => new { y.USER_GUID },
                                    (x, y) => new { education = x, personal = y })
                                    .SelectMany(x => x.personal.DefaultIfEmpty(),
                                    (x, y) => new { x.education, personal = y })
                                // Degree
                                .GroupJoin(degreeQuery,
                                        x => new { x.education.Degree },
                                        y => new { Degree = y.Code },
                                    (x, y) => new { x.education, x.personal, degree = y })
                                    .SelectMany(x => x.degree.DefaultIfEmpty(),
                                    (x, y) => new { x.education, x.personal, degree = y })
                                // Academic System
                                .GroupJoin(academicSystemQuery,
                                        x => new { x.education.Academic_System },
                                        y => new { Academic_System = y.Code },
                                    (x, y) => new { x.education, x.personal, x.degree, academicSystem = y })
                                    .SelectMany(x => x.academicSystem.DefaultIfEmpty(),
                                    (x, y) => new { x.education, x.personal, x.degree, academicSystem = y })
                                // Major
                                .GroupJoin(majorQuery,
                                        x => new { x.education.Major },
                                        y => new { Major = y.Code },
                                    (x, y) => new { x.education, x.personal, x.degree, x.academicSystem, major = y })
                                    .SelectMany(x => x.major.DefaultIfEmpty(),
                                    (x, y) => new { x.education, x.personal, x.degree, x.academicSystem, major = y })

                                .OrderByDescending(x => x.education.Update_Time)
                                .Select(x => new HRMS_Emp_EducationalDto()
                                {
                                    USER_GUID = x.education.USER_GUID,
                                    Nationality = x.personal != null ? x.personal.Nationality : string.Empty,
                                    Identification_Number = x.personal != null ? x.personal.Identification_Number : string.Empty,
                                    Local_Full_Name = x.personal != null ? x.personal.Local_Full_Name : string.Empty,

                                    DegreeName = !string.IsNullOrWhiteSpace(x.degree.Code_Lang) ? x.degree.Code_Lang : x.degree.Code_Name,
                                    Academic_SystemName = !string.IsNullOrWhiteSpace(x.academicSystem.Code_Lang) ? x.academicSystem.Code_Lang : x.academicSystem.Code_Name,
                                    MajorName = !string.IsNullOrWhiteSpace(x.major.Code_Lang) ? x.major.Code_Lang : x.major.Code_Name,

                                    Degree = x.education.Degree,
                                    Academic_System = x.education.Academic_System,
                                    Major = x.education.Major,

                                    School = x.education.School,
                                    Department = x.education.Department,
                                    Period_Start = x.education.Period_Start,
                                    Period_End = x.education.Period_End,
                                    Graduation = x.education.Graduation,
                                    Update_By = x.education.Update_By,
                                    Update_Time = x.education.Update_Time,
                                })
                                .OrderByDescending(x => x.Period_Start)
                                .ToListAsync();
            return query;
        }

        public async Task<List<HRMS_Emp_Educational_FileUpload>> GetEducationFiles(string user_GUID)
        {
            return await _repositoryAccessor.HRMS_Emp_Educational_File.FindAll(x => x.USER_GUID == user_GUID.Trim(), true)
                                    .Project().To<HRMS_Emp_Educational_FileUpload>()
                                    .ToListAsync();
        }

        private IQueryable<HRMS_Emp_Educational_QueryData> GetQueryBasicCode(string code, string language)
        {
            return _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == code, true)
                            .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                                x => new { x.Type_Seq, x.Code },
                                y => new { y.Type_Seq, y.Code },
                                (basicCode, basicLanguage) => new { basicCode, basicLanguage })
                                .SelectMany(x => x.basicLanguage.DefaultIfEmpty(),
                                (prev, basicLanguage) => new { prev.basicCode, basicLanguage })
                                .Select(x => new HRMS_Emp_Educational_QueryData()
                                {
                                    Type_Seq = x.basicCode.Type_Seq,
                                    Code = x.basicCode.Code,
                                    Code_Name = x.basicCode.Code_Name,
                                    Code_Lang = x.basicLanguage.Code_Name
                                });
        }

        private async Task<List<KeyValuePair<string, string>>> GetDataFromBasicCode(string code, string language)
        {

            return await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == code, true)
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                    HBC => new { HBC.Type_Seq, HBC.Code },
                    HBCL => new { HBCL.Type_Seq, HBCL.Code },
                    (HBC, HBCL) => new { HBC, HBCL })
                    .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                    (prev, HBCL) => new { prev.HBC, HBCL })
                .Select(x => new KeyValuePair<string, string>(x.HBC.Code, $"{x.HBC.Code} - {(x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)}"))
                .Distinct()
                .ToListAsync();
        }

        public async Task<OperationResult> Update(HRMS_Emp_EducationalDto model)
        {
            var education = await FindEducation(model);

            if (education == null) return new OperationResult(false, "item not exist");

            education = Mapper.Map(model).Over(education);
            try
            {
                _repositoryAccessor.HRMS_Emp_Educational.Update(education);
                await _repositoryAccessor.Save();
                return new OperationResult(true);
            }
            catch
            {
                return new OperationResult(false);
            }
        }

        public async Task<OperationResult> UploadFiles(EducationUpload model)
        {
            var addModels = new List<HRMS_Emp_Educational_File>();

            // Kiểm tra trong ngày hiện tại đã Uploads hay chưa ?
            var code = DateTime.Now.ToString("yyyyMMdd");

            var educations = await _repositoryAccessor.HRMS_Emp_Educational_File
                .FindAll(x => x.USER_GUID == model.USER_GUID && x.SerNum.StartsWith(code))
                .ToListAsync();

            string preSerNum = educations.Any() ? educations.Max(x => x.SerNum) : null;
            int preFileId = educations.Any() ? educations.Max(x => x.FileID) : 0;

            string currentSerNum = string.IsNullOrWhiteSpace(preSerNum)
                ? code + "0000001"
                : preSerNum[0..8] + (int.Parse(preSerNum[9..]) + 1).ToString("0000000");
            int nextFileId = preFileId == 0 ? 1 : preFileId + 1;
            var currentTime = DateTime.Now;

            foreach (var item in model.Files)
            {
                // Kiểm tra file đã tồn tại hay chưa ?
                if (await _repositoryAccessor.HRMS_Emp_Educational_File.AnyAsync(x =>
                    x.USER_GUID == model.USER_GUID &&
                    x.FileName.ToLower() == item.FileName.ToLower()))
                    return new OperationResult(false, "FileExisted");

                // Upload file
                string path = $"{folder}\\{model.USER_GUID}";
                var filename = await FilesUtility.UploadAsync(item.File, path);

                addModels.Add(new HRMS_Emp_Educational_File()
                {
                    USER_GUID = model.USER_GUID,
                    SerNum = currentSerNum,
                    FileID = nextFileId,
                    FileName = filename,
                    FileSize = item.FileSize,
                    Update_Time = currentTime,
                    Update_By = model.UpdateBy
                });

                // Set Next Curent
                currentSerNum = currentSerNum[0..8] + (int.Parse(currentSerNum[9..]) + 1).ToString("0000000");
                nextFileId++;
            }
            try
            {
                if (addModels.Any())
                    _repositoryAccessor.HRMS_Emp_Educational_File.AddMultiple(addModels);

                await _repositoryAccessor.Save();
                return new OperationResult(true);
            }
            catch
            {
                return new OperationResult(false, "UploadErrorMsg");
            }
        }

        public async Task<OperationResult> DownloadFile(EducationFile model)
        {
            var file = await _repositoryAccessor.HRMS_Emp_Educational_File.FirstOrDefaultAsync(x => x.USER_GUID == model.USER_GUID.Trim() && x.SerNum == model.SerNum.Trim() && x.FileID == model.FileID, true);
            if (file == null)
                return new OperationResult(false, "Education File Not Exist");

            string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string path = $"{webRootPath}\\{folder}\\{model.USER_GUID}\\{file.FileName}";
            if (!File.Exists(path))
                return new OperationResult(false, "This File Was Not Existed");

            byte[] bytes = File.ReadAllBytes(path);

            var fileNameArray = file.FileName.Split(".");
            HRMS_Emp_EducationalDownload fileData = new()
            {
                FileName = file.FileName,
                File = $"data:{fileNameArray[^1]};base64,{Convert.ToBase64String(bytes)}"
            };
            return new OperationResult(true, fileData);
        }

        public async Task<OperationResult> Delete(HRMS_Emp_EducationalDto model)
        {
            var education = await FindEducation(model);
            if (education == null) return new OperationResult(false, "item not exist");

            try
            {
                // Kiểm tra thông tin danh sách theo Quốc tịch và CMND
                if (!await _repositoryAccessor.HRMS_Emp_Educational.AnyAsync(x =>
                                                                    x.USER_GUID == model.USER_GUID
                                                                    && x.Degree != model.Degree
                                                                    && x.Period_Start != model.Period_Start))
                {
                    var fileOfEducations = await _repositoryAccessor.HRMS_Emp_Educational_File.FindAll(x => x.USER_GUID == model.USER_GUID).ToListAsync();
                    if (fileOfEducations.Any())
                    {
                        _repositoryAccessor.HRMS_Emp_Educational_File.RemoveMultiple(fileOfEducations);
                        // Xoá File

                        foreach (var file in fileOfEducations)
                        {
                            string path = $"{folder}\\{model.USER_GUID}\\{file.FileName}";
                            FilesUtility.DeleteFile(path);
                        }
                    }
                }
                _repositoryAccessor.HRMS_Emp_Educational.Remove(education);
                await _repositoryAccessor.Save();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                return new OperationResult(false);
            }
        }

        public async Task<OperationResult> DeleteEducationFile(EducationFile model)
        {
            var file = await _repositoryAccessor.HRMS_Emp_Educational_File.FirstOrDefaultAsync(x => x.USER_GUID == model.USER_GUID.Trim() && x.SerNum == model.SerNum.Trim() && x.FileID == model.FileID);
            if (file == null) return new OperationResult(false, "Education File Not Exist");

            try
            {
                // Xoá file
                string path = $"{folder}\\{model.USER_GUID}\\{file.FileName}";
                FilesUtility.DeleteFile(path);
                _repositoryAccessor.HRMS_Emp_Educational_File.Remove(file);
                await _repositoryAccessor.Save();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                return new OperationResult(false);
            }
        }

        private async Task<HRMS_Emp_Educational> FindEducation(HRMS_Emp_EducationalDto model, bool isTracking = false)
        {
            return await _repositoryAccessor.HRMS_Emp_Educational
                                    .FirstOrDefaultAsync(x => x.USER_GUID == model.USER_GUID
                                                        && x.Degree == model.Degree
                                                        && x.Period_Start == model.Period_Start
                                                        , isTracking);
        }
    }
}