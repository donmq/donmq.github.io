using LinqKit;
using Machine_API._Accessor;
using Machine_API.Models.MachineCheckList;
using Machine_API.Models.SAP;
using Microsoft.EntityFrameworkCore;
using NLog;
using Quartz;
namespace Machine_API.Helpers.Jobs
{
    public class CreateDataFromSAPJobDTO
    {
        public Control_File Control_File { get; set; }
        public List<MT_Asset> MT_Asset { get; set; }
    }
    // Job work at 20:00:00pm every day
    public class CreateDataFromSAPJob : IJob
    {
        private readonly IMachineRepositoryAccessor _machineRepository;
        private readonly ISAPRepositoryAccessor _sapRepository;
        private readonly string _OwnerFty;
        public CreateDataFromSAPJob(
            IMachineRepositoryAccessor machineRepository,
            ISAPRepositoryAccessor sapRepository,
            IConfiguration configuration)
        {
            _machineRepository = machineRepository;
            _sapRepository = sapRepository;
            _OwnerFty = configuration.GetSection("AppSettings:OwnerFty").Value;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using var _machineTransaction = await _machineRepository.BeginTransactionAsync(); // Use transaction on MT db only
            try
            {
                WorkLog($"Job start in {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                var Control_File = _sapRepository.Control_File.FindAll(x =>
                    x.SPEC_ID.Trim() == "SAP_FI013" &&
                    x.Table_Name.Trim() == "MT_Asset" &&
                    x.System_ID.Trim() == "MT" &&
                    x.Control_Flag.Trim() == "N"
                );
                var data = await Control_File
                    .GroupJoin(_sapRepository.MT_Asset.FindAll(),
                        x => x.SID,
                        y => y.SID,
                        (x, y) => new { Control_File = x, MT_Asset = y })
                    .SelectMany(x => x.MT_Asset.DefaultIfEmpty(),
                        (x, y) => new { x.Control_File, MT_Asset = y })
                    .GroupBy(x => x.Control_File)
                    .Select(x => new CreateDataFromSAPJobDTO { Control_File = x.Key, MT_Asset = x.Select(y => y.MT_Asset).ToList() })
                    .OrderBy(x => x.Control_File.SID)
                    .ToListAsync();
                if (data.Count > 0)
                {
                    List<hp_a04> hp04_Adds = new();
                    List<hp_a04> hp04_Updates = new();
                    List<Control_File> control_Updates = new();
                    foreach (var item in data)
                    {
                        item.Control_File.Control_Flag = "Y";
                        control_Updates.Add(item.Control_File);
                        if (item.MT_Asset.Count > 0)
                        {
                            List<string> mainAssetNumbers = item.MT_Asset.Select(x => x.Main_Asset_Number).Distinct().ToList();
                            List<string> listPlnos = item.MT_Asset.Select(x => x.Plno).Distinct().ToList();
                            List<string> assnos = item.MT_Asset
                                .Select(x =>
                                    string.IsNullOrWhiteSpace(x.HP_Assno_ID)
                                        ? x.Main_Asset_Number
                                        : x.HP_Assno_ID)
                                .Distinct()
                                .ToList();
                            var hp04_Olds = await _machineRepository.hp_a04
                                .FindAll(x =>
                                    mainAssetNumbers.Contains(x.Main_Asset_Number) &&
                                    assnos.Contains(x.AssnoID) &&
                                    x.OwnerFty == _OwnerFty)
                                .ToListAsync();
                            var hp15_Olds = await _machineRepository.hp_a15
                                .FindAll(x => listPlnos.Contains(x.Plno))
                                .ToListAsync();
                            foreach (var asset in item.MT_Asset)
                            {
                                string state = hp15_Olds.FirstOrDefault(x => x.Plno == asset.Plno)?.State ?? "A";
                                var hp04_Update = hp04_Olds.FirstOrDefault(x =>
                                    x.Main_Asset_Number == asset.Main_Asset_Number &&
                                    (string.IsNullOrWhiteSpace(asset.HP_Assno_ID)
                                        ? x.AssnoID == asset.Main_Asset_Number
                                        : x.AssnoID == asset.HP_Assno_ID) &&
                                    x.OwnerFty == _OwnerFty
                                );
                                if (hp04_Update?.Visible == false)
                                    continue;
                                if (hp04_Update != null)
                                {
                                    hp04_Update.Askid = asset.Askid;
                                    hp04_Update.Company_Code = asset.Owner_Fty;
                                    hp04_Update.Dept_ID = asset.Dept_ID;
                                    hp04_Update.MachineName_EN = asset.Machine_Name_CN;
                                    // hp04_Update.Plno = asset.Plno;
                                    hp04_Update.Spec = asset.Spec;
                                    hp04_Update.Supplier = asset.Supplier_ID;
                                    hp04_Update.Trdate = asset.Trdate;
                                    // hp04_Update.State = state; 
                                    // hp04_Update.Visible = asset.Deactivation_Date == "00000000";
                                    hp04_Update.Is_Update_To_SAP = false;
                                    int index = hp04_Updates.FindIndex(x =>
                                        x.Main_Asset_Number == hp04_Update.Main_Asset_Number &&
                                        x.AssnoID == hp04_Update.AssnoID &&
                                        x.OwnerFty == hp04_Update.OwnerFty);
                                    if (index == -1)
                                        hp04_Updates.Add(hp04_Update);
                                    else
                                        hp04_Updates[index] = hp04_Update;
                                }
                                else
                                {
                                    hp_a04 hp04_Add = new()
                                    {
                                        Askid = asset.Askid,
                                        AssnoID = string.IsNullOrWhiteSpace(asset.HP_Assno_ID)
                                            ? asset.Main_Asset_Number
                                            : asset.HP_Assno_ID,
                                        Company_Code = asset.Owner_Fty,
                                        Dept_ID = asset.Dept_ID,
                                        MachineName_EN = asset.Machine_Name_CN,
                                        Main_Asset_Number = asset.Main_Asset_Number,
                                        OwnerFty = _OwnerFty,
                                        Plno = asset.Plno,
                                        Spec = asset.Spec,
                                        State = state,
                                        Supplier = asset.Supplier_ID,
                                        Trdate = asset.Trdate,
                                        Visible = asset.Deactivation_Date == null || asset.Deactivation_Date == "00000000",
                                        Is_Update_To_SAP = true
                                    };
                                    int index = hp04_Adds.FindIndex(x =>
                                        x.Main_Asset_Number == hp04_Add.Main_Asset_Number &&
                                        x.AssnoID == hp04_Add.AssnoID &&
                                        x.OwnerFty == hp04_Add.OwnerFty);
                                    if (index == -1)
                                        hp04_Adds.Add(hp04_Add);
                                    else
                                        hp04_Adds[index] = hp04_Add;
                                }
                            }
                        }
                    }
                    if (hp04_Adds.Any() || hp04_Updates.Any())
                    {
                        if (hp04_Adds.Any())
                            _machineRepository.hp_a04.AddMultiple(hp04_Adds);
                        if (hp04_Updates.Any())
                            _machineRepository.hp_a04.UpdateMultiple(hp04_Updates);
                        await _machineRepository.SaveChangesAsync();
                    }
                    if (control_Updates.Any())
                    {
                        _sapRepository.Control_File.UpdateMultiple(control_Updates);
                        await _sapRepository.SaveChangesAsync();
                    }
                }
                SuccessLog(data);
                await _machineTransaction.CommitAsync();
            }
            catch (Exception ex)
            {
                ErrorLog(ex);
                await _machineTransaction.RollbackAsync();
            }
            finally
            {
                WorkLog($"Job end in {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                await Task.CompletedTask;
            }
        }

        private static void WorkLog(string message)
        {
            Logger logger = LogManager.GetLogger("createdatafromsapwork");
            logger.Log(NLog.LogLevel.Info, message);
        }

        private static void SuccessLog(List<CreateDataFromSAPJobDTO> logData)
        {
            string content = "";
            if (logData.Any())
            {
                logData
                .ForEach(x =>
                {
                    content += $"SID: {x.Control_File.SID} | Asset Numbers: {string.Join(", ", x.MT_Asset.Select(y => y.Main_Asset_Number))}{Environment.NewLine}";
                });
            }
            else
            {
                content += "No new data added";
            }

            Logger logger = LogManager.GetLogger("createdatafromsapsuccess");
            logger.Log(NLog.LogLevel.Info, $"{DateTime.Now} - Body: {Environment.NewLine}{content}");
        }

        private static void ErrorLog(Exception ex)
        {
            Logger logger = LogManager.GetLogger("createdatafromsaperror");
            logger.Log(NLog.LogLevel.Error, $"{DateTime.Now} {Environment.NewLine}==> Error: {ex}, {Environment.NewLine}==>Inner: {ex?.InnerException}");
        }
    }
}