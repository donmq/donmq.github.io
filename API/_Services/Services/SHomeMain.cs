
using API._Repositories;
using API._Services.Interfaces;
using API.DTO;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace API._Services.Services
{
    public class SHomeMain : IHomeMain
    {
        private readonly IRepositoryAccessor _repositoryAccessor;
        public SHomeMain(IRepositoryAccessor repositoryAccessor)
        {
            _repositoryAccessor = repositoryAccessor;
        }
        public async Task<HomeMainDto> GetData(HomeMainParam param)
        {
            var pred = PredicateBuilder.New<ThongTin>(true);

            var data = await _repositoryAccessor.ThongTin.FindAll().ToListAsync();
            var dataChatLuong = await _repositoryAccessor.ChatLuongCauThu.FindAll().ToListAsync();

            var result = dataChatLuong.Join(data,
                    x => x.IDThongTin,
                    y => y.ID,
                    (x, y) => new { ChatLuong = x, ThongTin = y })
                .Join(_repositoryAccessor.ThuocTinhChinh.FindAll(),
                    x => x.ChatLuong.IDThuocTinhChinh,
                    y => y.ID,
                    (x, y) => new { x.ChatLuong, x.ThongTin, ThuocTinhChinh = y })
                .Select(x => new HomeMainDto{
                    // Ten = x.ThongTin.Ten,
                    // ViTri = "",
                    // TuChat = "",
                    // CanPha = x.
                })
                .FirstOrDefault();
            return result;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListExercise()
        {
            var data = await _repositoryAccessor.BaiTap.FindAll().ToListAsync();
            return data.Select(x => new KeyValuePair<string, string>($"{x.ID}-{x.PhanLoai}", x.TenBaiTap)).ToList();
        }

        public async Task<List<KeyValuePair<string, string>>> GetListPlayers()
        {
            var data = await _repositoryAccessor.ThongTin.FindAll().ToListAsync();
            return data.Select(x => new KeyValuePair<string, string>(x.ID.ToString(), x.Ten)).ToList();
        }
    }
}