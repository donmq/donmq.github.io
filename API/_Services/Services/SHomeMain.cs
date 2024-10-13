
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using API._Repositories;
using API._Services.Interfaces;
using API.DTO;
using API.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SD3_API.Helpers.Utilities;

namespace API._Services.Services
{
    public class SHomeMain : IHomeMain
    {
        private readonly IRepositoryAccessor _repositoryAccessor;
        public SHomeMain(IRepositoryAccessor repositoryAccessor)
        {
            _repositoryAccessor = repositoryAccessor;
        }
        #region Create        
        public async Task<OperationResult> Create(DataCreate data)
        {
            var checkDataAfter = await _repositoryAccessor.ThongTin.FindAll().ToListAsync();

            // if (checkDataAfter.Any(x => x.ID == data.DataTable.ID))
            //     return new OperationResult(false, "Trùng dữ liệu");
            var dataThongTin = new ThongTin
            {
                Ten = data.DataTable.Ten,
                Tuoi = "18",
            };
            _repositoryAccessor.ThongTin.Add(dataThongTin);
            await _repositoryAccessor.Save();

            var maxIDThongTin = await _repositoryAccessor.ThongTin.FindAll().MaxAsync(x => x.ID);


            var ViTri = data.DataTable.ViTri.Split("+");

            var idViTri = await _repositoryAccessor.ViTri.FindAll(x => ViTri.Contains(x.TenViTri)).Select(x => x.ID).ToListAsync();

            var dataViTri = new List<P_ThongTinViTriCauThu>();
            var dataChatLuongAfter = new ChatLuongAfter { };
            var dataChatLuongBefore = new List<ChatLuongBefore>();

            foreach (var item in idViTri)
            {
                dataViTri.Add(new P_ThongTinViTriCauThu
                {
                    ThongTinID = maxIDThongTin,
                    ViTriID = item
                });
            }


            dataChatLuongAfter = new ChatLuongAfter
            {
                IDThongTin = maxIDThongTin,
                CanPha = data.DataTable.CanPha,
                KemNguoi = data.DataTable.KemNguoi,
                ChayCho = data.DataTable.ChayCho,
                DanhDau = data.DataTable.DanhDau,
                DungCam = data.DataTable.DungCam,
                ChuyenBong = data.DataTable.ChuyenBong,
                ReBong = data.DataTable.ReBong,
                TatCanh = data.DataTable.TatCanh,
                SutManh = data.DataTable.SutManh,
                DutDiem = data.DataTable.DutDiem,
                TheLuc = data.DataTable.TheLuc,
                SucManh = data.DataTable.SucManh,
                XongXao = data.DataTable.XongXao,
                TocDo = data.DataTable.TocDo,
                SangTao = data.DataTable.SangTao
            };


            foreach (var item in data.DataBefore)
            {
                dataChatLuongBefore.Add(new ChatLuongBefore
                {
                    IDThongTin = maxIDThongTin,
                    IDBaiTap = item.IDBaiTap,
                    DiemTB = item.DiemTB,
                    CanPha = item.CanPha,
                    KemNguoi = item.KemNguoi,
                    ChayCho = item.ChayCho,
                    DanhDau = item.DanhDau,
                    DungCam = item.DungCam,
                    ChuyenBong = item.ChuyenBong,
                    ReBong = item.ReBong,
                    TatCanh = item.TatCanh,
                    SutManh = item.SutManh,
                    DutDiem = item.DutDiem,
                    TheLuc = item.TheLuc,
                    SucManh = item.SucManh,
                    XongXao = item.XongXao,
                    TocDo = item.TocDo,
                    SangTao = item.SangTao
                });
            }

            _repositoryAccessor.P_ThongTinViTriCauThu.AddMultiple(dataViTri);
            _repositoryAccessor.ChatLuongAfter.Add(dataChatLuongAfter);
            _repositoryAccessor.ChatLuongBefore.AddMultiple(dataChatLuongBefore);


            return new OperationResult(await _repositoryAccessor.Save());
        }
        #endregion
        #region Update

        public async Task<OperationResult> Update(DataCreate data)
        {

            // Xóa table để cập nhật lại
            var delBefore = await _repositoryAccessor.ChatLuongBefore.FindAll(x => x.IDThongTin == data.DataTable.ID).ToListAsync();
            var delViTri = await _repositoryAccessor.P_ThongTinViTriCauThu.FindAll(x => x.ThongTinID == data.DataTable.ID).ToListAsync();
            _repositoryAccessor.ChatLuongBefore.RemoveMultiple(delBefore);
            _repositoryAccessor.P_ThongTinViTriCauThu.RemoveMultiple(delViTri);
            await _repositoryAccessor.Save();

            // Cập nhật ThongTin
            var dataThongTin = await _repositoryAccessor.ThongTin.FirstOrDefaultAsync(x => x.ID == data.DataTable.ID);
            if (dataThongTin == null)
                return new OperationResult(false);
            dataThongTin.Ten = data.DataTable.Ten.Trim();
            dataThongTin.Tuoi = "18";
            dataThongTin.CLTT = 0;
            dataThongTin.ViTriID = 0;
            _repositoryAccessor.ThongTin.Update(dataThongTin);


            // Chỉnh sửa vị trí
            var ViTri = data.DataTable.ViTri.Split("+");
            var idViTri = await _repositoryAccessor.ViTri.FindAll(x => ViTri.Contains(x.TenViTri)).Select(x => x.ID).ToListAsync();
            var dataViTri = new List<P_ThongTinViTriCauThu>();
            foreach (var item in idViTri)
            {
                dataViTri.Add(new P_ThongTinViTriCauThu
                {
                    ThongTinID = data.DataTable.ID,
                    ViTriID = item
                });
            }
            _repositoryAccessor.P_ThongTinViTriCauThu.AddMultiple(dataViTri);

            // Chỉnh sửa Chất lượng After
            var dataAfter = await _repositoryAccessor.ChatLuongAfter.FirstOrDefaultAsync(x => x.IDThongTin == data.DataTable.ID);
            if (dataAfter == null)
                return new OperationResult(false);
            dataAfter.CanPha = data.DataTable.CanPha;
            dataAfter.KemNguoi = data.DataTable.KemNguoi;
            dataAfter.ChayCho = data.DataTable.ChayCho;
            dataAfter.DanhDau = data.DataTable.DanhDau;
            dataAfter.DungCam = data.DataTable.DungCam;
            dataAfter.ChuyenBong = data.DataTable.ChuyenBong;
            dataAfter.ReBong = data.DataTable.ReBong;
            dataAfter.TatCanh = data.DataTable.TatCanh;
            dataAfter.SutManh = data.DataTable.SutManh;
            dataAfter.DutDiem = data.DataTable.DutDiem;
            dataAfter.TheLuc = data.DataTable.TheLuc;
            dataAfter.SucManh = data.DataTable.SucManh;
            dataAfter.XongXao = data.DataTable.XongXao;
            dataAfter.TocDo = data.DataTable.TocDo;
            dataAfter.SangTao = data.DataTable.SangTao;
            _repositoryAccessor.ChatLuongAfter.Update(dataAfter);

            // Chỉnh sửa Chất lượng Before
            var dataChatLuongBefore = new List<ChatLuongBefore>();
            if (data.DataBefore.Length != 0)
            {
                foreach (var item in data.DataBefore)
                {
                    dataChatLuongBefore.Add(new ChatLuongBefore
                    {
                        IDThongTin = data.DataTable.ID,
                        IDBaiTap = item.IDBaiTap,
                        DiemTB = item.DiemTB,
                        CanPha = item.CanPha,
                        KemNguoi = item.KemNguoi,
                        ChayCho = item.ChayCho,
                        DanhDau = item.DanhDau,
                        DungCam = item.DungCam,
                        ChuyenBong = item.ChuyenBong,
                        ReBong = item.ReBong,
                        TatCanh = item.TatCanh,
                        SutManh = item.SutManh,
                        DutDiem = item.DutDiem,
                        TheLuc = item.TheLuc,
                        SucManh = item.SucManh,
                        XongXao = item.XongXao,
                        TocDo = item.TocDo,
                        SangTao = item.SangTao
                    });
                }
                _repositoryAccessor.ChatLuongBefore.AddMultiple(dataChatLuongBefore);
            }
            using var _transaction = await _repositoryAccessor.BeginTransactionAsync();
            try
            {
                await _repositoryAccessor.Save();
                await _transaction.CommitAsync();
                return new OperationResult(true);
            }
            catch
            {
                await _transaction.RollbackAsync();
                return new OperationResult(false);
            }
        }
        #endregion
        public async Task<OperationResult> Delete(int id)
        {
            var dataThongTin = await _repositoryAccessor.ThongTin.FirstOrDefaultAsync(x => x.ID == id);
            if (dataThongTin != null)
                _repositoryAccessor.ThongTin.Remove(dataThongTin);

            var dataViTri = await _repositoryAccessor.P_ThongTinViTriCauThu.FindAll(x => x.ThongTinID == id).ToListAsync();
            if (dataViTri != null)
                _repositoryAccessor.P_ThongTinViTriCauThu.RemoveMultiple(dataViTri);

            var dataAfter = await _repositoryAccessor.ChatLuongAfter.FirstOrDefaultAsync(x => x.IDThongTin == id);
            if (dataAfter != null)
                _repositoryAccessor.ChatLuongAfter.Remove(dataAfter);

            var dataBefore = await _repositoryAccessor.ChatLuongBefore.FindAll(x => x.IDThongTin == id).ToListAsync();
            if (dataBefore != null)
                _repositoryAccessor.ChatLuongBefore.RemoveMultiple(dataBefore);

            return new OperationResult(await _repositoryAccessor.Save());
        }
        #region  GetData
        public async Task<HomeMainDto> GetData(HomeMainParam param)
        {
            var pred = PredicateBuilder.New<ChatLuongAfter>(true);
            if (!string.IsNullOrWhiteSpace(param.Ten))
                pred.And(x => x.IDThongTin == int.Parse(param.Ten));

            var data = await _repositoryAccessor.ThongTin.FindAll(x => x.ID == int.Parse(param.Ten)).ToListAsync();
            var dataChatLuong = await _repositoryAccessor.ChatLuongAfter.FindAll(pred).ToListAsync();
            var dataViTri = data
                .GroupJoin(_repositoryAccessor.P_ThongTinViTriCauThu.FindAll(),
                    x => x.ID,
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
                    ChatLuongBefore = dataBefore ?? new List<ChatLuongBefore>()
                })
                .FirstOrDefault();
            return result;
        }
        #endregion

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

        public async Task<List<KeyValuePair<string, string>>> GetListThuocTinh(int IDBaiTap, string ViTri)
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



            var dataDisabled = await GetListDisable(ViTri);

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