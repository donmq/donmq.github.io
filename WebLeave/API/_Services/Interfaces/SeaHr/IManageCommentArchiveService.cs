using API.Dtos.Common;
namespace API._Services.Interfaces.SeaHr
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IManageCommentArchiveService
    {
       Task<PaginationUtility<CommentArchiveDto>> GetDataPagination(PaginationParam param);
      
       Task<OperationResult> Add(CommentArchiveDto commentArchiveDto, string username);
       Task<OperationResult> Delete(int commentArchiveID);

    }
}