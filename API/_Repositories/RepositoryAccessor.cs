using API.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace API._Repositories
{
    public class RepositoryAccessor : IRepositoryAccessor
    {
        private readonly DBContext _dbContext;
        public RepositoryAccessor(DBContext dbContext)
        {
            _dbContext = dbContext;
            BaiTap = new Repository<BaiTap>(_dbContext);
            ChatLuongCauThu = new Repository<ChatLuongCauThu>(_dbContext);
            LoaiThuocTinh = new Repository<LoaiThuocTinh>(_dbContext);
            P_ThongTinViTriCauThu = new Repository<P_ThongTinViTriCauThu>(_dbContext);
            P_ThuocTinhSangToi = new Repository<P_ThuocTinhSangToi>(_dbContext);
            ThongTin = new Repository<ThongTin>(_dbContext);
            ThuocTinhChinh = new Repository<ThuocTinhChinh>(_dbContext);
            ViTri = new Repository<ViTri>(_dbContext);
        }

        public IRepository<BaiTap> BaiTap { get; set; }

        public IRepository<LoaiThuocTinh> LoaiThuocTinh { get; set; }

        public IRepository<P_ThongTinViTriCauThu> P_ThongTinViTriCauThu { get; set; }

        public IRepository<P_ThuocTinhSangToi> P_ThuocTinhSangToi { get; set; }

        public IRepository<ThongTin> ThongTin { get; set; }

        public IRepository<ThuocTinhChinh> ThuocTinhChinh { get; set; }

        public IRepository<ViTri> ViTri { get; set; }

        public IRepository<ChatLuongCauThu> ChatLuongCauThu { get; set; }

        public async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }
    }
}