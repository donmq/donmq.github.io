using API._Repositories;
using API._Services.Interfaces.SeaHr;
using API.Dtos.Common;
using API.Helpers.Enums;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
namespace API._Services.Services.SeaHr
{
    public class ManageCommentArchiveService : IManageCommentArchiveService
    {
        private readonly IRepositoryAccessor _repository;
        private readonly IMapper _mapper;

        public ManageCommentArchiveService(
            IRepositoryAccessor repository,
            IMapper mapper
           )
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<List<CommentArchiveDto>> GetData()
        {
            var data = await _repository.CommentArchive.FindAll().ToListAsync();
            var result = _mapper.Map<List<CommentArchiveDto>>(data);
            return result;
        }

        public async Task<PaginationUtility<CommentArchiveDto>> GetDataPagination(PaginationParam param)
        {
            List<CommentArchiveDto> data = await _repository.CommentArchive.FindAll()
            .Include(x => x.User)
            .Select(x => new CommentArchiveDto
            {
                CommentArchiveID = x.CommentArchiveID,
                    UserID = x.UserID,
                    Value = x.Value,
                    Comment = x.Comment,
                    CreateName = x.User.FullName,
                    CreateTime = x.CreateTime
            }).ToListAsync();
            
            return PaginationUtility<CommentArchiveDto>.Create(data, param.PageNumber, param.PageSize);
        }

        public async Task<OperationResult> Add(CommentArchiveDto commentArchiveDto, string username)
        {
            CommentArchive data = await _repository.CommentArchive.FirstOrDefaultAsync(x => x.Value == commentArchiveDto.Value);
            // Nếu Item có value đã tồn tại thì trả ra lỗi
            if (data != null)
                return new OperationResult(false, MessageConstants.EXISTS);
            // Ngược lại thêm mới
            else
            {
                // Tìm Id của User
                Users user = _repository.Users.FirstOrDefault(x => x.UserName == username);
                // Nếu user có giá trị thì gán
                if (user != null)
                    commentArchiveDto.UserID = user.UserID;

                CommentArchive model = _mapper.Map<CommentArchive>(commentArchiveDto);
                _repository.CommentArchive.Add(model);
                await _repository.SaveChangesAsync();
                return new OperationResult(true, MessageConstants.ADD_SUCCESS, MessageConstants.SUCCESS);
            }
        }

        public async Task<OperationResult> Delete(int commentArchiveID)
        {
            CommentArchive data = await _repository.CommentArchive.FirstOrDefaultAsync(x => x.CommentArchiveID == commentArchiveID);

            if (data == null)
                return new OperationResult(false, MessageConstants.REMOVE_ERROR);

            _repository.CommentArchive.Remove(data);
            await _repository.SaveChangesAsync();
            return new OperationResult(true, MessageConstants.REMOVE_SUCCESS);
        }
    }
}