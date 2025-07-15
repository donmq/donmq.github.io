

using System.Data.Entity;
using System.Globalization;
using Aspose.Cells;
using AutoMapper;
using LinqKit;
using Machine_API._Accessor;
using Machine_API._Service.interfaces;
using Machine_API.DTO;
using Machine_API.Helpers.Params;
using Machine_API.Helpers.Utilities;
using Machine_API.Models.MachineCheckList;

namespace Machine_API._Service.service
{
    public class AssetsLendMaintainService : IAssetsLendMaintainService
    {
        private readonly IMachineRepositoryAccessor _repository;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly IWebHostEnvironment _webHostEnv;

        public AssetsLendMaintainService(
            IMachineRepositoryAccessor repository,
            IMapper mapper,
            MapperConfiguration mapperConfiguration,
            IWebHostEnvironment webHostEnv)
        {
            _repository = repository;
            _mapper = mapper;
            _mapperConfiguration = mapperConfiguration;
            _webHostEnv = webHostEnv;
        }
        public async Task<OperationResult> Update(AssetsLendMaintainDto data)
        {
            var item = await _repository.Machine_IO
                .FirstOrDefaultAsync(x => x.IO_Date == data.IO_Date
                    && x.AssnoID == data.AssnoID
                    && x.IO_Kind == "O");
            var itemHp_a04 = await _repository.hp_a04
                .FirstOrDefaultAsync(x => x.AssnoID == data.AssnoID
                    && x.OwnerFty == data.OwnerFty);
            if (item == null)
                return new OperationResult("Not Found", false);

            if (data.Re_Confirm == "Y")
            {
                item.Re_Confirm = "Y";
                item.Re_Date = data.Re_Date;
            }
            itemHp_a04.Visible = true;
            try
            {
                _repository.Machine_IO.Update(item);
                _repository.hp_a04.Update(itemHp_a04);
                await _repository.SaveChangesAsync();
                return new OperationResult("Update Successfully", true);
            }
            catch
            {
                return new OperationResult("Update Failed", false);
            }
        }
        public async Task<List<AssetsLendMaintainDto>> GetData(AssetsLendMaintainParam param)
        {
            var pred = PredicateBuilder.New<Machine_IO>(true);
            if (!string.IsNullOrWhiteSpace(param.LendDate))
                pred = pred.And(x => x.IO_Date == Convert.ToDateTime(param.LendDate));

            if (!string.IsNullOrWhiteSpace(param.MachineID))
                pred = pred.And(x => x.AssnoID == param.MachineID);

            if (!string.IsNullOrWhiteSpace(param.LendTo))
                pred = pred.And(x => x.OwnerFty == param.LendTo);

            if (!string.IsNullOrWhiteSpace(param.Return) && param.Return != "ALL")
                pred = pred.And(x => x.Re_Confirm == param.Return);

            List<AssetsLendMaintainDto> data = new List<AssetsLendMaintainDto>();
            return data = await _repository.Machine_IO.FindAll(pred)
                    .Select((x, i) => new AssetsLendMaintainDto
                    {
                        STT = i + 1,
                        AssnoID = x.AssnoID,
                        MachineName_EN = x.MachineName_EN,
                        Spec = x.Spec,
                        Supplier = x.Supplier,
                        OwnerFty = x.OwnerFty,
                        IO_Reason = x.IO_Reason,
                        IO_Date = x.IO_Date,
                        IO_Confirm = x.IO_Confirm,
                        Re_Date = x.Re_Date,
                        Re_Confirm = x.Re_Confirm,
                        Remark = x.Remark
                    })
                    .ToListAsync();
        }
        public async Task<PageListUtility<AssetsLendMaintainDto>> GetDataPagination(PaginationParams pagination, AssetsLendMaintainParam param)
        {
            var data = await GetData(param);
            return PageListUtility<AssetsLendMaintainDto>.PageList(data, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<byte[]> DownloadExcel(AssetsLendMaintainParam param)
        {
            var data = await GetData(param);
            MemoryStream stream = new();
            if (data.Any())
            {
                var path = Path.Combine(_webHostEnv.ContentRootPath, "wwwroot\\Resources\\Template\\AssetsLendMaintainExport.xlsx");
                WorkbookDesigner designer = new();
                designer.Workbook = new Workbook(path);
                Worksheet ws = designer.Workbook.Worksheets[0];

                int itemNumber = 1;
                foreach (var item in data)
                {
                    // item.Item = itemNumber;
                    itemNumber++;
                }

                designer.SetDataSource("result", data);
                designer.Process();

                ws.AutoFitColumns();
                ws.PageSetup.CenterHorizontally = true;
                ws.PageSetup.FitToPagesWide = 1;
                ws.PageSetup.FitToPagesTall = 0;

                designer.Workbook.Save(stream, SaveFormat.Xlsx);
            }
            return stream.ToArray();
        }

        public async Task<OperationResult> UploadExcel(IFormFile file)
        {
            if (file == null)
                return new OperationResult("File not found.", false);
            using var stream = file.OpenReadStream();
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(stream);
            Worksheet ws = designer.Workbook.Worksheets[0];

            int rows = ws.Cells.MaxDataRow;
            if (rows < 2)
                return new OperationResult("An empty excel file", false);
            List<Machine_IO> data = new List<Machine_IO>();
            using (var _transaction = await _repository.BeginTransactionAsync())
            {
                for (int i = 2; i <= rows; i++)
                {
                    if (ws.Cells[i, 0].Value == null || ws.Cells[i, 0].StringValue.Length > 20)
                        return new OperationResult { Success = false, Message = $"AssnoID at row {i} invalid !" };

                    if (ws.Cells[i, 1].Value == null || ws.Cells[i, 1].StringValue.Length > 100)
                        return new OperationResult { Success = false, Message = $"Spec at row {i} invalid !" };

                    if (ws.Cells[i, 2].Value == null || ws.Cells[i, 2].StringValue.Length > 10)
                        return new OperationResult { Success = false, Message = $"Dept ID at row {i} invalid !" };

                    if (ws.Cells[i, 3].Value == null || ws.Cells[i, 3].StringValue.Length > 20)
                        return new OperationResult { Success = false, Message = $"Plno at row {i} invalid !" };

                    if (ws.Cells[i, 4].Value == null || ws.Cells[i, 4].StringValue.Length > 10)
                        return new OperationResult { Success = false, Message = $"State at row {i} invalid !" };

                    if (ws.Cells[i, 5].Value == null || ws.Cells[i, 5].StringValue.Length > 100)
                        return new OperationResult { Success = false, Message = $"Machine Name EN at row {i} invalid !" };

                    if (ws.Cells[i, 6].Value == null || ws.Cells[i, 6].StringValue.Length > 100)
                        return new OperationResult { Success = false, Message = $"Machine Name Local at row {i} invalid !" };

                    if (ws.Cells[i, 7].Value == null || ws.Cells[i, 7].StringValue.Length > 100)
                        return new OperationResult { Success = false, Message = $"Machine Name CN at row {i} invalid !" };

                    if (ws.Cells[i, 8].Value == null || ws.Cells[i, 8].StringValue.Length > 100)
                        return new OperationResult { Success = false, Message = $"Supplier at row {i} invalid !" };

                    if (ws.Cells[i, 9].Value == null || ws.Cells[i, 9].StringValue.Length > 1)
                        return new OperationResult { Success = false, Message = $"OwnerFty at row {i} invalid !" };

                    if (ws.Cells[i, 10].Value == null || ws.Cells[i, 10].StringValue.Length > 1)
                        return new OperationResult { Success = false, Message = $"Using Fty at row {i} invalid !" };

                    var MachineID = ws.Cells[i, 0].StringValue;
                    var Spec = ws.Cells[i, 1].StringValue;
                    var DeptID = ws.Cells[i, 2].StringValue;
                    var Plno = ws.Cells[i, 3].StringValue;
                    var State = ws.Cells[i, 4].StringValue;
                    var MachineName_EN = ws.Cells[i, 5].StringValue;
                    var MachineName_Local = ws.Cells[i, 6].StringValue;
                    var MachineName_CN = ws.Cells[i, 7].StringValue;
                    var Supplier = ws.Cells[i, 8].StringValue;
                    var OwnerFty = ws.Cells[i, 9].StringValue;
                    var UsingFty = ws.Cells[i, 10].StringValue;

                    if (_repository.Machine_IO.Any(x => x.AssnoID == MachineID.Trim()))
                        return new OperationResult($"AssnoID at row {i} already exists.", false);
                    else
                    {
                        var newData = new Machine_IO();

                        newData.AssnoID = MachineID;
                        newData.Spec = Spec;
                        newData.Dept_ID = DeptID;
                        newData.Plno = Plno;
                        newData.State = State;
                        newData.MachineName_EN = MachineName_EN;
                        newData.MachineName_Local = MachineName_Local;
                        newData.MachineName_CN = MachineName_CN;
                        newData.Supplier = Supplier;
                        newData.OwnerFty = OwnerFty;
                        newData.UsingFty = UsingFty;
                        newData.IO_Kind = "O";
                        newData.IO_Reason = "";
                        newData.IO_Date = DateTime.Now;
                        newData.IO_Confirm = "N";
                        newData.Re_Date = DateTime.Now;
                        newData.Re_Confirm = "N";
                        newData.Remark = "";
                        newData.Insert_At = DateTime.Now;
                        newData.Insert_By = "userName";
                        newData.Update_At = DateTime.Now;
                        newData.Update_By = "userName";

                        data.Add(newData);
                    }
                }

                _repository.Machine_IO.AddMultiple(data);
                try
                {
                    await _repository.SaveChangesAsync();
                    await _transaction.CommitAsync();
                    var extension = Path.GetExtension(file.FileName).ToLower();
                    var uploadedFile = $"AssetsLendMaintainUpload_{DateTime.Now.ToString("yyyyMMddHHmmss")}{extension}";
                    string uploadedPath = @"uploaded/excels/AssetsLendMaintain";
                    string folder = Path.Combine(_webHostEnv.WebRootPath, uploadedPath);

                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);
                    string filePath = Path.Combine(folder, uploadedFile);

                    using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        stream.CopyTo(fileStream);
                    }
                    return new OperationResult { Success = false, Message = "File was uploaded." };
                }
                catch (System.Exception)
                {
                    await _transaction.RollbackAsync();
                    return new OperationResult { Success = false, Message = "Uploading file failed on save. Please check the excel data again" };
                }
            }
        }
        public async Task<List<KeyValuePair<string, string>>> GetListLendTo()
        {
            var data = await _repository.Machine_IO.FindAll()
                .Select(x => new KeyValuePair<string, string>(x.OwnerFty, x.OwnerFty))
                .ToListAsync();
            return data;
        }
    }
}