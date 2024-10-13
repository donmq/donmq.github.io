using API.Helper.Attributes;
using API.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace API._Repositories
{
    [DependencyInjectionAttribute(ServiceLifetime.Scoped)]
    public interface IRepositoryAccessor
    {

        Task<bool> Save();
        Task<IDbContextTransaction> BeginTransactionAsync();
        IRepository<BaiTap> BaiTap { get; }
        IRepository<ChatLuongAfter> ChatLuongAfter { get; }
        IRepository<ChatLuongBefore> ChatLuongBefore { get; }
        IRepository<LoaiThuocTinh> LoaiThuocTinh { get; }
        IRepository<P_ThongTinViTriCauThu> P_ThongTinViTriCauThu { get; }
        IRepository<P_ThuocTinhSangToi> P_ThuocTinhSangToi { get; }
        IRepository<P_ThuocTinhBaiTap> P_ThuocTinhBaiTap { get; }
        IRepository<ThongTin> ThongTin { get; }
        IRepository<ThuocTinhChinh> ThuocTinhChinh { get; }
        IRepository<ViTri> ViTri { get; }
    }
}