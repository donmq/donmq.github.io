using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspose.Cells;
using eTierV2_API._Repositories;
using eTierV2_API._Repositories.Interfaces;
using eTierV2_API._Services.Interfaces.Production.T1.C2B;
using eTierV2_API.Helpers.Utilities;
using eTierV2_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace eTierV2_API._Services.Services.Production.T1.C2B
{
    public class T5ExternalUploadService : IT5ExternalUploadService
    {
        private readonly IFunctionUtility _functionUtility;
         private readonly IRepositoryAccessor _repoAccessor;
        public T5ExternalUploadService(IFunctionUtility functionUtility, IRepositoryAccessor repoAccessor)
        {
            _functionUtility = functionUtility;
            _repoAccessor = repoAccessor;
        }

        public async Task<PaginationUtility<eTM_HP_Efficiency_Data_External>> GetData(PaginationParam pagination)
        {
            var data = _repoAccessor.eTM_HP_Efficiency_Data_External.FindAll();
            return await PaginationUtility<eTM_HP_Efficiency_Data_External>.CreateAsync(data, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<OperationResult> UploadExcel(IFormFile file)
        {
            // Check excel 
            var excelResult = ExcelUtility.CheckExcel(file, "Resources\\Template\\Transaction\\T5ExternalUpload\\T5ExternalUploadTemplate.xlsx");
            if (!excelResult.IsSuccess)
                return new OperationResult(false, excelResult.Error);
            List<eTM_HP_Efficiency_Data_External> datas = new();
            foreach (Worksheet ws in excelResult.wb.Worksheets)
            {
                Worksheet wsTemp = excelResult.wbTemp.Worksheets[ws.Index];
                ws.Cells.DeleteBlankRows();
                wsTemp.Cells.DeleteBlankRows();
                int start = wsTemp.Cells.Rows.Count;
                int count = ws.Cells.Rows.Count;
                for (int j = start; j < count; j++)
                {
                    var date_Range_Type = ws.Cells[j, 0].StringValue;
                    var date_Range_Label = ws.Cells[j, 1].StringValue;
                    var sequence = !string.IsNullOrWhiteSpace(ws.Cells[j, 2].Value?.ToString()) ? ws.Cells[j, 2].StringValue?.Replace("%", "") : null;
                    var performance_Type = ws.Cells[j, 3].StringValue;
                    var shc = !string.IsNullOrWhiteSpace(ws.Cells[j, 4].Value?.ToString()) ? ws.Cells[j, 4].StringValue?.Replace("%", "") : null;
                    var cb = !string.IsNullOrWhiteSpace(ws.Cells[j, 5].Value?.ToString()) ? ws.Cells[j, 5].StringValue?.Replace("%", "") : null;
                    var tsh = !string.IsNullOrWhiteSpace(ws.Cells[j, 6].Value?.ToString()) ? ws.Cells[j, 6].StringValue?.Replace("%", "") : null;
                    var entity = new eTM_HP_Efficiency_Data_External
                    {
                        Date_Range_Type = date_Range_Type,
                        Date_Range_Label = date_Range_Label,
                        Sequence = Convert.ToDecimal(sequence),
                        Performance_Type = performance_Type,
                        SHC = Convert.ToDecimal(shc),
                        CB = Convert.ToDecimal(cb),
                        TSH = Convert.ToDecimal(tsh)
                    };
                    datas.Add(entity);
                }
            }

            try
            {
                var dataDeletes = await _repoAccessor.eTM_HP_Efficiency_Data_External.FindAll().ToListAsync();
                _repoAccessor.eTM_HP_Efficiency_Data_External.RemoveMultiple(dataDeletes);
                _repoAccessor.eTM_HP_Efficiency_Data_External.AddMultiple(datas);
                await _repoAccessor.eTM_HP_Efficiency_Data_External.SaveAll();
                string folder = "uploaded/excels/Transaction/T5ExternalUpload";
                await _functionUtility.UploadAsync(file, folder, $"T5ExternalUpload_{DateTime.Now:yyyyMMddHHmmss}");
                return new OperationResult(true);
            }
            catch (System.Exception)
            {
                return new OperationResult(false);
            }
        }
    }
}