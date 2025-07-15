using Aspose.Cells;
using Machine_API._Accessor;
using Machine_API._Service.interfaces;
using Machine_API.Helpers.Utilities;
using Machine_API.Models.MachineCheckList;
using Microsoft.EntityFrameworkCore;

namespace Machine_API._Service.service
{
    public class UploadDataHPA15Service : IUploadDataHPA15Service
    {
        private readonly IMachineRepositoryAccessor _machineRepository;

        public UploadDataHPA15Service(IMachineRepositoryAccessor machineRepository)
        {
            _machineRepository = machineRepository;
        }

        public async Task<OperationResult> ImportDataExcel(IFormFile file)
        {
            if (file == null)
            {
                return new OperationResult("File not found.", false);
            }

            using Stream stream = file.OpenReadStream();
            WorkbookDesigner designer = new()
            {
                Workbook = new Workbook(stream)
            };
            Worksheet ws = designer.Workbook.Worksheets[0];
            ws.Cells.DeleteBlankColumns();
            ws.Cells.DeleteBlankRows();
            var rows = ws.Cells.MaxDataRow + 1;

            List<hp_a15> datas = await _machineRepository.hp_a15.FindAll().ToListAsync();
            for (int i = 2; i <= rows; i++)
            {
                var hp_a15 = datas.FirstOrDefault(x => x.Plno.Trim() == ws.Cells["A" + i].StringValue.Trim());
                if (hp_a15 != null)
                    hp_a15.State = ws.Cells["C" + i].StringValue[..1];
            }

            try
            {
                _machineRepository.hp_a15.UpdateMultiple(datas);
                await _machineRepository.SaveChangesAsync();

                return new OperationResult("Upload file successfully", true);
            }
            catch (System.Exception)
            {
                return new OperationResult("Upload file failed", true);
            }
        }
    }
}