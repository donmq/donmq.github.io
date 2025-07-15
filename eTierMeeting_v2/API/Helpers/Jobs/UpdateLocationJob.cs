using Machine_API._Accessor;
using Machine_API.Models.MT;
using Microsoft.EntityFrameworkCore;
using NLog;
using Quartz;

namespace Machine_API.Helpers.Jobs
{
    // Job work at 20:05:00pm every day
    public class UpdateLocationJob : IJob
    {
        private readonly IMachineRepositoryAccessor _machineRepository;
        private readonly IMTRepositoryAccessor _mtRepository;
        private readonly string _OwnerFty;
        private readonly string _factory_id;

        public UpdateLocationJob(
            IMachineRepositoryAccessor machineRepository,
            IMTRepositoryAccessor mtRepository,
            IConfiguration configuration)
        {
            _machineRepository = machineRepository;
            _mtRepository = mtRepository;
            _OwnerFty = configuration.GetSection("AppSettings:OwnerFty").Value;
            _factory_id = configuration.GetSection("AppSettings:Factory_Id").Value;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            // Kiểm tra trong ngày hôm nay có bao nhiêu máy di chuyển vị trí(tại bảng History)
            var hpData = await _machineRepository.hp_a04.FindAll(x => x.Is_Update_To_SAP == false && x.OwnerFty.Trim() == _OwnerFty).ToListAsync();
            if (hpData.Any())
            {
                WorkLog($"Job start in {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                // lấy danh sách AssNoId (Mã máy) không trùng 
                var assnoIDsCount = hpData.Select(x => x.AssnoID).Distinct().Count();
                // Tạo mã SidId
                string sidIdDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                string sidId = $"{sidIdDateTime}{(Convert.ToInt32(0) + 1).ToString().PadLeft(6, '0')}";

                var mainAssetNumbers = new List<string>();
                var sap_Cost_Center_Changeds = new List<SAP_Cost_Center_Changed_Record>();
                // Tổng số máy di chuyển
                try
                {
                    // Thêm mới vào bảng [Control_File_Temp]
                    _mtRepository.Control_File_Temp.Add(new Control_File_Temp()
                    {
                        SPEC_ID = "MT_FI001",
                        SID = sidId, // chung 1 mã tự động Gen
                        Table_Name = "VW_SAP_Cost_Center_Changed",
                        Factory_ID = _factory_id,
                        Count = assnoIDsCount, // Số lượng máy được di chuyển
                        Control_Flag = "N",
                        Update_Time = DateTime.Now,
                        Table_Count = 1
                    });
                    await _mtRepository.SaveChangesAsync();

                    //số máy di chuyển
                    foreach (var hp in hpData)
                    {
                        // lấy vị trí hiện tại của máy theo assnoId
                        mainAssetNumbers.Add(hp.Main_Asset_Number);
                        sap_Cost_Center_Changeds.Add(new SAP_Cost_Center_Changed_Record()
                        {
                            SID = sidId, // chung 1 mã tự động Gen
                            Company_Code = hp.Company_Code, // Mã công ty
                            Main_Asset_Number = hp.Main_Asset_Number, // Mã máy
                            Asset_Subnumber = "0", // Người yêu cầu chuyển máy
                            Asset_Location = hp.Plno, // vị trí hiện tại của máy
                        });
                    }

                    if (sap_Cost_Center_Changeds.Any())
                    {
                        _mtRepository.SAP_Cost_Center_Changed_Record.AddMultiple(sap_Cost_Center_Changeds);
                        await _mtRepository.SaveChangesAsync();
                    }

                    // Cập nhật trạng thái [hp_a04] đã cập nhật tới SAP
                    hpData.ForEach(x => x.Is_Update_To_SAP = true);
                    _machineRepository.hp_a04.UpdateMultiple(hpData);
                    await _machineRepository.SaveChangesAsync();

                    SuccessLog(sidId, mainAssetNumbers);
                }
                catch (Exception ex)
                {
                    ErrorLog(ex);
                }

                WorkLog($"Job end in {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            }
            await Task.CompletedTask;
        }

        private static void WorkLog(string message)
        {
            Logger logger = LogManager.GetLogger("updatelocationwork");
            logger.Log(NLog.LogLevel.Info, message);
        }

        private static void SuccessLog(string sidId, List<string> mainAssetNumbers)
        {
            string content = $"SID: {sidId} | Asset Numbers: {string.Join(", ", mainAssetNumbers)}";
            Logger logger = NLog.LogManager.GetLogger("updatelocationsuccess");
            logger.Log(NLog.LogLevel.Info, $"{DateTime.Now} - Body: {Environment.NewLine}{content}");
        }

        private static void ErrorLog(Exception ex)
        {
            Logger logger = NLog.LogManager.GetLogger("updatelocationerror");
            logger.Log(NLog.LogLevel.Error, $"{DateTime.Now} {Environment.NewLine}==> Error: {ex}, {Environment.NewLine}==>Inner: {ex?.InnerException}");
        }
    }
}