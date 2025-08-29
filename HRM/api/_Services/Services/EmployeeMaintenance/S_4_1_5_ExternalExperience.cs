using API.Data;
using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.EmployeeMaintenance
{
    public class S_4_1_5_ExternalExperience : BaseServices, I_4_1_5_ExternalExperience
    {
        public S_4_1_5_ExternalExperience(DBContext dbContext) : base(dbContext)
        {

        }

        public async Task<int> GetMaxSeq(HRMS_Emp_External_ExperienceDto data)
        {
            var dataExist = await _repositoryAccessor.HRMS_Emp_External_Experience
                .FindAll(x => x.USER_GUID == data.USER_GUID)
                .ToListAsync();

            if (!dataExist.Any())
                return 1;

            var seqList = new List<int>(dataExist.Select(x => x.Seq));
            var max_seq = seqList.Max();

            var result = Enumerable.Range(1, max_seq + 1)
                .Except(seqList)
                .ToList();

            return result.FirstOrDefault();
        }

        public async Task<OperationResult> AddNew(HRMS_Emp_External_ExperienceDto data, string userName)
        {
            if (await _repositoryAccessor.HRMS_Emp_External_Experience.AnyAsync(x =>
            x.USER_GUID == data.USER_GUID && x.Seq == data.Seq))
                return new OperationResult(false, "Already exist");

            var dataNew = new HRMS_Emp_External_Experience
            {
                USER_GUID = data.USER_GUID,
                Seq = data.Seq,
                Company_Name = data.Company_Name,
                Department = data.Department,
                Leadership_Role = data.Leadership_Role,
                Position_Title = data.Position_Title,
                Tenure_Start = data.Tenure_Start,
                Tenure_End = data.Tenure_End,
                Update_Time = DateTime.Now,
                Update_By = userName
            };

            _repositoryAccessor.HRMS_Emp_External_Experience.Add(dataNew);
            await _repositoryAccessor.Save();

            return new OperationResult(true);
        }

        public async Task<OperationResult> Edit(HRMS_Emp_External_ExperienceDto data, string userName)
        {
            var dataExist = await _repositoryAccessor.HRMS_Emp_External_Experience.FirstOrDefaultAsync(x =>
            x.USER_GUID == data.USER_GUID && x.Seq == data.Seq);
            if (dataExist == null)
                return new OperationResult(false, "No data");

            dataExist.Company_Name = data.Company_Name;
            dataExist.Department = data.Department;
            dataExist.Leadership_Role = data.Leadership_Role;
            dataExist.Position_Title = data.Position_Title;
            dataExist.Tenure_Start = data.Tenure_Start;
            dataExist.Tenure_End = data.Tenure_End;
            dataExist.Update_By = userName;
            dataExist.Update_Time = DateTime.Now;
            _repositoryAccessor.HRMS_Emp_External_Experience.Update(dataExist);
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

        public async Task<OperationResult> Delete(HRMS_Emp_External_ExperienceDto data)
        {
            var item = await _repositoryAccessor.HRMS_Emp_External_Experience.FirstOrDefaultAsync(x =>
                    x.USER_GUID == data.USER_GUID && x.Seq == data.Seq);
            if (item == null)
                return new OperationResult(false, "Data not exist");
            _repositoryAccessor.HRMS_Emp_External_Experience.Remove(item);
            if (await _repositoryAccessor.Save())
                return new OperationResult(true, "Delete Successfully");
            return new OperationResult(false, "Delete failed");
        }

        public async Task<List<HRMS_Emp_External_ExperienceDto>> GetData(HRMS_Emp_External_ExperienceParam filter)
        {
            var predExternalExperience = PredicateBuilder.New<HRMS_Emp_External_Experience>(true);
            var predPersonal = PredicateBuilder.New<HRMS_Emp_Personal>(true);

            if (!string.IsNullOrWhiteSpace(filter.USER_GUID))
            {
                predExternalExperience.And(x => x.USER_GUID == filter.USER_GUID);
                predPersonal.And(x => x.USER_GUID == filter.USER_GUID);
            }

            var result = await _repositoryAccessor.HRMS_Emp_External_Experience.FindAll(predExternalExperience)
                .GroupJoin(_repositoryAccessor.HRMS_Emp_Personal.FindAll(predPersonal),
                x => new { x.USER_GUID },
                y => new { y.USER_GUID },
                (x, y) => new { exp = x, ep = y })
                .SelectMany(x => x.ep.DefaultIfEmpty(),
                (x, y) => new { x.exp, ep = y })
                .Select(x => new HRMS_Emp_External_ExperienceDto
                {
                    USER_GUID = x.ep.USER_GUID,
                    Nationality = x.ep.Nationality,
                    Identification_Number = x.ep.Identification_Number,
                    Local_Full_Name = x.ep.Local_Full_Name,
                    Seq = x.exp.Seq,
                    Company_Name = x.exp.Company_Name,
                    Department = x.exp.Department,
                    Leadership_Role = x.exp.Leadership_Role,
                    Position_Title = x.exp.Position_Title,
                    Tenure_Start = x.exp.Tenure_Start,
                    Tenure_End = x.exp.Tenure_End,
                    Update_By = x.exp.Update_By,
                    Update_Time = x.exp.Update_Time
                }).ToListAsync();
            return result;
        }

    }
}