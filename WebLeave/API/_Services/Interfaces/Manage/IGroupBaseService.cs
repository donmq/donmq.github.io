using API.Dtos.Manage;
using API.Dtos.Manage.GroupBaseManage;
namespace API._Services.Interfaces.Manage
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    public interface IGroupBaseService
    {
        Task<List<GroupBaseDto>> GetGroupBaseData();
        Task<GroupBaseAndGroupLangDto> GetDetailGroupBase(int IDGroupBase);
        Task<OperationResult> AddGroup(GroupBaseAndGroupLangDto GroupBaseAndGroupLang);
        Task<OperationResult> EditGroup(GroupBaseAndGroupLangDto GroupBaseAndGroupLang);
        Task<OperationResult> RemoveGroup(int GBID);
        Task<OperationResult> ExportExcel(GroupBaseTitleExcel title);
    }
}