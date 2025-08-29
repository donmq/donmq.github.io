using API.Data;
using API._Services.Interfaces.EmployeeMaintenance;
using API.DTOs.EmployeeMaintenance;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.EmployeeMaintenance
{
    public class S_4_1_6_IdentificationCardHistory : BaseServices, I_4_1_6_IdentificationCardHistory
    {
        public S_4_1_6_IdentificationCardHistory(DBContext dbContext) : base(dbContext)
        {
        }
        public async Task<OperationResult> Create(HRMS_Emp_Identity_Card_HistoryDto dto)
        {       
            if (await _repositoryAccessor.HRMS_Emp_Identity_Card_History.AnyAsync(x =>
                    x.Identification_Number_After == dto.Identification_Number_After &&
                    x.Nationality_After == dto.Nationality_After &&
                    x.Issued_Date_After == dto.Issued_Date_After))
                return new OperationResult(false, "System.Message.DataExisted");

            var history_GUID = Guid.NewGuid().ToString();
            while (await _repositoryAccessor.HRMS_Emp_Identity_Card_History.AnyAsync(x => x.History_GUID == history_GUID))
            {
                history_GUID = Guid.NewGuid().ToString();
            }

            var dataNew = new HRMS_Emp_Identity_Card_History
            {
                History_GUID = history_GUID,
                USER_GUID = dto.USER_GUID,
                Nationality_Before = dto.Nationality_Before,
                Identification_Number_Before = dto.Identification_Number_Before,
                Issued_Date_Before = dto.Issued_Date_Before,
                Nationality_After = dto.Nationality_After,
                Identification_Number_After = dto.Identification_Number_After,
                Issued_Date_After = dto.Issued_Date_After,
                Update_By = dto.Update_By,
                Update_Time = dto.Update_Time,
            };

            HRMS_Emp_Personal dataPersonal = await _repositoryAccessor.HRMS_Emp_Personal.FirstOrDefaultAsync(x => x.USER_GUID == dto.USER_GUID);
            if (dataPersonal != null)
            {
                dataPersonal.Nationality = dto.Nationality_After;
                dataPersonal.Identification_Number = dto.Identification_Number_After;
                dataPersonal.Issued_Date = dto.Issued_Date_After;
                dataPersonal.Update_Time = dto.Update_Time;
                dataPersonal.Update_By = dto.Update_By;

                _repositoryAccessor.HRMS_Emp_Personal.Update(dataPersonal);
            }
            
            _repositoryAccessor.HRMS_Emp_Identity_Card_History.Add(dataNew);

            try
            {
                await _repositoryAccessor.Save();
                return new OperationResult(true, "System.Message.UpdateOKMsg");
            }
            catch (Exception)
            {
                return new OperationResult(false, "System.Message.UpdateErrorMsg");
            }
        }

        public async Task<List<HRMS_Emp_Identity_Card_HistoryDto>> GetData(HRMS_Emp_Identity_Card_HistoryParam param)
        {
            var predicate = PredicateBuilder.New<HRMS_Emp_Identity_Card_History>(true);

            if (!string.IsNullOrWhiteSpace(param.Nationality))
                predicate.And(x => x.Nationality_After == param.Nationality);
            if (!string.IsNullOrWhiteSpace(param.Identification_Number))
                predicate.And(x => x.Identification_Number_After == param.Identification_Number);

            var userGuidPer = await _repositoryAccessor.HRMS_Emp_Identity_Card_History
                .FirstOrDefaultAsync(predicate);

            if (userGuidPer == null)
                return new List<HRMS_Emp_Identity_Card_HistoryDto>();

            var resultData = await _repositoryAccessor.HRMS_Emp_Identity_Card_History.FindAll(x => x.USER_GUID == userGuidPer.USER_GUID).ToListAsync();
         
            var dataList = resultData.GroupJoin(_repositoryAccessor.HRMS_Emp_Personal.FindAll(),
                                     x => new { x.USER_GUID },
                                     y => new { y.USER_GUID },
                                     (x, y) => new { IdentityCardHistory = x, Personal = y }
                                )
                                .SelectMany(
                                    x => x.Personal.DefaultIfEmpty(),
                                    (x, y) => new { x.IdentityCardHistory, Personal = y }
                                )
                                .Select(x => new HRMS_Emp_Identity_Card_HistoryDto
                                {
                                    Nationality_Before = x.IdentityCardHistory.Nationality_Before,
                                    Identification_Number_Before = x.IdentityCardHistory.Identification_Number_Before,
                                    Issued_Date_Before = x.IdentityCardHistory.Issued_Date_Before,
                                    Nationality_After = x.IdentityCardHistory.Nationality_After,
                                    Identification_Number_After = x.IdentityCardHistory.Identification_Number_After,
                                    Issued_Date_After = x.IdentityCardHistory.Issued_Date_After,
                                    Update_By = x.IdentityCardHistory.Update_By,
                                    Update_Time = x.IdentityCardHistory.Update_Time,
                                    Local_Full_Name = x.Personal?.Local_Full_Name,
                                    USER_GUID = x.Personal.USER_GUID,
                                    History_GUID = x.IdentityCardHistory?.History_GUID
                                }).OrderBy(x => x.Issued_Date_After).ThenBy(x => x.Update_Time).Distinct().ToList();

            if (dataList.Count > 0)
            {
                dataList[0].Issued_Date_Before = DateTime.Parse("0001/01/01");
                dataList[0].Identification_Number_Before = "";
                dataList[0].Nationality_Before = "";
            }

            return dataList;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListNationality(string Language)
        {
            var data = await _repositoryAccessor.HRMS_Basic_Code.FindAll(x => x.Type_Seq == "10")
                .GroupJoin(_repositoryAccessor.HRMS_Basic_Code_Language.FindAll(x => x.Language_Code.ToLower() == Language.ToLower()),
                                x => new { x.Type_Seq, x.Code },
                                y => new { y.Type_Seq, y.Code },
                                (x, y) => new { x, y })
                                .SelectMany(x => x.y.DefaultIfEmpty(),
                                (x, y) => new { BasicCode = x.x, BasicCodeLanguage = y })
            .Select(x => new KeyValuePair<string, string>(x.BasicCode.Code, $"{x.BasicCode.Code} - {(x.BasicCodeLanguage != null ? x.BasicCodeLanguage.Code_Name : x.BasicCode.Code_Name)}")).ToListAsync();
            return data;
        }

        public async Task<List<string>> GetListTypeHeadIdentificationNumber(string nationality)
        {
            var pred = PredicateBuilder.New<HRMS_Emp_Personal>(true);
            if (!string.IsNullOrWhiteSpace(nationality))
                pred = pred.And(x => x.Nationality == nationality);
            return await _repositoryAccessor.HRMS_Emp_Personal.FindAll(pred, true).Select(x => x.Identification_Number).Distinct().ToListAsync();
        }
    }
}