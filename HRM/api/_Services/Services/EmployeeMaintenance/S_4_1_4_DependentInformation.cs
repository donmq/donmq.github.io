using API.Data;
using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using API.Helper.Constant;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.EmployeeMaintenance
{
    public class S_4_1_4_DependentInformation : BaseServices, I_4_1_4_DependentInformation
    {
        public S_4_1_4_DependentInformation(DBContext dbContext) : base(dbContext)
        {
        }

        public async Task<OperationResult> AddNew(HRMS_Emp_DependentDto model, string userName)
        {
            if (await _repositoryAccessor.HRMS_Emp_Dependent.AnyAsync(x => x.USER_GUID == model.USER_GUID && x.Seq == model.Seq))
                return new OperationResult(false, $" USER GUID: {model.USER_GUID}, Seq: {model.Seq}  is existed");

            HRMS_Emp_Dependent dataDependent = new()
            {
                USER_GUID = model.USER_GUID,
                Seq = model.Seq,
                Name = model.Name,
                Relationship = model.Relationship,
                Occupation = model.Occupation,
                Dependents = model.Dependents,
                Update_By = userName,
                Update_Time = DateTime.Now
            };
            _repositoryAccessor.HRMS_Emp_Dependent.Add(dataDependent);

            if (await _repositoryAccessor.Save())
            {
                return new OperationResult(true);
            }

            return new OperationResult(false);
        }

        public async Task<OperationResult> Delete(HRMS_Emp_DependentDto model)
        {
            var isExist = await _repositoryAccessor.HRMS_Emp_Dependent.FirstOrDefaultAsync(x => x.USER_GUID == model.USER_GUID && x.Seq == model.Seq);
            if (isExist == null)
                return new OperationResult(false, "This data none existed");
            _repositoryAccessor.HRMS_Emp_Dependent.Remove(isExist);
            await _repositoryAccessor.Save();
            return new OperationResult(true);
        }

        public async Task<OperationResult> Edit(HRMS_Emp_DependentDto model, string userName)
        {
            var item = await _repositoryAccessor.HRMS_Emp_Dependent.FirstOrDefaultAsync(x => x.USER_GUID == model.USER_GUID && x.Seq == model.Seq);
            if (item != null)
            {
                item.Name = model.Name;
                item.Relationship = model.Relationship;
                item.Occupation = model.Occupation;
                item.Dependents = model.Dependents;
                item.Update_By = userName;
                item.Update_Time = DateTime.Now;

                _repositoryAccessor.HRMS_Emp_Dependent.Update(item);
                if (await _repositoryAccessor.Save())
                    return new OperationResult(true, "Update Successfully");
                return new OperationResult(false, "Update failed");
            }
            return new OperationResult(false);
        }

        public async Task<List<HRMS_Emp_DependentDto>> GetData(HRMS_Emp_DependentParam param)
        {
            var predDependent = PredicateBuilder.New<HRMS_Emp_Dependent>(true);
            var predPersonal = PredicateBuilder.New<HRMS_Emp_Personal>(true);
            if (!string.IsNullOrWhiteSpace(param.USER_GUID))
            {
                predDependent.And(x => x.USER_GUID == param.USER_GUID);
                predPersonal.And(x => x.USER_GUID == param.USER_GUID);
            }
            var HBC = _repositoryAccessor.HRMS_Basic_Code.FindAll();
            var HBCL = _repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == param.Lang.ToLower());
            var codeLang = HBC.GroupJoin(HBCL,
                x => new { x.Type_Seq, x.Code },
                y => new { y.Type_Seq, y.Code },
                    (x, y) => new { hbc = x, hbcl = y })
                .SelectMany(x => x.hbcl.DefaultIfEmpty(),
                    (x, y) => new { x.hbc, hbcl = y })
                .Select(x => new
                {
                    x.hbc.Code,
                    x.hbc.Type_Seq,
                    Code_Name = x.hbcl != null ? x.hbcl.Code_Name : x.hbc.Code_Name
                }).Distinct();
            var query = await _repositoryAccessor.HRMS_Emp_Dependent.FindAll(predDependent, true)
                               .GroupJoin(_repositoryAccessor.HRMS_Emp_Personal.FindAll(predPersonal, true),
                                   x => new { x.USER_GUID },
                                   y => new { y.USER_GUID },
                                   (x, y) => new { dependent = x, personal = y })
                               .SelectMany(x => x.personal.DefaultIfEmpty(),
                                   (x, y) => new { x.dependent, personal = y })
                               .Select(x => new HRMS_Emp_DependentDto()
                               {
                                   Nationality = x.personal != null ? x.personal.Nationality : string.Empty,
                                   Identification_Number = x.personal != null ? x.personal.Identification_Number : string.Empty,
                                   Local_Full_Name = x.personal != null ? x.personal.Local_Full_Name : string.Empty,
                                   Seq = x.dependent.Seq,
                                   Name = x.dependent.Name,
                                   Relationship = x.dependent.Relationship,
                                   Relationship_Name = codeLang.FirstOrDefault(y => y.Code == x.dependent.Relationship && y.Type_Seq == BasicCodeTypeConstant.Relationship).Code_Name,
                                   Occupation = x.dependent.Occupation,
                                   Dependents = x.dependent.Dependents,
                                   USER_GUID = x.dependent.USER_GUID
                               })
                               .ToListAsync();
            return query;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListRelationship(string language)
        {
            return await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == BasicCodeTypeConstant.Relationship, true)
            .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == language.ToLower(), true),
                    HBC => new { HBC.Type_Seq, HBC.Code },
                    HBCL => new { HBCL.Type_Seq, HBCL.Code },
                    (HBC, HBCL) => new { HBC, HBCL })
                    .SelectMany(x => x.HBCL.DefaultIfEmpty(),
                    (prev, HBCL) => new { prev.HBC, HBCL })
            .Select(x => new KeyValuePair<string, string>(x.HBC.Code, x.HBCL != null ? x.HBCL.Code_Name : x.HBC.Code_Name)).Distinct().ToListAsync();
        }

        public async Task<int> GetMaxSeq(HRMS_Emp_DependentDto model)
        {
            var dataExist = await _repositoryAccessor.HRMS_Emp_Dependent.FindAll(x => x.USER_GUID == model.USER_GUID).ToListAsync();

            if (!dataExist.Any())
                return 1;

            var listSeq = new List<int>(dataExist.Select(x => x.Seq));
            var max_seq = listSeq.Max();

            var result = Enumerable.Range(1, max_seq + 1)
                .Except(listSeq)
                .ToList();

            return result.FirstOrDefault();
        }
    }
}