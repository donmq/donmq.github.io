using Machine_API._Accessor;
using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Machine_API.Helpers.Utilities;
using Machine_API.Models.MachineCheckList;
using Microsoft.EntityFrameworkCore;
namespace Machine_API._Service.service
{
    public class CheckMachineSafetyService : ICheckMachineSafetyService
    {
        private readonly IMachineRepositoryAccessor _repository;
        private readonly IConfiguration _configuration;
        private readonly string _factory;

        public CheckMachineSafetyService(
            IMachineRepositoryAccessor repository,
            IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
            _factory = _configuration.GetSection("AppSettings:Factory").Value;
        }

        public async Task<Machine_Safe_CheckDto> GetMachine(string idMachine, string lang)
        {
            //Kiểm tra nếu có dấu '/' trong mã máy
            if (idMachine.Contains('/'))
            {
                idMachine = idMachine.Replace(@"/", "");
            }
            string ownfty = idMachine[..1];
            string idMachineSplit = idMachine[1..];

            var hpa04 = _repository.hp_a04.FindAll(x => x.Visible == true && x.AssnoID == idMachineSplit && x.OwnerFty == ownfty);
            var hpa15 = _repository.hp_a15.FindAll();
            var dataJoin = await hpa04.Join(hpa15, x => x.Plno, y => y.Plno, (x, y)
                => new Machine_Safe_CheckDto()
                {
                    MachineID = x.AssnoID,
                    OwnerFty = x.OwnerFty,
                    MachineName = lang == "vi-VN" ? x.MachineName_Local : (lang == "zh-TW" ? x.MachineName_CN : x.MachineName_EN),
                    Location = y.Place == null ? "" : $"{y.Place}-{y.Plno}"
                }).FirstOrDefaultAsync();

            return dataJoin;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListQuestion(string lang)
        {
            return await _repository.Machine_Safe_Checklist.FindAll()
                .Select(x => KeyValuePair.Create(
                    x.Id.ToString(),
                    lang == "vi-VN" ? x.ChecklistName_Local :
                    (lang == "zh-TW" ? x.ChecklistName_CN : x.ChecklistName_EN)
                ))
                .ToListAsync();
        }

        public async Task<OperationResult> SaveMachineSafetyCheck(SurveyRequest request)
        {
            if (request?.Questions == null || !request.Questions.Any())
                return new OperationResult { Success = false, Message = "Please select at least one question" };
            // check data exists
            List<Machine_Safe_Check> dataExists = await _repository.Machine_Safe_Check.FindAll(x => x.AssnoID == request.AssnoID && x.CheckDate.Date == request.CheckDate.Date).ToListAsync();
            // remove data exists
            List<string> imagesToRemove = new();
            if (dataExists.Any())
            {
                // list image to remove
                imagesToRemove = dataExists.FindAll(x => !string.IsNullOrEmpty(x.Pic_Path)).Select(x => x.Pic_Path).ToList();
                _repository.Machine_Safe_Check.RemoveMultiple(dataExists);
            }
            string dept_ID = _repository.hp_a04.FirstOrDefault(x => x.Visible == true && x.AssnoID == request.AssnoID && x.OwnerFty == request.OwnerFty)?.Dept_ID;
            string timeNow = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string timeNowFolder = DateTime.Now.ToString("yyyyMMdd");
            List<Machine_Safe_Check> data = new();
            List<string> savedFiles = new();
            List<(IFormFile Image, string FilePath)> imagesToSave = new(); // Danh sách ảnh temp
            foreach (var question in request.Questions)
            {
                string pathImage = string.Empty;
                // nếu có hình ảnh
                if (question.Image != null)
                {
                    string fileName = $"{request.OwnerFty}{request.AssnoID}_{question.Key}_{question.Value.NonUnicode()}_{timeNow}.jpg";
                    pathImage = Path.Combine(_factory, "2_4_CheckMachineSafety", timeNowFolder, fileName);
                    // Đường dẫn lưu file trong thư mục wwwroot/uploads/2_4_CheckMachineSafety/....
                    imagesToSave.Add((question.Image, Path.Combine(pathImage)));
                }
                Machine_Safe_Check _model = new()
                {
                    CheckDate = DateTime.Now,
                    AssnoID = request.AssnoID, // remove first character of machine_code
                    Dept_ID = dept_ID, // get from hp_a04
                    Check_Item = question.Key, // id of question (Machine_Safe_Checklist),
                    Resault = question.Answer.ToUpper(), // N/A, Y, N
                    Pic_Path = pathImage.Replace("\\", "/"),
                    CreateTime = DateTime.Now,
                    CreateBy = request.UserName
                };
                data.Add(_model);
            }
            _repository.Machine_Safe_Check.AddMultiple(data);
            try
            {
                // save database
                await _repository.SaveChangesAsync();
                // remove images old
                foreach (var item in imagesToRemove)
                {
                    string pathImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", item.Replace("/", "\\"));
                    if (!string.IsNullOrEmpty(pathImage) && File.Exists(pathImage))
                    {
                        File.Delete(pathImage);
                    }
                }
                // save images
                foreach (var (image, filePath) in imagesToSave)
                {
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", filePath);

                    string directoryPath = Path.GetDirectoryName(fullPath);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    // Save the image
                    using var stream = new FileStream(fullPath, FileMode.Create);
                    await image.CopyToAsync(stream);
                }

                return new OperationResult { Success = true };

            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = $"Inner exception: {ex.InnerException?.Message ?? "Unknown error"}" };
            };
        }
    }
}