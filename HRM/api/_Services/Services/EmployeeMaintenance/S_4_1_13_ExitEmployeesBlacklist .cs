using API.Data;
using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.EmployeeMaintenance
{
    public class S_4_1_13_ExitEmployeesBlacklist : BaseServices, I_4_1_13_ExitEmployeesBlacklist
    {
        public S_4_1_13_ExitEmployeesBlacklist(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<OperationResult> Edit(HRMS_Emp_BlacklistDto model, string userName)
        {
            var dataExist = await _repositoryAccessor.HRMS_Emp_Blacklist.FirstOrDefaultAsync(x =>
             x.USER_GUID == model.USER_GUID && x.Maintenance_Date == model.Maintenance_Date);
            if (dataExist == null)
                return new OperationResult(false, "No data");

            dataExist.Description = model.Description;
            dataExist.Update_By = userName;
            dataExist.Update_Time = DateTime.Now;

            _repositoryAccessor.HRMS_Emp_Blacklist.Update(dataExist);
            try
            {
                await _repositoryAccessor.Save();
                return new OperationResult(true);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message);
            }
        }

        public async Task<PaginationUtility<HRMS_Emp_BlacklistDto>> GetDataPagination(PaginationParam pagination, HRMS_Emp_BlacklistParam param)
        {
            var predBlacklist = PredicateBuilder.New<HRMS_Emp_Blacklist>(true);
            var predPersonal = PredicateBuilder.New<HRMS_Emp_Personal>(true);

            if (!string.IsNullOrWhiteSpace(param.Nationality))
            {
                predBlacklist.And(x => x.Nationality == param.Nationality);
                predPersonal.And(x => x.Nationality == param.Nationality);
            }

            if (!string.IsNullOrWhiteSpace(param.Identification_Number))
            {
                predBlacklist.And(x => x.Identification_Number == param.Identification_Number);
                predPersonal.And(x => x.Identification_Number == param.Identification_Number);
            }

            var result = await _repositoryAccessor.HRMS_Emp_Blacklist.FindAll(predBlacklist)
                .GroupJoin(_repositoryAccessor.HRMS_Emp_Personal.FindAll(predPersonal),
                x => new { x.USER_GUID },
                y => new { y.USER_GUID },
                (x, y) => new { bl = x, ps = y })
                .SelectMany(x => x.ps.DefaultIfEmpty(),
                (x, y) => new { x.bl, ps = y })
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "33", true),
                 x => x.bl.Resign_Reason,
                 code => code.Code,
                 (x, code) => new { x.bl, x.ps, Code = code })
                .SelectMany(x => x.Code.DefaultIfEmpty(), (x, code) => new { x.bl, x.ps, Code = code })
                .Select(x => new HRMS_Emp_BlacklistDto
                {
                    USER_GUID = x.bl.USER_GUID,
                    Maintenance_Date = x.bl.Maintenance_Date,
                    Nationality = x.bl.Nationality,
                    Identification_Number = x.bl.Identification_Number,
                    Local_Full_Name = x.ps.Local_Full_Name,
                    Resign_Reason = $"{x.bl.Resign_Reason} - {x.Code.Code_Name}",
                    Description = x.bl.Description,
                    Update_By = x.bl.Update_By,
                    Update_Time = x.bl.Update_Time
                }).ToListAsync();

            return PaginationUtility<HRMS_Emp_BlacklistDto>.Create(result, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<List<KeyValuePair<string, string>>> GetIdentificationNumber()
        {
            return await _repositoryAccessor.HRMS_Emp_Personal.FindAll(true).Select(x => new KeyValuePair<string, string>(x.Identification_Number, x.Identification_Number)).Distinct().ToListAsync();
        }

        public async Task<List<KeyValuePair<string, string>>> GetListNationality(string language)
        {
            var data = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "10")
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower()),
                                x => new { x.Type_Seq, x.Code },
                                y => new { y.Type_Seq, y.Code },
                                (x, y) => new { x, y })
                                .SelectMany(x => x.y.DefaultIfEmpty(),
                                (x, y) => new { BasicCode = x.x, BasicCodeLanguage = y })
            .Select(x => new KeyValuePair<string, string>(x.BasicCode.Code, $"{(x.BasicCodeLanguage != null ? x.BasicCodeLanguage.Code_Name : x.BasicCode.Code_Name)}")).ToListAsync();
            return data;
        }
    }
}