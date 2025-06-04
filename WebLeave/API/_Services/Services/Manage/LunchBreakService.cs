using API._Repositories;
using API._Services.Interfaces.Manage;
using API.Dtos.Common;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services.Manage
{
    public class LunchBreakService : ILunchBreakService
    {
        private readonly IRepositoryAccessor _repositoryAccessor;

        public LunchBreakService(IRepositoryAccessor repositoryAccessor)
        {
            _repositoryAccessor = repositoryAccessor;
        }

        public async Task<OperationResult> Create(LunchBreakDto dto)
        {
            if (await _repositoryAccessor.LunchBreak.AnyAsync(x => x.Key.Trim() == dto.Key.Trim() && x.WorkTimeStart == TimeSpan.Parse(dto.WorkTimeStart as string) && x.WorkTimeEnd == TimeSpan.Parse(dto.WorkTimeEnd as string)))
                return new OperationResult { IsSuccess = false };

            LunchBreak data = new()
            {
                Id = dto.Id,
                Key = dto.Key.Trim(),
                WorkTimeStart = TimeSpan.Parse(dto.WorkTimeStart as string),
                WorkTimeEnd = TimeSpan.Parse(dto.WorkTimeEnd as string),
                LunchTimeStart = TimeSpan.Parse(dto.LunchTimeStart as string),
                LunchTimeEnd = TimeSpan.Parse(dto.LunchTimeEnd as string),
                Value_en = dto.Value_en,
                Value_vi = dto.Value_vi,
                Value_zh = dto.Value_zh,
                Seq = dto.Seq,
                Visible = dto.Visible,
                CreatedBy = dto.CreatedBy,
                CreatedTime = DateTime.Now
            };

            try
            {
                _repositoryAccessor.LunchBreak.Add(data);
                await _repositoryAccessor.SaveChangesAsync();

                return new OperationResult { IsSuccess = true };
            }
            catch
            {
                return new OperationResult { IsSuccess = false };
            }
        }

        public async Task<OperationResult> Delete(int Id)
        {
            var data = await _repositoryAccessor.LunchBreak.FirstOrDefaultAsync(x => x.Id == Id);
            if (data is null)
                return new OperationResult { IsSuccess = false, Error = "Data not existed !" };

            _repositoryAccessor.LunchBreak.Remove(data);
            return new OperationResult { IsSuccess = await _repositoryAccessor.SaveChangesAsync() };
        }

        public async Task<PaginationUtility<LunchBreakDto>> GetDataPagination(PaginationParam pagination, bool isPaging)
        {
            var data = _repositoryAccessor.LunchBreak.FindAll(true)
                .Select(x => new LunchBreakDto
                {
                    Id = x.Id,
                    Key = x.Key,
                    WorkTimeStart = x.WorkTimeStart.ToString(@"hh\:mm"),
                    WorkTimeEnd = x.WorkTimeEnd.ToString(@"hh\:mm"),
                    LunchTimeStart = x.LunchTimeStart.ToString(@"hh\:mm"),
                    LunchTimeEnd = x.LunchTimeEnd.ToString(@"hh\:mm"),
                    Value_en = x.Value_en,
                    Value_vi = x.Value_vi,
                    Value_zh = x.Value_zh,
                    Seq = x.Seq,
                    Visible = x.Visible,
                    CreatedTime = x.CreatedTime,
                    UpdatedTime = x.UpdatedTime
                }).OrderBy(x => x.Seq);

            return await PaginationUtility<LunchBreakDto>.CreateAsync(data, pagination.PageNumber, pagination.PageSize, isPaging);
        }

        public async Task<LunchBreakDto> GetDetail(int Id)
        {
            var data = await _repositoryAccessor.LunchBreak
                .FindAll(x => x.Id == Id)
                .Select(x => new LunchBreakDto
                {
                    Id = x.Id,
                    Key = x.Key,
                    WorkTimeStart = new DateTime(x.WorkTimeStart.Ticks),
                    WorkTimeEnd = new DateTime(x.WorkTimeEnd.Ticks),
                    LunchTimeStart = new DateTime(x.LunchTimeStart.Ticks),
                    LunchTimeEnd = new DateTime(x.LunchTimeEnd.Ticks),
                    Value_en = x.Value_en,
                    Value_vi = x.Value_vi,
                    Value_zh = x.Value_zh,
                    Seq = x.Seq,
                    Visible = x.Visible,
                    CreatedTime = x.CreatedTime,
                    UpdatedTime = x.UpdatedTime
                }).FirstOrDefaultAsync();

            return data;
        }

        public async Task<List<LunchBreakDto>> GetListLunchBreak()
        {
            var data = await _repositoryAccessor.LunchBreak
                .FindAll(x => x.Visible == true, true)
                .Select(x => new LunchBreakDto
                {
                    Id = x.Id,
                    Key = x.Key,
                    WorkTimeStart = x.WorkTimeStart.ToString(@"hh\:mm"),
                    WorkTimeEnd = x.WorkTimeEnd.ToString(@"hh\:mm"),
                    LunchTimeStart = x.LunchTimeStart.ToString(@"hh\:mm"),
                    LunchTimeEnd = x.LunchTimeEnd.ToString(@"hh\:mm"),
                    Value_en = x.Value_en,
                    Value_vi = x.Value_vi,
                    Value_zh = x.Value_zh,
                    Seq = x.Seq,
                    Visible = x.Visible,
                    CreatedTime = x.CreatedTime,
                    UpdatedTime = x.UpdatedTime
                })
                .OrderBy(x => x.Seq).ToListAsync();

            return data;
        }

        public async Task<OperationResult> Update(LunchBreakDto dto)
        {
            var data = await _repositoryAccessor.LunchBreak.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (data is null)
                return new OperationResult { IsSuccess = false, Error = "Data not existed !" };

            data.WorkTimeStart = TimeSpan.Parse(dto.WorkTimeStart as string);
            data.WorkTimeEnd = TimeSpan.Parse(dto.WorkTimeEnd as string);
            data.LunchTimeStart = TimeSpan.Parse(dto.LunchTimeStart as string);
            data.LunchTimeEnd = TimeSpan.Parse(dto.LunchTimeEnd as string);
            data.Value_en = dto.Value_en;
            data.Value_vi = dto.Value_vi;
            data.Value_zh = dto.Value_zh;
            data.Seq = dto.Seq;
            data.Visible = dto.Visible;
            data.UpdatedBy = dto.UpdatedBy;
            data.UpdatedTime = DateTime.Now;

            try
            {
                _repositoryAccessor.LunchBreak.Update(data);
                await _repositoryAccessor.SaveChangesAsync();

                return new OperationResult { IsSuccess = true };
            }
            catch
            {
                return new OperationResult { IsSuccess = false };
            }
        }
    }
}