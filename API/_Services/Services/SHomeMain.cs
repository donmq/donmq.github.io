
using System.Linq;
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
            var pred = PredicateBuilder.New<ChatLuongAfter>(true);
            if (string.IsNullOrWhiteSpace(param.Ten))
                pred.And(x => x.ID == int.Parse(param.Ten));

            var data = await _repositoryAccessor.ThongTin.FindAll(x => x.ID == int.Parse(param.Ten)).ToListAsync();
            var dataChatLuong = await _repositoryAccessor.ChatLuongAfter.FindAll(pred).ToListAsync();
            var dataViTri = data
                .GroupJoin(_repositoryAccessor.P_ThongTinViTriCauThu.FindAll(),
                    x => x.ViTriID,
                    y => y.ThongTinID,
                    (x, y) => new { ThongTin = x, ThongTinViTri = y })
                .SelectMany(x => x.ThongTinViTri.DefaultIfEmpty(),
                    (x, y) => new { x.ThongTin, ThongTinViTri = y })
                .GroupJoin(_repositoryAccessor.ViTri.FindAll(),
                    x => x.ThongTinViTri.ViTriID,
                    y => y.ID,
                    (x, y) => new { x.ThongTin, x.ThongTinViTri, ViTri = y })
                .SelectMany(x => x.ViTri.DefaultIfEmpty(),
                    (x, y) => new { x.ThongTin, x.ThongTinViTri, ViTri = y })
                .Select(x => x.ViTri.TenViTri).ToList();

            var dataBefore = await _repositoryAccessor.ChatLuongBefore.FindAll(x => x.IDThongTin == int.Parse(param.Ten)).ToListAsync();



            var result = dataChatLuong.Join(data,
                    x => x.IDThongTin,
                    y => y.ID,
                    (x, y) => new { ChatLuong = x, ThongTin = y })
                .Select(x => new HomeMainDto
                {
                    Ten = x.ThongTin.Ten,
                    ViTri = string.Join("+", dataViTri),
                    TuChat = "",
                    CanPha = x.ChatLuong.CanPha,
                    KemNguoi = x.ChatLuong.KemNguoi,
                    ChayCho = x.ChatLuong.ChayCho,
                    DanhDau = x.ChatLuong.DanhDau,
                    DungCam = x.ChatLuong.DungCam,
                    ChuyenBong = x.ChatLuong.ChuyenBong,
                    ReBong = x.ChatLuong.ReBong,
                    TatCanh = x.ChatLuong.TatCanh,
                    SutManh = x.ChatLuong.SutManh,
                    DutDiem = x.ChatLuong.DutDiem,
                    TheLuc = x.ChatLuong.TheLuc,
                    SucManh = x.ChatLuong.SucManh,
                    XongXao = x.ChatLuong.XongXao,
                    TocDo = x.ChatLuong.TocDo,
                    SangTao = x.ChatLuong.SangTao,
                    ChatLuongBefore = dataBefore
                })
                .FirstOrDefault();
            return result;
        }


        public async Task<List<KeyValuePair<int, string>>> GetListExercise()
        {
            var data = await _repositoryAccessor.BaiTap.FindAll().ToListAsync();
            return data.Select(x => new KeyValuePair<int, string>(x.ID, x.TenBaiTap)).ToList();
        }

        public async Task<List<KeyValuePair<string, string>>> GetListPlayers()
        {
            var data = await _repositoryAccessor.ThongTin.FindAll().ToListAsync();
            return data.Select(x => new KeyValuePair<string, string>(x.ID.ToString(), x.Ten)).ToList();
        }

        public async Task<List<KeyValuePair<string, string>>> GetListThuocTinh(int IDBaiTap)
        {


            var data = await _repositoryAccessor.BaiTap.FindAll(x => x.ID == IDBaiTap).ToListAsync();
            var result = data
                .GroupJoin(_repositoryAccessor.P_ThuocTinhBaiTap.FindAll(x => x.IDBaiTap == IDBaiTap),
                    x => x.ID,
                    y => y.IDBaiTap,
                    (x, y) => new { BaiTap = x, P_ThuocTinhBaiTap = y })
                .SelectMany(x => x.P_ThuocTinhBaiTap.DefaultIfEmpty(),
                    (x, y) => new { x.BaiTap, P_ThuocTinhBaiTap = y })
                .GroupJoin(_repositoryAccessor.ThuocTinhChinh.FindAll(),
                    x => x.P_ThuocTinhBaiTap.IDThuocTinhChinh,
                    y => y.ID,
                    (x, y) => new { x.BaiTap, x.P_ThuocTinhBaiTap, ThuocTinhChinh = y })
                .SelectMany(x => x.ThuocTinhChinh.DefaultIfEmpty(),
                    (x, y) => new { x.BaiTap, x.P_ThuocTinhBaiTap, ThuocTinhChinh = y })
                .Select(x => x.ThuocTinhChinh.TenThuocTinh)
                .ToList();



            var dataDisabled = await GetListDisable("ST+AMC");

            var filteredResult = dataDisabled
                .Where(x => result.Contains(x.Key))
                .ToList();


            return filteredResult;
        }

        public async Task<List<KeyValuePair<string, string>>> GetListDisable(string ViTri)
        {
            var listViTri = ViTri.Split("+");
            var dataViTri = await _repositoryAccessor.ViTri.FindAll(x => listViTri.Contains(x.TenViTri.Trim())).Select(x => x.ID).ToListAsync();

            var result = _repositoryAccessor.P_ThuocTinhSangToi
                .FindAll(x => dataViTri.Contains(x.IDViTri))
                .Join(_repositoryAccessor.ThuocTinhChinh.FindAll(),
                    x => x.IDThuocTinhChinh,
                    y => y.ID,
                    (x, y) => new { P_ThuocTinhSangToi = x, ThuocTinhChinh = y })
                .GroupBy(x => x.P_ThuocTinhSangToi.IDThuocTinhChinh)
                .Select(g => new
                {
                    ID = g.Key,
                    Name = g.Select(x => x.ThuocTinhChinh.TenThuocTinh).FirstOrDefault(),
                    KQ = g.Max(x => Convert.ToInt32(x.P_ThuocTinhSangToi.LoaiThuocTinh)) == 0 ? 0 : 1
                })
                .ToList();
            return result.Select(x => new KeyValuePair<string, string>(x.Name, x.KQ.ToString())).ToList();
        }
    }
}